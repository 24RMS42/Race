using System;

namespace MotionsRace.Core.Models
{
	public class Settings
	{
		public int AppVersion { get; set; }

		public WebServiceMode WebServiceMode { get; set; }
		public string LoginID { get; set; }
		public string LoginSecretHashed { get; set; }
		public int PersonID { get; set; }
		public int? RaceID { get; set; }
		public string HostName { get; set; }
		public bool AllowLogin { get; set; }
		public bool AllowLiveRecord { get; set; }
		public int? MaxNumberOfDaysUserCanWaitToRegister { get; set; }
		public bool IsIntensityActivated { get; set; }
		public bool IsDistanceActivated { get; set; }
		public DateTime RaceStartDate { get; set; }
		public DateTime RaceEndDate { get; set; }
		public int? MaxMinutesTotalPerDay { get; set; }
		public int? MaxPointsPerWeek { get; set; }
		public bool IsSecureProtocol { get; set; }
		public bool IsShareToFacebook { get; set; }
		public bool ShowShareToFacebook { get; set; }

		public DateTime? LastRunDateTimeGetParticipantOverview { get; set; }
		public DateTime ServerDate { get; set; }
		public DateTime MinvalidRegistrationDate { get; set; }
		public DateTime MaxvalidRegistrationDate { get; set; }
		public int? RequiredMinutes { get; set; }

		public LiveRecordSettings LiveRecord { get; set; }

		public bool CredentialsExists
		{
			get
			{
				return !string.IsNullOrWhiteSpace(LoginID)
					&& !string.IsNullOrWhiteSpace(LoginSecretHashed);
			}
		}

		public void Reset()
		{
			AppVersion = 0;
			WebServiceMode = WebServiceMode.Prodaction;
			LoginID = "";
			PersonID = 0;
			RaceID = null;
			HostName = null;
			AllowLogin = false;
			AllowLiveRecord = false;
			MaxNumberOfDaysUserCanWaitToRegister = null;
			MaxMinutesTotalPerDay = null;
			MaxPointsPerWeek = null;
			IsSecureProtocol = false;
			IsShareToFacebook = false;
			ShowShareToFacebook = false;
		}
	}

	/// <summary>
	/// Save live record data if task go to background
	/// </summary>
	public class LiveRecordSettings
	{
		public int TrainingId { get; set; }
		public bool TimerIsRunning { get; set; }
		public TimeSpan TimerElapsed { get; set; }
		public DateTime PauseDateTime { get; set; }
		public bool NoteVisibility { get; set; }
	}
}