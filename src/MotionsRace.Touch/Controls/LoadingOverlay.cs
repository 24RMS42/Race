using CoreGraphics;
using Foundation;
using ObjCRuntime;
using UIKit;
using MotionsRace.Core.Services;
using MotionsRace.Core.ViewModels;
using System;
using MvvmCross.Platform;
using MvvmCross.Plugins.Color.iOS;

namespace MotionsRace.Touch.Views
{
	public class LoadingOverlay : UIView {
		// control declarations
		UIActivityIndicatorView activitySpinner;
		internal UILabel LoadingLabel;

		public LoadingOverlay (CGRect frame) : base (frame)
		{
			var theme = Mvx.Resolve<IThemesManager> ();

			// configurable bits
			BackgroundColor = UIColor.Black;
			//Alpha = 0.75f;
			Alpha = 0.75f;
			AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;

			float labelHeight = 22;
			nfloat labelWidth = Frame.Width - 20;

			// derive the center x and y
			var centerX = Frame.Width / 2;
			var centerY = Frame.Height / 2;

			// create the activity spinner, center it horizontall and put it 5 points above center x
			activitySpinner = new UIActivityIndicatorView(UIActivityIndicatorViewStyle.WhiteLarge);
			activitySpinner.Frame = new CGRect (
				centerX - (activitySpinner.Frame.Width / 2) ,
				centerY - activitySpinner.Frame.Height - 20 ,
				activitySpinner.Frame.Width ,
				activitySpinner.Frame.Height);
			activitySpinner.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (activitySpinner);
			activitySpinner.StartAnimating ();

			// create and configure the "Loading Data" label
			LoadingLabel = new UILabel(new CGRect (
				centerX - (labelWidth / 2),
				centerY + 20 ,
				labelWidth ,
				labelHeight
			));
			LoadingLabel.BackgroundColor = UIColor.Clear;
			LoadingLabel.TextColor = theme.CurrentTheme.Colors.LoadingIndicatorColor.ToNativeColor ();
			//LoadingLabel.Text = "Loading...";
			LoadingLabel.TextAlignment = UITextAlignment.Center;
			LoadingLabel.AutoresizingMask = UIViewAutoresizing.FlexibleMargins;
			AddSubview (LoadingLabel);
		}

		/// <summary>
		/// Fades out the control and then removes it from the super view
		/// </summary>
		public void Show ()
		{
			UIView.Animate (
				0.5, // duration
				() => { Alpha = 0.75f; },
				() => { ; }
			);
		}

		public void Hide ()
		{
			UIView.Animate (
				0, // duration
				() => { Alpha = 0; },
				() => { ; }
			);
		}

		public bool IsVisible
		{
			get
			{ return false;}
			set 
			{ 
				if (value == false)
					Hide ();
				else
					Show ();
			}

		}
	};
}