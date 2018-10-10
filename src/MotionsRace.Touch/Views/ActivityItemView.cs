using MvvmCross.Binding.BindingContext;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System;
using Acr.UserDialogs;
using System.Timers;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Plugins.Color.iOS;

namespace MotionsRace.Touch.Views
{
	[Register("ActivityItemView")]
	public class ActivityItemView : MvxViewController<ActivityItemViewModel>
    {
		private UIScrollView _scrollView;
		private UIBarButtonItem _loginButton;
		private UIButton _btnSave;
		private UIButton _btnCancel;
		private UIButton _btnImage;
		private ActivityItemViewModel _vm;
		private List<UISwitch> _days = new List<UISwitch>();
		private UITextField _tbUnits;
		private UITextField _tbDistance;
		private UISlider _slider = new UISlider();
		private UILabel _imgLabel;
		private UILabel _noteLabel;

		private UIButton _btnRefresh;

		private UIView _headerBorder;

		private UIView _liveRecordViewPlaceholder = new UIView();
		private UIView _liveRecordView = new UIView();
		private UILabel _liveRecordTimeLabel;
		private UIButton _btnLiveRecordStartCancel;
		private UIButton _btnLiveRecordFinish;
		private UIButton _btnLiveRecordPauseResume;
		private UILabel _liveRecordNoteLabel;
		private MvxImageView _clockImageView;

		private UISwitch _checkboxShareToFacebook = new UISwitch();

		private Timer _timer;
		private int _animFrame;

		private readonly List<string> _clockFrameList = new List<string>
		{
			"clock_animation_01.png",
			"clock_animation_02.png",
			"clock_animation_03.png",
			"clock_animation_04.png",
			"clock_animation_05.png",
			"clock_animation_06.png",
			"clock_animation_07.png",
			"clock_animation_08.png",
			"clock_animation_09.png",
			"clock_animation_10.png",
			"clock_animation_11.png",
			"clock_animation_12.png"
		};

        public override void ViewDidLoad()
        {
			try
			{
				base.ViewDidLoad();
				var backgroundColor = ViewModel.Theme.Colors.ActivityItemBackgroundColor.ToNativeColor ();
				View = new UIView { BackgroundColor = backgroundColor };

				this.Title = "";

				InitTimerAnimation();

				//NSNotificationCenter.DefaultCenter.AddObserver (new NSString( "UIKeyboardWillShowNotification"), KeyboardWillShow);

				_vm = ViewModel as ActivityItemViewModel;

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
				btnHeaderLogo.AutoresizingMask = UIViewAutoresizing.FlexibleHeight | UIViewAutoresizing.FlexibleWidth;
				headerTopView.AddSubview(btnHeaderLogo);

				this.NavigationItem.TitleView = headerTopView;
				// -----------

				// header panel
				var headerView = new UIView (new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width , 40));
				headerView.BackgroundColor = ViewModel.Theme.Colors.PageTitleColor.ToNativeColor ();
				View.AddSubview(headerView);

				// welcome label
				var titleLabel = new UILabel(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 40));
				titleLabel.TextColor =  ViewModel.Theme.Colors.TextPageTitleColor.ToNativeColor();
				titleLabel.TextAlignment = UITextAlignment.Center;
				titleLabel.Text = ViewModel["Activity_RegisterActivity"];
				headerView.AddSubview(titleLabel);


				// ios7 layout
				if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
				{
					EdgesForExtendedLayout = UIRectEdge.None;
				}

				// header
				_loginButton = new UIBarButtonItem (UIBarButtonSystemItem.Action, (sender, args) => {
					ViewModel.LoginCommand.Execute ();
				});

				this.NavigationItem.SetRightBarButtonItem(_loginButton, true);

				_loginButton.TintColor = ViewModel.ShowLoginButton ? ViewModel.Theme.Colors.TextHeaderColor.ToNativeColor() : UIColor.Clear;

				var separatorLine = new UIView (new CGRect(0, headerView.Bounds.Height - 2, UIScreen.MainScreen.Bounds.Width, 2));
				separatorLine.BackgroundColor = ViewModel.Theme.Colors.ActivityTypesPanelColor.ToNativeColor();
				this.View.AddSubview(separatorLine);

				_scrollView = new UIScrollView (new CGRect(0, 40, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 40));
				View.AddSubview (_scrollView);

				// ----- LIVE RECORD -----
				_liveRecordViewPlaceholder.Frame = new CGRect (0, 40, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - 40);
				_liveRecordViewPlaceholder.BackgroundColor = UIColor.FromRGB (0,0,0).ColorWithAlpha(0.5f);
				View.AddSubview (_liveRecordViewPlaceholder);

