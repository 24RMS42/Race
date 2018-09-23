using System;


namespace MotionsRace.Core.Models
{
	public class GetRacesResult
	{
		public int RaceID { get; set; }
		public string RaceTitle { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public string CompanyName { get; set; }
		public string HostName { get; set; }
		public string MobileHostName { get; set; }
		public string BrandName { get; set; }
	}
}