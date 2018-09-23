using Acr.UserDialogs;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Platform.IoC;

namespace MotionsRace.Core
{
    public class App : MvxApplication
    {
        public override void Initialize()
        {
            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            MvvmCross.Plugins.File.PluginLoader.Instance.EnsureLoaded();
            MvvmCross.Plugins.Json.PluginLoader.Instance.EnsureLoaded();
			MvvmCross.Plugins.Sqlite.PluginLoader.Instance.EnsureLoaded();
			MvvmCross.Plugins.Color.PluginLoader.Instance.EnsureLoaded();
			MvvmCross.Plugins.Messenger.PluginLoader.Instance.EnsureLoaded();

			Mvx.RegisterSingleton<IUserDialogs>(() => UserDialogs.Instance);

            RegisterAppStart(new CustomAppStart());
        }
    }
}