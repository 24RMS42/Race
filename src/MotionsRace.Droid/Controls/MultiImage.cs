using System;
using Android.Widget;
using Android.Content;
using Android.Runtime;
using Android.Util;
using Android.Views;
using System.Collections.Generic;
using Android.Graphics;

namespace MotionsRace.Droid
{
	public class MultiImage : ImageView, GestureDetector.IOnGestureListener, IJavaObject, IDisposable
	{
		GestureDetector _gestureDetector;
		List<Bitmap> _bitmaps;

		public MultiImage (Context context) : base (context)
		{
			Constructor ();
		}

		protected MultiImage (IntPtr javaReference, JniHandleOwnership transfer) : base (javaReference, transfer)
		{
			Constructor ();
		}

		public MultiImage (Context context, IAttributeSet attrs) : base (context, attrs)
		{
			Constructor ();
		}

		public MultiImage (Context context, IAttributeSet attrs, int defStyle): base(context, attrs, defStyle)
		{
			Constructor ();
		}

		private void Constructor()
		{
			//this.imageList = new List<string>();
			_gestureDetector = new GestureDetector(this);
		}


		public override bool OnTouchEvent (MotionEvent e)
		{
			return _gestureDetector.OnTouchEvent(e);
		}


		protected override void OnDraw (Android.Graphics.Canvas canvas)
		{
			base.OnDraw (canvas);
		}

		public void ImageList(List<Bitmap> bitmaps)
		{
			_bitmaps = bitmaps;
		}

		public bool OnDown (MotionEvent e)
		{
			return true;
			//throw new NotImplementedException ();
		}

		public bool OnFling (MotionEvent e1, MotionEvent e2, float velocityX, float velocityY)
		{
			return true;
			//throw new NotImplementedException ();
		}

		public void OnLongPress (MotionEvent e)
		{
			//throw new NotImplementedException ();
		}

		public bool OnScroll (MotionEvent e1, MotionEvent e2, float distanceX, float distanceY)
		{
			return true;
		//	throw new NotImplementedException ();
		}

		public void OnShowPress (MotionEvent e)
		{
		//	throw new NotImplementedException ();
		}

		public bool OnSingleTapUp (MotionEvent e)
		{
			return true;
		//	throw new NotImplementedException ();
		}
	}
}

