namespace Rxmxnx.PInvoke.Internal;

internal partial class Utf8Comparator
{
	/// <summary>
	/// Represents a 32-bit chunk containing two UTF-16 code units.
	/// </summary>
#if !PACKAGE && NETCOREAPP && !NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[StructLayout(LayoutKind.Explicit, Size = sizeof(Int32))]
	private readonly struct CharSpanChunk
	{
		/// <summary>
		/// Integer value for fast comparision.
		/// </summary>
		[FieldOffset(0)]
		public readonly Int32 Value;

		/// <summary>
		/// Pair for compare calculation.
		/// </summary>
		[FieldOffset(0)]
		private readonly Pair<Char> _pair;

		/// <summary>
		/// Compares two instances of <see cref="CharSpanChunk"/> instance.
		/// </summary>
		/// <param name="chunkA">First <see cref="CharSpanChunk"/> instance.</param>
		/// <param name="chunkB">Second <see cref="CharSpanChunk"/> instance.</param>
		/// <returns>Comparision value.</returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static Int32 Compare(CharSpanChunk chunkA, CharSpanChunk chunkB)
		{
			if (chunkA.Value == chunkB.Value) return 0;
			Int32 result = chunkA._pair.T0 - chunkB._pair.T0;
			return result != 0 ? result : chunkA._pair.T1 - chunkB._pair.T1;
		}
	}
}