#if !UAP10_0
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
		protected static unsafe class StandardC
		{
			/// <summary>
			/// POSIX process identifier.
			/// </summary>
			public static readonly Int32 ProcessId = StandardC.GetProcessId();

			/// <summary>
			/// Indicates whether <paramref name="address"/> belongs to a loaded image.
			/// </summary>
			/// <param name="address">Function address.</param>
			/// <returns>A non-zero value when <paramref name="address"/> belongs to a loaded image.</returns>
			public static Int32 LocateImage(IntPtr address)
			{
				Span<Byte> image = stackalloc Byte[4 * IntPtr.Size];
				fixed (void* imagePtr = &MemoryMarshal.GetReference(image))
					return StandardC.LocateImage(address, imagePtr);
			}

#pragma warning disable SYSLIB1054
			[DllImport("libc", EntryPoint = "free")]
			public static extern void Free(void* ptr);
			[DllImport("libc", EntryPoint = "getpid", SetLastError = false)]
			private static extern Int32 GetProcessId();
			[DllImport("libc", EntryPoint = "dladdr", SetLastError = false)]
			private static extern Int32 LocateImage(IntPtr address, void* image);
#pragma warning restore SYSLIB1054
		}
	}
}
#endif