#if !UAP10_0
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

		/// <inheritdoc/>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
		public override unsafe Boolean IsImageMethod(RuntimeMethodHandle methodHandle)
			=> this.IsReadOnlyAddress(methodHandle.Value.ToPointer());
	}
}
#endif