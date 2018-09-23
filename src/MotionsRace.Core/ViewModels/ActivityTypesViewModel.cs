using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using MotionsRace.Core.Services;
using MvvmCross.Core.ViewModels;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.ViewModels
{
	public class ActivityTypesViewModel : HeaderScreenViewModel
	{
		private readonly IDataService _dataService;
		private ObservableCollection<GetTrainingTypesResult> _allTrainingCategoryItems;
		
		public ActivityTypesViewModel(
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

		public bool CanOnBackShowCategories
		{
			get
			{
				if ((TrainingCategories == null || TrainingCategories.Count > 1) && AnyCategorySelected)
				{
					return true;
				}
				return false;
			}
		}

		private ObservableCollection<TrainingCategory> _trainingCategories;
		public ObservableCollection<TrainingCategory> TrainingCategories
		{
			get { return _trainingCategories; }
			set
			{
				_trainingCategories = value;
				RaisePropertyChanged(() => TrainingCategories);
			}
		}

		public bool TrainingCategoriesVisibility
		{
			get { return (_trainingCategories != null && _trainingCategories.Count > 1); }
		}

		private TrainingCategory _trainingCategorySelected;
		public TrainingCategory TrainingCategorySelected
		{
			get { return _trainingCategorySelected; }
			set
			{
				_trainingCategorySelected = value;
				RaisePropertyChanged(() => TrainingCategorySelected);
			}
		}

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

		private bool _anyCategorySelected;
		public bool AnyCategorySelected
		{
			get { return _anyCategorySelected; }
			set
			{
				_anyCategorySelected = value;
				RaisePropertyChanged(() => AnyCategorySelected);
			}
		}


		private MvxCommand<TrainingCategory> _itemSelectedCommand;
		public ICommand ItemSelectedCommand
		{
			get
			{
				_itemSelectedCommand = _itemSelectedCommand ?? new MvxCommand<TrainingCategory>(doSelectItem);
				return _itemSelectedCommand;
			}
		}

		public IMvxCommand ItemSelectedTrainingCommand
		{
			get
			{
				return new MvxCommand<TrainingCategory>(category =>
				{
					if (TrainingCategorySelected != null)
						TrainingCategorySelected.IsSelected = false;
					TrainingCategorySelected =
						TrainingCategories.FirstOrDefault(x => x.ActivityCategoryID == category.ActivityCategoryID);
					TrainingCategorySelected.IsSelected = true;
					Task.Run(() =>
					{
						TrainingCategoryItems =
							_allTrainingCategoryItems.Where(x => x.ActivityCategoryID == TrainingCategorySelected.ActivityCategoryID)
								.ToObservableCollection();
						ClearSelectedItems();
					});
				});
			}
		}

		public IMvxCommand RecordActivityCommand
		{
			get { return new MvxCommand(() => { }); } // Empty command;
		}

		public IMvxCommand TrainingCategoryItemsSelectedCommand
		{
			get
			{
				return new MvxCommand(() =>
				{
					ClearSelectedItems();
					TrainingCategoryItemsSelected.IsSelected = true;
					ShowViewModel<ActivityItemViewModel>(
						new {liveRecord = false, trainingId = TrainingCategoryItemsSelected.TrainingTypeID});
				});
			}
		}

		public void Init()
		{
			IsBusy = true;

			if (!PlatformService.IsInternetAvailable())
			{
				DialogService.AlertAsync(this["GLOBAL_NoInternetConnection"], this["GLOBAL_Error"], this["GLOBAL_Ok"]);
				GoBackCommand.Execute();
				IsBusy = false;
				return;
			}

			TrainingCategories = _dataService.GetTrainingCategories().ToObservableCollection();
			_allTrainingCategoryItems = _dataService.GetTrainingTypes().ToObservableCollection();
			TrainingCategoryItems = new ObservableCollection<GetTrainingTypesResult>();

			if (TrainingCategories != null && TrainingCategories.Count == 1)
			{
				TrainingCategorySelected = TrainingCategories[0];
				TrainingCategorySelected.IsSelected = true;
				TrainingCategoryItems =
					_allTrainingCategoryItems.Where(x => x.ActivityCategoryID == TrainingCategorySelected.ActivityCategoryID)
						.ToObservableCollection();
				AnyCategorySelected = true;
			}

			IsBusy = false;
		}

		public void OnBackShowCategories()
		{
			if (CanOnBackShowCategories)
			{
				TrainingCategorySelected.IsSelected = false;
				AnyCategorySelected = false;
			}
		}

		private void doSelectItem(TrainingCategory item)
		{
			if (TrainingCategorySelected != null)
			{
				TrainingCategorySelected.IsSelected = false;
			}
			TrainingCategorySelected = item;
			TrainingCategorySelected.IsSelected = true;
			Task.Run(() =>
			{
				TrainingCategoryItems =
					_allTrainingCategoryItems.Where(
						x => x.ActivityCategoryID == TrainingCategorySelected.ActivityCategoryID).ToObservableCollection();
				AnyCategorySelected = true;
			});
		}

		public void ClearSelectedItems()
		{
			if (_allTrainingCategoryItems != null)
				foreach (GetTrainingTypesResult item in _allTrainingCategoryItems)
					item.IsSelected = false;
		}

		public void CloseViewModel()
		{
			Close(this);
		}
	}
}