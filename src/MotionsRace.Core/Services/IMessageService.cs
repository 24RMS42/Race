namespace MotionsRace.Core.Services
{
	public interface IMessageService
	{
		void ShowAlertAsync(string caption, string message);
		void HideAlert();
	}
}