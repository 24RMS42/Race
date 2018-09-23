using System;
using Android.Views;
using Android.Content;
using Android.Util;
using Android.Widget;
using MvvmCross.Binding.Droid.Views;

namespace MotionsRace.Droid.Controls
{
	public class MvxListViewNoScroll : MvxListView
	{

		protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
		{
			base.OnMeasure (widthMeasureSpec, MeasureSpec.MakeMeasureSpec(0,MeasureSpecMode.Unspecified));
			int childHeight = MeasuredHeight - (ListPaddingTop + ListPaddingBottom + VerticalFadingEdgeLength * 2);
			int fullHeight = ListPaddingTop + ListPaddingBottom + childHeight * Count;
			SetMeasuredDimension (MeasuredWidth, fullHeight);
		}

		public MvxListViewNoScroll (Context context, IAttributeSet attrs, IMvxAdapter adapter) : base (context, attrs)
		{
			if (adapter == null) {
				return;
			}
			int itemTemplateId = MvxAttributeHelpers.ReadListItemTemplateId (context, attrs);
			adapter.ItemTemplateId = itemTemplateId;
			this.Adapter = adapter;
		}

		public MvxListViewNoScroll (Context context, IAttributeSet attrs) : this (context, attrs, new MvxAdapter (context))
		{
		}

	}
}

