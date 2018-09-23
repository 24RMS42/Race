using Acr.UserDialogs;
using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
	public class ImageViewerViewModel : HeaderScreenViewModel
	{
		private string _imageURL;

		public ImageViewerViewModel(INavigationService navigationService, IUserDialogs dialogService,
			IPlatformService platformService, IMvxMessenger messenger)
			: base(navigationService, dialogService, platformService, messenger)
		{
		}

		public string ImageURL
		{
			get { return _imageURL; }
			set
			{
				_imageURL = value;
				RaisePropertyChanged(() => ImageURL);
			}
		}

		public IMvxCommand Close
		{
			get { return new MvxCommand(() => { Close(this); }); }
		}

		public void Init(string imageURL)
		{
			ImageURL = imageURL;
		}
	}
}