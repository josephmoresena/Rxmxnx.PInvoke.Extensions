#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe ref partial struct ReadOnlyFixedContextValue<T>
{
#pragma warning disable CS8500
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="ptr">Unmanaged fixed pointer.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	internal ReadOnlyFixedContextValue(void* ptr, Int32 count)
	{
		if (ptr == default) return;
		this._value = new((IntPtr)ptr, count * sizeof(T))
		{
			IsReadOnly = true, IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(), Type = typeof(T),
		};
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<T>(ptr), count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(ptr, count) :
			MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(ptr, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="handle">A <see cref="MemoryHandle"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal ReadOnlyFixedContextValue(MemoryHandle handle, Int32 count, Boolean isReadOnly, out IDisposable disposable)
	{
		if (handle.Pointer == default)
		{
			disposable = FixedValueHandle.EmptyDisposable;
			return;
		}
		this._value = new((IntPtr)handle.Pointer, count * sizeof(T))
		{
			IsReadOnly = isReadOnly,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = new FixedValueHandle.Memory(handle),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<T>(handle.Pointer), count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(handle.Pointer, count) :
			MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(handle.Pointer, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal ReadOnlyFixedContextValue(ReadOnlyValPtr<T> valPtr, Int32 count, out IDisposable disposable)
	{
		if (valPtr.IsZero)
		{
			disposable = FixedValueHandle.EmptyDisposable;
			return;
		}
		this._value = new(valPtr.Pointer, count * sizeof(T))
		{
			IsReadOnly = true,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = new(),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		this.Values = typeof(T).IsPrimitive ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(valPtr.Pointer.ToPointer(), count);
#elif !NET8_0_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in valPtr.Reference), count);
#else
		this.Values = MemoryMarshal.CreateReadOnlySpan(in valPtr.Reference, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="value">Internal value.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	internal ReadOnlyFixedContextValue(FixedPointerValue value)
	{
		this._value = value;
		Int32 count = value.Size / sizeof(T);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T refT = ref Unsafe.AsRef<T>(value.Pointer.ToPointer());
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref refT, count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(value.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(value.Pointer.ToPointer(), count);
#endif
	}

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private ReadOnlyFixedContextValue(ReadOnlyValPtr<T> valPtr, Int32 count, FixedValueHandle handle)
	{
		this._value = new(valPtr.Pointer, count * sizeof(T))
		{
			IsReadOnly = true,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = handle,
			Type = typeof(T),
		};
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		this.Values = typeof(T).IsPrimitive ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeReadOnlySpan<T>(valPtr.Pointer.ToPointer(), count);
#elif !NET8_0_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in valPtr.Reference), count);
#else
		this.Values = MemoryMarshal.CreateReadOnlySpan(in valPtr.Reference, count);
#endif
	}
#pragma warning restore CS8500
}