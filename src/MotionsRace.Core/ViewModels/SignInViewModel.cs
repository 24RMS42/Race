using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
	public class SignInViewModel : BaseScreenViewModel
	{
		private readonly IUserDialogs _dialogs;
		private readonly IMessageService _messageService;
		private readonly ISettingsService _settingsService;
		private readonly IWebService _webService;

		public SignInViewModel(INavigationService navigationService,
			ISettingsService settingsService,
			IMessageService messageService,
			IWebService webService,
			IUserDialogs dialogService,
			IPlatformService platformService,
			IMvxMessenger messenger
			)
			: base(navigationService, platformService, messenger)
		{
			_webService = webService;
			_settingsService = settingsService;
			_messageService = messageService;
			_dialogs = dialogService;
		}

		private string _userName;
		public string UserName
		{
			get { return _userName; }
			set
			{
				_userName = value;
				RaisePropertyChanged(() => UserName);
			}
		}

		private string _password;
		public string Password
		{
			get { return _password; }
			set
			{
				_password = value;
				RaisePropertyChanged(() => Password);
			}
		}

		public IMvxCommand SignInCommand
		{
            get { return new MvxCommand(SignInByEmailAndPasswordAsync); }
		}

		public IMvxCommand ForgotPassword
		{
			get
			{
                return new MvxCommand(() => 
                {
                    var navigateUrl = Theme.ForgotPasswordURL;
					ShowViewModel<WebViewModel>(new { url = navigateUrl });
                    //Mvx.Resolve<IPlatformService>().LauchUrl(Theme.ForgotPasswordURL); 
                });
			}
		}

		public string AppVersion
		{
			get { return "v" + Mvx.Resolve<IPlatformService>().GetAppVersion(); }
		}

		public void Init()
		{
#if DEBUG
			// TEST ACCOUNTS
			// https://www.dropbox.com/s/2a5is6w29cq7kdv/Test%20accounts%20DEV%20and%20PROD.docx?dl=0

			// Develop
			//UserName = "dev-jen.sen"; // One Category
			//UserName = "dev-petra"; // avatar
			//UserName = "dev-lev71"; // race end data < today (no register button)
			//UserName = "dev-ewab"; // 2 races, avatar
			//UserName = "develop.marit"; 
			//UserName = "develop.elisabeth.granstrom@kalixtele24.se"; 
			//UserName = "cejn.lars.darolf";

			//UserName = "james1"; //share to facebook

			// Prodaction
			//UserName = "zingmar";
			//UserName = "duffy";
            UserName = "hans.niward@atea.se";

            Password = "123";
			//Password = "123";
			//Password = "utmanad";
#endif
		}

        public void SignInByEmailAndPasswordAsync()
        {
            SignInAsync(isOAuth: false);
        }

		public void SignInByOAuthAsync()
		{
		    SignInAsync(isOAuth: true);
		}

        public async void SignInAsync(bool isOAuth = false)
		{
			if (!PlatformService.IsInternetAvailable())
			{
				_messageService.ShowAlertAsync(this["GLOBAL_Error"], this["GLOBAL_NoInternetConnection"]);
				return;
			}

			if (string.IsNullOrWhiteSpace(UserName))
			{
				_messageService.ShowAlertAsync(this["GLOBAL_Error"], this["Login_EmailRequired"]);
				return;
			}

			if (string.IsNullOrWhiteSpace(Password))
			{
				_messageService.ShowAlertAsync(this["GLOBAL_Error"], this["Login_PasswordRequired"]);
				return;
			}

			var userName = UserName.Trim();
			Password = Password.Trim();
			var serviceMode = WebServiceSettings.GetServiceModeByUserName(ref userName);
			_webService.SetMode(serviceMode);

			ShowBusy();
			var result = await WebServiceSettings.LoginServerLogic(_webService, serviceMode, userName, Password, isOAuth);

			if (result.IsSuccess)
			{
				var racesResult = await _webService.GetRacesAsync();
				RunOnMainThread(HideBusy);

				if (racesResult.IsSuccess)
				{
					if (racesResult.Response.Result.Count == 0)
					{
						_messageService.ShowAlertAsync("", this["Login_NoActiveRaces"]);
						return;
					}
					if (racesResult.Response.Result.Count == 1)
					{
						showView(racesResult.Response.Result[0]);
					}
					else
					{
						var actionSheetConfig = new ActionSheetConfig().SetTitle(this["Login_SelectRace"]);
						foreach (var item in racesResult.Response.Result.OrderByDescending(x => x.EndDate))
						{
							actionSheetConfig.Add(item.RaceTitle, () => showView(item));
						}
						_dialogs.ActionSheet(actionSheetConfig);
					}
				}
				else
				{
					_messageService.ShowAlertAsync (this ["GLOBAL_Error"], racesResult.ApiErrorMessage);

					//ShowViewModel<MainViewModel>(new { disableMode = true });
				}
				RunOnMainThread(HideBusy);
			}
			else
			{
				if (result.ApiErrorMessage.Contains ("ApplicationIDOutdatedVersionException")) {
					_messageService.ShowAlertAsync (this ["GLOBAL_ERROR_API_NeedUpdate"], "");
				} else {
					_messageService.ShowAlertAsync (this ["GLOBAL_Error"], result.ApiErrorMessage);
				}
			}
			RunOnMainThread(HideBusy);
		}

		private void showView(GetRacesResult race)
		{
			// TODO: validate, id date end etc...
			_settingsService.Options.RaceID = race.RaceID;
			_settingsService.Options.RaceStartDate = race.StartDate;
			_settingsService.Options.RaceEndDate = race.EndDate;

			_settingsService.Save();
			_webService.LoadCredentialsFromSettings();

			ShowViewModel<MainViewModel>();
			NavigationService.ClearNavStack();
		}
	}
}