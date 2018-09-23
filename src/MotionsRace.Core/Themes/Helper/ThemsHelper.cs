using System;
using MvvmCross.Platform.UI;

namespace MobileTheming.Core.Themes.Helper
{
	public class ThemsHelper
	{
		public static MvxColor ConvertHexToColor(string hex)
		{
			if (hex.Contains("#"))
			{
				hex = hex.Replace("#", "");
			}
			int[] rgba = new int[] { 255, 255, 255, 255 };

			if (hex.Length == 6)
			{
				rgba[0] = Convert.ToInt32(hex.Substring(0, 2), 16);
				rgba[1] = Convert.ToInt32(hex.Substring(2, 2), 16);
				rgba[2] = Convert.ToInt32(hex.Substring(4, 2), 16);
			}
			else if (hex.Length == 8)
			{
				rgba[0] = Convert.ToInt32(hex.Substring(0, 2), 16);
				rgba[1] = Convert.ToInt32(hex.Substring(2, 2), 16);
				rgba[2] = Convert.ToInt32(hex.Substring(4, 2), 16);
				rgba[3] = Convert.ToInt32(hex.Substring(6, 2), 16);
			}
			else
			{
				throw new ArgumentException("Hash must be color code six or eight characters in length.");
			}

			return new MvxColor(rgba[0], rgba[1], rgba[2], rgba[3]);
		}
	}
}
