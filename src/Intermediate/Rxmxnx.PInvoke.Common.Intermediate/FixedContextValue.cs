#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a context from a block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the fixed memory block.</typeparam>
[Preserve(AllMembers = true)]
public readonly unsafe ref struct FixedContextValue<T>
#if NET9_0_OR_GREATER
	: IFixedPointerOperators<FixedContextValue<T>>,
#if !OBSOLETE_FIXED_INTERFACES
		IFixedContext<T>
#else
#pragma warning disable CS0612
		IObsoleteFixedContext<T>
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
	/// Gets the value pointer to the fixed block of memory.
	/// </summary>
	public ValPtr<T> ValuePointer => (ValPtr<T>)this._value.Pointer;
	/// <summary>
	/// Gets a <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	public Span<T> Values
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			this._value.ValidateOperation();
			return field;
		}
	}
	/// <summary>
	/// Indicates whether current memory block is null-referenced or empty.
	/// </summary>
	public Boolean IsNullOrEmpty => this._value.IsNullOrEmpty;
	/// <summary>
	/// Gets a binary span over the fixed block of memory.
	/// </summary>
	public Span<Byte> Bytes
	{
		get
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			if (!this._value.IsUnmanaged || this._value.Type is { IsValueType: false, }) return default;
			ref Byte refByte = ref Unsafe.As<T, Byte>(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshal.CreateSpan(ref refByte, this._value.Size);
#else
			if (!this._value.IsUnmanaged || this._value.Type?.GetTypeInfo() is { IsValueType: false, }) return default;
			void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(this.Values));
			return new(ptr, this._value.Size);
#endif
		}
	}
	/// <summary>
	/// Gets an object span over the fixed block of memory.
	/// </summary>
	public Span<Object> Objects
	{
		get
		{
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			if (this._value.IsUnmanaged || this._value.Type is not { IsValueType: false, }) return default;
			ref Object refObject = ref Unsafe.As<T, Object>(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshal.CreateSpan(ref refObject, this._value.Size / sizeof(IntPtr));
#else
			if (this._value.IsUnmanaged || this._value.Type?.GetTypeInfo() is not { IsValueType: false, })
				return default;
			void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(this.Values));
			return MemoryMarshalCompat.CreateUnsafeSpan<Object>(ptr, this._value.Size / sizeof(IntPtr));
#endif
		}
	}

	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="ptr">Unmanaged fixed pointer.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
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
		this.Values = this._value.IsUnmanaged ? new(ptr, count) : MemoryMarshalCompat.CreateUnsafeSpan<T>(ptr, count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="handle">A <see cref="MemoryHandle"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="disposable">Output. Disposable instance to release memory fixing.</param>
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
			Handle = new(handle),
			Type = typeof(T),
		};
		disposable = this._value.Handle;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		this.Values = MemoryMarshal.CreateSpan(ref Unsafe.AsRef<T>(handle.Pointer), count);
#else
		this.Values = this._value.IsUnmanaged ?
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
		this.Values = this._value.IsUnmanaged ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(valPtr.Pointer.ToPointer(), count);
#endif
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="value">Internal value.</param>
	internal FixedContextValue(FixedPointerValue value)
	{
		if (value.IsNullOrEmpty) return;
		this._value = value;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		ref T refT = ref Unsafe.AsRef<T>(value.Pointer.ToPointer());
		this.Values = MemoryMarshal.CreateSpan(ref refT, value.Size / sizeof(T));
#else
		this.Values = this._value.IsUnmanaged ?
			new(value.Pointer.ToPointer(), this._value.Size / sizeof(T)) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(value.Pointer.ToPointer(), this._value.Size / sizeof(T));
#endif
	}

	/// <summary>
	/// Private constructor.
	/// </summary>
	/// <param name="valPtr">A <see cref="ValPtr{T}"/> instance.</param>
	/// <param name="count">Count of <typeparamref name="T"/> items in the fixed memory block.</param>
	/// <param name="handle">A <see cref="FixedValueHandle"/> instance.</param>
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
		this.Values = MemoryMarshal.CreateSpan(ref valPtr.Reference, count);
#else
		this.Values = this._value.IsUnmanaged ?
			new(valPtr.Pointer.ToPointer(), count) :
			MemoryMarshalCompat.CreateUnsafeSpan<T>(valPtr.Pointer.ToPointer(), count);
#endif
	}
