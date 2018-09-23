using System;
using UIKit;
using CoreGraphics;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Touch
{
	public class BoolToBarButtonEmptyViewValueConverter : MvxValueConverter<bool, UIColor>
	{
		protected override UIColor Convert (bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return value ? UIColor.White : UIColor.Clear;
		}
	}
}

