using System;
using Acr.UserDialogs;
using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Services;
using MotionsRace.Core.Themes;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Core;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
	public abstract class BaseScreenViewModel : MvxViewModel, IDisposable
	{
		protected readonly INavigationService NavigationService;
		protected readonly IPlatformService PlatformService;
		public readonly IMvxMessenger Messenger;
		private readonly BindColors _bindColors;

		protected BaseScreenViewModel(INavigationService navigationService, IPlatformService platformService, IMvxMessenger messenger)
		{
			NavigationService = navigationService;
			PlatformService = platformService;
			Messenger = messenger;
			_bindColors = new BindColors();
		}

		public IMvxCommand GoBackCommand
		{
			get { return new MvxCommand(() => NavigationService.GoBack()); }
		}

		public IMvxCommand ExitCommand
		{
			get
			{
				return new MvxCommand(async () =>
				{
					if (!OnTryClose())
					{
						return;
					}
					var mustExit = await Mvx.Resolve<IUserDialogs>().ConfirmAsync(
						this["GLOBAL_Exit_Dialog_Message"],
						this["GLOBAL_Exit_Dialog_Title"],
						this["GLOBAL_Exit_Dialog_Button_Yes"],
						this["GLOBAL_Exit_Dialog_Button_No"]);

					if (mustExit)
					{
						PlatformService.AppExit();
					}
				});
			}
		}

		private bool _isBusy;
		public bool IsBusy
		{
			get { return _isBusy; }
			set
			{
				_isBusy = value;
				RaisePropertyChanged(() => IsBusy);
			}
		}

		private string _statusMessage;
		public string StatusMessage
		{
			get { return _statusMessage; }
			set
			{
				_statusMessage = value;
				RaisePropertyChanged(() => StatusMessage);
			}
		}

		public string this[string index]
		{
			get
			{
				var res = string.Empty;
				try
				{
					res = Mvx.Resolve<ILanguageService>().GetString(index);
				}
				catch (Exception e)
				{
					throw new Exception(e.Message);
				}
				return res;;
			}
		}

		public static ILanguageService LanguageService
		{
			get
			{
				return Mvx.Resolve<ILanguageService>();
			}
		}

		protected virtual bool OnTryClose()
		{
			return true;
		}

		public void ShowBusy(string msg = "")
		{
			IsBusy = true;
			StatusMessage = msg;
		}

		public void HideBusy()
		{
			IsBusy = false;
			StatusMessage = "";
		}

		public void RunOnMainThread(Action action)
		{
			var dispatcher = Mvx.Resolve<IMvxMainThreadDispatcher>();
			dispatcher.RequestMainThreadAction(action);
		}

		public ITheme Theme
		{
			get
			{
				return Mvx.Resolve<IThemesManager>().CurrentTheme;
			}
		}

		public virtual void Dispose()
		{
		}
	}
}