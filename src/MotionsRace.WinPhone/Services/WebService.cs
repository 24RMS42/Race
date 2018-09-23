using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.ServiceModel;
using System.Threading.Tasks;
using MotionsRace.Core;
using MotionsRace.Core.ApiClient;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using MotionsRace.WinPhone.MobileService;
using Newtonsoft.Json;
using AuthorizeRaceAccessResponse = MotionsRace.Core.Models.AuthorizeRaceAccessResponse;
using GetMyNewsFeedResponse = MotionsRace.Core.Models.GetMyNewsFeedResponse;
using GetParticipantOverviewResponse = MotionsRace.Core.Models.GetParticipantOverviewResponse;
using GetServerDateTimeResponse = MotionsRace.Core.Models.GetServerDateTimeResponse;
using GetTrainingTypesFrequentResponse = MotionsRace.Core.GetTrainingTypesFrequentResponse;
using GetTrainingTypesResponse = MotionsRace.Core.GetTrainingTypesResponse;
using PersonInfo = MotionsRace.Core.Models.PersonInfo;
using ServerDate = MotionsRace.Core.Models.ServerDate;
using Training = MotionsRace.Core.Models.Training;


namespace MotionsRace.WinPhone.Services
{
	public class WebService : BaseWebService
	{
		public override Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new ValidateAndCreateSecretRequest
				{
					Body = new ValidateAndCreateSecretRequestBody(ApplicationId, username, password,
						AuthenticationMethod.UserNameAndPassword)
				};

				var response = await Task.Factory.FromAsync<MobileService.ValidateAndCreateSecretResponse>(
					client.BeginValidateAndCreateSecret(request, null, null),
					client.EndValidateAndCreateSecret);

				SettingsService.Options.LoginID = response.Body.ValidateAndCreateSecretResult.LoginID;
				SettingsService.Options.LoginSecretHashed =
					PlatformService.HashEncode(response.Body.ValidateAndCreateSecretResult.LoginSecret,
						ApplicationSecret);
				SettingsService.Options.PersonID = response.Body.ValidateAndCreateSecretResult.UserInfo.PersonID;
				SettingsService.Save();

