using System;
using System.Net;
using System.Threading.Tasks;
using MotionsRace.Core.ApiClient;
using MotionsRace.Core.Models;
using MvvmCross.Platform;

namespace MotionsRace.Core.Services
{
	public abstract class BaseWebService : IWebService
	{
		// used only on Windows Phone
		private bool _inDormantState;

		protected static ISettingsService SettingsService;
		protected readonly ILanguageService LanguageService;
		protected readonly IPlatformService PlatformService;

		protected BaseWebService()
		{
			PlatformService = Mvx.Resolve<IPlatformService>();
			LanguageService = Mvx.Resolve<ILanguageService>();
			SettingsService = Mvx.Resolve<ISettingsService>();
			_inDormantState = false;
		}

		protected static WebServiceMode Mode
		{
			get { return SettingsService.Options.WebServiceMode; }
		}

		protected static string LoginId
		{
			get { return SettingsService.Options.LoginID; }
		}

		protected static string LoginSecretHashed
		{
			get { return SettingsService.Options.LoginSecretHashed; }
		}

		protected static int PersonId
		{
			get { return SettingsService.Options.PersonID; }
		}

		protected static int RaceId
		{
			get { return SettingsService.Options.RaceID ?? 0; }
		}

		protected string ApplicationId
		{
			get { return WebServiceSettings.GetServiceData(Mode, ServiceCharacteristic.ApplicationId); }
		}

		protected string ApplicationSecret
		{
			get { return WebServiceSettings.GetServiceData(Mode, ServiceCharacteristic.ApplicationSecret); }
		}

		public void SetMode(WebServiceMode mode)
		{
			SettingsService.Options.WebServiceMode = mode;
		}

		public void LoadCredentialsFromSettings()
		{
		}

		protected async Task<ApiResponse<T>> TryInvokeAsync<T>(Func<Task<ApiResponse<T>>> func)
		{
			try
			{
				var operation = await func();
				while (IsWebExceptionRequestCanceled(operation.Exception) && _inDormantState)
				{
					_inDormantState = false;
					operation = await func();
				}

				return operation;
			}
			catch (Exception ex)
			{
				return new ApiResponse<T>
				{
					IsSuccess = false,
					ErrorMessage = ex.Message,
					Exception = ex
				};
			}
		}

		private bool IsWebExceptionRequestCanceled(Exception ex)
		{
			var wEx = ex as WebException;
			if (wEx == null)
			{
				return false;
			}
			return wEx.Status == WebExceptionStatus.RequestCanceled;
		}

		public abstract Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password, bool isOAuth);
		public abstract Task<ApiResponse<GetRacesResponse>> GetRacesAsync();
		public abstract Task<ApiResponse<AuthorizeRaceAccessResponse>> AuthorizeRaceAccessAsync();
		public abstract Task<ApiResponse<GetParticipantOverviewResponse>> GetParticipantOverviewAsync();
		public abstract Task<ApiResponse<GetMyNewsFeedResponse>> GetMyNewsFeedAsync(bool activitiesOnly);
		public abstract Task<ApiResponse<GetTrainingTypesResponse>> GetTrainingTypesAsync();
		public abstract Task<ApiResponse<GetTrainingTypesFrequentResponse>> GetFrequentTrainingTypesAsync();
		public abstract Task<ApiResponse<SaveTrainingShareToFacebookResponse>> SaveTrainingShareToFacebookAsync(Training training, bool isShare);
		public abstract Task<ApiResponse<GetServerDateTimeResponse>> GetServerDateTime();
		public abstract Task<ApiResponse<SaveImageResponse>> SaveImageAsync(string trainingIds, byte[] imageBytes);
	}
}
