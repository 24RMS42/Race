using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views;
using MotionsRace.Core.Services;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace MotionsRace.WindowsPhone.Views
{
    public sealed partial class SignInView : MvxWindowsPage
    {
        public SignInView()
        {
            this.InitializeComponent();            
        }

        private void TextBox_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                tbPassword.Focus(Windows.UI.Xaml.FocusState.Programmatic);
        }

        private void tbPassword_KeyUp(object sender, Windows.UI.Xaml.Input.KeyRoutedEventArgs e)
        {
            if (e.Key == VirtualKey.Enter)
                btnLogin.Focus(Windows.UI.Xaml.FocusState.Programmatic); // sending focus to Page to hide keyboard
        }
    }
}
