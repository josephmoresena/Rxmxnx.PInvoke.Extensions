#if NET5_0_OR_GREATER
namespace Rxmxnx.PInvoke;

public readonly partial struct ValPtr<T>
#if NET7_0_OR_GREATER
	: IParsable<ValPtr<T>>
#endif
{
	/// <inheritdoc cref="IntPtr.Parse(String)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ValPtr<T> Parse(String s) => (ValPtr<T>)IntPtr.Parse(s);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ValPtr<T> Parse(String s, NumberStyles style) => (ValPtr<T>)IntPtr.Parse(s, style);
	/// <inheritdoc cref="IntPtr.Parse(String, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ValPtr<T> Parse(String s, IFormatProvider? provider) => (ValPtr<T>)IntPtr.Parse(s, provider);
	/// <inheritdoc cref="IntPtr.Parse(String, NumberStyles, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ValPtr<T> Parse(String s, NumberStyles style, IFormatProvider? provider)
		=> (ValPtr<T>)IntPtr.Parse(s, style, provider);
#if NET6_0_OR_GREATER
	/// <inheritdoc cref="IntPtr.Parse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static ValPtr<T> Parse(ReadOnlySpan<Char> s, NumberStyles style = NumberStyles.Integer,
		IFormatProvider? provider = default)
		=> (ValPtr<T>)IntPtr.Parse(s, style, provider);
#endif
	/// <inheritdoc cref="IntPtr.TryParse(String?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(String?, NumberStyles, IFormatProvider?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, NumberStyles style, IFormatProvider? provider,
		out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
#if NET6_0_OR_GREATER
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse(ReadOnlySpan<Char> s, out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
	/// <inheritdoc cref="IntPtr.TryParse(ReadOnlySpan{Char}, NumberStyles, IFormatProvider?, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse(ReadOnlySpan<Char> s, NumberStyles style, IFormatProvider? provider,
		out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, style, provider, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
#endif
#if NET7_0_OR_GREATER
	/// <inheritdoc cref="IntPtr.TryParse(String, IFormatProvider, out IntPtr)"/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean TryParse([NotNullWhen(true)] String? s, IFormatProvider? provider, out ValPtr<T> result)
	{
		Unsafe.SkipInit(out result);
		return IntPtr.TryParse(s, provider, out Unsafe.As<ValPtr<T>, IntPtr>(ref result));
	}
#endif
}
#endif