using MotionsRace.Core.Services;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MotionsRace.WinPhone.Services
{
	public class ImageResizeService : IImageResizeService
	{
		public byte[] ResizeImage(byte[] imageData, float width, float height, int jpegQuality)
		{
			var originalImage = new BitmapImage();
			originalImage.SetSource(new MemoryStream(imageData));

			float finalWidth = 0;
			float finalHeight = 0;
			float originalWidth = originalImage.PixelWidth;
			float originalHeight = originalImage.PixelHeight;

			if (width > 0 && height > 0)
			{
				finalWidth = width;
				finalHeight = height;
			}
			else if (width == 0 && height > 0)
			{
				finalWidth = (int) (height/originalHeight*originalWidth);
				finalHeight = height;
			}
			else if (width > 0 && height == 0)
			{
				finalWidth = width;
				finalHeight = (int) (width/originalWidth*originalHeight);
			}
			else if (width == 0 && height == 0)
			{
				finalWidth = width;
				finalHeight = height;
			}

			var temporaryImage = new Image { Source = originalImage, Width = originalWidth, Height = originalHeight };
			var wBitmap = new WriteableBitmap((int) finalWidth, (int) finalHeight);
			var scaleTransform = new ScaleTransform
			{
				ScaleX = finalWidth/originalWidth,
				ScaleY = finalHeight/originalHeight
			};
			wBitmap.Render(temporaryImage, scaleTransform);
			wBitmap.Invalidate();

			using (var ms = new MemoryStream())
			{
				wBitmap.SaveJpeg(ms, (int) finalWidth, (int) finalHeight, 0, jpegQuality);
				return ms.ToArray();
			}
		}
	}
}
