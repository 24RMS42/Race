using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MotionsRace.WindowsPhone.Views
{
    public sealed partial class ActivityItemView : MvxWindowsPage
    {
        public ActivityItemView()
        {
            this.InitializeComponent();            
        }

        private void StackPanel_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            var vm = this.ViewModel as ActivityItemViewModel;
            vm.UploadImageCommand.Execute();

        } 

    }
}
