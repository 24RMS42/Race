using MotionsRace.Core.Models;
using MvvmCross.Platform;
using MvvmCross.Platform.Platform;
using MvvmCross.Plugins.File;

namespace MotionsRace.Core.Services
{
	public class SettingsService : ISettingsService
    {
		
        private const string SETTINGS_FILE_NAME = "settings.json";

        protected IMvxFileStore _store;
        protected IMvxJsonConverter _jsonConverter;

        private Settings _options;
        public Settings Options
        {
            get
            {
                if (_options == null)
                    _options = Load();

                return _options;
            }
        }

        public SettingsService(IMvxFileStore store, IMvxJsonConverter jsonConverter)
        {
            _store = store;
            _jsonConverter = jsonConverter;
        }

        private Settings Load()
        {
            string json;
            if (_store.TryReadTextFile(SETTINGS_FILE_NAME, out json))
            {
				try
				{
	                var settings = _jsonConverter.DeserializeObject<Settings>(json);
					if (settings == null)
						settings = new Settings ();
					
	                return settings;
				}
				catch 
				{
					return new Settings();
				}
            }
            else
            {
                return new Settings();
            }
        }

        public void Save()
        {
			Options.AppVersion = Mvx.Resolve<IPlatformService> ().GetAppVersion ();
            var json = _jsonConverter.SerializeObject(Options);
            _store.WriteFile(SETTINGS_FILE_NAME, json);
        }

		public bool IsNewVersion {
			get {
				return Options.AppVersion < Mvx.Resolve<IPlatformService> ().GetAppVersion ();
			}
		}

    }

}
