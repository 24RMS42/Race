using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Graphics.Drawables;
using MvvmCross.Plugins.Color.Droid;
using Android.Widget;
using Android.Graphics;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using MvvmCross.Platform;
using Android;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Linq;

namespace MotionsRace.Droid.Views
{

    [Activity(
        Theme = "@style/Theme.SignInView",
        NoHistory = false,
#if !ATEA
        FinishOnTaskLaunch = true,
#endif
        ScreenOrientation = ScreenOrientation.Portrait)]
    public class SignInView : BaseActivity<SignInViewModel>
    {
        protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.SignInView);
			var theme = Mvx.Resolve<IThemesManager>().CurrentTheme;

			var gdDefault = new GradientDrawable();
			gdDefault.SetColor(theme.Colors.LoginButtonColor.ToAndroidColor());
			gdDefault.SetCornerRadius(100);

			var imageViewHeder = FindViewById<ImageView>(Resource.Id.imageView1);

#if COROMATIC
			LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.MatchParent, LinearLayout.LayoutParams.MatchParent);
			lp.SetMargins(0, 0, 0, 0);
			imageViewHeder.LayoutParameters = lp;
#endif
			var bt = FindViewById<Button>(Resource.Id.customButton);
			bt.SetBackgroundDrawable(gdDefault);

			var password = FindViewById<EditText>(Resource.Id.textPassword);
			password.SetTypeface(Typeface.Default, TypefaceStyle.Normal);


#if ATEA
            var layoutEmail = FindViewById<LinearLayout>(Resource.Id.layoutEmail);
			var layoutPassword = FindViewById<LinearLayout>(Resource.Id.layoutPassword);
			var forgotPassword = FindViewById<TextView>(Resource.Id.forgotPassword);
			var customButton = FindViewById<TextView>(Resource.Id.customButton);

			layoutEmail.Visibility = Android.Views.ViewStates.Gone;
			layoutPassword.Visibility = Android.Views.ViewStates.Gone;
			forgotPassword.Visibility = Android.Views.ViewStates.Gone;
			customButton.Visibility = Android.Views.ViewStates.Gone;

			//// login OAuth panel
			var layoutOauth = FindViewById<LinearLayout>(Resource.Id.layoutOauth);
            layoutOauth.Visibility = Android.Views.ViewStates.Visible;

			//// close OAuth panel button
			var closeOAuthLink = FindViewById<TextView>(Resource.Id.closeOAuthLink);
			closeOAuthLink.Click += (sender, e) => 
            { 
                layoutOauth.Visibility = Android.Views.ViewStates.Gone;
				layoutEmail.Visibility = Android.Views.ViewStates.Visible;
				layoutPassword.Visibility = Android.Views.ViewStates.Visible;
				forgotPassword.Visibility = Android.Views.ViewStates.Visible;
				customButton.Visibility = Android.Views.ViewStates.Visible;
			};

			var buttonOAuth = FindViewById<Button>(Resource.Id.oauthButton);
            buttonOAuth.Text = string.Format(ViewModel["Login_SignInOAuth"], "ATEA");
            buttonOAuth.SetBackgroundDrawable(gdDefault);

			buttonOAuth.Click += async (sender, e) =>
			{
				var authContext = new AuthenticationContext(commonAuthority);

                authContext.TokenCache.Clear();

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
				catch { }
			};
#endif
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


        protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            AuthenticationAgentContinuationHelper.SetAuthenticationAgentContinuationEventArgs(requestCode, resultCode, data);
        }
	}
}