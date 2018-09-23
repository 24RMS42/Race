using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
    public class AuthorizeRaceAccessResult
    {
//        public int RaceID { get; set; }
//        public string RaceTitle { get; set; }
//        public int CompanyID { get; set; }
//        public string IntranetSecretKey { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
//        public DateTime LoginsOpenDateTime { get; set; }
//        public DateTime LoginsCloseDateTime { get; set; }
//        public DateTime? GoesSecretDateTime { get; set; }
//        public object GoesSecretEndDate { get; set; }
        public DateTime LastRegisterDateTime { get; set; }
//        public DateTime ShowWinnerDateTime { get; set; }
//        public object ShowInvitationToExternalSiteDateTime { get; set; }
//        public object MinutesRequiredForAnActivityToBeOk { get; set; }
//        public object MaxMinutesForTrainingTypePerDay { get; set; }
        public int? MaxMinutesTotalPerDay { get; set; }
        public int MaxNumberOfDaysUserCanWaitToRegister { get; set; }
//        public string LoginURL { get; set; }
        [JsonProperty("isDistanceActivated")]
		[XmlElement("isDistanceActivated")]
        public bool IsDistanceActivated { get; set; }
        [JsonProperty("isIntensityActivated")]
		[XmlElement("isIntensityActivated")]
        public bool IsIntensityActivated { get; set; }
//		public bool isStepsActivated { get; set; }
//		public bool isDemoRace { get; set; }
//		public string CompanyName { get; set; }
		public string HostName { get; set; }
//		public string MobileHostName { get; set; }
//		public object GoogleSiteVerification { get; set; }
//		public string TeamLevelTitles { get; set; }
//		public string BookingRaceTypeID { get; set; }
		public bool isLoginPossible { get; set; }
//		public bool isShowWinnersPossible { get; set; }
//		public bool isShowTeamScoresPossible { get; set; }
		public bool isRegisterTrainingPossible { get; set; }
//		public bool isSecretMode { get; set; }
//		public bool isLastTrainingDayOver { get; set; }
//		public bool isHasRaceStarted { get; set; }
//		public bool isAwaitingShowWinners { get; set; }
//		public bool isShowAddTrainingOnTablet { get; set; }
//		public int MaxDistanceFromFirstCommonParent { get; set; }
//		public object MaxTeamsPerList { get; set; }
//		public bool isPersonsCanHideTrainingCalendar { get; set; }
//		public bool isParticipantSearch { get; set; }
//		public int PointsToShowFBLike { get; set; }
//		public object PauseFirstDate { get; set; }
//		public object PauseLastDate { get; set; }
//		public object PauseLastTrainingDate { get; set; }
//		public bool isRacePaused { get; set; }
//		public bool IsShowHonestyCheckbox { get; set; }
//		public bool IsShowGuide { get; set; }
//		public object GuideURL { get; set; }
//		public int WhatsGoingOnThreshold { get; set; }
//		public int RankingThreshold { get; set; }
//		public int CacheDurationMinutesHighscores { get; set; }
//		public bool IsAllowRequestNewActivity { get; set; }
//		public bool IsRestrictTeamNameChange { get; set; }
//		public bool IsRestrictTeamNameChangeForEverybody { get; set; }
//		public object SuggestTeamOpenDateTime { get; set; }
//		public object SuggestTeamCloseDateTime { get; set; }
//		public bool IsShowMyListAlsoDuringSecretPeriod { get; set; }
//		public string CustomerPanel { get; set; }
//		public int MaxRecentNewsListing { get; set; }
//		public bool IsIntensityUsedForScores { get; set; }
		public int? MaxPointsPerWeek { get; set; }
//		public bool IsAllowManageTeamMembers { get; set; }
//		public string DefaultPassword { get; set; }
//		public string TeamAdminComment { get; set; }
//		public bool TeamAdminAutomaticAdd { get; set; }
//		public bool IsShowMyScore { get; set; }
//		public bool IsShowMyScoreTeamList { get; set; }
//		public int TargetPersonScore { get; set; }
//		public bool SignUpGarminConnect { get; set; }
//		public bool IsCustomScorelists { get; set; }
//		public bool IsPersonalScoresThisWeekOnly { get; set; }
    }
}
