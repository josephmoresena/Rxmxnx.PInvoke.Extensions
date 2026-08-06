using System;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal static class Program
	{
		public static void Main(String[] args)
		{
#if NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
			FeatureHelper.MainEntryPoint(Console.Out);
#endif
		}
	}
}