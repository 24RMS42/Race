using MvvmCross.Binding.BindingContext;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using MvvmCross.iOS.Views;
using MvvmCross.Plugins.Color.iOS;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Linq;
using System;

namespace MotionsRace.Touch.Views
{
    [Register("SignInView")]
    public class SignInView : MvxTableViewController<SignInViewModel>
    {
		UITextField _tfUsername;
		UITextField _tfPassword;

        public override void ViewDidLoad()
        {
            View = new UIView { BackgroundColor = UIColor.White };
            base.ViewDidLoad();

			Title = "";

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
               EdgesForExtendedLayout = UIRectEdge.None;
            }				

			var statusBarAndNavBarHeight = UIApplication.SharedApplication.StatusBarFrame.Height + this.NavigationController.NavigationBar.Bounds.Height;
			var backImageHeight = UIApplication.SharedApplication.StatusBarFrame.Height + this.NavigationController.NavigationBar.Bounds.Height + UIScreen.MainScreen.Bounds.Height;
			   
			// background image
			var backgroundImageView = new UIImageView (UIImage.FromBundle("SignInBackground.png"));
			backgroundImageView.Frame = new CGRect (0, -statusBarAndNavBarHeight, UIScreen.MainScreen.Bounds.Width, backImageHeight);
			backgroundImageView.ContentMode = UIViewContentMode.ScaleAspectFill;
			View.AddSubview(backgroundImageView);

			var logoHeight = 100;

			#if COROMATIC
			logoHeight = 230;
			#endif

			// logo image
			var logoImage = UIImage.FromFile("SignInLogo.png");
			var logoImageView = new UIImageView(new CGRect(0, 0, UIScreen.MainScreen.Bounds.Width, logoHeight));
			logoImageView.Image = logoImage;
			logoImageView.ContentMode = UIViewContentMode.ScaleAspectFit;
			View.AddSubview(logoImageView);

			// login panel
			var loginView = new UIView (new CGRect (20, UIScreen.MainScreen.Bounds.Height / 2 - 120, UIScreen.MainScreen.Bounds.Width - 40, 220));
			loginView.BackgroundColor = ViewModel.Theme.Colors.LoginPanelBackgroundColor.ToNativeColor();
			View.AddSubview(loginView);

			// welcome label
			var welcomeLabel = new UILabel(new CGRect(0, 10, UIScreen.MainScreen.Bounds.Width - 40, 25));
			welcomeLabel.TextColor = ViewModel.Theme.Colors.LoginWelcomeColor.ToNativeColor();
			welcomeLabel.TextAlignment = UITextAlignment.Center;
#if KRONOBERG
			welcomeLabel.Text = ViewModel["Login_WelcomeTo"] + " Grön Effekt";
#else
			welcomeLabel.Text = ViewModel ["Login_WelcomeTo"];
#endif

			loginView.AddSubview(welcomeLabel);

			var welcomeLabel2 = new UILabel(new CGRect(0, 30, UIScreen.MainScreen.Bounds.Width - 40, 25));
			welcomeLabel2.TextColor = ViewModel.Theme.Colors.LoginWelcomeColor.ToNativeColor();
			welcomeLabel2.TextAlignment = UITextAlignment.Center;
			welcomeLabel2.Text = ViewModel.Theme.Name;
			loginView.AddSubview(welcomeLabel2);

			var userImage = UIImage.FromFile("LoginUser.png");
			var userImageView = new UIImageView(new CGRect(0, 0, 24, 24));
			userImageView.Image = userImage;
			userImageView.ContentMode = UIViewContentMode.Center;

			// Email
			_tfUsername = new UITextField();
			_tfUsername.LeftViewMode = UITextFieldViewMode.Always;
			_tfUsername.LeftView = userImageView;
			_tfUsername.AdjustsFontSizeToFitWidth = true;
			_tfUsername.Placeholder = ViewModel["Login_Email"];
			_tfUsername.AttributedPlaceholder = new NSAttributedString(_tfUsername.Placeholder, foregroundColor: ViewModel.Theme.Colors.LoginTextBoxColor.ToNativeColor ());
			_tfUsername.KeyboardType = UIKeyboardType.EmailAddress;
			_tfUsername.AutocapitalizationType = UITextAutocapitalizationType.None;
			_tfUsername.ReturnKeyType = UIReturnKeyType.Next;
			_tfUsername.BorderStyle = UITextBorderStyle.RoundedRect;
			_tfUsername.Frame = new CGRect (10, 60, UIScreen.MainScreen.Bounds.Width - 60, 30);
			_tfUsername.BackgroundColor = ViewModel.Theme.Colors.LoginTextBoxBackgroundColor.ToNativeColor ().ColorWithAlpha(0.5f);
			_tfUsername.TextColor = ViewModel.Theme.Colors.LoginTextBoxColor.ToNativeColor ();
			_tfUsername.ShouldReturn += (textField) => { 
				_tfUsername.ResignFirstResponder();
				_tfPassword.BecomeFirstResponder();
				return true; 
			};
			loginView.AddSubview(_tfUsername);

