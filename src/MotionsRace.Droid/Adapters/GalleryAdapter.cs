using Android.Widget;
using Android.Views;
using Android.Content;
using Android.Graphics;

namespace MotionsRace.Droid.Adapters
{
	public class GalleryAdapter: BaseAdapter
	{
		Context context;
		Bitmap[] slides;

		public GalleryAdapter (Context c, Bitmap[] thumbIds)
		{
			slides = thumbIds;
			context = c;
		}

		public override int Count { get { return slides.Length; } }

		public override Java.Lang.Object GetItem (int position)
		{
			return null;
		}

		public override long GetItemId (int position)
		{
			return 0;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ImageView i = new ImageView (context);

			i.SetImageBitmap(slides[position]);
			i.SetScaleType(ImageView.ScaleType.CenterCrop);
			i.LayoutParameters = new Gallery.LayoutParams(Gallery.LayoutParams.MatchParent, Gallery.LayoutParams.MatchParent);
			//i.SetScaleType (ImageView.ScaleType.CenterCrop);
			return i;
		}

	}
}