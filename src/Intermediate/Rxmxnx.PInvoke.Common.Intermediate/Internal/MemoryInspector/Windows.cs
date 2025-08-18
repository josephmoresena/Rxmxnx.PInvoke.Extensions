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
		public override Boolean IsLiteral(ReadOnlySpan<Byte> span)
		{
#pragma warning disable CS8500
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
			{
				UIntPtr result = Kernel32.VirtualQuery(ptr, out MemoryInfo memInfo, MemoryInfo.Size);
				Kernel32.ValidateResult(result);
				return result != UIntPtr.Zero && memInfo.Protect is MemoryState.ReadOnly or MemoryState.ExecuteRead &&
					memInfo.Type.Value is MemoryState.Image or MemoryState.Mapped;
			}
		}
	}
}