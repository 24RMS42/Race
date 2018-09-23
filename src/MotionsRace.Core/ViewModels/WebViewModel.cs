using System;
using MotionsRace.Core.Services;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
    public class WebViewModel: BaseScreenViewModel
    {
		private string _url;
		public string Url
		{
			get { return _url; }
			set
			{
				_url = value;
				RaisePropertyChanged(() => Url);
			}
		}

		public WebViewModel(INavigationService navigationService, IPlatformService platformService,
			IMvxMessenger messenger)
            : base(navigationService, platformService, messenger)
        {
		}

		public void Init(string url)
		{
			Url = url;
		}
    }
}