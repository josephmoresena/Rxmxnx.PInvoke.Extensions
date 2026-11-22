#if UAP10_0
namespace Rxmxnx.PInvoke.Internal;

internal abstract partial class MemoryInspector
{
	/// <summary>
	/// A <see cref="MemoryInspector"/> instance.
	/// </summary>
	public static MemoryInspector Instance { get; } = new Windows();
}
#endif