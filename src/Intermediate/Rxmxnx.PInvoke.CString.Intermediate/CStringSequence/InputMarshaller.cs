namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public unsafe partial class CStringSequence
{
#if NET7_0_OR_GREATER || !PACKAGE
	/// <summary>
	/// Custom marshaller for <see cref="CStringSequence"/> to native null-terminated UTF-8 text array.
	/// </summary>
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
#if NET7_0_OR_GREATER
	[CustomMarshaller(typeof(CStringSequence), MarshalMode.ManagedToUnmanagedIn, typeof(InputMarshaller))]
	[CustomMarshaller(typeof(Utf8View), MarshalMode.ManagedToUnmanagedIn, typeof(InputMarshaller))]
#endif
	public ref struct InputMarshaller
	{
		/// <summary>
		/// Managed instance.
		/// </summary>
		private CStringSequence? _managed;
		/// <summary>
		/// Handle for sequence data.
		/// </summary>
		private GCHandle _handle;
		/// <summary>
		/// Memory handle for <see cref="CString.Empty"/>.
		/// </summary>
		private MemoryHandle _emptyHandle;
		/// <summary>
		/// Pointer to the text array.
		/// </summary>
		private IntPtr _pointer;
		/// <summary>
		/// Indicates whether unmanaged buffer should include empty instances.
		/// </summary>
		private Boolean _includeEmpty;

		/// <summary>
		/// Releases the unmanaged memory.
		/// </summary>
		public void Free()
		{
			if (this._pointer == IntPtr.Zero) return;
			if (this._emptyHandle.Pointer != default)
			{
				this._emptyHandle.Dispose();
				this._emptyHandle = default;
			}
			if (this._handle.IsAllocated)
			{
				this._handle.Free();
				this._pointer = IntPtr.Zero;
			}
			Marshal.FreeHGlobal(this._pointer);
			this._pointer = IntPtr.Zero;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="InputMarshaller"/> struct from a managed <see cref="CStringSequence"/>.
		/// </summary>
		/// <param name="managed">A <see cref="CStringSequence"/> instance.</param>
		public void FromManaged(CStringSequence? managed)
		{
			this.Free();
			this._managed = managed;
			this._includeEmpty = false;
		}
		/// <summary>
		/// Initializes a new instance of the <see cref="InputMarshaller"/> struct from a managed <see cref="Utf8View"/>.
		/// </summary>
		/// <param name="managed">A <see cref="Utf8View"/> instance.</param>
		public void FromManaged(Utf8View managed)
		{
			this.Free();
			this._managed = managed.Source;
			this._includeEmpty = managed.EmptyItemsIncluded;
		}

		/// <summary>
		/// Retrieves unmanaged pointer to the text array.
		/// </summary>
		/// <returns>A pointer to the text array.</returns>
		public IntPtr ToUnmanaged()
		{
			if (this._managed is null) return IntPtr.Zero;
			if (this._pointer != IntPtr.Zero) return this._pointer;

			this._handle = GCHandle.Alloc(this._managed._value, GCHandleType.Pinned);
			this._pointer = InputMarshaller.CreateUtf8Memory(this._managed, this._includeEmpty);
			if (this._includeEmpty)
				this._emptyHandle = CString.Empty.TryPin(out _);
			return this._pointer;
		}

		/// <summary>
		/// Creates a pointer to the unmanaged memory containing UTF-8 text array.
		/// </summary>
		/// <param name="source">A <see cref="CStringSequence"/> instance.</param>
		/// <param name="includeEmpty">Indicates whether non-empty elements should be included.</param>
		/// <returns>A pointer to the unmanaged memory containing UTF-8 text array.</returns>
		private static IntPtr CreateUtf8Memory(CStringSequence source, Boolean includeEmpty)
		{
			IntPtr ptr = InputMarshaller.Alloc(source, includeEmpty, out Int32 length);
			Span<ReadOnlyValPtr<Byte>> output = new(ptr.ToPointer(), length);

			if (!includeEmpty)
				InputMarshaller.InitializeNonEmptyUtf8Array(source, output);
			else
				InputMarshaller.InitializeUtf8Array(source, output);
			output[^1] = ReadOnlyValPtr<Byte>.Zero; // Null-terminate the array
			return ptr;
		}
		/// <summary>
		/// Retrieves the unmanaged memory address of <paramref name="ref0"/>.
		/// </summary>
		/// <param name="ref0">A read-only byte reference.</param>
		/// <returns>Unmanaged memory address of <paramref name="ref0"/>.</returns>
		private static ReadOnlyValPtr<Byte> GetAddress(ref readonly Byte ref0)
		{
			ReadOnlyValPtr<Byte> valPtr = (ReadOnlyValPtr<Byte>)Unsafe.AsPointer(ref Unsafe.AsRef(in ref0));
			return valPtr;
		}
		/// <summary>
		/// Initializes the UTF-8 array for all items on <paramref name="source"/>.
		/// </summary>
		/// <param name="source">A <see cref="CStringSequence"/> instance.</param>
		/// <param name="arrayBuffer">Buffer of output array.</param>
		private static void InitializeUtf8Array(CStringSequence source, Span<ReadOnlyValPtr<Byte>> arrayBuffer)
		{
			ReadOnlyValPtr<Byte> emptyPtr = InputMarshaller.GetAddress(in CString.Empty.GetPinnableReference());
			ReadOnlyValPtr<Byte> valPtr = InputMarshaller.GetAddress(in source.GetPinnableReference());
			for (Int32 i = 0; i < source.Count; i++)
			{
				Int32? currentLength = source._lengths[i];
				switch (currentLength)
				{
					case null:
						arrayBuffer[i] = ReadOnlyValPtr<Byte>.Zero;
						continue;
					case <= 0:
						arrayBuffer[i] = emptyPtr;
						continue;
					default:
						arrayBuffer[i] = valPtr;
						valPtr += currentLength.Value + 1; // +1 for the null terminator
						break;
				}
			}
		}
		/// <summary>
		/// Initializes the UTF-8 array for non-empty items on <paramref name="source"/>.
		/// </summary>
		/// <param name="source">A <see cref="CStringSequence"/> instance.</param>
		/// <param name="arrayBuffer">Buffer of output array.</param>
		private static void InitializeNonEmptyUtf8Array(CStringSequence source, Span<ReadOnlyValPtr<Byte>> arrayBuffer)
		{
			ReadOnlyValPtr<Byte> valPtr = InputMarshaller.GetAddress(in source.GetPinnableReference());
			Span<Int32> offsets = stackalloc Int32[source.NonEmptyCount];

			source.GetOffsets(offsets);
			for (Int32 i = 0; i < offsets.Length; i++)
				arrayBuffer[i] = valPtr + offsets[i];
		}
		/// <summary>
		/// Allocates unmanaged memory for the text array.
		/// </summary>
		/// <param name="source">A <see cref="CStringSequence"/></param>
		/// <param name="includeEmpty">Indicates whether output array should contain empty texts.</param>
		/// <param name="length">Output. Array length.</param>
		/// <returns>Pointer to unmanaged memory for the text array.</returns>
		private static IntPtr Alloc(CStringSequence source, Boolean includeEmpty, out Int32 length)
		{
			length = (includeEmpty ? source.Count : source.NonEmptyCount) + 1;
			return Marshal.AllocHGlobal(length * IntPtr.Size);
		}
	}
#endif
}