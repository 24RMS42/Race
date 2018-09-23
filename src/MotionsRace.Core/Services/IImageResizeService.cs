using System;

namespace MotionsRace.Core.Services
{
	public interface IImageResizeService
	{
		byte[] ResizeImage (byte[] imageData, float width, float height, int jpegQuality);
	}
}

