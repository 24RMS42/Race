using System;
using MotionsRace.Core.Models;
using MvvmCross.Platform.Converters;

namespace MotionsRace.Touch
{
	public class NewsTimeValueConverter : MvxValueConverter<NewsFeedItemModel, string>
	{
		protected override string Convert (NewsFeedItemModel value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			return string.IsNullOrWhiteSpace (value.EventTimeSecondLine) ? value.EventTimeFirstLine : value.EventTimeFirstLine + "\n" + value.EventTimeSecondLine;
		}
	}
}

