using System;

using AppKit;

namespace MacAppTest
{
	public partial class ViewController : NSViewController
	{
		private const NSViewResizingMask resizingMask = NSViewResizingMask.WidthSizable | NSViewResizingMask.HeightSizable;

		public ViewController(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NSScrollView scrollView = new(View.Bounds)
			{
				HasVerticalScroller = true,
				HasHorizontalScroller = false,
				AutoresizingMask = ViewController.resizingMask,
				DocumentView = new NSTextView(View.Bounds)
				{
					Editable = false,
					Selectable = true,
					VerticallyResizable = true,
					HorizontallyResizable = false,
					AutoresizingMask = ViewController.resizingMask,
					Font = NSFont.SystemFontOfSize(16),
					TextColor = NSApplication.SharedApplication.EffectiveAppearance.Name == NSAppearance.NameDarkAqua ?
						NSColor.White : NSColor.Black,
					BackgroundColor = NSColor.WindowBackground,
					Value = MainClass.RuntimeInfo
				}
			};
			View.AddSubview(scrollView);
		}
		public override void ViewDidAppear()
		{
			base.ViewDidAppear();
			if (this.View.Window is null) return;
			this.View.Window.SetContentSize(new(862, 480));
			this.View.Window.Center();
		}
	}
}