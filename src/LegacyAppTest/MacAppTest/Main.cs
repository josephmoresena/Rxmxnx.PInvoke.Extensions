using System;
using System.Text;

using AppKit;

using Rxmxnx.PInvoke.ApplicationTest;

namespace MacAppTest
{
	internal static class MainClass
	{
		public static readonly String RuntimeInfo;

		static MainClass()
		{
			StringBuilder sb = new();
			RuntimeHelper.PrintRuntimeInfo(sb);
			MainClass.RuntimeInfo = sb.ToString();
		}

		private static void Main(String[] args)
		{
			NSApplication.Init();
			NSApplication.Main(args);
		}
	}
}