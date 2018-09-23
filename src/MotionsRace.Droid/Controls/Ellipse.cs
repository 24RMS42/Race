using System;
using Android.Views;
using Android.Content;
using Android.Util;

using Android.Graphics;

namespace MotionsRace.Droid.Controls
{
	public class Ellipse : View
    {
	
        public Ellipse(Context context) : base(context)
		{
			//var color = (ColorDrawable)this.Background;
			//this.Color = color.Color;
		}   

		public Ellipse (Context context, IAttributeSet attrs): base(context, attrs)
		{		
		}
		public Ellipse (Context context, IAttributeSet attrs, int defStyle): base(context, attrs, defStyle )
		{
		}	

		private Color _color;
		public Color Color{
			get
				{				
					return _color;
				}
			set
				{
					_color = value;
					this.Invalidate ();
				}
		}


		public override void Draw (Canvas canvas)
		{
			var centerY = canvas.ClipBounds.ExactCenterY ();
			var centerX = canvas.ClipBounds.ExactCenterX ();
			float radius = centerY > centerX ? centerX : centerY;			

			Paint paint = new Paint();
			paint.SetStyle(Paint.Style.Fill);
			paint.Color = Color; 
			paint.AntiAlias = true;
			paint.Alpha = 150; // max 255

			canvas.DrawCircle(centerX, centerY, radius, paint); 
		}
    }
}

