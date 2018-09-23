using System;
using CoreGraphics;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Touch
{
	public class TypeNameFrameValueConverter : MvxValueConverter<bool, CGRect>
	{
		protected override CGRect Convert (bool value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var frame = (CGRect)parameter;
			return value ? new CGRect (10, 5, frame.Width - 80, frame.Height / 2) : new CGRect (10, 0, frame.Width - 80, frame.Height);
		}
	}
}

