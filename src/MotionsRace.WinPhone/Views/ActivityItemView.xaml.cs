using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;
using MotionsRace.Core.ViewModels;

namespace MotionsRace.WinPhone.Views
{
	public partial class ActivityItemView
	{
		private DispatcherTimer _timer;
		private int _animFrame;

		private readonly List<BitmapImage> _clockFrameList = new List<BitmapImage>
		{
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_01.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_02.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_03.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_04.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_05.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_06.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_07.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_08.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_09.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_10.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_11.png", UriKind.Relative)),
			new BitmapImage(new Uri("../Assets/Clock/clock_animation_12.png", UriKind.Relative))
		};

		public new ActivityItemViewModel ViewModel
		{
			get { return (ActivityItemViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public ActivityItemView()
		{
			InitializeComponent();
			InitTimerAnimation();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (ViewModel.AnimateLiveRecordClock == null)
			{
				ViewModel.AnimateLiveRecordClock += AnimateLiveRecordClock;
			}
			ViewModel.OnResume();
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			ViewModel.OnPause();
			base.OnNavigatedFrom(e);
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			e.Cancel = true;
			if (ViewModel.IsLiveRegister)
			{
				ViewModel.ConfirmExitCommand.Execute();
			}
			else
			{
				e.Cancel = false;
			}
			base.OnBackKeyPress(e);
		}

		private void AnimateLiveRecordClock(bool isRunning)
		{
			if (isRunning)
			{
				_timer.Start();
			}
			else
			{
				_timer.Stop();
			}
		}

		private void InitTimerAnimation()
		{
			_animFrame = 0;
			_timer = new DispatcherTimer();
			_timer.Tick += (sender, args) =>
			{
				if (_animFrame > 11)
				{
					_animFrame = 0;
				}
				TimerImage.Source = _clockFrameList[_animFrame];
				_animFrame++;
			};
			_timer.Interval = new TimeSpan(0, 0, 1);
		}

		private void UploadImage(object sender, System.Windows.Input.GestureEventArgs e)
		{
			ViewModel.UploadImageCommand.Execute();
		}
	}
}