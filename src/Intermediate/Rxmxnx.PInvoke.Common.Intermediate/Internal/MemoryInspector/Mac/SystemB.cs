namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Mac
	{
		/// <summary>
		/// Interop API for <c>libSystemB.dylib</c> library.
		/// </summary>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		private static unsafe class SystemB
		{
			/// <summary>
			/// Validates <paramref name="result"/> value.
			/// </summary>
			/// <param name="result">Resulting value.</param>
			public static void ValidateResult(Int32 result)
			{
				if (result >= 0) return;
				SystemB.ClearError();
			}
			/// <summary>
			/// Clears errno value.
			/// </summary>
			private static void ClearError()
			{
				ref Int32 err = ref *SystemB.GetErrorPointer();
				err = 0;
			}
#pragma warning disable SYSLIB1054
			[DllImport("libSystem.B.dylib", EntryPoint = "mach_task_self", SetLastError = false)]
			public static extern UInt32 GetTaskHandle();
			[DllImport("libSystem.B.dylib", EntryPoint = "mach_vm_region", SetLastError = false)]
			public static extern Int32 MemoryRegion(UInt32 taskHandle, void** address, out UInt64 size, UInt32 flavor,
				out MemoryInfo info, ref UInt32 count, out UInt32 objName);

			[DllImport("libSystem.B.dylib", EntryPoint = "__error", SetLastError = false)]
			private static extern Int32* GetErrorPointer();
#pragma warning restore SYSLIB1054
		}
	}
}