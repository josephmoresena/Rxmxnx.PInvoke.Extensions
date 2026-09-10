namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of utilities for exchange data within the P/Invoke context.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe partial class NativeUtilities
{
	/// <summary>
	/// Size in bytes of a memory pointer.
	/// </summary>
	public static readonly Int32 PointerSize = sizeof(IntPtr);

	/// <summary>
	/// Gets the memory size of <typeparamref name="T"/> structure.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
	/// <returns>Size of <typeparamref name="T"/> structure.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 SizeOf<T>() where T : unmanaged => sizeof(T);

	/// <summary>
	/// Creates an <see cref="FuncPtr{TDelegate}"/> from a memory reference to a <typeparamref name="TDelegate"/> delegate
	/// instance.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the <see cref="Delegate"/> to be referenced by the pointer.</typeparam>
	/// <param name="delegateInstance">Instance of the <typeparamref name="TDelegate"/> delegate.</param>
	/// <returns>An <see cref="FuncPtr{TDelegate}"/> pointer.</returns>
	/// <remarks>
	/// The pointer will point to the address in memory where the delegate instance was located at the moment this method was
	/// called.
	/// To ensure that the pointer remains valid, the delegate instance must be kept alive and not allowed to be collected by
	/// the GC.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static FuncPtr<TDelegate> GetUnsafeFuncPtr<TDelegate>(TDelegate delegateInstance) where TDelegate : Delegate
		=> (FuncPtr<TDelegate>)Marshal.GetFunctionPointerForDelegate(delegateInstance);
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="ReadOnlyValPtr{T}"/> pointer from a read-only reference to a
	/// <typeparamref name="T"/> value.
	/// </summary>
	/// <typeparam name="T">The type of the managed reference.</typeparam>
	/// <param name="value">A read-only reference to a <typeparamref name="T"/> value.</param>
	/// <returns><see cref="ReadOnlyValPtr{T}"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ReadOnlyValPtr<T> GetUnsafeValPtr<T>(in T value)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		ref T refValue = ref Unsafe.AsRef(in value);
		return new(Unsafe.AsPointer(ref refValue));
	}
	/// <summary>
	/// Retrieves an unsafe pointer of type <see cref="ValPtr{T}"/> from a reference to a value of type
	/// <typeparamref name="T"/>.
	/// </summary>
	/// <typeparam name="T">The type of the managed reference.</typeparam>
	/// <param name="refValue">The reference to the value from which to retrieve the pointer.</param>
	/// <returns>An unsafe pointer of type <see cref="ValPtr{T}"/> pointing to the referenced value.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ValPtr<T> GetUnsafeValPtrFromRef<T>(ref T refValue)
#if NET9_0_OR_GREATER
		where T : allows ref struct
