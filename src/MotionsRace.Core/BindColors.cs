using System;
using MotionsRace.Core.Services;
using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core
{
	public class BindColors
	{
		public MvxColor this[string index]
		{
			get
			{
				var theme = Mvx.Resolve<IThemesManager>().CurrentTheme;
				var color = theme.Colors.GetType().GetProperty(index, BindingFlags.Public).GetValue(theme.Colors, null) as MvxColor;
				if (color != null)
					return color;
				else
					throw new Exception(string.Format("Color wit name {0} not defined in Constants.cs", index));
			}
		}
	}
}
