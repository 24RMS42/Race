using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Text.Method;
using Android.Views;
using Android.Widget;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using Android.Support.V4.Widget;
using MvvmCross.Platform;
using MvvmCross.Plugins.Color.Droid;
using Android.Util;
using Java.Lang;

namespace MotionsRace.Droid.Views
{
	[Activity(
		Theme = "@style/Theme.MainView",
		LaunchMode = LaunchMode.SingleTask,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class MainView : BaseActivity<MainViewModel>
	{
		private Button _buttonRecordActivity;
		private ListView _mvxListView;

	    public static Bitmap RotatetImage;

		public int dpToPx(int dp)
		{
			DisplayMetrics displayMetrics = this.Resources.DisplayMetrics;
			int px = Math.Round(dp * (displayMetrics.Xdpi / (int)DisplayMetricsDensity.Default));
			return px;
		}

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.MainView);


			var imageViewHeder = FindViewById<ImageView>(Resource.Id.imageViewHeder);

//#if NORDEN
//			LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.MatchParent);
//			lp.SetMargins(0, 0, 0, 0); ;
//			imageViewHeder.LayoutParameters = lp;
//#elif NETENT
//			LinearLayout.LayoutParams lp = new LinearLayout.LayoutParams(LinearLayout.LayoutParams.WrapContent, LinearLayout.LayoutParams.MatchParent);
//			lp.SetMargins(0, 0, 0, 0); ;
//			imageViewHeder.LayoutParameters = lp;
//#endif

			_buttonRecordActivity = FindViewById<Button>(Resource.Id.buttonRecordActivity);
			_mvxListView = FindViewById<ListView>(Resource.Id.mvx_MvxListView2);
			_buttonRecordActivity.Click +=
				(sender, e) =>
				{
					if (Mvx.Resolve<IPlatformService>().IsInternetAvailable())
					{
						_buttonRecordActivity.Enabled = false;
					}
				};

			var swipeRefreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.swipeRefreshLayout);
			swipeRefreshLayout.SetColorSchemeColors(ViewModel.Theme.Colors.LoadingIndicatorColor.ToAndroidColor());

			swipeRefreshLayout.Refresh += async delegate
			{
				if (Mvx.Resolve<IPlatformService>().IsInternetAvailable())
				{
					await ViewModel.GetMyFeeds();
				}
				swipeRefreshLayout.Refreshing = false;
			};

			// To can open HTML link's like <a href=www.google,com>google</a>
			var tvFullMessage = FindViewById<TextView>(Resource.Id.tvFullMessage);
			tvFullMessage.MovementMethod = LinkMovementMethod.Instance;

            var feedImage = FindViewById<ImageView>(Resource.Id.feedImage);
		    feedImage.Click += (sender, args) =>
		    {
		        Bitmap bitmap = ((BitmapDrawable) feedImage.Drawable).Bitmap;
		        if (bitmap.Width > bitmap.Height)
		        {
                    Matrix mat = new Matrix();
                    mat.PostRotate(90);
                    RotatetImage = Bitmap.CreateBitmap(bitmap, 0, 0, bitmap.Width, bitmap.Height, mat, true);
		        }
		        else
		        {
                    RotatetImage = bitmap;
		        }
		        ViewModel.ShowDetailFullFeedImage.Execute();
		    };
		}

		protected override async void OnResume()
		{
			base.OnResume();
			if (_buttonRecordActivity.Enabled == false)
			{
				if (Mvx.Resolve<IPlatformService>().IsInternetAvailable())
				{
					ViewModel.IsBusy = true;
					await ViewModel.GetMyFeeds();
					ViewModel.IsBusy = false;
				}
			}
			_mvxListView.SmoothScrollToPosition(0);
			_buttonRecordActivity.Enabled = true;

			ViewModel.CheckLiveRegister();
		}

		public override void OnBackPressed()
		{
			ViewModel.ExitCommand.Execute();
		}

		public override bool OnKeyDown(Keycode keyCode, KeyEvent e)
		{
			if (keyCode == Keycode.Menu)
			{
				ViewModel.ShowMenuCommand.Execute();
			}
			return base.OnKeyDown(keyCode, e);
		}
	}
}