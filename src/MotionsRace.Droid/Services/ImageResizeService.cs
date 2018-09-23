using System;
using MotionsRace.Core.Services;
using Android.Graphics;
using System.IO;

namespace MotionsRace.Droid
{
	public class ImageResizeService : IImageResizeService
	{
		public byte[] ResizeImage(byte[] imageData, float width, float height, int jpegQuality)
		{
			
			Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
			float finalWidth = 0;
			float finalHeight = 0;
			float originalWidth = originalImage.Width;
			float originalHeight = originalImage.Height;

			if (width > 0 && height > 0) 
			{
				finalWidth = width;
				finalHeight = height;
			} 
			else if (width == 0 && height > 0) 
			{
				finalWidth = (int)(height/originalHeight*originalWidth);
				finalHeight = height;
			} 
			else if (width > 0 && height == 0) 
			{
				finalWidth = width;
				finalHeight = (int)(width/originalWidth*originalHeight);
			} 
			else if (width == 0 && height == 0) 
			{
				finalWidth = width;
				finalHeight = height;
			}
			Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)finalWidth, (int)finalHeight, false);
			// 
			using (MemoryStream ms = new MemoryStream())
			{
				resizedImage.Compress(Bitmap.CompressFormat.Jpeg, jpegQuality, ms);
				return ms.ToArray();
			}
		}



	}
}

