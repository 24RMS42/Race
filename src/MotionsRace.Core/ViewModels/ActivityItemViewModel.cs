using Acr.UserDialogs;
using MotionsRace.Core.Helpers;
using MotionsRace.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;
using MotionsRace.Core.Models;
using System.Collections.ObjectModel;
using MotionsRace.Core.Messages;
using System.IO;
using MotionsRace.Core.ApiClient;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;
using MvvmCross.Plugins.PictureChooser;
using MvvmCross.Platform.UI;
using MvvmCross.Platform;

namespace MotionsRace.Core.ViewModels
{
	public class ActivityItemViewModel : HeaderScreenViewModel
	{
		private readonly IMessageService _messageService;
		private readonly IDataService _dataService;
		private readonly ISettingsService _settingsService;
		private readonly ILanguageService _languageService;
		private readonly IWebService _webService;
		private readonly IPhotoService _photoService;
		private readonly IImageResizeService _imageResizeService;
		private readonly Timer _timer;
		private static MvxSubscriptionToken _imageReceiveToken;
		private int? _maxNumberOfDaysUserCanWaitToRegister;
		private byte[] _imageBytes;
		private string _trainingIDs;
		private int _trainingId;

		public bool IsLiveRegister;
		public readonly IMvxPictureChooserTask ChooseService;
		public Action AnimateLiveRecordButtonSlide { get; set; }
		public Action<bool> AnimateLiveRecordClock { get; set; }


		private bool _showShareToFacebookCheckBox;
		public bool ShowShareToFacebookCheckBox {
			get {
				return _showShareToFacebookCheckBox;
			}
			set {
				_showShareToFacebookCheckBox = value;
				RaisePropertyChanged(() => ShowShareToFacebookCheckBox);
			}
		}

		private bool _isShareToFacebook;
		public bool IsShareToFacebook {
			get {
				return _isShareToFacebook;
			}
			set {
				_isShareToFacebook = value;
				RaisePropertyChanged(() => IsShareToFacebook);
			}
		}

		private MvxColor _saveButtonColor;
		public MvxColor SaveButtonColor
		{
			get { return _saveButtonColor; }
			set
			{
				if (_saveButtonColor != value)
				{
					_saveButtonColor = value;
					RaisePropertyChanged(() => SaveButtonColor);
				}
			}
		}

		private MvxColor _pauseResumeButtonColor;
		public MvxColor PauseResumeButtonColor
		{
			get { return _pauseResumeButtonColor; }
			set
			{
				if (_pauseResumeButtonColor != value)
				{
					_pauseResumeButtonColor = value;
					RaisePropertyChanged(() => PauseResumeButtonColor);
				}
			}
		}

		private MvxColor _pauseResumeButtonTextColor;
		public MvxColor PauseResumeButtonTextColor
		{
			get { return _pauseResumeButtonTextColor; }
			set
			{
				if (_pauseResumeButtonTextColor != value)
				{
					_pauseResumeButtonTextColor = value;
					RaisePropertyChanged(() => PauseResumeButtonTextColor);
				}
			}
		}

		private ObservableCollection<ActivityCheckBox> _dateCheckBoxes;
		public ObservableCollection<ActivityCheckBox> DateCheckBoxes
		{
			get { return _dateCheckBoxes; }
			set
			{
				if (_dateCheckBoxes != value)
				{
					_dateCheckBoxes = value;
					RaisePropertyChanged(() => DateCheckBoxes);
				}
			}
		}

		private bool _isIntensityVisible;
		public bool IsIntensityVisible
		{
			get { return _isIntensityVisible; }
			set
			{
				_isIntensityVisible = value;
				RaisePropertyChanged(() => IsIntensityVisible);
			}
		}

		private string _unitType;
		public string UnitType
		{
			get { return _unitType; }
			set
			{
				_unitType = value;
				RaisePropertyChanged(() => UnitType);
			}
		}

		private string _unitValue;
		public string UnitValue
		{
			get { return _unitValue; }
			set
			{
				_unitValue = value;
				RaisePropertyChanged(() => UnitValue);
			}
		}

