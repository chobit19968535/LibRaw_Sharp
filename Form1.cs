using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.IO;

namespace LibRaw_Wrapper
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            var handler = LibRAW.libraw_init(LibRAW.LibRaw_init_flags.LIBRAW_OPTIONS_NONE);



            #region Decode Setting
            LibRAW.libraw_set_bright(handler, 5.0f);   //對於影像強度有顯著變化, 是線性倍率，但小數點會被捨去的樣子, 5 to 10, 4 to 9
            LibRAW.libraw_set_no_auto_bright(handler, 1); 
            LibRAW.libraw_set_demosaic(handler, LibRAW.LibRaw_Interpolation_quility.LINEAR);
            LibRAW.libraw_set_output_color(handler, LibRAW.LibRaw_output_color.RAW);
            LibRAW.libraw_set_output_bps(handler, LibRAW.LibRaw_output_bps.BPS8);
            LibRAW.libraw_set_gamma(handler, LibRAW.LibRaw_gammaIndex.default0, 1.0f);
            LibRAW.libraw_set_gamma(handler, LibRAW.LibRaw_gammaIndex.default1, 1.0f);
            LibRAW.libraw_set_highlight(handler, LibRAW.LibRaw_highlight_mode.CLIP); 
            LibRAW.libraw_set_fbdd_noiserd(handler, LibRAW.LibRaw_FBDD_noise_reduction.FULL_FBDD); 
            //LibRAW.libraw_set_user_mul(handler, 0, 4.0f);
            //LibRAW.libraw_set_user_mul(handler, 1, 1.0f);
            //LibRAW.libraw_set_user_mul(handler, 2, 1.0f);
            LibRAW.libraw_set_output_tif(handler, LibRAW.LibRaw_output_formats.TIFF);



            // set callback function for libraw processing
            LibRAW.libraw_set_progress_handler(handler, (unused_data, state, iter, expected) => {
                if (iter == 0)
                {
                    var progress = Marshal.PtrToStringAnsi(LibRAW.libraw_strprogress(state));
                    Console.WriteLine("Callback: {0,30}, expected {1,6} iterations", progress, expected);
                }
                return 0;
            }, IntPtr.Zero);



            #endregion

            #region Decode Raw File according above setting
            string fileName = "IMG.CR3";
            var flag = LibRAW.libraw_open_file(handler, fileName);

            #region EXIF data
            /*
             * Exchangeable image file format (Exif), 可交換圖檔格式，用來記錄影像的各種屬性以及拍攝條件資料
             * Exif 可支援音訊檔
             * Exif 可附件於Tiff之下
             * 
             * Exif資訊是可以被任意編輯的，因此只有參考的功能。
             * Exif資訊以0xFFE1作為開頭標記，後兩個位元組表示Exif資訊的長度。所以Exif資訊最大為64 kB，而內部採用TIFF格式。
             */
            int raw_w = LibRAW.libraw_get_raw_width(handler);
            int rwa_h = LibRAW.libraw_get_raw_height(handler);

            int img_w = LibRAW.libraw_get_iwidth(handler);
            int img_h = LibRAW.libraw_get_iheight(handler);

            // other image parameters
            Console.WriteLine("\nOther image parameters:");
            var poparam = LibRAW.libraw_get_imgother(handler);
            var oparam = Marshal.PtrToStructure<LibRAW.libraw_imgother_t>(poparam);
            Console.WriteLine("ISO:          {0}", oparam.isospeed);
            Console.WriteLine("Shutter:      {0}s", oparam.shutter);
            Console.WriteLine("Aperture:     f/{0}", oparam.aperture);
            Console.WriteLine("Focal length: {0}mm", oparam.focal_len);
            // C-style time_t equals to seconds elapsed since 1970-1-1
            var ts = new DateTime(1970, 1, 1).AddSeconds(oparam.timestamp).ToLocalTime();
            Console.WriteLine("Timestamp:    {0}", ts.ToString("yyyy-MMM-dd HH:mm:ss"));
            Console.WriteLine("Img serial no {0}", oparam.shot_order);
            Console.WriteLine("Description:  {0}", oparam.desc);
            Console.WriteLine("Artist:       {0}", oparam.artist);
            Console.WriteLine("Analog balance: {0}", oparam.analogbalance[0]);

            // get lens info
            Console.WriteLine("\nLens information:");
            var plensparam = LibRAW.libraw_get_lensinfo(handler);
            var lensparam = Marshal.PtrToStructure<LibRAW.libraw_lensinfo_t>(plensparam);
            Console.WriteLine("Minimum focal length:                     {0}mm", lensparam.MinFocal);
            Console.WriteLine("Maximum focal length:                     {0}mm", lensparam.MaxFocal);
            Console.WriteLine("Maximum aperture at minimum focal length: {0}mm", lensparam.MaxAp4MinFocal);
            Console.WriteLine("Maximum aperture at maximum focal length: {0}mm", lensparam.MaxAp4MaxFocal);
            Console.WriteLine("EXIF tag 0x9205:                          {0}", lensparam.EXIF_MaxAp);
            Console.WriteLine("Lens make:                                {0}", lensparam.LensMake);
            Console.WriteLine("Lens:                                     {0}", lensparam.Lens);
            Console.WriteLine("Lens serial:                              {0}", lensparam.LensSerial);
            Console.WriteLine("Internal lens serial:                     {0}", lensparam.InternalLensSerial);
            Console.WriteLine("EXIF tag 0xA405:                          {0}", lensparam.FocalLengthIn35mmFormat);
            Console.WriteLine("Makernotes lens:                          {0}\n", lensparam.makernotes.Lens);
            #endregion

            // unpack data from raw file
            flag = LibRAW.libraw_unpack(handler);
            if (flag != LibRAW.LibRaw_ERR_flags.LIBRAW_SUCCESS)
            {
                Console.WriteLine("Unpack: " + Marshal.PtrToStringAnsi(LibRAW.libraw_strerror(flag)));
                LibRAW.libraw_close(handler);
                return;
            }

            // process data using previously defined settings
            flag = LibRAW.libraw_dcraw_process(handler);
            if (flag != LibRAW.LibRaw_ERR_flags.LIBRAW_SUCCESS)
            {
                Console.WriteLine("Process: " + Marshal.PtrToStringAnsi(LibRAW.libraw_strerror(flag)));
                LibRAW.libraw_close(handler);
                return;
            }



            // extract raw data into allocated memory buffer
            var errc = 0;
            IntPtr ptr;
            #region mem_image
            ptr = LibRAW.libraw_dcraw_make_mem_image(handler, ref errc);
            if (errc != 0)
            {
                Console.WriteLine("Mem_img: " + Marshal.PtrToStringAnsi(LibRAW.strerror(errc)));
                LibRAW.libraw_close(handler);
                return;
            }
            #endregion
            

            // convert pointer to structure to get image info and raw data
            var img = Marshal.PtrToStructure<LibRAW.libraw_processed_image_t>(ptr);
            Console.WriteLine("\nImage type:   " + img.type);
            Console.WriteLine("Image height: " + img.height);
            Console.WriteLine("Image width:  " + img.width);
            Console.WriteLine("Image colors: " + img.colors);
            Console.WriteLine("Image bits:   " + img.bits);
            Console.WriteLine("Data size:    " + img.data_size);
            Console.WriteLine("Checksum:     " + img.height * img.width * img.colors * (img.bits / 8));




            #endregion


            Console.WriteLine();

            // rqeuired step before accessing the "data" array
            Array.Resize(ref img.data, (int)img.data_size);
            var adr = ptr + Marshal.OffsetOf(typeof(LibRAW.libraw_processed_image_t), "data").ToInt32();
            Marshal.Copy(adr, img.data, 0, (int)img.data_size);

            // calculate padding for lines and add padding
            var num = img.width % 4;
            var padding = new byte[num];
            var stride = img.width * img.colors * (img.bits / 8);
            var line = new byte[stride];
            var tmp = new List<byte>();
            for (var i = 0; i < img.height; i++)
            {
                Buffer.BlockCopy(img.data, stride * i, line, 0, stride);
                tmp.AddRange(line);
                tmp.AddRange(padding);
            }

            // release memory allocated by [libraw_dcraw_make_mem_image]
            LibRAW.libraw_dcraw_clear_mem(ptr);

            // create/save bitmap from mem image/array
            var bmp = new Bitmap(img.width, img.height, PixelFormat.Format24bppRgb);
            var bmd = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadWrite, bmp.PixelFormat);
            Marshal.Copy(tmp.ToArray(), 0, bmd.Scan0, (int)img.data_size);
            bmp.UnlockBits(bmd);
            var outJPEG = fileName.Replace(Path.GetExtension(fileName), ".jpg");
            Console.WriteLine("Saving image to: " + outJPEG);
            bmp.Save(outJPEG, ImageFormat.Jpeg);

            // save to TIFF
            var outTIFF = fileName.Replace(Path.GetExtension(fileName), ".tiff ");
            Console.WriteLine("\nSaving TIFF to: " + outTIFF);
            flag = LibRAW.libraw_dcraw_ppm_tiff_writer(handler, outTIFF);
            if (flag != LibRAW.LibRaw_ERR_flags.LIBRAW_SUCCESS) Console.WriteLine("TIFF writer:     " + Marshal.PtrToStringAnsi(LibRAW.libraw_strerror(flag)));


            // close RAW file
            LibRAW.libraw_close(handler);





        }


    }
}
