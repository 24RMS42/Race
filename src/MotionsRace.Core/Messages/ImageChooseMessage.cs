using System;
using MvvmCross.Plugins.Messenger;

namespace MotionsRace.Core.Messages
{
	public class ImageChooseMessage : MvxMessage
	{
		public ImageChooseMessage (object sender, object item)
			: base(sender)
		{
			ImageBytes = (byte[])item;
		}

		public byte[] ImageBytes { get; set; }
	}
}


