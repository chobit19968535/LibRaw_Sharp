using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace LibRaw_Wrapper
{
    class LibRAW
    {
        //private const string LibraryName = "libraw";
        private const string LibraryName = "libraw";


        [Flags]
        public enum LibRaw_init_flags : uint
        {
            LIBRAW_OPTIONS_NONE = 0
        }

        [Flags]
        public enum LibRaw_ERR_flags : int
        {
            // None-Fatal Errors
            LIBRAW_SUCCESS = 0,
            LIBRAW_UNSPECIFIED_ERROR = -1,
            LIBRAW_FILE_UNSUPPORTED = -2,
            LIBRAW_REQUEST_FOR_NONEXISTENT_IMAGE = -3,
            LIBRAW_OUT_OF_ORDER_CALL = -4,
            LIBRAW_NO_THUMBNAIL = -5,
            LIBRAW_UNSUPPORTED_THUMBNAIL = -6,
            LIBRAW_INPUT_CLOSED = -7,
            LIBRAW_NOT_IMPLEMENTED = -8,
            LIBRAW_REQUEST_FOR_NONEXISTENT_THUMBNAIL = -9,

            // Fatal Erros
            LIBRAW_UNSUFFICIENT_MEMORY = -100007,
            LIBRAW_DATA_ERROR = -100008,
            LIBRAW_IO_ERROR = -100009,
            LIBRAW_CANCELLED_BY_CALLBACK = -100010,
            LIBRAW_BAD_CROP = -100011,
            LIBRAW_TOO_BIG = -100012,
            LIBRAW_MEMPOOL_OVERFLOW = -100013
        }

        [Flags]
        public enum LibRaw_Interpolation_quility : int
        {
            LINEAR = 0,
            VNG = 1,
            PPG = 2,
            AHD = 3,
            DCB = 4,
            DHT = 11,
            MODIFIED_AHD = 12
        }

        [Flags]
        public enum LibRaw_output_color : int
        {
            RAW = 0,
            SRGB = 1,
            ADOBE = 2,
            WIDE = 3,
            PROPHOTO = 4,
            XYZ = 5,
            ACES = 6,
            DCI_P3 = 7,
            Rec2020 = 8
        }

        [Flags]
        public enum LibRaw_output_bps : int
        {
            BPS8 = 8,
            BPS16 = 16
        }

        [Flags]
        public enum LibRaw_gammaIndex : int
        {
            @default0 = 0,
            @default1 = 1
        }

        [Flags]
        public enum LibRaw_highlight_mode : int
        {
            CLIP = 0,
            UNCLIP = 1,
            BLEND = 2,
            REBUILD = 3,
            REBUILD4 = 4,
            REBUILD5 = 5,
            REBUILD6 = 6,
            REBUILD7 = 7,
            REBUILD8 = 8,
            REBUILD9 = 9
        }

        [Flags]
        public enum LibRaw_FBDD_noise_reduction : int
        {
            NO_FBDD = 0,
            LIGHT_FBDD = 1,
            FULL_FBDD = 2
        }

        [Flags]
        public enum LibRaw_output_formats : int
        {
            PPM = 0,
            TIFF = 1
        }

        [Flags]
        public enum LibRaw_progress : int
        {
            LIBRAW_PROGRESS_START = 0,
            LIBRAW_PROGRESS_OPEN = 1,
            LIBRAW_PROGRESS_IDENTIFY = 1 << 1,
            LIBRAW_PROGRESS_SIZE_ADJUST = 1 << 2,
            LIBRAW_PROGRESS_LOAD_RAW = 1 << 3,
            LIBRAW_PROGRESS_RAW2_IMAGE = 1 << 4,
            LIBRAW_PROGRESS_REMOVE_ZEROES = 1 << 5,
            LIBRAW_PROGRESS_BAD_PIXELS = 1 << 6,
            LIBRAW_PROGRESS_DARK_FRAME = 1 << 7,
            LIBRAW_PROGRESS_FOVEON_INTERPOLATE = 1 << 8,
            LIBRAW_PROGRESS_SCALE_COLORS = 1 << 9,
            LIBRAW_PROGRESS_PRE_INTERPOLATE = 1 << 10,
            LIBRAW_PROGRESS_INTERPOLATE = 1 << 11,
            LIBRAW_PROGRESS_MIX_GREEN = 1 << 12,
            LIBRAW_PROGRESS_MEDIAN_FILTER = 1 << 13,
            LIBRAW_PROGRESS_HIGHLIGHTS = 1 << 14,
            LIBRAW_PROGRESS_FUJI_ROTATE = 1 << 15,
            LIBRAW_PROGRESS_FLIP = 1 << 16,
            LIBRAW_PROGRESS_APPLY_PROFILE = 1 << 17,
            LIBRAW_PROGRESS_CONVERT_RGB = 1 << 18,
            LIBRAW_PROGRESS_STRETCH = 1 << 19,
            /* reserved */
            LIBRAW_PROGRESS_STAGE20 = 1 << 20,
            LIBRAW_PROGRESS_STAGE21 = 1 << 21,
            LIBRAW_PROGRESS_STAGE22 = 1 << 22,
            LIBRAW_PROGRESS_STAGE23 = 1 << 23,
            LIBRAW_PROGRESS_STAGE24 = 1 << 24,
            LIBRAW_PROGRESS_STAGE25 = 1 << 25,
            LIBRAW_PROGRESS_STAGE26 = 1 << 26,
            LIBRAW_PROGRESS_STAGE27 = 1 << 27,
            LIBRAW_PROGRESS_THUMB_LOAD = 1 << 28,
            LIBRAW_PROGRESS_TRESERVED1 = 1 << 29,
            LIBRAW_PROGRESS_TRESERVED2 = 1 << 30
        }

        [Flags]
        public enum LibRaw_image_formats : int
        {
            LIBRAW_IMAGE_JPEG = 1,
            LIBRAW_IMAGE_BITMAP = 2
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_lensinfo_t
        {
            public float MinFocal;
            public float MaxFocal;
            public float MaxAp4MinFocal;
            public float MaxAp4MaxFocal;
            public float EXIF_MaxAp;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string LensMake;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Lens;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string LensSerial;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string InternalLensSerial;
            public ushort FocalLengthIn35mmFormat;
            [MarshalAs(UnmanagedType.Struct)] public libraw_nikonlens_t nikon;
            [MarshalAs(UnmanagedType.Struct)] public libraw_dnglens_t dng;
            [MarshalAs(UnmanagedType.Struct)] public libraw_makernotes_lens_t makernotes;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_nikonlens_t
        {
            public float EffectiveMaxAp;
            public byte LensIDNumber;
            public byte LensFStops;
            public byte MCUVersion;
            public byte LensType;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_dnglens_t
        {
            public float MinFocal;
            public float MaxFocal;
            public float MaxAp4MinFocal;
            public float MaxAp4MaxFocal;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_makernotes_lens_t
        {
            public ulong LensID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Lens;
            public ushort LensFormat;  /* to characterize the image circle the lens covers */
            public ushort LensMount;   /* 'male', lens itself */
            public ulong CamID;
            public ushort CameraFormat;    /* some of the sensor formats */
            public ushort CameraMount; /* 'female', body throat */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string body;
            short FocalType; /* -1/0 is unknown; 1 is fixed focal; 2 is zoom */
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)] public string LensFeatures_pre;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)] public string LensFeatures_suf;
            public float MinFocal;
            public float MaxFocal;
            public float MaxAp4MinFocal;
            public float MaxAp4MaxFocal;
            public float MinAp4MinFocal;
            public float MinAp4MaxFocal;
            public float MaxAp;
            public float MinAp;
            public float CurFocal;
            public float CurAp;
            public float MaxAp4CurFocal;
            public float MinAp4CurFocal;
            public float MinFocusDistance;
            public float FocusRangeIndex;
            public float LensFStops;
            public ulong TeleconverterID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Teleconverter;
            public ulong AdapterID;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)] public string Attachment;
            public ushort FocalUnits;
            public float FocalLengthIn35mmFormat;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_imgother_t
        {
            public float isospeed;
            public float shutter;
            public float aperture;
            public float focal_len;
            public long timestamp; //世界時間
            public ushort shot_order;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U4, SizeConst = 32)] public uint[] gpsdata;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 512)] public string desc;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string artist;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 4)] public float[] analogbalance;


        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_gps_info_t
        {   
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)] public float[] latitude; /* Deg,min,sec */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)] public float[] longitude; /* Deg,min,sec */
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.R4, SizeConst = 3)] public float[] gpstimestamp; /* Deg,min,sec */
            public float altitude;
            public char altref, latref, longref, gpsstatus;
            public char gpsparsed;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi /*, Pack = 1 */)]
        public struct libraw_processed_image_t
        {
            public LibRaw_image_formats type;
            public ushort height;
            public ushort width;
            public ushort colors;
            public ushort bits;
            public uint data_size;
            [MarshalAs(UnmanagedType.ByValArray, ArraySubType = UnmanagedType.U1, SizeConst = 1)] public byte[] data;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct libraw_iparams_t
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)] public string guard;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string make;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string model;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string software;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string normalize_make;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)] public string normalize_model;
            


        }


        #region Initialzation and denitialzation
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_init(LibRaw_init_flags flag);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_close(IntPtr handler);
        #endregion

        #region Data Loading from a File/Bugger

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_ERR_flags libraw_open_file(IntPtr handler, string file);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int open_datastream();

        #endregion

        #region Parameters getting/setting

        #region Getting CMDs
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_raw_width(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_raw_height(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_iheight(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_get_iwidth(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_get_lensinfo(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_get_imgother(IntPtr handler);

        #region UnKnow Functions
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern float libraw_get_cam_mul(IntPtr handler, int index);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern int libraw_open_buffer(IntPtr handler, byte[] buffer, uint size); // No using

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern libraw_iparams_t libraw_get_iparams(IntPtr handler);


        #endregion

        #endregion

        #region Setting CMDs
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_bright(IntPtr handler, float value = 1.0f);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_no_auto_bright(IntPtr handler, int value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_demosaic(IntPtr handler, LibRaw_Interpolation_quility method);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_output_color(IntPtr handler, LibRaw_output_color format);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_output_bps(IntPtr handler, LibRaw_output_bps value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_gamma(IntPtr handler, LibRaw_gammaIndex index, float value = 1.0f);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        //Highlight recovery in raw images, no using
        public static extern void libraw_set_highlight(IntPtr handler, LibRaw_highlight_mode mode);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        //[Flexible High Dynamic Range DeBayering and Denoising], no using
        public static extern void libraw_set_fbdd_noiserd(IntPtr handler, LibRaw_FBDD_noise_reduction mode);

        #region Unknow-Functions
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        // index can be 0..3, 微調 B, G, R 三個Channel 的 Gain value
        public static extern void libraw_set_user_mul(IntPtr handler, int index, float value);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_output_tif(IntPtr handler, LibRaw_output_formats value);

        #endregion

        #endregion

        #endregion

        #region LibRaw_Process
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_set_progress_handler(IntPtr handler, ProgressCallback callback, IntPtr datap);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_ERR_flags libraw_unpack(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_ERR_flags libraw_unpack_thumb(IntPtr handler);
        
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_ERR_flags libraw_dcraw_process(IntPtr handler);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_dcraw_make_mem_image(IntPtr handler, ref int flag);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_dcraw_make_mem_thumb(IntPtr handler, ref int flag);


        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern void libraw_dcraw_clear_mem(IntPtr img);
        #endregion

        #region CallBack Functions
        // callback functions
        [UnmanagedFunctionPointer(CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        public delegate int ProgressCallback(IntPtr unused_data, LibRaw_progress state, int iter, int expected);
        #endregion

        #region Auxiliary Functions
        [DllImport(LibraryName, CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern IntPtr libraw_strprogress(LibRaw_progress progress);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern IntPtr libraw_strerror(LibRaw_ERR_flags errorcode);

        // Microsoft Visual C runtime functions
        [DllImport("msvcrt", CharSet = CharSet.Ansi)]
        public static extern IntPtr strerror(int errc);

        #endregion

        #region Output Format Functions
        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_ERR_flags libraw_dcraw_ppm_tiff_writer(IntPtr handler, string filename);

        [DllImport(LibraryName, CharSet = CharSet.Ansi)]
        public static extern LibRaw_ERR_flags libraw_dcraw_thumb_writer(IntPtr handler, string filename);

        
        #endregion

    }



}
