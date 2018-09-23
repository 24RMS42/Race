using System.IO;
using System.Threading.Tasks;
using MotionsRace.Core.Messages;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.PictureChooser;

namespace MotionsRace.Core.Services
{
	public class PhotoService : IPhotoService
	{
		public byte[] GetBytes(Stream stream)
		{
			using (var memoryStream = new MemoryStream())
			{
				stream.CopyTo(memoryStream);
				return memoryStream.ToArray();
			}
		}

		public void GetPictureFromLibrary(int maxPixelDimension, int percentQuality)
		{
			try
			{
				getPicture(maxPixelDimension, percentQuality, PictureResource.Library);
			}
			catch 
			{
				Mvx.Resolve<IMessageService>().ShowAlertAsync("", Mvx.Resolve<ILanguageService>().GetString("Activity_PictureError"));
			}
		}

		public void GetPictureFromCamera(int maxPixelDimension, int percentQuality)
		{
			try
			{
				getPicture(maxPixelDimension, percentQuality, PictureResource.Camera);
			}
			catch 
			{
				Mvx.Resolve<IMessageService>().ShowAlertAsync("", Mvx.Resolve<ILanguageService>().GetString("Activity_PictureError"));
			}
		}

		private void getPicture(int maxPixelDimension, int percentQuality, PictureResource pictureResource)
		{
			var chooseService = Mvx.Resolve<IMvxPictureChooserTask>();

		    if (pictureResource == PictureResource.Camera)
		    {
				chooseService.TakePicture (maxPixelDimension, percentQuality, Success, () => {});
		    }
            else
		    {
				chooseService.ChoosePictureFromLibrary(maxPixelDimension, percentQuality, Success, () => {});
		    }
		}

		public void Success(Stream stream)
		{
			getImageFromStream (stream);
		}

	    private void getImageFromStream(Stream stream)
		{
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "getImageFromStream");

			var messanger = Mvx.Resolve<IMvxMessenger>();
			var message = new ImageChooseMessage(this, stream != null ? GetBytes(stream) : null);
			messanger.Publish(message);
		}
	}

	public enum PictureResource
	{
		Camera,
		Library
	}
}