		private string _distanceValue;
		public string DistanceValue
		{
			get { return _distanceValue; }
			set
			{
				_distanceValue = value;
				RaisePropertyChanged(() => DistanceValue);
			}
		}

		private bool _unitValueVisibility;
		public bool UnitValueVisibility
		{
			get { return _unitValueVisibility; }
			set
			{
				_unitValueVisibility = value;
				RaisePropertyChanged(() => UnitValueVisibility);
			}
		}

		private bool _distanceVisibility;
		public bool DistanceVisibility
		{
			get { return _distanceVisibility; }
			set
			{
				_distanceVisibility = value;
				RaisePropertyChanged(() => DistanceVisibility);
			}
		}

		private bool _liveRecord;
		public bool LiveRecord
		{
			get { return _liveRecord; }
			set
			{
				_liveRecord = value;
				RaisePropertyChanged(() => LiveRecord);
			}
		}

		private bool _imageSelected;
		public bool ImageSelected
		{
			get { return _imageSelected; }
			set
			{
				_imageSelected = value;
				RaisePropertyChanged(() => ImageSelected);
				RaisePropertyChanged(() => ImageSelectBackground);
				RaisePropertyChanged(() => UploadImageText);
			}
		}

		private string _pauseResumeButtonTitle;
		public string PauseResumeButtonTitle
		{
			get { return _pauseResumeButtonTitle; }
			set
			{
				_pauseResumeButtonTitle = value;
				RaisePropertyChanged(() => PauseResumeButtonTitle);
			}
		}

		private string _startCancelButtonTitle;
		public string StartCancelButtonTitle
		{
			get { return _startCancelButtonTitle; }
			set
			{
				_startCancelButtonTitle = value;
				RaisePropertyChanged(() => StartCancelButtonTitle);
			}
		}

		private string _time;
		public string LiveRecordTime
		{
			get { return _time; }
			set
			{
				_time = value;
				RaisePropertyChanged(() => LiveRecordTime);
			}
		}

		private bool _liveRecordVisibility;
		public bool LiveRecordVisibility
		{
			get { return _liveRecordVisibility; }
			set
			{
				_liveRecordVisibility = value;
				RaisePropertyChanged(() => LiveRecordVisibility);
			}
		}

		private bool _liveRecordNoteVisibility;
		public bool LiveRecordNoteVisibility
		{
			get { return _liveRecordNoteVisibility; }
			set
			{
				_liveRecordNoteVisibility = value;
				RaisePropertyChanged(() => LiveRecordNoteVisibility);
			}
		}

		private bool _showRefreshButton;
		public bool ShowRefreshButton
		{
			get { return _showRefreshButton; }
			set
			{
				_showRefreshButton = value;
				RaisePropertyChanged(() => ShowRefreshButton);
			}
		}

		private string _liveRecordNoteText;

		public string LiveRecordNoteText
		{
			get { return _liveRecordNoteText; }
			set
			{
				_liveRecordNoteText = value;
				RaisePropertyChanged(() => LiveRecordNoteText);
			}
		}

		private bool _liveRecordCancelButtonVisibility;
		public bool LiveRecordCancelButtonVisibility
		{
			get { return _liveRecordCancelButtonVisibility; }
			set
			{
				_liveRecordCancelButtonVisibility = value;
				RaisePropertyChanged(() => LiveRecordCancelButtonVisibility);
			}
		}

		public IMvxCommand LiveRecordPauseResumeCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (!_timer.IsRunning)
					{
						_timer.Start();
						PauseResumeButtonColor = Theme.Colors.ActivityCancelButtonColor;
						PauseResumeButtonTextColor = Theme.Colors.ActivityCancelButtonTextColor;
						PauseResumeButtonTitle = this["Live_Record_Pause"];
					}
					else
					{
						_timer.Stop();
						PauseResumeButtonColor = Theme.Colors.ActivityResumeButtonColor;
						PauseResumeButtonTextColor = Theme.Colors.ActivityResumeButtonTextColor;
						PauseResumeButtonTitle = this["Live_Record_Resume"];
					}

