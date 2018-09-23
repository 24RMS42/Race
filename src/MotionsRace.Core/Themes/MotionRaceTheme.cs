using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Themes.Base;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core.Themes
{
	public class MotionRaceTheme : ITheme
	{
		public string Name { get { return "MotionsRace"; } }
		public string SignUpURL { get { return "http://app.motionsrace.com"; } }
		public string ForgotPasswordURL { get { return "http://app.motionsrace.com/forgotpassword.aspx"; } }

		public IThemeColors Colors { get; set; }
		public IThemeImages Images { get; set; }

		public MotionRaceTheme()
		{
            Colors = new MotionRaceColors();
            Images = new MotionRaceImages();
		}

        public class MotionRaceColors : IThemeColors
		{
			public static readonly MvxColor COLOR_ORANGE = new MvxColor(255, 154, 0);
			public static readonly MvxColor COLOR_WHITE = new MvxColor(255, 255, 255);
			public static readonly MvxColor COLOR_BLACK = new MvxColor(0, 0, 0);
			public static readonly MvxColor COLOR_LIGHTGRAY = new MvxColor(240, 235, 240);
			public static readonly MvxColor COLOR_GRAY = new MvxColor(217, 218, 220);
			public static readonly MvxColor COLOR_DARKGRAY = new MvxColor(90, 90, 90);
            public static readonly MvxColor COLOR_DARKGRAY_TRANSPARENT = new MvxColor(120, 120, 120, 150);
			public static readonly MvxColor COLOR_BLUE = new MvxColor(0, 143, 197);
			public static readonly MvxColor COLOR_DARKBLUE = new MvxColor(22, 62, 85);

			public MvxColor MainProgressBarForegroundColor { get { return new MvxColor(55, 158, 255); } }
			public MvxColor MainProgressBarBackgroundColor { get { return new MvxColor(179, 179, 179); } }

			public MvxColor MainColor { get { return COLOR_WHITE; } }
			public MvxColor TextColor { get { return COLOR_BLACK; } }

			public MvxColor HeaderBottomBorderColor { get { return new MvxColor(0, 0, 0, 0); } }
			public MvxColor HeaderColor { get { return new MvxColor(0, 103, 139); } }
			public MvxColor TextHeaderColor { get { return COLOR_WHITE; } }

			public MvxColor PageTitleColor { get { return COLOR_WHITE; } }
			public MvxColor TextPageTitleColor { get { return COLOR_BLACK; } }
			public MvxColor TextPageSecondaryTitleColor { get { return COLOR_BLACK; } }

			//Splash screen
			public MvxColor WelcomeSignUpColor { get { return COLOR_BLUE; } }
			public MvxColor WelcomeSignInColor { get { return COLOR_WHITE; } }
			public MvxColor WelcomeSignUpTextColor { get { return COLOR_WHITE; } }
			public MvxColor WelcomeSignInTextColor { get { return COLOR_BLUE; } }
			public MvxColor WelcomeSliderIndicatorNotSelectorColor { get { return new MvxColor(0, 0, 0, 125); } }
			public MvxColor WelcomeSliderIndicatorSelectorColor { get { return new MvxColor(55, 158, 255); } }

			public MvxColor WelcomeTextColor { get { return COLOR_WHITE; } }

			// SignIn screen
			public MvxColor LoginButtonColor { get { return COLOR_DARKBLUE; } }
			public MvxColor LoginPanelBackgroundColor { get { return new MvxColor(255, 255, 255, 125); } } // Black, 50% transparent
			public MvxColor LoginWelcomeColor { get { return COLOR_DARKBLUE; } }

			public MvxColor LoginTextBoxColor { get { return COLOR_WHITE; } }
			public MvxColor LoginTextBoxBackgroundColor { get { return COLOR_DARKGRAY_TRANSPARENT; } }


			// Training Category
			public MvxColor TrainingCategoryItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor TrainingCategoryItemColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingCategoryItemSelectedColor { get { return COLOR_ORANGE; } }
			public MvxColor TrainingCategoryItemSelectedTextColor { get { return COLOR_WHITE; } }

			public MvxColor ActivityTypesPanelColor { get { return COLOR_GRAY; } }

			// Training item
			public MvxColor TrainingItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor TrainingItemColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingItemSelectedColor { get { return COLOR_ORANGE; } }
			public MvxColor TrainingItemSelectedTextColor { get { return COLOR_WHITE; } }
			public MvxColor TrainingItemPointsColor { get { return COLOR_ORANGE; } }
			public MvxColor TrainingItemPointsSelectedColor { get { return COLOR_WHITE; } }
			public MvxColor ClockBackGroundColor { get { return COLOR_BLUE; } }
			public MvxColor ClockSelectedBackGroundColor { get { return COLOR_ORANGE; } }

			// Activity item
			public MvxColor ActivityItemBackgroundColor { get { return COLOR_WHITE; } }

			public MvxColor ActivityItemPanelTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityItemPanelColor { get { return COLOR_GRAY; } }
			public MvxColor ActivityItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityItemBoxesColor { get { return COLOR_GRAY; } }
			public MvxColor ActivitySaveButtonColor { get { return COLOR_DARKBLUE; } }
			public MvxColor ActivitySaveButtonPressedColor { get { return COLOR_ORANGE; } }

			public MvxColor ActivityCancelButtonColor { get { return COLOR_WHITE; } }
			public MvxColor ActivityCancelButtonTextColor { get { return COLOR_BLACK; } }
			public MvxColor ActivityResumeButtonColor { get { return COLOR_ORANGE; } }
			public MvxColor ActivityResumeButtonTextColor { get { return COLOR_WHITE; } }
			public MvxColor ImageSelectBackgroundColor { get { return COLOR_ORANGE; } }
			public MvxColor ImageBackgroundColor { get { return COLOR_GRAY; } }

			// News feed
			public MvxColor NewsFeedListBackgroundColor { get { return COLOR_GRAY; } }

			// News feed item
			public MvxColor FeedItemTextColor { get { return COLOR_BLACK; } }
			public MvxColor FeedItemColor { get { return COLOR_WHITE; } }
			public MvxColor FeedItemSelectedColor { get { return COLOR_ORANGE; } }
			public MvxColor FeedItemSelectedTextColor { get { return COLOR_WHITE; } }
			public MvxColor FeedItemSeparatorColor { get { return COLOR_ORANGE; } }
			public MvxColor FeedItemOddSeparatorColor { get { return COLOR_BLUE; } }

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

			public MvxColor ButtonColor { get { return COLOR_DARKBLUE; } }
			public MvxColor ButtonTextColor { get { return COLOR_WHITE; } }
			public MvxColor SelectedButtonColor { get { return COLOR_ORANGE; } }
			public MvxColor SelectedButtonTextColor { get { return COLOR_WHITE; } }

			public MvxColor LoadingIndicatorColor { get { return COLOR_DARKBLUE; } }

			public MvxColor SplashColor { get { return COLOR_BLUE; } }
		}

        public class MotionRaceImages : IThemeImages
		{
			public string Logo { get { return @"motionrace/motionlogo.png"; } }

			public string FirstSlide { get { return @"motionrace/slide1.png"; } }
			public string SecondSlide { get { return @"motionrace/slide2.png"; } }
			public string ThirdSlide { get { return @"motionrace/slide3.png"; } }

			public string LoginBackground { get { return @"motionrace/loginBackground.png"; } }
			public string LoginLogo { get { return @"motionrace/motionlogo.png"; } }

			public string Close { get { return @"motionrace/ic_close.png"; } }

			public string HeaderLogo { get { return @"motionrace/motionlogo2.png"; } }
			public string HeaderRegister { get { return @"motionrace/ic_plus.png"; } }
	        public string HeaderRegisterFavorit { get { return @"motionrace/ic_star.png"; }
		}
	        public string HeaderGoToWeb { get { return @"motionrace/ic_next.png"; } }

			public string TrainingCategoriesPath { get { return @"motionrace/TraningCategories"; } }

			public string Face { get { return @"motionrace/face.png"; } }
			public string CircleFace { get { return @"motionrace/circleFace.png"; } }
		}
	}
}
