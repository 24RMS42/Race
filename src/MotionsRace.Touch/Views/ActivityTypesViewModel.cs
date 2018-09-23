using Cirrious.MvvmCross.Binding.BindingContext;
using Cirrious.MvvmCross.Touch.Views;
using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using Cirrious.CrossCore;
using MotionsRace.Core.ViewModels;
using Cirrious.MvvmCross.Plugins.Color.Touch;


namespace MotionsRace.Touch.Views
{
	[Register("ActivityTypesView")]
	public class ActivityTypesView : MvxViewController<ActivityTypesViewModel>
    {
        public override void ViewDidLoad()
        {
			var backgroundColor = ViewModel.Colors ["ACTIVITY_TYPES_PANELS_BACKGROUND"].ToNativeColor ();
			View = new UIView { BackgroundColor = backgroundColor };
            base.ViewDidLoad();

			this.Title = ViewModel["Activity_RegisterActivity"];

			/*
			UIImageView titleView = new UIImageView (new RectangleF (0, 0, 150, 30));
			titleView.Image = new UIImage ("Title.png");

			NavigationItem.TitleView = titleView;
			*/

			// ios7 layout
            if (RespondsToSelector(new Selector("edgesForExtendedLayout")))
            {
               EdgesForExtendedLayout = UIRectEdge.None;
            }

			// header
			this.NavigationItem.SetLeftBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Add, (sender,args) => {
					ViewModel.LiveRecordCommand.Execute();
				})
				, true);

			this.NavigationItem.SetRightBarButtonItem(
				new UIBarButtonItem(UIBarButtonSystemItem.Action, (sender,args) => {
					ViewModel.LoginCommand.Execute();
				})
				, true);
			   
            var label = new UILabel(new CGRect(10, 10, 300, 40));
            Add(label);
            var textField = new UITextField(new CGRect(10, 50, 300, 40));
            Add(textField);

			var btnSignUp = new UIButton(UIButtonType.RoundedRect);
			btnSignUp.Frame = new CGRect(40, 130, UIScreen.MainScreen.Bounds.Width - 80, 30);
			btnSignUp.Layer.CornerRadius = 10;
			btnSignUp.Layer.MasksToBounds = true;
			btnSignUp.SetTitle (ViewModel["Login_SignIn"], UIControlState.Normal); 
			btnSignUp.SetTitleColor(ViewModel.Colors ["LOGIN_BUTTON_FOREGROUND_COLOR"].ToNativeColor (), UIControlState.Normal);
			btnSignUp.BackgroundColor = ViewModel.Colors ["LOGIN_BUTTON_BACKGROUND_COLOR"].ToNativeColor ();
			View.AddSubview(btnSignUp);

			var set = this.CreateBindingSet<ActivityTypesView, Core.ViewModels.ActivityTypesViewModel>();
            //set.Bind(label).To(vm => vm.Hello);
            //set.Bind(textField).To(vm => vm.Hello);
			set.Bind(btnSignUp).To(vm => vm.GoBackCommand);
            set.Apply();
        }

		public override void ViewWillAppear (bool animated)
		{
			base.ViewWillAppear (animated);
			this.NavigationController.SetNavigationBarHidden(false, true);
			this.NavigationController.NavigationBar.BarStyle = UIBarStyle.Black;

			this.NavigationController.NavigationBar.TintColor = UIColor.White;
			this.NavigationController.NavigationBar.BarTintColor = ViewModel.Colors ["GLOBAL_HEADER_COLOR"].ToNativeColor ();
			//this.NavigationController.NavigationBar.BackItem.Title = "";
		}
    }
}