				_liveRecordView.Frame = new CGRect (0, UIScreen.MainScreen.Bounds.Height / 2 - 150, UIScreen.MainScreen.Bounds.Width, 300);
				_liveRecordView.BackgroundColor = UIColor.FromRGB (237,235,238);

				var yLiveRecordPos = 20;

				// name
				var nameLabel = new UILabel(new CGRect(10, yLiveRecordPos, _liveRecordView.Bounds.Width - 20, 0));
				nameLabel.Font = UIFont.FromName("HelveticaNeue-Light", 16f);
				nameLabel.TextColor = UIColor.Black;
				nameLabel.TextAlignment = UITextAlignment.Left;
				nameLabel.Text = ViewModel.TrainingTypeName;
				nameLabel.LineBreakMode = UILineBreakMode.WordWrap;
				nameLabel.Lines = 0;
				//nameLabel.LayoutMargins = new UIEdgeInsets (100, 100, 100, 100);
				nameLabel.SizeToFit ();
				_liveRecordView.AddSubview(nameLabel);

				// description
				var descrLabel = new UILabel(new CGRect(10, nameLabel.Frame.Bottom + 5, _liveRecordView.Bounds.Width-20, 0));
				descrLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
				descrLabel.TextColor = UIColor.Black;
				descrLabel.TextAlignment = UITextAlignment.Left;
				descrLabel.Text = ViewModel.TrainingDescription;
				descrLabel.LineBreakMode = UILineBreakMode.WordWrap;
				descrLabel.Lines = 0;
				descrLabel.SizeToFit ();
				_liveRecordView.AddSubview(descrLabel);

				_clockImageView = new MvxImageView(new CGRect(10, descrLabel.Frame.Bottom + 5, UIScreen.MainScreen.Bounds.Width - 20, 50));
				_clockImageView.Image = UIImage.FromFile("clock_animation_01.png");
				_clockImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
				_liveRecordView.AddSubview(_clockImageView);

				_liveRecordTimeLabel = new UILabel(new CGRect(0, _clockImageView.Frame.Bottom + 5, _liveRecordView.Frame.Width, 60));
				_liveRecordTimeLabel.Font = UIFont.FromName("HelveticaNeue-Light", 35f);
				_liveRecordTimeLabel.TextColor = UIColor.Black;
				_liveRecordTimeLabel.TextAlignment = UITextAlignment.Center;
				_liveRecordView.AddSubview(_liveRecordTimeLabel);

				// save button
				_btnLiveRecordStartCancel = new UIButton(UIButtonType.RoundedRect);
				_btnLiveRecordStartCancel.Frame = new CGRect(10, _liveRecordTimeLabel.Frame.Bottom + 5, UIScreen.MainScreen.Bounds.Width - 20, 30);
				_btnLiveRecordStartCancel.Layer.BorderWidth = 1;
				_btnLiveRecordStartCancel.Layer.BorderColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor().CGColor;
				_liveRecordView.AddSubview(_btnLiveRecordStartCancel);

				// finish button
				_btnLiveRecordFinish = new UIButton(UIButtonType.RoundedRect);
				_btnLiveRecordFinish.Frame = new CGRect(10, _liveRecordTimeLabel.Frame.Bottom + 5, (UIScreen.MainScreen.Bounds.Width - 20) / 2, 30);
				_btnLiveRecordFinish.SetTitle (this.ViewModel ["Live_Record_Finish"], UIControlState.Normal);
				_btnLiveRecordFinish.SetTitleColor (this.ViewModel.Theme.Colors.ActivityCancelButtonColor.ToNativeColor(), UIControlState.Normal);
				_btnLiveRecordFinish.BackgroundColor = this.ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor ();
				_btnLiveRecordFinish.Layer.BorderWidth = 1;
				_btnLiveRecordFinish.Layer.BorderColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor().CGColor;
				_liveRecordView.AddSubview(_btnLiveRecordFinish);

				// pause-resume button
				_btnLiveRecordPauseResume = new UIButton(UIButtonType.RoundedRect);
				_btnLiveRecordPauseResume.Frame = new CGRect(10 + (UIScreen.MainScreen.Bounds.Width - 20) / 2, _liveRecordTimeLabel.Frame.Bottom + 5, (UIScreen.MainScreen.Bounds.Width - 20) / 2, 30);
				_btnLiveRecordPauseResume.Layer.BorderWidth = 1;
				_btnLiveRecordPauseResume.Layer.BorderColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor().CGColor;
				_liveRecordView.AddSubview(_btnLiveRecordPauseResume);

