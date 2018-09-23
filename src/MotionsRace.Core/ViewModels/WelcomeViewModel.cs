using MotionsRace.Core.Services;
using System.Collections.Generic;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
    public class WelcomeViewModel : BaseScreenViewModel
    {
        private List<string> _welcomeSlides;
        public List<string> WelcomeSlides
        {
            get { return _welcomeSlides; }
            set
            {
                _welcomeSlides = value;
                RaisePropertyChanged(() => WelcomeSlides);
            }
        }

        public string WelcomeText
        {
            get
            {
                if (this.Theme.Name == "Sverigestafetten")
                    return this["Login_SplashtextAtea"];
                else if (this.Theme.Name == "Norden Activity Challenge")
                    return this["Login_SplashtextNorden"];
                else
                    return this["Login_Splashtext"];
            }
        }

        public IMvxCommand SignInCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<SignInViewModel>();
                });
            }
        }

        public IMvxCommand SignUpCommand
        {
            get
            {
                return new MvxCommand(() =>
		          {
		              var navigateUrl = Theme.SignUpURL;
		              ShowViewModel<WebViewModel>(new { url = navigateUrl });
		              //PlatformService.LauchUrl(Theme.SignUpURL);
		          });
            }
        }

        public WelcomeViewModel(INavigationService navigationService, IPlatformService platformService,
            IMvxMessenger messenger)
            : base(navigationService, platformService, messenger)
        {
        }

        public void Init()
        {
            NavigationService.ClearNavStack();
            WelcomeSlides = new List<string> { "/Assets/" + Theme.Images.FirstSlide, "/Assets/" + Theme.Images.SecondSlide, "/Assets/" + Theme.Images.ThirdSlide };
        }
    }
}
