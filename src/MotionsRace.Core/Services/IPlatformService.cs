
using MotionsRace.Core.Models;
namespace MotionsRace.Core.Services
{
    public interface IPlatformService
    {
        string HashEncode(string key, string message);
        void LauchUrl(string url);
		bool IsInternetAvailable ();
		void AppExit ();
        Platform Platform { get; } 
		long FreeRAM ();
		string UnGZipByteArray (byte[] data); // need implement only for iOS
		int GetAppVersion();
	}
}