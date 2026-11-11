#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public sealed unsafe partial class CString
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class that contains the UTF-8 string
	/// returned by the specified <see cref="ReadOnlySpanFunc{Byte}"/>.
	/// </summary>
	/// <param name="func">
	/// <see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns the UTF-8 string to initialize
	/// the new instance.
	/// </param>
	public CString(delegate*<ReadOnlySpan<Byte>> func) : this(func, true) { }

	/// <summary>
	/// Initializes a new instance of the <see cref="CString"/> class that contains the UTF-8 string
	/// returned by the specified <see cref="ReadOnlySpanFunc{Byte}"/>.
	/// </summary>
	/// <param name="func"><see cref="ReadOnlySpanFunc{Byte}"/> delegate that returns the UTF-8 string.</param>
	/// <param name="isLiteral">Indicates whether returned span is from UTF-8 literal.</param>
	private CString(delegate*<ReadOnlySpan<Byte>> func, Boolean isLiteral)
	{
		this._isLocal = false;
		this._data = ValueRegion<Byte>.Create(func);
		this.IsFunction = true;

		ReadOnlySpan<Byte> data = func();
		if (!isLiteral)
		{
			this._isNullTerminated = CString.IsNullTerminatedSpan(data, out this._length);
			return;
		}

		this._isNullTerminated = true;
		this._length = data.Length;
	}

	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <typeparam name="TState">Type of the state object.</typeparam>
	/// <param name="state">Function state parameter.</param>
	/// <param name="getSpan">Function to retrieve utf-8 span from the state.</param>
	/// <param name="isNullTerminated">Indicates whether resulting UTF-8 text is null-terminated.</param>
	/// <param name="tryAlloc">Allocation state function pointer.</param>
	/// <returns>A new instance of the <see cref="CString"/> class.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static CString Create<TState>(TState state, delegate*<TState, ReadOnlySpan<Byte>> getSpan,
		Boolean isNullTerminated, delegate*<TState, GCHandleType, out GCHandle, Boolean> tryAlloc = default)
	{
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(state, getSpan, tryAlloc);
		return new(data, true, isNullTerminated, getSpan(state).Length);
	}
#if NET7_0_OR_GREATER
	/// <summary>
	/// Creates a new instance of the <see cref="CString"/> class using a <typeparamref name="TState"/> instance.
	/// </summary>
	/// <typeparam name="TState">An <see cref="IUtf8FunctionState{TState}"/> type.</typeparam>
	/// <param name="state">Function state parameter.</param>
	/// <returns>
	/// A new instance of the <see cref="CString"/> class.
	/// </returns>
	public static CString Create<TState>(TState state) where TState : struct, IUtf8FunctionState<TState>
	{
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(state, &TState.GetSpan, &TState.TryAlloc);
		Int32 length = TState.GetLength(state);
		return new(data, true, state.IsNullTerminated, length);
	}
	/// <summary>
	/// Creates a new <see cref="CString"/> instance from the UTF-8 literal provided by the specified source type.
	/// </summary>
	/// <typeparam name="TSource">Type that provides the UTF-8 literal associated with the specified identifier.</typeparam>
	/// <typeparam name="TIdentifier">Type of the identifier used to retrieve the UTF-8 literal.</typeparam>
	/// <param name="identifier">Identifier used to retrieve the UTF-8 literal.</param>
	/// <returns>
	/// A <see cref="CString"/> containing the UTF-8 literal associated with <paramref name="identifier"/>.
	/// </returns>
	public static CString Create<TSource, TIdentifier>(TIdentifier identifier)
		where TSource : IUtf8LiteralSource<TIdentifier>
	{
		Int32 length = TSource.GetLiteral(identifier).Length;
		ValueRegion<Byte> data = ValueRegion<Byte>.Create(identifier, &TSource.GetLiteral);
		return new(data, true, true, length);
	}
#endif
}
#endif