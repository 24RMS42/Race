using System;
using Android.Graphics;
using Android.Content.Res;
using System.IO;
using Android.Text;
using Xamarin;

namespace MotionsRace.Droid.Helpers
{
	public struct ImageHelper
	{
		public static Bitmap DecodeSampledBitmapFromStream(Stream streem, int reqWidth, int reqHeight, string text = "")
		{
			Bitmap result = null;
			try
			{
				var options = GetBitmapOption(reqWidth, reqHeight);
				result = string.IsNullOrEmpty(text)
					? BitmapFactory.DecodeStream(streem, new Rect(0, 0, reqWidth, reqHeight), options)
					: createImage(BitmapFactory.DecodeStream(streem, new Rect(0, 0, reqWidth, reqHeight), options), text);
			}
			catch (Exception e)
			{
				//Insights.Report(e);
			}

			return result;
		}

		public static Bitmap DecodeSampledBitmapFromResource(Resources res, int resId, int reqWidth, int reqHeight, string text = "")
		{
			Bitmap result = null;
			try
			{
				var options = GetBitmapOption(reqWidth, reqHeight);
				result = string.IsNullOrEmpty (text) 
				? BitmapFactory.DecodeResource (res, resId, options) 
				: createImage (BitmapFactory.DecodeResource (res, resId, options), text);
			}
			catch (Exception e)
			{
				//Insights.Report(e);
			}

			return result;
		}

		private static BitmapFactory.Options GetBitmapOption(int reqWidth, int reqHeight)
		{
			// First decode with inJustDecodeBounds=true to check dimensions
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;
			//BitmapFactory.DecodeResource(res, resId, options);

			// Calculate inSampleSize
			options.InSampleSize = CalculateInSampleSize(options, reqWidth, reqHeight);

			// Decode bitmap with inSampleSize set
			options.InJustDecodeBounds = false;

			return options;
		}

		public static int CalculateInSampleSize( BitmapFactory.Options options, int reqWidth, int reqHeight) {
			// Raw height and width of image
			int height = options.OutHeight;
			int width = options.OutWidth;
			int inSampleSize = 1;

			if (height > reqHeight || width > reqWidth) {

				int halfHeight = height / 2;
				int halfWidth = width / 2;

				// Calculate the largest inSampleSize value that is a power of 2 and keeps both
				// height and width larger than the requested height and width.
				while ((halfHeight / inSampleSize) >= reqHeight&& (halfWidth / inSampleSize) >= reqWidth) 
				{
					inSampleSize *= 2;
				}
			}
			return inSampleSize;
		}

		public static Bitmap createImage( Bitmap bitmap ,String user_text) {
			// canvas object with bitmap image as constructor
			Bitmap bitm = Bitmap.CreateBitmap(bitmap);

			Bitmap mutableBitmap = bitm.Copy (Bitmap.Config.Argb8888, true);

			Canvas canvas = new Canvas(mutableBitmap );
			TextPaint tp = new TextPaint();
			tp.Color = Android.Graphics.Color.White;
			tp.TextSize = 40;
			//tp.TextAlign =   Android.Graphics.Paint.Align.Center;
			tp.AntiAlias = true;

			// draw text to the Canvas center
			StaticLayout sl = new StaticLayout(user_text, tp, canvas.Width-30, Layout.Alignment.AlignCenter, 1, 0, false);
			canvas.Translate(30, canvas.Height / 2);
			sl.Draw(canvas);
			return mutableBitmap;
		}
	}
}