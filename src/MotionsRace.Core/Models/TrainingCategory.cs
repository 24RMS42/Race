using System;
using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.Services;
using MotionsRace.Core.Themes;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core
{
	public class TrainingCategory : MvxNotifyPropertyChanged
	{
		private ITheme _theme;

		public int ActivityCategoryID { get; set; }
		public string ActivityCategoryName { get; set; }

		public string IconFile { get; set; }
		public string IconFileSelected { get; set; }

		private bool _isSelected;
		public bool IsSelected
		{ 
			get{ return _isSelected; } 
			set
			{ 
				_isSelected = value;
				RaisePropertyChanged (() => IsSelected);
				RaisePropertyChanged (() => BackGroundColor);
				RaisePropertyChanged (() => TextColor);
			} 
		}
		public bool IsVisible { get; set; }

		public MvxColor BackGroundColor {
			get { return _isSelected ? _theme.Colors.TrainingCategoryItemSelectedColor : _theme.Colors.TrainingCategoryItemColor; }
		}
		public MvxColor TextColor {
			get { return _isSelected ? _theme.Colors.TrainingCategoryItemSelectedTextColor : _theme.Colors.TrainingCategoryItemTextColor; }
		}

		public TrainingCategory()
		{
			_theme = Mvx.Resolve<IThemesManager>().CurrentTheme;
		}

	}
}

