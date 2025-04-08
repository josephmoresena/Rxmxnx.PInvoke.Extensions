namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
		/// <summary>
		/// Represents a native linux file buffer.
		/// </summary>
		internal struct NativeFile : IDisposable
		{
			/// <summary>
			/// File name of <c>/proc/self/maps</c>.
			/// </summary>
			private const String SelfMapsFileName = "瀯潲\u2f63敳晬洯灡s";

			[DllImport("libc", EntryPoint = "open", SetLastError = true)]
			private static extern unsafe Int32 Open(void* pathname, Int32 flags);
			[DllImport("libc", EntryPoint = "read", SetLastError = true)]
			private static extern unsafe Int64 Read(Int32 fd, Byte* buffer, UInt16 count);
			[DllImport("libc", EntryPoint = "lseek", SetLastError = true)]
			private static extern Int64 Seek(Int32 fd, Int64 offset, Int32 whence);
			[DllImport("libc", EntryPoint = "close", SetLastError = true)]
			private static extern Int32 Close(Int32 fd);

			/// <summary>
			/// Native file descriptor.
			/// </summary>
			private Int32 _fd;

			/// <summary>
			/// Reads a sequence of bytes from the current file and copy them to <paramref name="buffer"/>.
			/// </summary>
			/// <param name="buffer">Destination buffer.</param>
			/// <returns>The total number of bytes read into the buffer.</returns>
			/// <remarks>The position within the file by the number of bytes read.</remarks>
			public unsafe Int32 Read(Span<Byte> buffer)
			{
				fixed (Byte* ptr = &MemoryMarshal.GetReference(buffer))
					return (Int32)NativeFile.Read(this._fd, ptr, (UInt16)buffer.Length);
			}
			/// <summary>
			/// Advances <paramref name="offset"/> position on the current file.
			/// </summary>
			/// <param name="offset">Offset to advance.</param>
			/// <returns>
			/// <see langword="true"/> if position file setting is successful; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public Boolean Seek(Int64 offset) => NativeFile.Seek(this._fd, offset, 1) >= 0;
			/// <inheritdoc/>
			public void Dispose()
			{
				if (this._fd <= 0) return;
				_ = NativeFile.Close(this._fd);
				this._fd = -1;
			}

			/// <summary>
			/// Creates a <see cref="NativeFile"/> instance for <c>/proc/self/maps</c> file.
			/// </summary>
			/// <returns>A <see cref="NativeFile"/> instance for <c>/proc/self/maps</c> file.</returns>
			public static unsafe NativeFile OpenSelfMaps()
			{
				void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(NativeFile.SelfMapsFileName.AsSpan()));
				Int32 fd = NativeFile.Open(ptr, 0);
				return new() { _fd = fd, };
			}
		}
	}
}