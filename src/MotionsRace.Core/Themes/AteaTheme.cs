using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Themes.Base;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core.Themes
{
	public class AteaTheme : ITheme
	{
		public string Name { get { return "Sverigestafetten"; } }
		public string SignUpURL { get { return "http://login.sverigestafetten.se/signup/index.aspx"; } }
		public string ForgotPasswordURL { get { return "http://login.sverigestafetten.se/forgotpassword.aspx"; } }

		public IThemeColors Colors { get; set; }
		public IThemeImages Images { get; set; }

		public AteaTheme()
		{
			Colors = new AteaThemeThemColors();
			Images = new AteaThemeThemImages();
		}

		public class AteaThemeThemColors : IThemeColors
		{
			public static readonly MvxColor COLOR_BLACK = new MvxColor(68, 68, 68);
			public static readonly MvxColor COLOR_WHITE = new MvxColor(255, 255, 255);
			public static readonly MvxColor COLOR_LIGHTBLACK = new MvxColor(51,51,51);

			public static readonly MvxColor COLOR_BLUE = new MvxColor(0, 120, 195);
			public static readonly MvxColor COLOR_LIGHTBLUE = new MvxColor(0, 156, 254);
			public static readonly MvxColor COLOR_DARKBLUE = new MvxColor(0, 70, 130);

            public static readonly MvxColor COLOR_GREEN = new MvxColor(100, 190, 40);

            public static readonly MvxColor COLOR_GRAY = new MvxColor(245, 245, 245);
			public static readonly MvxColor COLOR_LIGHTGRAY = new MvxColor(240, 235, 240);
			public static readonly MvxColor COLOR_DARKGRAY = new MvxColor(90, 90, 90);

			// Main
			public MvxColor MainProgressBarForegroundColor { get { return COLOR_BLUE; } }
			public MvxColor MainProgressBarBackgroundColor { get { return COLOR_DARKGRAY; } }

			public MvxColor MainColor { get { return COLOR_GRAY; } }
			public MvxColor TextColor { get { return COLOR_BLACK; } }

			public MvxColor HeaderBottomBorderColor { get { return new MvxColor(0, 0, 0, 0); } }
			public MvxColor HeaderColor { get { return COLOR_BLUE; } }
			public MvxColor TextHeaderColor { get { return COLOR_WHITE; } }

			public MvxColor PageTitleColor { get { return COLOR_WHITE; } }
			public MvxColor TextPageTitleColor { get { return COLOR_DARKBLUE; } }
			public MvxColor TextPageSecondaryTitleColor { get { return COLOR_BLACK; } }

			//Splash screen
			public MvxColor WelcomeSignUpColor { get { return COLOR_BLUE; } }
			public MvxColor WelcomeSignInColor { get { return COLOR_WHITE; } }
			public MvxColor WelcomeSignUpTextColor { get { return COLOR_WHITE; } }
			public MvxColor WelcomeSignInTextColor { get { return COLOR_BLUE; } }
			public MvxColor WelcomeSliderIndicatorNotSelectorColor { get { return new MvxColor(0, 0, 0, 125); } }
			public MvxColor WelcomeSliderIndicatorSelectorColor { get { return COLOR_BLUE; } }

			public MvxColor WelcomeTextColor { get { return COLOR_BLUE; } }

			// SignIn screen
			public MvxColor LoginButtonColor { get { return COLOR_GREEN; } }
			public MvxColor LoginPanelBackgroundColor { get { return new MvxColor(0, 100, 190, 200); } }
			public MvxColor LoginWelcomeColor { get { return COLOR_WHITE; } }

			public MvxColor LoginTextBoxColor { get { return COLOR_WHITE; } }
            public MvxColor LoginTextBoxBackgroundColor { get { return new MvxColor(56, 135, 206, 150); } }

			// Training Category
			public MvxColor TrainingCategoryItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor TrainingCategoryItemColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingCategoryItemSelectedColor { get { return new MvxColor(0, 156, 254); } }
			public MvxColor TrainingCategoryItemSelectedTextColor { get { return COLOR_WHITE; } }

			public MvxColor ActivityTypesPanelColor { get { return COLOR_GRAY; } }

			// Training item
			public MvxColor TrainingItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor TrainingItemColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingItemSelectedColor { get { return COLOR_BLUE; } }
			public MvxColor TrainingItemSelectedTextColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingItemPointsColor { get { return COLOR_BLUE; } }
			public MvxColor TrainingItemPointsSelectedColor { get { return COLOR_WHITE; } }
			public MvxColor ClockBackGroundColor { get { return COLOR_LIGHTBLUE; } }
			public MvxColor ClockSelectedBackGroundColor { get { return COLOR_BLUE; } }

			// Activity item
			public MvxColor ActivityItemBackgroundColor { get { return COLOR_WHITE; } }

			public MvxColor ActivityItemPanelTextColor { get { return COLOR_DARKBLUE; } }
			public MvxColor ActivityItemPanelColor { get { return COLOR_GRAY; } }

			public MvxColor ActivityItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityItemBoxesColor { get { return COLOR_GRAY; } }

			public MvxColor ActivitySaveButtonColor { get { return COLOR_DARKBLUE; } }
			public MvxColor ActivityCancelButtonColor { get { return COLOR_WHITE; } }
			public MvxColor ActivitySaveButtonPressedColor { get { return COLOR_BLUE; } }
			public MvxColor ActivityCancelButtonTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityResumeButtonColor { get { return COLOR_BLUE; } }
			public MvxColor ActivityResumeButtonTextColor { get { return COLOR_WHITE; } }
			public MvxColor ImageSelectBackgroundColor { get { return COLOR_BLUE; } }
			public MvxColor ImageBackgroundColor { get { return COLOR_GRAY; } }

			// News feed
			public MvxColor NewsFeedListBackgroundColor { get { return COLOR_GRAY; } }

			// News feed item
			public MvxColor FeedItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor FeedItemColor { get { return COLOR_WHITE; } }
			public MvxColor FeedItemSelectedColor { get { return COLOR_BLUE; } }
			public MvxColor FeedItemSelectedTextColor { get { return COLOR_WHITE; } }
			public MvxColor FeedItemSeparatorColor { get { return COLOR_BLUE; } }
			public MvxColor FeedItemOddSeparatorColor { get { return COLOR_DARKGRAY; } }

			// Feed detail page
			public MvxColor FeedDetailTextTitleColor { get { return COLOR_BLACK; } }
			public MvxColor FeedDetailTextColor { get { return COLOR_BLACK; } }

			public MvxColor FeedDetailButtonColor { get { return COLOR_BLUE; } }
			public MvxColor FeedDetailButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor FeedDetailCloseButtonColor { get { return COLOR_WHITE; } }
			public MvxColor FeedDetailCloseButtonTextColor { get { return COLOR_BLUE; } }

			public MvxColor FeedDetailBackgroundColor { get { return COLOR_WHITE; } }

			// Busy
			public MvxColor BusyBackgroundColor { get { return COLOR_LIGHTGRAY; } }

			public MvxColor ButtonColor { get { return COLOR_BLUE; } }
			public MvxColor ButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor SelectedButtonColor { get { return COLOR_DARKBLUE; } }
			public MvxColor SelectedButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor LoadingIndicatorColor { get { return COLOR_DARKBLUE; } }

			public MvxColor SplashColor { get { return COLOR_LIGHTBLACK; } }
		}

		public class AteaThemeThemImages : IThemeImages
		{
			public string Logo { get { return @"atea/atealogo.png"; } }

			public string FirstSlide { get { return @"atea/slide1.png"; } }
			public string SecondSlide { get { return @"atea/slide2.png"; } }
			public string ThirdSlide { get { return @"atea/slide3.png"; } }

			public string LoginBackground { get { return @"atea/loginBackground.png"; } }
			public string LoginLogo { get { return @"atea/atealogo.png"; } }

			public string Close { get { return @"atea/ic_close.png"; } }

			public string HeaderLogo { get { return @"atea/atealogo2.png"; } }
			public string HeaderRegister { get { return @"atea/ic_plus.png"; } }
			public string HeaderRegisterFavorit { get { return @"atea/ic_star.png"; } }
			public string HeaderGoToWeb { get { return @"atea/bar-chart.png"; } }

			public string TrainingCategoriesPath { get { return @"atea/TraningCategories"; } }

			public string Face { get { return @"atea/face.png"; } }
			public string CircleFace { get { return @"atea/circleFace.png"; } }
		}
	}
}
