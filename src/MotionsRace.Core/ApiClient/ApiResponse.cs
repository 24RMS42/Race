using MotionsRace.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using MvvmCross.Platform;

namespace MotionsRace.Core.ApiClient
{
    [DataContract]
    public class ApiResponse<T>
    {
        public string ApiErrorMessage
        {
            get
            {
                var langService = Mvx.Resolve<ILanguageService>();

				if (ErrorMessage.Contains("NotFoundOrInvalidPasswordException"))
                    return langService.GetString("GLOBAL_ERROR_API_LOGIN_FunBeatUserNotFoundOrPasswordIncorrect");
                if (ErrorMessage.Contains("TrainingPersonIDCannotDifferFromLoggedInPersonID"))
                    return langService.GetString("GLOBAL_ERROR_API_SAVE_TRAINING_TrainingPersonIDCannotDifferFromLoggedInPersonID");
                if (ErrorMessage.Contains("DailyLimitExceeded"))
                    return langService.GetString("GLOBAL_ERROR_API_SAVE_TRAINING_DailyLimitExceeded");
                if (ErrorMessage.Contains("CouldNotSaveTraining"))
                    return langService.GetString("GLOBAL_ERROR_API_SAVE_TRAINING_CouldNotSaveTraining");
                if (ErrorMessage.Contains("CouldNotPostToFacebook"))
                    return langService.GetString("GLOBAL_ERROR_API_SAVE_TRAINING_CouldNotPostToFacebook");
				if (ErrorMessage.Contains("Error: NameResolutionFailure"))
					return langService.GetString("GLOBAL_NoInternetConnection");
				if (ErrorMessage.Contains("Error: ConnectFailure (Network is unreachable)"))
					return langService.GetString("GLOBAL_NoInternetConnection");

                return ErrorMessage;
            }
        }

        public string ErrorMessage { get; set; }
        public Exception Exception { get; set; }

        [DataMember(Name = "Success")]
        public bool IsSuccess { get; set; }
        [DataMember(Name = "Message")]
        public string Message { get; set; }
        [DataMember(Name = "Response")]
        public T Response { get; set; }

        public ApiResponse()
        {

        }

        public ApiResponse(T response, bool isSuccess = true)
        {
            Response = response;
            IsSuccess = isSuccess;
        }
    }

}
