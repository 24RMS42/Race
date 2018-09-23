using System;

namespace MotionsRace.Core.Models
{
	public class Training
	{
		public int? TrainingID { get; set; }
		public int PersonID { get; set; }
		public int RaceID { get; set; }
		public int TrainingTypeID { get; set; }
		public DateTime StartDateTime { get; set; }
		public string Description { get; set; }
		public int? Intensity { get; set; }
		public int? Minutes { get; set; }
		public int? Steps { get; set; }
		public int? Points { get; set; }
		public double? Distance { get; set; }
		public TrainingUnit Unit { get; set; } 
	}
}

