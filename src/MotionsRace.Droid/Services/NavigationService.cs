using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MotionsRace.Droid.Services
{
	public class NavigationService : MvxViewModel, INavigationService
	{
		void INavigationService.ClearPrevPageFromBackStack ()
		{
			//TODO: INavigationService.ClearPrevPageFromBackStack ()
		}

		public void GoBack ()
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
			activity.OnBackPressed ();
		}
		public void ClearNavStack ()
		{
			// TODO: ClearNavStack
		}
	}
}