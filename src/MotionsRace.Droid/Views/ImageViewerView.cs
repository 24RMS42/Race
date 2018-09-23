using Android.App;
using Android.Content.PM;
using Android.OS;
using MotionsRace.Core.ViewModels;
using MotionsRace.Droid.Controls;

namespace MotionsRace.Droid.Views
{
	[Activity(
		Theme = "@style/Theme.MainView",
		LaunchMode = LaunchMode.SingleTask)]
	public class ImageViewerView : BaseActivity<ImageViewerViewModel>
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.ImageViewerView);
            var feedImage = FindViewById<ScaleImageView>(Resource.Id.feedImage);
            feedImage.SetImageBitmap(MainView.RotatetImage);
		}
	}
}