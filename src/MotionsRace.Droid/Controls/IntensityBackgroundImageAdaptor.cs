using System;
using Android.Widget;
using Android.Content;
using Android.Views;

namespace MotionsRace.Droid.Controls
{
	public class IntensityBackgroundImageAdaptor : BaseAdapter
	{
		Context context;

		public IntensityBackgroundImageAdaptor (Context c, int count, int drawable)
		{
			context = c;
			thumbIds = new int[count];
			for (var i = 0; i < count; i++) 
			{
				thumbIds[i] = drawable;
				
			}
		}

		public override int Count {
			get { return thumbIds.Length; }
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		// create a new ImageView for each item referenced by the Adapter
		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ImageView imageView;

			if (convertView == null) {  // if it's not recycled, initialize some attributes
				imageView = new ImageView (context);
			} else {
				imageView = (ImageView)convertView;
			}

			imageView.SetImageResource (thumbIds[position]);
			return imageView;
		}

		// references to our images
		int[] thumbIds;


	}
}

