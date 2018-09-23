using Cirrious.CrossCore;
using Cirrious.MvvmCross.WindowsCommon.Views;
using MotionsRace.Core;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace MotionsRace.WindowsPhone.Views
{
    public sealed partial class ActivityTypesView : MvxWindowsPage
    {
        public ActivityTypesView()
        {
            this.InitializeComponent();            
        }

        private void GridView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var vm = DataContext as ActivityTypesViewModel;
            vm.ItemSelectedCommand.Execute(e.AddedItems[0]);
        }

        private void ListView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            var vm = DataContext as ActivityTypesViewModel;
            vm.TrainingCategoryItemsSelected = e.AddedItems[0] as GetTrainingTypesResult;
            vm.TrainingCategoryItemsSelectedCommand.Execute(e.AddedItems[0]);            
        }
    }
}
