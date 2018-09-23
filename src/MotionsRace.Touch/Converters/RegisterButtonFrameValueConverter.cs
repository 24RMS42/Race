using System;
using CoreGraphics;
using MotionsRace.Touch.Views;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Touch
{
	public class RegisterButtonFrameValueConverter : MvxValueConverter<bool, CGRect>
	{
		protected override CGRect Convert (bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var view = parameter as MainView;
			return view.GetRegisterButtonFrame();
		}
	}
}

