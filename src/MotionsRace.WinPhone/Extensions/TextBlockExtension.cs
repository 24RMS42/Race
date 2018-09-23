using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Phone.Controls;

namespace MotionsRace.WinPhone.Extensions
{
	public static class TextBlockExtension
	{
		public static string GetFormattedText(DependencyObject obj)
		{
			return (string) obj.GetValue(FormattedTextProperty);
		}

		public static void SetFormattedText(DependencyObject obj, string value)
		{
			obj.SetValue(FormattedTextProperty, value);
		}

		public static readonly DependencyProperty FormattedTextProperty =
			DependencyProperty.Register("FormattedText", typeof (string), typeof (TextBlock), new PropertyMetadata(string.Empty,
				(sender, e) =>
				{
					var text = e.NewValue as string;
					var textBl = sender as TextBlock;
					if (textBl != null && !string.IsNullOrWhiteSpace(text))
					{
						textBl.Inlines.Clear();
						var str = text.Split(new [] {"<b>", "</b>"}, StringSplitOptions.RemoveEmptyEntries);
						for (int i = 0; i < str.Length; i++)
							textBl.Inlines.Add(new Run { Text = str[i], FontWeight = i % 2 != 1 ? FontWeights.Bold : FontWeights.Normal });
					}
				}));

		public static string GetFormattedHtmlText(DependencyObject obj)
		{
			return (string)obj.GetValue(FormattedHtmlTextProperty);
		}

		public static void SetFormattedHtmlText(DependencyObject obj, string value)
		{
			obj.SetValue(FormattedHtmlTextProperty, value);
		}

		public static readonly DependencyProperty FormattedHtmlTextProperty =
			DependencyProperty.Register("FormattedHtmlText", typeof(string), typeof(WebBrowser), new PropertyMetadata(string.Empty,
				(sender, e) =>
				{
					var text = e.NewValue as string;
					var webBrowser = sender as WebBrowser;
					if (webBrowser != null && !string.IsNullOrWhiteSpace(text))
					{
						webBrowser.NavigateToString(string.Format("<html><head><meta name='viewport' content='width=380, user-scalable=yes' /></head><body>{0}</body></html>", text)); 
					}
				}));
	}
}