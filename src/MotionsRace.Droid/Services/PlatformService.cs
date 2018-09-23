using System;
using MotionsRace.Core.Services;
using System.Security.Cryptography;
using System.Text;
using Android.Content;
using Java.Lang;
using Android.Net;
using Android.App;
using MotionsRace.Core.Models;
using Java.Net;
using MotionsRace.Core;
using System.IO;
using System.IO.Compression;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Platform;

[assembly:Permission (Name = Android.Manifest.Permission.Internet)]
[assembly:Permission (Name = Android.Manifest.Permission.AccessNetworkState)]

namespace MotionsRace.Droid.Services
{
	public class PlatformService : IPlatformService
	{
		public int GetAppVersion ()
		{
			return Application.Context.PackageManager.GetPackageInfo(Application.Context.PackageName, 0).VersionCode;
		}

		public long FreeRAM ()
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;

			Android.App.ActivityManager.MemoryInfo mi = new Android.App.ActivityManager.MemoryInfo();

			ActivityManager activityManager = (ActivityManager)activity.GetSystemService(Context.ActivityService);
			activityManager.GetMemoryInfo(mi);
			long availableMegs = mi.AvailMem / 1048576L;
			var ava = mi.AvailMem ;

			Runtime info = Runtime.GetRuntime();
			var ewfawqdeqwdfeaw = (info.FreeMemory() / 1024f);

			return 3;
		}

		public void AppExit ()
		{
			var intPID = Android.OS.Process.MyPid();
			Android.OS.Process.KillProcess (intPID);
		}

		public Platform Platform {
			get {
				return Platform.Android;
			}
		}

		public bool IsInternetAvailable ()
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
			bool rez = false;
			activity.RunOnUiThread(new Runnable(() => 
				{
					var connectivityManagerInterne = (ConnectivityManager)activity.GetSystemService (Context.ConnectivityService);
					var activeConnection = connectivityManagerInterne.ActiveNetworkInfo;			

					if (activeConnection != null && activeConnection.IsConnected)
					{
						try {
							Runtime runtime = Runtime.GetRuntime();
							Process ipProcess = runtime.Exec("/system/bin/ping -c 1 " + Constants.CHECK_INTERNET_AVAILABILITY_IP);
							int exitValue = ipProcess.WaitFor();
							rez = true;

						} 
						catch (IOException)          
						{ 
							rez = false;
						} 
						catch (InterruptedException) 
						{ 
							rez = false;
						}
					}
				}
			));
			return rez;
		}

		public void LauchUrl (string url)
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
			activity.RunOnUiThread(new Runnable(() => 
				{
					var uri = Android.Net.Uri.Parse (url);
					var intent = new Intent (Intent.ActionView, uri); 
					activity.StartActivity (intent);  
				}
			));
		}

		public string HashEncode (string key, string message)
		{
			HMACSHA1 hashAlgorithm = new HMACSHA1 ();

			hashAlgorithm.Key = Encoding.ASCII.GetBytes (message);
			byte[] bytes = Encoding.ASCII.GetBytes (key);


			byte[] hashedBytes = hashAlgorithm.ComputeHash (bytes);

			return Convert.ToBase64String (hashedBytes);
		}

		public string UnGZipByteArray(byte[] data)
		{
			using (var compressedStream = new MemoryStream(data))
			using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
			using (var resultStream = new MemoryStream())
			{
				zipStream.CopyTo(resultStream); // show error "can not read" in PCL for iOS
				var result = resultStream.ToArray();
				return Encoding.UTF8.GetString(result, 0, result.Length);
			}
		}

	}
}