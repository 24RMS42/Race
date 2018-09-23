using MotionsRace.Core.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.System;
using Windows.UI.Xaml;

namespace MotionsRace.WindowsPhone.Services
{
    public class PlatformService : IPlatformService
    {
        public string HashEncode(string keyString, string message)
        {
            var crypt = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha1);
            var buffer = CryptographicBuffer.ConvertStringToBinary(keyString, BinaryStringEncoding.Utf8);
            var keyBuffer = CryptographicBuffer.ConvertStringToBinary(message, BinaryStringEncoding.Utf8);
            var key = crypt.CreateKey(keyBuffer);

            var sigBuffer = CryptographicEngine.Sign(key, buffer);
            string signature = CryptographicBuffer.EncodeToBase64String(sigBuffer);
            return signature;
        }

        public async void LauchUrl(string url)
        {
            await Launcher.LaunchUriAsync(new Uri(url));
        }


        public bool IsInternetAvailable()
        {
            var connectionProfile = NetworkInformation.GetInternetConnectionProfile();
            return (connectionProfile != null && connectionProfile.GetNetworkConnectivityLevel() == NetworkConnectivityLevel.InternetAccess);
        }


        public MotionsRace.Core.Models.Platform Platform
        {
            get
            {
                return Core.Models.Platform.WindowsPhone;
            }
        }


        public void AppExit()
        {
            Application.Current.Exit();
        }


        public long FreeRAM()
        {
            throw new NotImplementedException();
        }

        public string UnGZipByteArray(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            using (var zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
            using (var resultStream = new MemoryStream())
            {
                zipStream.CopyTo(resultStream); // show error "can not read" in PCL for iOS
                var result = resultStream.ToArray();
                return Encoding.UTF8.GetString(result, 0, result.Length);
            }
        }




        public int GetAppVersion()
        {
            throw new NotImplementedException();
        }
    }
}
