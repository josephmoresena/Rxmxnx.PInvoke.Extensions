namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// State for temporal functional <see cref="CString"/>.
	/// </summary>
	internal readonly struct CStringStringState(String value) : IUtf8FunctionState<CStringStringState>
	{
		/// <summary>
		/// Internal text value.
		/// </summary>
		private readonly String _value = value;
		/// <summary>
		/// Internal text UTF-8 length.
		/// </summary>
		private readonly Int32 _utf8Length = Encoding.UTF8.GetByteCount(value);

		Boolean IUtf8FunctionState<CStringStringState>.IsNullTerminated => false;
#if NET6_0
		[RequiresPreviewFeatures]
#endif
		static ReadOnlySpan<Byte> IUtf8FunctionState<CStringStringState>.GetSpan(CStringStringState state)
			=> Encoding.UTF8.GetBytes(state._value);
#if NET6_0
		[RequiresPreviewFeatures]
#endif
		static Int32 IUtf8FunctionState<CStringStringState>.GetLength(CStringStringState state) => state._utf8Length;
	}
}