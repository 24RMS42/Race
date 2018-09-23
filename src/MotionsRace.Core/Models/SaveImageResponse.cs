using System;
using Newtonsoft.Json;

namespace MotionsRace.Core.Models
{
	public class SaveImageResponse
	{
		[JsonProperty("d")]
		public string ImagePath { get; set; }
	}
}

