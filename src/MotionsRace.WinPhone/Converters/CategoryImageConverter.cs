using System;
using System.Windows.Data;
using Cirrious.CrossCore;
using MotionsRace.Core.Services;

namespace MotionsRace.WinPhone.Converters
{
	public class CategoryImageConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var theme = Mvx.Resolve<IThemesManager>().CurrentTheme;
			return string.Format(@"/Assets/{0}/{1}.png", theme.Images.TrainingCategoriesPath, value);
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
