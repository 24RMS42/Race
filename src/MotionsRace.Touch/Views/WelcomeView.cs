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

namespace MotionsRace.Touch.Views
{
    [Register("WelcomeView")]
    public class WelcomeView : MvxViewController<WelcomeViewModel>
    {
        public override void ViewDidLoad()
        {
			
            base.ViewDidLoad();

			View = new UIView { BackgroundColor = ViewModel.Theme.Colors.SplashColor.ToNativeColor() };

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
               EdgesForExtendedLayout = UIRectEdge.None;
            }

			var size = new CGRect (0, 0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

			var image1 = new UIImageView (UIImage.FromBundle("Slide1.png"));
			var image2 = new UIImageView (UIImage.FromBundle("Slide2.png"));
			var image3 = new UIImageView (UIImage.FromBundle("Slide3.png"));

			var images = new UIImageView[] { image1, image2, image3 };

			var scrollView = new UIScrollView (size);
			scrollView.PagingEnabled = true;
			scrollView.IndicatorStyle = UIScrollViewIndicatorStyle.White;

			int numberOfViews = 3;
			for (int i = 0; i < numberOfViews; i++)
			{
				var xOrigin = i * UIScreen.MainScreen.Bounds.Width;
				images[i].ContentMode = UIViewContentMode.ScaleAspectFill;
				images[i].Frame = new CGRect (0,0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

				var view = new UIView (size);
				view.Frame = new CGRect (xOrigin,0, UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);
				view.AddSubview (images [i]);

				var yMessagePosition = UIScreen.MainScreen.Bounds.Height / 2 - 25;
#if TWITCH
				yMessagePosition = UIScreen.MainScreen.Bounds.Height / 2 + 5;
#endif

				// label

				var titleTextheight = 60;

				#if NORDEN
				titleTextheight = 90; // 3 lines
				#endif

				var titleLabel = new UILabel(new CGRect (10, yMessagePosition, UIScreen.MainScreen.Bounds.Width - 20, titleTextheight));
				titleLabel.TextColor =  UIColor.White;
				titleLabel.BackgroundColor = UIColor.FromRGBA (0, 0, 0, 125);
				titleLabel.TextAlignment = UITextAlignment.Center;
                titleLabel.Text = ViewModel.WelcomeText;
                titleLabel.Font = UIFont.FromName("HelveticaNeue-Light", 18f);
				titleLabel.LineBreakMode = UILineBreakMode.WordWrap;
				titleLabel.Lines = 0;
				//titleLabel.ShadowOffset = new CGSize (0, 1);
				//titleLabel.ShadowColor = UIColor.Black;
				//titleLabel.SizeToFit ();
				view.AddSubview(titleLabel);


				scrollView.AddSubview (view);
			}

			// Set the contentSize equal to the size of the UIImageView
			scrollView.ContentSize = new CGSize(numberOfViews * UIScreen.MainScreen.Bounds.Width, UIScreen.MainScreen.Bounds.Height);

			View.AddSubview (scrollView);

			var pageControl = new UIPageControl (new CGRect(0,UIScreen.MainScreen.Bounds.Height - 150, UIScreen.MainScreen.Bounds.Width, 50));
			pageControl.AutoresizingMask = UIViewAutoresizing.None;
			pageControl.CurrentPageIndicatorTintColor = ViewModel.Theme.Colors.WelcomeSliderIndicatorSelectorColor.ToNativeColor ();
			pageControl.PageIndicatorTintColor = ViewModel.Theme.Colors.WelcomeSliderIndicatorNotSelectorColor.ToNativeColor ();
			pageControl.Pages = 3;
			pageControl.CurrentPage = 0;
			View.AddSubview (pageControl);

			scrollView.Scrolled += (sender, e) => 
			{
				var pageWidth = scrollView.Frame.Width;
				var page = (scrollView.ContentOffset.X - pageWidth / 2 ) / pageWidth + 1; //this provide you the page number
				pageControl.CurrentPage = (int)page;// 
			};

			var btnSignUp = new UIButton(UIButtonType.RoundedRect);
			btnSignUp.Frame = new CGRect(0, UIScreen.MainScreen.Bounds.Height - 100, UIScreen.MainScreen.Bounds.Width /2, 100);
			btnSignUp.SetTitle (ViewModel["Welcome_SignUp"], UIControlState.Normal); 
			btnSignUp.TitleLabel.Font = UIFont.SystemFontOfSize (24);
			btnSignUp.SetTitleColor(ViewModel.Theme.Colors.WelcomeSignUpTextColor.ToNativeColor(), UIControlState.Normal);
			btnSignUp.BackgroundColor = ViewModel.Theme.Colors.WelcomeSignUpColor.ToNativeColor ();
			Add(btnSignUp);

			//<key>ITSAppUsesNonExemptEncryption</key><false/>
			var mustSignInBeHidden = DateTime.Now.Date < new DateTime (2016, 6, 20);

//#if NORDEN || COROMATIC || NETENT || KRONOBERG ||
			mustSignInBeHidden = true;
//#endif

			var btnSignIn = new UIButton(UIButtonType.RoundedRect);
			btnSignIn.Frame = new CGRect(mustSignInBeHidden ? 0 : UIScreen.MainScreen.Bounds.Width /2, UIScreen.MainScreen.Bounds.Height - 100, mustSignInBeHidden ? UIScreen.MainScreen.Bounds.Width : UIScreen.MainScreen.Bounds.Width / 2, 100);
			btnSignIn.SetTitle (ViewModel["Welcome_SignIn"], UIControlState.Normal); 
			btnSignIn.TitleLabel.Font = UIFont.SystemFontOfSize (24);
			btnSignIn.SetTitleColor(ViewModel.Theme.Colors.WelcomeSignInTextColor.ToNativeColor (), UIControlState.Normal);
			btnSignIn.BackgroundColor = ViewModel.Theme.Colors.WelcomeSignInColor.ToNativeColor ();
			Add(btnSignIn);

            var set = this.CreateBindingSet<WelcomeView, Core.ViewModels.WelcomeViewModel>();
			set.Bind(btnSignUp).To(vm => vm.SignUpCommand);
			set.Bind(btnSignIn).To(vm => vm.SignInCommand);
            set.Apply();
        }

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			NavigationController.SetNavigationBarHidden(true, true);
		}

