#if !NETCOREAPP || NET7_0_OR_GREATER
namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator
{
	/// <summary>
	/// Represents a 64-bit chunk containing four UTF-16 code units.
	/// </summary>
	[StructLayout(LayoutKind.Explicit, Size = sizeof(Int64))]
	private readonly struct DoubleCharSpanChunk
	{
		/// <summary>
		/// Long value for fast comparision.
		/// </summary>
		[FieldOffset(0)]
		public readonly Int64 Value;

		/// <summary>
		/// Pair for compare calculation.
		/// </summary>
		[FieldOffset(0)]
		private readonly Pair<CharSpanChunk> _pair;

		/// <summary>
		/// Compares two instances of <see cref="DoubleCharSpanChunk"/> instance.
		/// </summary>
		/// <param name="chunkA">First <see cref="DoubleCharSpanChunk"/> instance.</param>
		/// <param name="chunkB">Second <see cref="DoubleCharSpanChunk"/> instance.</param>
		/// <returns>Comparision value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int32 Compare(DoubleCharSpanChunk chunkA, DoubleCharSpanChunk chunkB)
		{
			if (chunkA.Value == chunkB.Value) return 0;
			Int32 result = CharSpanChunk.Compare(chunkA._pair.T0, chunkB._pair.T0);
			return result != 0 ? result : CharSpanChunk.Compare(chunkA._pair.T1, chunkB._pair.T1);
		}
	}
}
#endif