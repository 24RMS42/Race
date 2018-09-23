using System;
using MotionsRace.Core.Services;
using Android.Content;
using Java.IO;
using Android.Provider;

namespace MotionsRace.Droid
{
	public class PhotoService : IPhotoService
	{

		public void GetPictureFromLibrary (int maxPixelDimension, int percentQuality)
		{
			throw new NotImplementedException ();
		}

		public void GetPictureFromCamera (int maxPixelDimension, int percentQuality)
		{
			Intent intent = new Intent (MediaStore.ActionImageCapture);
			var file = new File (App._dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));
			intent.PutExtra (MediaStore.ExtraOutput, Uri.FromFile (App._file));
			StartActivityForResult (intent, 0);
		}

		public byte[] GetBytes (System.IO.Stream stream)
		{
			throw new NotImplementedException ();
		}

	}
}

