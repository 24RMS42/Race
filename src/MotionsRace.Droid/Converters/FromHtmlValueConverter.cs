using System;
using Android.Text;
using System.Globalization;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Droid.Converter
{
	public class FromHtmlValueConverter : MvxValueConverter<string>
	{
		protected override object Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			return Html.FromHtml(value);
		}
	}
}

