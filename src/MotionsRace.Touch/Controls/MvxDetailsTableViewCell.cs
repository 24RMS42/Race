using System;
using UIKit;
using Cirrious.MvvmCross.Binding.Touch.Views;
using Cirrious.MvvmCross.Binding.BindingContext;
using System.Collections.Generic;
using Cirrious.MvvmCross.Binding.Bindings;
using CoreGraphics;
using Foundation;

namespace MotionsRace.Touch
{
	public class MvxDetailsTableViewCell
		: UITableViewCell
	, IMvxBindable
	{
		public IMvxBindingContext BindingContext { get; set; }

		public MvxDetailsTableViewCell()
			: this(string.Empty)
		{
		}

		public MvxDetailsTableViewCell(string bindingText)
		{
			this.CreateBindingContext(bindingText);
		}

		public MvxDetailsTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions)
		{
			this.CreateBindingContext(bindingDescriptions);
		}

		public MvxDetailsTableViewCell(string bindingText, CGRect frame)
			: base(frame)
		{
			this.CreateBindingContext(bindingText);
		}

		public MvxDetailsTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, CGRect frame)
			: base(frame)
		{
			this.CreateBindingContext(bindingDescriptions);
		}

		public MvxDetailsTableViewCell(IntPtr handle)
			: this(string.Empty, handle)
		{
		}

		public MvxDetailsTableViewCell(string bindingText, IntPtr handle)
			: base(handle)
		{
			this.CreateBindingContext(bindingText);
		}

		public MvxDetailsTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions, IntPtr handle)
			: base(handle)
		{
			this.CreateBindingContext(bindingDescriptions);
		}

		public MvxDetailsTableViewCell(string bindingText, UITableViewCellStyle cellStyle, NSString cellIdentifier,
			UITableViewCellAccessory tableViewCellAccessory =
			UITableViewCellAccessory.None)
			: base(cellStyle, cellIdentifier)
		{
			Accessory = tableViewCellAccessory;
			this.CreateBindingContext(bindingText);
		}

		public MvxDetailsTableViewCell(IEnumerable<MvxBindingDescription> bindingDescriptions,
			UITableViewCellStyle cellStyle, NSString cellIdentifier,
			UITableViewCellAccessory tableViewCellAccessory =
			UITableViewCellAccessory.None)
			: base(cellStyle, cellIdentifier)
		{
			Accessory = tableViewCellAccessory;
			this.CreateBindingContext(bindingDescriptions);
		}

		public MvxDetailsTableViewCell(IntPtr handle, UITableViewCellStyle cellStyle, string cellIdentifier)
			: base(cellStyle, cellIdentifier)
		{
			this.Handle = handle;
			//Accessory = tableViewCellAccessory;
			this.CreateBindingContext("");
		}

		public override void MovedToSuperview()
		{
			base.MovedToSuperview();
		}

		// we seal Accessory here so that we can use it in the constructor - otherwise virtual issues.
		public override sealed UITableViewCellAccessory Accessory
		{
			get { return base.Accessory; }
			set { base.Accessory = value; }
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				BindingContext.ClearAllBindings();
			}
			base.Dispose(disposing);
		}

		public virtual object DataContext
		{
			get { return BindingContext.DataContext; }
			set { BindingContext.DataContext = value; }
		}
	}
}

