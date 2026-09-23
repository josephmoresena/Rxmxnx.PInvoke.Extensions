namespace Rxmxnx.PInvoke;

public readonly ref partial struct FixedPointerValue
{
	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="ptr">Unmanaged fixed pointer.</param>
	/// <param name="byteCount">Memory block byte count.</param>
	internal FixedPointerValue(IntPtr ptr, Int32 byteCount)
	{
		this._ptr = ptr;
		this._byteCount = byteCount;
	}

	/// <summary>
	/// Constructor.
	/// </summary>
	/// <param name="value">Original <see cref="FixedPointerValue"/> instance.</param>
	/// <param name="offset">Memory offset to apply.</param>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private FixedPointerValue(FixedPointerValue value, Int32 offset)
	{
		this._ptr = value._ptr;
		this._byteCount = value._byteCount;
		this._offset = offset;

		this.Handle = value.Handle;
		this.Type = value.Type;
		this.IsUnmanaged = value.IsUnmanaged;
		this.IsReadOnly = value.IsReadOnly;
	}
}