#endif
	{
		void* ptr = Unsafe.AsPointer(ref refValue);
		return new(ptr);
	}

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="IntPtr"/> pointer from a read-only reference to a
	/// <typeparamref name="T"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">A read-only reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
	/// <returns><see cref="IntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IntPtr GetUnsafeIntPtr<T>(in T value) where T : unmanaged
	{
		ref T refValue = ref Unsafe.AsRef(in value);
		return (IntPtr)Unsafe.AsPointer(ref refValue);
	}
	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="UIntPtr"/> pointer from a read-only reference to a
	/// <typeparamref name="T"/> <see langword="unmanaged"/> value.
	/// </summary>
	/// <typeparam name="T"><see cref="ValueType"/> of the referenced <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">Read-only reference to a <typeparamref name="T"/> <see langword="unmanaged"/> value.</param>
	/// <returns><see cref="UIntPtr"/> pointer.</returns>
	/// <remarks>
	/// The pointer obtained is "unsafe" as it doesn't guarantee that the referenced value
	/// won't be moved or collected by the garbage collector.
	/// The pointer will point to the address in memory the reference had at the moment this method was called.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static UIntPtr GetUnsafeUIntPtr<T>(in T value) where T : unmanaged
	{
		ref T refValue = ref Unsafe.AsRef(in value);
		return (UIntPtr)Unsafe.AsPointer(ref refValue);
	}
	/// <summary>
	/// Transforms a read-only reference of an <see langword="unmanaged"/> value of type <typeparamref name="TSource"/> into a
	/// read-only reference of an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the source value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TDestination">
	/// The type of the destination value to which to create a reference. This must be an <see langword="unmanaged"/> value
	/// type.
	/// </typeparam>
	/// <param name="value">
	/// The read-only reference to the source value from which to create the destination reference.
	/// </param>
	/// <returns>A read-only reference to an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> do not have the same memory size.
	/// </exception>
	/// <remarks>
	/// The transformation occurs at the memory level, without copying or moving data.
	/// This transformation can be performed between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>
	/// types
	/// that have the same size in memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref readonly TDestination Transform<TSource, TDestination>(in TSource value)
		where TSource : unmanaged where TDestination : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidCastType(sizeof(TDestination), sizeof(TSource));
		ref TSource refValue = ref Unsafe.AsRef(in value);
		return ref Unsafe.As<TSource, TDestination>(ref refValue);
	}
	/// <summary>
	/// Transforms a reference of an <see langword="unmanaged"/> value of type <typeparamref name="TSource"/> into a
	/// reference of an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.
	/// </summary>
	/// <typeparam name="TSource">
	/// The type of the source value being referenced. This must be an <see langword="unmanaged"/> value type.
	/// </typeparam>
	/// <typeparam name="TDestination">
	/// The type of the destination value to which to create a reference. This must be an <see langword="unmanaged"/> value
	/// type.
	/// </typeparam>
	/// <param name="refValue">
	/// The reference to the source value from which to create the destination reference.
	/// </param>
	/// <returns>A reference to an <see langword="unmanaged"/> value of type <typeparamref name="TDestination"/>.</returns>
	/// <exception cref="InvalidOperationException">
	/// Thrown when <typeparamref name="TSource"/> and <typeparamref name="TDestination"/> do not have the same memory size.
	/// </exception>
	/// <remarks>
	/// The transformation occurs at the memory level, without copying or moving data.
	/// This transformation can be performed between <typeparamref name="TSource"/> and <typeparamref name="TDestination"/>
	/// types
	/// that have the same size in memory.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static ref TDestination TransformReference<TSource, TDestination>(ref TSource refValue)
		where TSource : unmanaged where TDestination : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidCastType(sizeof(TDestination), sizeof(TSource));
		return ref Unsafe.As<TSource, TDestination>(ref refValue);
	}
	/// <summary>
	/// Retrieves a <see cref="Byte"/> array from a read-only reference to a <typeparamref name="TSource"/> value.
	/// </summary>
	/// <typeparam name="TSource"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value">A read-only reference to <typeparamref name="TSource"/> value.</param>
	/// <returns><see cref="Byte"/> array.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Byte[] ToBytes<TSource>(in TSource value) where TSource : unmanaged
	{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref TSource refValue = ref Unsafe.AsRef(in value);
		ReadOnlySpan<TSource> intermediateSpan = MemoryMarshal.CreateReadOnlySpan(ref refValue, 1);
		ReadOnlySpan<Byte> bytes = MemoryMarshal.AsBytes(intermediateSpan);
#if !NET5_0_OR_GREATER
		Byte[] result = new Byte[bytes.Length];
#else
		Byte[] result = GC.AllocateUninitializedArray<Byte>(bytes.Length);
#endif
		bytes.CopyTo(result);
		return result;
#else
		fixed (TSource* valuePtr = &value)
			return new ReadOnlySpan<Byte>(valuePtr, sizeof(TSource)).ToArray();
#endif
	}
	/// <summary>
	/// Performs a binary copy of the given <typeparamref name="TSource"/> to the <paramref name="destination"/> span.
	/// </summary>
	/// <typeparam name="TSource"><see cref="ValueType"/> of <see langword="unmanaged"/> value.</typeparam>
	/// <param name="value"><typeparamref name="TSource"/> value.</param>
	/// <param name="destination">Destination <see cref="Span{T}"/> instance.</param>
	/// <param name="offset">
	/// The offset in <paramref name="destination"/> at which <paramref name="value"/> will be copied.
	/// </param>
	/// <exception cref="ArgumentException">
	/// Throws an exception when the length of <paramref name="destination"/> span minus the offset is less
	/// than the size of <typeparamref name="TSource"/>.
	/// </exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void CopyBytes<TSource>(in TSource value, Span<Byte> destination, Int32 offset = 0)
		where TSource : unmanaged
	{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ValidationUtilities.ThrowIfInvalidCopyType(value, destination, offset, out ReadOnlySpan<Byte> bytes);
		bytes.CopyTo(destination[offset..]);
#else
		fixed (TSource* valuePtr = &value)
		{
			ValidationUtilities.ThrowIfInvalidCopyType(valuePtr, destination, offset, out ReadOnlySpan<Byte> bytes);
			bytes.CopyTo(destination[offset..]);
		}
#endif
	}
	/// <summary>
	/// Creates a new span over an array of the values of the constants in a specified enumeration type.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>A read-only span that contains the values of the constants in <typeparamref name="TEnum"/>.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static ReadOnlySpan<TEnum> GetEnumValuesSpan<TEnum>() where TEnum : struct, Enum
		=> EnumValueHelper<TEnum>.Values.Span;
	/// <summary>
	/// Creates a new span over an array of the names of the constants in a specified enumeration type.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <returns>The span representation of the array.</returns>
	/// <returns>A string read-only span of the names of the constants in <typeparamref name="TEnum"/>.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static ReadOnlySpan<String> GetEnumNamesSpan<TEnum>() where TEnum : struct, Enum
		=> EnumNameHelper<TEnum>.Values.Span;
	/// <summary>
	/// Creates a <see cref="ReadOnlyFixedContextValue{TEnum}"/> instance by pinning an array of the values of  the
	/// constants in a specified enumeration type.
	/// </summary>
	/// <typeparam name="TEnum">The type of the enumeration.</typeparam>
	/// <param name="fixedContext">
	/// Output. The <see cref="ReadOnlyFixedContextValue{T}"/> instance representing the pinned memory.
	/// </param>
	/// <returns>An <see cref="IDisposable"/> instance representing the pinned memory releasing.</returns>
	/// <remarks>
	/// The output context owns the pinned memory and releases it when the returning object is disposed.
	/// Consumers should use a <see langword="using"/> statement or otherwise dispose the returned object.
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static IDisposable GetValuesFixedContext<TEnum>(out ReadOnlyFixedContextValue<TEnum> fixedContext)
		where TEnum : unmanaged, Enum
	{
		ReadOnlyMemory<TEnum> mem = EnumValueHelper<TEnum>.Values;
		MemoryHandle handle = mem.Pin();
		fixedContext = new(handle, mem.Length, true, out IDisposable result);
		return result;
	}

	/// <summary>
	/// Creates an <see cref="IFixedMethod{TDelegate}.IDisposable"/> instance by marshalling the current
	/// <typeparamref name="TDelegate"/> instance, ensuring a safe interop context.
	/// </summary>
	/// <typeparam name="TDelegate">Type of the method delegate which is being fixed.</typeparam>
	/// <param name="method">Delegate of the method to be fixed.</param>
	/// <returns>An <see cref="IFixedMethod{TDelegate}.IDisposable"/> instance representing the marshalled method.</returns>
	/// <remarks>
	/// This method marshalls and protect the managed delegate to prevent the garbage collector from moving it.
	/// Ensure that the <see cref="IDisposable"/> object returned is properly disposed to release the managed delegate
	/// and avoid memory leaks.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static IFixedMethod<TDelegate>.IDisposable GetFixedMethod<TDelegate>(TDelegate? method)
		where TDelegate : Delegate
		=> method is null ?
			FixedDelegate<TDelegate>.EmptyDisposable :
			new FixedDelegate<TDelegate>(method).ToDisposable(default);

	/// <summary>
	/// Allocates a native memory block for <paramref name="count"/> values of type <typeparamref name="T"/> and exposes
	/// it through a <see cref="FixedContextValue{T}"/> instance.
	/// </summary>
	/// <typeparam name="T">The unmanaged value type stored in the allocated memory block.</typeparam>
	/// <param name="count">The number of values of type <typeparamref name="T"/> to allocate.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the pinned memory.
	/// </param>
	/// <returns>An <see cref="IDisposable"/> instance representing the allocated memory releasing.</returns>
	/// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="count"/> is negative.</exception>
	/// <exception cref="OverflowException">
	/// Thrown when the requested allocation size exceeds <see cref="Int32.MaxValue"/>.
	/// </exception>
	/// <remarks>
	/// The output context owns the native memory allocation and releases it when the returning object is disposed.
	/// The allocated memory is not initialized.
	/// Consumers should use a <see langword="using"/> statement or otherwise dispose the returned object.
	/// </remarks>
	public static IDisposable HeapAlloc<T>(Int32 count, out FixedContextValue<T> fixedContext) where T : unmanaged
	{
		ValidationUtilities.ThrowIfInvalidLength(count);
		return NativeMemoryOwner.CreateContext(count, out fixedContext);
	}
}