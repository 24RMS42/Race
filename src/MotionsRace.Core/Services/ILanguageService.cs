using System.Collections.Generic;
using System.Globalization;
using System.Resources;


namespace MotionsRace.Core.Services
{
	public interface ILanguageService
	{
		void SetCurrentLanguage(List<string> preferencesLangs);
		string GetCurrentLanguage();
		CultureInfo GetCurrentCulture();
		string GetString(string key);
		ResourceManager GetResourceManager();
	}
}