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

namespace MotionsRace.Droid.Views
{
	[Activity(
		Theme = "@style/Theme.WelcomeView",
		NoHistory = true,
		ScreenOrientation = ScreenOrientation.Portrait)]
	public class WelcomeView : BaseActivity<WelcomeViewModel>
	{
		protected override void OnCreate(Bundle bundle)
		{
			base.OnCreate(bundle);
			SetContentView(Resource.Layout.WelcomeView);

#if ATEA
            // in real it is signUp button(I'm new)
            var signIn = FindViewById<Button>(Resource.Id.signIn);
            signIn.Visibility = Android.Views.ViewStates.Gone;

			// in real it is signIn button
			var signUp = FindViewById<Button>(Resource.Id.signUp);

            var layoutParams = (LinearLayout.LayoutParams)signUp.LayoutParameters;
            layoutParams.Weight = 1f;
            signUp.LayoutParameters = layoutParams;

			
#endif

            var gallery = FindViewById<CustomGallery>(Resource.Id.welcomGallery);
			var galleryWidth = gallery.Context.WallpaperDesiredMinimumWidth;
			var galleryHeight = gallery.Context.WallpaperDesiredMinimumHeight;

			galleryWidth = (galleryWidth/3);
			galleryHeight = (int) (galleryHeight*0.85);

			var ellipses = new[]
			{
				FindViewById<Ellipse>(Resource.Id.ellipse1),
				FindViewById<Ellipse>(Resource.Id.ellipse2),
				FindViewById<Ellipse>(Resource.Id.ellipse3)
			};

			var theme = Mvx.Resolve<IThemesManager>().CurrentTheme;

			//Slides Gallery
			Bitmap[] thumbIds =
			{
				ImageHelper.DecodeSampledBitmapFromStream(Assets.Open(theme.Images.FirstSlide), galleryWidth, galleryHeight, ViewModel.WelcomeText),
				ImageHelper.DecodeSampledBitmapFromStream(Assets.Open(theme.Images.SecondSlide), galleryWidth, galleryHeight, ViewModel.WelcomeText),
				ImageHelper.DecodeSampledBitmapFromStream(Assets.Open(theme.Images.ThirdSlide), galleryWidth, galleryHeight, ViewModel.WelcomeText),
			};

			gallery.Adapter = new GalleryAdapter(this, thumbIds);

			gallery.ItemSelected += delegate(object sender, AdapterView.ItemSelectedEventArgs e)
			{
				foreach (var item in ellipses)
				{
					item.Color = theme.Colors.WelcomeSliderIndicatorNotSelectorColor.ToAndroidColor();
				}
				ellipses[e.Position].Color = theme.Colors.WelcomeSliderIndicatorSelectorColor.ToAndroidColor() ;
			};
		}

		public override void OnBackPressed()
		{
			ViewModel.ExitCommand.Execute();
		}
	}
}