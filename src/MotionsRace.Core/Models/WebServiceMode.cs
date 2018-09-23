using System;
using System.Linq;
using System.Threading.Tasks;
using MotionsRace.Core.ApiClient;
using MotionsRace.Core.Services;
using MvvmCross.Platform;

namespace MotionsRace.Core.Models
{
	public enum WebServiceMode
	{
		Prodaction = 0,
		SecondaryProdaction = 1,
		Develop = 2
	}

	public enum ServiceCharacteristic
	{
		ApplicationId,
		ApplicationSecret,
		ApiUrl,
		ApiImageUploadUrl,
	}

	public static class WebServiceSettings
	{
		public static WebServiceMode GetServiceModeByUserName(ref string userName)
		{
			var tmpUserName = userName;
			var devPrefixes = new[] { "dev-", "develop." };
			var devUserPrefix = devPrefixes.FirstOrDefault(x => tmpUserName.StartsWith(x, StringComparison.OrdinalIgnoreCase));
			if (devUserPrefix != null)
			{
				userName = tmpUserName.Remove(0, devUserPrefix.Length);
				return WebServiceMode.Develop;
			}

			return WebServiceMode.Prodaction;
		}

		public static async Task<ApiResponse<LoginResponse>> LoginServerLogic(IWebService webService, WebServiceMode serviceMode, string userName, string password, bool isOAuth)
		{
			// Connect priority, if user not contains "dev-" 
			// 1. Prod
			// 2. Prod2
			// 3. Dev
			webService.SetMode(serviceMode);
			var result = await webService.LoginAsync(userName, password, isOAuth);
			if (!result.IsSuccess && (int)serviceMode < 2)
			{
				return await LoginServerLogic(webService, serviceMode + 1, userName, password, isOAuth);
			}
			return result;
		}

		public static string GetServiceData(WebServiceMode serviceMode, ServiceCharacteristic characteristic)
		{
//			switch (serviceMode)
//			{
				//case WebServiceMode.Prodaction:
					return getProdServerData(characteristic);
//				case WebServiceMode.SecondaryProdaction:
//					return getProd2ServerData(characteristic);
//				case WebServiceMode.Develop:
//					return getDevServerData(characteristic);
//			}
//			return null;
		}

		private static string getProdServerData(ServiceCharacteristic characteristic)
		{
			switch (characteristic)
			{
				case ServiceCharacteristic.ApplicationId:
					return GetAppId();
				case ServiceCharacteristic.ApplicationSecret:
					return GetAppSecret();
				case ServiceCharacteristic.ApiUrl:
					return Constants.API_SERVICE_URL_PROD;
				case ServiceCharacteristic.ApiImageUploadUrl:
					return Constants.API_SERVICE_UPLOAD_PHOTO_URL_PROD;
			}
			return null;
		}

		private static string GetAppId()
		{
			var platform = Mvx.Resolve<IPlatformService> ().Platform;

			switch (platform) {
			case Platform.iOS:
				return Constants.APPLICATION_IOS_ID_PROD;
			case Platform.Android:
				return Constants.APPLICATION_DROID_ID_PROD;
			case Platform.WindowsPhone:
				return Constants.APPLICATION_WP_ID_PROD;
			default:
				return "";
			}
		}

		private static string GetAppSecret()
		{
			var platform = Mvx.Resolve<IPlatformService> ().Platform;

			switch (platform) {
			case Platform.iOS:
				return Constants.APPLICATION_IOS_SECRET_PROD;
			case Platform.Android:
				return Constants.APPLICATION_DROID_SECRET_PROD;
			case Platform.WindowsPhone:
				return Constants.APPLICATION_WP_SECRET_PROD;
			default:
				return "";
			}
		}

//		private static string getProd2ServerData(ServiceCharacteristic characteristic)
//		{
//			switch (characteristic)
//			{
//				case ServiceCharacteristic.ApplicationId:
//					return Constants.APPLICATION_ID_PROD2;
//				case ServiceCharacteristic.ApplicationSecret:
//					return Constants.APPLICATION_SECRET_PROD2;
//				case ServiceCharacteristic.ApiUrl:
//					return Constants.API_SERVICE_URL_PROD2;
//				case ServiceCharacteristic.ApiImageUploadUrl:
//					return Constants.API_SERVICE_UPLOAD_PHOTO_URL_PROD2;
//			}
//			return null;
//		}
//
//		private static string getDevServerData(ServiceCharacteristic characteristic)
//		{
//			switch (characteristic)
//			{
//				case ServiceCharacteristic.ApplicationId:
//					return Constants.APPLICATION_ID_DEV;
//				case ServiceCharacteristic.ApplicationSecret:
//					return Constants.APPLICATION_SECRET_DEV;
//				case ServiceCharacteristic.ApiUrl:
//					return Constants.API_SERVICE_URL_DEV;
//				case ServiceCharacteristic.ApiImageUploadUrl:
//					return Constants.API_SERVICE_UPLOAD_PHOTO_URL_DEV;
//			}
//			return null;
//		}
	}
}
