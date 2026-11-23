#if !NETSTANDARD2_1 && !NETCOREAPP2_0_OR_GREATER && !UAP10_0_16299
using RuntimeHelpers = Rxmxnx.PInvoke.Internal.FrameworkCompat.RuntimeHelpersCompat;
#endif

namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a pointer to a fixed block of memory.
/// </summary>
#if !NET5_0_OR_GREATER
[Preserve(AllMembers = true)]
#endif
public readonly ref partial struct FixedPointerValue
{
	/// <summary>
	/// No-op instance for <see langword="null"/> or <see langword="unsafe"/> fixed instances.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static IDisposable UnsafeDisposable => FixedValueHandle.EmptyDisposable;

	/// <summary>
	/// Internal pointer.
	/// </summary>
	private readonly IntPtr _ptr;
	/// <summary>
	/// The offset of the memory.
	/// </summary>
	private readonly Int32 _offset;
	/// <summary>
	/// Internal byte count.
	/// </summary>
	private readonly Int32 _byteCount;

	/// <inheritdoc cref="IFixedPointer.Pointer"/>
	public IntPtr Pointer
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			this.ValidateOperation(true);
			return this._ptr + this._offset;
		}
	}

	/// <summary>
	/// Size of the memory block.
	/// </summary>
	internal Int32 Size => this._byteCount - this._offset;
	/// <summary>
	/// Indicates whether the current memory block is null-referenced or empty.
	/// </summary>
	internal Boolean IsNullOrEmpty => this._ptr == IntPtr.Zero || this.Size == 0;
	/// <summary>
	/// Current memory block handle.
	/// </summary>
	internal FixedValueHandle? Handle
	{
		get;
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		init;
	}
	/// <summary>
	/// The type of memory block.
	/// </summary>
	internal Type? Type
	{
		get;
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		init;
	}
	/// <summary>
	/// Indicates whether the current memory block is unmanaged.
	/// </summary>
	internal Boolean IsUnmanaged
	{
		get;
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		init;
	}
	/// <summary>
	/// Indicates whether the current instance is read-only.
	/// </summary>
	internal Boolean IsReadOnly
	{
		get;
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecurityCritical]
#endif
		init;
	}

	/// <summary>
	/// Creates a new <see cref="FixedPointerValue"/> from current instance applying <paramref name="offset"/>.
	/// </summary>
	/// <param name="offset">The memory offset to apply.</param>
	/// <returns>A new <see cref="FixedPointerValue"/> instance.</returns>
	internal FixedPointerValue CreateOffset(Int32 offset) => new(this, offset);
	/// <summary>
	/// Validates any operation over the fixed memory block.
	/// </summary>
	/// <param name="isReadOnly">Indicates whether current operation is read-only one.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateOperation(Boolean isReadOnly = false)
	{
		if (this.Handle is not null)
			ValidationUtilities.ThrowIfInvalidPointer(this.Handle);
		ValidationUtilities.ThrowIfReadOnlyPointer(isReadOnly, this.IsReadOnly);
	}
	/// <summary>
	/// Validates any transformation operation over the fixed memory block.
	/// </summary>
	/// <param name="type">Destination type.</param>
	/// <param name="unmanagedType">Indicates whether <paramref name="type"/> is unmanaged.</param>
	/// <param name="isReadOnlyOperation">Indicates whether the current transformation operation is read-only.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateTransformation(Type type, Boolean unmanagedType, Boolean isReadOnlyOperation = true)
	{
		if (type == this.Type) return;
		ValidationUtilities.ThrowIfInvalidTransformation(this.Type, this.IsUnmanaged, type, unmanagedType,
		                                                 isReadOnlyOperation);
	}
	/// <summary>
	/// Validates the size of the referenced value type from the current instance.
	/// </summary>
	/// <param name="typeOf">CLR Type.</param>
	/// <param name="sizeOf">Type size in bytes.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateReferenceSize(Type typeOf, Int32 sizeOf)
		=> ValidationUtilities.ThrowIfInvalidRefTypePointer(this.Size, typeOf, sizeOf);
	/// <summary>
	/// Indicates whether the current memory block can be transformed into a <typeparamref name="T"/> buffer.
	/// </summary>
	/// <typeparam name="T">Destination type.</typeparam>
	/// <param name="isReadOnlyOperation">
	/// Indicates whether the transformation is read-only. The default value is <see langword="true"/>.
	/// </param>
	/// <returns>
	/// <see langword="false"/> when the transformation writes a read-only block. Otherwise,
	/// <see langword="true"/> if the block can be viewed as <typeparamref name="T"/>.
	/// </returns>
	/// <remarks>
	/// Unmanaged types can be transformed into each other. When both types are reference types,
	/// <see cref="Object"/> is allowed in either direction. A read also allows a destination that accepts the
	/// current type. A write of a reference type allows only the same type or <see cref="Object"/>.
	/// </remarks>
	internal Boolean IsValidTransformation<T>(Boolean isReadOnlyOperation = true)
	{
		if (!isReadOnlyOperation && this.IsReadOnly) return false;
		Type destinationType = typeof(T);
		if (this.Type == destinationType) return true;
		if (this.IsUnmanaged && !RuntimeHelpers.IsReferenceOrContainsReferences<T>()) return true;
		if (this.Type is not { IsValueType: false, } || destinationType.IsValueType) return false;
		if (this.Type == typeof(Object) || destinationType == typeof(Object)) return true;
		return isReadOnlyOperation && destinationType.IsAssignableFrom(this.Type);
	}
	/// <summary>
	/// Indicates whether the current memory block can be transformed into a <typeparamref name="T"/> value.
	/// </summary>
	/// <typeparam name="T">Destination type.</typeparam>
	/// <param name="isReadOnlyOperation">
	/// Indicates whether the transformation is read-only. The default value is <see langword="true"/>.
	/// </param>
	/// <returns>
	/// <see langword="false"/> when the transformation writes a read-only block or the block is smaller than
	/// <typeparamref name="T"/>. Otherwise, <see langword="true"/> if the block can be viewed as one
	/// <typeparamref name="T"/> value.
	/// </returns>
	/// <remarks>
	/// Requires <see cref="FixedPointerValue.Size"/> to be at least <c>sizeof(T)</c>. Unmanaged types can be
	/// transformed into each other. When both types are reference types, <see cref="Object"/> is allowed in either
	/// direction. A read also allows a destination that accepts the current type. A write of a reference type allows
	/// only the same type or <see cref="Object"/>.
	/// </remarks>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
#pragma warning disable CS8500
	internal unsafe Boolean IsValidValueTransformation<T>(Boolean isReadOnlyOperation = true)
		=> this.IsValidTransformation<T>(isReadOnlyOperation) && this.Size >= sizeof(T);
#pragma warning restore CS8500
}