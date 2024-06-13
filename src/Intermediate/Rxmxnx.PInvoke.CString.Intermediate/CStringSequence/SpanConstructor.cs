namespace Rxmxnx.PInvoke;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS107)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public unsafe partial class CStringSequence
{
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// from <paramref name="span0"/>.
	/// </summary>
	/// <param name="span0">A read-only binary span containing UTF-8 text.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0)
	{
		this._lengths = [span0.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptrSpan = &MemoryMarshal.GetReference(stackalloc IntPtr[] { (IntPtr)ptr0, }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1)
	{
		this._lengths = [span0.Length, span1.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptrSpan = &MemoryMarshal.GetReference(stackalloc IntPtr[] { (IntPtr)ptr0, (IntPtr)ptr1, }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	/// <param name="span2">3rd UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2)
	{
		this._lengths = [span0.Length, span1.Length, span2.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptrSpan =
			       &MemoryMarshal.GetReference(stackalloc IntPtr[] { (IntPtr)ptr0, (IntPtr)ptr1, (IntPtr)ptr2, }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	/// <param name="span2">3rd UTF-8 span.</param>
	/// <param name="span3">4th UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2,
		ReadOnlySpan<Byte> span3)
	{
		this._lengths = [span0.Length, span1.Length, span2.Length, span3.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptrSpan =
			       &MemoryMarshal.GetReference(stackalloc IntPtr[]
			       {
				       (IntPtr)ptr0, (IntPtr)ptr1, (IntPtr)ptr2, (IntPtr)ptr3,
			       }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	/// <param name="span2">3rd UTF-8 span.</param>
	/// <param name="span3">4th UTF-8 span.</param>
	/// <param name="span4">5th UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2,
		ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4)
	{
		this._lengths = [span0.Length, span1.Length, span2.Length, span3.Length, span4.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptrSpan =
			       &MemoryMarshal.GetReference(stackalloc IntPtr[]
			       {
				       (IntPtr)ptr0, (IntPtr)ptr1, (IntPtr)ptr2, (IntPtr)ptr3,
				       (IntPtr)ptr4,
			       }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	/// <param name="span2">3rd UTF-8 span.</param>
	/// <param name="span3">4th UTF-8 span.</param>
	/// <param name="span4">5th UTF-8 span.</param>
	/// <param name="span5">6th UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2,
		ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4, ReadOnlySpan<Byte> span5)
	{
		this._lengths = [span0.Length, span1.Length, span2.Length, span3.Length, span4.Length, span5.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptrSpan = &MemoryMarshal.GetReference(stackalloc IntPtr[]
		       {
			       (IntPtr)ptr0, (IntPtr)ptr1, (IntPtr)ptr2,
			       (IntPtr)ptr3, (IntPtr)ptr4, (IntPtr)ptr5,
		       }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	/// <param name="span2">3rd UTF-8 span.</param>
	/// <param name="span3">4th UTF-8 span.</param>
	/// <param name="span4">5th UTF-8 span.</param>
	/// <param name="span5">6th UTF-8 span.</param>
	/// <param name="span6">7th UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2,
		ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4, ReadOnlySpan<Byte> span5, ReadOnlySpan<Byte> span6)
	{
		this._lengths =
			[span0.Length, span1.Length, span2.Length, span3.Length, span4.Length, span5.Length, span6.Length,];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptrSpan = &MemoryMarshal.GetReference(stackalloc IntPtr[]
		       {
			       (IntPtr)ptr0, (IntPtr)ptr1, (IntPtr)ptr2,
			       (IntPtr)ptr3, (IntPtr)ptr4, (IntPtr)ptr5,
			       (IntPtr)ptr6,
		       }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
	/// <summary>
	/// Initializes a new instance of the <see cref="CStringSequence"/> class
	/// using UTF-8 texts contained in given read-only spans.
	/// </summary>
	/// <param name="span0">1st UTF-8 span.</param>
	/// <param name="span1">2nd UTF-8 span.</param>
	/// <param name="span2">3rd UTF-8 span.</param>
	/// <param name="span3">4th UTF-8 span.</param>
	/// <param name="span4">5th UTF-8 span.</param>
	/// <param name="span5">6th UTF-8 span.</param>
	/// <param name="span6">7th UTF-8 span.</param>
	/// <param name="span7">8th UTF-8 span.</param>
	public CStringSequence(ReadOnlySpan<Byte> span0, ReadOnlySpan<Byte> span1, ReadOnlySpan<Byte> span2,
		ReadOnlySpan<Byte> span3, ReadOnlySpan<Byte> span4, ReadOnlySpan<Byte> span5, ReadOnlySpan<Byte> span6,
		ReadOnlySpan<Byte> span7)
	{
		this._lengths =
		[
			span0.Length, span1.Length, span2.Length, span3.Length, span4.Length, span5.Length, span6.Length,
			span7.Length,
		];
		this._cache = CStringSequence.CreateCache(this._lengths);
		fixed (void* ptr0 = &MemoryMarshal.GetReference(span0))
		fixed (void* ptr1 = &MemoryMarshal.GetReference(span1))
		fixed (void* ptr2 = &MemoryMarshal.GetReference(span2))
		fixed (void* ptr3 = &MemoryMarshal.GetReference(span3))
		fixed (void* ptr4 = &MemoryMarshal.GetReference(span4))
		fixed (void* ptr5 = &MemoryMarshal.GetReference(span5))
		fixed (void* ptr6 = &MemoryMarshal.GetReference(span6))
		fixed (void* ptr7 = &MemoryMarshal.GetReference(span7))
		fixed (void* ptrSpan = &MemoryMarshal.GetReference(stackalloc IntPtr[]
		       {
			       (IntPtr)ptr0, (IntPtr)ptr1, (IntPtr)ptr2,
			       (IntPtr)ptr3, (IntPtr)ptr4, (IntPtr)ptr5,
			       (IntPtr)ptr6, (IntPtr)ptr7,
		       }))
			this._value = CStringSequence.CreateBuffer(ptrSpan, this._lengths);
	}
}