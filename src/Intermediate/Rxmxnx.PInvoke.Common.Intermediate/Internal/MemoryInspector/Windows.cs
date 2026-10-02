namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Windows OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed unsafe partial class Windows : MemoryInspector
	{
		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Boolean IsReadOnlyAddress(void* ptr)
		{
			UIntPtr result = Kernel32.VirtualQuery(ptr, out MemoryInfo memInfo, MemoryInfo.Size);
			Kernel32.ValidateResult(result);
			return result != UIntPtr.Zero && memInfo.Protect is MemoryState.ReadOnly or MemoryState.ExecuteRead &&
				memInfo.Type.Value is MemoryState.Image or MemoryState.Mapped;
		}
		/// <inheritdoc/>
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public override Boolean IsImageMethod(RuntimeMethodHandle methodHandle)
		{
			void* address = methodHandle.GetFunctionPointer().ToPointer();
			Int32 found = Kernel32.LocateImage(Kernel32.AddressFlags, address, out IntPtr module);
			if (found == 0) Kernel32.ValidateResult(default);
			return found != 0 && module != IntPtr.Zero;
		}
	}
}