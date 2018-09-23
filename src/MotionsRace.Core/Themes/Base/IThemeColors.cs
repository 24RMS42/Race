
using MvvmCross.Platform.UI;

namespace MotionsRace.Core.Themes.Base
{
	public interface IThemeColors
	{
		// Main
		MvxColor MainColor { get; } // ACTIVITY_ITEM_PANELS_BACKGROUND
		MvxColor TextColor { get; }
		MvxColor HeaderBottomBorderColor { get; }
		MvxColor HeaderColor { get; } // GLOBAL_HEADER_COLOR
		MvxColor TextHeaderColor { get; }
		MvxColor PageTitleColor { get; } // MAIN_NEWS_FEED_ITEM_BACKGROUND_COLOR, TITLE_BACKGROUND_COLOR
		MvxColor TextPageTitleColor { get; } // MAIN_FEED_TITLE_FONT_COLOR, TITLE_FOREGROUND_COLOR
		MvxColor TextPageSecondaryTitleColor { get; } // [MAIN_FEED_TITLE_FONT_COLOR
		MvxColor MainProgressBarForegroundColor { get; }
		MvxColor MainProgressBarBackgroundColor { get; }

		// Welcome screen
		MvxColor WelcomeSignUpColor { get; } // WELCOME_SIGN_UP_BACKGROUND_COLOR
		MvxColor WelcomeSignInColor { get; } // WELCOME_SIGN_IN_BACKGROUND_COLOR
		MvxColor WelcomeSignUpTextColor { get; } // WELCOME_SIGN_UP_FOREGROUND_COLOR
		MvxColor WelcomeSignInTextColor { get; } // WELCOME_SIGN_IN_FOREGROUND_COLOR
		MvxColor WelcomeSliderIndicatorNotSelectorColor { get; } // WELCOME_SLIDER_INDICATOR_NOT_SELECTED_COLOR
		MvxColor WelcomeSliderIndicatorSelectorColor { get; } // WELCOME_SLIDER_INDICATOR_SELECTED_COLOR

		MvxColor WelcomeTextColor { get; } // WELCOME_SIGN_IN_FOREGROUND_COLOR

		// SignIn screen
		MvxColor LoginButtonColor { get; }
		MvxColor LoginPanelBackgroundColor { get; } // LOGIN_PANEL_BACKGROUND_COLOR
		MvxColor LoginWelcomeColor { get; } // LOGIN_WELCOME_FOREGROUND_COLOR
		MvxColor LoginTextBoxColor { get; } // LOGIN_TEXTBOX_BACKGROUND_COLOR
		MvxColor LoginTextBoxBackgroundColor { get; } // LOGIN_TEXTBOX_FOREGROUND_COLOR

		// Training Category
		MvxColor TrainingCategoryItemTextColor { get; }
		MvxColor TrainingCategoryItemColor { get; }
		MvxColor TrainingCategoryItemSelectedColor { get; }
		MvxColor TrainingCategoryItemSelectedTextColor { get; }

		MvxColor ActivityTypesPanelColor { get; } // ACTIVITY_TYPES_PANELS_BACKGROUND

		// Training item
		MvxColor TrainingItemTextColor { get; }
		MvxColor TrainingItemColor { get; }
		MvxColor TrainingItemSelectedColor { get; }
		MvxColor TrainingItemSelectedTextColor { get; }
		MvxColor TrainingItemPointsColor { get; }
		MvxColor TrainingItemPointsSelectedColor { get; }
		MvxColor ClockBackGroundColor { get; }
		MvxColor ClockSelectedBackGroundColor { get; }

		// Activity item
		MvxColor ActivityItemBackgroundColor { get; } // PAGES_BACKGROUND

		MvxColor ActivityItemPanelTextColor { get; } // ACTIVITY_ITEM_PANELS_TEXT_COLOR
		MvxColor ActivityItemPanelColor { get; } // ACTIVITY_ITEM_PANELS_BACKGROUND
		MvxColor ActivityItemTextColor { get; } // ACTIVITY_ITEM_TEXT_COLOR
		MvxColor ActivityItemBoxesColor { get; } // ACTIVITY_ITEM_BOXES_BACKGROUND

		MvxColor ActivitySaveButtonColor { get; } // ACTIVITY_ITEM_SAVE_BUTTON_COLOR
		MvxColor ActivitySaveButtonPressedColor { get; }

		MvxColor ActivityCancelButtonColor { get; } // ACTIVITY_ITEM_CANCEL_BUTTON_COLOR
		MvxColor ActivityCancelButtonTextColor { get; }
		MvxColor ActivityResumeButtonColor { get; }
		MvxColor ActivityResumeButtonTextColor { get; }

		MvxColor ImageSelectBackgroundColor { get; }
		MvxColor ImageBackgroundColor { get; }

		// News feed
		MvxColor NewsFeedListBackgroundColor { get; } // MAIN_PAGE_BACKGROUND_COLOR

		// News feed item
		MvxColor FeedItemTextColor { get; }
		MvxColor FeedItemColor { get; }
		MvxColor FeedItemSelectedColor { get; }
		MvxColor FeedItemSelectedTextColor { get; }
		MvxColor FeedItemSeparatorColor { get; }
		MvxColor FeedItemOddSeparatorColor { get; }

		// Detail feed info
		MvxColor FeedDetailTextTitleColor { get; } //MAIN_FEED_DATAIL_TITLE_FONT_COLOR
		MvxColor FeedDetailTextColor { get; } // MAIN_FEED_DATAIL_FULL_TEXT_FONT_COLOR
		MvxColor FeedDetailButtonColor { get; } //MAIN_FEED_DATAIL_READ_MORE_BUTTON_BACKGROUND_COLOR
		MvxColor FeedDetailButtonTextColor { get; } // MAIN_FEED_DATAIL_READ_MORE_BUTTON_FOREGROUND_COLOR
		MvxColor FeedDetailBackgroundColor { get; } // MAIN_NEWS_FEED_ITEM_BACKGROUND_COLOR
		MvxColor FeedDetailCloseButtonTextColor { get; }
		MvxColor FeedDetailCloseButtonColor { get; }

		// Busy
		MvxColor BusyBackgroundColor { get; }

		// Buttons
		MvxColor ButtonColor { get; }
		MvxColor SelectedButtonColor { get; }
		MvxColor ButtonTextColor { get; }
		MvxColor SelectedButtonTextColor { get; }

		MvxColor LoadingIndicatorColor { get; }

		MvxColor SplashColor { get; }
	}

}
