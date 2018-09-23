using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotionsRace.Core.Models
{
    public class PersonInfo
    {
        //public int PersonID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public object PasswordEncrypted { get; set; }
        //public object PasswordSalt { get; set; }
        //public object InternalPictureURL { get; set; }
        //public object Motto { get; set; }
        //public object PersonalDescription { get; set; }
        //public DateTime CreatedDateTime { get; set; }
        //public DateTime MostRecentLoginDateTime { get; set; }
        public string ImageGUID { get; set; }
        //public object FacebookSecret { get; set; }
        //public object FunBeatLoginID { get; set; }
        //public object FunBeatLoginSecret { get; set; }
        //public bool IsShowTrainingData { get; set; }
        //public object FitbitAuthAccessToken { get; set; }
        //public object FitbitAuthSecret { get; set; }
        //public DateTime MostRecentFitbitUpdatedDate { get; set; }
        //public int FitBitDeviceRaceID { get; set; }
        //public object GarminAuthAccessToken { get; set; }
        //public object GarminAuthSecret { get; set; }
        //public DateTime MostRecentGarminUpdateDate { get; set; }
        //public bool IsShowGuideOnLogin { get; set; }
		public bool? IsShareToFacebook { get; set; }
        //public object Weight { get; set; }
        //public object Height { get; set; }
        //public object Age { get; set; }
        //public object Sex { get; set; }
		public string FacebookAccessToken { get; set; }
        //public string PreferredCultureName { get; set; }
    }
}
