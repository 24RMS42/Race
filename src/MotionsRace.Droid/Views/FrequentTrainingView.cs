using Android.Content.PM;
using Android.App;
using Android.Views;
using MotionsRace.Core.ViewModels;
using MvvmCross.Binding.Droid.Views;

namespace MotionsRace.Droid.Views
{
	[Activity(
		Theme = "@style/Theme.Splash",
		FinishOnTaskLaunch = true,
		LaunchMode = LaunchMode.SingleTask,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class FrequentTrainingView : BaseActivity<FrequentTrainingViewModel>, View.IOnTouchListener
	{
		protected override void OnCreate(Android.OS.Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.FrequentTrainingView);

			var t = FindViewById<MvxListView>(Resource.Id.list);
			t.SetOnTouchListener(this);
		}

		public bool OnTouch(View v, MotionEvent e)
		{
			if (e.Action == MotionEventActions.Down)
			{
				ViewModel.TapLogoCommand.Execute();
			}
			return false;
		}
	}
}