using MotionsRace.Core.Models;
using MotionsRace.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Xml.Serialization;
using MotionsRace.Core.Services;
using System.Text.RegularExpressions;
using MotionsRace.Core.ApiClient;
using MotionsRace.Core;
using System.Net.Http;
using System.Xml;
using MvvmCross.Platform;

namespace MotionsRace
{
	public class MotionsRaceClient
	{
		private WebServiceMode _webServiceMode;

		private static string boundary = "----CustomBoundary" + DateTime.Now.Ticks.ToString("x");

		public MotionsRaceClient(WebServiceMode webServiceMode)
		{
			_webServiceMode = webServiceMode;
		}

		public async Task<ApiResponse<ValidateAndCreateSecretResponse>> ValidateAndCreateSecret(
			ValidateAndCreateSecretParams parameters)
		{
			return await PostAsync<ValidateAndCreateSecretResponse>("ValidateAndCreateSecret", parameters);
		}

		public async Task<ApiResponse<GetRacesResponse>> GetRacesAPersonCanLoginTo(GetRacesParams parameters)
		{
			return await PostAsync<GetRacesResponse>("GetRacesAPersonCanLoginTo", parameters);
		}

		public async Task<ApiResponse<AuthorizeRaceAccessResponse>> AuthorizeRaceAccess(AuthorizeRaceAccessParams parameters)
		{
			return await PostAsync<AuthorizeRaceAccessResponse>("AuthorizeRaceAccess", parameters);
		}

		public async Task<ApiResponse<GetParticipantOverviewResponse>> GetParticipantOverview(
			GetParticipantOverviewParams parameters)
		{
			return await PostAsync<GetParticipantOverviewResponse>("GetParticipantOverview", parameters);
		}

		public async Task<ApiResponse<GetMyNewsFeedResponse>> GetMyNewsFeed(GetMyNewsFeedParams parameters)
		{
			return await PostAsync<GetMyNewsFeedResponse>("GetMyNewsFeed", parameters);
		}

		public async Task<ApiResponse<GetTrainingTypesResponse>> GetTrainingTypes(GetTrainingTypesParams parameters)
		{
			return await PostAsync<GetTrainingTypesResponse>("GetTrainingTypes", parameters);
		}

		public async Task<ApiResponse<GetTrainingTypesFrequentResponse>> GetFrequentTrainingTypes(GetTrainingTypesParams parameters)
		{
			return await PostAsync<GetTrainingTypesFrequentResponse>("GetTrainingTypesFrequent", parameters);
		}

		[Obsolete]
		public async Task<ApiResponse<SaveTrainingResponse>> SaveTraining(SaveTrainingParams parameters)
		{
			return await PostAsync<SaveTrainingResponse>("SaveTraining", parameters);
		}

		public async Task<ApiResponse<SaveTrainingShareToFacebookResponse>> SaveTrainingShareToFacebook(SaveTrainingShareToFacebookParams parameters)
		{
			return await PostAsync<SaveTrainingShareToFacebookResponse>("SaveTrainingShareToFacebook", parameters);
		}

		public async Task<ApiResponse<GetServerDateTimeResponse>> GetServerDateTime()
		{
			return await PostAsync<GetServerDateTimeResponse>("GetServerDateTime", null);
		}



		public async Task<ApiResponse<SaveImageResponse>> SaveImage(string trainingIds, byte[] imageBytes)
		{
			return await PostImageAsync<SaveImageResponse>(trainingIds, imageBytes);
		}




		#region Helper Methods

