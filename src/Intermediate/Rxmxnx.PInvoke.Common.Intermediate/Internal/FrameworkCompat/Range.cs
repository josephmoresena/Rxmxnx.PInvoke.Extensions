// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Bcl.Memory

#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER && !NET462_OR_GREATER
namespace System;

/// <summary>
/// Represent a range has start and end indexes.
/// </summary>
/// <remarks>
/// Range is used by the C# compiler to support the range syntax.
/// <code>
/// int[] someArray = new int[5] { 1, 2, 3, 4, 5 };
/// int[] subArray1 = someArray[0..2]; // { 1, 2 }
/// int[] subArray2 = someArray[1..^0]; // { 2, 3, 4, 5 }
/// </code>
/// </remarks>
internal readonly struct Range : IEquatable<Range>
{
	/// <summary>
	/// Create a Range object starting from first element to the end.
	/// </summary>
	public static Range All => new(Index.Start, Index.End);

	/// <summary>
	/// Represent the inclusive start index of the Range.
	/// </summary>
	public Index Start { get; }
	/// <summary>
	/// Represent the exclusive end index of the Range.
	/// </summary>
	public Index End { get; }

	/// <summary>Construct a Range object using the start and end indexes.</summary>
	/// <param name="start">Represent the inclusive start index of the range.</param>
	/// <param name="end">Represent the exclusive end index of the range.</param>
	public Range(Index start, Index end)
	{
		this.Start = start;
		this.End = end;
	}

	/// <inheritdoc/>
	public override Boolean Equals([NotNullWhen(true)] Object? value)
		=> value is Range r && r.Start.Equals(this.Start) && r.End.Equals(this.End);
	/// <inheritdoc/>
	public Boolean Equals(Range other) => other.Start.Equals(this.Start) && other.End.Equals(this.End);
	/// <inheritdoc/>
	public override Int32 GetHashCode() => HashCode.Combine(this.Start.GetHashCode(), this.End.GetHashCode());
	/// <inheritdoc/>
	public override String ToString() => $"{this.Start}..{this.End}";

	/// <summary>
	/// Create a Range object starting from start index to the end of the collection.
	/// </summary>
	public static Range StartAt(Index start) => new(start, Index.End);

	/// <summary>
	/// Create a Range object starting from first element in the collection to the end Index.
	/// </summary>
	public static Range EndAt(Index end) => new(Index.Start, end);

	/// <summary>
	/// Calculate the start offset and length of range object using a collection length.
	/// </summary>
	/// <param name="length">
	/// The length of the collection that the range will be used with. length has to be a positive value.
	/// </param>
	/// <remarks>
	/// For performance reason, we don't validate the input length parameter against negative values.
	/// It is expected Range will be used with collections which always have non negative length/count.
	/// We validate the range is inside the length scope though.
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