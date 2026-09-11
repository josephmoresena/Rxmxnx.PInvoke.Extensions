#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a context from a read-only block of fixed memory.
/// </summary>
/// <typeparam name="T">Type of objects in the read-only fixed memory block.</typeparam>
[Preserve(AllMembers = true)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe ref partial struct ReadOnlyFixedContextValue<T>
{
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
	/// Gets the value pointer to the read-only fixed block of memory.
	/// </summary>
	public ReadOnlyValPtr<T> ValuePointer
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		get => (ReadOnlyValPtr<T>)this._value.Pointer;
	}
	/// <summary>
	/// Gets a read-only <typeparamref name="T"/> span over the fixed block of memory.
	/// </summary>
	public ReadOnlySpan<T> Values
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			this._value.ValidateOperation(true);
			// ReSharper disable once PropertyFieldKeywordIsNeverAssigned
			return field;
		}
	}
	/// <summary>
	/// Indicates whether the current memory block is null-referenced or empty.
	/// </summary>
	public Boolean IsNullOrEmpty => this._value.IsNullOrEmpty;
	/// <summary>
	/// Gets a read-only binary span over the fixed block of memory.
	/// </summary>
	public ReadOnlySpan<Byte> Bytes
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
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
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
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
#pragma warning disable CS8500
		Int32 sizeOf = sizeof(TDestination);
#pragma warning restore CS8500
		Int32 count = this._value.Size / sizeOf;
		Int32 offset = count * sizeOf;
		residual = this._value.CreateOffset(offset);
		return new(this._value);
	}
}