				return
					new ApiResponse<LoginResponse>(new LoginResponse
					{
						LoginID = LoginId,
						LoginSecretHashed = LoginSecretHashed,
						PersonID = PersonId
					});
			});
		}

		public override Task<ApiResponse<GetRacesResponse>> GetRacesAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new GetRacesAPersonCanLoginToRequest
				{
					Body = new GetRacesAPersonCanLoginToRequestBody(ApplicationId, LoginId, LoginSecretHashed, "")
				};

				var response = await Task.Factory.FromAsync<GetRacesAPersonCanLoginToResponse>(
					client.BeginGetRacesAPersonCanLoginTo(request, null, null),
					client.EndGetRacesAPersonCanLoginTo);

				var getRacesResponseModel = new GetRacesResponse { Result = new List<GetRacesResult>() };
				foreach (var race in response.Body.GetRacesAPersonCanLoginToResult)
				{
					var raceResult = new GetRacesResult
					{
						BrandName = race.BrandName,
						CompanyName = race.CompanyName,
						EndDate = race.EndDate,
						HostName = race.HostName,
						MobileHostName = race.MobileHostName,
						RaceID = race.RaceID,
						RaceTitle = race.RaceTitle,
						StartDate = race.StartDate
					};

					getRacesResponseModel.Result.Add(raceResult);
				}

				return new ApiResponse<GetRacesResponse>(getRacesResponseModel);
			});
		}

		public override Task<ApiResponse<AuthorizeRaceAccessResponse>> AuthorizeRaceAccessAsync()
		{
			throw new NotImplementedException();
		}

		public override Task<ApiResponse<GetParticipantOverviewResponse>> GetParticipantOverviewAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new GetParticipantOverviewRequest
				{
					Body = new GetParticipantOverviewRequestBody(ApplicationId, LoginId, LoginSecretHashed, PersonId, RaceId)
				};

				var response = await Task.Factory.FromAsync<MobileService.GetParticipantOverviewResponse>(
					client.BeginGetParticipantOverview(request, null, null),
					client.EndGetParticipantOverview);

				var getParticipantOverviewResponse = new GetParticipantOverviewResponse
				{
					Result = new GetParticipantOverviewResult
					{
						MaxvalidRegistrationDate = response.Body.GetParticipantOverviewResult.maxvalidRegistrationDate,
						MinvalidRegistrationDate = response.Body.GetParticipantOverviewResult.minvalidRegistrationDate,
						NumberOfMedals = response.Body.GetParticipantOverviewResult.NumberOfMedals,
						NumberOfMedalsEarned = response.Body.GetParticipantOverviewResult.NumberOfMedalsEarned,
						PersonInfo = new PersonInfo
						{
							FirstName = response.Body.GetParticipantOverviewResult.PersonInfo.FirstName,
							ImageGUID =
								response.Body.GetParticipantOverviewResult.PersonInfo.ImageGUID.HasValue
									? response.Body.GetParticipantOverviewResult.PersonInfo.ImageGUID.ToString()
									: null,
							LastName = response.Body.GetParticipantOverviewResult.PersonInfo.LastName,
							IsShareToFacebook = response.Body.GetParticipantOverviewResult.PersonInfo.IsShareToFacebook.HasValue && 
											response.Body.GetParticipantOverviewResult.PersonInfo.IsShareToFacebook.Value,
							FacebookAccessToken = response.Body.GetParticipantOverviewResult.PersonInfo.FacebookAccessToken,
                        },
					}
				};

				var infoResponseResult = response.Body.GetParticipantOverviewResult;
				getParticipantOverviewResponse.Result.RaceDetails = new RaceDetails
				{
					EndDate = infoResponseResult.RaceDetails.EndDate,
					HostName = infoResponseResult.RaceDetails.HostName,
					IsDistanceActivated = infoResponseResult.RaceDetails.isDistanceActivated,
					IsIntensityActivated = infoResponseResult.RaceDetails.isIntensityActivated,
					IsLoginPossible = infoResponseResult.RaceDetails.isLoginPossible,
					IsRegisterTrainingPossible =
						infoResponseResult.RaceDetails.isRegisterTrainingPossible,
					MaxMinutesTotalPerDay = infoResponseResult.RaceDetails.MaxMinutesTotalPerDay,

					MaxPointsPerWeek = infoResponseResult.RaceDetails.MaxPointsPerWeek,
					RaceID = infoResponseResult.RaceDetails.RaceID,
					RaceTitle = infoResponseResult.RaceDetails.RaceTitle,
					StartDate = infoResponseResult.RaceDetails.StartDate,
					LoginURL = infoResponseResult.RaceDetails.LoginURL,
					RequiredMinutes = infoResponseResult.RaceDetails.MinutesRequiredForAnActivityToBeOk,
					MaxNumberOfDaysUserCanWaitToRegister = infoResponseResult.RaceDetails.MaxNumberOfDaysUserCanWaitToRegister
				};

				getParticipantOverviewResponse.Result.TeamInfo = new TeamInfo();
				getParticipantOverviewResponse.Result.TotalNumberOfPoints =
					response.Body.GetParticipantOverviewResult.TotalNumberOfPoints;

				getParticipantOverviewResponse.Result.ServerDate = new ServerDate
				{
					TodayTime = response.Body.GetParticipantOverviewResult.ServerDate.TodayTime,
					TodayDate = response.Body.GetParticipantOverviewResult.ServerDate.TodayDate
				};

				return new ApiResponse<GetParticipantOverviewResponse>(getParticipantOverviewResponse);
			});
		}

		//TODO implement activitiesOnly
		public override Task<ApiResponse<GetMyNewsFeedResponse>> GetMyNewsFeedAsync(bool activitiesOnly)
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new GetMyNewsFeedRequest
				{
					Body =
						new GetMyNewsFeedRequestBody(ApplicationId, LoginId, LoginSecretHashed,
							LanguageService.GetCurrentLanguage(), RaceId, Convert.ToInt16(activitiesOnly))
				};

				var response = await Task.Factory.FromAsync<MobileService.GetMyNewsFeedResponse>(
					client.BeginGetMyNewsFeed(request, null, null),
					client.EndGetMyNewsFeed);

				var getMyNewsFeedResponse = new GetMyNewsFeedResponse
				{
					Result = new List<GetMyNewsFeedResult>()
				};
				foreach (var item in response.Body.GetMyNewsFeedResult)
				{
					var itemNew = new GetMyNewsFeedResult
					{
						BaseName = item.BaseName,
						BasePoints = item.BasePoints,
						EventTime = item.EventTime,
						EventType = item.EventType,
						ImageURL = item.ImageURL,
						Minutes = item.Minutes,
						PersonName = item.PersonName,
						Points = item.Points,
						RelevanceRank = item.RelevanceRank,
						TranslatedName = item.TranslatedName,
						ThumbnailURL = item.ThumbnailURL,
						FullMessage = item.FullMessage,
						ReadMoreURL = item.ReadMoreURL,
					};

					getMyNewsFeedResponse.Result.Add(itemNew);
				}

				return new ApiResponse<GetMyNewsFeedResponse>(getMyNewsFeedResponse);
			});
		}

		public override Task<ApiResponse<GetTrainingTypesResponse>> GetTrainingTypesAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new GetTrainingTypesRequest
				{
					Body =
						new GetTrainingTypesRequestBody(ApplicationId, LoginId, LoginSecretHashed,
							LanguageService.GetCurrentLanguage(), RaceId)
				};

				var response = await Task.Factory.FromAsync<MobileService.GetTrainingTypesResponse>(
					client.BeginGetTrainingTypes(request, null, null),
					client.EndGetTrainingTypes);

				var getTrainingTypesResponse = new GetTrainingTypesResponse
				{
					Result = new List<GetTrainingTypesResult>()
				};
				foreach (var item in response.Body.GetTrainingTypesResult)
				{
					var itemNew = new GetTrainingTypesResult
					{
						ActivityCategoryID = item.ActivityCategoryID,
						ActivityCategoryName = item.ActivityCategoryName,
						DailyLimit = item.DailyLimit,
						Description = item.Description,
						IsIntensity = item.IsIntensity,
						TrainingTypeID = item.TrainingTypeID,
						TrainingTypeName = item.TrainingTypeName
					};

					if (item.Unit == TrainingTypeUnits.Fixed)
						itemNew.Unit = TrainingUnit.Fixed;
					else if (item.Unit == TrainingTypeUnits.Minutes)
						itemNew.Unit = TrainingUnit.Minutes;
					else if (item.Unit == TrainingTypeUnits.Score)
						itemNew.Unit = TrainingUnit.Score;
					else if (item.Unit == TrainingTypeUnits.Steps)
						itemNew.Unit = TrainingUnit.Steps;

					itemNew.UnitAsString = item.UnitAsString;
					itemNew.Weight = item.Weight;
					getTrainingTypesResponse.Result.Add(itemNew);
				}

				return new ApiResponse<GetTrainingTypesResponse>(getTrainingTypesResponse);
			});
		}

		public override Task<ApiResponse<GetTrainingTypesFrequentResponse>> GetFrequentTrainingTypesAsync()
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new GetTrainingTypesFrequentRequest
				{
					Body =
						new GetTrainingTypesFrequentRequestBody(ApplicationId, LoginId, LoginSecretHashed,
							LanguageService.GetCurrentLanguage(), RaceId)
				};

				var response = await Task.Factory.FromAsync<MobileService.GetTrainingTypesFrequentResponse>(
					client.BeginGetTrainingTypesFrequent(request, null, null),
					client.EndGetTrainingTypesFrequent);

				var getTrainingTypesResponse = new GetTrainingTypesFrequentResponse
				{
					Result = new List<GetTrainingTypesResult>()
				};

				foreach (var item in response.Body.GetTrainingTypesFrequentResult)
				{
					var itemNew = new GetTrainingTypesResult
					{
						ActivityCategoryID = item.ActivityCategoryID,
						ActivityCategoryName = item.ActivityCategoryName,
						DailyLimit = item.DailyLimit,
						Description = item.Description,
						IsIntensity = item.IsIntensity,
						TrainingTypeID = item.TrainingTypeID,
						TrainingTypeName = item.TrainingTypeName
					};

					if (item.Unit == TrainingTypeUnits.Fixed)
						itemNew.Unit = TrainingUnit.Fixed;
					else if (item.Unit == TrainingTypeUnits.Minutes)
						itemNew.Unit = TrainingUnit.Minutes;
					else if (item.Unit == TrainingTypeUnits.Score)
						itemNew.Unit = TrainingUnit.Score;
					else if (item.Unit == TrainingTypeUnits.Steps)
						itemNew.Unit = TrainingUnit.Steps;

					itemNew.UnitAsString = item.UnitAsString;
					itemNew.Weight = item.Weight;
					getTrainingTypesResponse.Result.Add(itemNew);
				}

				return new ApiResponse<GetTrainingTypesFrequentResponse>(getTrainingTypesResponse);
			});
		}

		public override Task<ApiResponse<Core.Models.SaveTrainingShareToFacebookResponse>> SaveTrainingShareToFacebookAsync(Training training, bool isShare)
		{
			return TryInvokeAsync(async () =>
			{
				var trainingSoap = new MobileService.Training
				{
					Description = training.Description,
					Distance = training.Distance,
					Intensity = training.Intensity,
					Minutes = training.Minutes,
					PersonID = training.PersonID,
					Points = training.Points,
					RaceID = training.RaceID,
					StartDateTime = training.StartDateTime,
					Steps = training.Steps,
					TrainingID = training.TrainingID,
					TrainingTypeID = training.TrainingTypeID
				};
				if (training.Unit == TrainingUnit.Fixed)
					trainingSoap.Unit = TrainingTypeUnits.Fixed;
				else if (training.Unit == TrainingUnit.Minutes)
					trainingSoap.Unit = TrainingTypeUnits.Minutes;
				else if (training.Unit == TrainingUnit.Score)
					trainingSoap.Unit = TrainingTypeUnits.Score;
				else if (training.Unit == TrainingUnit.Steps)
					trainingSoap.Unit = TrainingTypeUnits.Steps;

				var client = GetClient();
				var request = new SaveTrainingShareToFacebookRequest
				{
					Body =
						new SaveTrainingShareToFacebookRequestBody(ApplicationId, LoginId, LoginSecretHashed, trainingSoap, isShare)
				};

				var response = await Task.Factory.FromAsync<MobileService.SaveTrainingShareToFacebookResponse>(
					client.BeginSaveTrainingShareToFacebook(request, null, null),
					client.EndSaveTrainingShareToFacebook);

				var result = new Core.Models.SaveTrainingShareToFacebookResponse
				{
					TrainingID = response.Body.SaveTrainingShareToFacebookResult ?? -1
				};

				return new ApiResponse<Core.Models.SaveTrainingShareToFacebookResponse>(result);
			});
		}

		/*public override Task<ApiResponse<SaveTrainingResponse>> SaveTrainingAsync(Training training)
		{
			return TryInvokeAsync(async () =>
			{
				var client = GetClient();
				var request = new SaveTrainingRequest();
				var trainingSoap = new MobileService.Training
				{
					Description = training.Description,
					Distance = training.Distance,
					Intensity = training.Intensity,
					Minutes = training.Minutes,
					PersonID = training.PersonID,
					Points = training.Points,
					RaceID = training.RaceID,
					StartDateTime = training.StartDateTime,
					Steps = training.Steps,
					TrainingID = training.TrainingID,
					TrainingTypeID = training.TrainingTypeID
				};
				if (training.Unit == TrainingUnit.Fixed)
					trainingSoap.Unit = TrainingTypeUnits.Fixed;
				else if (training.Unit == TrainingUnit.Minutes)
					trainingSoap.Unit = TrainingTypeUnits.Minutes;
				else if (training.Unit == TrainingUnit.Score)
					trainingSoap.Unit = TrainingTypeUnits.Score;
				else if (training.Unit == TrainingUnit.Steps)
					trainingSoap.Unit = TrainingTypeUnits.Steps;

				request.Body = new SaveTrainingRequestBody(ApplicationId, LoginId, LoginSecretHashed, trainingSoap);

				var response = await Task.Factory.FromAsync<MobileService.SaveTrainingResponse>(
					client.BeginSaveTraining(request, null, null),
					client.EndSaveTraining);

				var result = new SaveTrainingResponse
				{
					TrainingID = response.Body.SaveTrainingResult ?? -1
				};
				return new ApiResponse<SaveTrainingResponse>(result);
			});
		} */

		public override Task<ApiResponse<GetServerDateTimeResponse>> GetServerDateTime()
		{
			throw new NotImplementedException();
		}

		public override Task<ApiResponse<SaveImageResponse>> SaveImageAsync(string trainingIds, byte[] imageBytes)
		{
			return TryInvokeAsync(async () =>
			{
				var result = await PostImageAsync<SaveImageResponse>(trainingIds, imageBytes);
				if (result.IsSuccess)
					return new ApiResponse<SaveImageResponse>(result.Response);
				throw new Exception(result.Message);
			});
		}

		//TODO this method is the same as a method in Droid.MotionRaceCilent.PostImageAsync<T>
		private async Task<ApiResponse<T>> PostImageAsync<T>(string trainingIds, byte[] imageBytes)
		{
			try
			{
				var httpClient = new HttpClient
				{
					BaseAddress = new Uri(WebServiceSettings.GetServiceData(Mode, ServiceCharacteristic.ApiImageUploadUrl))
				};

				var boundary = "----CustomBoundary" + DateTime.Now.Ticks.ToString("x");
				using (var content = new MultipartFormDataContent(boundary))
				{
					content.Add(new StreamContent(new MemoryStream(imageBytes)), "TrainingImageFile", "upload.jpg");
					content.Add(new StringContent(trainingIds), "TrainingIDs");

					var httpResponse = await httpClient.PostAsync("", content);
					if (!httpResponse.IsSuccessStatusCode)
					{
						var errorJson = await httpResponse.Content.ReadAsStringAsync();
						var apiError = JsonConvert.DeserializeObject<ApiErrorMessage>(errorJson);
						throw new Exception(apiError.Message);
					}

					var json = await httpResponse.Content.ReadAsStringAsync();
					if (json.Contains("<!DOCTYPE html PUBLIC"))
						json = json.Substring(0, json.IndexOf("<!DOCTYPE html PUBLIC", StringComparison.Ordinal) - 1);

					json = "{\"d\":\"" + json + "\"}";
					var objResponse = JsonConvert.DeserializeObject<T>(json);
					var response = new ApiResponse<T>(objResponse);
					if (!response.IsSuccess)
						response.ErrorMessage = response.Message;

					return response;
				}
			}
			catch (Exception)
			{
				return null;
			}
		}

		//TODO connect to cliein by Mode ()
		private MobileServiceSoap GetClient()
		{
			//return new ServicePROD.MobileServiceSoapClient();
			return new MobileServiceSoapClient(new BasicHttpBinding(), 
				new EndpointAddress(new Uri(WebServiceSettings.GetServiceData(Mode, ServiceCharacteristic.ApiUrl))));
		}
	}
}