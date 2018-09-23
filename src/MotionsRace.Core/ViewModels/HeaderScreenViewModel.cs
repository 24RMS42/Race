using System.Linq;
using Acr.UserDialogs;
using MotionsRace.Core.Helpers;
using MotionsRace.Core.Messages;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
	public class HeaderScreenViewModel : BaseScreenViewModel
	{
		protected readonly IUserDialogs DialogService;

		public HeaderScreenViewModel(INavigationService navigationService, IUserDialogs dialogService, IPlatformService platformService, IMvxMessenger messenger)
			: base(navigationService, platformService, messenger)
		{
			DialogService = dialogService;
		}

		private bool _showLoginButton;
		public bool ShowLiveRecordButton
		{
			get
			{
				_showLiveRecordButton = Mvx.Resolve<ISettingsService>().Options.AllowLiveRecord &&
					GetType() == typeof (MainViewModel);
				return _showLiveRecordButton;
			}
			set
			{
				_showLiveRecordButton = value;
				RaisePropertyChanged(() => ShowLiveRecordButton);
			}
		}

		private bool _showFavoritButton;
		public bool ShowFavoritButton
		{
			get
			{
				return _showFavoritButton;
			}
			set
			{
				_showFavoritButton = value;
				RaisePropertyChanged(() => ShowFavoritButton);
			}
		}
		

		private bool _showLiveRecordButton;
		public bool ShowLoginButton
		{
			get
			{
				_showLoginButton = Mvx.Resolve<ISettingsService>().Options.AllowLogin;
				return _showLoginButton;
			}
			set
			{
				_showLiveRecordButton = value;
				RaisePropertyChanged(() => ShowLoginButton);
			}
		}

		public IMvxCommand LoginCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (!ShowLoginButton)
						return;
					if (PlatformService.IsInternetAvailable())
					{
                        //Mvx.Resolve<IPlatformService>().LauchUrl(UrlHelper.GetAutologinMainUrl());
                        var navigateUrl = UrlHelper.GetAutologinMainUrl();
						ShowViewModel<WebViewModel>(new { url = navigateUrl });
					}
					else
					{
						ShowErrorMessage(this["GLOBAL_NoInternetConnection"]);
					}
				});
			}
		}

		public IMvxCommand LiveRecordCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (!ShowLiveRecordButton)
						return;
					if (PlatformService.IsInternetAvailable())
					{
						ShowViewModel<ActivityTypesViewModel>(new {liveRecord = false});
					}
					else
					{
						ShowErrorMessage(this["GLOBAL_NoInternetConnection"]);
					}
				});
			}
		}

		public IMvxCommand FavoritRecordCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (!ShowFavoritButton)
						return;
					if (PlatformService.IsInternetAvailable())
					{
						var vm = this as MainViewModel;
						if (vm != null)
						{
							ShowViewModel<FrequentTrainingViewModel>(new {frequentTrainingTypesString = vm.FrequentTrainingTypesString});
						}
					}
					else
					{
						ShowErrorMessage(this["GLOBAL_NoInternetConnection"]);
					}
				});
			}
		}

		public IMvxCommand TapLogoCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					var viewModels = (this as MainViewModel);
					var message = new RefreshNewsFeedListMessage(this);
					Messenger.Publish(message);

					if (viewModels != null)
					{
						viewModels.LoadDataAsync();
					}
					else
					{
						switch (Mvx.Resolve<IPlatformService>().Platform)
						{
							case Platform.iOS:
								ShowViewModel<MainViewModel>();
								break;
							case Platform.WindowsPhone:
								ShowViewModel<MainViewModel>();
								Mvx.Resolve<INavigationService>().ClearNavStack();
								break;
							default:
								ShowViewModel<MainViewModel>();
								break;
						}
					}
				});
			}
		}

		private async void ShowErrorMessage(string message)
		{
			var viewModels = (this as MainViewModel);
			if (viewModels != null)
			{
				viewModels.ShowErrorMessage(message);
				return;
			}
			await DialogService.AlertAsync(message, this["GLOBAL_Error"], this["GLOBAL_Ok"]);
		}
	}
}