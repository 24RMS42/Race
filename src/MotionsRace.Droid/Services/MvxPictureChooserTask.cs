using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Provider;
using Uri = Android.Net.Uri;
using MotionsRace.Core.Services;
using MvvmCross.Platform.Droid.Platform;
using MvvmCross.Plugins.PictureChooser;
using MvvmCross.Platform.Droid.Views;
using MvvmCross.Platform.Droid;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform.Exceptions;

namespace MotionsRace.Droid.Services
{
	public class MvxPictureChooserTask : MvxAndroidTask, IMvxPictureChooserTask

	{
		private Uri _cachedUriLocation;
		private RequestParameters _currentRequestParameters;

		#region IMvxPictureChooserTask Members

		public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable, Action assumeCancelled)
		{
			var intent = new Intent(Intent.ActionGetContent);
			intent.SetType("image/*");
			ChoosePictureCommon(MvxIntentRequestCode.PickFromFile, intent, maxPixelDimension, percentQuality,
				pictureAvailable, null, assumeCancelled);
		}

		public void ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailableAsStream, Action assumeCancelled)
		{
			var intent = new Intent(Intent.ActionGetContent);
			intent.SetType("image/*");
			ChoosePictureCommon(MvxIntentRequestCode.PickFromFile, intent, maxPixelDimension, percentQuality,
				null, pictureAvailableAsStream, assumeCancelled);
		}


		public void TakePicture(int maxPixelDimension, int percentQuality, Action<Stream> pictureAvailableAsStream, Action assumeCancelled)
		{
			var intent = new Intent(MediaStore.ActionImageCapture);

			_cachedUriLocation = GetNewImageUri();
			intent.PutExtra(MediaStore.ExtraOutput, _cachedUriLocation);
			intent.PutExtra("outputFormat", Bitmap.CompressFormat.Jpeg.ToString());
			intent.PutExtra("return-data", true);

			ChoosePictureCommon(MvxIntentRequestCode.PickFromCamera, intent, maxPixelDimension, percentQuality,
				null, pictureAvailableAsStream, assumeCancelled);
		}

		public Task<Stream> ChoosePictureFromLibrary(int maxPixelDimension, int percentQuality)
		{
			var task = new TaskCompletionSource<Stream>();
			ChoosePictureFromLibrary(maxPixelDimension, percentQuality,(stream, str) => task.SetResult(stream), () => task.SetResult(null));
			return task.Task;
		}

		public Task<Stream> TakePicture(int maxPixelDimension, int percentQuality)
		{
			var task = new TaskCompletionSource<Stream>();
			TakePicture(maxPixelDimension, percentQuality, task.SetResult, () => task.SetResult(null));
			return task.Task;
		}

		public void ContinueFileOpenPicker(object args)
		{
		}

		#endregion

		private Uri GetNewImageUri()
		{
			// Optional - specify some metadata for the picture
			var contentValues = new ContentValues();
			//contentValues.Put(MediaStore.Images.ImageColumnsConsts.Description, "A camera photo");

			// Specify where to put the image
			return
				Mvx.Resolve<IMvxAndroidGlobals>()
					.ApplicationContext.ContentResolver.Insert(MediaStore.Images.Media.ExternalContentUri, contentValues);
		}

		public void ChoosePictureCommon(MvxIntentRequestCode pickId, Intent intent, int maxPixelDimension,
			int percentQuality, Action<Stream, string> pictureAvailable, Action<Stream> pictureAvailableAsStream, Action assumeCancelled)
		{
			if (_currentRequestParameters != null)
				throw new MvxException("Cannot request a second picture while the first request is still pending");

			_currentRequestParameters = new RequestParameters(maxPixelDimension, percentQuality, pictureAvailable, pictureAvailableAsStream,
				assumeCancelled);
			StartActivityForResult((int) pickId, intent);
		}

		protected override void ProcessMvxIntentResult(MvxIntentResultEventArgs result)
		{
			MvxTrace.Trace("ProcessMvxIntentResult started...");

			Uri uri;

			switch ((MvxIntentRequestCode) result.RequestCode)
			{
			case MvxIntentRequestCode.PickFromFile:
				uri = (result.Data == null) ? null : result.Data.Data;
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "PickFromFile: " + uri);
				break;
			case MvxIntentRequestCode.PickFromCamera:
				uri = _cachedUriLocation;
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "PickFromCamera: " + uri);
				break;
			default:
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Unexpected request received from MvxIntentResult - request was {0}");
				//throw new Exception ("Unexpected request received from MvxIntentResult - request was {0}");
				//MvxTrace.Trace("Unexpected request received from MvxIntentResult - request was {0}",
				//	result.RequestCode);
				// ignore this result - it's not for us
				//MvxTrace.Trace("Unexpected request received from MvxIntentResult - request was {0}",
				//	result.RequestCode);
				return;
			}

			ProcessPictureUri(result, uri);
		}

		private void ProcessPictureUri(MvxIntentResultEventArgs result, Uri uri)
		{
			if (_currentRequestParameters == null)
			{
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Internal error - response received but _currentRequestParameters is null");
				//throw new Exception("Internal error - response received but _currentRequestParameters is null");
				//MvxTrace.Error("Internal error - response received but _currentRequestParameters is null");
				return; // we have not handled this - so we return null
			}

			var responseSent = false;
			try
			{
				// Note for furture bug-fixing/maintenance - it might be better to use var outputFileUri = data.GetParcelableArrayExtra("outputFileuri") here?
				if (result.ResultCode != Result.Ok)
				{
					//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "ResultCode: " + result.ResultCode.ToString());
					// TODO: add debug infor here

					//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Non-OK result received from MvxIntentResult - {0} - request was {1}");
					//MvxTrace.Trace("Non-OK result received from MvxIntentResult - {0} - request was {1}",
					//	result.ResultCode, result.RequestCode);
					return;
				}

				if (uri == null
					|| string.IsNullOrEmpty(uri.Path))
				{
					//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Empty uri or file path received for MvxIntentResult");
					//MvxTrace.Trace("Empty uri or file path received for MvxIntentResult");
					return;
				}

				MvxTrace.Trace("Loading InMemoryBitmap started...");
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Loading InMemoryBitmap started...");
				var memoryStream = LoadInMemoryBitmap(uri);
				if (memoryStream == null)
				{
					//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Loading InMemoryBitmap failed...");
					//MvxTrace.Trace("Loading InMemoryBitmap failed...");
					return;
				}
				MvxTrace.Trace("Loading InMemoryBitmap complete...");
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Loading InMemoryBitmap complete...");
				responseSent = true;
				MvxTrace.Trace("Sending pictureAvailable...");
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Sending pictureAvailable...");
				_currentRequestParameters.PictureAvailable(memoryStream, null);
				MvxTrace.Trace("pictureAvailable completed...");
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "pictureAvailable completed...");
				return;
			}
			finally
			{
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Finally");
				if (!responseSent) {
					//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Finally - AssumeCancelled");
					_currentRequestParameters.AssumeCancelled ();
				}

				_currentRequestParameters = null;
			}
		}

		private MemoryStream LoadInMemoryBitmap(Uri uri)
		{
			var memoryStream = new MemoryStream();
			var bitmap = LoadScaledBitmap(uri);
			if (bitmap == null)
				return null;
			using (bitmap)
			{
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, _currentRequestParameters.PercentQuality, memoryStream);
			}
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}

		private Bitmap LoadScaledBitmap(Uri uri)
		{
			ContentResolver contentResolver = Mvx.Resolve<IMvxAndroidGlobals>().ApplicationContext.ContentResolver;
			var maxDimensionSize = GetMaximumDimension(contentResolver, uri);
			var sampleSize = (int) Math.Ceiling((maxDimensionSize)/
				((double) _currentRequestParameters.MaxPixelDimension));
			if (sampleSize < 1)
			{
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Warning - sampleSize of {0} was requested - how did this happen - based on requested {1} and returned image size ");
				//throw new Exception("Warning - sampleSize of {0} was requested - how did this happen - based on requested {1} and returned image size {2}");
				// this shouldn't happen, but if it does... then trace the error and set sampleSize to 1
				//MvxTrace.Trace(
				//	"Warning - sampleSize of {0} was requested - how did this happen - based on requested {1} and returned image size {2}",
				//	sampleSize,
				//	_currentRequestParameters.MaxPixelDimension,
				//	maxDimensionSize);
				// following from https://github.com/MvvmCross/MvvmCross/issues/565 we return null in this case
				// - it suggests that Android has returned a corrupt image uri
				return null;
			}
			var sampled = LoadResampledBitmap(contentResolver, uri, sampleSize);
			try
			{
				var rotated = ExifRotateBitmap(contentResolver, uri, sampled);
				return rotated;
			}
			catch (Exception pokemon)
			{
				Mvx.Trace("Problem seem in Exit Rotate {0}", pokemon.ToLongString());
				return sampled;
			}
		}

		private Bitmap LoadResampledBitmap(ContentResolver contentResolver, Uri uri, int sampleSize)
		{
			using (var inputStream = contentResolver.OpenInputStream(uri))
			{
				var optionsDecode = new BitmapFactory.Options {InSampleSize = sampleSize};

				return BitmapFactory.DecodeStream(inputStream, null, optionsDecode);
			}
		}

		private static int GetMaximumDimension(ContentResolver contentResolver, Uri uri)
		{
			using (var inputStream = contentResolver.OpenInputStream(uri))
			{
				var optionsJustBounds = new BitmapFactory.Options
				{
					InJustDecodeBounds = true
				};
				var metadataResult = BitmapFactory.DecodeStream(inputStream, null, optionsJustBounds);
				var maxDimensionSize = Math.Max(optionsJustBounds.OutWidth, optionsJustBounds.OutHeight);
				return maxDimensionSize;
			}
		}

		private Bitmap ExifRotateBitmap(ContentResolver contentResolver, Uri uri, Bitmap bitmap)
		{
			if (bitmap == null)
				return null;

			var exif = new Android.Media.ExifInterface(GetRealPathFromUri(contentResolver, uri));
			var rotation = exif.GetAttributeInt(Android.Media.ExifInterface.TagOrientation, (Int32)Android.Media.Orientation.Normal);
			var rotationInDegrees = ExifToDegrees(rotation);
			if (rotationInDegrees == 0)
				return bitmap;

			using (var matrix = new Matrix())
			{
				matrix.PreRotate(rotationInDegrees);
				return Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, matrix, true);
			}
		}

		private String GetRealPathFromUri(ContentResolver contentResolver, Uri uri)
		{
			var proj = new String[] { MediaStore.Images.ImageColumns.Data };
			using (var cursor = contentResolver.Query(uri, proj, null, null, null))
			{
				var columnIndex = cursor.GetColumnIndexOrThrow(MediaStore.Images.ImageColumns.Data);
				cursor.MoveToFirst();
				return cursor.GetString(columnIndex);
			}
		}

		private static Int32 ExifToDegrees(Int32 exifOrientation)
		{
			switch (exifOrientation)
			{
			case (Int32)Android.Media.Orientation.Rotate90:
				return 90;
			case (Int32)Android.Media.Orientation.Rotate180:
				return 180;
			case (Int32)Android.Media.Orientation.Rotate270:
				return 270;
			}

			return 0;
		}




		#region Nested type: RequestParameters

		private class RequestParameters
		{
			public RequestParameters(int maxPixelDimension, int percentQuality, Action<Stream, string> pictureAvailable, Action<Stream> pictureAvailableAsStream,
				Action assumeCancelled)
			{
				PercentQuality = percentQuality;
				MaxPixelDimension = maxPixelDimension;
				AssumeCancelled = assumeCancelled;
				PictureAvailable = pictureAvailable;
				PictureAvailableAsStream = pictureAvailableAsStream;

			}

			public Action<Stream, string> PictureAvailable { get; private set; }
			public Action<Stream> PictureAvailableAsStream { get; private set; }
			public Action AssumeCancelled { get; private set; }
			public int MaxPixelDimension { get; private set; }
			public int PercentQuality { get; private set; }
		}

		#endregion
	}
}

