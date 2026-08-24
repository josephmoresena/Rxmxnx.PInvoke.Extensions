using System;
using System.IO;
using System.Text;

using Rxmxnx.PInvoke.ApplicationTest;

using UIKit;

namespace iOSAppTest
{
	public class Application
	{
		public static readonly String RuntimeInfo;

		static Application()
		{
			StringBuilder sb = new();
			using (StringWriter writer = new(sb))
				FeatureHelper.MainEntryPoint(writer);
			Application.RuntimeInfo = sb.ToString();
		}

		// This is the main entry point of the application.
		[Obsolete]
		private static void Main(String[] args)
		{
			// if you want to use a different Application Delegate class from "AppDelegate"
			// you can specify it here.
			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}