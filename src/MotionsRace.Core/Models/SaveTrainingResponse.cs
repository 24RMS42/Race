using System;
using Newtonsoft.Json;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[Obsolete("Use SaveTrainingShareToFacebookResponse")]
	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "SaveTrainingResponse", IsNullable=true)]
	public class SaveTrainingResponse
	{
		[JsonProperty("d")]
		[XmlElement("SaveTrainingResult")]
		public int TrainingID { get; set; }

        // set manually
        public string ErrorMessage { get; set; }
	}

	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "SaveTrainingShareToFacebookResponse", IsNullable=true)]
	public class SaveTrainingShareToFacebookResponse
	{
		[JsonProperty("d")]
		[XmlElement("SaveTrainingShareToFacebookResult")]
		public int TrainingID { get; set; }

		// set manually
		public string ErrorMessage { get; set; }
	}
}

