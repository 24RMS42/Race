using System;


namespace MotionsRace.Core.Models
{
	public class GetMyNewsFeedResult
	{
		public string EventType { get; set; }
		public DateTime EventTime { get; set; }
		public int? Points { get; set; }
		public string BaseName { get; set; }
		public int? BasePoints { get; set; }
		public string PersonName { get; set; }
		public int? Minutes { get; set; }
		public string TranslatedName { get; set; }
		public int RelevanceRank { get; set; }
		public string ImageURL { get; set; }
		public string ThumbnailURL { get; set; }
		public string FullMessage { get; set; }
		public string ReadMoreURL { get; set; }
	}
}