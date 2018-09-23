using System;
using System.Globalization;
using MotionsRace.Core.Services;
using MotionsRace.Core.Themes;
using MvvmCross.Platform;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Droid.Converters
{
	public class TrainingCategoryImageValueConverter : MvxValueConverter<string, string>
	{
		protected override string Convert(string value, Type targetType, object parameter, CultureInfo culture)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				return null;
			}

			var theme = Mvx.Resolve<IThemesManager>().CurrentTheme;
			return string.Format(@"{0}/{1}.png", theme.Images.TrainingCategoriesPath, value);
		}
	}
}