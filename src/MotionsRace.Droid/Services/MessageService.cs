using MotionsRace.Core.Services;
using Java.Lang;
using Android.Widget;
using MvvmCross.Platform;
using MvvmCross.Platform.Droid.Platform;

namespace MotionsRace.Droid.Services
{
	public class MessageService: IMessageService
	{
		private Toast _toast ;

		public void ShowAlertAsync (string caption, string message)
		{
			var activity = Mvx.Resolve<IMvxAndroidCurrentTopActivity>().Activity;
			activity.RunOnUiThread(new Runnable(() => 
				{
					_toast = Toast.MakeText(activity.BaseContext, message, ToastLength.Long);
					_toast.Show();
				}
			));
		}

		public void HideAlert()
		{
			if (_toast == null)
			{
				return;
			}
			_toast.Cancel();
		}
	}
}