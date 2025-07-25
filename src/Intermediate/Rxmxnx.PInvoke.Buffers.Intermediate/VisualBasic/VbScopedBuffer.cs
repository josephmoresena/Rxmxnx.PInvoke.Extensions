namespace Rxmxnx.PInvoke.VisualBasic;

/// <summary>
/// A <see cref="ScopedBuffer{T}"/> wrapper for Visual Basic .NET compliant.
/// </summary>
#if !PACKAGE
[ExcludeFromCodeCoverage]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
[StructLayout(LayoutKind.Sequential)]
public readonly unsafe struct VbScopedBuffer<T> : IEnumerableSequence<T>
{
	/// <summary>
	/// Size of the element type.
	/// </summary>
#pragma warning disable CS8500
	private static readonly Int32 sizeOfT = sizeof(T);
#pragma warning restore CS8500

	/// <summary>
	/// Managed array.
	/// </summary>
	private readonly T[]? _array;
	/// <summary>
	/// Unmanaged pointer.
	/// </summary>
	private readonly void* _pointer;

	/// <inheritdoc cref="Span{T}.this"/>
	[IndexerName("Item")]
	public T this[Int32 index]
	{
		get
		{
			ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this.Length);
			ref T refT = ref this._array is not null ?
				ref this._array.AsSpan()[index] :
				ref Unsafe.AsRef<T>((new IntPtr(this._pointer) + index * VbScopedBuffer<T>.sizeOfT).ToPointer());
			return refT;
		}
		set
		{
			ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this.Length);
			ref T refT = ref this._array is not null ?
				ref this._array.AsSpan()[index] :
				ref Unsafe.AsRef<T>((new IntPtr(this._pointer) + index * VbScopedBuffer<T>.sizeOfT).ToPointer());
			refT = value;
		}
	}
	/// <inheritdoc cref="ScopedBuffer{T}.InStack"/>
	public Boolean InStack => this._array is null;
	/// <inheritdoc cref="ScopedBuffer{T}.FullLength"/>
	public Int32 FullLength => this._array?.Length ?? this.BufferMetadata?.Size ?? this.Length;
	/// <inheritdoc cref="ScopedBuffer{T}.BufferMetadata"/>
	public BufferTypeMetadata? BufferMetadata { get; }
	/// <inheritdoc cref="Span{T}.Length"/>
	public UInt16 Length { get; }

	/// <summary>
	/// Creates a <see cref="ScopedBuffer{T}"/> from current instance.
	/// </summary>
	/// <returns>A <see cref="ScopedBuffer{T}"/> instance.</returns>
	public ScopedBuffer<T> ToValue()
	{
		Span<T> span;
		UInt16 length;
		if (this._array is not null)
		{
			span = this._array.AsSpan()[..this.Length];
			length = (UInt16)this._array.Length;
		}
		else
		{
			ref T refT = ref Unsafe.AsRef<T>(this._pointer);
			span = MemoryMarshal.CreateSpan(ref refT, this.Length);
			length = this.BufferMetadata?.Size ?? this.Length;
		}
		return new(span, this._array is not null, length, this.BufferMetadata);
	}

	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="refT">A <typeparamref name="T"/> managed reference.</param>
	/// <param name="length">Buffer length.</param>
	/// <param name="bufferTypeMetadata">Buffer type metadata.</param>
	internal VbScopedBuffer(ref T refT, UInt16 length, BufferTypeMetadata? bufferTypeMetadata = default)
	{
		this._pointer = Unsafe.AsPointer(ref refT);
		this.Length = length;
		this.BufferMetadata = bufferTypeMetadata;
		this._array = default;
	}
	/// <summary>
	/// Internal constructor.
	/// </summary>
	/// <param name="array">A <typeparamref name="T"/> array.</param>
	/// <param name="length">Buffer length.</param>
	internal VbScopedBuffer(T[] array, UInt16 length)
	{
		this._array = array;
		this.Length = length;
		this.BufferMetadata = default;
		this._pointer = default;
	}

	T IEnumerableSequence<T>.GetItem(Int32 index) => this[index];
	Int32 IEnumerableSequence<T>.GetSize() => this.Length;
}