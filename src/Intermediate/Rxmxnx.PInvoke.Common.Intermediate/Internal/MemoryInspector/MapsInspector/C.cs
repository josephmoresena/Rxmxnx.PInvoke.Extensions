namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private abstract partial class MapsInspector
	{
		/// <summary>
		/// Interop API for <c>libc.so</c> library.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		protected static unsafe class C
		{
			/// <summary>
			/// POSIX process identifier.
			/// </summary>
			public static readonly Int32 ProcessId = C.GetProcessId();

#pragma warning disable SYSLIB1054
			[DllImport("libc", EntryPoint = "free")]
			public static extern void Free(void* ptr);

			[DllImport("libc", EntryPoint = "getpid", SetLastError = false)]
			private static extern Int32 GetProcessId();
#pragma warning restore SYSLIB1054
		}
	}
}