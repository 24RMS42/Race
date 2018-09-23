using System;
using Android.Widget;
using Android.Content;
using Android.Util;

namespace MotionsRace.Droid.Controls
{
	public class CustomGallery : Gallery
	{
		public CustomGallery (Context context) : base(context)
		{
		}

		public CustomGallery (Context context, IAttributeSet attrs) : base (context , attrs)
		{
		}


		public CustomGallery (Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
		}

		public override bool OnFling (Android.Views.MotionEvent e1, Android.Views.MotionEvent e2, float velocityX, float velocityY)
		{
			//return base.OnFling (e1, e2, velocityX, velocityY);
			return false;
//
//			if (velocityX > 1200.0f)
//			{
//				velocityX = 1200.0f;
//			}
//			else if(velocityX < -1200.0f)
//			{
//			velocityX = -1200.0f;
//			}
//
		//return base.OnFling(e1, e2, velocityX, velocityY);
		}
	}
}

