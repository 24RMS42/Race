using System;
using MotionsRace.Core.Services;
using System.IO;
using UIKit;
using Foundation;

namespace MotionsRace.Touch
{
	public class ImageResizeService : IImageResizeService
	{
		public byte[] ResizeImage(byte[] imageData, float width, float height, int jpegQuality)
		{
			var img = ToImage(imageData);

			float finalWidth = 0;
			float finalHeight = 0;
			nfloat originalWidth = img.Size.Width;
			nfloat originalHeight = img.Size.Height;

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

			var scaledImage = img.Scale (new CoreGraphics.CGSize (finalWidth, finalHeight));
			return ToNSData (scaledImage);
		}

		public byte[] ToNSData(UIImage image){

			if (image == null) {
				return null;
			}
			NSData data = null;

			try {
				data = image.AsJPEG();
				return data.ToArray ();
			} catch (Exception ) {
				return null;
			}
			finally
			{
				if (image != null) {
					image.Dispose ();
					image = null;
				}
				if (data != null) {
					data.Dispose ();
					data = null;
				}
			}
		}

		public UIImage ToImage(byte[] data)
		{
			if (data==null) {
				return null;
			}
			UIImage image = null;
			try {

				image = new UIImage(NSData.FromArray(data));
				data = null;
			} catch (Exception ) {
				return null;
			}
			return image;
		}



	}
}

