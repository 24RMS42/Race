using System.Linq;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using MotionsRace.Core;
using System.Drawing;
using System;
using Xamarin;
using System.Collections.Generic;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.iOS.Views;
using MvvmCross.Binding.BindingContext;
using MvvmCross.Plugins.Color.iOS;
using MvvmCross.Platform;

namespace MotionsRace.Touch.Views
{
	[Register("FrequentTrainingView")]
	public class FrequentTrainingView : MvxViewController<FrequentTrainingViewModel>
    {
		private UIBarButtonItem _loginButton;
		private UITableView _itemsTable;
		private UIView _headerBorder;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

			var backgroundColor = ViewModel.Theme.Colors.ActivityTypesPanelColor.ToNativeColor ();
			View = new UIView { BackgroundColor = backgroundColor };

			this.Title = " ";

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

			/*
			var button = new UIButton ();
				button.Frame = new CGRect(0.0f, 0.0f, 32.0f, 32.0f);
			button.SetTitle ("<", UIControlState.Normal);
			//button.TitleLabel.Font = UIFont.(20.0f);
			button.TitleLabel.ShadowColor = UIColor.Black;
			button.TitleLabel.ShadowOffset = new CGSize(0.0f, -1.0f);
			//button addTarget:target action:action forControlEvents:UIControlEventTouchUpInside];
			//button.showsTouchWhenHighlighted = YES;
			var barButtonItem = new UIBarButtonItem(button);

			// back button
			var backButton = new UIBarButtonItem ("❮", UIBarButtonItemStyle.Plain, (sender, args) => {
				var vm = ViewModel as ActivityTypesViewModel;
				if (vm.CanOnBackShowCategories)
					vm.OnBackShowCategories();
				else
					vm.CloseViewModel();
			});
		
			this.NavigationItem.SetHidesBackButton (true, false);
			this.NavigationItem.LeftBarButtonItem = backButton;
			*/

			// header panel
			var headerView = new UIView (new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width , 40));
			headerView.BackgroundColor = ViewModel.Theme.Colors.PageTitleColor.ToNativeColor ();
			View.AddSubview(headerView);