				_liveRecordNoteLabel = new UILabel();
				_liveRecordNoteLabel.Frame = new CGRect(10, _btnLiveRecordStartCancel.Frame.Bottom + 5, UIScreen.MainScreen.Bounds.Width - 20, 40);
				_liveRecordNoteLabel.Font = UIFont.FromName("HelveticaNeue-Light", 10f);
				_liveRecordNoteLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				_liveRecordNoteLabel.TextAlignment = UITextAlignment.Left;
				_liveRecordNoteLabel.ContentMode = UIViewContentMode.TopLeft;
				_liveRecordNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
				_liveRecordNoteLabel.Lines = 0;
				_liveRecordView.AddSubview (_liveRecordNoteLabel);

				_liveRecordNoteLabel = new UILabel();
				_liveRecordNoteLabel.Frame = new CGRect(10, _btnLiveRecordStartCancel.Frame.Bottom + 5, UIScreen.MainScreen.Bounds.Width - 20, 40);
				_liveRecordNoteLabel.Font = UIFont.FromName("HelveticaNeue-Light", 10f);
				_liveRecordNoteLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				_liveRecordNoteLabel.TextAlignment = UITextAlignment.Left;
				_liveRecordNoteLabel.ContentMode = UIViewContentMode.TopLeft;
				_liveRecordNoteLabel.LineBreakMode = UILineBreakMode.WordWrap;
				_liveRecordNoteLabel.Lines = 0;
				_liveRecordView.AddSubview (_liveRecordNoteLabel);


				_liveRecordView.Frame = new CGRect (0, UIScreen.MainScreen.Bounds.Height / 2 - 150, UIScreen.MainScreen.Bounds.Width, _btnLiveRecordStartCancel.Frame.Bottom + 50);

				// ------- LIVE RECORD END ------

				View.AddSubview (_liveRecordView);

				// training type name/description panel
				var trainingTitleView = new UIView(new CGRect(10, 10, UIScreen.MainScreen.Bounds.Width - 20, 0));

				// training type name label
				var trainingTypeNameLabel = new UILabel(new CGRect(5, 5, trainingTitleView.Bounds.Width - 10, 0));
				trainingTypeNameLabel.BackgroundColor = ViewModel.Theme.Colors.ActivityItemPanelColor.ToNativeColor();
				trainingTypeNameLabel.Font = UIFont.FromName("HelveticaNeue-Light", 16f);
				trainingTypeNameLabel.TextColor =  ViewModel.Theme.Colors.ActivityItemPanelTextColor.ToNativeColor();
				trainingTypeNameLabel.TextAlignment = UITextAlignment.Left;
				trainingTypeNameLabel.Text = ViewModel.TrainingTypeName;
				trainingTypeNameLabel.LineBreakMode = UILineBreakMode.WordWrap;
				trainingTypeNameLabel.Lines = 0;
				//trainingTypeNameLabel.LayoutMargins = new UIEdgeInsets (10, 10, 10, 10);
				trainingTypeNameLabel.SizeToFit ();
				trainingTitleView.AddSubview(trainingTypeNameLabel);

				// training type name label
				var trainingTypeDescrLabel = new UILabel(new CGRect(5, trainingTypeNameLabel.Bounds.Height + 5, trainingTitleView.Bounds.Width-10, 0));
				trainingTypeDescrLabel.BackgroundColor = ViewModel.Theme.Colors.ActivityItemPanelColor.ToNativeColor();
				trainingTypeDescrLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
				trainingTypeDescrLabel.TextColor =  ViewModel.Theme.Colors.ActivityItemPanelTextColor.ToNativeColor();
				trainingTypeDescrLabel.TextAlignment = UITextAlignment.Left;
				trainingTypeDescrLabel.Text = ViewModel.TrainingDescription;
				trainingTypeDescrLabel.LineBreakMode = UILineBreakMode.WordWrap;
				trainingTypeDescrLabel.Lines = 0;
				trainingTypeDescrLabel.SizeToFit ();
				trainingTitleView.AddSubview(trainingTypeDescrLabel);

				trainingTitleView.Frame = new CGRect (trainingTitleView.Frame.X, trainingTitleView.Frame.Y, trainingTitleView.Frame.Width, trainingTypeNameLabel.Bounds.Height + trainingTypeDescrLabel.Bounds.Height + 10);
				trainingTitleView.BackgroundColor = ViewModel.Theme.Colors.ActivityItemPanelColor.ToNativeColor();
				_scrollView.AddSubview(trainingTitleView);

				var checkboxY = trainingTitleView.Frame.Top + trainingTitleView.Bounds.Height + 17;