#if NET9_0_OR_GREATER
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<Byte> IReadOnlyFixedMemory.Bytes => this.Bytes;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<Object> IReadOnlyFixedMemory.Objects => this.Objects;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlySpan<T> IReadOnlyFixedMemory<T>.Values => this.Values;
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	ReadOnlyValPtr<T> IReadOnlyFixedMemory<T>.ValuePointer => this.ValuePointer;

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<Byte> IFixedMemory.AsBinaryContext()
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(Byte), true);
		if (this.IsNullOrEmpty) return FixedContext<Byte>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		return new FixedContext<Byte>(this._value.Pointer.ToPointer(), this._value.Size, handle);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Byte> IReadOnlyFixedMemory.AsBinaryContext() => IFixedContext<T>.AsBinaryContext(this);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<Object> IFixedMemory.AsObjectContext()
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(Object), true);
		if (this.IsNullOrEmpty) return FixedContext<Object>.Empty;
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 count = this._value.Size / IntPtr.Size;
		return new FixedContext<Object>(this._value.Pointer.ToPointer(), count, handle);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IReadOnlyFixedContext<Object> IReadOnlyFixedMemory.AsObjectContext() => IFixedContext<T>.AsObjectContext(this);
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IFixedMemory residual)
	{
		this._value.ValidateOperation();
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		if (this.IsNullOrEmpty)
		{
			residual = FixedContext<Byte>.Empty;
			return FixedContext<TDestination>.Empty;
		}
		FixedValueHandle handle = FixedPointerValue.GetValidationObject(this);
		Int32 sizeOf = sizeof(TDestination);
		Int32 count = this._value.Size / sizeof(T);
		Int32 offset = count * sizeOf;
		residual = offset == 0 ?
			FixedContext<Byte>.Empty :
			new((this._value.Pointer + offset).ToPointer(), this._value.Size - offset, handle);
		return new FixedContext<TDestination>(this._value.Pointer.ToPointer(), count, handle);
	}
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if OBSOLETE_FIXED_INTERFACES
	[Obsolete]
#endif
	IFixedContext<TDestination> IFixedContext<T>.Transformation<TDestination>(out IReadOnlyFixedMemory residual)
	{
		Unsafe.SkipInit(out residual);
		ref IFixedMemory refResidual = ref Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual);
		return IFixedContext<T>.Transformation<FixedContextValue<T>, TDestination>(this, out refResidual);
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
		Unsafe.SkipInit(out residual);
		ref IFixedMemory refResidual = ref Unsafe.As<IReadOnlyFixedMemory, IFixedMemory>(ref residual);
		return IFixedContext<T>.Transformation<FixedContextValue<T>, TDestination>(this, out refResidual);
	}
#endif

	/// <summary>
	/// Reinterprets the <typeparamref name="T"/> fixed memory block as a <typeparamref name="TDestination"/> memory block.
	/// </summary>
	/// <typeparam name="TDestination">Type of objects in the reinterpreted memory block.</typeparam>
	/// <param name="residual">Output. Residual fixed pointer from the transformation.</param>
	/// <returns>Reinterpreted <typeparamref name="TDestination"/> memory block.</returns>
	public FixedContextValue<TDestination> Transformation<TDestination>(out FixedPointerValue residual)
	{
		this._value.ValidateOperation();
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
	/// Defines an implicit conversion of a <see cref="FixedContextValue{T}"/> to a <see cref="FixedPointerValue"/>
	/// instance.
	/// </summary>
	/// <param name="value">A pointer to implicitly convert.</param>
	public static implicit operator FixedPointerValue(FixedContextValue<T> value) => value._value;
	/// <summary>
	/// Defines an implicit conversion of a <see cref="FixedContextValue{T}"/> to a <see cref="ReadOnlyFixedContextValue{T}"/>
	/// instance.
	/// </summary>
	/// <param name="value">A pointer to implicitly convert.</param>
	public static implicit operator ReadOnlyFixedContextValue<T>(FixedContextValue<T> value) => new(value._value);
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="FixedPointerValue"/> to a <see cref="FixedContextValue{T}"/>
	/// instance.
	/// </summary>
	/// <param name="value">An <see cref="FixedPointerValue"/> to explicitly convert.</param>
	public static explicit operator FixedContextValue<T>(FixedPointerValue value)
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
	public static FixedContextValue<T> CreateValue(IFixedMemory<T> instance)
	{
		if (FixedPointerValue.TryCreateFixedValue(instance, out FixedPointerValue value))
			return new(value);
		if (instance is not IDisposable dis)
			return new(instance.ValuePointer, instance.Values.Length);
		FixedContextValue<T>.CreateDisposable(instance.ValuePointer, instance.Values.Length, dis,
		                                      out FixedContextValue<T> result);
		return result;
	}
#endif

	/// <summary>
	/// Retrieves an <see langword="unsafe"/> <see cref="FixedContextValue{T}"/> instance from current reference pointer.
	/// </summary>
	/// <typeparam name="TDisposable">Type of <see cref="IDisposable"/> instance.</typeparam>
	/// <param name="ptr">Current <see cref="ValPtr{T}"/> value.</param>
	/// <param name="count">The number of items of type <typeparamref name="T"/> in the memory block.</param>
	/// <param name="disposable">Object to dispose in order to free <see langword="unmanaged"/> resources.</param>
	/// <param name="fixedContext">
	/// Output. The <see cref="FixedContextValue{T}"/> instance representing the fixed memory.
	/// </param>
	/// <returns>The <see cref="IDisposable"/> instance to release <see langword="unmanaged"/> resources.</returns>
	/// <remarks>
	/// The instance obtained is "unsafe" as it doesn't guarantee that the referenced values
	/// won't be moved or collected by garbage collector.
	/// The <paramref name="disposable"/> parameter allows for custom management of resource cleanup.
	/// This object will be disposed of when the fixed reference is disposed.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static IDisposable CreateDisposable<TDisposable>(ValPtr<T> ptr, Int32 count, TDisposable disposable,
		out FixedContextValue<T> fixedContext) where TDisposable : IDisposable
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