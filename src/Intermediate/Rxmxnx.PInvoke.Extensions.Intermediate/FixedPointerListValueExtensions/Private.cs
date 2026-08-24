#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe partial class FixedPointerListValueExtensions
{
#pragma warning disable CS8500
#if !NET5_0_OR_GREATER
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="typeRef">Output. Current type.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(this ReadOnlySpan<T> span, void* ptr, out Type typeRef)
	{
		typeRef = typeof(T);
		return new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
			ConstructorOrFunctionPointer = default,
#endif
			TypeOrFunctionPointer = Unsafe.AsPointer(ref typeRef),
		};
	}
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="typeRef">Output. Current type.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(this Span<T> span, void* ptr, out Type typeRef)
	{
		typeRef = typeof(T);
		return new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
			ConstructorOrFunctionPointer = default,
#endif
			TypeOrFunctionPointer = Unsafe.AsPointer(ref typeRef),
		};
	}
#else
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(this ReadOnlySpan<T> span, void* ptr)
		=> new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorOrFunctionPointer = default,
			TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<T>),
		};
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(this Span<T> span, void* ptr)
		=> new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorOrFunctionPointer = default,
			TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<T>),
		};
#endif
#pragma warning restore CS8500
}