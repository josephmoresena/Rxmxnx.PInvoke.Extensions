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
		public override Boolean IsLiteral<T>(ReadOnlySpan<T> span)
		{
#pragma warning disable CS8500
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
			{
				UIntPtr result = Kernel32.VirtualQuery(ptr, out MemoryInfo memInfo, MemoryInfo.Size);
				Kernel32.ValidateResult(result);
				return result != UIntPtr.Zero && memInfo.Protect switch
				{
					MemoryState.Image => true,
					MemoryState.ReadOnly => memInfo.Type is MemoryState.Image,
					MemoryState.ExecuteRead => memInfo.Type is MemoryState.Image,
					_ => false,
				};
			}
		}
	}
}