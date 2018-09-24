using MvvmCross.Binding.BindingContext;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using System;
using System.Reflection;
using System.IO;
using MvvmCross.iOS.Views;
using MvvmCross.Plugins.Color.iOS;
using MvvmCross.Binding.iOS.Views;

namespace MotionsRace.Touch.Views
{
    //[Register("WebView")]
    public class WebView : MvxViewController<WebViewModel>
    {
        public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var backgroundColor = ViewModel.Theme.Colors.ActivityTypesPanelColor.ToNativeColor();
			View = new UIView { BackgroundColor = backgroundColor };

			this.Title = " ";


#if NETENT || KRONOBERG
            var headerTopView = new UIView(new CGRect(0, 0, 80, 40));
#else
			// header top logo
			var headerTopView = new UIView(new CGRect(0, 0, 40, 40));
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

			var statusBarAndNavBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height + this.NavigationController.NavigationBar.Bounds.Height;

			var size = new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height - statusBarAndNavBarHeight);

			var _webView = new UIWebView(size);
            _webView.ScalesPageToFit = true;
            View.AddSubview(_webView);

            _webView.LoadRequest(new NSUrlRequest(new NSUrl(ViewModel.Url)));

			var set = this.CreateBindingSet<WebView, Core.ViewModels.WebViewModel>();
            //set.Bind(gridSource).To(v => v.TrainingCategories);
			set.Apply();
		}

        public override void ViewWillAppear(bool animated)
        {
			try
			{
				base.ViewWillAppear(animated);
                this.NavigationController.NavigationBar.Translucent = false;
				this.NavigationController.SetNavigationBarHidden(false, true);
				this.NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

				this.NavigationController.NavigationBar.TintColor = UIColor.White;
				this.NavigationController.NavigationBar.BarTintColor = ViewModel.Theme.Colors.HeaderColor.ToNativeColor();
			}
			catch (Exception exception)
			{
				
			}
        }
    }
}
