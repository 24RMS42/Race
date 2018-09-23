using Cirrious.CrossCore;
using Cirrious.CrossCore.Core;
using MotionsRace.Core.Services;
using System;
using Windows.UI.Popups;


namespace MotionsRace.WindowsPhone.Services
{
	public class MessageService : IMessageService
	{
		private MessageDialog _messageDialog;

		public async void ShowAlertAsync(string caption, string message)
		{
			_messageDialog = new MessageDialog(message, caption);
			var btnText = Mvx.Resolve<ILanguageService>().GetString("GLOBAL_Close");
			_messageDialog.Commands.Add(new UICommand(btnText, (s) => { }));
			await _messageDialog.ShowAsync();
		}

		public void HideAlert()
		{
			if (_messageDialog == null)
			{
				return;
			}
			//TODO Hide message logic
			_messageDialog.DisposeIfDisposable();
		}
	}
}
