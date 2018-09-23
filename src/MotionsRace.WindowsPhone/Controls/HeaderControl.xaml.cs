using MotionsRace.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace MotionsRace.WindowsPhone.Controls
{
    public sealed partial class HeaderControl : UserControl
    {
        public static readonly DependencyProperty ShowAddButtonProperty =
              DependencyProperty.Register("ShowAddButton", typeof(bool), typeof(HeaderControl),
              new PropertyMetadata(true, OnAddButtonVisibilityChanged));

        private static void OnAddButtonVisibilityChanged(DependencyObject dependencyObject,
               DependencyPropertyChangedEventArgs e)
        {
            var myUserControl = dependencyObject as HeaderControl;
            myUserControl.btnAdd.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool ShowAddButton
        {
            get { return (bool)GetValue(ShowAddButtonProperty); }
            set { SetValue(ShowAddButtonProperty, value); }
        }

        public static readonly DependencyProperty ShowLoginButtonProperty =
              DependencyProperty.Register("ShowLoginButton", typeof(bool), typeof(HeaderControl),
              new PropertyMetadata(true, OnLoginButtonVisibilityChanged));

        private static void OnLoginButtonVisibilityChanged(DependencyObject dependencyObject,
               DependencyPropertyChangedEventArgs e)
        {
            var myUserControl = dependencyObject as HeaderControl;
            myUserControl.btnLogin.Visibility = (bool)e.NewValue ? Visibility.Visible : Visibility.Collapsed;
        }

        public bool ShowLoginButton
        {
            get { return (bool)GetValue(ShowLoginButtonProperty); }
            set { SetValue(ShowLoginButtonProperty, value); }
        }

        public HeaderControl()
        {
            this.InitializeComponent();
        }

        private void btnAdd_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = this.DataContext as HeaderScreenViewModel;
            vm.LiveRecordCommand.Execute();
        }

        private void btnLogin_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = this.DataContext as HeaderScreenViewModel;
            vm.LoginCommand.Execute();
        }

        private void btnHeaderLogo_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var vm = this.DataContext as HeaderScreenViewModel;
            vm.TapLogoCommand.Execute();
        }
    }
}
