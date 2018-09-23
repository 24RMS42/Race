

using MvvmCross.Platform.UI;

namespace MotionsRace.Core
{
    public class Constants
    {		
		public const string SOAP_NAMESPACE = "http://service.funbeatrace.com/MobileService.asmx";
		                                           
//		public const string API_SERVICE_URL_DEV = "http://service.funbeatrace.c2labs.se/MobileService.asmx";
//      public const string API_SERVICE_UPLOAD_PHOTO_URL_DEV = "http://service.funbeatrace.c2labs.se/uploadtraining.aspx/";
//		public const string APPLICATION_ID_DEV = "2a67c6d4-21b0-479e-b2a4-f023dfddc18d";
//		public const string APPLICATION_SECRET_DEV = "C86F2242-1EA4-4C55-8CB7-E5490464A93F";

        public const string API_SERVICE_URL_PROD = "http://service.motionsrace.com/MobileService.asmx";
        public const string API_SERVICE_UPLOAD_PHOTO_URL_PROD = "http://service.motionsrace.com/uploadtraining.aspx/";

		public const string APPLICATION_IOS_ID_PROD = "FF06105F-5456-48B6-9E0F-8CF98787FDE6";
		public const string APPLICATION_IOS_SECRET_PROD = "54AE0BF5-EEA9-404C-A575-120D678B0BF9";

		public const string APPLICATION_DROID_ID_PROD = "FBA1ACC9-0974-410D-B8E0-B3EB49DE33B3";
		public const string APPLICATION_DROID_SECRET_PROD = "DE0E2741-A777-4C0B-8FF7-1F1825F6EE3A";

		public const string APPLICATION_WP_ID_PROD = "6D8522B4-B471-4D20-9A6B-F88A7CDCECE1";
		public const string APPLICATION_WP_SECRET_PROD = "7AEF200B-EEE8-46C9-B06C-D770676903BE";

//		public const string API_SERVICE_URL_PROD2 = "http://service.companyraces.com/MobileService.asmx";
//		public const string API_SERVICE_UPLOAD_PHOTO_URL_PROD2 = "http://service.companyraces.com/uploadtraining.aspx";
//		public const string APPLICATION_ID_PROD2 = "FF06105F-5456-48B6-9E0F-8CF98787FDE6";
//		public const string APPLICATION_SECRET_PROD2 = "54ae0bf5-eea9-404c-a575-120d678b0bf9";
		
		// SPLASH
		public static readonly MvxColor MOTION_RACE_SPLASH_BACKGROUND_COLOR = new MvxColor(22, 116, 168);
		public static readonly string MOTION_RACE_SPLASH_LOGO = @"motionrace/splashlogo.png";

        public static readonly MvxColor TWITCH_SPLASH_BACKGROUND_COLOR = new MvxColor(51, 51, 51);
        public static readonly string TWITCH_SPLASH_LOGO = @"twitch/splashlogo.png";

        public static readonly MvxColor ATEA_SPLASH_BACKGROUND_COLOR = new MvxColor(0, 120, 195);
		public static readonly string ATEA_SPLASH_LOGO = @"atea/splashlogo.png";

		public static readonly MvxColor NORDEN_SPLASH_BACKGROUND_COLOR = new MvxColor(207, 20, 43);
		public static readonly string NORDEN_SPLASH_LOGO = @"norden/splashlogo.png";

		public static readonly MvxColor COROMATIC_SPLASH_BACKGROUND_COLOR = new MvxColor(255, 255, 255);
		public static readonly string COROMATIC_SPLASH_LOGO = @"coromatic/splashlogo.png";

		public static readonly MvxColor NETENT_SPLASH_BACKGROUND_COLOR = new MvxColor(32, 32, 32);
		public static readonly string NETENT_SPLASH_LOGO = @"netent/splashlogo.png";

		public static readonly MvxColor KRONOBERG_SPLASH_BACKGROUND_COLOR = new MvxColor(255, 255, 255);
		public static readonly string KRONOBERG_SPLASH_LOGO = @"kronoberg/splashlogo.png";

        // Internet checking
        public const string CHECK_INTERNET_AVAILABILITY_IP = "8.8.8.8"; // Google DNS

		public const int CATEGORY_MIN_ID = 1000;
		public const int CATEGORY_MAX_ID = 3999;

		//ActivityItemViewModel Image Pick
		public const int MAX_PIXEL_DIMENTION = 1536; // From camera dont change!
		public const int DEFAULT_JPEG_QUALITY = 70;
		public const int MAX_PICK_IMAGE_WIDTH = 640; // Use this value for image width;
    }
}
