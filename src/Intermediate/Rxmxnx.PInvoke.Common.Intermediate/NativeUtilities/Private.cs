namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of utilities for exchange data within the P/Invoke context.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#if NETSTANDARD2_1 || NETCOREAPP
public unsafe partial class NativeUtilities
#else
public partial class NativeUtilities
#endif
{
	/// <summary>
	/// Cache for <see cref="GlobalizationInvariantModeEnabled"/>
	/// </summary>
	private static Boolean? globalizationInvariantMode;

	/// <summary>
	/// Checks if globalization-invariant mode is enabled.
	/// </summary>
	/// <returns>
	/// <see langword="true"/> if globalization-invariant mode is enabled; otherwise,
	/// <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private static Boolean IsGlobalizationInvariantMode()
	{
		try
		{
			return CultureInfo.GetCultureInfo(0x409).LCID == 0x1000;
		}
		catch (CultureNotFoundException)
		{
			CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.NeutralCultures);
			return cultures.Length <= 1;
		}
	}
	/// <summary>
	/// Retrieves the Iso639-1 language code enum value corresponding to the current user interface culture.
	/// </summary>
	/// <returns><see cref="Iso639P1"/> code integer value.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Int32 GetUserInterfaceTwoLetterLangCode()
		=> (Int32)NativeUtilities.GetIso639P1(CultureInfo.CurrentUICulture);
#if NETSTANDARD2_1 || NETCOREAPP
	/// <summary>
	/// Writes <paramref name="span"/> using <paramref name="arg"/> and <paramref name="action"/>.
	/// </summary>
	/// <typeparam name="T">Unmanaged type of elements in <paramref name="span"/>.</typeparam>
	/// <typeparam name="TArg">Type of state object.</typeparam>
	/// <param name="span">A <typeparamref name="T"/> writable memory block.</param>
	/// <param name="arg">A <typeparamref name="TArg"/> instance.</param>
	/// <param name="action">A <see cref="SpanAction{T, TState}"/> delegate.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static void WriteSpan<T, TArg>(Span<T> span, TArg arg, SpanAction<T, TArg> action)
#if NET9_0_OR_GREATER
		where TArg : allows ref struct
#endif
	{
#pragma warning disable CS8500
		fixed (void* _ = &MemoryMarshal.GetReference(span))
#pragma warning restore CS8500
			action(span, arg);
	}
	/// <inheritdoc cref="Delegate.GetInvocationList()"/>
	/// <returns>A read-only span of delegates in this delegate instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<Delegate> GetInvocationSpan(Delegate del)
	{
		Delegate[] array = del.GetInvocationList();
		return MemoryMarshal.CreateReadOnlySpan(ref NativeUtilities.GetArrayDataReference(array), array.Length);
	}
#pragma warning disable CS8500
#if !NET5_0_OR_GREATER
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <param name="typeRef">Output. Current type.</param>
	/// <param name="ctorRef">Output. Constructor memory delegate.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(void* ptr, ReadOnlySpan<T> span, out Type typeRef,
		out Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory> ctorRef)
	{
		typeRef = typeof(T);
		ctorRef = ReadOnlyFixedContext<T>.CreateInstance;
		return new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorOrFunctionPointer = Unsafe.AsPointer(ref ctorRef),
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
	/// <param name="ctorRef">Output. Constructor memory delegate.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(void* ptr, Span<T> span, out Type typeRef,
		out Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory> ctorRef)
	{
		typeRef = typeof(T);
		ctorRef = FixedContext<T>.CreateInstance;
		return new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorOrFunctionPointer = Unsafe.AsPointer(ref ctorRef),
			TypeOrFunctionPointer = Unsafe.AsPointer(ref typeRef),
		};
	}
#else
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(void* ptr, ReadOnlySpan<T> span)
		=> new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&ReadOnlyFixedContext<T>.CreateInstance),
			TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<T>),
		};
	/// <summary>
	/// Retrieves the <see cref="FixedPointerInfo"/> instance for given parameters.
	/// </summary>
	/// <typeparam name="T">Type of fixed memory block.</typeparam>
	/// <param name="ptr">Fixed unmanaged pointer.</param>
	/// <param name="span">A read-only <typeparamref name="T"/> span.</param>
	/// <returns>A <see cref="FixedPointerInfo"/> instance.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static FixedPointerInfo CreateFixedPointerInfo<T>(void* ptr, Span<T> span)
		=> new()
		{
			Pointer = ptr,
			Count = span.Length,
			SizeOf = sizeof(T),
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			ConstructorOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&FixedContext<T>.CreateInstance),
			TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<T>),
		};
#endif
	/// <summary>
	/// Creates a <see cref="ReadOnlyFixedMemory"/> span from <typeparamref name="TBuffer"/> reference.
	/// </summary>
	/// <typeparam name="TBuffer">A <see cref="ValueType"/> buffer type.</typeparam>
	/// <param name="buffer">Managed reference to <typeparamref name="TBuffer"/> value.</param>
	/// <returns>Created span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static Span<ReadOnlyFixedMemory?> CreateReadOnlyFixedMemorySpan<TBuffer>(ref TBuffer buffer)
#if !PACKAGE
		where TBuffer : struct
#else
		where TBuffer : struct, IManagedBinaryBuffer<Object>
#endif
	{
#if !PACKAGE
		Int32 length = sizeof(TBuffer) / IntPtr.Size;
#else
		Int32 length = buffer.Metadata.Size;
#endif
		ref ReadOnlyFixedMemory? r0 = ref Unsafe.As<TBuffer, ReadOnlyFixedMemory?>(ref buffer);
		return MemoryMarshal.CreateSpan(ref r0, length);
	}
#pragma warning restore CS8500
#endif
}