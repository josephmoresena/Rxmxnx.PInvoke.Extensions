namespace Rxmxnx.PInvoke;

/// <summary>
/// Ref-struct representing a pointer to a fixed block of memory.
/// </summary>
[Preserve(AllMembers = true)]
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
		[SecurityCritical]
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
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal void ValidateTransformation(Type type, Boolean unmanagedType)
	{
		if (type == this.Type) return;
		ValidationUtilities.ThrowIfInvalidTransformation(this.Type, this.IsUnmanaged, type, unmanagedType);
	}
}