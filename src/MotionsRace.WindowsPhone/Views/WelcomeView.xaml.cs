using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views;
using MotionsRace.Core;
using MotionsRace.Core.Services;
using Windows.UI;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Cirrious.MvvmCross.Plugins.Color.WindowsCommon;
using Windows.Phone.UI.Input;
using MotionsRace.Core.ViewModels;

namespace MotionsRace.WindowsPhone.Views
{
    public sealed partial class WelcomeView : MvxWindowsPage
    {
        public WelcomeView()
        {
            this.InitializeComponent();
            Loaded += WelcomeView_Loaded;
        }

        void WelcomeView_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            StatusBar statusBar = Windows.UI.ViewManagement.StatusBar.GetForCurrentView();
            statusBar.BackgroundOpacity = 0;
            statusBar.BackgroundColor = Colors.Red;

            var navEllipse = navList.Children[0] as Ellipse;
            navEllipse.Fill = new SolidColorBrush(Constants.WELCOME_SLIDER_INDICATOR_SELECTED_COLOR.ToNativeColor());
        }

        private void sliderFlipView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var flipView = sender as FlipView;
            var selectedIndex = flipView.SelectedIndex;
            var stackPanel = flipView.FindName("navList") as StackPanel;
            if (stackPanel == null)
                return;

            foreach (Ellipse item in stackPanel.Children)
            {
                item.Fill = new SolidColorBrush(Constants.WELCOME_SLIDER_INDICATOR_NOT_SELECTED_COLOR.ToNativeColor());
            }
            var navEllipse = navList.Children[selectedIndex] as Ellipse;
            navEllipse.Fill = new SolidColorBrush(Constants.WELCOME_SLIDER_INDICATOR_SELECTED_COLOR.ToNativeColor());
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
            var vm = ViewModel as WelcomeViewModel;
            vm.ExitCommand.Execute();
        }
    }
}
