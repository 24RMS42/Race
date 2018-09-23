using MotionsRace.Core.Themes.Base;

namespace MobileTheming.Core.Themes.Base
{
	public interface ITheme
	{
		string Name { get; }

		string SignUpURL { get; }
		string ForgotPasswordURL { get; }

		IThemeColors Colors { get; set; }
		IThemeImages Images { get; set; }
	}
}
