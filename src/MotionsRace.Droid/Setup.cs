using System.Collections.Generic;
using Android.Content;
using MotionsRace.Core.Services;
using MotionsRace.Droid.Services;
using System.Linq;
using MotionsRace.Services;
using MvvmCross.Droid.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.PictureChooser;
using MvvmCross.Platform;
using Acr.UserDialogs;
using MvvmCross.Platform.Logging;

namespace MotionsRace.Droid
{
	public class Setup : MvxAndroidSetup
	{
		public Setup(Context applicationContext) : base(applicationContext)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new Core.App();
		}

		protected override IMvxTrace CreateDebugTrace()
		{
			return new MvxDebugTrace();
		}

		protected override void InitializeFirstChance()
		{
			Mvx.RegisterSingleton<IPlatformService>(new PlatformService());
			Mvx.RegisterSingleton<IMessageService>(new MessageService());
			Mvx.RegisterSingleton<INavigationService>(new NavigationService());
			Mvx.RegisterSingleton<IPhotoService>(new PhotoService());
			Mvx.RegisterSingleton<IImageResizeService>(new ImageResizeService());
			Mvx.RegisterSingleton<IMvxPictureChooserTask> (new MvxPictureChooserTask());
			Mvx.RegisterSingleton<IInsights>(new XamarinInsights());

			base.InitializeFirstChance();
		}

        protected override MvxLogProviderType GetDefaultLogProviderType()
            => MvxLogProviderType.None;

		protected override void InitializeLastChance()
		{
			var lang = Java.Util.Locale.Default;

#if NORDEN
			lang = new Java.Util.Locale("en");
#endif

			Mvx.Resolve<ILanguageService>().SetCurrentLanguage(new [] {lang.ToString()}.ToList());
			Mvx.RegisterSingleton<IWebService>(new WebService());

			var themesManager = new ThemesManager();
#if TWITCH
			themesManager.SetCurrentTheme(ThemesManager.TwitchTheme);
#elif ATEA
            themesManager.SetCurrentTheme(ThemesManager.AteaTheme);
#elif NORDEN
            themesManager.SetCurrentTheme(ThemesManager.NordenMachineryTheme);
#elif COROMATIC
            themesManager.SetCurrentTheme(ThemesManager.CoromaticTheme);
#elif NETENT
			themesManager.SetCurrentTheme(ThemesManager.NetEntTheme);
#elif KRONOBERG
			themesManager.SetCurrentTheme(ThemesManager.KronobergTheme);
#else
			themesManager.SetCurrentTheme(ThemesManager.MotionRaceTheme);
#endif
			Mvx.RegisterSingleton<IThemesManager>(themesManager);

			base.InitializeLastChance();
		}

		protected override IDictionary<string, string> ViewNamespaceAbbreviations
		{
			get
			{
				var toReturn = base.ViewNamespaceAbbreviations;
				toReturn["cc"] = "MotionsRace.Droid.Controls";
				return toReturn;
			}
		}
	}
}