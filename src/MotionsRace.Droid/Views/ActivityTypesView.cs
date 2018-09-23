using Android.Content.PM;
using Android.App;
using MotionsRace.Core.ViewModels;

namespace MotionsRace.Droid.Views
{
	[Activity(
		Theme = "@style/Theme.Splash",
		FinishOnTaskLaunch = true,
		LaunchMode = LaunchMode.SingleTask,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class ActivityTypesView : BaseActivity<ActivityTypesViewModel>
	{
		protected override void OnCreate(Android.OS.Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ActivityTypesView);
		}

		protected override void OnResume()
		{
			base.OnResume();
			ViewModel.ClearSelectedItems();
		}

		public override void OnBackPressed()
		{
			if (ViewModel.CanOnBackShowCategories)
			{
				ViewModel.OnBackShowCategories();
			}
			else
			{
				base.OnBackPressed();
			}
		}
	}
}