				// refresh button
				_btnRefresh = new UIButton(UIButtonType.RoundedRect);
				//_btnRefresh.SetTitle(" refresh", UIControlState.Normal);
				_btnRefresh.SetImage(UIImage.FromFile ("refresh_icon.png"), UIControlState.Normal);
				_btnRefresh.TintColor = UIColor.White;
				_btnRefresh.Frame = new CGRect ((UIScreen.MainScreen.Bounds.Width / 2) - 15, trainingTitleView.Frame.Bottom + 15, 30, 30);
				_btnRefresh.SetTitleColor (ViewModel.Theme.Colors.ActivityCancelButtonColor.ToNativeColor(), UIControlState.Normal);
				_btnRefresh.BackgroundColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor();
				_scrollView.AddSubview(_btnRefresh);

				// dates
				foreach (var item in _vm.DateCheckBoxes) {

					var checkbox = new UISwitch ();
					checkbox.Frame = new CGRect (10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 0);
					checkbox.OnImage = UIImage.FromFile ("checkbox_selected.png");
					checkbox.OffImage = UIImage.FromFile ("checkbox.png");
					checkbox.OnTintColor = ViewModel.Theme.Colors.ActivitySaveButtonPressedColor.ToNativeColor();
					_scrollView.AddSubview (checkbox);
					_days.Add (checkbox);
					checkbox.SizeToFit();

					var dayLabel = new UILabel(new CGRect(checkbox.Frame.X + checkbox.Frame.Width + 10, checkboxY + 8, UIScreen.MainScreen.Bounds.Width-100, checkbox.Frame.Height));
					dayLabel.Font = UIFont.FromName("HelveticaNeue-Light", 13f);
					//dayLabel.TextColor =  item.c.ToNativeColor();
					dayLabel.TextAlignment = UITextAlignment.Left;
					dayLabel.ContentMode = UIViewContentMode.Center;
					dayLabel.Text = item.DateText;
					dayLabel.SizeToFit ();
					_scrollView.AddSubview (dayLabel);

					checkboxY += checkbox.Frame.Height + 2;
				}



				// units-distance

				var keyboardToolbar = new UIToolbar ();
                keyboardToolbar.BarTintColor = UIColor.FromRGBA(22, 62, 85, 255);
				keyboardToolbar.SizeToFit ();
				var flexBarButton = new UIBarButtonItem (UIBarButtonSystemItem.FixedSpace);
				var doneBarButton = new UIBarButtonItem (UIBarButtonSystemItem.Done, (s, e) =>
					{
						_tbUnits.ResignFirstResponder();
						_tbDistance.ResignFirstResponder();
					});
				keyboardToolbar.Items = new UIBarButtonItem[] { flexBarButton, doneBarButton };

				var daysTop = _days.Count != 0 ? _days.Last ().Frame.Top : 0;
				var daysHeight = _days.Count != 0 ? _days.Last ().Frame.Height : 0;


				var tbRect = new CGRect (UIScreen.MainScreen.Bounds.Width / 2 + 50, daysTop, UIScreen.MainScreen.Bounds.Width / 2 - 60, daysHeight);
				var lblRect = new CGRect (UIScreen.MainScreen.Bounds.Width / 2 + 50, daysTop - 15, UIScreen.MainScreen.Bounds.Width / 2 - 60, daysHeight);

				// Units
				var unitsLabel = new UILabel();
				unitsLabel.Frame = lblRect;
				unitsLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
				unitsLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				unitsLabel.TextAlignment = UITextAlignment.Left;
				unitsLabel.ContentMode = UIViewContentMode.Center;
				unitsLabel.Text = _vm.UnitType;
				unitsLabel.SizeToFit();
				_scrollView.AddSubview(unitsLabel);

				_tbUnits = new UITextField();
				_tbUnits.AdjustsFontSizeToFitWidth = true;
				_tbUnits.KeyboardType = UIKeyboardType.NumberPad;
				_tbUnits.ReturnKeyType = UIReturnKeyType.Done;
				_tbUnits.Frame = tbRect;
				_tbUnits.BackgroundColor = ViewModel.Theme.Colors.ActivityItemBoxesColor.ToNativeColor();
				_tbUnits.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				_tbUnits.InputAccessoryView = keyboardToolbar;
				_scrollView.AddSubview(_tbUnits);

                if (_vm.UnitValueVisibility && _vm.DistanceVisibility)
                {
                    tbRect = new CGRect(UIScreen.MainScreen.Bounds.Width / 2 + 50, daysTop - daysHeight - 15, UIScreen.MainScreen.Bounds.Width / 2 - 60, daysHeight);
                    lblRect = new CGRect(UIScreen.MainScreen.Bounds.Width / 2 + 50, daysTop - 15 - daysHeight - 15, UIScreen.MainScreen.Bounds.Width / 2 - 60, daysHeight);
                }

