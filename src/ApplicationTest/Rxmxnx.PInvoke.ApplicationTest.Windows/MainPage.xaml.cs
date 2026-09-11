using System;
using System.IO;
using System.Text;

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Rxmxnx.PInvoke.ApplicationTest
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage
	{
		public MainPage()
		{
			this.InitializeComponent();
			this.Content = new ScrollViewer
			{
				Content = new TextBlock
				{
					Text = MainPage.GetRuntimeInfo(),
					IsTextSelectionEnabled = true,
					TextWrapping = TextWrapping.Wrap,
					HorizontalAlignment = HorizontalAlignment.Stretch,
					VerticalAlignment = VerticalAlignment.Stretch,
#if !CSHARP9_0
					Margin = new Thickness(15),
#else
					Margin = new(15),
#endif
				},
				HorizontalAlignment = HorizontalAlignment.Stretch,
				VerticalAlignment = VerticalAlignment.Stretch,
				HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
				VerticalScrollBarVisibility = ScrollBarVisibility.Auto
			};
		}

		private static String GetRuntimeInfo()
		{
#if !CSHARP9_0
			StringBuilder sb = new();
			using (StringWriter writer = new(sb))
#else
			StringBuilder sb = new StringBuilder();
			using (StringWriter writer = new StringWriter(sb))
#endif
				FeatureHelper.MainEntryPoint(writer);
			return sb.ToString();
		}
	}
}