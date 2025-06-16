namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// State for temporal functional <see cref="CString"/>.
	/// </summary>
	private readonly struct CStringStringState(String value)
#if !PACKAGE && NET6_0 || NET7_0_OR_GREATER
		: IUtf8FunctionState<CStringStringState>
#endif
	{
		/// <summary>
		/// Internal text value.
		/// </summary>
		private readonly String _value = value;

		/// <summary>
		/// Internal text UTF-8 length.
		/// </summary>
		public Int32 Utf8Length { get; } = Encoding.UTF8.GetByteCount(value);

#if !PACKAGE && NET6_0 || NET7_0_OR_GREATER
		Boolean IUtf8FunctionState<CStringStringState>.IsNullTerminated => false;

#if NET6_0
		[RequiresPreviewFeatures]
#endif
		static ReadOnlySpan<Byte> IUtf8FunctionState<CStringStringState>.GetSpan(CStringStringState state)
			=> Encoding.UTF8.GetBytes(state._value);
#if NET6_0
		[RequiresPreviewFeatures]
#endif
		static Int32 IUtf8FunctionState<CStringStringState>.GetLength(in CStringStringState state) => state.Utf8Length;
#endif
#if (PACKAGE || !NET6_0) && !NET7_0_OR_GREATER
		/// <summary>
		/// Retrieves the span of the UTF-8 text represented by this state.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static ReadOnlySpan<Byte> GetSpan(CStringStringState state) => Encoding.UTF8.GetBytes(state._value);
#endif
	}
}