using System;
using MotionsRace.Core.Services;
using UIKit;
using Foundation;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.IO.Compression;

namespace MotionsRace.Touch
{
	public class PlatformService : IPlatformService
	{
		public string UnGZipByteArray (byte[] data)
		{
			using (var compressedStream = new MemoryStream(data))
			using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
			using (var resultStream = new MemoryStream())
			{
					zipStream.CopyTo(resultStream); // show error "can not read" in PCL for iOS
					var result = resultStream.ToArray();
					return Encoding.UTF8.GetString (result, 0, result.Length);
			}
		}

		public long FreeRAM ()
		{
			throw new NotImplementedException ();
		}

		public void AppExit ()
		{
			throw new NotImplementedException ();
		}

		#region IPlatformService implementation

		public string HashEncode (string key, string message)
		{
			HMACSHA1 hashAlgorithm = new HMACSHA1 ();
			hashAlgorithm.Key = Encoding.ASCII.GetBytes (message);

			byte[] bytes = Encoding.ASCII.GetBytes (key);
			byte[] hashedBytes = hashAlgorithm.ComputeHash (bytes);

			return Convert.ToBase64String (hashedBytes);
		}

		public void LauchUrl (string url)
		{
			UIApplication.SharedApplication.OpenUrl(new NSUrl(url));
		}

		public bool IsInternetAvailable ()
		{
			return Reachability.InternetConnectionStatus() != NetworkStatus.NotReachable && Reachability.IsHostReachable("google.com");
		}

		public MotionsRace.Core.Models.Platform Platform {
			get {
				return MotionsRace.Core.Models.Platform.iOS;
			}
		}

		public int GetAppVersion ()
		{
			try
			{
				var strVersion = NSBundle.MainBundle.InfoDictionary [new NSString ("CFBundleVersion")].ToString ();
				return (int)float.Parse (strVersion);
			}
			catch 
			{
				return -1;
			}
		}

		#endregion
	}
}
