namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a context from a read-only block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the read-only fixed memory block.</typeparam>
public readonly unsafe ref struct ReadOnlyFixedContextValue<T>
#if NET9_0_OR_GREATER
	: IFixedPointerOperators<ReadOnlyFixedContextValue<T>>,
#if !OBSOLETE_FIXED_INTERFACES
		IReadOnlyFixedContext<T>
#else
#pragma warning disable CS0612
		IObsoleteReadOnlyFixedContext<T>
#pragma warning restore CS0612
#endif
#endif
{
#pragma warning disable CS8500
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly FixedPointerValue _value;

	/// <inheritdoc cref="IFixedPointer.Pointer"/>
	public IntPtr Pointer => this._value.Pointer;
	/// <inheritdoc cref="IReadOnlyFixedMemory{T}.ValuePointer"/>
	public ReadOnlyValPtr<T> ValuePointer => (ReadOnlyValPtr<T>)this._value.Pointer;
	/// <inheritdoc cref="IReadOnlyFixedMemory{T}.Values"/>
	public ReadOnlySpan<T> Values
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			this._value.ValidateOperation();
			return field;
		}
	}
	/// <inheritdoc cref="IReadOnlyFixedMemory.IsNullOrEmpty"/>
	public Boolean IsNullOrEmpty => this._value.IsNullOrEmpty;
	/// <inheritdoc cref="IReadOnlyFixedMemory.Bytes"/>
	public ReadOnlySpan<Byte> Bytes
	{
		get
		{
			if (!this._value.IsUnmanaged || this._value.Type is { IsValueType: false, }) return default;
			ref Byte refByte = ref Unsafe.As<T, Byte>(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshal.CreateReadOnlySpan(ref refByte, this._value.Size);
		}
	}
	/// <inheritdoc cref="IReadOnlyFixedMemory.Objects"/>
	public ReadOnlySpan<Object> Objects
	{
		get
		{
			if (this._value.IsUnmanaged || this._value.Type is not { IsValueType: true, }) return default;
			ref Object refObject = ref Unsafe.As<T, Object>(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshal.CreateReadOnlySpan(ref refObject, this._value.Size / sizeof(T));
		}
	}

	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="ptr">Unmanaged fixed pointer.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	internal ReadOnlyFixedContextValue(void* ptr, Int32 count)
	{
		if (ptr == default) return;
		this._value = new((IntPtr)ptr, count * sizeof(T))
		{
			IsReadOnly = true, IsUnmanaged = RuntimeHelpers.IsReferenceOrContainsReferences<T>(), Type = typeof(T),
		};
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<T>(ptr), count);
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="handle">A <see cref="MemoryHandle"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="isReadOnly">Indicates whether the memory block is read-only.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal ReadOnlyFixedContextValue(MemoryHandle handle, Int32 count, Boolean isReadOnly, out IDisposable disposable)
	{
		if (handle.Pointer == default)
		{
#pragma warning disable CS0612
			disposable = ReadOnlyFixedContext<T>.EmptyDisposable;
#pragma warning restore CS0612
			return;
		}
		this._value = new((IntPtr)handle.Pointer, count * sizeof(T))
		{
			IsReadOnly = isReadOnly,
			IsUnmanaged = RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = new(handle),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
		this.Values = MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(handle.Pointer), count);
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal ReadOnlyFixedContextValue(ReadOnlyValPtr<T> valPtr, Int32 count, out IDisposable disposable)
	{
		if (valPtr.IsZero)
		{
#pragma warning disable CS0612
			disposable = ReadOnlyFixedContext<T>.EmptyDisposable;
#pragma warning restore CS0612
			return;
		}
		this._value = new(valPtr.Pointer, count * sizeof(T))
		{
			IsReadOnly = true,
			IsUnmanaged = RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = new(),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if !NET8_0_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in valPtr.Reference), count);
#else
		this.Values = MemoryMarshal.CreateReadOnlySpan(in valPtr.Reference, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="value">Internal value.</param>
	internal ReadOnlyFixedContextValue(FixedPointerValue value)
	{
		if (value.IsNullOrEmpty) return;
		ref T refT = ref Unsafe.AsRef<T>(value.Pointer.ToPointer());
		this._value = value;
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref refT, value.Size / sizeof(T));
	}

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ReadOnlyValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
	private ReadOnlyFixedContextValue(ReadOnlyValPtr<T> valPtr, Int32 count, FixedValueHandle handle)
	{
		this._value = new(valPtr.Pointer, count * sizeof(T))
		{
			IsReadOnly = true,
			IsUnmanaged = RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = handle,
			Type = typeof(T),
		};
#if !NET8_0_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in valPtr.Reference), count);
#else
		this.Values = MemoryMarshal.CreateReadOnlySpan(in valPtr.Reference, count);
#endif
	}

#if NET9_0_OR_GREATER
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext()
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(Byte), true);
		if (this.IsNullOrEmpty) return ReadOnlyFixedContext<Byte>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		return new ReadOnlyFixedContext<Byte>(this._value.Pointer.ToPointer(), this._value.Size, handle);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext()
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(Object), false);
		if (this.IsNullOrEmpty) return ReadOnlyFixedContext<Object>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 count = this._value.Size / IntPtr.Size;
		return new ReadOnlyFixedContext<Object>(this._value.Pointer.ToPointer(), count, handle);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<TDestination> IReadOnlyFixedContext<T>.Transformation<TDestination>(
		out IReadOnlyFixedMemory residual)
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		if (this.IsNullOrEmpty)
		{
			residual = ReadOnlyFixedContext<Byte>.Empty;
			return ReadOnlyFixedContext<TDestination>.Empty;
		}
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this._value.Size / sizeof(T);
		Int32 offset = count * sizeOf;
		residual = offset == 0 ?
			ReadOnlyFixedContext<Byte>.Empty :
			new((this._value.Pointer + offset).ToPointer(), this._value.Size - offset, handle);
		return new ReadOnlyFixedContext<TDestination>(this._value.Pointer.ToPointer(), count, handle);
	}
