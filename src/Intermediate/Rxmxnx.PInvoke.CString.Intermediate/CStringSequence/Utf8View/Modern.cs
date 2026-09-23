#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER || UAP10_0_16299
namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly ref partial struct Utf8View
	{
#pragma warning disable CS0282
		public ref partial struct Enumerator
		{
			/// <summary>
			/// Remaining lengths.
			/// </summary>
			private ReadOnlySpan<Int32> _remaining;
			/// <summary>
			/// Remaining UTF-8 buffer.
			/// </summary>
			private ReadOnlySpan<Byte> _buffer;
			/// <summary>
			/// Current item span.
			/// </summary>
			private ReadOnlySpan<Byte> _current;

			public partial ReadOnlySpan<Byte> Current
			{
				get
				{
					ValidationUtilities.ThrowIfInvalidEnumerator(this._instance is null, !this._active,
					                                             this._remaining.IsEmpty);
					return this._current;
				}
			}

			public partial Boolean MoveNext()
			{
				while (!this._remaining.IsEmpty)
				{
					Int32 length = this._remaining[0];
					this._remaining = this._remaining[1..];

					if (this._excludeEmptyItems && length <= 0) continue;

					this._current = length switch
					{
						0 => CString.Empty.AsSpan(),
						> 0 => this._buffer[..length],
						_ => ReadOnlySpan<Byte>.Empty,
					};
					if (length > 0) this._buffer = this._buffer[(length + 1)..];
					this._active = true;
					return true;
				}

				this._active = false;
				this._current = default;
				return false;
			}
			public partial void Reset()
			{
				this._remaining = this._instance?._lengths;
				this._buffer = this.GetInstanceBuffer();
				this._current = default;
				this._active = false;
			}
		}
#pragma warning restore CS0282
	}
}
#endif