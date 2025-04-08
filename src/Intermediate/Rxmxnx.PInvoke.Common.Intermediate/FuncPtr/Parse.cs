namespace Rxmxnx.PInvoke;

public readonly partial struct FuncPtr<TDelegate>
#if NET7_0_OR_GREATER
	: IParsable<FuncPtr<TDelegate>>
{
	/// <inheritdoc cref="IntPtr.TryParse(String, IFormatProvider, out IntPtr)"/>
	public static Boolean TryParse([NotNullWhen(true)] String? s, IFormatProvider? provider,
		out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, provider, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
#else
{
#endif
	/// <inheritdoc cref="IntPtr.Parse(String)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static FuncPtr<TDelegate> Parse(String s) => (FuncPtr<TDelegate>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static FuncPtr<TDelegate> Parse(String s, NumberStyles style) => (FuncPtr<TDelegate>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static FuncPtr<TDelegate> Parse(String s, IFormatProvider? provider)
		=> (FuncPtr<TDelegate>)IntPtr.Parse(s, provider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static FuncPtr<TDelegate> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (FuncPtr<TDelegate>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static FuncPtr<TDelegate> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = default)
		=> (FuncPtr<TDelegate>)IntPtr.Parse(s, style, provider);
	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse(ReadOnlySpan<Char> s, out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out FuncPtr<TDelegate> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<FuncPtr<TDelegate>, IntPtr>(ref result));
	}
}