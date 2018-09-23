using System.ComponentModel;
using System.Windows;
using Cirrious.MvvmCross.WindowsPhone.Views;

namespace MotionsRace.WinPhone.Views
{
	public partial class SignInView : MvxPhonePage
	{
		public SignInView()
		{
			InitializeComponent();
		}

		private void tbPassword_KeyUp_1(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
				btnLogin.Focus(); // sending focus to Page to hide keyboard
		}

		private void TextBox_KeyUp_1(object sender, System.Windows.Input.KeyEventArgs e)
		{
			if (e.Key == System.Windows.Input.Key.Enter)
				tbPassword.Focus();
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			Application.Current.Terminate();
		}
	}
}