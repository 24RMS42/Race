using System;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Touch
{
	public class PercentsValueConverter : MvxValueConverter<int, float>
	{
		protected override float Convert (int value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return (float)(value / 100f);
		}
	}
}

