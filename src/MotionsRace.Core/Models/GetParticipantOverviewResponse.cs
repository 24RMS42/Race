using Newtonsoft.Json;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	[XmlRoot(Namespace = "http://service.funbeatrace.com/MobileService.asmx",
		ElementName = "GetParticipantOverviewResponse", IsNullable = true)]
	public class GetParticipantOverviewResponse
	{
		[JsonProperty("d")]
		[XmlElement("GetParticipantOverviewResult")]
		public GetParticipantOverviewResult Result { get; set; }
	}
}
