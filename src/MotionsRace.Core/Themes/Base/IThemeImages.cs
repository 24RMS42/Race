namespace MobileTheming.Core.Themes.Base
{
	public interface IThemeImages
	{
		string Logo { get; }

		string FirstSlide { get; }
		string SecondSlide { get; }
		string ThirdSlide { get; }

		string LoginBackground { get; }
		string LoginLogo { get; }

		string Close { get; }

		string HeaderLogo { get; }
		string HeaderRegister { get; }
		string HeaderRegisterFavorit { get; }
		string HeaderGoToWeb { get; }

		string TrainingCategoriesPath { get; }

		string Face { get; }
		string CircleFace { get; }
	}
}
