using System;
using System.Threading.Tasks;
using MotionsRace.Core;
using MotionsRace.Core.ApiClient;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;

// ReSharper disable once CheckNamespace
namespace MotionsRace.Services
{
	public class WebService : BaseWebService
	{
		public override Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password, bool isOAuth)
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new ValidateAndCreateSecretParams
				{
					applicationID = ApplicationId,
					username = username,
					secret = password,
					method = isOAuth ? "OAuth" : "UserNameAndPassword"
				};

				var client = new MotionsRaceClient(Mode);
				var result = await client.ValidateAndCreateSecret(parameters);

				if (result.IsSuccess)
				{
					SettingsService.Options.LoginID = result.Response.Result.LoginID;
					SettingsService.Options.LoginSecretHashed = PlatformService.HashEncode(result.Response.Result.LoginSecret,
							ApplicationSecret.ToLower());
					SettingsService.Options.PersonID = result.Response.Result.UserInfo.PersonID;
					SettingsService.Save();

					return
						new ApiResponse<LoginResponse>(new LoginResponse
						{
							LoginID = LoginId,
							LoginSecretHashed = LoginSecretHashed,
							PersonID = PersonId
						});
				}
				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<GetRacesResponse>> GetRacesAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new GetRacesParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					hostName = ""
				};

				var client = new MotionsRaceClient(Mode);
				var result = await client.GetRacesAPersonCanLoginTo(parameters);

				if (result.IsSuccess)
					return new ApiResponse<GetRacesResponse>(result.Response);

				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<AuthorizeRaceAccessResponse>> AuthorizeRaceAccessAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new AuthorizeRaceAccessParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					raceID = RaceId
				};

				var client = new MotionsRaceClient(Mode);
				var result = await client.AuthorizeRaceAccess(parameters);

				if (result.IsSuccess)
					return new ApiResponse<AuthorizeRaceAccessResponse>(result.Response);

				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<GetParticipantOverviewResponse>> GetParticipantOverviewAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new GetParticipantOverviewParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					personID = PersonId,
					raceID = RaceId
				};

				var client = new MotionsRaceClient(Mode);

				var result = await client.GetParticipantOverview(parameters);
				if (result.IsSuccess)
				{
					return new ApiResponse<GetParticipantOverviewResponse>(result.Response);
				}

				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<GetMyNewsFeedResponse>> GetMyNewsFeedAsync(bool activitiesOnly)
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new GetMyNewsFeedParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					cultureName = LanguageService.GetCurrentLanguage(),
					raceID = RaceId,
				};

				if (activitiesOnly)
				{
					parameters.activitiesOnly = 1;
				}
				else
				{
					parameters.activitiesOnly = null;
				}

				var client = new MotionsRaceClient(Mode);
				var result = await client.GetMyNewsFeed(parameters);

				if (result.IsSuccess)
					return new ApiResponse<GetMyNewsFeedResponse>(result.Response);
				
				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<GetTrainingTypesResponse>> GetTrainingTypesAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new GetTrainingTypesParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					cultureName = LanguageService.GetCurrentLanguage(),
					raceID = RaceId
				};

				var client = new MotionsRaceClient(Mode);
				var result = await client.GetTrainingTypes(parameters);

				if (result.IsSuccess)
					return new ApiResponse<GetTrainingTypesResponse>(result.Response);
				
				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<GetTrainingTypesFrequentResponse>> GetFrequentTrainingTypesAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new GetFrequentTrainingTypesParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					cultureName = LanguageService.GetCurrentLanguage(),
					raceID = RaceId
				};

				var client = new MotionsRaceClient(Mode);
				var result = await client.GetFrequentTrainingTypes(parameters);

				if (result.IsSuccess)
					return new ApiResponse<GetTrainingTypesFrequentResponse>(result.Response);

				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<SaveTrainingShareToFacebookResponse>> SaveTrainingShareToFacebookAsync(Training training, bool isShare)
		{
			return TryInvokeAsync(async () =>
			{
				var parameters = new SaveTrainingShareToFacebookParams
				{
					applicationID = ApplicationId,
					loginID = LoginId,
					loginSecret = LoginSecretHashed,
					training = training,
					isShare = isShare
				};

				var client = new MotionsRaceClient(Mode);
				var result = await client.SaveTrainingShareToFacebook(parameters);

				if (result.IsSuccess)
						return new ApiResponse<SaveTrainingShareToFacebookResponse>(result.Response);
				
				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<GetServerDateTimeResponse>> GetServerDateTime()
		{
			return TryInvokeAsync(async () =>
			{
				var client = new MotionsRaceClient(Mode);
				var result = await client.GetServerDateTime();
				if (result.IsSuccess)
					return new ApiResponse<GetServerDateTimeResponse>(result.Response);
				
				throw new Exception(result.Message);
			});
		}

		public override Task<ApiResponse<SaveImageResponse>> SaveImageAsync(string trainingIds, byte[] imageBytes)
		{
			return TryInvokeAsync(async () =>
			{
				var client = new MotionsRaceClient(Mode);
				var result = await client.SaveImage(trainingIds, imageBytes);

				if (result.IsSuccess)
					return new ApiResponse<SaveImageResponse>(result.Response);
				
				throw new Exception(result.Message);
			});
		}
	}
}