using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using MvvmCross.Plugins.Color.Droid;
using MotionsRace.Droid.Adapters;
using MotionsRace.Core.ViewModels;
using Android.Graphics;
using MotionsRace.Core.Services;
using MotionsRace.Droid.Controls;
using MotionsRace.Droid.Helpers;
using MvvmCross.Platform;
using Android.Webkit;

namespace MotionsRace.Droid.Views
{
	[Activity(
        Theme = "@style/Theme.MainView",
        NoHistory = false,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class WebView : BaseActivity<WebViewModel>
	{
        Android.Webkit.WebView web_view;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.WebView);

            var leftPanel = this.FindViewById(Resource.Id.leftPanel);
            leftPanel.Visibility = Android.Views.ViewStates.Invisible;

			var rightPanel = this.FindViewById(Resource.Id.rightPanel);
			rightPanel.Visibility = Android.Views.ViewStates.Invisible;

			web_view = this.FindViewById<Android.Webkit.WebView>(Resource.Id.webview);
			web_view.SetWebViewClient(new WebViewClient());
			web_view.Settings.JavaScriptEnabled = true;
			web_view.LoadUrl(ViewModel.Url);
		}
	}
}