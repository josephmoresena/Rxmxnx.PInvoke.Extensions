namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// NetBsd OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed partial class NetBsd : BsdInspector
	{
		/// <inheritdoc/>
		protected override void ProcessMaps()
		{
#if NET5_0_OR_GREATER
			if (SystemInfo.IsLinux || SystemInfo.IsFreeBsd) return;
#endif
			Util.AppendMaps(this);
		}
	}
}