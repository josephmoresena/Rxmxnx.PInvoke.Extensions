namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CString
{
#if NET7_0_OR_GREATER || !PACKAGE
	/// <summary>
	/// Custom marshaller for <see cref="CString"/> to native null-terminated UTF-8 text.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
#if NET7_0_OR_GREATER
	[CustomMarshaller(typeof(CString), MarshalMode.Default, typeof(Marshaller))]
#endif
	public ref struct Marshaller
	{
		/// <summary>
		/// Managed instance.
		/// </summary>
		private CString? _managed;
		/// <summary>
		/// Pointer to unmanaged memory containing UTF-8 text.
		/// </summary>
		private IntPtr _pointer;
		/// <summary>
		/// Memory handle for pinning the managed instance.
		/// </summary>
		private MemoryHandle _pinnable;
		/// <summary>
		/// Indicates whether memory is allocated.
		/// </summary>
		private Boolean _allocated;

		/// <summary>
		/// Releases the unmanaged memory.
		/// </summary>
		public void Free()
		{
			if (this._pointer == IntPtr.Zero) return;
			if (this._pinnable.Pointer == this._pointer.ToPointer())
			{
				this._pinnable.Dispose();
				this._pointer = IntPtr.Zero;
				return;
			}
			if (this._allocated)
				Marshal.FreeHGlobal(this._pointer);
			this._pointer = IntPtr.Zero;
			this._allocated = false;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Marshaller"/> struct form a managed <see cref="CString"/>.
		/// </summary>
		/// <param name="managed">A <see cref="CString"/> instance.</param>
		public void FromManaged(CString? managed)
		{
			this.Free();
			this._managed = managed;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Marshaller"/> struct from a null-terminated UTF-8 text pointer.
		/// </summary>
		/// <param name="unmanaged">A null-terminated UTF-8 text pointer.</param>
		public void FromUnmanaged(IntPtr unmanaged)
		{
			this.Free();
			this._pointer = unmanaged;
		}
		/// <summary>
		/// Retrieves the pointer to the unmanaged memory containing UTF-8 text.
		/// </summary>
		/// <returns>A pointer to unmanaged memory containing UTF-8 text.</returns>
		public IntPtr ToUnmanaged()
		{
			if (this._managed is null) return IntPtr.Zero;
			if (this._pointer != IntPtr.Zero) return this._pointer;

			ReadOnlySpan<Byte> utf8Span = this._managed.AsSpan();
			if (this._managed.IsNullTerminated)
			{
				if (this._managed.IsReference)
				{
					// If the CString is a reference type, we can use the pointer directly.
					this._pointer = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));
					return this._pointer;
				}

				this._pinnable = this._managed.TryPin(out Boolean pinned);
				if (pinned)
				{
					// If the CString is a value type and pinned, we can use the pointer from the pinning handle.
					this._pointer = (IntPtr)this._pinnable.Pointer;
					return this._pointer;
				}

				if (this._managed.IsFunction && !MemoryInspector.MayBeNonLiteral(utf8Span))
				{
					// If the CString is a literal, we can use the pointer directly.
					this._pointer = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));
					return this._pointer;
				}
			}

			// If the CString is not null-terminated or not pinned, we need to allocate unmanaged memory.
			this._pointer = Marshal.AllocHGlobal(this._managed.Length + 1);
			this._allocated = true;

			Span<Byte> output = new(this._pointer.ToPointer(), this._managed.Length + 1);
			utf8Span.CopyTo(output);
			output[^1] = default; // Ensure null-termination.

			return this._pointer;
		}
		/// <summary>
		/// Retrieves the managed <see cref="CString"/> instance.
		/// </summary>
		/// <returns>A <see cref="CString"/> instance.</returns>
		public CString? ToManaged()
		{
			if (IntPtr.Zero == this._pointer) return null;
			return this._managed ??= CString.CreateNullTerminatedUnsafe(this._pointer);
		}
	}
#endif
}