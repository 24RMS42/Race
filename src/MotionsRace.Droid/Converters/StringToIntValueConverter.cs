using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Content.Res;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Droid.Converters
{
	public class StringToIntValueConverter : MvxValueConverter<string, int>
	{
		protected override int Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return 0;
			}

			return (int)typeof(Resource.Drawable).GetField(value).GetValue(null);
		}
	}
}