using System;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Droid
{
	public class InvertBoolConverter:MvxValueConverter<bool,bool>
	{
		protected override bool Convert(bool value, Type targetType, object parameter, CultureInfo culture)
		{
			return !value;
		}
	}
}