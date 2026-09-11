#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
using MemoryMarshalCompat = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class BufferTypeMetadata
{
#pragma warning disable CS8500
	/// <summary>
	/// Creates a new span of <typeparamref name="T"/> elements.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="buffer">A managed <typeparamref name="TBuffer"/> reference.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <returns>A <typeparamref name="T"/> span.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Span<T> CreateSpan<T, TBuffer>(ref TBuffer buffer, Int32 spanLength)
		=> MemoryMarshalCompat.CreateUnsafeSpan<T>(Unsafe.AsPointer(ref buffer), spanLength);
	/// <summary>
	/// Touches buffer elements so <typeparamref name="TBuffer"/> stays on the stack.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="buffer">A managed <typeparamref name="TBuffer"/> reference.</param>
	/// <param name="spanLength">Required span length.</param>
	/// <remarks>
	/// Old JIT can drop the unused local struct. This is a stack keep-alive (not
	/// <see cref="GC.KeepAlive"/>, which is for objects). Always writes the first element; writes
	/// the last only when <paramref name="spanLength"/> is greater than 1.
	/// </remarks>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Clear<T, TBuffer>(ref TBuffer buffer, Int32 spanLength) where TBuffer : struct
	{
		ref T r0 = ref Unsafe.As<TBuffer, T>(ref buffer);
		r0 = default!; // First element.
		if (spanLength <= 1) return;
		ref T r = ref Unsafe.Add(ref r0, spanLength - 1);
		r = default!; // Last element.
	}
#pragma warning restore CS8500
}
#endif