					if (AnimateLiveRecordClock != null)
					{
						AnimateLiveRecordClock.Invoke(_timer.IsRunning);
					}
				});
			}
		}

		public IMvxCommand LiveRecordFinishCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (_timer.IsRunning)
					{
						LiveRecordPauseResumeCommand.Execute();
					}
					_timer.Stop();
					// Today check box must be
					DateCheckBoxes[0].Checked = true;
					SelectedTraining.Unit = TrainingUnit.Minutes;
					UnitValue = Math.Round(_timer.Elapsed.TotalMinutes, 0).ToString();
					LiveRecordVisibility = false;
					IsLiveRegister = false;
				});
			}
		}

		public IMvxCommand ConfirmExitCommand
		{
			get
			{
				return new MvxCommand(async () =>
				{
					var mustExit = await Mvx.Resolve<IUserDialogs>().ConfirmAsync(
						this["LiveRecord_ConfirmExit"],
						this["GLOBAL_Confirm_Dialog_Title"],
						this["GLOBAL_Exit_Dialog_Button_Yes"],
						this["GLOBAL_Exit_Dialog_Button_No"]);
					if (mustExit)
					{
						IsLiveRegister = false;
						Close(this);
					}
				});
			}
		}

		public IMvxCommand LiveRecordStartCancelCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (!_timer.IsRunning)
					{
						LiveRecordPauseResumeCommand.Execute();
						LiveRecordNoteVisibility = true;
						StartCancelButtonTitle = this["Live_Record_Cancel"];
						IsLiveRegister = true;
					}
					else
					{
						_timer.Stop();
						IsLiveRegister = false;
						CancelCommand.Execute();
					}
				});
			}
		}

		public IMvxCommand LiveRecordCancelCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					if (!IsLiveRegister)
					{
						IsLiveRegister = false;
						CancelCommand.Execute();
					}
				});
			}
		}

		public MvxColor ImageSelectBackground { get { return ImageSelected ? Theme.Colors.ImageSelectBackgroundColor : Theme.Colors.ImageBackgroundColor; } }

		public string UploadImageText { get { return ImageSelected ? this["Activity_ImageChoosen"] : this["Activity_UploadAnyImage"]; } }

		public string TrainingTypeName
		{
			get { return SelectedTraining != null ? SelectedTraining.TrainingTypeName : ""; }
		}

		public string TrainingDescription
		{
			get { return SelectedTraining != null ? SelectedTraining.Description : ""; }
		}

		public bool DescriptionVisibility
		{
			get { return !string.IsNullOrEmpty(TrainingDescription); }
		}

		public bool MatchRaceDateRange(int pos)
		{
			bool ifDisplay = true;
			var date = (_settingsService.Options.ServerDate).AddDays(-pos);
			//if (!(_maxNumberOfDaysUserCanWaitToRegister != null && _maxNumberOfDaysUserCanWaitToRegister >= pos))
			//	ifDisplay = false;
			//if (_settingsService.Options.RaceStartDate > date || date > _settingsService.Options.RaceEndDate)
			if (_settingsService.Options.MinvalidRegistrationDate.Date > date.Date || date.Date > _settingsService.Options.MaxvalidRegistrationDate.Date)
				ifDisplay = false;
			return ifDisplay;
		}

		private int _intensitySelected;
		public int IntensitySelected
		{
			get { return _intensitySelected; }
			set
			{
				_intensitySelected = value;
				RaisePropertyChanged(() => IntensitySelected);
			}
		}

		private string _comments;
		public string Comments
		{
			get { return _comments; }
			set
			{
				_comments = value;
				RaisePropertyChanged(() => Comments);
			}
		}

		public string NoteText
		{
			get
			{
				var options = Mvx.Resolve<ISettingsService>().Options;
				if (options.MaxPointsPerWeek != null)
					return string.Format(this["Activity_NoteTextWeekMax"], options.MaxPointsPerWeek);
				if (options.MaxMinutesTotalPerDay != null)
					return string.Format(this["Activity_NoteTextDayMax"], options.MaxMinutesTotalPerDay);
				return this["Activity_NoteText"];
			}
		}

		private GetTrainingTypesResult _selectedTraining;
		public GetTrainingTypesResult SelectedTraining
		{
			get { return _selectedTraining; }
			set
			{
				_selectedTraining = value;
				RaisePropertyChanged(() => SelectedTraining);
				RaisePropertyChanged(() => DescriptionVisibility);
				RaisePropertyChanged(() => TrainingDescription);
				RaisePropertyChanged(() => TrainingTypeName);
			}
		}

		public ActivityItemViewModel(INavigationService navigationService, IMessageService messageService, IDataService dataService,
			ISettingsService settingsService, ILanguageService languageService, IWebService webService, IPhotoService photoService,
			IUserDialogs dialogueService, IPlatformService platformService, IImageResizeService imageResizeService,
			IMvxPictureChooserTask chooseService, IMvxMessenger messenger)
			: base(navigationService, dialogueService, platformService, messenger)
		{
			_messageService = messageService;
			_dataService = dataService;
			_settingsService = settingsService;
			_languageService = languageService;
			_webService = webService;
			_photoService = photoService;
			ChooseService = chooseService;
			_imageResizeService = imageResizeService;
			_imageReceiveToken = Messenger.Subscribe<ImageChooseMessage>(message =>
			{
				//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "ImageChooseMessage subscriber received");

				if (message.ImageBytes == null || message.ImageBytes.Length == 0)
				{
					//Mvx.Resolve<IMessageService>().ShowAlertAsync("", Mvx.Resolve<ILanguageService>().GetString("Activity_PictureError"));
					return;
				}
				ImageRecieved(message.ImageBytes);
			});

			ShowShareToFacebookCheckBox = _settingsService.Options.ShowShareToFacebook;
			IsShareToFacebook = _settingsService.Options.IsShareToFacebook;

			_timer = new Timer();
			_timer.TimerEvent += onTimer;

			initLiveRecord();
		}

		protected override bool OnTryClose()
		{
			if (_timer.IsRunning)
			{
				_timer.Stop();
			}
			return base.OnTryClose();
		}

		public void OnResume()
		{
			var settingsLiveRecord = _settingsService.Options.LiveRecord;
			if (settingsLiveRecord != null)
			{
				var elapsed = settingsLiveRecord.TimerElapsed;
				if (settingsLiveRecord.TimerIsRunning && settingsLiveRecord.PauseDateTime != DateTime.MinValue)
				{
					elapsed = elapsed + (DateTime.Now - settingsLiveRecord.PauseDateTime);
				}

				IsLiveRegister = true;
				LiveRecordNoteVisibility = settingsLiveRecord.NoteVisibility;
				StartCancelButtonTitle = this["Live_Record_Cancel"];
				LiveRecordTime = _timer.Elapsed.ToString(@"hh\:mm\:ss");
				_timer.Reset(elapsed, !settingsLiveRecord.TimerIsRunning);
				LiveRecordPauseResumeCommand.Execute();
				_settingsService.Options.LiveRecord = null;
				_settingsService.Save();
			}
		}

		public void OnPause()
		{
			if (!IsLiveRegister)
			{
				return;
			}
			var settingsLiveRecord = new LiveRecordSettings
			{
				TrainingId = _trainingId,
				PauseDateTime = _timer.IsRunning ? DateTime.Now : DateTime.MinValue,
				TimerElapsed = _timer.Elapsed,
				TimerIsRunning = _timer.IsRunning,
				NoteVisibility = LiveRecordNoteVisibility,
			};
			_settingsService.Options.LiveRecord = settingsLiveRecord;
			_settingsService.Save();
		}

		private void onTimer()
		{
			LiveRecordTime = _timer.Elapsed.ToString(@"hh\:mm\:ss");
			if (LiveRecordCancelButtonVisibility && _timer.Elapsed.TotalMinutes >= 1)
			{
				if (AnimateLiveRecordButtonSlide != null)
				{
					Task.Run(AnimateLiveRecordButtonSlide);
				}
				LiveRecordNoteText = getLiverecordNote(false);
				LiveRecordCancelButtonVisibility = false;
			}
		}

		public void Init(bool liveRecord, int trainingId)
		{
			if (_settingsService.Options.LastRunDateTimeGetParticipantOverview.HasValue &&
				DateTime.UtcNow > _settingsService.Options.LastRunDateTimeGetParticipantOverview.Value.AddHours(6)) 
			{
				ShowRefreshButton = true;
			}

			SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
			if (PlatformService.Platform == Platform.WindowsPhone)
				NavigationService.ClearPrevPageFromBackStack();
			_trainingId = trainingId;
			loadDate(liveRecord, trainingId);
		}

		private void loadDate(bool liveRecord, int trainingId)
		{
			LiveRecord = liveRecord;
			SelectedTraining = _dataService.GetTrainingTypeById(trainingId);
			// If LiveRecord then register activity only for today 
			_maxNumberOfDaysUserCanWaitToRegister = LiveRecord ? 0 : _settingsService.Options.MaxNumberOfDaysUserCanWaitToRegister;


			IsIntensityVisible = SelectedTraining.IsIntensity && _settingsService.Options.IsIntensityActivated;

			if (_settingsService.Options.IsDistanceActivated)
				DistanceVisibility = true;

			if (SelectedTraining.Unit == TrainingUnit.Minutes || SelectedTraining.Unit == TrainingUnit.Steps)
			{
				UnitValueVisibility = true;
				UnitType = this["Activity_" + SelectedTraining.UnitAsString];
				UnitValue = "";
			}

			// Count of registration day
			var registrationDays = (_settingsService.Options.MaxvalidRegistrationDate.Date -
				_settingsService.Options.MinvalidRegistrationDate.Date).TotalDays;

			DateCheckBoxes = new ObservableCollection<ActivityCheckBox>();
			for (var day = registrationDays; day >= 0; day--)
			{
				var date = _settingsService.Options.MinvalidRegistrationDate.AddDays(day).Date;
				var dateCheckBox = new ActivityCheckBox
				{
					TextColor = Theme.Colors.ActivityItemTextColor,
					DateText =
						date == DateTime.Now.Date 
							? this["GLOBAL_Today"] 
							: (date == DateTime.Now.Date.AddDays(-1) 
								? this["GLOBAL_Yesterday"] 
								: date.ToString("dddd, MMMM dd", _languageService.GetCurrentCulture())),
					Date = date,
					ViewModel = this
				};
				DateCheckBoxes.Add(dateCheckBox);
			}

			// Show liveRecord only for TrainingUnit.Minutes
			LiveRecordVisibility = liveRecord && SelectedTraining.Unit == TrainingUnit.Minutes;
			if (LiveRecordVisibility)
			{
				initLiveRecord();
			}

			//IsBusy = false;
		}

		private void initLiveRecord()
		{
			IsLiveRegister = false;
			if (_timer != null)
			{
				_timer.Reset();
			}
			StartCancelButtonTitle = this["Live_Record_Start"];
			PauseResumeButtonTitle = this["Live_Record_Pause"];
			LiveRecordTime = "00:00:00";
			PauseResumeButtonColor = Theme.Colors.ActivitySaveButtonColor;
			PauseResumeButtonTextColor = Theme.Colors.ActivityResumeButtonTextColor;
			LiveRecordCancelButtonVisibility = true;
			LiveRecordNoteVisibility = false;
			LiveRecordNoteText = getLiverecordNote(true);
		}

		private string getLiverecordNote(bool fullMessage)
		{
			var note2 = _settingsService.Options.RequiredMinutes != null
					? string.Format(this["LiveRecord_NoteLabel2"], _settingsService.Options.RequiredMinutes)
					: string.Empty;
			if (fullMessage)
			{
				return string.Concat(this["LiveRecord_NoteLabel1"], " ", note2);
			}

			return note2;
		}

		private async Task saveActivityAsync()
		{
			IsBusy = true;
			SaveButtonColor = Theme.Colors.ActivitySaveButtonPressedColor;
			if (!PlatformService.IsInternetAvailable())
			{
				await DialogService.AlertAsync(this["GLOBAL_NoInternetConnection"], this["GLOBAL_Error"], this["GLOBAL_Ok"]);
				IsBusy = false;
				SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
				return;
			}

			_trainingIDs = "";

			// Validations

			if (!(DateCheckBoxes.Any(x => x.Checked)))
			{
				_messageService.ShowAlertAsync("", this["Activity_SelectAtLeastOneDay"]);
				IsBusy = false;
				SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
				return;
			}

			if (UnitValueVisibility)
			{
				int tryUnit;
				int.TryParse(UnitValue, out tryUnit);

				if (tryUnit == 0)
				{
					if (SelectedTraining.Unit == TrainingUnit.Minutes)
						_messageService.ShowAlertAsync("", this["Activity_EnterValidMinutes"]);
					if (SelectedTraining.UnitAsString == TrainingUnit.Steps.ToString())
						_messageService.ShowAlertAsync("", this["Activity_EnterValidSteps"]);
					IsBusy = false;
					SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
					return;
				}
			}

			var isShate = ShowShareToFacebookCheckBox && IsShareToFacebook;

			var countCheckedDays = 0;
			var countCheckedDaysSaved = 0;
			var countDaylyLimitExceeded = 0;
			foreach (var item in DateCheckBoxes)
			{
				if (item.Checked)
				{
					countCheckedDays++;
					var saveResult = await saveTrainingAsync(item.Date, isShate);

					if (saveResult.IsSuccess)
					{
						countCheckedDaysSaved++;
						var trainingId = saveResult.Response.TrainingID;
						if (trainingId != 0)
							_trainingIDs += trainingId + ",";
					}
					else
					{
						if (saveResult.ErrorMessage == Mvx.Resolve<ILanguageService> ().GetString ("GLOBAL_Server_not_available")) {
							_messageService.ShowAlertAsync ("", saveResult.ErrorMessage);
							IsBusy = false;
							SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
							return;
						} else if (saveResult.ErrorMessage.Contains ("DailyLimitExceeded")) {
							countDaylyLimitExceeded++;
						} 
						else {
							_messageService.ShowAlertAsync ("", saveResult.ErrorMessage);
							IsBusy = false;
							SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
							return;
						}
					}
				}
			}

			if (_imageBytes != null && countCheckedDaysSaved > 0)
			{
				//if (_platformService.Platform == Platform.Android) //TODO: remove when done ImageResizeService!
				await _webService.SaveImageAsync(_trainingIDs.Trim(','), _imageBytes);
			}

			if (countCheckedDays > 0 && countCheckedDays == countCheckedDaysSaved)
			{
				_messageService.ShowAlertAsync("", this["Activity_AllTrainingsSaved"]);
			}
			else if (countCheckedDays > 0 && countCheckedDays > countCheckedDaysSaved && countCheckedDaysSaved > 0)
			{
				_messageService.ShowAlertAsync("", this["Activity_SavedNotAllDays"]);
			}
			else if (countCheckedDays == 1 && countCheckedDaysSaved == 0 && countDaylyLimitExceeded > 0)
			{
				_messageService.ShowAlertAsync("", this["Activity_TrainingLimitExceeded1Day"]);
				SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
				IsBusy = false;
				return;
			}
			else if (countCheckedDays > 0 && countCheckedDaysSaved == 0)
			{
				if (countCheckedDays == 1)
				{
					_messageService.ShowAlertAsync("", this["Activity_TrainingLimitExceeded1Day"]);
					SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
					IsBusy = false;
					return;
				}
				_messageService.ShowAlertAsync("", this["Activity_TrainingLimitExceededAllDays"]);
				SaveButtonColor = Theme.Colors.ActivitySaveButtonColor;
				IsBusy = false;
				return;
			}

			// Send message to refresh feeds
			var message = new RefreshNewsFeedListMessage(this);
			Messenger.Publish(message);

			ShowViewModel<MainViewModel>();
			
			Close(this);
		}

		private async Task<ApiResponse<SaveTrainingShareToFacebookResponse>> saveTrainingAsync(DateTime date, bool isShare)
		{
			var training = new Training { Description = Comments };

			if (SelectedTraining.Unit == TrainingUnit.Minutes)
			{
				// TODO : correct validate
				training.Minutes = int.Parse(UnitValue);
			}
			else if (SelectedTraining.UnitAsString == TrainingUnit.Steps.ToString())
			{
				// TODO : correct validate
				training.Steps = int.Parse(UnitValue);
			}
				
			if (IsIntensityVisible) {
				if (PlatformService.Platform == Platform.Android) {
					training.Intensity = IntensitySelected + 1;
				} else {
					training.Intensity = IntensitySelected;
				}
			} else {
				training.Intensity = 1;
			}

			training.PersonID = _settingsService.Options.PersonID;
			if (_settingsService.Options.RaceID != null) 
				training.RaceID = _settingsService.Options.RaceID.Value;
			training.TrainingTypeID = SelectedTraining.TrainingTypeID;
			training.Unit = SelectedTraining.Unit;
			training.StartDateTime = date;

			if (DistanceValue != null)
			{
				double dist;
				if (double.TryParse(DistanceValue, out dist))
					training.Distance = dist;
			}

			return await _webService.SaveTrainingShareToFacebookAsync(training, isShare);
		}

		public IMvxCommand SaveActivityCommand { get { return new MvxCommand(async () =>
		{
			if (IsBusy)
			{
				return;
			}
			IsBusy = true; 
			await saveActivityAsync();
		}); } }

		public IMvxCommand CancelCommand { get { return new MvxCommand(() => Close(this)); } }

		public IMvxCommand RecordActivityCommand
		{
			get { return new MvxCommand(() => { }); }// Empty command;
		}

		public IMvxCommand UploadImageCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					var actionSheetConfig = new ActionSheetConfig();

					if (PlatformService.Platform == Platform.iOS)
					{
						actionSheetConfig.SetTitle(this["Activity_UploadAnyImage"]);
					}

					actionSheetConfig.Add(this["Activity_ChooseFromGallery"], () =>
					{
						try
						{
							_photoService.GetPictureFromLibrary(Constants.MAX_PIXEL_DIMENTION, Constants.DEFAULT_JPEG_QUALITY);
						}
						catch (Exception ex)
						{
							_messageService.ShowAlertAsync("", ex.Message);
						}
					});
					actionSheetConfig.Add(this["Activity_TakePhoto"], () =>
					{
						try
						{
							_photoService.GetPictureFromCamera(Constants.MAX_PIXEL_DIMENTION, Constants.DEFAULT_JPEG_QUALITY);
						}
						catch (Exception ex)
						{
							_messageService.ShowAlertAsync("", ex.Message);
						}
					});

					if (PlatformService.Platform == Platform.iOS)
					{
						actionSheetConfig.Add(this["Activity_Cancel"], () =>
						{

						});
					}

					DialogService.ActionSheet(actionSheetConfig);
				});
			}
		}

		public void ImageRecieved(byte[] imageBytes)
		{
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "ImageRecieved start");
			_imageBytes = _imageResizeService.ResizeImage(imageBytes, 
				Constants.MAX_PICK_IMAGE_WIDTH, 0, Constants.DEFAULT_JPEG_QUALITY);
			ImageSelected = true;
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "ImageRecieved end");
		}

		public void ImageRecieved(Stream imageBytes)
		{
			_imageBytes = _imageResizeService.ResizeImage(_photoService.GetBytes(imageBytes), 
				Constants.MAX_PICK_IMAGE_WIDTH, 0, Constants.DEFAULT_JPEG_QUALITY);
			ImageSelected = true;
		}

		public override void Dispose()
		{
			//Mvx.Resolve<IMessageService>().ShowAlertAsync("", "Dispose");
			_timer.Dispose();
//			if (_imageReceiveToken != null)
//			{
//				//Messenger.Unsubscribe<ImageChooseMessage>(_imageReceiveToken);
//				_imageReceiveToken = null;
//			}
			base.Dispose();
		}
	}
}