#endif

	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual read-only memory from the transformation.</param>
	/// <returns>Reinterpreted <typeparamref name="TDestination"/> memory block.</returns>
	public ReadOnlyFixedContextValue<TDestination> Transformation<TDestination>(
		out ReadOnlyFixedContextValue<Byte> residual)
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this._value.Size / sizeof(T);
		Int32 offset = count * sizeOf;
		residual = offset != 0 ? new(this._value.CreateOffset(offset)) : default;
		return new(this._value);
	}
#pragma warning restore CS8500

	/// <summary>
	/// Defines an implicit conversion of a <see cref="ReadOnlyFixedContextValue{T}"/> to a <see cref="FixedPointerValue"/>
	/// instance.
	/// </summary>
	/// <param name="value">A pointer to implicitly convert.</param>
	public static implicit operator FixedPointerValue(ReadOnlyFixedContextValue<T> value) => value._value;
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="ReadOnlyFixedContextValue{T}"/> to a
	/// <see cref="FixedContextValue{Object}"/> instance.
	/// </summary>
	/// <param name="value">An <see cref="ReadOnlyFixedContextValue{T}"/> to explicitly convert.</param>
	public static explicit operator FixedContextValue<T>(ReadOnlyFixedContextValue<T> value)
	{
		ValidationUtilities.ThrowIfReadOnlyPointer(false, value._value.IsReadOnly);
		return new(value);
	}
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="FixedPointerValue"/> to a
	/// <see cref="ReadOnlyFixedContextValue{T}"/>
	/// instance.
	/// </summary>
	/// <param name="value">An <see cref="FixedPointerValue"/> to explicitly convert.</param>
	public static explicit operator ReadOnlyFixedContextValue<T>(FixedPointerValue value)
	{
		ValidationUtilities.ThrowIfReadOnlyPointer(false, value.IsReadOnly);
		value.ValidateTransformation(typeof(T), !RuntimeHelpers.IsReferenceOrContainsReferences<T>());
		return new(value);
	}

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="ReadOnlyFixedContextValue{T}"/> instance from
	/// current read-only reference pointer.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="ptr">Current <see cref="ReadOnlyValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="ReadOnlyFixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// This object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static IDisposable CreateDisposable<TDisposable>(ReadOnlyValPtr<T> ptr, Int32 count,
		TDisposable disposable, out ReadOnlyFixedContextValue<T> fixedContext) where TDisposable : IDisposable
	{
		if (ptr.IsZero)
		{
			fixedContext = default;
#pragma warning disable CS0612
			return ReadOnlyFixedContext<T>.EmptyDisposable;
#pragma warning restore CS0612
		}
		FixedValueHandle result = FixedValueHandle.CreateFromDisposable(disposable);
		fixedContext = new(ptr, count, result);
		return result;
	}
}