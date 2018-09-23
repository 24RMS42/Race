using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using System;
using MotionsRace.Core.Models;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Plugins.Color.iOS;
using MvvmCross.Platform;

namespace MotionsRace.Touch.Views
{
    [Register("MainView")]
    public class MainView : MvxViewController<MainViewModel>
    {
		public static UIImage RotatedImage { get; private set;} 

		private nfloat _navBarHeight;
		private UIView _headerBorder;

		private UILabel _raceLabel;
		private UILabel _userLabel;
		private UILabel _scoreLabel;
		private UILabel _dateEndsLabel;

		private UIBarButtonItem _liveRecordButton;
		private UIBarButtonItem _liveRecordFavButton;
		private UIBarButtonItem _loginButton;

		private MvxImageView _avatarPlaceholderImageView;
		private MvxImageView _avatarImageView;
		private UIProgressView _progressView;
		private UIButton _btnRegister;
		private UIButton _btnRegisterFav;

		private UIScrollView _detailsScrollView = new UIScrollView();
		private UIView _detailsView;
		private UILabel _detailsTimeLabel = new UILabel();
		private UILabel _detailsTextLabel = new UILabel();
		private UITextView _detailsMessageLabel = new UITextView();
		private UIButton _detailsReadMoreButton = new UIButton();
		private UIButton _detailsHideButton = new UIButton();

		private UIView _crashView = new UIView();
		private UIButton _btnRefresh = new UIButton();
		private UIButton _btnRestart = new UIButton();

		private MvxImageView _detailsImage = new MvxImageView();

		private UIView _myActivitiesView;
		private UISwitch _isNewsFeedPersonalChk = new UISwitch();
		private UIButton _btnCloseMyActivitiesPanel = new UIButton();

		private UIView _userView;
		private UITableView _tableNews;
		private NewsSource _source;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			_navBarHeight = this.NavigationController.NavigationBar.Bounds.Height;

			var backgroundColor = ViewModel.Theme.Colors.NewsFeedListBackgroundColor.ToNativeColor ();
			View = new UIView { BackgroundColor = backgroundColor };

			this.Title = "";

			ViewModel.PropertyChanged += (sender, e) => 
			{
				if (e.PropertyName == "ShowLiveRecordButton")
				{
					InvokeOnMainThread ( () => {
						_liveRecordButton.TintColor = ViewModel.ShowLiveRecordButton ? ViewModel.Theme.Colors.TextHeaderColor.ToNativeColor() : UIColor.Clear;
					});
				}
				else if (e.PropertyName == "ShowFavoritButton")
				{
					InvokeOnMainThread ( () => {
						_liveRecordFavButton.TintColor = ViewModel.ShowFavoritButton ? ViewModel.Theme.Colors.TextHeaderColor.ToNativeColor() : UIColor.Clear;
					});
				}
				else if (e.PropertyName == "ShowLoginButton")
				{
					_loginButton.TintColor = ViewModel.ShowLoginButton ? ViewModel.Theme.Colors.TextHeaderColor.ToNativeColor() : UIColor.Clear;
				}
			};



#if NETENT || KRONOBERG
			var headerTopView = new UIView(new CGRect(0, 0, 80, 40));
#else
			// header top logo
			var headerTopView = new UIView(new CGRect (0, 0, 40, 40));
#endif

			var headerImageView = new MvxImageView(new CGRect(0, 0, 40, 40));
#if TWITCH
			headerImageView = new MvxImageView(new CGRect(0, 0, 40, 40));
#elif NORDEN
			headerImageView = new MvxImageView(new CGRect(-20, -20, 70, 70));
#elif COROMATIC
			headerImageView = new MvxImageView(new CGRect(-10, -10, 50, 50));
#elif NETENT || KRONOBERG
			headerImageView = new MvxImageView(new CGRect(0, 0, 80, 40));
#endif
			headerImageView.Image = UIImage.FromFile("SmallLogo.scale-240.png");
			headerImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			headerTopView.AddSubview(headerImageView);

