namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// Linux OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed partial class FreeBsd : MapsInspector
	{
		/// <inheritdoc/>
		protected override void ProcessMaps() => Procstat.AppendMaps(this);
	}
}