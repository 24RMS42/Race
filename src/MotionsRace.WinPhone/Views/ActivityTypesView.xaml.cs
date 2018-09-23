using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using MotionsRace.Core.ViewModels;
using MotionsRace.Core;

namespace MotionsRace.WinPhone.Views
{
	public partial class ActivityTypesView
	{
		private GetTrainingTypesResult _selectedItem
		{
			get
			{
				if (TrainingCategoryList.SelectedItems.Count > 0)
				{
					return (TrainingCategoryList.SelectedItems[0] as GetTrainingTypesResult);
				}
				return null;
			}
		}

		public new ActivityTypesViewModel ViewModel
		{
			get { return (ActivityTypesViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public ActivityTypesView()
		{
			InitializeComponent();
		}

		private void GridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			ViewModel.ItemSelectedCommand.Execute(e.AddedItems[0]);
		}

		private void LiveRecordCommand(object sender, GestureEventArgs e)
		{
			if (_selectedItem != null)
			{
				_selectedItem.TrainingCategoryItemsSelectedCommand.Execute();
				ViewModel.TrainingCategoryItemsSelected = _selectedItem;
			}
		}

		private void ActivityRegisterCommand(object sender, GestureEventArgs e)
		{
			if (_selectedItem != null)
			{
				ViewModel.TrainingCategoryItemsSelected = _selectedItem;
				ViewModel.TrainingCategoryItemsSelectedCommand.Execute(_selectedItem);
			}
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			if (_selectedItem != null)
			{
				ViewModel.TrainingCategoryItemsSelected = null;
				_selectedItem.IsSelected = false;
			}
		}
	}
}