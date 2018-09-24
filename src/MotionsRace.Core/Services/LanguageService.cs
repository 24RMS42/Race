using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Resources;
using MotionsRace.Core;
using MotionsRace.Core.Localize;

namespace MotionsRace.Core.Services
{
	public class LanguageService : ILanguageService
	{
		private string _lang;
		private CultureInfo _culture;

		public void SetCurrentLanguage(List<string> preferencesLangs)
		{
			foreach (var lang in preferencesLangs)
			{
				if (lang.StartsWith("sv"))
				{
					_lang = "sv-SE";
					_culture = new CultureInfo(_lang);
					return;
				}
				if (lang.StartsWith("en"))
				{
					_lang = "en-GB";
					_culture = new CultureInfo(_lang);
					return;
				}
			}

			_lang = "en-GB";
			_culture = new CultureInfo(_lang);
		}

		public string GetCurrentLanguage()
		{
			return _lang;
		}

		public CultureInfo GetCurrentCulture()
		{
			return _culture;
		}

		public string GetString(string key)
		{
			ResourceManager resourceManager = null;
			resourceManager = GetCurrentLanguage() == "en-GB" 
				? new ResourceManager("MotionsRace.Core.Localize.en", typeof (en).GetTypeInfo().Assembly) 
				: new ResourceManager("MotionsRace.Core.Localize.sv", typeof (sv).GetTypeInfo().Assembly);
			return resourceManager.GetString(key);
		}

		public ResourceManager GetResourceManager()
		{
			return GetCurrentLanguage() == "en-GB"
				? en.ResourceManager
				: sv.ResourceManager;
		}
	}
}
