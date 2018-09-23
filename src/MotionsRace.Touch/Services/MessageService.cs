using MotionsRace.Core.Services;
using UIKit;

namespace MotionsRace.Touch
{
	public class MessageService : IMessageService
	{
		#region IMessageService implementation

		private UIAlertView _alert;

		public void ShowAlertAsync(string caption, string message)
		{
			//var alertController = UIAlertController.Create (caption, message, UIAlertControllerStyle.Alert);
			//alertController.AddAction(UIAlertActionStyle.
			_alert = new UIAlertView(caption, message, null, "ok", null);
			_alert.Show();
		}

		public void HideAlert()
		{
			if (_alert == null)
			{
				return;
			}
			_alert.DismissWithClickedButtonIndex(0, true);
		}

		#endregion
	}
}
