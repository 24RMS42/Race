using System.Windows.Media;
using Cirrious.MvvmCross.Plugins.Color.WindowsPhone;
using MotionsRace.Core.ViewModels;

namespace MotionsRace.WinPhone.Views
{
	public partial class WelcomeView
	{
		public new WelcomeViewModel ViewModel
		{
			get { return (WelcomeViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public WelcomeView()
		{
			InitializeComponent();
			Loaded += (sender, args) =>
			{
				SetColorToBrush("FlipNavigationSelectEllipseCommonBrush", ViewModel.Theme.Colors.WelcomeSliderIndicatorSelectorColor.ToNativeColor());
				SetColorToBrush("FlipNavigationEllipseCommonBrush", ViewModel.Theme.Colors.WelcomeSliderIndicatorNotSelectorColor.ToNativeColor());
				SetColorToBrush("WelcomeTextBrush", ViewModel.Theme.Colors.WelcomeTextColor.ToNativeColor());
			};
		}

		private void SetColorToBrush(string resourceName, Color color)
		{
			var flipNavigationSelectEllipseCommonBrush = (LayoutRoot.Resources[resourceName] as SolidColorBrush);
			if (flipNavigationSelectEllipseCommonBrush != null)
				flipNavigationSelectEllipseCommonBrush.Color = color;
		}
	}
}