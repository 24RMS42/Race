using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using Windows.Networking.Connectivity;
using Windows.System;
using MotionsRace.Core.Services;

namespace MotionsRace.WinPhone.Services
{
	public class PlatformService : IPlatformService
	{
		public string HashEncode(string keyString, string message)
		{
			string signature = CreateToken(keyString, message);
			return signature;
		}

		public async void LauchUrl(string url)
		{
			await Launcher.LaunchUriAsync(new Uri(url));
		}

		public bool IsInternetAvailable()
		{
			var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
			return (connectionProfile != null &&
			        connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
		}

		public Core.Models.Platform Platform
		{
			get { return Core.Models.Platform.WindowsPhone; }
		}

		public void AppExit()
		{
			Application.Current.Terminate();
		}

		public long FreeRAM()
		{
			throw new NotImplementedException();
		}

		public string UnGZipByteArray(byte[] data)
		{
			throw new NotImplementedException();
		}

		public int GetAppVersion()
		{
			var nameHelper = new AssemblyName(Assembly.GetExecutingAssembly().FullName);
			return nameHelper.Version.Major;
		}

		private string CreateToken(string message, string secret)
		{
			secret = secret ?? "";
			var encoding = new UTF8Encoding();
			byte[] keyByte = encoding.GetBytes(secret);
			byte[] messageBytes = encoding.GetBytes(message);
			using (var hmacsha256 = new HMACSHA1(keyByte))
			{
				byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
				return Convert.ToBase64String(hashmessage);
			}
		}

		private static string ShaHash(string value, string key)
		{
			using (var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(key)))
			{
				return ByteToString(hmac.ComputeHash(Encoding.UTF8.GetBytes(value)));
			}
		}

		private static string ByteToString(IEnumerable<byte> data)
		{
			return string.Concat(data.Select(b => b.ToString("x2")));
		}
	}
}