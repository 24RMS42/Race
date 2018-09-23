using System;
using System.Threading;
using System.Threading.Tasks;


namespace MotionsRace.Core.Helpers
{
	public class Timer : IDisposable
	{
		private DateTime _starTime;
		private TimeSpan _timeToStop;
		private CancellationTokenSource _cancellationTokenSource;
		public int Interval { get; set; }
		public bool IsRunning { get; private set; }

		public Timer()
		{
			Interval = 1000;
			_timeToStop = new TimeSpan();
			_starTime = DateTime.Now;
		}

		public void Start()
		{
			IsRunning = true;
			_starTime = DateTime.Now;
			_cancellationTokenSource = new CancellationTokenSource();
			Task.Run(async () =>
			{
				while (true)
				{
					onTimerAction();
					await Task.Delay(Interval, _cancellationTokenSource.Token);
				}
			}, _cancellationTokenSource.Token);
		}

		public void Stop()
		{
			if (!IsRunning)
			{
				return;
			}

			if (_cancellationTokenSource != null)
			{
				_cancellationTokenSource.Cancel();
				_cancellationTokenSource.Dispose();
			}

			IsRunning = false;
			_timeToStop = Elapsed;
			_starTime = DateTime.MinValue;
		}

		public TimeSpan Elapsed
		{
			get
			{
				return DateTime.Now -
					(_starTime == DateTime.MinValue ? DateTime.Now : _starTime) + _timeToStop;
			}
		}

		public void Reset()
		{
			_starTime = DateTime.MinValue;
			_timeToStop = new TimeSpan();
			IsRunning = false;
		}

		public void Reset(TimeSpan elapsed, bool isRunning)
		{
			Reset();
			_timeToStop = elapsed;
			if (isRunning)
			{
				Start();
			}
		}

		public event Action TimerEvent;
		private void onTimerAction()
		{
			Action handler = TimerEvent;
			if (handler != null)
				handler();
		}

		public void Dispose()
		{
			Stop();
		}
	}
}
