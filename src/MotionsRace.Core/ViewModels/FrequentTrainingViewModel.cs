using System.Collections.ObjectModel;
using System.Linq;
using Acr.UserDialogs;
using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
	public class FrequentTrainingViewModel : HeaderScreenViewModel
	{
		private readonly IDataService _dataService;

		private ObservableCollection<GetTrainingTypesResult> _trainingCategoryItems;
		public ObservableCollection<GetTrainingTypesResult> TrainingCategoryItems
		{
			get { return _trainingCategoryItems; }
			set
			{
				_trainingCategoryItems = value;
				RaisePropertyChanged(() => TrainingCategoryItems);
			}
		}

		private GetTrainingTypesResult _trainingCategoryItemsSelected;
		public GetTrainingTypesResult TrainingCategoryItemsSelected
		{
			get { return _trainingCategoryItemsSelected; }
			set
			{
				_trainingCategoryItemsSelected = value;
				RaisePropertyChanged(() => TrainingCategoryItemsSelected);
			}
		}

		public FrequentTrainingViewModel(
			INavigationService navigationService,
			IDataService dataService,
			IPlatformService platformService,
			IUserDialogs dialogService,
			IMvxMessenger messenger
			)
			: base(navigationService, dialogService, platformService, messenger)
		{
			_dataService = dataService;
		}

		public void Init(string frequentTrainingTypesString)
		{
			IsBusy = true;

			var frequentTrainingTypes = frequentTrainingTypesString.Split(',');
			var trainingTypes = _dataService.GetTrainingTypes();
			TrainingCategoryItems = new ObservableCollection<GetTrainingTypesResult>(
				trainingTypes.Where(t => frequentTrainingTypes.Contains(t.TrainingTypeID.ToString())));

			IsBusy = false;
		}

		public IMvxCommand TrainingCategoryItemsSelectedCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					TrainingCategoryItemsSelected.IsSelected = true;
					ShowViewModel<ActivityItemViewModel>(
						new { liveRecord = false, trainingId = TrainingCategoryItemsSelected.TrainingTypeID });
					TrainingCategoryItemsSelected.IsSelected = false;
				});
			}
		}
	}
}
