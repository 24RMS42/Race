using System;
using CoreGraphics;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Touch
{
	public class NewsItemFrameValueConverter : MvxValueConverter<string, CGRect>
	{
		protected override CGRect Convert (string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.IsNullOrWhiteSpace(value) ? new CGRect(0, 0, 0, 0) : new CGRect(0, 0, 0, 0);
		}
	}
}

