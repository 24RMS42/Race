using System;
using MotionsRace.Core.ViewModels;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform.UI;

namespace MotionsRace.Core.Models
{
	public class ActivityCheckBox : MvxNotifyPropertyChanged
	{
		private bool _checked;
		public bool Checked {
			get { return _checked; }
			set
			{
				_checked = value;
				RaisePropertyChanged(() => Checked);
			}
		}

		public MvxColor TextColor { get; set; }
		public string DateText { get; set; }
		public DateTime Date { get; set; }
        public ActivityItemViewModel ViewModel { get; set;} // for WP binding
	}
}

