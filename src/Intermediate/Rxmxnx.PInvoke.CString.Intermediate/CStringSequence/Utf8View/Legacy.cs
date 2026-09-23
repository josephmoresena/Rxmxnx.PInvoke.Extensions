#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER && !UAP10_0_16299
namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly ref partial struct Utf8View
	{
#pragma warning disable CS0282
		public ref partial struct Enumerator
		{
			/// <summary>
			/// Internal status.
			/// </summary>
			private enum SpanStatus : Byte
			{
				Null = 0x0,
				Empty = 0x1,
				NonEmpty = 0x2,
			}

			/// <summary>
			/// Current length offset.
			/// </summary>
			private Int32 _lengthOffset;
			/// <summary>
			/// Current buffer offset.
			/// </summary>
			private Int32 _bufferOffset;
			/// <summary>
			/// Current item offset.
			/// </summary>
			private Int32 _currentOffset;
			/// <summary>
			/// Current item length.
			/// </summary>
			private Int32 _currentLength;
			/// <summary>
			/// Current item status.
			/// </summary>
			private SpanStatus _currentStatus;

			public partial ReadOnlySpan<Byte> Current
			{
#if NETFRAMEWORK || NETSTANDARD2_0
				[SecuritySafeCritical]
#endif
				get
				{
					ValidationUtilities.ThrowIfInvalidEnumerator(this._instance is null, !this._active,
					                                             this._lengthOffset < (this._instance?.Count)
					                                             .GetValueOrDefault());
					return this._currentStatus switch
					{
						SpanStatus.Null => ReadOnlySpan<Byte>.Empty,
						SpanStatus.Empty => CString.Empty.AsSpan(),
						_ => this.GetInstanceBuffer().Slice(this._currentOffset, this._currentLength),
					};
				}
			}

#if NETFRAMEWORK || NETSTANDARD2_0
			[SecuritySafeCritical]
#endif
			public partial Boolean MoveNext()
			{
				if (this._instance is null) return false;
				while (this._lengthOffset < this._instance._lengths.Length)
				{
					this._currentLength = this._instance._lengths[this._lengthOffset++];
					if (this._excludeEmptyItems && this._currentLength <= 0) continue;
					switch (this._currentLength)
					{
						case 0:
							this._currentStatus = SpanStatus.Empty;
							break;
						case > 0:
							this._currentStatus = SpanStatus.NonEmpty;
							this._currentOffset = this._bufferOffset;
							this._bufferOffset += this._currentLength + 1;
							break;
						default:
							this._currentStatus = SpanStatus.Null;
							break;
					}
					this._active = true;
					return true;
				}

				this._active = false;
				this._currentStatus = SpanStatus.Null;
				return false;
			}
#if NETFRAMEWORK || NETSTANDARD2_0
			[SecuritySafeCritical]
#endif
			public partial void Reset()
			{
				this._lengthOffset = 0;
				this._bufferOffset = 0;
				this._currentStatus = SpanStatus.Null;
				this._active = false;
			}
		}
#pragma warning restore CS0282
	}
}
#endif