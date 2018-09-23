using System;
using Foundation;
using UIKit;

namespace MotionsRace.Touch
{
	[Register("CustomButton")]
	public sealed class CustomButton : UIButton
	{
		public const float DefaultHeight = 30f;

		private static readonly UIImage BackgroundDisabledImage =
			ImageUtils.ImageFromColor(UIColor.Gray);

		private static readonly UIImage BackgroundHighlightedImage =
			ImageUtils.ImageFromColor(UIColor.Orange);

		private static readonly UIImage BackgroundNormalImage = 
			ImageUtils.ImageFromColor(UIColor.Green);

		private static readonly UIImage BackgroundPressedImage =
			ImageUtils.ImageFromColor(UIColor.Red);

		public CustomButton(string title)
			: this()
		{
			this.SetTitle(title, UIControlState.Normal);
			this.SetTitleColor(UIColor.Black, UIControlState.Normal);
			this.SetTitleShadowColor(UIColor.White, UIControlState.Normal);

			this.SetBackgroundImage(BackgroundDisabledImage, UIControlState.Disabled);
			this.SetBackgroundImage(BackgroundHighlightedImage, UIControlState.Highlighted);
			this.SetBackgroundImage(BackgroundNormalImage, UIControlState.Normal);
			this.SetBackgroundImage(BackgroundPressedImage, UIControlState.Selected);

			this.Layer.BorderColor = UIColor.Yellow.CGColor;
			this.Layer.BorderWidth = 1.0f;
			this.Layer.CornerRadius = 5;
			this.Layer.MasksToBounds = true;
		}

		private CustomButton()
			: base(UIButtonType.Custom)
		{
		}
	}
}

