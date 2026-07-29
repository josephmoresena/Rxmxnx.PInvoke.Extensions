#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
namespace Rxmxnx.PInvoke.Internal.FrameworkCompat;

internal static partial class MemoryMarshalCompat
{
	/// <summary>
	/// Internal struct with span offsets.
	/// </summary>
	private readonly struct SpanOffset
	{
		/// <summary>
		/// Offset to <c>Pinnable&lt;T&gt; _pinnable</c> field.
		/// </summary>
		public Int32 PinnableOffset { get; init; }
		/// <summary>
		/// Offset to <c>IntPtr _byteOffset</c> field.
		/// </summary>
		public Int32 PointerOffset { get; init; }
		/// <summary>
		/// Offset to <c>_length</c> field.
		/// </summary>
		public Int32 LengthOffset { get; init; }
	}
}
#endif