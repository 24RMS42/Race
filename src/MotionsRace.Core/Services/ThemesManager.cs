using System.Collections.Generic;
using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Themes;

namespace MotionsRace.Core.Services
{
	public class ThemesManager : IThemesManager
	{
		public const string MotionRaceTheme = "MotionRaceTheme";
		public const string TwitchTheme = "TwitchTheme";
        public const string AteaTheme = "AteaTheme";
		public const string NordenMachineryTheme = "NordenMachineryTheme";
		public const string CoromaticTheme = "CoromaticTheme";
		public const string NetEntTheme = "NetEnt";
		public const string KronobergTheme = "Kronoberg";

        public ITheme CurrentTheme { get; private set; }
		public Dictionary<string, ITheme> Themes { get; private set; }

		public void SetCurrentTheme(string key)
		{
			CurrentTheme = Themes[key];
		}

		public ThemesManager()
		{
			Themes = new Dictionary<string, ITheme>
			{
				{ "MotionRaceTheme", new MotionRaceTheme() },
				{ "TwitchTheme", new TwitchTheme() },
                { "AteaTheme", new AteaTheme() },
				{ "NordenMachineryTheme", new NordenMachineryTheme() },
				{ "CoromaticTheme", new CoromaticTheme() },
				{ "NetEnt", new NetEntTheme() },
				{ "Kronoberg", new KronobergTheme() }
            };
		}
	}
}
