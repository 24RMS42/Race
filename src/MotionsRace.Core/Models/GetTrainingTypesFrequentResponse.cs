using System;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace MotionsRace.Core
{
	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "GetTrainingTypesFrequentResponse", IsNullable=true)]
	public class GetTrainingTypesFrequentResponse
	{
		[JsonProperty("d")]
		[XmlArray("GetTrainingTypesFrequentResult")]
		[XmlArrayItem("TrainingType")]
		public List<GetTrainingTypesResult> Result { get; set; }
	}
}

