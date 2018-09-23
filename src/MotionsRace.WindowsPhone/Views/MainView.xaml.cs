using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace MotionsRace.WindowsPhone.Views
{
    public sealed partial class MainView : MvxWindowsPage
    {
        public MainView()
        {
            this.InitializeComponent();            
        }

        private void tbUserName_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var vm = ViewModel as MainViewModel;
            vm.ShowMenuCommand.Execute();
        }

        protected override void OnNavigatedTo(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
 	         base.OnNavigatedTo(e);
             HardwareButtons.BackPressed += HardwareButtons_BackPressed;           
        }

        protected override void OnNavigatedFrom(Windows.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            HardwareButtons.BackPressed -= HardwareButtons_BackPressed;           
        }

        void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            e.Handled = true;
            var vm = ViewModel as MainViewModel;
            vm.ExitCommand.Execute();            
        }
    }
}
