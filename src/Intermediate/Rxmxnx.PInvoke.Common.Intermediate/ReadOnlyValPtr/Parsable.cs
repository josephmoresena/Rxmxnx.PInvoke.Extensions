namespace Rxmxnx.PInvoke;

public readonly partial struct ReadOnlyValPtr<T>
#if NET7_0_OR_GREATER
	: IParsable<ReadOnlyValPtr<T>>
{
	/// <inheritdoc cref="IntPtr.TryParse(String, IFormatProvider, out IntPtr)"/>
	public static Boolean TryParse([NotNullWhen(true)] String? s, IFormatProvider? provider,
		out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, provider, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
#else
{
#endif
	/// <inheritdoc cref="IntPtr.Parse(String)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyValPtr<T> Parse(String s) => (ReadOnlyValPtr<T>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyValPtr<T> Parse(String s, NumberStyles style) => (ReadOnlyValPtr<T>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyValPtr<T> Parse(String s, IFormatProvider? provider)
		=> (ReadOnlyValPtr<T>)IntPtr.Parse(s, provider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyValPtr<T> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (ReadOnlyValPtr<T>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ReadOnlyValPtr<T> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = default)
		=> (ReadOnlyValPtr<T>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse(ReadOnlySpan<Char> s, out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out ReadOnlyValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ReadOnlyValPtr<T>, IntPtr>(ref result));
	}
}