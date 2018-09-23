using System;
using Foundation;
using UIKit;
using MotionsRace.Core.Services;
using MvvmCross.Platform.Converters;
using MvvmCross.Platform;
using MvvmCross.Plugins.Color.iOS;

namespace MotionsRace.Touch
{
	public class NewsItemTextValueConverter : MvxValueConverter<string, NSMutableAttributedString>
	{
		protected override NSMutableAttributedString Convert (string value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			var theme = Mvx.Resolve<IThemesManager> ();

			var normalAttributes = new UIStringAttributes {
				ForegroundColor = theme.CurrentTheme.Colors.FeedItemTextColor.ToNativeColor().ColorWithAlpha(0.7f)
			};

			var boldAttributes = new UIStringAttributes {
				ForegroundColor = theme.CurrentTheme.Colors.FeedItemTextColor.ToNativeColor()
			};

			var strArray = value.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.RemoveEmptyEntries);
			var htmlString = new NSMutableAttributedString (string.Join("", strArray));
			var start = 0;
			for (int i = 0; i < strArray.Length; i++) {

				var attrs = i % 2 != 1 ? boldAttributes : normalAttributes;
				htmlString.SetAttributes (attrs.Dictionary, new NSRange (start, strArray[i].Length));
				start += strArray [i].Length;
			}

			return htmlString;
		}
	}
}

