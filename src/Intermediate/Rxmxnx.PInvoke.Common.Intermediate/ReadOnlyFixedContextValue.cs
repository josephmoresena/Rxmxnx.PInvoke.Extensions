#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a context from a read-only block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the read-only fixed memory block.</typeparam>
[Preserve(AllMembers = true)]
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
	/// <summary>
	/// Gets the value pointer to the read-only fixed block of memory.
	/// </summary>
	public ReadOnlyValPtr<T> ValuePointer => (ReadOnlyValPtr<T>)this._value.Pointer;
	/// <summary>
	/// Gets a read-only <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	public ReadOnlySpan<T> Values
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			this._value.ValidateOperation(true);
			return field;
		}
	}
	/// <summary>
	/// Indicates whether current memory block is null-referenced or empty.
	/// </summary>
	public Boolean IsNullOrEmpty => this._value.IsNullOrEmpty;
	/// <summary>
	/// Gets a read-only binary span over the fixed block of memory.
	/// </summary>
	public ReadOnlySpan<Byte> Bytes
	{
		get
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			if (!this._value.IsUnmanaged || this._value.Type is { IsValueType: false, }) return default;
			ref Byte refByte = ref Unsafe.As<T, Byte>(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshal.CreateReadOnlySpan(ref refByte, this._value.Size);
#else
			if (!this._value.IsUnmanaged || this._value.Type?.GetTypeInfo() is { IsValueType: false, }) return default;
			void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(this.Values));
			return new(ptr, this._value.Size);
#endif
		}
	}
	/// <summary>
	/// Gets a read-only object span over the fixed block of memory.
	/// </summary>
	public ReadOnlySpan<Object> Objects
	{
		get
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			if (this._value.IsUnmanaged || this._value.Type is not { IsValueType: false, }) return default;
			ref Object refObject = ref Unsafe.As<T, Object>(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshal.CreateReadOnlySpan(ref refObject, this._value.Size / sizeof(IntPtr));
#else
			if (this._value.IsUnmanaged || this._value.Type?.GetTypeInfo() is not { IsValueType: false, })
				return default;
			void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshalCompat.CreateUnsafeReadOnlySpan<Object>(ptr, this._value.Size / sizeof(IntPtr));
#endif
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
			IsReadOnly = true, IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(), Type = typeof(T),
		};
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<T>(ptr), count);
#else
		this.Values = this._value.IsUnmanaged ?
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
			Handle = new(handle),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef<T>(handle.Pointer), count);
#else
		this.Values = this._value.IsUnmanaged ?
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
		this.Values = this._value.IsUnmanaged ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(valPtr.Pointer.ToPointer(), count);
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
	internal ReadOnlyFixedContextValue(FixedPointerValue value)
	{
		this._value = value;
		Int32 count = value.Size / sizeof(T);
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T refT = ref Unsafe.AsRef<T>(value.Pointer.ToPointer());
		this.Values = MemoryMarshal.CreateReadOnlySpan(ref refT, count);
#else
		this.Values = this._value.IsUnmanaged ?
			new(value.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(value.Pointer.ToPointer(), count);
#endif
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
			IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(),
			Handle = handle,
			Type = typeof(T),
		};
#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
		this.Values = this._value.IsUnmanaged ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(valPtr.Pointer.ToPointer(), count);
#elif !NET8_0_OR_GREATER
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
	/// <param name="residual">Output. Residual fixed pointer from the transformation.</param>
	/// <returns>Reinterpreted <typeparamref name="TDestination"/> memory block.</returns>
	public ReadOnlyFixedContextValue<TDestination> Transformation<TDestination>(out FixedPointerValue residual)
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this._value.Size / sizeOf;
		Int32 offset = count * sizeOf;
		residual = this._value.CreateOffset(offset);
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
		value._value.ValidateOperation();
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
		value.ValidateOperation();
		value.ValidateTransformation(typeof(T), !RuntimeHelpers.IsReferenceOrContainsReferences<T>());
		return new(value);
	}

#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
	/// <summary>
	/// Creates a new <see cref="FixedPointerValue"/> value from <paramref name="instance"/>.
	/// </summary>
	/// <param name="instance">A <see cref="IFixedPointer"/> instance.</param>
	/// <returns>
	/// A new <see cref="FixedContextValue{T}"/> instance.
	/// </returns>
	public static ReadOnlyFixedContextValue<T> CreateValue(IFixedMemory<T> instance)
	{
		if (FixedPointerValue.TryCreateFixedValue(instance, out FixedPointerValue value))
			return new(value);
		if (instance is not IDisposable dis)
			return new(instance.ValuePointer, instance.Values.Length);
		ReadOnlyFixedContextValue<T>.CreateDisposable(instance.ValuePointer, instance.Values.Length, dis,
		                                              out ReadOnlyFixedContextValue<T> result);
		return result;
	}
#endif

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
			return FixedValueHandle.EmptyDisposable;
		}
		FixedValueHandle result = FixedValueHandle.CreateFromDisposable(disposable);
		fixedContext = new(ptr, count, result);
		return result;
	}
}