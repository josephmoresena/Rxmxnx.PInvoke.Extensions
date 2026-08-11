#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
namespace Rxmxnx.PInvoke;

/// <summary>
/// Represents a <see cref="CStringSequence"/> that is fixed in memory.
/// </summary>
[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS1121)]
#endif
#pragma warning disable CS0618
public readonly unsafe ref struct FixedCStringSequence
{
	/// <summary>
	/// The <see cref="CString"/> representation of UTF-8 sequence.
	/// </summary>
	private readonly CString? _value;
	/// <summary>
	/// Array of <see cref="CString"/> values.
	/// </summary>
	private readonly CString[]? _values;
	/// <summary>
	/// Indicates whether the current instance remains valid.
	/// </summary>
	private readonly FixedValueHandle? _handle;

	/// <summary>
	/// Gets the list of <see cref="CString"/> values in the sequence.
	/// </summary>
	public IReadOnlyList<CString> Values => this._values ?? [];
	/// <summary>
	/// Gets the element at the given index in the sequence.
	/// </summary>
	/// <param name="index">A position in the current instance.</param>
	/// <returns>The object at position <paramref name="index"/>.</returns>
	/// <exception cref="IndexOutOfRangeException">
	/// Thrown when <paramref name="index"/> is greater than or equal to the length of this object or less than zero.
	/// </exception>
	[IndexerName("Item")]
	public IReadOnlyFixedMemory this[Int32 index]
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			ValidationUtilities.ThrowIfInvalidSequenceIndex(index, this.Values.Count);
			void* ptr = this.GetPointer(index, out Int32 length);
			return new ReadOnlyFixedContext<Byte>(ptr, length, this._handle!);
		}
	}

	/// <summary>
	/// Initializes a new instance of the <see cref="FixedCStringSequence"/> struct.
	/// </summary>
	/// <param name="values">Array of <see cref="CString"/> values.</param>
	/// <param name="value">The <see cref="CString"/> representation of UTF-8 sequence.</param>
	internal FixedCStringSequence(CString[] values, CString value)
	{
		this._values = values;
		this._value = value;
		this._handle = new();
	}

	/// <summary>
	/// Creates an array of <see cref="IReadOnlyFixedMemory"/> instances from the current instance.
	/// </summary>
	/// <returns>An array of <see cref="IReadOnlyFixedMemory"/> instances.</returns>
	public IReadOnlyFixedMemory[] ToArray()
	{
		IReadOnlyFixedMemory[] result = new IReadOnlyFixedMemory[this.Values.Count];
		for (Int32 i = 0; i < result.Length; i++)
			result[i] = this[i];
		return result;
	}

	/// <inheritdoc/>
	public override String? ToString() => this._value?.ToString();

	/// <summary>
	/// Implicitly converts a <see cref="FixedCStringSequence"/> to a <see cref="ReadOnlyFixedMemoryList"/>.
	/// </summary>
	/// <param name="fseq">A <see cref="FixedCStringSequence"/> instance.</param>
	public static implicit operator ReadOnlyFixedMemoryList(FixedCStringSequence fseq)
	{
		FixedPointerInfo[] info = new FixedPointerInfo[fseq.Values.Count];
		ReadOnlyFixedMemory?[] memories = new ReadOnlyFixedMemory?[fseq.Values.Count];
		for (Int32 i = 0; i < memories.Length; i++)
		{
			void* ptr = fseq.GetPointer(i, out Int32 length);
			info[i] = new()
			{
				Pointer = ptr,
				Count = length,
				SizeOf = sizeof(Byte),
				IsUnmanaged = true,
#if !NET5_0_OR_GREATER
				ConstructorOrFunctionPointer = CStringSequence.ConstructorPointer,
				TypeOrFunctionPointer = CStringSequence.TypePointer,
#else
				ConstructorOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&ReadOnlyFixedContext<Byte>.CreateInstance),
				TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<Byte>),
#endif
			};
		}
		return new(new()
		{
			IsReadOnly = true, Handle = fseq._handle!, Information = info, Instances = memories,
		});
	}
	/// <summary>
	/// Implicitly converts a <see cref="FixedCStringSequence"/> to a <see cref="FixedPointerInfo"/>.
	/// </summary>
	/// <param name="fseq">A <see cref="FixedCStringSequence"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static implicit operator FixedPointerValueList(FixedCStringSequence fseq)
	{
		FixedPointerInfo[] info = new FixedPointerInfo[fseq.Values.Count];
		ReadOnlyFixedMemory?[] memories = new ReadOnlyFixedMemory?[fseq.Values.Count];
		for (Int32 i = 0; i < memories.Length; i++)
		{
			void* ptr = fseq.GetPointer(i, out Int32 length);
			info[i] = new()
			{
				Pointer = ptr,
				Count = length,
				SizeOf = sizeof(Byte),
				IsUnmanaged = true,
#if !NET5_0_OR_GREATER
				ConstructorOrFunctionPointer = CStringSequence.ConstructorPointer,
				TypeOrFunctionPointer = CStringSequence.TypePointer,
#else
				ConstructorOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&ReadOnlyFixedContext<Byte>.CreateInstance),
				TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<Byte>),
#endif
			};
		}
		return new()
		{
			IsReadOnly = true, Handle = fseq._handle!, Information = info, Instances = memories,
		};
	}

	/// <summary>
	/// Retrieves read-only span enumerator from current instance.
	/// </summary>
	/// <returns>A read-only span enumerator from current instance.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public ReadOnlySpan<CString?>.Enumerator GetEnumerator()
	{
		ReadOnlySpan<CString?> span = this._values;
		return span.GetEnumerator();
	}

	/// <summary>
	/// Invalidates the current sequence.
	/// </summary>
	internal void Unload() => this._handle?.Dispose();

	/// <summary>
	/// Retrieves an unmanaged pointer for the element at the specified <paramref name="index"/>.
	/// </summary>
	/// <param name="index">The index of the element to retrieve.</param>
	/// <param name="length">Output. Length of the element.</param>
	/// <returns>An unmanaged pointer for the element.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private void* GetPointer(Int32 index, out Int32 length)
	{
		CString cstr = this._values![index];
		fixed (void* ptr = cstr) // Use Pinnable reference
		{
			length = cstr.Length;
			return ptr;
		}
	}
}
#pragma warning restore CS0618
#endif