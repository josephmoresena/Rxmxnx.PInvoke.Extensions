using System;

using AppKit;

using CoreGraphics;

using Foundation;

namespace MacAppTest
{
	public partial class ViewController : NSViewController
	{
		public ViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			// Do any additional setup after loading the view.
			CGRect frame = new(20, 20, 300, 200);
			NSScrollView scrollView = new(frame)
			{
				HasVerticalScroller = true,
				HasHorizontalScroller = false,
				AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
				DocumentView = new NSTextView(new CGRect(0, 0, frame.Width, frame.Height))
				{
					Editable = false,
					Selectable = true,
					VerticallyResizable = true,
					HorizontallyResizable = false,
					AutoresizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable,
					Font = NSFont.SystemFontOfSize(16),
					TextColor = NSColor.WindowFrameText,
					BackgroundColor = NSColor.WindowBackground,
					Value = MainClass.RuntimeInfo,
				},
			};
			this.View.AddSubview(scrollView);
		}
	}
}