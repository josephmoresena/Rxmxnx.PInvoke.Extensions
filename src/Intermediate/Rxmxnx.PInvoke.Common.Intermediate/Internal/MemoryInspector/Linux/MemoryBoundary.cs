namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private sealed partial class Linux
	{
		/// <summary>
		/// Memory boundary struct.
		/// </summary>
		private readonly unsafe struct MemoryBoundary : IEquatable<MemoryBoundary>, IComparable<MemoryBoundary>,
			IWrapper<UIntPtr>
		{
			/// <summary>
			/// Internal value.
			/// </summary>
#if NET5_0_OR_GREATER
			public UIntPtr Value { get; private init; }
#else
			public readonly UIntPtr Value;

			UIntPtr IWrapper<UIntPtr>.Value => this.Value;
#endif

#if NET5_0_OR_GREATER
#else
#endif
			/// <summary>
			/// Indicates whether current boundary is terminal.
			/// </summary>

#if NET5_0_OR_GREATER
			public Boolean IsEnd { get; private init; }
#else
			public readonly Boolean IsEnd;
#endif

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="addressText">Textual UTF-8 boundary address.</param>
			/// <param name="isEnd">Indicates whether current boundary is an end boundary.</param>
			public MemoryBoundary(ReadOnlySpan<Byte> addressText, Boolean isEnd)
			{
				this.Value = MemoryBoundary.Parse(addressText);
				this.IsEnd = isEnd;
			}

#if !NET5_0_OR_GREATER
			/// <summary>
			/// Private constructor.
			/// </summary>
			/// <param name="value">Internal value.</param>
			/// <param name="isEnd">Indicates whether current boundary is terminal.</param>
			private MemoryBoundary(UIntPtr value, Boolean isEnd)
			{
				this.Value = value;
				this.IsEnd = isEnd;
			}
#endif

			/// <summary>
			/// Defines an implicit conversion of a given <see cref="IntPtr"/> to a <see cref="MemoryBoundary"/> instance.
			/// </summary>
			/// <param name="value">An <see cref="IntPtr"/> to implicitly convert.</param>
			public static implicit operator MemoryBoundary(void* value)
#if NET5_0_OR_GREATER
				=> new() { Value = new(value), IsEnd = false, };
#else
				=> new(new UIntPtr(value), false);
#endif

			/// <inheritdoc/>
			public Int32 CompareTo(MemoryBoundary other)
#if !NET5_0_OR_GREATER
				=> ((UInt64)this.Value).CompareTo((UInt64)other.Value);
#else
				=> this.Value.CompareTo(other.Value);
#endif
			/// <inheritdoc/>
			public Boolean Equals(MemoryBoundary other) => this.Value == other.Value;
			/// <inheritdoc/>
			public override Boolean Equals([NotNullWhen(true)] Object? obj)
				=> obj is MemoryBoundary boundary && this.Equals(boundary);
			/// <inheritdoc/>
			public override Int32 GetHashCode() => this.Value.GetHashCode();

			/// <summary>
			/// Compares two values to determine equality.
			/// </summary>
			/// <param name="left">The value to compare with <paramref name="right"/>.</param>
			/// <param name="right">The value to compare with <paramref name="left"/>.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="left"/> is equal to <paramref name="right"/>; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public static Boolean operator ==(MemoryBoundary left, MemoryBoundary right) => left.Value == right.Value;
			/// <summary>
			/// Compares two values to determine inequality.
			/// </summary>
			/// <param name="left">The value to compare with <paramref name="right"/>.</param>
			/// <param name="right">The value to compare with <paramref name="left"/>.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="left"/> is not equal to <paramref name="right"/>; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public static Boolean operator !=(MemoryBoundary left, MemoryBoundary right) => left.Value != right.Value;
			/// <summary>
			/// Compares two values to determine which is less.
			/// </summary>
			/// <param name="left">The value to compare with <paramref name="right"/>.</param>
			/// <param name="right">The value to compare with <paramref name="left"/>.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="left"/> is less than <paramref name="right"/>; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public static Boolean operator <(MemoryBoundary left, MemoryBoundary right)
#if NET7_0_OR_GREATER
				=> left.Value < right.Value;
#elif NET5_0_OR_GREATER
				=> left.Value.CompareTo(right.Value) < 0;
#else
				=> ((UInt64)left.Value).CompareTo((UInt64)right.Value) < 0;
#endif
			/// <summary>
			/// Compares two values to determine which is less or equal.
			/// </summary>
			/// <param name="left">The value to compare with <paramref name="right"/>.</param>
			/// <param name="right">The value to compare with <paramref name="left"/>.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="left"/> is less than or equal to <paramref name="right"/>; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public static Boolean operator <=(MemoryBoundary left, MemoryBoundary right)
#if NET7_0_OR_GREATER
				=> left.Value <= right.Value;
#elif NET5_0_OR_GREATER
				=> left.Value.CompareTo(right.Value) <= 0;
#else
				=> ((UInt64)left.Value).CompareTo((UInt64)right.Value) <= 0;
#endif
			/// <summary>
			/// Compares two values to determine which is greater.
			/// </summary>
			/// <param name="left">The value to compare with <paramref name="right"/>.</param>
			/// <param name="right">The value to compare with <paramref name="left"/>.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="left"/> is greater than <paramref name="right"/>; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public static Boolean operator >(MemoryBoundary left, MemoryBoundary right)
#if NET7_0_OR_GREATER
				=> left.Value > right.Value;
#elif NET5_0_OR_GREATER
				=> left.Value.CompareTo(right.Value) > 0;
#else
				=> ((UInt64)left.Value).CompareTo((UInt64)right.Value) > 0;
#endif
			/// <summary>
			/// Compares two values to determine which is greater or equal.
			/// </summary>
			/// <param name="left">The value to compare with <paramref name="right"/>.</param>
			/// <param name="right">The value to compare with <paramref name="left"/>.</param>
			/// <returns>
			/// <see langword="true"/> if <paramref name="left"/> is greater than or equal to <paramref name="right"/>; otherwise,
			/// <see langword="false"/>.
			/// </returns>
			public static Boolean operator >=(MemoryBoundary left, MemoryBoundary right)
#if NET7_0_OR_GREATER
				=> left.Value >= right.Value;
#elif NET5_0_OR_GREATER
				=> left.Value.CompareTo(right.Value) >= 0;
#else
				=> ((UInt64)left.Value).CompareTo((UInt64)right.Value) >= 0;
#endif

			/// <summary>
			/// Parses the span of UTF-8 hexadecimal characters into a <see cref="IntPtr"/> value.
			/// </summary>
			/// <param name="addressText">The span of UTF-8 characters to parse.</param>
			/// <returns>The result of parsing <paramref name="addressText"/>.</returns>
			private static UIntPtr Parse(ReadOnlySpan<Byte> addressText)
			{
				Span<Byte> addressBuffer = stackalloc Byte[UIntPtr.Size];
				unchecked
				{
					for (Int32 i = 0; i < addressText.Length / 2; i++)
					{
						addressBuffer[i] = NativeUtilities.GetDecimalValue(addressText[^(2 * i + 2)]);
						addressBuffer[i] <<= 4;
						addressBuffer[i] += NativeUtilities.GetDecimalValue(addressText[^(2 * i + 1)]);
					}
				}
				return MemoryMarshal.Read<UIntPtr>(addressBuffer);
			}
		}
	}
}