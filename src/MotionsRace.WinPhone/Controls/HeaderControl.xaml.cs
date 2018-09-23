using System.Windows;
using System.Windows.Input;
using MotionsRace.Core.ViewModels;

namespace MotionsRace.WinPhone.Controls
{
	public partial class HeaderControl
	{
		public HeaderScreenViewModel ViewModel
		{
			get { return (HeaderScreenViewModel) DataContext; }
			set { DataContext = value; }
		}

		public static readonly DependencyProperty ShowAddButtonProperty =
			DependencyProperty.Register("ShowAddButton", typeof (bool), typeof (HeaderControl),
				new PropertyMetadata(true, OnAddButtonVisibilityChanged));

		public static readonly DependencyProperty ShowFavoritButtonProperty =
			DependencyProperty.Register("ShowFavoritButton", typeof(bool), typeof(HeaderControl),
				new PropertyMetadata(true, OnShowFavoritButtonVisibilityChanged));


		//Visibility ShowFavoritButton

		private static void OnAddButtonVisibilityChanged(DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
		{
			var myUserControl = dependencyObject as HeaderControl;
			if (myUserControl != null)
				myUserControl.btnAdd.Visibility = (bool) e.NewValue 
					? Visibility.Visible 
					: Visibility.Collapsed;
		}

		private static void OnShowFavoritButtonVisibilityChanged(DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
		{
			var myUserControl = dependencyObject as HeaderControl;
			if (myUserControl != null)
				myUserControl.btnStar.Visibility = (bool)e.NewValue
					? Visibility.Visible
					: Visibility.Collapsed;
		}

		public bool ShowAddButton
		{
			get { return (bool) GetValue(ShowAddButtonProperty); }
			set { SetValue(ShowAddButtonProperty, value); }
		}

		public bool ShowFavoritButton
		{
			get { return (bool)GetValue(ShowAddButtonProperty); }
			set { SetValue(ShowAddButtonProperty, value); }
		}

		public static readonly DependencyProperty ShowLoginButtonProperty =
			DependencyProperty.Register("ShowLoginButton", typeof (bool), typeof (HeaderControl),
				new PropertyMetadata(true, OnLoginButtonVisibilityChanged));

		private static void OnLoginButtonVisibilityChanged(DependencyObject dependencyObject,
			DependencyPropertyChangedEventArgs e)
		{
			var myUserControl = dependencyObject as HeaderControl;
			if (myUserControl != null)
				myUserControl.btnLogin.Visibility = (bool) e.NewValue 
					? Visibility.Visible 
					: Visibility.Collapsed;
		}

		public bool ShowLoginButton
		{
			get { return (bool) GetValue(ShowLoginButtonProperty); }
			set { SetValue(ShowLoginButtonProperty, value); }
		}

		public HeaderControl()
		{
			InitializeComponent();
		}

		private void btnAdd_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ViewModel.LiveRecordCommand.Execute();
		}

		private void btnHeaderLogo_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ViewModel.TapLogoCommand.Execute();
		}

		private void btnLogin_Tap(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ViewModel.LoginCommand.Execute();
		}

		private void btnStar_Tap(object sender, GestureEventArgs e)
		{
			ViewModel.FavoritRecordCommand.Execute();
		}
	}
}
