using System.Collections.Generic;
using MobileTheming.Core.Themes.Base;

namespace MotionsRace.Core.Services
{
	public interface IThemesManager
	{
		ITheme CurrentTheme { get; }
		Dictionary<string, ITheme> Themes { get; }

		void SetCurrentTheme(string key);
	}
}