				// Distance
				var distanceLabel = new UILabel();
				distanceLabel.Frame = lblRect;
				distanceLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
				distanceLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				distanceLabel.TextAlignment = UITextAlignment.Left;
				distanceLabel.ContentMode = UIViewContentMode.Center;
				distanceLabel.Text = _vm["Activity_Distance"];
				distanceLabel.SizeToFit ();
				_scrollView.AddSubview (distanceLabel);

				_tbDistance = new UITextField();
				_tbDistance.AdjustsFontSizeToFitWidth = true;
				_tbDistance.Placeholder = _vm.DistanceValue;
				_tbDistance.KeyboardType = UIKeyboardType.NumberPad;
				_tbDistance.ReturnKeyType = UIReturnKeyType.Done;
				_tbDistance.Frame = tbRect;
				_tbDistance.BackgroundColor = ViewModel.Theme.Colors.ActivityItemBoxesColor.ToNativeColor();
				_tbDistance.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				_tbDistance.InputAccessoryView = keyboardToolbar;
				_scrollView.AddSubview(_tbDistance);



				checkboxY = _days.Count != 0 ? _days.Last ().Frame.Bottom + 10 : 10;

				// Intensity
				if (_vm.IsIntensityVisible) {
					var sliderLabel = new UILabel ();
					sliderLabel.Frame = new CGRect (10, checkboxY, UIScreen.MainScreen.Bounds.Width, 0);
					sliderLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 12f);
					sliderLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
					sliderLabel.TextAlignment = UITextAlignment.Left;
					sliderLabel.ContentMode = UIViewContentMode.Center;
					sliderLabel.Text = ViewModel ["Activity_Intensity"];
					sliderLabel.SizeToFit ();
					_scrollView.AddSubview (sliderLabel);

					var itemWidth = UIScreen.MainScreen.Bounds.Width / 5;
					nfloat x = 0;

					for (int i = 1; i < 6; i++) {

						if (i == 1)
							x = 10 + 12;
						else if (i == 2)
							x = 10 + itemWidth * 1 + 15;
						else if (i == 3)
							x = 10 + itemWidth * 2 + 18;
						else if (i == 4)
							x = 10 + itemWidth * 3 + 22;
						else if (i == 5)
							x = UIScreen.MainScreen.Bounds.Width - 27;

						var numberLabel = new UILabel ();
						numberLabel.Frame = new CGRect (x, checkboxY + 17, itemWidth, 0);
						numberLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 10f);
						numberLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
						numberLabel.TextAlignment = UITextAlignment.Left;
						numberLabel.ContentMode = UIViewContentMode.Left;
						numberLabel.Text = i.ToString();
						numberLabel.SizeToFit ();
						_scrollView.AddSubview (numberLabel);
					}

					checkboxY += sliderLabel.Frame.Height + 10;

					_slider = new UISlider ();
					_slider.Frame = new CGRect (10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 0);
					//slider.SetThumbImage(UIImage.FromFile("intensity_circle.png"), UIControlState.Normal);
					_slider.ThumbTintColor = ViewModel.Theme.Colors.ActivitySaveButtonPressedColor.ToNativeColor();
					_slider.TintColor = ViewModel.Theme.Colors.ActivitySaveButtonPressedColor.ToNativeColor();
					_slider.MinimumTrackTintColor = ViewModel.Theme.Colors.ActivitySaveButtonPressedColor.ToNativeColor();
					_slider.MaximumTrackTintColor = ViewModel.Theme.Colors.ActivityItemBoxesColor.ToNativeColor();
					_slider.MinValue = 1;
					_slider.MaxValue = 5;
					_slider.SizeToFit ();
					_slider.ValueChanged += (sender, e) => {
						UISlider sl = sender as UISlider;
						sl.Value = (int)Math.Round ((double)sl.Value);
					};
					_scrollView.AddSubview (_slider);
					// TODO: add 1,2,3,4,5 point above slider

