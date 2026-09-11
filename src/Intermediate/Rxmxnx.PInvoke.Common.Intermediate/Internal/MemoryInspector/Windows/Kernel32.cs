namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	private sealed partial class Windows
	{
		/// <summary>
		/// Interop API for <c>Kernel32.dll</c> library.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static unsafe class Kernel32
		{
			/// <summary>
			/// Validates <paramref name="result"/> value.
			/// </summary>
			/// <param name="result">Resulting value.</param>
			public static void ValidateResult(UIntPtr result)
			{
				if (result != default) return;
				Kernel32.SetLastError(0);
			}
#pragma warning disable SYSLIB1054
			[DllImport("kernel32.dll")]
			public static extern UIntPtr VirtualQuery(void* lpAddress, out MemoryInfo memInfo, UIntPtr dwLength);

			[DllImport("kernel32.dll")]
			private static extern void SetLastError(UInt32 dwErrCode);
#pragma warning restore SYSLIB1054
		}
	}
}