using System;
using MotionsRace.Core.ViewModels;
using Foundation;
using UIKit;
using ObjCRuntime;
using CoreGraphics;
using MvvmCross.iOS.Views;
using MvvmCross.Binding.iOS.Views;

namespace MotionsRace.Touch.Views
{
	[Register("ImageViewerView")]
	public class ImageViewerView : MvxViewController<ImageViewerViewModel>
	{
		private UIScrollView _scrollView;

		public override void ViewDidLoad()
		{

			base.ViewDidLoad();

			View = new UIView { BackgroundColor = UIColor.Black };

			// ios7 layout
			if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
			{
				EdgesForExtendedLayout = UIRectEdge.None;
			}

			var statusBarAndNavBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height + this.NavigationController.NavigationBar.Bounds.Height;

			var sizeScroll = new CGRect (0, -statusBarAndNavBarHeight, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

			_scrollView = new UIScrollView (sizeScroll);
			_scrollView.ContentSize = new CGSize(sizeScroll.Width, sizeScroll.Height);

			// background image
			var backgroundImageView = new MvxImageView ();
			backgroundImageView.Frame = new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);;
			backgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFit;

			if (MainView.RotatedImage != null)
				backgroundImageView.Image = MainView.RotatedImage;
			else
				backgroundImageView.ImageUrl = ViewModel.ImageURL;

			_scrollView.AddSubview(backgroundImageView);

			_scrollView.MaximumZoomScale = 3f;
			_scrollView.MinimumZoomScale = 1f;
			_scrollView.ViewForZoomingInScrollView += (UIScrollView sv) => { return backgroundImageView; };

			View.AddSubview(_scrollView);
		}
			
		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.NavigationController.SetNavigationBarHidden(false, true);
			this.NavigationController.NavigationBar.BarStyle = UIBarStyle.BlackTranslucent;
			this.NavigationController.NavigationBar.TintColor = UIColor.White;

			this.NavigationController.NavigationBar.Translucent = true;
			this.NavigationController.NavigationBar.ShadowImage = new UIImage ();
			this.NavigationController.NavigationBar.SetBackgroundImage (new UIImage (), UIBarMetrics.Default);

			if (this.NavigationController.NavigationBar.BackItem != null)
				this.NavigationController.NavigationBar.BackItem.Title = "";
		}
	}
}

