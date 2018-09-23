using System;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace MotionsRace.Core.Models
{
	[XmlRoot(Namespace="http://service.funbeatrace.com/MobileService.asmx", ElementName = "GetServerDateTimeResponse", IsNullable=true)]
	public class GetServerDateTimeResponse
	{
		[JsonProperty("d")]
		[XmlElement("GetServerDateTimeResult")]
		public string ServerDateString { get; set; }

		public DateTime ServerTimeShift 
		{
			get{ return DateTime.Parse (ServerDateString); }
		}
		public DateTime ServerDate 
		{
			get{ return ServerTimeShift.Date; }
		}
	}
}