			var btnHeaderLogo = UIButton.FromType(UIButtonType.Custom);
#if NETENT || KRONOBERG
			btnHeaderLogo.Frame = new CGRect(0, 0, 80, 40);
#else
			btnHeaderLogo.Frame = new CGRect(0, 0, 40, 40);
#endif
			btnHeaderLogo.ContentMode = UIViewContentMode.ScaleAspectFill;
			btnHeaderLogo.AutoresizingMask = UIViewAutoresizing.FlexibleHeight|UIViewAutoresizing.FlexibleWidth;
			headerTopView.AddSubview (btnHeaderLogo);

			this.NavigationItem.TitleView = headerTopView;
			// -----------

			// header panel
			_userView = new UIView (new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width , 90));
			_userView.BackgroundColor = ViewModel.Theme.Colors.PageTitleColor.ToNativeColor ();
			View.AddSubview(_userView);

			// avatar placeholder
			_avatarPlaceholderImageView = new MvxImageView(new CGRect(10, 8, 60, 60));
			_avatarPlaceholderImageView.Image = UIImage.FromFile("no_profile_96.png");
			_avatarPlaceholderImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			_userView.AddSubview (_avatarPlaceholderImageView);

			_avatarImageView = new MvxImageView (new CGRect (10, 8, 60, 60));
			_userView.AddSubview (_avatarImageView);

			// avatar border
			var avatarBorderImageView = new MvxImageView(new CGRect (10, 8, 60, 60));
			avatarBorderImageView.Image = UIImage.FromFile("faceborder.png");
			avatarBorderImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
			_userView.AddSubview (avatarBorderImageView);

			// race title
			_raceLabel = new UILabel(new CGRect(75, 6, UIScreen.MainScreen.Bounds.Width, 16));
			_raceLabel.TextColor =  ViewModel.Theme.Colors.TextPageTitleColor.ToNativeColor ();
			_raceLabel.TextAlignment = UITextAlignment.Left;
			_raceLabel.Font = UIFont.SystemFontOfSize (14);
			_userView.AddSubview(_raceLabel);

			// first last name
			_userLabel = new UILabel(new CGRect(75, 22, UIScreen.MainScreen.Bounds.Width, 16));
			_userLabel.TextColor =  ViewModel.Theme.Colors.TextPageTitleColor.ToNativeColor ();
			_userLabel.TextAlignment = UITextAlignment.Left;
			_userLabel.Font = UIFont.SystemFontOfSize (14);
			_userView.AddSubview(_userLabel);