		public override UIStatusBarStyle PreferredStatusBarStyle ()
		{
			return UIStatusBarStyle.LightContent;
		}

		public static System.DateTime CompileDate
		{
			get
			{
				if (!compileDate.HasValue)
					compileDate = RetrieveLinkerTimestamp(Assembly.GetExecutingAssembly().Location);
				return compileDate ?? new System.DateTime();
			}
		}
		private static System.DateTime? compileDate;

		/// <summary>
		/// Retrieves the linker timestamp.
		/// </summary>
		/// <param name="filePath">The file path.</param>
		/// <returns></returns>
		/// <remarks>http://www.codinghorror.com/blog/2005/04/determining-build-date-the-hard-way.html</remarks>
		private static System.DateTime RetrieveLinkerTimestamp(string filePath)
		{
			const int peHeaderOffset = 60;
			const int linkerTimestampOffset = 8;
			var b = new byte[2048];
			System.IO.FileStream s = null;
			try
			{
				s = new System.IO.FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);
				s.Read(b, 0, 2048);
			}
			finally
			{
				if(s != null)
					s.Close();
			}
			var dt = new System.DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(System.BitConverter.ToInt32(b, System.BitConverter.ToInt32(b, peHeaderOffset) + linkerTimestampOffset));
			return dt.AddHours(System.TimeZone.CurrentTimeZone.GetUtcOffset(dt).Hours);
		}
    }
}