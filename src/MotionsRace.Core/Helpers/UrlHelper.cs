using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using System;
using MvvmCross.Platform;

namespace MotionsRace.Core.Helpers
{
	public class UrlHelper
	{
		public static string GetFullFeedUrl(string imageUrl)
		{
			if (string.IsNullOrWhiteSpace(imageUrl))
				return null;

			var options = Mvx.Resolve<ISettingsService>().Options;
			return string.Format("{0}://{1}/{2}", GetProtocol(), Uri.EscapeUriString(options.HostName),
				Uri.EscapeDataString(imageUrl.Trim('/')));
		}

		public static string GetProfileImageUrl(string imageGUID)
		{
			if (imageGUID == null || string.IsNullOrWhiteSpace(imageGUID))
				return null;

			var options = Mvx.Resolve<ISettingsService>().Options;
			return string.Format("{0}://{1}/_uploads/personimages/normal/{2}.png", GetProtocol(), options.HostName, imageGUID);
		}

		public static string GetAutologinMainUrl()
		{
			return GetAutologinUrl();
		}

		public static string GetAutologinSettingsUrl()
		{
			return GetAutologinUrl("/settings");
		}

		public static string GetAutologinUrl(string wantedURL = "/")
		{
			var options = Mvx.Resolve<ISettingsService>().Options;
			var applicationId = WebServiceSettings.GetServiceData(options.WebServiceMode, ServiceCharacteristic.ApplicationId);

			var url = string.Format("{0}://{1}/applogin.aspx?applicationid={2}&loginid={3}&ticket={4}&raceid={5}&wantedURL={6}",
				GetProtocol(),
				Uri.EscapeUriString(options.HostName),
				Uri.EscapeDataString(applicationId),
				Uri.EscapeDataString(options.LoginID),
				Uri.EscapeDataString(options.LoginSecretHashed),
				Uri.EscapeDataString(options.RaceID.ToString()),
				Uri.EscapeDataString(wantedURL));

			return url;
		}

		public static string GetProtocol()
		{
			var options = Mvx.Resolve<ISettingsService>().Options;
			return options.IsSecureProtocol ? "https" : "http";
		}
	}
}
