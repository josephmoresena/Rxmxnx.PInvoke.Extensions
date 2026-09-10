#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct FixedContextValue<T>
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
	internal FixedContextValue(void* ptr, Int32 count)
	{
		if (ptr == default) return;
		this._value = new((IntPtr)ptr, count * sizeof(T))
		{
			IsReadOnly = false,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Type = typeof(T),
		};
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(ptr), count);
#else
		this.Values = typeof(T).IsPrimitive ? new(ptr, count) : MemoryMarshalCompat.CreateUnsafeSpan<T>(ptr, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="handle">A <see cref="MemoryHandle"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal FixedContextValue(MemoryHandle handle, Int32 count, out IDisposable disposable)
	{
		if (handle.Pointer == default)
		{
			disposable = FixedValueHandle.EmptyDisposable;
			return;
		}
		this._value = new((IntPtr)handle.Pointer, count * sizeof(T))
		{
			IsReadOnly = false,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = new FixedValueHandle.Memory(handle),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(handle.Pointer), count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(handle.Pointer, count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(handle.Pointer, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal FixedContextValue(ValPtr<T> valPtr, Int32 count, out IDisposable disposable)
	{
		if (valPtr.IsZero)
		{
			disposable = FixedValueHandle.EmptyDisposable;
			return;
		}
		this._value = new(valPtr.Pointer, count * sizeof(T))
		{
			IsReadOnly = false,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = new(),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateSpan(ref valPtr.Reference, count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(valPtr.Pointer.ToPointer(), count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="value">Internal value.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	internal FixedContextValue(FixedPointerValue value)
	{
		this._value = value;
		Int32 count = value.Size / sizeof(T);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T refT = ref Unsafe.AsRef<T>(value.Pointer.ToPointer());
		this.Values = MemoryMarshal.CreateSpan(ref refT, count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(value.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(value.Pointer.ToPointer(), count);
#endif
	}

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private FixedContextValue(ValPtr<T> valPtr, Int32 count, FixedValueHandle handle)
	{
		this._value = new(valPtr.Pointer, count * sizeof(T))
		{
			IsReadOnly = false,
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = handle,
			Type = typeof(T),
		};
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateSpan(ref valPtr.Reference, count);
#else
		this.Values = typeof(T).IsPrimitive ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(valPtr.Pointer.ToPointer(), count);
#endif
	}
#pragma warning restore CS8500
}