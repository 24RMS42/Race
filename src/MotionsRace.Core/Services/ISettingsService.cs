using MotionsRace.Core.Models;

namespace MotionsRace.Core.Services
{
	public interface ISettingsService
	{
		void Save();
		Settings Options { get; }
		bool IsNewVersion { get; }
	}
}
