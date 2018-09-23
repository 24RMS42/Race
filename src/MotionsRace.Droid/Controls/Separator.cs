using System;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Graphics;

namespace MotionsRace.Droid
{
	public class Separator: View
	{
		public Separator (Context context) : base(context)
		{
		}

		public Separator (Context context, IAttributeSet attrs): base(context, attrs)
		{		
		}
		public Separator (Context context, IAttributeSet attrs, int defStyle): base(context, attrs, defStyle )
		{
		}
	
		public override void Draw (Canvas canvas)
		{
			var centerY = canvas.ClipBounds.ExactCenterY ();
			var centerX = canvas.ClipBounds.ExactCenterX ();
			float radius = centerY > centerX ? centerX : centerY;			

			Paint paint = new Paint();
			paint.SetStyle(Paint.Style.Fill);
			paint.StrokeWidth = 10;
			paint.Color = Color.DimGray; 
			paint.AntiAlias = true;
			//paint.Alpha = 150; // max 255
			canvas.DrawLine(10, 0 , canvas.ClipBounds.Right-10, 0, paint); 
		}
	}
}

