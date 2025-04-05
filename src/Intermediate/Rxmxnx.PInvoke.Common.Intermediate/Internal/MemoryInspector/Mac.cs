namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// MacOS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
	private sealed unsafe partial class Mac : MemoryInspector
	{
		/// <inheritdoc/>
		public override Boolean IsLiteral<T>(ReadOnlySpan<T> span)
		{
#pragma warning disable CS8500
			fixed (void* ptr = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
			{
				UInt32 taskHandle = SystemB.GetTaskHandle();
				UInt32 count = MemoryInfo.Count;
				Int32 result = SystemB.MemoryRegion(taskHandle, &ptr, out _, MemoryInfo.Flavor, out MemoryInfo info,
				                                    ref count, out _);
				SystemB.ValidateResult(result);
				return info.Protection is Protection.Read and not Protection.Write;
			}
		}
	}
}