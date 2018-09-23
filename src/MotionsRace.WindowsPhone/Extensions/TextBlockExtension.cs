using MotionsRace.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;
using Cirrious.MvvmCross.Plugins.Color.WindowsCommon;

namespace MotionsRace.WindowsPhone.Extensions
{
    public static class TextBlockExtension
    {
        private static readonly SolidColorBrush BoldColor = new SolidColorBrush(Constants.MAIN_NEWS_FEED_ITEM_FOREGROUND_BOLD_COLOR.ToNativeColor());
        private static readonly SolidColorBrush NormalColor = new SolidColorBrush(Constants.MAIN_NEWS_FEED_ITEM_FOREGROUND_NORMAL_COLOR.ToNativeColor());

        public static string GetFormattedText(DependencyObject obj)
        { return (string)obj.GetValue(FormattedTextProperty); }

        public static void SetFormattedText(DependencyObject obj, string value)
        { obj.SetValue(FormattedTextProperty, value); }

        public static readonly DependencyProperty FormattedTextProperty =
            DependencyProperty.Register("FormattedText", typeof(string), typeof(TextBlockExtension),
            new PropertyMetadata(string.Empty, (sender, e) =>
            {
                string text = e.NewValue as string;
                var textBl = sender as TextBlock;
                if (textBl != null)
                {
                    textBl.Inlines.Clear();
                    var str = text.Split(new string[] { "<b>", "</b>" }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i < str.Length; i++)
                        textBl.Inlines.Add(new Run { Text = str[i], Foreground = i % 2 != 1 ? BoldColor : NormalColor });
                }
            }));
    }
}
