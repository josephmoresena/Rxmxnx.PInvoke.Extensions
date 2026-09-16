#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
using MemoryMarshalCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic operations for managed buffer instances.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
public static class BufferExtensions
{
	/// <summary>
	/// Casts a span of <typeparamref name="TBuffer"/> binary buffer to a span of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="TBuffer">The type of buffer.</typeparam>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="span">Span of <typeparamref name="TBuffer"/> binary buffer.</param>
	/// <param name="many">Output. Span of <typeparamref name="T"/> items.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static void AsMany<TBuffer, T>(this Span<TBuffer> span, out Span<T> many)
		where TBuffer : struct, IManagedBuffer<T>
	{
		BufferTypeMetadata<T> bufferTypeMetadata = BuffersHelper.GetStaticMetadata<T, TBuffer>();
		ValidationUtilities.ThrowIfNotNestedBuffer(typeof(TBuffer), typeof(T), bufferTypeMetadata.Size,
		                                           bufferTypeMetadata.SizeOf, bufferTypeMetadata.SizeOfElement);
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		many = MemoryMarshalCompat.Cast<TBuffer, T>(span);
#else
		many = MemoryMarshal.CreateSpan(ref Unsafe.As<TBuffer, T>(ref MemoryMarshal.GetReference(span)),
		                                span.Length * BuffersHelper.GetStaticMetadata<T, TBuffer>().Size);
#endif
	}
	/// <summary>
	/// Casts a read-only span of <typeparamref name="TBuffer"/> binary buffer to a read-only span of <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="TBuffer">The type of buffer.</typeparam>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <param name="span">Read-only span of <typeparamref name="TBuffer"/> binary buffer.</param>
	/// <param name="many">Output. Read-only span of <typeparamref name="T"/> items.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static void AsMany<TBuffer, T>(this ReadOnlySpan<TBuffer> span, out ReadOnlySpan<T> many)
		where TBuffer : struct, IManagedBuffer<T>
	{
		BufferTypeMetadata<T> bufferTypeMetadata = BuffersHelper.GetStaticMetadata<T, TBuffer>();
		ValidationUtilities.ThrowIfNotNestedBuffer(typeof(TBuffer), typeof(T), bufferTypeMetadata.Size,
		                                           bufferTypeMetadata.SizeOf, bufferTypeMetadata.SizeOfElement);
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		many = MemoryMarshalCompat.Cast<TBuffer, T>(span);
#else
		many = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.As<TBuffer, T>(ref MemoryMarshal.GetReference(span)),
		                                        span.Length * BuffersHelper.GetStaticMetadata<T, TBuffer>().Size);
#endif
	}
}