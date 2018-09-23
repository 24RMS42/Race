using MotionsRace.Core.ApiClient;
using MotionsRace.Core.Models;
using System.Threading.Tasks;

namespace MotionsRace.Core.Services
{
	public interface IWebService
	{
		void SetMode(WebServiceMode mode);
		void LoadCredentialsFromSettings();

		Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password, bool isOAuth);
		Task<ApiResponse<GetRacesResponse>> GetRacesAsync();
		Task<ApiResponse<AuthorizeRaceAccessResponse>> AuthorizeRaceAccessAsync();
		Task<ApiResponse<GetParticipantOverviewResponse>> GetParticipantOverviewAsync();
		Task<ApiResponse<GetMyNewsFeedResponse>> GetMyNewsFeedAsync(bool activitiesOnly);
		Task<ApiResponse<GetTrainingTypesResponse>> GetTrainingTypesAsync();
		Task<ApiResponse<GetTrainingTypesFrequentResponse>> GetFrequentTrainingTypesAsync();
		Task<ApiResponse<SaveTrainingShareToFacebookResponse>> SaveTrainingShareToFacebookAsync(Training training, bool isShare);
		Task<ApiResponse<GetServerDateTimeResponse>> GetServerDateTime();
		Task<ApiResponse<SaveImageResponse>> SaveImageAsync(string trainingIds, byte[] imageBytes);
	}
}
