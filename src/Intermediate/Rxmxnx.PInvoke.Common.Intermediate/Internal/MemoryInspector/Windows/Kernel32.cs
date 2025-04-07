namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Windows
	{
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static unsafe class Kernel32
		{
			[DllImport("kernel32.dll")]
			public static extern UIntPtr VirtualQuery(void* lpAddress, out MemoryInfo memInfo, UIntPtr dwLength);

			[DllImport("kernel32.dll")]
			private static extern void SetLastError(UInt32 dwErrCode);

			/// <summary>
			/// Validates <paramref name="result"/> value.
			/// </summary>
			/// <param name="result">Resulting value.</param>
			public static void ValidateResult(UIntPtr result)
			{
				if (result != default) return;
				Kernel32.SetLastError(0);
			}
		}
	}
}