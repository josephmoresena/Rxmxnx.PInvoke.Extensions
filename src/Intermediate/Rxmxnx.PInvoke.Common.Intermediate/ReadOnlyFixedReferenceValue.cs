#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER && !UAP10_0_16299
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a read-only fixed reference to a <typeparamref name="T"/> value.
/// </summary>
/// <typeparam name="T">Type of the referenced value.</typeparam>
[Preserve(AllMembers = true)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct ReadOnlyFixedReferenceValue<T>
#if NET9_0_OR_GREATER
	where T : allows ref struct
#endif
{
#pragma warning disable CS8500
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly FixedPointerValue _value;

	/// <inheritdoc cref="IFixedPointer.Pointer"/>
	public IntPtr Pointer
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get => this._value.Pointer;
	}
	/// <summary>
	/// Gets the value pointer to the fixed reference.
	/// </summary>
	public ReadOnlyValPtr<T> ValuePointer
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get => (ReadOnlyValPtr<T>)this._value.Pointer;
	}
	/// <summary>
	/// Gets a read-only reference to the fixed value.
	/// </summary>
	public ref readonly T Reference
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get
		{
			this._value.ValidateOperation(true);
			return ref Unsafe.AsRef<T>(this._value.Pointer.ToPointer());
		}
	}
	/// <summary>
	/// Indicates whether the current reference is null.
	/// </summary>
	public Boolean IsNullOrEmpty => this._value.IsNullOrEmpty;
	/// <summary>
	/// Gets a read-only binary span over the fixed reference.
	/// </summary>
	public ReadOnlySpan<Byte> Bytes
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get
		{
			if (!this._value.IsUnmanaged || this._value.Type is { IsValueType: false, }) return default;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			ref Byte refByte = ref Unsafe.As<T, Byte>(ref Unsafe.AsRef(in this.Reference));
			return MemoryMarshal.CreateReadOnlySpan(ref refByte, this._value.Size);
#else
			void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in this.Reference));
			return new(ptr, this._value.Size);
#endif
		}
	}
	/// <summary>
	/// Gets a read-only object span over the fixed reference.
	/// </summary>
	public ReadOnlySpan<Object> Objects
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get
		{
			if (this._value.IsUnmanaged || this._value.Type is not { IsValueType: false, }) return default;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
			ref Object refObject = ref Unsafe.As<T, Object>(ref Unsafe.AsRef(in this.Reference));
			return MemoryMarshal.CreateReadOnlySpan(ref refObject, this._value.Size / sizeof(IntPtr));
#else
			void* ptr = Unsafe.AsPointer(ref Unsafe.AsRef(in this.Reference));
			return MemoryMarshalCompat.CreateUnsafeReadOnlySpan<Object>(ptr, this._value.Size / sizeof(IntPtr));
#endif
		}
	}

	/// <summary>
	/// Initializes a read-only fixed reference from a pinned address. No handle is allocated.
	/// </summary>
	/// <param name="ptr">Pinned address of the value.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	internal ReadOnlyFixedReferenceValue(void* ptr)
	{
		if (ptr == default) return;
		this._value = new((IntPtr)ptr, sizeof(T))
		{
			IsReadOnly = true, IsUnmanaged = !RuntimeHelpers.IsReferenceOrContainsReferences<T>(), Type = typeof(T),
		};
	}
	/// <summary>
	/// Initializes a read-only fixed reference from an existing <see cref="FixedPointerValue"/>.
	/// </summary>
	/// <param name="value">Internal value.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	internal ReadOnlyFixedReferenceValue(FixedPointerValue value) => this._value = value;

	/// <summary>
	/// Reinterprets the fixed reference as a read-only <typeparamref name="TDestination"/> reference.
	/// </summary>
	/// <typeparam name="TDestination">Type of the reinterpreted reference.</typeparam>
	/// <param name="residual">Output. Residual fixed pointer from the transformation.</param>
	/// <returns>The reinterpreted read-only fixed reference.</returns>
	public ReadOnlyFixedReferenceValue<TDestination> Transformation<TDestination>(out FixedPointerValue residual)
#if NET9_0_OR_GREATER
		where TDestination : allows ref struct
#endif
	{
		this._value.ValidateOperation(true);
		this._value.ValidateTransformation(typeof(TDestination),
		                                   !RuntimeHelpers.IsReferenceOrContainsReferences<TDestination>());
		Int32 sizeOf = sizeof(TDestination);
		this._value.ValidateReferenceSize(typeof(TDestination), sizeOf);
		residual = this._value.CreateOffset(sizeOf);
		return new(this._value);
	}

	/// <summary>
	/// Defines an implicit conversion of a <see cref="ReadOnlyFixedReferenceValue{T}"/> to a
	/// <see cref="FixedPointerValue"/> instance.
	/// </summary>
	/// <param name="value">A reference to implicitly convert.</param>
	public static implicit operator FixedPointerValue(ReadOnlyFixedReferenceValue<T> value) => value._value;
	/// <summary>
	/// Defines an explicit conversion of a given <see cref="FixedPointerValue"/> to a
	/// <see cref="ReadOnlyFixedReferenceValue{T}"/> instance.
	/// </summary>
	/// <param name="value">A <see cref="FixedPointerValue"/> to explicitly convert.</param>
	public static explicit operator ReadOnlyFixedReferenceValue<T>(FixedPointerValue value)
	{
		value.ValidateOperation(true);
		value.ValidateTransformation(typeof(T), !RuntimeHelpers.IsReferenceOrContainsReferences<T>());
		value.ValidateReferenceSize(typeof(T), sizeof(T));
		return new(value);
	}
#pragma warning restore CS8500
}