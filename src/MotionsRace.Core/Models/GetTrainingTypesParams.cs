using System;
using System.Xml.Serialization;

namespace MotionsRace.Core
{
	[XmlRootAttribute("GetTrainingTypes")]
	public class GetTrainingTypesParams
	{
		public string applicationID { get; set; }
		public string loginID { get; set; }
		public string loginSecret { get; set; }        
		public string cultureName { get; set; }        
		public int raceID { get; set; }
		
	}
}

