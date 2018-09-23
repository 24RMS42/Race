using System.Xml.Serialization;
using System;

namespace MotionsRace.Core.Models
{
	[Obsolete("Use SaveTrainingShareToFacebookParams")]
	[XmlRootAttribute("SaveTraining")]
	public class SaveTrainingParams
	{
		public string applicationID { get; set; }
		public string loginID { get; set; }
		public string loginSecret { get; set; }
		public Training training { get; set; }
	}

	[XmlRootAttribute("SaveTrainingShareToFacebook")]
	public class SaveTrainingShareToFacebookParams
	{
		public string applicationID { get; set; }
		public string loginID { get; set; }
		public string loginSecret { get; set; }
		public Training training { get; set; }
		public bool isShare { get; set; }
	}
}

