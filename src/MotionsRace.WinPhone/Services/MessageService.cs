using System.Windows;
using MotionsRace.Core.Services;


namespace MotionsRace.WinPhone.Services
{
	public class MessageService : IMessageService
	{
		public void ShowAlertAsync(string caption, string message)
		{
			MessageBox.Show(message, caption, MessageBoxButton.OK);
		}

		public void HideAlert()
		{
			//TODO Write hide message logic
		}
	}
}
