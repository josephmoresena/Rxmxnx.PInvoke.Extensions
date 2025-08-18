namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class FreeBsd
	{
		/// <summary>
		/// Interop API for <c>libutil.so</c> library.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static class Util
		{
			/// <summary>
			/// Pointer to current process Kernel Information.
			/// </summary>
			public static readonly IntPtr KernelInfo = Util.GetKInfo(C.ProcessId);
#pragma warning disable SYSLIB1054
			[DllImport("libutil", EntryPoint = "kinfo_getproc", SetLastError = true)]
			private static extern IntPtr GetKInfo(Int32 pid);
#pragma warning restore SYSLIB1054
		}
	}
}