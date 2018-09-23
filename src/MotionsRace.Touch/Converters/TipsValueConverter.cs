using System;
using Foundation;
using UIKit;
using MotionsRace.Core.Services;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform;
using MvvmCross.Plugins.Color.iOS;

namespace MotionsRace.Touch
{
	public class TipsValueConverter : MvxValueConverter<string, NSMutableAttributedString>
	{
		protected override NSMutableAttributedString Convert (string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var theme = Mvx.Resolve<IThemesManager> ();

			var normalAttributes = new UIStringAttributes {
				Font = UIFont.FromName("HelveticaNeue-Light", 8f),
				ForegroundColor = theme.CurrentTheme.Colors.ActivityItemTextColor.ToNativeColor()
			};

			var boldAttributes = new UIStringAttributes {
				Font = UIFont.FromName("HelveticaNeue-Light", 10f),
				ForegroundColor = theme.CurrentTheme.Colors.ActivityItemTextColor.ToNativeColor()
			};

			var boldText = Mvx.Resolve<ILanguageService>().GetString("Activity_NoteLabel") + ": ";
			var fullText = boldText + value;
			var result = new NSMutableAttributedString (fullText);

			result.SetAttributes (boldAttributes.Dictionary, new NSRange(0, boldText.Length));
			result.SetAttributes (normalAttributes.Dictionary, new NSRange(boldText.Length, value.Length));

			return result;
		}
	}
}

