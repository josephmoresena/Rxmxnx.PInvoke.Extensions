namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
		/// <summary>
		/// Represents a native linux file buffer.
		/// </summary>
		internal unsafe struct NativeFile : IDisposable
		{
			/// <summary>
			/// File name of <c>/proc/self/maps</c>.
			/// </summary>
			private const String SelfMapsFileName = "瀯潲\u2f63敳晬洯灡s";

			[DllImport("libc", EntryPoint = "open", SetLastError = false)]
			private static extern unsafe Int32 Open(void* pathname, Int32 flags);
			[DllImport("libc", EntryPoint = "read", SetLastError = false)]
			private static extern unsafe Int64 Read(Int32 fd, Byte* buffer, UInt16 count);
			[DllImport("libc", EntryPoint = "lseek", SetLastError = false)]
			private static extern Int64 Seek(Int32 fd, Int64 offset, Int32 whence);
			[DllImport("libc", EntryPoint = "close", SetLastError = false)]
			private static extern Int32 Close(Int32 fd);
			[DllImport("libc", EntryPoint = "__errno_location", SetLastError = false)]
			private static extern Int32* GetErrorPointer();

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
			public Int32 Read(Span<Byte> buffer)
			{
				fixed (Byte* ptr = &MemoryMarshal.GetReference(buffer))
				{
					Int64 result = NativeFile.Read(this._fd, ptr, (UInt16)buffer.Length);
					NativeFile.ValidateResult(result);
					return (Int32)result;
				}
			}
			/// <summary>
			/// Advances <paramref name="offset"/> position on the current file.
			/// </summary>
			/// <param name="offset">Offset to advance.</param>
			/// <returns>
			/// <see langword="true"/> if position file setting is successful; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public Boolean Seek(Int64 offset)
			{
				Int64 result = NativeFile.Seek(this._fd, offset, 1);
				NativeFile.ValidateResult(result);
				return result >= 0;
			}
			/// <inheritdoc/>
			public void Dispose()
			{
				if (this._fd <= 0) return;
				NativeFile.ValidateResult(NativeFile.Close(this._fd));
				this._fd = -1;
			}

			/// <summary>
			/// Creates a <see cref="NativeFile"/> instance for <c>/proc/self/maps</c> file.
			/// </summary>
			/// <returns>A <see cref="NativeFile"/> instance for <c>/proc/self/maps</c> file.</returns>
			public static NativeFile OpenSelfMaps()
			{
				void* ptr = Unsafe.AsPointer(ref MemoryMarshal.GetReference(NativeFile.SelfMapsFileName.AsSpan()));
				Int32 fd = NativeFile.Open(ptr, 0);
				return new() { _fd = fd, };
			}

			/// <summary>
			/// Validates <paramref name="result"/> value.
			/// </summary>
			/// <param name="result">Resulting value.</param>
			private static void ValidateResult(Int32 result)
			{
				if (result >= 0) return;
				NativeFile.ClearError();
			}
			/// <summary>
			/// Validates <paramref name="result"/> value.
			/// </summary>
			/// <param name="result">Resulting value.</param>
			private static void ValidateResult(Int64 result)
			{
				if (result >= 0) return;
				NativeFile.ClearError();
			}
			/// <summary>
			/// Clears errno value.
			/// </summary>
			private static void ClearError()
			{
				ref Int32 err = ref Unsafe.AsRef<Int32>(NativeFile.GetErrorPointer());
				err = 0;
			}
		}
	}
}