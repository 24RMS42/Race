using System.IO;
using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Widget;
using MotionsRace.Core;
using MvvmCross.Droid.Views;
using MvvmCross.Plugins.Color.Droid;
using Xamarin;

namespace MotionsRace.Droid
{

	[Activity(
#if TWITCH
		Label = "Twitch Health Challenge"
#elif ATEA
		Label = "Sverigestafetten Atea"
#elif NORDEN
		Label = "Norden Activity Challenge"
#elif COROMATIC
		Label = "Coromatic Hälsoutmaningen"
#elif NETENT
		Label = "The Challenge"
#elif KRONOBERG
		Label = "Kronoberg"
#else
		Label = "MotionsRace"
#endif
        , MainLauncher = true
		, Theme = "@style/Theme.Splash"
		, NoHistory = true
		, ScreenOrientation = ScreenOrientation.Portrait)]
	public class SplashScreen : MvxSplashScreenActivity
	{
		public SplashScreen() : base(Resource.Layout.SplashScreen)
		{

		}

		protected override void OnCreate(Android.OS.Bundle bundle)
		{
			base.OnCreate(bundle);

			var splashLayout = FindViewById<LinearLayout>(Resource.Id.splashLayout);
			var logo = FindViewById<ImageView>(Resource.Id.imageView1);

			Color bgColor;
			Stream stream;

#if TWITCH
			bgColor = Constants.TWITCH_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.TWITCH_SPLASH_LOGO);
#elif ATEA
			bgColor = Constants.ATEA_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.ATEA_SPLASH_LOGO);
#elif NORDEN
			bgColor = Constants.NORDEN_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.NORDEN_SPLASH_LOGO);
#elif COROMATIC
			bgColor = Constants.COROMATIC_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.COROMATIC_SPLASH_LOGO);
#elif NETENT
			bgColor = Constants.NETENT_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.NETENT_SPLASH_LOGO);
#elif KRONOBERG
			bgColor = Constants.KRONOBERG_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.KRONOBERG_SPLASH_LOGO);
#else
			bgColor = Constants.MOTION_RACE_SPLASH_BACKGROUND_COLOR.ToAndroidColor();
			stream = Assets.Open(Constants.MOTION_RACE_SPLASH_LOGO);
#endif

            splashLayout.SetBackgroundColor(bgColor);
			logo.SetImageBitmap(BitmapFactory.DecodeStream(stream));

            // -------- Xamarin Insights integration START -----

            //Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
            //{
            //    if (isStartupCrash)
            //    {
            //        Insights.PurgePendingCrashReports().Wait();
            //    }
            //};

#if DEBUG
            //Insights.Initialize(Insights.DebugModeKey, this);
#elif TWITCH
			Insights.Initialize("a49b718ff6aaa7eacdd05ce6ba5121443216e229", this);
#else
			Insights.Initialize("e6c8308ae7e5f4841a77564c802066aa01b441bf", this);
#endif

            // -------- Xamarin Insights integration END -----

			UserDialogs.Init(this);
		}
	}
}