﻿using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Themes.Base;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core.Themes
{
	public class NetEntTheme : ITheme
	{
		public string Name { get { return "Health and Energy Challenge"; } }
		public string SignUpURL { get { return "http://netent.motionsrace.com/login.aspx"; } }
		public string ForgotPasswordURL { get { return "http://netent.motionsrace.com/forgotpassword.aspx"; } }

		public IThemeColors Colors { get; set; }
		public IThemeImages Images { get; set; }

		public NetEntTheme()
		{
			Colors = new NetEntThemColors();
			Images = new NetEntThemImages();
		}

		public class NetEntThemColors : IThemeColors
		{
			public static readonly MvxColor COLOR_BLACK = new MvxColor(0, 0, 0);
			public static readonly MvxColor COLOR_WHITE = new MvxColor(255, 255, 255);
			public static readonly MvxColor COLOR_LIGHTBLACK = new MvxColor(51,51,51);
			public static readonly MvxColor COLOR_GREEN = new MvxColor(120, 190, 32);
			public static readonly MvxColor COLOR_DARKGREEN = new MvxColor(75, 151, 75);
			public static readonly MvxColor COLOR_GRAY = new MvxColor(217, 218, 220);
			public static readonly MvxColor COLOR_LIGHTGRAY = new MvxColor(32, 32, 32);
			public static readonly MvxColor COLOR_DARKGRAY = new MvxColor(90, 90, 90);

			// Main
			public MvxColor MainProgressBarForegroundColor { get { return COLOR_DARKGREEN; } }
			public MvxColor MainProgressBarBackgroundColor { get { return new MvxColor(179, 179, 179); } }

			public MvxColor MainColor { get { return COLOR_GRAY; } }
			public MvxColor TextColor { get { return COLOR_BLACK; } }

			public MvxColor HeaderBottomBorderColor { get { return new MvxColor(0, 0, 0, 0); } }
			public MvxColor HeaderColor { get { return COLOR_WHITE; } }
			public MvxColor TextHeaderColor { get { return COLOR_DARKGREEN; } }

			public MvxColor PageTitleColor { get { return COLOR_WHITE; } }
			public MvxColor TextPageTitleColor { get { return COLOR_DARKGREEN; } }
			public MvxColor TextPageSecondaryTitleColor { get { return COLOR_BLACK; } }

			//Splash screen
			public MvxColor WelcomeSignUpColor { get { return COLOR_GREEN; } }
			public MvxColor WelcomeSignInColor { get { return COLOR_WHITE; } }
			public MvxColor WelcomeSignUpTextColor { get { return COLOR_WHITE; } }
			public MvxColor WelcomeSignInTextColor { get { return COLOR_GREEN; } }
			public MvxColor WelcomeSliderIndicatorNotSelectorColor { get { return new MvxColor(0, 0, 0, 125); } }
			public MvxColor WelcomeSliderIndicatorSelectorColor { get { return COLOR_GREEN; } }

			public MvxColor WelcomeTextColor { get { return COLOR_GREEN; } }

			// SignIn screen
			public MvxColor LoginButtonColor { get { return COLOR_GREEN; } }
			public MvxColor LoginPanelBackgroundColor { get { return new MvxColor(90, 90, 90, 100); } }
			public MvxColor LoginWelcomeColor { get { return COLOR_WHITE; } }

			public MvxColor LoginTextBoxColor { get { return COLOR_WHITE; } }
            public MvxColor LoginTextBoxBackgroundColor { get { return new MvxColor(150, 150, 150, 150); } }

			// Training Category
			public MvxColor TrainingCategoryItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor TrainingCategoryItemColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingCategoryItemSelectedColor { get { return COLOR_GREEN; } }
			public MvxColor TrainingCategoryItemSelectedTextColor { get { return COLOR_WHITE; } }

			public MvxColor ActivityTypesPanelColor { get { return COLOR_GRAY; } }

			// Training item
			public MvxColor TrainingItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor TrainingItemColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingItemSelectedColor { get { return COLOR_GREEN; } }
			public MvxColor TrainingItemSelectedTextColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingItemPointsColor { get { return COLOR_GREEN; } }
			public MvxColor TrainingItemPointsSelectedColor { get { return COLOR_WHITE; } }
			public MvxColor ClockBackGroundColor { get { return COLOR_DARKGRAY; } }
			public MvxColor ClockSelectedBackGroundColor { get { return COLOR_GREEN; } }

			// Activity item
			public MvxColor ActivityItemBackgroundColor { get { return COLOR_WHITE; } }

			public MvxColor ActivityItemPanelTextColor { get { return COLOR_WHITE; } }
			public MvxColor ActivityItemPanelColor { get { return COLOR_DARKGRAY; } }

			public MvxColor ActivityItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityItemBoxesColor { get { return COLOR_GRAY; } }

			public MvxColor ActivitySaveButtonColor { get { return COLOR_DARKGREEN; } }
			public MvxColor ActivityCancelButtonColor { get { return COLOR_WHITE; } }
			public MvxColor ActivitySaveButtonPressedColor { get { return COLOR_GREEN; } }
			public MvxColor ActivityCancelButtonTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityResumeButtonColor { get { return COLOR_GREEN; } }
			public MvxColor ActivityResumeButtonTextColor { get { return COLOR_WHITE; } }
			public MvxColor ImageSelectBackgroundColor { get { return COLOR_GREEN; } }
			public MvxColor ImageBackgroundColor { get { return COLOR_GRAY; } }

			// News feed
			public MvxColor NewsFeedListBackgroundColor { get { return COLOR_GRAY; } }

			// News feed item
			public MvxColor FeedItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor FeedItemColor { get { return COLOR_WHITE; } }
			public MvxColor FeedItemSelectedColor { get { return COLOR_GREEN; } }
			public MvxColor FeedItemSelectedTextColor { get { return COLOR_WHITE; } }
			public MvxColor FeedItemSeparatorColor { get { return COLOR_GREEN; } }
			public MvxColor FeedItemOddSeparatorColor { get { return COLOR_LIGHTBLACK; } }

			// Feed detail page
			public MvxColor FeedDetailTextTitleColor { get { return COLOR_BLACK; } }
			public MvxColor FeedDetailTextColor { get { return COLOR_BLACK; } }

			public MvxColor FeedDetailButtonColor { get { return COLOR_GREEN; } }
			public MvxColor FeedDetailButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor FeedDetailCloseButtonColor { get { return COLOR_WHITE; } }
			public MvxColor FeedDetailCloseButtonTextColor { get { return COLOR_GREEN; } }

			public MvxColor FeedDetailBackgroundColor { get { return COLOR_WHITE; } }

			// Busy
			public MvxColor BusyBackgroundColor { get { return COLOR_LIGHTGRAY; } }

			public MvxColor ButtonColor { get { return COLOR_GREEN; } }
			public MvxColor ButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor SelectedButtonColor { get { return COLOR_DARKGREEN; } }
			public MvxColor SelectedButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor LoadingIndicatorColor { get { return COLOR_DARKGREEN; } }

			public MvxColor SplashColor { get { return COLOR_LIGHTBLACK; } }
		}

		public class NetEntThemImages : IThemeImages
		{
			public string Logo { get { return @"netent/twitchlogo.png"; } }

			public string FirstSlide { get { return @"netent/slide1.png"; } }
			public string SecondSlide { get { return @"netent/slide2.png"; } }
			public string ThirdSlide { get { return @"netent/slide3.png"; } }

			public string LoginBackground { get { return @"netent/loginBackground.png"; } }
			public string LoginLogo { get { return @"netent/loginlogo.png"; } }

			public string HeaderLogo { get { return @"netent/headerlogo.png"; } }
			public string HeaderRegister { get { return @"netent/ic_plus.png"; } }
			public string HeaderRegisterFavorit { get { return @"netent/ic_star.png"; } }
			public string HeaderGoToWeb { get { return @"netent/ic_next.png"; } }

			public string Close { get { return @"netent/ic_close.png"; } }

			public string TrainingCategoriesPath { get { return @"netent/TraningCategories"; } }

			public string Face { get { return @"netent/face.png"; } }
			public string CircleFace { get { return @"netent/circleFace.png"; } }
		}
	}
}
