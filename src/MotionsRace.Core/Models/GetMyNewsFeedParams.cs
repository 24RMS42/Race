using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[XmlRootAttribute("GetMyNewsFeed")]
    public class GetMyNewsFeedParams
    {
        public string applicationID { get; set; }
        public string loginID {get; set;}
        public string loginSecret {get; set;}
        public string cultureName {get; set;}
        public int raceID {get; set;}
		[XmlElement("activitiesonly")]
		public int? activitiesOnly { get; set; }
    }
}