			var passwordImage = UIImage.FromFile("LoginPassword.png");
			var passwordImageView = new UIImageView(new CGRect(0, 0, 24, 24));
			passwordImageView.Image = passwordImage;
			passwordImageView.ContentMode = UIViewContentMode.Center;

			// Password
			_tfPassword = new UITextField(new CGRect(10, 100, UIScreen.MainScreen.Bounds.Width - 60, 30));
			_tfPassword.LeftViewMode = UITextFieldViewMode.Always;
			_tfPassword.LeftView = passwordImageView;
			_tfPassword.AdjustsFontSizeToFitWidth = true;
			_tfPassword.Placeholder = ViewModel["Login_Password"];
			_tfPassword.AttributedPlaceholder = new NSAttributedString(_tfPassword.Placeholder, foregroundColor:  ViewModel.Theme.Colors.LoginTextBoxColor.ToNativeColor ());
			_tfPassword.KeyboardType = UIKeyboardType.EmailAddress;
			_tfPassword.ReturnKeyType = UIReturnKeyType.Done;
			_tfPassword.SecureTextEntry = true;
			_tfPassword.BorderStyle = UITextBorderStyle.RoundedRect;
			_tfPassword.BackgroundColor = ViewModel.Theme.Colors.LoginTextBoxBackgroundColor.ToNativeColor ().ColorWithAlpha(0.5f);
			_tfPassword.TextColor = ViewModel.Theme.Colors.LoginTextBoxColor.ToNativeColor ();
			_tfPassword.ShouldReturn += (textField) => { 
				_tfPassword.ResignFirstResponder();
				return true;
			};
			loginView.AddSubview(_tfPassword);

			// login button
			var btnSignIn = new UIButton(UIButtonType.RoundedRect);
			btnSignIn.Frame = new CGRect(10, 140, UIScreen.MainScreen.Bounds.Width - 60, 30);
			btnSignIn.SetTitle (ViewModel["Login_SignIn"], UIControlState.Normal); 
			btnSignIn.SetTitleColor (UIColor.White, UIControlState.Normal);
			btnSignIn.BackgroundColor = ViewModel.Theme.Colors.LoginButtonColor.ToNativeColor ();
			btnSignIn.Layer.CornerRadius = 10;
			loginView.AddSubview(btnSignIn);

			// forgot password button
			var btnForgotPassword = new UIButton(UIButtonType.RoundedRect);
			btnForgotPassword.Frame = new CGRect(10, 180, 0, 30);
			btnForgotPassword.SetTitle (ViewModel["Login_ForgotPassword"], UIControlState.Normal); 
			btnForgotPassword.SetTitleColor (ViewModel.Theme.Colors.LoginWelcomeColor.ToNativeColor(), UIControlState.Normal);
			btnForgotPassword.SizeToFit ();
			loginView.AddSubview(btnForgotPassword);

			// version label
			var versionLabel = new UILabel(new CGRect(10, UIScreen.MainScreen.Bounds.Height - statusBarAndNavBarHeight - 30, UIScreen.MainScreen.Bounds.Width - 20, 25));
			versionLabel.TextColor = UIColor.White;
			versionLabel.TextAlignment = UITextAlignment.Right;
			versionLabel.Font = UIFont.FromName("HelveticaNeue-Light", 12f);
			versionLabel.Text = ViewModel.AppVersion;
			View.AddSubview(versionLabel);

			var loadingOverlay = new LoadingOverlay (UIScreen.MainScreen.Bounds);
			loadingOverlay.AutoresizingMask = UIViewAutoresizing.FlexibleTopMargin | UIViewAutoresizing.FlexibleBottomMargin | UIViewAutoresizing.FlexibleLeftMargin | UIViewAutoresizing.FlexibleRightMargin;
			View.AddSubview (loadingOverlay);

