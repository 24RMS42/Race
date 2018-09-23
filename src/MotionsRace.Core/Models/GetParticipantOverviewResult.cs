using Newtonsoft.Json;
using System;
using System.Xml.Serialization;

namespace MotionsRace.Core.Models
{
	public class GetParticipantOverviewResult
	{
		public RaceDetails RaceDetails { get; set; }
		public PersonInfo PersonInfo { get; set; }
		public TeamInfo TeamInfo { get; set; }
		public int NumberOfMedals { get; set; }
		public int NumberOfMedalsEarned { get; set; }
		public int TotalNumberOfPoints { get; set; }
		public ServerDate ServerDate { get; set; }

		[JsonProperty("minvalidRegistrationDate")]
		[XmlElement("minvalidRegistrationDate")]
		public DateTime MinvalidRegistrationDate { get; set; }

		[JsonProperty("maxvalidRegistrationDate")]
		[XmlElement("maxvalidRegistrationDate")]
		public DateTime MaxvalidRegistrationDate { get; set; }
	}
}
