using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Windows.Phone.UI.Input;
using Cirrious.CrossCore;
using MotionsRace.Core.Models;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using Telerik.Windows.Controls;
using GestureEventArgs = System.Windows.Input.GestureEventArgs;


namespace MotionsRace.WinPhone.Views
{
	public partial class MainView 
	{
		public static BitmapImage RotatetImage;

		public new MainViewModel ViewModel
		{
			get { return (MainViewModel)base.ViewModel; }
			set { base.ViewModel = value; }
		}

		public MainView()
		{
			// Add localization to Telerik controls 
			LocalizationManager.GlobalResourceManager = Mvx.Resolve<ILanguageService>().GetResourceManager();
			InitializeComponent();
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			HardwareButtons.BackPressed += HardwareButtons_BackPressed;
		}

		protected override void OnNavigatedFrom(NavigationEventArgs e)
		{
			base.OnNavigatedFrom(e);
			HardwareButtons.BackPressed -= HardwareButtons_BackPressed;
		}


		private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
		{
			e.Handled = true;
			ViewModel.ExitCommand.Execute();
		}

		private void ShowMenuCommand(object sender, GestureEventArgs e)
		{
			ViewModel.ShowMenuCommand.Execute();
		}

		protected override void OnBackKeyPress(CancelEventArgs e)
		{
			base.OnBackKeyPress(e);
			e.Cancel = true;
			ViewModel.ExitCommand.Execute();
		}

		/// <summary>
		/// Show detail view page
		/// </summary>
		private void RadDataBoundListBox_OnItemTap(object sender, ListBoxItemTapEventArgs e)
		{
			// Clear previews settings
			FullImage.Source = null;
			DetailFeedScrollViewer.ScrollToVerticalOffset(0);
			FullImage.Visibility = Visibility.Collapsed;
			var newsFeed = (e.Item.DataContext as NewsFeedItemModel);
			if (newsFeed == null )
			{
				return;
			}

			// Set image stratch in detail prewive
			if (!string.IsNullOrWhiteSpace(newsFeed.FullImageURL))
			{
				Uri myUri = new Uri(newsFeed.FullImageURL, UriKind.Absolute);
				var bitmap = new BitmapImage(myUri);
                bitmap.DownloadProgress += (obj, args) =>
				{
					if (args.Progress == 100)
					{
                        var width = ((BitmapImage) obj).PixelWidth;
						FullImage.Stretch = width > Application.Current.Host.Content.ActualWidth
							? Stretch.Uniform
							: Stretch.None;
						FullImage.Visibility = Visibility.Visible;
					}
				};
				FullImage.Source = RotatetImage = bitmap;
			}
			ViewModel.ShowReadMorePageCommand.Execute(e.Item.DataContext);
		}

		/// <summary>
		/// Refresh feed list on pull to refresh
		/// </summary>
		private async void OnRefreshTemplateRequested(object sender, EventArgs e)
		{
			await ViewModel.GetMyFeeds();
			FeedList.StopPullToRefreshLoading(true, true);
		}

		private void FullImage_Tap(object sender, GestureEventArgs e)
		{
			ViewModel.ShowDetailFullFeedImage.Execute(new { imageUrl = ViewModel.SelectedFeedItem.FullImageURL });
		}

		private void CloseMyActivitiesPanelCommand(object sender, GestureEventArgs e)
		{
			ViewModel.CloseMyActivitiesPanelCommand.Execute();
		}
	}
}