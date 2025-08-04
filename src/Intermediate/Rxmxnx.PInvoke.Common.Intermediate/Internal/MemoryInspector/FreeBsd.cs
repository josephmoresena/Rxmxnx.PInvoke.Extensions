namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	/// <summary>
	/// FreeBSD OS implementation of <see cref="MemoryInspector"/> class.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private sealed partial class FreeBsd : BsdInspector
	{
		/// <inheritdoc/>
		protected override void ProcessMaps() => Procstat.AppendMaps(this);
	}
}