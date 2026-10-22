// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Bcl.Memory

// ReSharper disable MemberCanBePrivate.Global

#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER && !NET462_OR_GREATER && !UAP10_0_16299
namespace System;

/// <summary>
/// Represents a range that has start and end indexes.
/// </summary>
/// <remarks>
/// Range is used by the C# compiler to support the range syntax.
/// <code>
/// int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
/// int[] subArray1 = someArray[0..2]; // { 1, 2 }
/// int[] subArray2 = someArray[1..^0]; // { 2, 3, 4, 5 }
/// </code>
/// </remarks>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct Range : IEquatable<Range>
{
	/// <summary>
	/// Creates a Range that starts at the first element and ends at the end.
	/// </summary>
	public static Range All => new(Index.Start, Index.End);

	/// <summary>
	/// Represents the inclusive start index of the Range.
	/// </summary>
	public Index Start { get; }
	/// <summary>
	/// Represents the exclusive end index of the Range.
	/// </summary>
	public Index End { get; }

	/// <summary>Constructs a Range using the start and end indexes.</summary>
	/// <param name="start">The inclusive start index of the range.</param>
	/// <param name="end">The exclusive end index of the range.</param>
	public Range(Index start, Index end)
	{
		this.Start = start;
		this.End = end;
	}

	/// <inheritdoc/>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS927)]
#endif
	public override Boolean Equals([NotNullWhen(true)] Object? value)
		=> value is Range r && r.Start.Equals(this.Start) && r.End.Equals(this.End);
	/// <inheritdoc/>
	public Boolean Equals(Range other) => other.Start.Equals(this.Start) && other.End.Equals(this.End);
	/// <inheritdoc/>
	public override Int32 GetHashCode()
		=> HashCode.Combine(this.Start.GetHashCode(), this.End.GetHashCode());
	/// <inheritdoc/>
	public override String ToString() => $"{this.Start}..{this.End}";

	/// <summary>
	/// Creates a Range that starts at the specified index and ends at the end of the collection.
	/// </summary>
	public static Range StartAt(Index start) => new(start, Index.End);

	/// <summary>
	/// Creates a Range that starts at the first element and ends at the specified index.
	/// </summary>
	public static Range EndAt(Index end) => new(Index.Start, end);

	/// <summary>
	/// Calculates the start offset and length of the range for a collection of the specified length.
	/// </summary>
	/// <param name="length">
	/// The length of the collection that the range will be used with. <paramref name="length"/> must be a positive
	/// value.
	/// </param>
	/// <remarks>
	/// For performance reasons, the input length is not validated against negative values.
	/// <see cref="Range"/> is expected to be used with collections that always have a non-negative length or count.
	/// The method does validate that the range lies within that length.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public (Int32 Offset, Int32 Length) GetOffsetAndLength(Int32 length)
	{
		Int32 start = this.Start.GetOffset(length);
		Int32 end = this.End.GetOffset(length);
		if ((UInt32)end > (UInt32)length || (UInt32)start > (UInt32)end)
			throw new ArgumentOutOfRangeException(nameof(length));
		return (start, end - start);
	}
}
#endif