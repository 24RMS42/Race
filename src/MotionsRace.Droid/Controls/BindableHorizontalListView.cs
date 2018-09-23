/*
 * BindableHorizontalListView.cs
 *  
 * Copyright (c) 2012 Tomasz Cielecki (tomasz@ostebaronen.dk)
 *  
 * The MIT License 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using System.Collections;
using System.Windows.Input;
using Android.Content;
using Android.Util;
using Cirrious.MvvmCross.Binding.Android.Views;
using MvvmCross.Binding.Attributes;
using MvvmCross.Binding.Droid.Views;

namespace MotionsRace.Droid.Controls
{
	public class BindableHorizontalListView
		: HorizontalListView
	{
		public BindableHorizontalListView(Context context, IAttributeSet attrs) 
			: this(context, attrs, new MvxAdapter(context))
		{
		}

		public BindableHorizontalListView(Context context, IAttributeSet attrs, MvxAdapter adapter)
			: base(context, attrs)
		{
			var itemTemplateId = MvxBindableListViewHelpers.ReadTemplatePath (context, attrs);

			adapter.ItemTemplateId = itemTemplateId;                                                                        
			Adapter = adapter;
			SetupItemClickListener();
			SetupScreenChangedListener();
		}

		public new MvxAdapter Adapter
		{
			get { return base.Adapter as MvxAdapter; }
			set
			{
				var existing = Adapter;
				if (existing == value)
					return;

				if (existing != null && value != null)
				{
					value.ItemsSource = existing.ItemsSource;
					value.ItemTemplateId = existing.ItemTemplateId;
				}

				base.Adapter = value;
			}
		}

		[MvxSetToNullAfterBinding]
		public IEnumerable ItemsSource
		{
			get { return Adapter.ItemsSource; }
			set { Adapter.ItemsSource = value; }
		}

		public int ItemTemplateId
		{
			get { return Adapter.ItemTemplateId; }
			set { Adapter.ItemTemplateId = value; }
		}

		public new ICommand ItemClick { get; set; }

		private void SetupItemClickListener()
		{
			base.ItemClick += (sender, args) =>
			{
				if (null == ItemClick)
					return;
				var item = Adapter.GetRawItem(args.Position);
				if (item == null)
					return;

				if (!ItemClick.CanExecute(item))
					return;

				ItemClick.Execute(item);
			};
		}



		public new ICommand ScreenChanged { get; set; }

		private void SetupScreenChangedListener()
		{
			base.ScreenChanged += (sender, args) =>
			{
				if (null == ScreenChanged)
					return;

				if (null == args)
					return;

				var cArgs = new {
					CurrentScreen = args.CurrentScreen,
					CurrentX = args.CurrentX
				};

				if (!ScreenChanged.CanExecute(cArgs))
					return;

				ScreenChanged.Execute(cArgs);
			};
		}
	}
}