namespace Rxmxnx.PInvoke.Internal;

internal partial class MemoryInspector
{
	private partial class Linux
	{
		/// <summary>
		/// Memory boundary struct.
		/// </summary>
		private readonly unsafe struct MemoryBoundary : IEquatable<MemoryBoundary>, IComparable<MemoryBoundary>,
			IWrapper<IntPtr>
#if NET7_0_OR_GREATER
			, IEqualityOperators<MemoryBoundary, MemoryBoundary, Boolean>
			, IComparisonOperators<MemoryBoundary, MemoryBoundary, Boolean>
#endif
		{
			/// <summary>
			/// Internal value.
			/// </summary>
			public IntPtr Value { get; private init; }
			/// <summary>
			/// Indicates whether current boundary is terminal.
			/// </summary>
			public Boolean IsEnd { get; private init; }

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

			/// <summary>
			/// Defines an implicit conversion of a given <see cref="IntPtr"/> to a <see cref="MemoryBoundary"/> instance.
			/// </summary>
			/// <param name="value">An <see cref="IntPtr"/> to implicitly convert.</param>
			public static implicit operator MemoryBoundary(void* value) => new() { Value = new(value), IsEnd = false, };
			/// <summary>
			/// Defines an implicit conversion of a given <see cref="ValueTuple{IntPtr, Boolean}"/> to a <see cref="MemoryBoundary"/>
			/// instance.
			/// </summary>
			/// <param name="value">An <see cref="ValueTuple{IntPtr, Boolean}"/> to implicitly convert.</param>
			public static implicit operator MemoryBoundary((IntPtr, Boolean) value)
				=> new() { Value = value.Item1, IsEnd = value.Item2, };

			/// <inheritdoc/>
			public Int32 CompareTo(MemoryBoundary other) => this.Value.CompareTo(other.Value);
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
#else
				=> left.Value.CompareTo(right.Value) < 0;
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
#else
				=> left.Value.CompareTo(right.Value) <= 0;
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
#else
				=> left.Value.CompareTo(right.Value) > 0;
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
#else
				=> left.Value.CompareTo(right.Value) >= 0;
#endif

			/// <summary>
			/// Parses the span of UTF-8 hexadecimal characters into a <see cref="IntPtr"/> value.
			/// </summary>
			/// <param name="addressText">The span of UTF-8 characters to parse.</param>
			/// <returns>The result of parsing <paramref name="addressText"/>.</returns>
			private static IntPtr Parse(ReadOnlySpan<Byte> addressText)
			{
				unchecked
				{
					Span<Byte> addressBuffer = stackalloc Byte[IntPtr.Size];
					for (Int32 i = 0; i < addressText.Length / 2; i++)
					{
						addressBuffer[i] = MemoryBoundary.GetDecimalValue(addressText[^(2 * i + 2)]);
						addressBuffer[i] <<= 4;
						addressBuffer[i] += MemoryBoundary.GetDecimalValue(addressText[^(2 * i + 1)]);
					}
					return MemoryMarshal.Read<IntPtr>(addressBuffer);
				}
			}
			/// <summary>
			/// Retrieves the decimal value of <paramref name="hexCharacter"/>.
			/// </summary>
			/// <param name="hexCharacter">ASCII hexadecimal character.</param>
			/// <returns>Decimal value from <paramref name="hexCharacter"/>.</returns>
			private static Byte GetDecimalValue(Byte hexCharacter)
				=> hexCharacter switch
				{
					(Byte)'a' => 10,
					(Byte)'A' => 10,
					(Byte)'b' => 11,
					(Byte)'B' => 11,
					(Byte)'c' => 12,
					(Byte)'C' => 12,
					(Byte)'d' => 13,
					(Byte)'D' => 13,
					(Byte)'e' => 14,
					(Byte)'E' => 14,
					(Byte)'f' => 15,
					(Byte)'F' => 15,
					(Byte)'1' => 1,
					(Byte)'2' => 2,
					(Byte)'3' => 3,
					(Byte)'4' => 4,
					(Byte)'5' => 5,
					(Byte)'6' => 6,
					(Byte)'7' => 7,
					(Byte)'8' => 8,
					(Byte)'9' => 9,
					_ => 0,
				};
		}
	}
}