			var btnMenu = new UIButton(UIButtonType.RoundedRect);
			btnMenu.Frame = new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width , 90);
			Add(btnMenu);

			// points
			_scoreLabel = new UILabel(new CGRect(75, 38, UIScreen.MainScreen.Bounds.Width, 16));
			_scoreLabel.TextColor = ViewModel.Theme.Colors.TextPageSecondaryTitleColor.ToNativeColor ().ColorWithAlpha(.7f);
			_scoreLabel.TextAlignment = UITextAlignment.Left;
			_scoreLabel.Font = UIFont.SystemFontOfSize (14);
			_userView.AddSubview(_scoreLabel);

			// points
			_dateEndsLabel = new UILabel(new CGRect(75, 54, UIScreen.MainScreen.Bounds.Width, 16));
			_dateEndsLabel.TextColor =  ViewModel.Theme.Colors.TextPageSecondaryTitleColor.ToNativeColor ().ColorWithAlpha(.7f);
			_dateEndsLabel.TextAlignment = UITextAlignment.Left;
			_dateEndsLabel.Font = UIFont.SystemFontOfSize (14);
			_userView.AddSubview(_dateEndsLabel);



			_progressView = new UIProgressView (new CGRect (10, 75, UIScreen.MainScreen.Bounds.Width - 20, 8));
			_progressView.Style = UIProgressViewStyle.Bar;
			_progressView.Progress = 0.7f;
			_progressView.Layer.Frame = new CGRect (10, 75, UIScreen.MainScreen.Bounds.Width - 20, 8);
			_progressView.Layer.CornerRadius = 3;
			_progressView.Layer.MasksToBounds = true;
			_progressView.ProgressTintColor = ViewModel.Theme.Colors.MainProgressBarForegroundColor.ToNativeColor();
			_progressView.TrackTintColor = ViewModel.Theme.Colors.MainProgressBarBackgroundColor.ToNativeColor();
			_userView.AddSubview(_progressView);

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
               EdgesForExtendedLayout = UIRectEdge.None;
            }

			/*
			_liveRecordButton = new UIBarButtonItem (UIImage.FromFile("ic_plus.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
				ViewModel.LiveRecordCommand.Execute ();
			});

			this.NavigationItem.SetLeftBarButtonItem(_liveRecordButton, true);

			_loginButton = new UIBarButtonItem (UIImage.FromFile("ic_next.png"), UIBarButtonItemStyle.Plain, (sender, args) => {
				ViewModel.LoginCommand.Execute ();
			});
			*/

			_liveRecordButton = new UIBarButtonItem (UIBarButtonSystemItem.Add, (sender, args) => {
				ViewModel.LiveRecordCommand.Execute ();
			});
			_liveRecordButton.TintColor = UIColor.Clear;

			_liveRecordFavButton = new UIBarButtonItem (UIBarButtonSystemItem.Bookmarks, (sender, args) => {
				ViewModel.FavoritRecordCommand.Execute ();
			});
			_liveRecordFavButton.TintColor = UIColor.Clear;

			this.NavigationItem.SetLeftBarButtonItems(new [] {_liveRecordButton, _liveRecordFavButton }, true);

			_loginButton = new UIBarButtonItem (UIImage.FromFile("bar-chart.png"), UIBarButtonItemStyle.Done, (sender, args) => {
				ViewModel.LoginCommand.Execute ();
			});
			_loginButton.TintColor = UIColor.Clear;
            //_loginButton.Image = UIImage.FromFile("scoreicon-3.png");

			this.NavigationItem.SetRightBarButtonItem(_loginButton, true);

			// register activity button
			_btnRegister = new UIButton(UIButtonType.RoundedRect);
			_btnRegister.Frame = new CGRect(0, 
				UIScreen.MainScreen.ApplicationFrame.Height - _navBarHeight - 50, 
				ViewModel.ShowFavoritButton ? UIScreen.MainScreen.Bounds.Width / 2 : UIScreen.MainScreen.Bounds.Width, 
				50);
			
			_btnRegister.SetTitle (ViewModel["Main_Register"], UIControlState.Normal);
			_btnRegister.SetTitleColor (ViewModel.Theme.Colors.ButtonTextColor.ToNativeColor (), UIControlState.Normal);
			_btnRegister.BackgroundColor = ViewModel.Theme.Colors.ButtonColor.ToNativeColor ();
			View.AddSubview(_btnRegister);

			// register activity fav button
			_btnRegisterFav = new UIButton(UIButtonType.RoundedRect);
			_btnRegisterFav.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width / 2 + 1, UIScreen.MainScreen.ApplicationFrame.Height - _navBarHeight - 50, UIScreen.MainScreen.Bounds.Width / 2, 50);
			_btnRegisterFav.SetTitle (ViewModel["Main_Favorit"], UIControlState.Normal);
			_btnRegisterFav.SetTitleColor (ViewModel.Theme.Colors.ButtonTextColor.ToNativeColor (), UIControlState.Normal);
			_btnRegisterFav.BackgroundColor = ViewModel.Theme.Colors.ButtonColor.ToNativeColor ();
			View.AddSubview(_btnRegisterFav);

			// details ------
			_detailsView = new UIView ();
			_detailsView.Frame = new CGRect(0, _userView.Frame.Bottom, UIScreen.MainScreen.Bounds.Width, _btnRegister.Frame.Bottom - _userView.Frame.Bottom);
			_detailsView.BackgroundColor = UIColor.White;
			View.AddSubview (_detailsView);

			_detailsScrollView.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, _btnRegister.Frame.Bottom - _userView.Frame.Bottom - 60);
			_detailsScrollView.BackgroundColor = UIColor.White;
			_detailsView.AddSubview (_detailsScrollView);
			//_detailsScrollView.UserInteractionEnabled = true;
			// ------

			// ------- Feeds -------
			_tableNews = new UITableView ();
			_tableNews.BackgroundColor = ViewModel.Theme.Colors.NewsFeedListBackgroundColor.ToNativeColor ();
			_tableNews.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			_tableNews.Frame = GetNewsTableFrame ();
			_tableNews.AllowsSelection = true;
			_tableNews.RowHeight = 60;
			//_tableNews.row

			var source = new NewsSource(_tableNews, this);
			_tableNews.Source = source;
			source.SelectedItemChanged  += (s, e) =>
			{
				InvokeOnMainThread(() => {

					ViewModel.ShowReadMorePageCommand.Execute(source.SelectedItem);

					if (ViewModel.SelectedFeedItem != null)
					{
						
						_detailsTimeLabel.Frame = new CGRect (10, 10, UIScreen.MainScreen.Bounds.Width - 20, 0);
						_detailsTimeLabel.Text = ViewModel.SelectedFeedItem.EventTime;
						_detailsTimeLabel.Font = UIFont.FromName("HelveticaNeue-Light", 16f);
						_detailsTimeLabel.TextColor = ViewModel.Theme.Colors.FeedDetailTextTitleColor.ToNativeColor();
						_detailsTimeLabel.SizeToFit ();
						_detailsScrollView.AddSubview(_detailsTimeLabel);

						var attrText = (NSMutableAttributedString)new NewsItemTextValueConverter().Convert(ViewModel.SelectedFeedItem.Text, typeof(NSMutableAttributedString), null, null);

						_detailsTextLabel.Frame = new CGRect (10, _detailsTimeLabel.Frame.Bottom + 10, UIScreen.MainScreen.Bounds.Width - 20, 0);
						_detailsTextLabel.AttributedText = attrText;
						_detailsTextLabel.Font = UIFont.FromName("HelveticaNeue-Light", 16f);
						//_detailsTextLabel.TextColor = ViewModel.Colors["MAIN_FEED_DATAIL_TITLE_FONT_COLOR"].ToNativeColor();
						_detailsTextLabel.LineBreakMode = UILineBreakMode.WordWrap;
						_detailsTextLabel.Lines = 0;
						_detailsTextLabel.SizeToFit ();
						_detailsScrollView.AddSubview(_detailsTextLabel);

						if (!string.IsNullOrWhiteSpace(ViewModel.SelectedFeedItem.FullImageURL))
						{
							var guestRecognizer = new UITapGestureRecognizer(() =>
								{
									try
									{
										if (_detailsImage.Image != null)
										{
											var imageSize = _detailsImage.Image.Size;
											if (imageSize.Width > imageSize.Height)
												RotatedImage = ImageUtils.ScaleAndRotateImage(_detailsImage.Image, UIImageOrientation.Right);
											else
												RotatedImage = null;

											ViewModel.ShowDetailFullFeedImage.Execute();
										}
									}
									catch (Exception ex)
									{
										var r = 5;
									}
								});

							_detailsImage.Frame = new CGRect(10, _detailsTextLabel.Frame.Bottom + 10, UIScreen.MainScreen.Bounds.Width - 20, 200);

							_detailsImage.InvokeOnMainThread(() =>
							{
								try
								{
									_detailsImage.ImageUrl = ViewModel.SelectedFeedItem.FullImageURL;
									_detailsImage.ContentMode = UIViewContentMode.ScaleAspectFit;
									_detailsImage.UserInteractionEnabled = true;
									_detailsImage.AddGestureRecognizer(guestRecognizer);
								}
								catch (Exception ex)
								{
									var r = 5;
								}
							});
						}
						else
						{
							_detailsImage.Frame = new CGRect(10, _detailsTextLabel.Frame.Bottom + 10, UIScreen.MainScreen.Bounds.Width - 20, 0);
						}
						_detailsScrollView.AddSubview(_detailsImage);


						_detailsMessageLabel.Frame = new CGRect (10, _detailsImage.Frame.Bottom + 10, UIScreen.MainScreen.Bounds.Width - 20, 20);
						if (!string.IsNullOrWhiteSpace(ViewModel.SelectedFeedItem.FullMessage))
							_detailsMessageLabel.AttributedText = GetAttributedStringFromHtml(ViewModel.SelectedFeedItem.FullMessage);
						else
							_detailsMessageLabel.AttributedText = GetAttributedStringFromHtml("");
						_detailsMessageLabel.Font = UIFont.FromName("HelveticaNeue-Light", 16f);
						_detailsMessageLabel.TextColor = ViewModel.Theme.Colors.FeedDetailTextTitleColor.ToNativeColor();
						_detailsMessageLabel.DataDetectorTypes = UIDataDetectorType.Link;
						_detailsMessageLabel.Editable = false;
						_detailsMessageLabel.ScrollEnabled = false;
						_detailsMessageLabel.SizeToFit ();


						_detailsScrollView.AddSubview(_detailsMessageLabel);

						_detailsScrollView.ContentSize = new CGSize(UIScreen.MainScreen.Bounds.Width, _detailsMessageLabel.Frame.Bottom);

						_detailsReadMoreButton.Frame = new CGRect(10, _detailsView.Frame.Height - 50, (UIScreen.MainScreen.Bounds.Width - 20) / 2 - 5, 40);
						_detailsReadMoreButton.SetTitle (ViewModel["Main_ReadOnSite"], UIControlState.Normal);
						_detailsReadMoreButton.SetTitleColor(ViewModel.Theme.Colors.FeedDetailButtonTextColor.ToNativeColor(), UIControlState.Normal);
						_detailsReadMoreButton.BackgroundColor = ViewModel.Theme.Colors.FeedDetailButtonColor.ToNativeColor ();
						_detailsView.AddSubview(_detailsReadMoreButton);

						_detailsHideButton.Frame = new CGRect(10 + _detailsReadMoreButton.Frame.Width + 10, _detailsView.Frame.Height - 50, (UIScreen.MainScreen.Bounds.Width - 20) / 2 - 5, 40);
						_detailsHideButton.SetTitle (ViewModel["Main_Close"], UIControlState.Normal);
						_detailsHideButton.SetTitleColor(ViewModel.Theme.Colors.FeedDetailCloseButtonTextColor.ToNativeColor(), UIControlState.Normal);
						_detailsHideButton.BackgroundColor = ViewModel.Theme.Colors.FeedDetailCloseButtonColor.ToNativeColor ();
						_detailsView.AddSubview(_detailsHideButton);
					}

					_tableNews.DeselectRow (_tableNews.IndexPathForSelectedRow, false);
				});
			};
			View.AddSubview (_tableNews);
			// ---------

			// --- crash panel
			_crashView.Frame = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.ApplicationFrame.Height - _navBarHeight);
			_crashView.BackgroundColor = UIColor.White;
			View.AddSubview(_crashView);

			var btnYPosition = _crashView.Frame.Height / 2 - 80;


			var serverErrorLabel = new UILabel();
			serverErrorLabel.Frame = new CGRect (20, btnYPosition, UIScreen.MainScreen.Bounds.Width - 40, 20);
			serverErrorLabel.Text = ViewModel["GLOBAL_Server_not_available"];
			serverErrorLabel.Font = UIFont.FromName("HelveticaNeue-Light", 16f);
			serverErrorLabel.TextColor = UIColor.Black;
			serverErrorLabel.TextAlignment = UITextAlignment.Center;
			serverErrorLabel.LineBreakMode = UILineBreakMode.WordWrap;
			serverErrorLabel.Lines = 0;
			serverErrorLabel.SizeToFit ();
			_crashView.AddSubview(serverErrorLabel);

			btnYPosition += 50;

			_btnRefresh.Frame = new CGRect(10, btnYPosition, UIScreen.MainScreen.Bounds.Width - 20, 50);
			_btnRefresh.SetTitle (ViewModel["Main_TryToRefresh"], UIControlState.Normal);
			_btnRefresh.SetTitleColor (ViewModel.Theme.Colors.ButtonTextColor.ToNativeColor (), UIControlState.Normal);
			_btnRefresh.BackgroundColor = ViewModel.Theme.Colors.ButtonColor.ToNativeColor ();
			_crashView.AddSubview(_btnRefresh);

			_btnRestart.Frame = new CGRect(10, btnYPosition + 60, UIScreen.MainScreen.Bounds.Width - 20, 50);
			_btnRestart.SetTitle (ViewModel["MAIN_Restart_after_crash"], UIControlState.Normal);
			_btnRestart.SetTitleColor (ViewModel.Theme.Colors.ButtonTextColor.ToNativeColor (), UIControlState.Normal);
			_btnRestart.BackgroundColor = ViewModel.Theme.Colors.ButtonColor.ToNativeColor ();
			_crashView.AddSubview(_btnRestart);
			// --- 

			_tableNews.ReloadData ();

			// MY ACTIVITES panel ------
			_myActivitiesView = new UIView ();
			_myActivitiesView.Frame = new CGRect(0, _userView.Frame.Bottom, UIScreen.MainScreen.Bounds.Width, 50);
			_myActivitiesView.BackgroundColor = UIColor.White;
			View.AddSubview (_myActivitiesView);

			_isNewsFeedPersonalChk.Frame = new CGRect(10, 10, UIScreen.MainScreen.Bounds.Width, 50);
			_isNewsFeedPersonalChk.OnTintColor = ViewModel.Theme.Colors.ActivitySaveButtonPressedColor.ToNativeColor ();
			_myActivitiesView.AddSubview (_isNewsFeedPersonalChk);

			var myActivitiesLabel = new UILabel();
			myActivitiesLabel.Frame = new CGRect(80, 0, UIScreen.MainScreen.Bounds.Width, 50);
			myActivitiesLabel.Text = ViewModel["Main_MyActivities"];
			_myActivitiesView.AddSubview (myActivitiesLabel);

			_btnCloseMyActivitiesPanel.Frame = new CGRect(UIScreen.MainScreen.Bounds.Width - 20, 0, 10, 50);
			_btnCloseMyActivitiesPanel.SetTitle ("x", UIControlState.Normal);
			_btnCloseMyActivitiesPanel.SetTitleColor (UIColor.Gray, UIControlState.Normal);
			//_btnCloseMyActivitiesPanel.BackgroundColor = ViewModel.Theme.Colors.ButtonColor.ToNativeColor ();
			_myActivitiesView.AddSubview(_btnCloseMyActivitiesPanel);
			// ------

			var loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			loadingOverlay.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
			View.AddSubview (loadingOverlay);

            var set = this.CreateBindingSet<MainView, Core.ViewModels.MainViewModel>();
			set.Bind(_liveRecordButton).For(x => x.Enabled).To(vm => vm.ShowLiveRecordButton);
			//set.Bind(_liveRecordButton).For(x => x.CustomView).To(vm => vm.ShowLiveRecordButton).WithConversion(new BoolToBarButtonEmptyViewValueConverter());
			//set.Bind(_loginButton).For(x => x.Enabled).To(vm => vm.ShowLoginButton);
			//set.Bind(_loginButton).For(x => x.CustomView).To(vm => vm.ShowLoginButton).WithConversion(new BoolToBarButtonEmptyViewValueConverter());
			//set.Bind(_liveRecordButton).For("Visibility").To(vm => vm.ShowLiveRecordButton).WithConversion("Visibility"); // not works yet
			//set.Bind(_loginButton).For("Visibility").To(vm => vm.ShowLoginButton).WithConversion("Visibility"); // not works yet
			set.Bind(_avatarPlaceholderImageView).For("Visibility").To(vm => vm.PictureURL).WithConversion("InvertedVisibility");
			set.Bind(_avatarImageView).For(x => x.ImageUrl).To(vm => vm.PictureURL);
			set.Bind(_progressView).For(x => x.Progress).To(vm => vm.Progress).WithConversion("Percents");
            set.Bind(_raceLabel).To(vm => vm.RaceTitle);
			set.Bind(_userLabel).To(vm => vm.FirstLastName);
			set.Bind(_scoreLabel).To(vm => vm.PointsPerPerson);
			set.Bind(_dateEndsLabel).To(vm => vm.EndsData);
			set.Bind(_tableNews).For("Visibility").To(vm => vm.FeedsListVisible).WithConversion("Visibility");
			set.Bind(_detailsView).For("Visibility").To(vm => vm.DetailFeedVisible).WithConversion("Visibility");
			set.Bind(_myActivitiesView).For("Visibility").To(vm => vm.ShowCloseMyActivitiesPanel).WithConversion("Visibility");
			set.Bind (_isNewsFeedPersonalChk).To (vm => vm.IsNewsFeedPersonal);
			set.Bind(_btnCloseMyActivitiesPanel).To(vm => vm.CloseMyActivitiesPanelCommand);
			set.Bind(_detailsHideButton).To(vm => vm.CloseDetailFeed);
			set.Bind(_detailsReadMoreButton).To(vm => vm.LunchReadMoreUrl);
			set.Bind(_detailsReadMoreButton).For("Visibility").To(vm => vm.ShowReadMoreButton).WithConversion("Visibility");

			set.Bind(_crashView).For("Visibility").To(vm => vm.IsCrash).WithConversion("Visibility");
			set.Bind(_btnRefresh).To(vm => vm.TapLogoCommand);
			set.Bind(_btnRestart).To(vm => vm.RestartCommand);

			set.Bind(source).To(vm => vm.News);
			set.Bind (loadingOverlay).For(v => v.IsVisible).To(vm => vm.IsBusy);


			set.Bind(btnMenu).To(vm => vm.ShowMenuCommand);
			set.Bind(btnHeaderLogo).To(vm => vm.TapLogoCommand);



			set.Bind(_tableNews).For(x => x.Frame).Mode(MvvmCross.Binding.MvxBindingMode.TwoWay).To(vm => vm.ShowRegisterActivityButton).WithConversion(new NewsTableFrameValueConverter(), this);
			set.Bind(_tableNews).For(x => x.Frame).Mode(MvvmCross.Binding.MvxBindingMode.TwoWay).To(vm => vm.ShowCloseMyActivitiesPanel).WithConversion(new NewsTableFrameValueConverter(), this);
			set.Bind(_btnRegister).To(vm => vm.RecordActivityCommand);
			set.Bind(_btnRegister).For("Visibility").To(vm => vm.ShowRegisterActivityButton).WithConversion("Visibility");
			set.Bind(_btnRegister).For(x => x.Frame).Mode(MvvmCross.Binding.MvxBindingMode.TwoWay).To(vm => vm.ShowFavoritButton).WithConversion(new RegisterButtonFrameValueConverter(), this);
			set.Bind(_btnRegisterFav).To(vm => vm.FavoritRecordCommand);
			set.Bind(_btnRegisterFav).For("Visibility").To(vm => vm.ShowFavoritButton).WithConversion("Visibility");
            set.Apply();
        }

		public CGRect GetNewsTableFrame()
		{
			var padding = 10;
			var height = (ViewModel as MainViewModel).ShowRegisterActivityButton 
				? _btnRegister.Frame.Y - _userView.Frame.Bottom - padding * 2
				: _btnRegister.Frame.Bottom - _userView.Frame.Bottom - padding * 2;

			if (ViewModel.ShowCloseMyActivitiesPanel) {
				height -= 50;
			}

			var y = ViewModel.ShowCloseMyActivitiesPanel ? _userView.Bounds.Height + padding + 50 : _userView.Bounds.Height + padding;

			return new CGRect(padding, y, UIScreen.MainScreen.Bounds.Width - padding * 2, height);
		}

		public CGRect GetRegisterButtonFrame()
		{
			var x = 0;
			var y = UIScreen.MainScreen.ApplicationFrame.Height - _navBarHeight - 50;
			var height = 50;
			var width = ViewModel.ShowFavoritButton ? UIScreen.MainScreen.Bounds.Width / 2 : UIScreen.MainScreen.Bounds.Width;
			return new CGRect(x, y, width, height);
		}

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden(false, true);
			this.NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

			this.NavigationController.NavigationBar.Translucent = false;

			this.NavigationController.NavigationBar.TintColor = UIColor.White;
			this.NavigationController.NavigationBar.BarTintColor = ViewModel.Theme.Colors.HeaderColor.ToNativeColor ();

			_headerBorder = new UIView(new CGRect(0, this.NavigationController.NavigationBar.Frame.Size.Height - 1, this.NavigationController.NavigationBar.Frame.Size.Width, 1));
			_headerBorder.BackgroundColor = ViewModel.Theme.Colors.HeaderBottomBorderColor.ToNativeColor();
			_headerBorder.Opaque = true;
			this.NavigationController.NavigationBar.AddSubview(_headerBorder);
		}

		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);

			if (_headerBorder != null)
			{
				_headerBorder.RemoveFromSuperview();
			}
		}

		private static NSAttributedString GetAttributedStringFromHtml(string html)
		{
			NSError error = null;
			var attrs = new NSAttributedStringDocumentAttributes{ DocumentType = NSDocumentType.HTML, StringEncoding = NSStringEncoding.UTF8 };
			var htmlString = new NSAttributedString (html, attrs, 
				ref error);
			return htmlString;
		}
    }

	public class NewsSource : MvxTableViewSource
	{
		private readonly MainView _view;

		public NewsSource(UITableView tableView, MainView view)
			: base(tableView)
		{
			_view = view;
			tableView.RegisterClassForCellReuse(typeof(ItemCell), ItemCell.Key);
		}

		protected override UITableViewCell GetOrCreateCellFor (UITableView tableView, NSIndexPath indexPath, object item)
		{
			return tableView.DequeueReusableCell(ItemCell.Key, indexPath);
		}
	}

	[Register("ItemCell")]
	public class ItemCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("ItemCell");
		private nfloat Width = UIScreen.MainScreen.Bounds.Width;

		private UILabel _labelTime;
		private UILabel _labelText;
		private UIView _separator;
		private UIView _background;
		private UIView _bottomLine;
		private MvxImageView _imageView;

		public ItemCell(IntPtr handle)
			: base(handle)
		{
			this.Frame = new CGRect (0, 0, Width , 60);

			var theme = Mvx.Resolve<IThemesManager> ();

			_background = new UIView (new CGRect(0, 0, Width, this.Frame.Height));
			_background.BackgroundColor = theme.CurrentTheme.Colors.FeedItemColor.ToNativeColor();
			Add (_background);

			_bottomLine = new UIView (new CGRect(0, this.Frame.Height - 2, Width, 2));
			_bottomLine.BackgroundColor = theme.CurrentTheme.Colors.NewsFeedListBackgroundColor.ToNativeColor ();
			Add (_bottomLine);

			_labelTime = new UILabel(new CGRect(0, 0, 70, this.Frame.Height - 2));
			_labelTime.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
			_labelTime.TextAlignment = UITextAlignment.Center;
			_labelTime.LineBreakMode = UILineBreakMode.WordWrap;
			_labelTime.Lines = 0;

			_labelText = new UILabel(new CGRect(80, 0, Width - 140, this.Frame.Height - 2));
			_labelText.Font = UIFont.FromName("HelveticaNeue-Light", 14f);
			_labelText.TextAlignment = UITextAlignment.Left;
			_labelText.LineBreakMode = UILineBreakMode.TailTruncation;
			_labelText.Lines = 3;

			_imageView = new MvxImageView ();
			_imageView.Frame = new CGRect(_labelText.Frame.Right + 5, 15, 30, 30);
			_imageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			Add(_labelTime);
			Add(_labelText);
			Add(_imageView);

			this.SelectedBackgroundView = new UIView (this.Frame);
			this.SelectedBackgroundView.BackgroundColor = theme.CurrentTheme.Colors.FeedItemSelectedColor.ToNativeColor ();

			//this.AccessoryView = _label2;

			_separator = new UIView(new CGRect(70, 0, 2, this.Frame.Height - 2));
			Add (_separator);

			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<ItemCell, NewsFeedItemModel>();
					set.Bind(_labelTime).To(vm => vm).WithConversion(new NewsTimeValueConverter());
					set.Bind(_separator).For(x => x.BackgroundColor).To(vm => vm.SeparatorColor).WithConversion("NativeColor");
					set.Bind(_labelText).For(x => x.AttributedText).To(vm => vm.Text).WithConversion(new NewsItemTextValueConverter());
					//set.Bind(_labelText).For(x => x.Frame).To(vm => vm.FullImageURL).WithConversion(new NewsItemTextValueConverter());
					set.Bind(_imageView).To(vm => vm.ThumbnailURL);

					set.Apply();
				});

		}
	}
}