					checkboxY += _slider.Bounds.Height + 10;
				}

				// open image button
				_btnImage = new UIButton(UIButtonType.RoundedRect);
				_btnImage.Frame = new CGRect(10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 30);
				//_btnImage.SetTitle (ViewModel.UploadImageText, UIControlState.Normal); 
				_btnImage.SetTitleColor (ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor(), UIControlState.Normal);
				_btnImage.BackgroundColor = ViewModel.Theme.Colors.ActivityItemBoxesColor.ToNativeColor();
				_scrollView.AddSubview(_btnImage);

				_imgLabel = new UILabel();
				_imgLabel.Frame = new CGRect(10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 30);
				_imgLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
				_imgLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				_imgLabel.TextAlignment = UITextAlignment.Center;
				_imgLabel.ContentMode = UIViewContentMode.Center;
				_scrollView.AddSubview (_imgLabel);

				// pic
				var picView = new UIImageView (UIImage.FromBundle("select_image.png"));
				picView.Frame = new CGRect(13, checkboxY + 3, 23, 23);
				picView.ContentMode = UIViewContentMode.ScaleAspectFit;
				_scrollView.AddSubview(picView);

				checkboxY += _btnImage.Bounds.Height + 10;


				// share to facebook
				if (ViewModel.ShowShareToFacebookCheckBox) {
					_checkboxShareToFacebook = new UISwitch ();
					_checkboxShareToFacebook.Frame = new CGRect (10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 0);
					_checkboxShareToFacebook.OnImage = UIImage.FromFile ("checkbox_selected.png");
					_checkboxShareToFacebook.OffImage = UIImage.FromFile ("checkbox.png");
					_checkboxShareToFacebook.OnTintColor = ViewModel.Theme.Colors.ActivitySaveButtonPressedColor.ToNativeColor ();
					_scrollView.AddSubview (_checkboxShareToFacebook);
					_checkboxShareToFacebook.SizeToFit ();

					var shareToFacebookLabel = new UILabel (new CGRect (_checkboxShareToFacebook.Frame.X + _checkboxShareToFacebook.Frame.Width + 10, checkboxY + 8, UIScreen.MainScreen.Bounds.Width - 100, _checkboxShareToFacebook.Frame.Height));
					shareToFacebookLabel.Font = UIFont.FromName ("HelveticaNeue-Light", 13f);
					shareToFacebookLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
					shareToFacebookLabel.TextAlignment = UITextAlignment.Left;
					shareToFacebookLabel.ContentMode = UIViewContentMode.Center;
					shareToFacebookLabel.Text = ViewModel["Activity_ShareToFacebook"];
					shareToFacebookLabel.SizeToFit ();
					_scrollView.AddSubview (shareToFacebookLabel);

					checkboxY += _checkboxShareToFacebook.Frame.Height + 2;
				}


				// Comments
				var commentsLabel = new UILabel();
				commentsLabel.Frame = new CGRect(10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 0);
				commentsLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
				commentsLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				commentsLabel.TextAlignment = UITextAlignment.Left;
				commentsLabel.ContentMode = UIViewContentMode.Center;
				commentsLabel.Text = ViewModel["Activity_Comments"];
				commentsLabel.SizeToFit ();
				_scrollView.AddSubview (commentsLabel);

				checkboxY += commentsLabel.Bounds.Height + 2;

				var tbComments = new UITextField();
				tbComments.ReturnKeyType = UIReturnKeyType.Done;
				tbComments.Frame = new CGRect (10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 30);
				tbComments.BackgroundColor = ViewModel.Theme.Colors.ActivityItemBoxesColor.ToNativeColor();
				tbComments.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				tbComments.ShouldReturn += (textField) => { 
					tbComments.ResignFirstResponder();
					return true;
				};
				tbComments.EditingDidBegin += (s, e) => { 
					
					_scrollView.Frame = new CGRect(_scrollView.Frame.X, 
						_scrollView.Frame.Y, 
						_scrollView.Frame.Width,
						_scrollView.Frame.Height - 215 + 50);   //resize

					_scrollView.ContentOffset = new CGPoint(0, tbComments.Frame.Location.Y - 150);

				};
				tbComments.EditingDidEnd += (s, e) => { 
					_scrollView.Frame = new CGRect(_scrollView.Frame.X, 
						_scrollView.Frame.Y, 
						_scrollView.Frame.Width,
						_scrollView.Frame.Height + 215 - 50);   //resize
				};
				_scrollView.AddSubview(tbComments);

				checkboxY += tbComments.Bounds.Height + 10;

				// save button
				_btnSave = new UIButton(UIButtonType.RoundedRect);
				_btnSave.Frame = new CGRect(10, checkboxY, UIScreen.MainScreen.Bounds.Width / 2 - 10, 30);
				_btnSave.SetTitle (ViewModel["Activity_Save"], UIControlState.Normal);
				_btnSave.SetTitleColor (ViewModel.Theme.Colors.ActivityCancelButtonColor.ToNativeColor(), UIControlState.Normal);
				_btnSave.BackgroundColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor();
				_scrollView.AddSubview(_btnSave);

				// cancel button
				_btnCancel = new UIButton(UIButtonType.RoundedRect);
				_btnCancel.Frame = new CGRect(10 + _btnSave.Frame.Width, checkboxY, UIScreen.MainScreen.Bounds.Width / 2 - 10, 30);
				_btnCancel.SetTitle (ViewModel["Activity_Cancel"], UIControlState.Normal); 
				_btnCancel.SetTitleColor(ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor(), UIControlState.Normal);
				_btnCancel.BackgroundColor = ViewModel.Theme.Colors.ActivityCancelButtonColor.ToNativeColor ();
				_btnCancel.Layer.BorderWidth = 1;
				_btnCancel.Layer.BorderColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor().CGColor;
				_scrollView.AddSubview(_btnCancel);

				checkboxY += _btnCancel.Bounds.Height + 10;

				_noteLabel = new UILabel();
				_noteLabel.Frame = new CGRect(10, checkboxY, UIScreen.MainScreen.Bounds.Width - 20, 40);
				_noteLabel.Font = UIFont.FromName("HelveticaNeue-Light", 10f);
				_noteLabel.TextColor = ViewModel.Theme.Colors.ActivityItemTextColor.ToNativeColor();
				_noteLabel.TextAlignment = UITextAlignment.Left;
				_noteLabel.ContentMode = UIViewContentMode.TopLeft;
				_noteLabel.LineBreakMode = UILineBreakMode.WordWrap;
				_noteLabel.Lines = 0;
				_scrollView.AddSubview (_noteLabel);

				checkboxY += _noteLabel.Bounds.Height + 10;

				_scrollView.ContentSize = new CGSize(UIScreen.MainScreen.Bounds.Width, checkboxY + 65);

				var loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
				loadingOverlay.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
				View.AddSubview (loadingOverlay);

				var set = this.CreateBindingSet<ActivityItemView, Core.ViewModels.ActivityItemViewModel>();
				set.Bind(_loginButton).For(x => x.Enabled).To(v => v.ShowLoginButton);
				//set.Bind(_loginButton).For("Visibility").To(vm => vm.ShowLoginButton).WithConversion("Visibility"); // not works yet
				//set.Bind(_loginButton).For(x => x.CustomView).To(vm => vm.ShowLoginButton).WithConversion(new BoolToBarButtonEmptyViewValueConverter());
				set.Bind (loadingOverlay).For(v => v.IsVisible).To(vm => vm.IsBusy);
				set.Bind(_btnSave).To(vm => vm.SaveActivityCommand);
				set.Bind(_btnCancel).To(vm => vm.CancelCommand);
				set.Bind(_btnImage).To(vm => vm.UploadImageCommand);
				set.Bind(btnHeaderLogo).To(vm => vm.TapLogoCommand);
				for (int i = 0; i < _days.Count; i++) {
					set.Bind(_days[i]).To(vm => vm.DateCheckBoxes[i].Checked);
				}
				set.Bind(unitsLabel).For("Visibility").To(vm => vm.UnitValueVisibility).WithConversion("Visibility");
				set.Bind(_tbUnits).For("Visibility").To(vm => vm.UnitValueVisibility).WithConversion("Visibility");
                set.Bind(_tbUnits).To(vm => vm.UnitValue);
				set.Bind(distanceLabel).For("Visibility").To(vm => vm.DistanceVisibility).WithConversion("Visibility");
				set.Bind(_tbDistance).For("Visibility").To(vm => vm.DistanceVisibility).WithConversion("Visibility");
				set.Bind(_tbDistance).To(vm => vm.DistanceValue);
				set.Bind(_slider).To(vm => vm.IntensitySelected);
				set.Bind(tbComments).To(vm => vm.Comments);
				set.Bind(_imgLabel).For(x => x.Text).To(vm => vm.UploadImageText);
				set.Bind(_btnImage).For(x => x.BackgroundColor).To(vm => vm.ImageSelectBackground).WithConversion("NativeColor");
				set.Bind(_noteLabel).For(x => x.AttributedText).To(vm => vm.NoteText).WithConversion(new TipsValueConverter());

				set.Bind(_liveRecordViewPlaceholder).For("Visibility").To(vm => vm.LiveRecordVisibility).WithConversion("Visibility");
				set.Bind(_liveRecordView).For("Visibility").To(vm => vm.LiveRecordVisibility).WithConversion("Visibility");
				set.Bind(_liveRecordTimeLabel).To(vm => vm.LiveRecordTime);

				set.Bind(_btnLiveRecordStartCancel).To(vm => vm.LiveRecordStartCancelCommand);
				set.Bind (_btnLiveRecordStartCancel).For ("Title").To (vm => vm.StartCancelButtonTitle);
				set.Bind (_btnLiveRecordStartCancel).For ("TitleColor").To (vm => vm.PauseResumeButtonTextColor).WithConversion("NativeColor");
				set.Bind (_btnLiveRecordStartCancel).For (x => x.BackgroundColor).To (vm => vm.PauseResumeButtonColor).WithConversion("NativeColor");
				set.Bind(_btnLiveRecordStartCancel).For("Visibility").To(vm => vm.LiveRecordCancelButtonVisibility).WithConversion("Visibility");

				set.Bind(_btnRefresh).For("Visibility").To(vm => vm.ShowRefreshButton).WithConversion("Visibility");
				set.Bind(_btnRefresh).To(vm => vm.TapLogoCommand);

				set.Bind(_btnLiveRecordFinish).To(vm => vm.LiveRecordFinishCommand);
				set.Bind(_btnLiveRecordFinish).For("Visibility").To(vm => vm.LiveRecordCancelButtonVisibility).WithConversion("InvertedVisibility");

				set.Bind(_btnLiveRecordPauseResume).To(vm => vm.LiveRecordPauseResumeCommand);
				set.Bind (_btnLiveRecordPauseResume).For ("Title").To (vm => vm.PauseResumeButtonTitle);
				set.Bind (_btnLiveRecordPauseResume).For ("TitleColor").To (vm => vm.PauseResumeButtonTextColor).WithConversion("NativeColor");
				set.Bind (_btnLiveRecordPauseResume).For (x => x.BackgroundColor).To (vm => vm.PauseResumeButtonColor).WithConversion("NativeColor");
				set.Bind(_btnLiveRecordPauseResume).For("Visibility").To(vm => vm.LiveRecordCancelButtonVisibility).WithConversion("InvertedVisibility");

				set.Bind(_checkboxShareToFacebook).To(vm => vm.IsShareToFacebook);

				set.Bind(_liveRecordNoteLabel).For("Visibility").To(vm => vm.LiveRecordNoteVisibility).WithConversion("Visibility");
				set.Bind(_liveRecordNoteLabel).For(x => x.AttributedText).To(vm => vm.LiveRecordNoteText).WithConversion(new TipsValueConverter());
				set.Apply();
			}
			catch(Exception exception)
			{
				//Insights.Report(exception, new Dictionary <string, string> { 
				//	{"info", "activityitemview,ViewDidLoad"}
				//}, Xamarin.Insights.Severity.Error);
			}

        }

		public override void ViewWillAppear (bool animated)
		{
			try
			{
				base.ViewWillAppear (animated);
				this.NavigationController.SetNavigationBarHidden(false, true);
				this.NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

				this.NavigationController.NavigationBar.TintColor = UIColor.White;
				this.NavigationController.NavigationBar.BarTintColor = ViewModel.Theme.Colors.HeaderColor.ToNativeColor ();

				_headerBorder = new UIView(new CGRect(0, this.NavigationController.NavigationBar.Frame.Size.Height - 1, this.NavigationController.NavigationBar.Frame.Size.Width, 1));
				_headerBorder.BackgroundColor = ViewModel.Theme.Colors.HeaderBottomBorderColor.ToNativeColor();
				_headerBorder.Opaque = true;
				this.NavigationController.NavigationBar.AddSubview(_headerBorder);

				if (ViewModel.AnimateLiveRecordClock == null)
				{
					ViewModel.AnimateLiveRecordClock += AnimateLiveRecordClock;

				}
				ViewModel.OnResume ();
			}
			catch(Exception exception)
			{
				//Insights.Report(exception, new Dictionary <string, string> { 
				//		{"info", "activityitemview,ViewWillAppear"}
				//}, Xamarin.Insights.Severity.Error);
			}

		}

		public override void ViewWillDisappear (bool animated)
		{
			base.ViewWillDisappear (animated);

			//ViewModel.OnPause ();

			if (_headerBorder != null)
			{
				_headerBorder.RemoveFromSuperview();
			}
		}

		private void AnimateLiveRecordClock(bool isRunning)
		{
			if (isRunning)
			{
				_timer.Start();
			}
			else
			{
				_timer.Stop();
			}
		}

		private void InitTimerAnimation()
		{
			_animFrame = 1;
			_timer = new Timer();
			_timer.Elapsed += (sender, args) =>
			{
				InvokeOnMainThread ( () => {
					if (_animFrame > 11)
					{
						_animFrame = 0;
					}
					_clockImageView.Image = UIImage.FromFile(_clockFrameList[_animFrame]);
					_animFrame++;
				});
				
			};
			_timer.Interval = 1000;
		}
    }
}