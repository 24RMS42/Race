using Newtonsoft.Json;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace MotionsRace.Core.Models
{
	[XmlRoot(Namespace = "http://service.funbeatrace.com/MobileService.asmx",
		ElementName = "GetRacesAPersonCanLoginToResponse", IsNullable = true)]
	public class GetRacesResponse
	{
		[JsonProperty("d")]
		[XmlArray("GetRacesAPersonCanLoginToResult")]
		[XmlArrayItem("RaceAvailableForLoginResult")]
		public List<GetRacesResult> Result { get; set; }
	}
}