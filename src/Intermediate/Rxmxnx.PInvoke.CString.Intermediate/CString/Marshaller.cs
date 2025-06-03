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
			Marshal.FreeHGlobal(this._pointer);
			this._pointer = IntPtr.Zero;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Marshaller"/> struct form a managed <see cref="CString"/>.
		/// </summary>
		/// <param name="managed">A <see cref="CString"/> instance.</param>
		public void FromManaged(CString? managed)
		{
			this._managed = managed;
			if (managed is null)
			{
				this._pointer = IntPtr.Zero;
				this._pinnable = default;
				return;
			}

			ReadOnlySpan<Byte> utf8Span = managed.AsSpan();
			if (managed.IsNullTerminated)
			{
				if (managed.IsReference)
				{
					// If the CString is a reference type, we can use the pointer directly.
					this._pointer = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));
					return;
				}

				this._pinnable = managed.TryPin(out Boolean pinned);
				if (pinned)
				{
					// If the CString is a value type and pinned, we can use the pointer from the pinning handle.
					this._pointer = (IntPtr)this._pinnable.Pointer;
					return;
				}
				if (managed.IsFunction && MemoryInspector.Instance.IsLiteral(utf8Span))
				{
					// If the CString is a literal, we can use the pointer directly.
					this._pointer = (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(utf8Span));
					return;
				}
			}

			// If the CString is not null-terminated or not pinned, we need to allocate unmanaged memory.
			this._pointer = Marshal.AllocHGlobal(managed.Length + 1);

			Span<Byte> output = new(this._pointer.ToPointer(), managed.Length + 1);
			utf8Span.CopyTo(output);
			output[^1] = default; // Ensure null-termination.
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="Marshaller"/> struct from a null-terminated UTF-8 text pointer.
		/// </summary>
		/// <param name="unmanaged">A null-terminated UTF-8 text pointer.</param>
		public void FromUnmanaged(IntPtr unmanaged)
		{
			if (unmanaged == IntPtr.Zero)
			{
				this._pointer = IntPtr.Zero;
				this._pinnable = default;
				this._managed = null;
				return;
			}

			this._managed = CString.CreateNullTerminatedUnsafe(unmanaged);
		}
		/// <summary>
		/// Retrieves the pointer to the unmanaged memory containing UTF-8 text.
		/// </summary>
		/// <returns>A pointer to unmanaged memory containing UTF-8 text.</returns>
		public IntPtr ToUnmanaged() => this._pointer;
		/// <summary>
		/// Retrieves the managed <see cref="CString"/> instance.
		/// </summary>
		/// <returns>A <see cref="CString"/> instance.</returns>
		public CString? ToManaged() => this._managed;
	}
#endif
}