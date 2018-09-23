using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MotionsRace.Core
{
	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "GetTrainingTypesResponse", IsNullable=true)]
	public class GetTrainingTypesResponse
	{
		[JsonProperty("d")]
		[XmlArray("GetTrainingTypesResult")]
		[XmlArrayItem("TrainingType")]
		public List<GetTrainingTypesResult> Result { get; set; }
	}
}

