using System;
using System.Text.RegularExpressions;
using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Helpers;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core.Models
{
	public class NewsFeedItemModel : MvxNavigatingObject
	{
		private readonly GetMyNewsFeedResult _item;
		private readonly MainViewModel _mainViewModel;
		private readonly DateTime _serverDate;
		private readonly ITheme _theme;

		public NewsFeedItemModel(MainViewModel mainViewModel, GetMyNewsFeedResult item, int index, DateTime serverDate)
		{
			_theme = Mvx.Resolve<IThemesManager>().CurrentTheme;
			_item = item;
			_mainViewModel = mainViewModel;
			_serverDate = serverDate;
			SeparatorColor = index%2 == 0
				? _theme.Colors.FeedItemSeparatorColor
				: _theme.Colors.FeedItemOddSeparatorColor;

			var isYou = item.PersonName == _mainViewModel.FirstLastName;
			var personNameOrYou = isYou ? _mainViewModel["Main_You"] : item.PersonName;
			Text = FillItem(item, personNameOrYou, isYou);

			var options = Mvx.Resolve<ISettingsService>().Options;
			// Remove all tags between "{" and "}" and form auto login li
			if (!string.IsNullOrWhiteSpace(_item.FullMessage))
			{
				FullMessage = Regex.Replace(_item.FullMessage, @"{(.|\n)*?}", string.Empty);

				// find links
				foreach (var match in Regex.Matches(_item.FullMessage, 
					@"((href)=(\\\""|""|')|)(www\.|(http|https|ftp|news|file)+\:\/\/)[&#95;.a-z0-9-]+\.[a-z0-9\/&#95;:@=.+?,##%&~-]*[^.|\'|\# |!|\(|?|,| |>|<|;|\"")]+(?=(\""|<|))"))
				{
					var link = match.ToString();
                    if (!link.Contains("href"))
				    {
                        FullMessage = FullMessage.Replace(link, string.Format("<a href='{0}'>{0}<a>", link));
				    }
                    else
					if (link.Contains(options.HostName))
					{
					    link = Regex.Match(link, "((http|https|ftp|news|file).*)").Value;
                        FullMessage = FullMessage.Replace(link, UrlHelper.GetAutologinUrl(link.Replace(
                        UrlHelper.GetProtocol() + "://" + options.HostName, string.Empty)));
					}
				}
			}
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				RaisePropertyChanged(() => BackgroundColor);
				RaisePropertyChanged(() => TextColor);
			}
		}

		public MvxColor BackgroundColor
		{
			get { return _isSelected ? _theme.Colors.FeedItemSelectedColor : _theme.Colors.FeedItemColor; }
		}

		public MvxColor TextColor
		{
			get { return _isSelected ? _theme.Colors.FeedItemSelectedTextColor : _theme.Colors.FeedItemTextColor; }
		}

		public MvxColor SeparatorColor { get; set; }

		public string FullImageURL
		{
			get { return _item.ImageURL; }
		}

		public string EventTimeFirstLine
		{
			get
			{
				if (_item.EventTime.Date == _serverDate) return _mainViewModel["GLOBAL_Today"];
				if (_item.EventTime.Date.AddDays(1) == _serverDate) return _mainViewModel["GLOBAL_Yesterday"];

				return _item.EventTime.ToString("dddd", Mvx.Resolve<ILanguageService>().GetCurrentCulture());
			}
		}

		public string EventTimeSecondLine
		{
			get
			{
				if (_item.EventTime.Date == _serverDate) return null;
				if (_item.EventTime.Date.AddDays(1) == _serverDate) return null;

				return _item.EventTime.ToString("MMMM d", Mvx.Resolve<ILanguageService>().GetCurrentCulture());
			}
		}

		public string EventTime
		{
			get { return string.Concat(EventTimeFirstLine, " ", EventTimeSecondLine); }
		}

		public string Text { get; private set; }

		public string ThumbnailURL
		{
			get { return string.IsNullOrWhiteSpace(_item.ThumbnailURL) ? null : _item.ThumbnailURL; }
		}

		public string FullMessage { get; private set; }

		public string ReadMoreURL
		{
			get { return _item.ReadMoreURL; }
		}

		private string FillItem(GetMyNewsFeedResult item, string personNameOrYou, bool isYou)
		{
			var langService = Mvx.Resolve<ILanguageService>();
			var result = "";

			switch (item.EventType)
			{
				case "ACTIVITY":
				{
					if (item.Minutes > 0)
					{
						//<b>{0} registred</b> {1} minutes of {2}. <b>{3} points.</b>
						result = string.Format(langService.GetString("Main_NewsFeed_Activity1"), personNameOrYou, _item.Minutes,
							_item.TranslatedName, _item.Points);
					}
					else if (item.Minutes == 0 | item.Minutes == null)
					{
						//<b>{0} registred</b> {1}. <b>{2} points.</b>
						result = string.Format(langService.GetString("Main_NewsFeed_Activity2"), personNameOrYou, _item.TranslatedName,
							_item.Points);
					}
					break;
				}
				case "MEDAL":
				{
					//<b>{0}</b> was awarded <b>bonus medal</b>  {1}. <b>{2} points.</b>
					result = string.Format(langService.GetString("Main_NewsFeed_MEDAL"), personNameOrYou, _item.TranslatedName,
						_item.Points);
					break;
				}
				case "FORUMALL":
				{
					//<b>{0}</b> posted to challenge forum <b> {1}</b> 
					result = string.Format(langService.GetString("Main_NewsFeed_FORUMALL"), personNameOrYou, _item.BaseName);
					break;
				}
				case "FORUMTEAM":
				{
					//<b>{0}</b> posted to your team discussion <b>{1}</b> 
					result = string.Format(langService.GetString("Main_NewsFeed_FORUMTEAM"), personNameOrYou, _item.BaseName);
					break;
				}

				case "NEWS":
				{
					//<b>{0}</b> News posted by {1}
					result = string.Format(langService.GetString("Main_NewsFeed_NEWS"), _item.BaseName, personNameOrYou);
					break;
				}

				case "MEDALNEWS":
				{
					if (item.Minutes == null)
					{
						//Bonus and medal for <b>{0}</b> can now be chosen! {1} points.
						result = string.Format(langService.GetString("Main_NewsFeed_MEDALNEWS1"), _item.TranslatedName, _item.BasePoints);
					}
					else if (item.Minutes > 0)
					{
						//The bonus medal <b>{0}</b> is available. {1} points. Earn it now!
						result = string.Format(langService.GetString("Main_NewsFeed_MEDALNEWS2"), _item.TranslatedName, _item.BasePoints);
					}
					break;
				}

				case "GOALSTEAM":
					//<b>{0}</b> set goal <b>{1}</b>
					result = string.Format(langService.GetString("Main_NewsFeed_GOALSTREAM"), personNameOrYou, _item.BaseName);
					break;

				case "OTHER":
					//<b>{0}</b> set goal <b>{1}</b>
					result = _item.TranslatedName;
					break;

				case "PERSONALMESSAGE":
					//Message from <b>{0}</b>
					result = string.Format(langService.GetString("Main_NewsFeed_MESSAGE"), _item.PersonName);
					break;

				default:
					result = !string.IsNullOrWhiteSpace(_item.TranslatedName) ? _item.TranslatedName : "None";
					break;
			}

			if (isYou) result = result.Replace("was", "were");

			return result;
		}
	}
}