		private async Task<ApiResponse<T>> PostAsync<T>(string api, object parameters)
		{
			var xmlResult = "";

			try
			{
				//var jsonSerSettings = new DataContractJsonSerializerSettings();
				StringContent theContent = new StringContent("");
				string serializerResult = "";
				if (parameters != null)
				{
					XmlSerializer xmlSer = new XmlSerializer(parameters.GetType());
					// use the serializer to write the object to a MemoryStream 
					MemoryStream ms = new MemoryStream();
					xmlSer.Serialize(ms, parameters);
					ms.Position = 0;

					//use a Stream reader to construct the StringContent (Json) 
					StreamReader sr = new StreamReader(ms);

					serializerResult = sr.ReadToEnd();

					// Refrorm result, delete xml header and remove xmlns:xsi and xmlns:xsd attributes
					var doc = new XmlDocument();
					doc.LoadXml(serializerResult);
					XmlElement root = doc.DocumentElement;
					root.RemoveAttribute("xmlns:xsi");
					root.RemoveAttribute("xmlns:xsd");
					root.SetAttribute("xmlns", Constants.SOAP_NAMESPACE);
					serializerResult = doc.InnerXml;
					serializerResult = Regex.Replace(serializerResult, "<\\?xml[^>]*?>", "");
					serializerResult = serializerResult.Replace("\n", "");

				}
				if (api == "GetServerDateTime")
				{
					serializerResult = "<GetServerDateTime xmlns=\"http://service.funbeatrace.com/MobileService.asmx\" />";
				}
				theContent = new StringContent(
					"<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\"><s:Body>" + serializerResult +
					"</s:Body></s:Envelope>",
					Encoding.UTF8, "text/xml");


				var httpClient = new HttpClient();
				// TODO: works only on EN, fix on other locale, tested on WP
				httpClient.DefaultRequestHeaders.Connection.Add("keep-alive");
				httpClient.DefaultRequestHeaders.ConnectionClose = false;
				httpClient.DefaultRequestHeaders.Add("SOAPAction", "\"" + Constants.SOAP_NAMESPACE + "/" + api + "\"");
				httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
				httpClient.BaseAddress = new Uri(GetBaseApiUrl());
				httpClient.Timeout = TimeSpan.FromSeconds(20);

				var httpResponse = await httpClient.PostAsync("", theContent);
				if (!httpResponse.IsSuccessStatusCode)
				{
					var errorSoap = await httpResponse.Content.ReadAsStringAsync();
					if (string.IsNullOrWhiteSpace(errorSoap))
						throw new Exception("api call error: " + httpResponse.StatusCode);

                    if (!errorSoap.Contains("<faultstring>"))
                    {
                        throw new Exception(Mvx.Resolve<ILanguageService>().GetString("GLOBAL_Server_not_available"));
				    }

				    ApiErrorMessage apiError = new ApiErrorMessage
				    {
				        Message = errorSoap.Substring(errorSoap.IndexOf("<faultstring>") + 13,
				            errorSoap.Length - (errorSoap.IndexOf("<faultstring>") + 13) -
				            (errorSoap.Length - (errorSoap.IndexOf("</faultstring>"))))
				    };

				    if (errorSoap.Contains("<ExceptionType>"))
						apiError.ExceptionType = errorSoap.Substring(errorSoap.IndexOf("<ExceptionType>") + 15,
							errorSoap.Length - (errorSoap.IndexOf("<ExceptionType>") + 15) -
							(errorSoap.Length - (errorSoap.IndexOf("</ExceptionType>"))));
					else
						apiError.ExceptionType = errorSoap.Substring(errorSoap.IndexOf("<faultcode>") + 11,
							errorSoap.Length - (errorSoap.IndexOf("<faultcode>") + 11) -
							(errorSoap.Length - (errorSoap.IndexOf("</faultcode>"))));
					throw new Exception(apiError.Message);
				}

				IEnumerable<string> encodingItems;
				var gzipEncoded = httpResponse.Content.Headers.TryGetValues("Content-Encoding", out encodingItems) &&
				                  encodingItems.Contains("gzip");

				if (gzipEncoded)
				{
					var gzipBytes = await httpResponse.Content.ReadAsByteArrayAsync();

					var platformService = Mvx.Resolve<IPlatformService>();
					if (platformService.Platform == Platform.iOS)
						xmlResult = platformService.UnGZipByteArray(gzipBytes);
					else
						xmlResult = UnGZip(gzipBytes);
				}
				else
				{
					xmlResult = await httpResponse.Content.ReadAsStringAsync();
				}

				xmlResult = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
				            xmlResult.Substring(xmlResult.IndexOf("<soap:Body>") + 11,
					            xmlResult.Length - (xmlResult.IndexOf("<soap:Body>") + 11) -
					            (xmlResult.Length - (xmlResult.IndexOf("</soap:Body>"))));
				xmlResult = xmlResult.Replace("xmlns=\"" + Constants.SOAP_NAMESPACE + "\"",
					"xmlns=\"" + Constants.SOAP_NAMESPACE + "\"" + " xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"");
				XmlSerializer xmlDes = new XmlSerializer(typeof (T));
				TextReader reader = new StringReader(xmlResult);
				var objResponse = (T) xmlDes.Deserialize(reader);
				var response = new ApiResponse<T>(objResponse);

				if (!response.IsSuccess)
					response.ErrorMessage = response.Message;

				return response;
			}
			catch (Exception ex)
			{
				return HandleException<T>(ex, api, xmlResult, parameters);
			}
		}

		private ApiResponse<T> HandleException<T>(Exception ex, string api, string json, object parameters)
		{
			var msg = "";
			var response = new ApiResponse<T>();
			response.IsSuccess = false;
			response.Message = ex.Message;

			if (ex is AggregateException)
			{

				var aEx = ex as AggregateException;

				if (aEx.InnerExceptions.Count > 0)
				{
					response.Exception = aEx.InnerExceptions[0];
					response.ErrorMessage = aEx.InnerExceptions[0].Message + Environment.NewLine + msg;
				}
				else
				{
					response.Exception = ex;
					response.ErrorMessage = ex.Message + Environment.NewLine + msg;
				}
			}
			else
			{
				response.Exception = ex;
				response.ErrorMessage = ex.Message + Environment.NewLine + msg;
			}

			return response;
		}


		private async Task<ApiResponse<T>> PostImageAsync<T>(string trainingIds, byte[] imageBytes)
		{
			string json = "";
			try
			{
				var httpClient = new HttpClient();
				httpClient.BaseAddress = new Uri(GetBaseApiImageUploadUrl());


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

					json = await httpResponse.Content.ReadAsStringAsync();
					if (json.Contains("<!DOCTYPE html PUBLIC"))
						json = json.Substring(0, json.IndexOf("<!DOCTYPE html PUBLIC") - 1);

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

		private string GetBaseApiUrl()
		{
			return WebServiceSettings.GetServiceData(_webServiceMode, ServiceCharacteristic.ApiUrl);
		}

		private string GetBaseApiImageUploadUrl()
		{
			return WebServiceSettings.GetServiceData(_webServiceMode, ServiceCharacteristic.ApiImageUploadUrl);
		}


		public static string UnGZip(byte[] data)
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

		#endregion
	}
}
