using Android.App;
using Android.Content.PM;
using MotionsRace.Core.ViewModels;
using MvvmCross.Droid.Views;

namespace MotionsRace.Droid.Views
{
	/// <summary>
	/// Base android activity 
	/// </summary>
	[Activity(
		ScreenOrientation = ScreenOrientation.Portrait)]
	public abstract class BaseActivity<TViewModel> : MvxActivity where TViewModel : BaseScreenViewModel
	{
		public new TViewModel ViewModel
		{
			get { return (TViewModel) base.ViewModel; }
			set { base.ViewModel = value; }
		}
	}
}