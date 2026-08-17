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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Span<T> CreateSpan<T, TBuffer>(ref TBuffer buffer, Int32 spanLength)
		=> MemoryMarshalCompat.CreateUnsafeSpan<T>(Unsafe.AsPointer(ref buffer), spanLength);
	/// <summary>
	/// Clears two elements from reference buffer.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <param name="buffer">A managed <typeparamref name="TBuffer"/> reference.</param>
	/// <param name="spanLength">Required span length.</param>
	[MethodImpl(MethodImplOptions.NoInlining)]
	private static void Clear<T, TBuffer>(ref TBuffer buffer, Int32 spanLength) where TBuffer : struct
	{
		ref T r0 = ref Unsafe.As<TBuffer, T>(ref buffer);
		ref T r = ref Unsafe.Add(ref Unsafe.As<TBuffer, T>(ref buffer), spanLength - 1);
		r0 = default!; // First element.
		r = default!; // Last element.
	}
#pragma warning restore CS8500
}
#endif