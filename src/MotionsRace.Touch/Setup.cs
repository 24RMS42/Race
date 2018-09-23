using UIKit;
using MotionsRace.Core.Services;
using Foundation;
using System.Linq;
using MotionsRace.Services;
using MvvmCross.iOS.Platform;
using MvvmCross.Core.ViewModels;
using MvvmCross.Binding.Bindings.Target;
using MvvmCross.Binding;
using MvvmCross.Platform.Platform;
using MvvmCross.Platform;
using System.Collections.Generic;

namespace MotionsRace.Touch
{
	public class Setup : MvxIosSetup	{
		public Setup(MvxApplicationDelegate applicationDelegate, UIWindow window)
            : base(applicationDelegate, window)
		{
		}

		protected override IMvxApplication CreateApp()
		{
			return new Core.App();
		}

		protected override MvvmCross.Platform.Platform.IMvxTrace CreateDebugTrace()
		{
			return base.CreateDebugTrace();
		}

		protected override void InitializeFirstChance()
		{            
			//UIButton.Appearance.TintColor = UIColor.LightGray;
			//UINavigationBar.Appearance.BarTintColor = UIColor.YourColor;
			UINavigationBar.Appearance.SetTitleTextAttributes( new UITextAttributes { TextColor = UIColor.White});
			UIButton.Appearance.SetTitleColor (UIColor.White, UIControlState.Normal);

			Mvx.RegisterSingleton<IPlatformService>(new PlatformService());
			Mvx.RegisterSingleton<IMessageService>(new MessageService());
			Mvx.RegisterSingleton<INavigationService>(new NavigationService());
			Mvx.RegisterSingleton<IPhotoService>(new PhotoService());
			Mvx.RegisterSingleton<IImageResizeService>(new ImageResizeService());
			Mvx.RegisterSingleton<IInsights>(new XamarinInsights());

			base.InitializeFirstChance();
		}

		protected override void InitializeLastChance()
		{
			var preferencesLangs = NSLocale.PreferredLanguages.ToList ();

#if NORDEN
			preferencesLangs = new List<string>() { "en" };
#endif


			Mvx.Resolve<ILanguageService>().SetCurrentLanguage(preferencesLangs);
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

		//protected override void FillTargetFactories(MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
		//{
		//	registry.RegisterCustomBindingFactory<UIButton>("TitleColor", button => new MvxUIButtonTitleColorTargetBinding(button));
		//	base.FillTargetFactories(registry);
		//}

		//protected override void FillTargetFactories (Cirrious.MvvmCross.Binding.Bindings.Target.Construction.IMvxTargetBindingFactoryRegistry registry)
		//{
		//	registry.RegisterCustomBindingFactory<UIButton> ("TitleColor", button => new MvxUIButtonTitleColorTargetBinding(button));
		//	base.FillTargetFactories (registry);
		//}
	}

	public class MvxUIButtonTitleColorTargetBinding : MvxConvertingTargetBinding
	{
		protected UIButton Button
		{
			get { return base.Target as UIButton; }
		}

		public MvxUIButtonTitleColorTargetBinding(UIButton button)
			: base(button)
		{
			if (button == null)
			{
				MvxBindingTrace.Trace(MvxTraceLevel.Error, "Error - UIButton is null in MvxUIButtonTitleColorTargetBinding");
			}
		}

		public override MvxBindingMode DefaultMode
		{
			get { return MvxBindingMode.OneWay; }
		}

		public override System.Type TargetType
		{
			get { return typeof (UIColor); }
		}

		protected override void SetValueImpl(object target, object value)
		{
			((UIButton)target).SetTitleColor(value as UIColor, UIControlState.Normal);
		}
	}

	/*
	public class asd : MvxModalNavSupportTouchViewPresenter
	{
		public override void Show (MvxViewModelRequest request)
		{
			base.Show (request);
		}


	}
	*/
}