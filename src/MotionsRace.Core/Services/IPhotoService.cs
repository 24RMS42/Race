using System;
using System.IO;
using System.Threading.Tasks;

namespace MotionsRace.Core.Services
{
	public interface IPhotoService
	{
		void GetPictureFromLibrary (int maxPixelDimension, int percentQuality);
		void GetPictureFromCamera (int maxPixelDimension, int percentQuality);
        byte[] GetBytes(Stream stream);
	}
}

