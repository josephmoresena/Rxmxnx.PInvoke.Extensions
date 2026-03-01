using System;

using CoreGraphics;

using UIKit;

namespace iOSAppTest
{
	public partial class ViewController : UIViewController
	{
		public ViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			if (this.View == null) return;
			// Perform any additional setup after loading the view, typically from a nib.
			UITextView textView = new(new CGRect(20, 100, this.View.Bounds.Width - 40, this.View.Bounds.Height - 120))
			{
				Editable = false,
				Selectable = true,
				ScrollEnabled = true,
				Bounces = true,
				ShowsVerticalScrollIndicator = true,
				IndicatorStyle = UIScrollViewIndicatorStyle.Default,
				Font = UIFont.SystemFontOfSize(16),
				TextColor = UIColor.Black,
#pragma warning disable CS0618
				BackgroundColor = UIColor.White,
#pragma warning restore CS0618
				Text = Application.RuntimeInfo,
			};
			this.View.AddSubview(textView);
		}
	}
}