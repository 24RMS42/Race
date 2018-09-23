using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;

namespace MotionsRace.Core
{
	public class CustomAppStart : MvxNavigatingObject, IMvxAppStart
	{
		public void Start(object hint = null)
		{
			var settingsService = Mvx.Resolve<ISettingsService>();

			if (settingsService.Options.CredentialsExists)
			{
				if (settingsService.IsNewVersion)
				{
					settingsService.Options.Reset();
					ShowViewModel<SignInViewModel>();
				}
				else
				{
					//TODO: delete 
					Mvx.Resolve<IWebService>().LoadCredentialsFromSettings();
					ShowViewModel<MainViewModel>();
				}
			}
			else
			{
				ShowViewModel<WelcomeViewModel>();
			}
		}
	}
}
