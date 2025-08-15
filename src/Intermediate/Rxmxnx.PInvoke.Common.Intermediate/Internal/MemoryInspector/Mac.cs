namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// MacOS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	private sealed unsafe partial class Mac : MemoryInspector
	{
		/// <inheritdoc/>
		public override Boolean IsLiteral(ReadOnlySpan<Byte> span)
		{
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
			{
				UInt32 taskHandle = SystemB.GetTaskHandle();
				UInt32 count = MemoryInfo.Count;
				Int32 result = SystemB.MemoryRegion(taskHandle, &ptr, out _, MemoryInfo.Flavor, out MemoryInfo info,
				                                    ref count, out _);
				SystemB.ValidateResult(result);
				return (info.Protection & Protection.Read) == Protection.Read &&
					(info.Protection & Protection.Write) == Protection.None;
			}
		}
	}
}