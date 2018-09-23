using System;
using MvvmCross.Platform.Converters;
using UIKit;

namespace MotionsRace.Touch
{
	public class CategoryImagePathValueConverter : MvxValueConverter<string, UIImage>
	{
		protected override UIImage Convert (string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return UIImage.FromFile(string.Format("Categories/{0}.png", value));
		}
	}
}

