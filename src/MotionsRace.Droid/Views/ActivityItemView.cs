using Android.App;
using Android.Content.PM;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Views;
using Android.Views.Animations;
using Android.Views.InputMethods;
using Android.Widget;
using MotionsRace.Core.ViewModels;
using MotionsRace.Droid.Controls;

namespace MotionsRace.Droid.Views
{
	[Activity(
		Theme = "@style/Theme.Splash",
		FinishOnTaskLaunch = true,
		LaunchMode = LaunchMode.SingleTask,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class ActivityItemView : BaseActivity<ActivityItemViewModel>
	{
		private ImageView _imageLayoutBlack;
		private GridView _gridLayoutBlack;
		private GridView _gridLayoutOrange;
		private SeekBar _intensitySlider;
		private LinearLayout _intensityTap1;
		private LinearLayout _intensityTap2;
		private LinearLayout _intensityTap3;
		private LinearLayout _intensityTap4;
		private LinearLayout _intensityTap5;

		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ActivityItemView);

			//TODO fix problem with run animation in timer
			//ViewModel.AnimateLiveRecordButtonSlide += animateLiveRecordButtonSlide;
			ViewModel.AnimateLiveRecordClock += animateLiveRecordClock;
		}

		private void animateLiveRecordButtonSlide()
		{
			var anim1 = AnimationUtils.LoadAnimation(this, Resource.Animation.move01);
			var anim2 = AnimationUtils.LoadAnimation(this, Resource.Animation.move02);
			var llPauseFinish = FindViewById<LinearLayout>(Resource.Id.llPauseFinish);
			var btnCancel = FindViewById<Button>(Resource.Id.btnCancel);

			anim1.AnimationStart += (sender, e) =>
			{
				llPauseFinish.Visibility = ViewStates.Visible;
			};
			anim1.AnimationEnd += (sender2, e2) =>
			{
				btnCancel.Visibility = ViewStates.Gone;
				llPauseFinish.Animate().Cancel();
				btnCancel.Animate().Cancel();
				btnCancel.StartAnimation(anim2);
			};

			llPauseFinish.StartAnimation(anim1);
			btnCancel.StartAnimation(anim2);
		}

		private void animateLiveRecordClock(bool isRunning)
		{
			var animateClock = FindViewById<ImageView>(Resource.Id.ivAnimateClock);
			if (isRunning)
			{
				animateClock.SetImageDrawable(Resources.GetDrawable(Resource.Animation.clock));
				var animation = animateClock.Drawable as AnimationDrawable;
				if (animation != null)
				{
					animation.Start ();
				}
			}
			else
			{
				animateClock.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.clock_animation_01));
			}
		}

		protected override void OnResume()
		{
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "OnResume");

			base.OnResume();

//			_gridLayoutBlack = FindViewById<GridView>(Resource.Id.IntensityGridBlack);
//			_gridLayoutBlack.Adapter = new IntensityBackgroundImageAdaptor(this, 5, Resource.Drawable.intensity_circle_black);
//			_gridLayoutOrange = FindViewById<GridView>(Resource.Id.IntensityGridOrange);
			_intensitySlider = FindViewById<SeekBar>(Resource.Id.intensity_slider);

//			_intensitySlider.ProgressChanged += (sender, e) =>
//			{
////				_gridLayoutOrange.Adapter = new IntensityBackgroundImageAdaptor(this,
////					_intensitySlider.Progress, Resource.Drawable.intensity_circle_orange);
//
//				ViewModel.IntensitySelected = _intensitySlider.Progress + 1;
//			};

			// Calculating Spaces between the circles in Intensity
//			_imageLayoutBlack = new ImageView(this);
//			_imageLayoutBlack.SetImageResource(Resource.Drawable.intensity_circle_black);
//			var dimensions = new BitmapFactory.Options { InJustDecodeBounds = true };
//			var gridSize = Resources.DisplayMetrics.WidthPixels - (Resources.DisplayMetrics.Density * Resources.DisplayMetrics.ScaledDensity) * 40;
//			BitmapFactory.DecodeResource(Resources, Resource.Drawable.intensity_circle_black, dimensions);
//			var imageWidth = dimensions.OutWidth;
//			var spacing = (int)(gridSize - imageWidth * 5) / 4;
//			_gridLayoutBlack.SetHorizontalSpacing(spacing);
//			_gridLayoutOrange.SetHorizontalSpacing(spacing);

//			_intensityTap1 = FindViewById<LinearLayout>(Resource.Id.intensitytap1);
//			_intensityTap1.Click += (sender, e) => { ViewModel.IntensitySelected = 1; };
//			_intensityTap2 = FindViewById<LinearLayout>(Resource.Id.intensitytap2);
//			_intensityTap2.Click += (sender, e) => { ViewModel.IntensitySelected = 2; };
//			_intensityTap3 = FindViewById<LinearLayout>(Resource.Id.intensitytap3);
//			_intensityTap3.Click += (sender, e) => { ViewModel.IntensitySelected = 3; };
//			_intensityTap4 = FindViewById<LinearLayout>(Resource.Id.intensitytap4);
//			_intensityTap4.Click += (sender, e) => { ViewModel.IntensitySelected = 4; };
//			_intensityTap5 = FindViewById<LinearLayout>(Resource.Id.intensitytap5);
//			_intensityTap5.Click += (sender, e) => { ViewModel.IntensitySelected = 5; };

//			_intensitySlider.Max = 4;
//			_intensitySlider.Progress = 0;

			ViewModel.OnResume();
		}

		protected override void OnPause()
		{
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "OnPause");
			ViewModel.OnPause();
			base.OnPause();
		}

		protected override void OnRestoreInstanceState(Bundle savedInstanceState)
		{
			base.OnRestoreInstanceState(new Bundle());
		}

		public override void OnBackPressed()
		{
			if (ViewModel.IsLiveRegister)
			{
				ViewModel.ConfirmExitCommand.Execute();
			}
			else
			{
				base.OnBackPressed();
			}
		}

		public void HideKeyboard(View pView)
		{
			if (pView != null)
			{
				var inputMethodManager = GetSystemService(InputMethodService) as InputMethodManager;
				if (inputMethodManager != null)
					inputMethodManager.HideSoftInputFromWindow(pView.WindowToken, HideSoftInputFlags.None);
			}
		}

		protected override void OnDestroy()
		{
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "OnDestroy");
			ViewModel.Dispose();
			base.OnDestroy();
		}
	}
}