			// title label
			var titleLabel = new UILabel(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, 40));
			titleLabel.TextColor =  ViewModel.Theme.Colors.TextPageTitleColor.ToNativeColor();
			titleLabel.TextAlignment = UITextAlignment.Center;
			titleLabel.Text = ViewModel["Activity_RegisterFavotieActivity"];
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


			// ITEMS
			_itemsTable = new UITableView (new CGRect(0, 42, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.ApplicationFrame.Height - (42 + this.NavigationController.NavigationBar.Bounds.Height) ));
			_itemsTable.BackgroundColor = ViewModel.Theme.Colors.ActivityTypesPanelColor.ToNativeColor ();
			_itemsTable.SeparatorStyle = UITableViewCellSeparatorStyle.None;
			_itemsTable.RowHeight = 60;
			var itemsSource = new FavItemsSource(_itemsTable, this);
			itemsSource.SelectedItemChanged  += (s, e) =>
			{
				try
				{
					var item = (GetTrainingTypesResult)itemsSource.SelectedItem;
					ViewModel.TrainingCategoryItemsSelected = item;
					ViewModel.TrainingCategoryItemsSelectedCommand.Execute(item);
				}
				catch(Exception exception)
				{
					Insights.Report(exception, new Dictionary <string, string> { 
						{"info", "activitytypesview,itemsSource.SelectedItemChanged"}
					}, Xamarin.Insights.Severity.Error);
				}

				//itemsSource.RowUnhighlighted(_itemsTable, itemsSource.)

				/*
				// remove current view from back stack
				this.NavigationController.ViewControllers[this.NavigationController.ViewControllers.Length - 2].WillMoveToParentViewController(null);
				this.NavigationController.ViewControllers[this.NavigationController.ViewControllers.Length - 2].View.RemoveFromSuperview();
				//NavigationController.ViewControllers[this.NavigationController.ViewControllers.Length - 2].NavigationController.PopViewController(false);
				this.NavigationController.ViewControllers[this.NavigationController.ViewControllers.Length - 2].RemoveFromParentViewController();
				*/
			};
			_itemsTable.Source = itemsSource;
			View.AddSubview (_itemsTable);

			var set = this.CreateBindingSet<FrequentTrainingView, Core.ViewModels.FrequentTrainingViewModel>();
			set.Bind(_loginButton).For(x => x.Enabled).To(v => v.ShowLoginButton);
			set.Bind(itemsSource).To(v => v.TrainingCategoryItems);
			set.Bind(btnHeaderLogo).To(vm => vm.TapLogoCommand);
            set.Apply();
        }

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden(false, true);
			this.NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

			this.NavigationController.NavigationBar.TintColor = UIColor.White;
			this.NavigationController.NavigationBar.BarTintColor = ViewModel.Theme.Colors.HeaderColor.ToNativeColor ();

			_itemsTable.DeselectRow (_itemsTable.IndexPathForSelectedRow, false);

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
    }

	public class FavItemsSource : MvxTableViewSource
	{
		private readonly FrequentTrainingView _view;

		public FavItemsSource(UITableView tableView, FrequentTrainingView view)
			: base(tableView)
		{
			_view = view;
			tableView.RegisterClassForCellReuse(typeof(ItemTrainingCell), FavItemTrainingCell.Key);
		}

		protected override UITableViewCell GetOrCreateCellFor(UITableView tableView, NSIndexPath indexPath, object item)
		{
			return tableView.DequeueReusableCell(FavItemTrainingCell.Key, indexPath);
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			base.RowSelected(tableView, indexPath);
		}
	}


	public class FavTypesLineSource : MvxCollectionViewSource
	{
		private readonly ActivityTypesView _view;

		public FavTypesLineSource (UICollectionView collection, ActivityTypesView view)
			:base(collection)
		{
			_view = view;
			this.CollectionView.RegisterClassForCell(typeof(FavTypeLineCell), FavTypeLineCell.Key);
		}

		protected override UICollectionViewCell GetOrCreateCellFor (UICollectionView collectionView, NSIndexPath indexPath, object item)
		{
			return (UICollectionViewCell)this.CollectionView.DequeueReusableCell(FavTypeLineCell.Key, indexPath);
		}
	}


	[Register("FavTypeLineCell")]
	public class FavTypeLineCell : MvxCollectionViewCell
	{
		public static readonly NSString Key = new NSString("FavTypeLineCell");

		private MvxImageView _image;
		private MvxImageView _imageSelected;

		public FavTypeLineCell(IntPtr handle)
			: base(handle)
		{
			_imageSelected = new MvxImageView(ContentView.Bounds);
			_imageSelected.ContentMode = UIViewContentMode.ScaleAspectFit;
			ContentView.AddSubview(_imageSelected);

			_image = new MvxImageView(ContentView.Bounds);
			_image.ContentMode = UIViewContentMode.ScaleAspectFit;
			ContentView.AddSubview(_image);

			this.DelayBind(() => {
				var set = this.CreateBindingSet<FavTypeLineCell, TrainingCategory>();
				set.Bind(this).For(x => x.BackgroundColor).To(category => category.BackGroundColor).WithConversion("NativeColor");
				set.Bind(_image).For(x => x.Image).To (category => category.IconFile).WithConversion(new CategoryImagePathValueConverter());
				set.Bind(_image).For("Visibility").To(category => category.IsSelected).WithConversion("InvertedVisibility");
				set.Bind(_imageSelected).For(x => x.Image).To (category => category.IconFileSelected).WithConversion(new CategoryImagePathValueConverter());
				set.Bind(_imageSelected).For("Visibility").To(category => category.IsSelected).WithConversion("Visibility");
				set.Bind(_imageSelected).For("Visibility").To(category => category.IsSelected).WithConversion("Visibility");

				set.Apply ();
			});
		}
	}
		

	[Register("FavItemTrainingCell")]
	public class FavItemTrainingCell : MvxTableViewCell
	{
		public static readonly NSString Key = new NSString("ItemTrainingCell");
		private nfloat Width = UIScreen.MainScreen.Bounds.Width;

		private UILabel _labelUnit;
		private UILabel _labelText;
		private UILabel _labelDescr;
		private UIView _bottomLine;
		private UIView _clockView;
		private UIButton _clockButton;
		private MvxImageView _clockImage;

		public FavItemTrainingCell(IntPtr handle)
			: base(handle)
		{
			var theme = Mvx.Resolve<IThemesManager> ();

			this.Frame = new CGRect (0, 0, Width, 60);

			_labelText = new UILabel(new CGRect(10, 5, Width - 80, this.Frame.Height /2));
			_labelText.TextColor = theme.CurrentTheme.Colors.ActivityItemTextColor.ToNativeColor();
			_labelText.Font = UIFont.FromName("HelveticaNeue-Light", 14f);
			_labelText.TextAlignment = UITextAlignment.Left;
			_labelText.LineBreakMode = UILineBreakMode.TailTruncation;
			Add(_labelText);

			_labelDescr = new UILabel(new CGRect(10, this.Frame.Height /2 - 3, Width - 80, this.Frame.Height /2));
			_labelDescr.TextColor = theme.CurrentTheme.Colors.ActivityItemTextColor.ToNativeColor();
			_labelDescr.Font = UIFont.FromName("HelveticaNeue-Light", 10f);
			_labelDescr.TextAlignment = UITextAlignment.Left;
			_labelDescr.LineBreakMode = UILineBreakMode.TailTruncation;
			_labelDescr.Lines = 2;
			Add(_labelDescr);


			var rightView = new UIView (new CGRect (this.Frame.Width - 60, 0, 60, this.Frame.Height - 2));

			_labelUnit = new UILabel(new CGRect(-10, 0, 60, this.Frame.Height - 2));
			_labelUnit.TextColor = theme.CurrentTheme.Colors.TrainingItemPointsColor.ToNativeColor();
			_labelUnit.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
			_labelUnit.TextAlignment = UITextAlignment.Center;
			_labelUnit.LineBreakMode = UILineBreakMode.TailTruncation;
			_labelUnit.Lines = 2;
			rightView.AddSubview (_labelUnit);

			_clockView = new UIView (new CGRect (0, 0, 60, this.Frame.Height - 2));
			_clockView.BackgroundColor = theme.CurrentTheme.Colors.ClockBackGroundColor.ToNativeColor();

			var imgView = new MvxImageView(new CGRect(0, 0, 60, this.Frame.Height - 2));
			imgView.Image = UIImage.FromFile("clock_icon_white.png");
			imgView.ContentMode = UIViewContentMode.Center;
			_clockView.AddSubview (imgView);


			_clockButton = new UIButton(new CGRect (0, 0, 60, this.Frame.Height - 2));
			_clockButton.BackgroundColor = UIColor.Clear;

			_clockView.AddSubview (_clockButton);

			rightView.AddSubview (_clockView);

			AddSubview (rightView);
			//this.AccessoryView = rightView;

			var selectedView = new UIView (new CGRect(0, this.Frame.Height - 2, Width, 2));
			selectedView.BackgroundColor = theme.CurrentTheme.Colors.ActivitySaveButtonPressedColor.ToNativeColor();

			this.SelectedBackgroundView = selectedView;

			_bottomLine = new UIView (new CGRect(0, this.Frame.Height - 2, Width, 2));
			_bottomLine.BackgroundColor = theme.CurrentTheme.Colors.ActivityTypesPanelColor.ToNativeColor();
			Add (_bottomLine);

			this.DelayBind(() =>
				{
					var set = this.CreateBindingSet<FavItemTrainingCell, GetTrainingTypesResult>();
					set.Bind(_labelText).To(vm => vm.TrainingTypeName);
					set.Bind(_labelText).For(x => x.Frame).To(vm => vm.DescriptionVisibility).WithConversion(new TypeNameFrameValueConverter(), this.Frame);
					set.Bind(_labelDescr).To(vm => vm.Description);
					set.Bind(_labelUnit).To(vm => vm.UnitString);
					set.Bind(_clockView).For("Visibility").To(vm => vm.LiveRecordVisibility).WithConversion("Visibility");
					set.Bind(_clockButton).To(vm => vm.TrainingCategoryItemsSelectedCommand);
					set.Apply();
				});
		}

		public override UILabel DetailTextLabel {
			get {
				return base.DetailTextLabel;
			}
		}
	}
}