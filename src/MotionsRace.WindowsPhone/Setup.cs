using Acr.MvvmCross.Plugins.UserDialogs;
using Cirrious.CrossCore;
using Cirrious.CrossCore.Platform;
using Cirrious.MvvmCross.ViewModels;
using Cirrious.MvvmCross.WindowsCommon.Platform;
using MotionsRace.Core.Services;
using MotionsRace.WindowsPhone.Services;
using Windows.UI.Xaml.Controls;
using System.Linq;
using Cirrious.MvvmCross.Plugins.PictureChooser;

namespace MotionsRace.WindowsPhone
{
    public class Setup : MvxWindowsSetup
    {
        public Setup(Frame rootFrame) : base(rootFrame)
        {
        }

        protected override IMvxApplication CreateApp()
        {
            return new Core.App();
        }
		
        protected override IMvxTrace CreateDebugTrace()
        {
            return new DebugTrace();
        }

        protected override void InitializeFirstChance()
        {            
            Mvx.RegisterSingleton<IPlatformService>(new PlatformService());
            Mvx.RegisterSingleton<IMessageService>(new MessageService());
            Mvx.RegisterSingleton<INavigationService>(new NavigationService());
            Mvx.RegisterSingleton<IUserDialogService>(new WinStoreUserDialogService());            
            base.InitializeFirstChance();
        }

        protected override void InitializeLastChance()
        {
            var preferencesLangs = Windows.System.UserProfile.GlobalizationPreferences.Languages.ToList();
            Mvx.Resolve<ILanguageService>().SetCurrentLanguage(preferencesLangs);
            
            base.InitializeLastChance();
        }
    }
}