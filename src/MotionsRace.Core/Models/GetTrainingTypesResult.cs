using System;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using MobileTheming.Core.Themes.Base;
using MotionsRace.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.UI;
using MvvmCross.Platform;

namespace MotionsRace.Core
{
	public class GetTrainingTypesResult : MvxNavigatingObject
	{
		private readonly ILanguageService _languageService;
		private readonly ISettingsService _settingsService;
		private readonly ITheme _theme;

		public GetTrainingTypesResult()
		{
			_languageService = Mvx.Resolve<ILanguageService>();
			_settingsService = Mvx.Resolve<ISettingsService>();
			_theme = Mvx.Resolve<IThemesManager>().CurrentTheme;

			_isSelected = _isSelectedClock = false;
		}

		public string TrainingTypeName { get; set; }
		public int TrainingTypeID { get; set; }
		public TrainingUnit Unit { get; set; }
		public string UnitAsString { get; set; }
		public double Weight { get; set; }
		public string Description { get; set; }
		public int? DailyLimit { get; set; }
		public int ActivityCategoryID { get; set; }
		public string ActivityCategoryName { get; set; }
		public bool IsIntensity { get; set; }

		public string UnitString
		{
			get
			{
				//if (Unit == TrainingUnit.Minutes && Weight == 1) return "";
				if (Unit == TrainingUnit.Minutes && Weight != 1)
					return String.Format(_languageService.GetString("Activity_Units_Minutes"), Weight);
				else if (Unit == TrainingUnit.Steps)
					return string.Format(_languageService.GetString("Activity_Units_Steps"), Weight*1000);
				else if (Unit == TrainingUnit.Fixed)
					return String.Format(_languageService.GetString("Activity_Units_Fixed"), Weight);
				else if (Unit == TrainingUnit.Score)
					return _languageService.GetString("Activity_Units_Score");
				else
					return null;
			}
		}

		public bool UnitVisibility
		{
			get { return !string.IsNullOrEmpty(UnitString); }
		}

		private bool _isSelected;
		public bool IsSelected
		{
			get { return _isSelected; }
			set
			{
				_isSelected = value;
				IsSelectedClock = _isSelected;
				RaisePropertyChanged(() => BackGroundColor);
				RaisePropertyChanged(() => TextColor);
				RaisePropertyChanged(() => TextPointsColor);
			}
		}

		private bool _isSelectedClock;
		public bool IsSelectedClock
		{
			get { return _isSelectedClock; }
			set
			{
				_isSelectedClock = value;
				RaisePropertyChanged(() => ClockBackGroundColor);
				RaisePropertyChanged(() => ClockIcon);
			}
		}

		public bool DescriptionVisibility
		{
			get { return !string.IsNullOrEmpty(Description); }
		}

		public MvxColor BackGroundColor
		{
			get { return _isSelected ? _theme.Colors.TrainingItemSelectedColor : _theme.Colors.TrainingItemColor; }
		}

		public MvxColor ClockBackGroundColor
		{
			get { return _isSelectedClock ? _theme.Colors.ClockSelectedBackGroundColor : _theme.Colors.ClockBackGroundColor; }
		}

		public MvxColor TextColor
		{
			get { return _isSelected ? _theme.Colors.TrainingItemSelectedTextColor : _theme.Colors.TrainingItemTextColor; }
		}

		public MvxColor TextPointsColor
		{
            get { return _isSelected ? _theme.Colors.TrainingItemPointsSelectedColor : _theme.Colors.TrainingItemPointsColor; }
		}

		//TODO uncoment in next realise 
		public bool LiveRecordVisibility
		{
			get { return Unit == TrainingUnit.Minutes && 
				_settingsService.Options.ServerDate >= _settingsService.Options.MinvalidRegistrationDate && 
				_settingsService.Options.ServerDate <= _settingsService.Options.MaxvalidRegistrationDate; }
		}

		public string ClockIcon
		{
			get { return "clock_icon_white"; }
		}

		public IMvxCommand TrainingCategoryItemsSelectedCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					IsSelectedClock = true;
					ShowViewModel<ActivityItemViewModel>(
						new {liveRecord = true, trainingId = TrainingTypeID});
				});
			}
		}
	}
}