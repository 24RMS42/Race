using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using MotionsRace.Core.Helpers;
using MotionsRace.Core.Messages;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
    public class MainViewModel : HeaderScreenViewModel
    {
        private readonly IDataService _dataService;
        private readonly IMessageService _messageService;
        private readonly ISettingsService _settingsService;
        private readonly IWebService _webService;
        private readonly IInsights _insights;
        private static MvxSubscriptionToken _refreshActivityListToken;

        public MainViewModel(INavigationService navigationService,
            IWebService webService,
            IMessageService messageService,
            IDataService dataService,
            IPlatformService platformService,
            ISettingsService settingsService,
            IUserDialogs dialogService,
            IMvxMessenger messenger,
            IInsights insights
            )
            : base(navigationService, dialogService, platformService, messenger)
        {
            _webService = webService;
            _messageService = messageService;
            _dataService = dataService;
            _settingsService = settingsService;
            _insights = insights;
            InternetExists = true;

            _refreshActivityListToken = Messenger.Subscribe<RefreshNewsFeedListMessage>(sender => LoadDataAsync());

            FeedsListVisible = true;
            DetailFeedVisible = false;
        }

        private bool _isNewsFeedPersonal;
        public bool IsNewsFeedPersonal
        {
            get { return _isNewsFeedPersonal; }
            set
            {
                _isNewsFeedPersonal = value;
                GetMyFeeds().GetAwaiter();
                RaisePropertyChanged(() => IsNewsFeedPersonal);
            }
        }

        private NewsFeedItemModel _selectedFeedItem;
        public NewsFeedItemModel SelectedFeedItem
        {
            get { return _selectedFeedItem; }
            set
            {
                _selectedFeedItem = value;
                RaisePropertyChanged(() => SelectedFeedItem);
            }
        }

        private bool _showCloseMyActivitiesPanel;
        public bool ShowCloseMyActivitiesPanel
        {
            get { return _showCloseMyActivitiesPanel && !DetailFeedVisible; }
            set
            {
                _showCloseMyActivitiesPanel = value;
                RaisePropertyChanged(() => ShowCloseMyActivitiesPanel);
            }
        }

        private bool _internetExists;
        public bool InternetExists
        {
            get { return _internetExists; }
            set
            {
                _internetExists = value;
                RaisePropertyChanged(() => InternetExists);
            }
        }

        private string _endDateTitle;
        public string EndDateTitle
        {
            get { return _endDateTitle; }
            set
            {
                _endDateTitle = value;
                RaisePropertyChanged(() => EndDateTitle);
            }
        }

        private string _pictureURL;
        public string PictureURL
        {
            get { return _pictureURL; }
            set
            {
                _pictureURL = value;
                RaisePropertyChanged(() => PictureURL);
            }
        }

        private string _firstLastName;
        public string FirstLastName
        {
            get { return _firstLastName; }
            set
            {
                _firstLastName = value;
                RaisePropertyChanged(() => FirstLastName);
            }
        }

        private string _raceTitle;
        public string RaceTitle
        {
            get { return _raceTitle; }
            set
            {
                _raceTitle = value;
                RaisePropertyChanged(() => RaceTitle);
            }
        }

        private string _pointsPerPerson;
        public string PointsPerPerson
        {
            get { return _pointsPerPerson; }
            set
            {
                _pointsPerPerson = value;
                RaisePropertyChanged(() => PointsPerPerson);
            }
        }

        private string _endsData;
        public string EndsData
        {
            get { return _endsData; }
            set
            {
                _endsData = value;
                RaisePropertyChanged(() => EndsData);
            }
        }

        private int _progress;
        public int Progress
        {
            get { return _progress; }
            set
            {
                _progress = value;
                RaisePropertyChanged(() => Progress);
            }
        }

        private bool _showRegisterActivityButton;
        public bool ShowRegisterActivityButton
        {
            get { return _showRegisterActivityButton; }
            set
            {
                _showRegisterActivityButton = value;
                RaisePropertyChanged(() => ShowRegisterActivityButton);
            }
        }

        private bool _showReadMoreButton;
        public bool ShowReadMoreButton
        {
            get { return _showReadMoreButton; }
            set
            {
                _showReadMoreButton = value;
                RaisePropertyChanged(() => ShowReadMoreButton);
            }
        }

        private string _settingsUrl;
        public string SettingsUrl
        {
            get { return _settingsUrl; }
            set
            {
                _settingsUrl = value;
                RaisePropertyChanged(() => SettingsUrl);
            }
        }

        private List<NewsFeedItemModel> _news;
        public List<NewsFeedItemModel> News
        {
            get { return _news; }
            set
            {
                _news = value;
                RaisePropertyChanged(() => News);
            }
        }

        public string FrequentTrainingTypesString { get; private set; }
        private List<GetTrainingTypesResult> _frequentTrainingTypes;
        public List<GetTrainingTypesResult> FrequentTrainingTypes
        {
            get { return _frequentTrainingTypes; }
            set
            {
                _frequentTrainingTypes = value;
                FrequentTrainingTypesString = String.Join(",", _frequentTrainingTypes.Select(t => t.TrainingTypeID).AsEnumerable());
                RaisePropertyChanged(() => FrequentTrainingTypes);
            }
        }

        private bool _feedsListVisible;
        public bool FeedsListVisible
        {
            get { return _feedsListVisible; }
            set
            {
                _feedsListVisible = value;
                RaisePropertyChanged(() => FeedsListVisible);
            }
        }

        private bool _detailFeedVisible;
        public bool DetailFeedVisible
        {
            get { return _detailFeedVisible; }
            set
            {
                _detailFeedVisible = value;
                RaisePropertyChanged(() => DetailFeedVisible);
                RaisePropertyChanged(() => ShowCloseMyActivitiesPanel);
            }
        }

        private bool _isCrash;
        public bool IsCrash
        {
            get { return _isCrash; }
            set
            {
                _isCrash = value;
                RaisePropertyChanged(() => IsCrash);
            }
        }


        public IMvxCommand RecordActivityCommand
        {
            get
            {
                return new MvxCommand(async () =>
                {
                    if (PlatformService.IsInternetAvailable()) ShowViewModel<ActivityTypesViewModel>(new { liveRecord = false });
                    else
                    {
                        await DialogService.AlertAsync(this["GLOBAL_NoInternetConnection"], this["GLOBAL_Error"], this["GLOBAL_Ok"]);
                    }
                });
            }
        }

        public IMvxCommand ShowMenuCommand
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var actionSheetConfig = new ActionSheetConfig();

                    actionSheetConfig
                        .Add(this["Main_Settings"], () =>
                        {
                            var navigateUrl = UrlHelper.GetAutologinSettingsUrl();
                            ShowViewModel<WebViewModel>(new { url = navigateUrl });
                            //PlatformService.LauchUrl(UrlHelper.GetAutologinSettingsUrl());
                        }).Add(string.Format("{0} {1}", this["Main_LogOut"], FirstLastName), () => Restart())
                            .Add(this[IsNewsFeedPersonal ? "Main_NewsFeedAll" : "Main_NewsFeedPersonal"], () =>
                            {
                                IsNewsFeedPersonal = !IsNewsFeedPersonal;
                                ShowCloseMyActivitiesPanel = true;
                            });

                    if (PlatformService.Platform == Platform.iOS)
                    {
                        actionSheetConfig.SetTitle(Theme.Name);
                        actionSheetConfig.Cancel = new ActionSheetOption(this["Activity_Cancel"]);
                    }
                    else
                    {
                        actionSheetConfig.Add(this["Main_StayInChallenge"]);
                    }

                    DialogService.ActionSheet(actionSheetConfig);
                });
            }
        }

        public IMvxCommand RestartCommand
        {
            get { return new MvxCommand(() => Restart(false)); }
        }

        public IMvxCommand ShowReadMorePageCommand
        {
            get
            {
                return new MvxCommand<NewsFeedItemModel>(async newsFeedItem =>
                {
                    _messageService.HideAlert();

                    if (string.IsNullOrWhiteSpace(newsFeedItem.FullImageURL) &&
                        string.IsNullOrWhiteSpace(newsFeedItem.FullMessage) &&
                        string.IsNullOrWhiteSpace(newsFeedItem.ReadMoreURL))
                    {
                        _messageService.ShowAlertAsync(this["GLOBAL_Error"], this["Main_NoDetailInformation"]);
                        return;
                    }

                    SelectedFeedItem = newsFeedItem;
                    SelectedFeedItem.IsSelected = true;
                    // Delay to show selected item with selected background
                    await Task.Delay(250);
                    ShowReadMoreButton = !string.IsNullOrWhiteSpace(SelectedFeedItem.ReadMoreURL);
                    FeedsListVisible = false;
                    DetailFeedVisible = true;
                    SelectedFeedItem.IsSelected = false;
                });
            }
        }

        public IMvxCommand CloseDetailFeed
        {
            get
            {
                return new MvxCommand(() =>
                {
                    FeedsListVisible = true;
                    DetailFeedVisible = false;
                    SelectedFeedItem = null;
                });
            }
        }

        public IMvxCommand ShowDetailFullFeedImage
        {
            get
            {
                return new MvxCommand(() =>
                {
                    ShowViewModel<ImageViewerViewModel>(new { imageURL = SelectedFeedItem.FullImageURL });
                });
            }
        }

        public IMvxCommand LunchReadMoreUrl
        {
            get
            {
                return new MvxCommand(() =>
                {
                    var navigateUrl = UrlHelper.GetAutologinUrl(SelectedFeedItem.ReadMoreURL);
                    ShowViewModel<WebViewModel>(new { url = navigateUrl });
                    //Mvx.Resolve<IPlatformService>().LauchUrl(UrlHelper.GetAutologinUrl(SelectedFeedItem.ReadMoreURL));
                });
            }
        }


        public IMvxCommand CloseMyActivitiesPanelCommand
        {
            get
            {
                return new MvxCommand(() => { ShowCloseMyActivitiesPanel = !ShowCloseMyActivitiesPanel; });
            }
        }

        protected override bool OnTryClose()
        {
            if (DetailFeedVisible)
            {
                SetFeedListVisible();
                return false;
            }
            return true;
        }

        public void Init(bool disableMode = false)
        {
            if (disableMode)
            {
                DisableControls();
                return;
            }
            ShowCloseMyActivitiesPanel = true;
            LoadDataAsync();
        }

        public void CheckLiveRegister()
        {
            if (_settingsService.Options.LiveRecord != null)
            {
                ShowViewModel<ActivityItemViewModel>(
                    new { liveRecord = true, trainingId = _settingsService.Options.LiveRecord.TrainingId });
            }
        }

        public async void LoadDataAsync()
        {
            if (PlatformService == null || !PlatformService.IsInternetAvailable())
            {
                InternetExists = false;
                IsCrash = true;
                ShowErrorMessage(this["GLOBAL_NoInternetConnection"]);
                DisableControls();
                return;
            }
            SetFeedListVisible();
            InternetExists = true;
            ShowBusy();

            IsCrash = false;

            // API GetParticipantOverview
            await GetParticipantOverviewAsync();
            // API GetTrainingTypes
            if (!IsCrash) await GetTrainingTypesAsync();
            // API GetMyNewsFeed
            if (!IsCrash) await GetMyFeeds();
            //  API GetTrainingTypesFrequent
            if (!IsCrash) await GetFrequentTrainingTypesAsync();

            RunOnMainThread(HideBusy);
        }

        private void SetFeedListVisible()
        {
            FeedsListVisible = true;
            DetailFeedVisible = false;
        }

        private void Restart(bool isWelcomeView = true)
        {
            _settingsService.Options.Reset();
            _settingsService.Save();
            _messageService.HideAlert();
            NavigationService.ClearNavStack();
            if (isWelcomeView)
            {
                ShowViewModel<WelcomeViewModel>();
            }
            else
            {
                ShowViewModel<SignInViewModel>();
            }
            Close(this);
        }

        #region LoadData methods

        public void ShowErrorMessage(string message)
        {
            RunOnMainThread(HideBusy);
            _messageService.HideAlert();
            _messageService.ShowAlertAsync(this["GLOBAL_Error"], message);
            DisableControls();
            RaisePropertyChanged(() => IsCrash);
        }

        private async Task GetTrainingTypesAsync()
        {
            var trainingsResponse = await _webService.GetTrainingTypesAsync();
            if (!trainingsResponse.IsSuccess)
            {
                ShowErrorMessage(trainingsResponse.ApiErrorMessage);
                DisableControls();
                IsCrash = true;
                return;
            }

            IsCrash = false;
            _dataService.SaveTrainingTypes(trainingsResponse.Response);
        }

        private async Task GetFrequentTrainingTypesAsync()
        {
            var trainingsResponse = await _webService.GetFrequentTrainingTypesAsync();
            if (!trainingsResponse.IsSuccess)
            {
                ShowErrorMessage(trainingsResponse.ApiErrorMessage);
            }

            FrequentTrainingTypes = new List<GetTrainingTypesResult>();
            if (trainingsResponse.Response != null && trainingsResponse.Response.Result != null)
            {
                FrequentTrainingTypes = trainingsResponse.Response.Result;
            }

            ShowFavoritButton = FrequentTrainingTypes.Any();
        }

        private async Task GetParticipantOverviewAsync()
        {
            var infoResponse = await _webService.GetParticipantOverviewAsync();
            if (!infoResponse.IsSuccess)
            {
                IsCrash = true;
                ShowErrorMessage(infoResponse.ApiErrorMessage);
                DisableControls();
                return;
            }
            var info = infoResponse.Response.Result;

            // Fill and save settings parameters
            if (!FillSettingsServiceParams(info))
            {
                IsCrash = true;
                DisableControls();
                _insights.Report(new Exception("Can't get user settings"),
                    new Dictionary<string, GetParticipantOverviewResult>
                    {
                        { "User", info }
                    }, Severity.Error);
                return;
            }

            _settingsService.Options.LastRunDateTimeGetParticipantOverview = DateTime.UtcNow;

            // Show  dependent buttons
            RaisePropertyChanged(() => ShowLiveRecordButton);
            RaisePropertyChanged(() => ShowLoginButton);
            ShowRegisterActivityButton = info.RaceDetails.IsRegisterTrainingPossible;
            // Fill header UI elements
            FillHeaderUIControls(info);
            IsCrash = false;
        }

        private bool FillSettingsServiceParams(GetParticipantOverviewResult infoResponseResult)
        {
            if (infoResponseResult == null)
            {
                ShowErrorMessage("Can't get settings parameters");
                return false;
            }

            _settingsService.Options.MinvalidRegistrationDate = infoResponseResult.MinvalidRegistrationDate;
            _settingsService.Options.MaxvalidRegistrationDate = infoResponseResult.MaxvalidRegistrationDate;

            if (infoResponseResult.ServerDate != null)
            {
                _settingsService.Options.ServerDate = DateTime.Parse(infoResponseResult.ServerDate.TodayDate);
            }

            if (infoResponseResult.RaceDetails != null)
            {
                _settingsService.Options.RequiredMinutes = infoResponseResult.RaceDetails.RequiredMinutes;
                _settingsService.Options.AllowLiveRecord = infoResponseResult.RaceDetails.IsRegisterTrainingPossible;
                _settingsService.Options.AllowLogin = infoResponseResult.RaceDetails.IsLoginPossible;
                _settingsService.Options.HostName = infoResponseResult.RaceDetails.HostName;
                _settingsService.Options.MaxNumberOfDaysUserCanWaitToRegister =
                    infoResponseResult.RaceDetails.MaxNumberOfDaysUserCanWaitToRegister;
                _settingsService.Options.IsIntensityActivated = infoResponseResult.RaceDetails.IsIntensityActivated;
                _settingsService.Options.IsDistanceActivated = infoResponseResult.RaceDetails.IsDistanceActivated;
                _settingsService.Options.MaxMinutesTotalPerDay = infoResponseResult.RaceDetails.MaxMinutesTotalPerDay;
                _settingsService.Options.MaxPointsPerWeek = infoResponseResult.RaceDetails.MaxPointsPerWeek;
            }
            else
            {
                ShowErrorMessage(" Can't get settings parameters of RaceDetails");
                return false;
            }

            if (infoResponseResult.PersonInfo != null)
            {
                _settingsService.Options.ShowShareToFacebook = infoResponseResult.PersonInfo.FacebookAccessToken != null;
                _settingsService.Options.IsShareToFacebook = infoResponseResult.PersonInfo.IsShareToFacebook.HasValue && infoResponseResult.PersonInfo.IsShareToFacebook.Value;
            }
            else
            {
                ShowErrorMessage(" Can't get settings parameters of PersonInfo");
                return false;
            }

            _settingsService.Save();
            return true;
        }

        private int CalcProgressPosition(GetParticipantOverviewResult infoResponseResult)
        {
            int progress;
            if (_settingsService.Options.ServerDate > infoResponseResult.RaceDetails.EndDate)
            {
                progress = 100;
            }
            else
            {
                var totalDaysCount = (float)(infoResponseResult.RaceDetails.EndDate -
                    infoResponseResult.RaceDetails.StartDate).TotalDays;
                var passedDaysCount = (float)(_settingsService.Options.ServerDate -
                    infoResponseResult.RaceDetails.StartDate).TotalDays;
                if (totalDaysCount > 0)
                {
                    progress = (int)(passedDaysCount / totalDaysCount * 100);
                }
                else
                {
                    progress = 100;
                }
            }
            return progress;
        }

        private void FillHeaderUIControls(GetParticipantOverviewResult infoResponseResult)
        {
            // Calculate progress position
            Progress = CalcProgressPosition(infoResponseResult);

            FirstLastName = string.Format("{0} {1}", infoResponseResult.PersonInfo.FirstName, infoResponseResult.PersonInfo.LastName);
            RaceTitle = infoResponseResult.RaceDetails.RaceTitle;
            PointsPerPerson = string.Format("{0} {1}", infoResponseResult.TotalNumberOfPoints, this["Main_Points"]);
            EndsData = string.Format("{0}: {1:yyyy-MM-dd}",
                _settingsService.Options.ServerDate < infoResponseResult.RaceDetails.EndDate ? this["Main_Ends"] : this["Main_LastDayWas"],
                infoResponseResult.RaceDetails.EndDate);
            _settingsService.Options.IsSecureProtocol = !string.IsNullOrWhiteSpace(infoResponseResult.RaceDetails.LoginURL) &&
                infoResponseResult.RaceDetails.LoginURL.IndexOf("https", StringComparison.OrdinalIgnoreCase) >= 0;
            PictureURL = UrlHelper.GetProfileImageUrl(infoResponseResult.PersonInfo.ImageGUID);
        }

        public async Task GetMyFeeds()
        {
            var getMyNewsFeedResponse = await _webService.GetMyNewsFeedAsync(_isNewsFeedPersonal);
            if (!getMyNewsFeedResponse.IsSuccess)
            {
                RunOnMainThread(HideBusy);
                IsCrash = true;
                ShowErrorMessage(getMyNewsFeedResponse.ApiErrorMessage);
                return;
            }

            IOrderedEnumerable<GetMyNewsFeedResult> sortedNews =
                getMyNewsFeedResponse.Response.Result.OrderBy(x => x.RelevanceRank);

            News =
                sortedNews.Select((item, index) => new NewsFeedItemModel(this, item, index,
                    _settingsService.Options.ServerDate)).ToList();

            IsCrash = false;
        }

        private void DisableControls()
        {
            ShowLiveRecordButton = false;
            ShowLoginButton = false;
            ShowRegisterActivityButton = false;

            _settingsService.Options.AllowLiveRecord = false;
            _settingsService.Options.AllowLogin = false;
            _settingsService.Options.HostName = "";
            _settingsService.Options.IsIntensityActivated = false;
            _settingsService.Options.IsDistanceActivated = false;
            RaisePropertyChanged(() => ShowLiveRecordButton);
            RaisePropertyChanged(() => ShowLoginButton);
            RaisePropertyChanged(() => News);
        }

        #endregion

        public override void Dispose()
        {
            Messenger.Unsubscribe<RefreshNewsFeedListMessage>(_refreshActivityListToken);
            _refreshActivityListToken.Dispose();
            base.Dispose();
        }
    }
}