            var set = this.CreateBindingSet<SignInView, Core.ViewModels.SignInViewModel>();
			set.Bind(_tfUsername).To(vm => vm.UserName);
			set.Bind(_tfPassword).To(vm => vm.Password);
#if ATEA
			// login OAuth panel
			var oAuthView = new UIView(new CGRect(0, 60, loginView.Frame.Width, loginView.Frame.Height - 60));
			oAuthView.BackgroundColor = ViewModel.Theme.Colors.ActivitySaveButtonColor.ToNativeColor();
			loginView.AddSubview(oAuthView);

            // close OAuth panel button
            var btnCloseOAuth = new UIButton(UIButtonType.RoundedRect);
            btnCloseOAuth.Frame = new CGRect(oAuthView.Frame.Width - 30, 0, 30, 30);
            btnCloseOAuth.SetTitle("X", UIControlState.Normal);
            btnCloseOAuth.SetTitleColor(UIColor.White, UIControlState.Normal);
            btnCloseOAuth.BackgroundColor = UIColor.Clear;
            btnCloseOAuth.Layer.CornerRadius = 0;
            oAuthView.AddSubview(btnCloseOAuth);
            btnCloseOAuth.TouchUpInside += (sender, e) => { oAuthView.Hidden = true; };

			// login OAuth button
			var btnSignInOAuth = new UIButton(UIButtonType.RoundedRect);
			btnSignInOAuth.Frame = new CGRect(10, 65, UIScreen.MainScreen.Bounds.Width - 60, 30);
            btnSignInOAuth.SetTitle(string.Format(ViewModel["Login_SignInOAuth"], "ATEA"), UIControlState.Normal);
			btnSignInOAuth.SetTitleColor(UIColor.White, UIControlState.Normal);
			btnSignInOAuth.BackgroundColor = ViewModel.Theme.Colors.LoginButtonColor.ToNativeColor();
			btnSignInOAuth.Layer.CornerRadius = 10;
			oAuthView.AddSubview(btnSignInOAuth);

			btnSignInOAuth.TouchUpInside += async (sender, e) =>
			{
				var authContext = new AuthenticationContext(commonAuthority);
				if (authContext.TokenCache.ReadItems().Count() > 0)
					authContext = new AuthenticationContext(authContext.TokenCache.ReadItems().First().Authority);

                try
                {
                    authResult = await authContext.AcquireTokenAsync(graphResourceUri, clientId, returnUri, new PlatformParameters(this));

                    if (authResult.AccessToken != null)
                    {
                        this.ViewModel.UserName = authResult.UserInfo.DisplayableId;
                        this.ViewModel.Password = authResult.UserInfo.UniqueId;
                        this.ViewModel.SignInByOAuthAsync();
                    }
                }
                catch {}
			};
#endif
            set.Bind(btnSignIn).To(vm => vm.SignInCommand);
			set.Bind(btnForgotPassword).To(vm => vm.ForgotPassword);
			set.Bind (loadingOverlay).For(v => v.IsVisible).To(vm => vm.IsBusy);
            set.Apply();
        }

		//Client ID from from step 1. point 6
		public static string clientId = "3ae8aa99-ea79-4081-b5e0-9b287b138e72";
		public static string commonAuthority = "https://login.windows.net/common";
		//Redirect URI from step 1. point 5<br />
		public static Uri returnUri = new Uri("http://sverigestafetten-mobile-redirect");
		//Graph URI if you've given permission to Azure Active Directory in step 1. point 6
		const string graphResourceUri = "https://graph.windows.net";
		public static string graphApiVersion = "2013-11-08";
		//AuthenticationResult will hold the result after authentication completes
		AuthenticationResult authResult = null;


		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);

			this.NavigationController.SetNavigationBarHidden(false, true);
			this.NavigationController.NavigationBar.BarStyle = UIBarStyle.BlackTranslucent	;
			this.NavigationController.NavigationBar.TintColor = UIColor.White;

			this.NavigationController.NavigationBar.Translucent = true;
			this.NavigationController.NavigationBar.ShadowImage = new UIImage ();
			this.NavigationController.NavigationBar.SetBackgroundImage (new UIImage (), UIBarMetrics.Default);

			if (this.NavigationController.NavigationBar.BackItem != null)
				this.NavigationController.NavigationBar.BackItem.Title = "";
		}
			
    }
}