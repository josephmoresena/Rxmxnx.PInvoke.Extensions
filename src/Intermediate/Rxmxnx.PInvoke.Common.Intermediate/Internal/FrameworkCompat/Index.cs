// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Bcl.Memory

// ReSharper disable MemberCanBePrivate.Global

#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER && !NET462_OR_GREATER && !UAP10_0_16299
namespace System;

/// <summary>
/// Represents a type that can be used to index a collection either from the start or from the end.
/// </summary>
/// <remarks>
/// Index is used by the C# compiler to support the new index syntax
/// <code>
/// int[] someArray = new int[5] { 1, 2, 3, 4, 5 } ;
/// int lastElement = someArray[^1]; // lastElement = 5
/// </code>
/// </remarks>
#if !PACKAGE
[ExcludeFromCodeCoverage]
#endif
internal readonly struct Index : IEquatable<Index>
{
	/// <summary>
	/// Internal value.
	/// </summary>
	private readonly Int32 _value;

	/// <summary>
	/// Creates an Index that points at the first element.
	/// </summary>
	public static Index Start => new(0);
	/// <summary>
	/// Creates an Index that points beyond the last element.
	/// </summary>
	public static Index End => new(~0);

	/// <summary>
	/// Returns the index value.
	/// </summary>
	public Int32 Value => this._value < 0 ? ~this._value : this._value;
	/// <summary>
	/// Indicates whether the index is from the start or the end.
	/// </summary>
	public Boolean IsFromEnd => this._value < 0;

	/// <summary>
	/// Constructs an Index using a value and indicating whether the index is from the start or from the end.
	/// </summary>
	/// <param name="value">The index value. It must be zero or a positive number.</param>
	/// <param name="fromEnd">
	/// <see langword="true"/> if the index is from the end; otherwise, the index is from the start.
	/// </param>
	/// <remarks>
	/// If the Index is constructed from the end, index value 1 points at the last element and index value 0 points
	/// beyond the last element.
	/// </remarks>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3427)]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Index(Int32 value, Boolean fromEnd = false)
	{
		ValidationUtilities.ThrowIfNegativeLengthOrIndex(value);
		this._value = fromEnd ? ~value : value;
	}

	// The following private constructors mainly created for perf reason to avoid the checks
	private Index(Int32 value) => this._value = value;

	/// <summary>
	/// Creates an Index from the start at the position indicated by the value.
	/// </summary>
	/// <param name="value">The index value from the start.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Index FromStart(Int32 value)
	{
		ValidationUtilities.ThrowIfNegativeLengthOrIndex(value);
		return new(value);
	}
	/// <summary>
	/// Creates an Index from the end at the position indicated by the value.
	/// </summary>
	/// <param name="value">The index value from the end.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Index FromEnd(Int32 value)
	{
		ValidationUtilities.ThrowIfNegativeLengthOrIndex(value);
		return new(~value);
	}

	/// <summary>
	/// Calculates the offset from the start using the given collection length.
	/// </summary>
	/// <param name="length">
	/// The length of the collection that the <see cref="Index"/> will be used with. <paramref name="length"/> must be a
	/// positive value.
	/// </param>
	/// <remarks>
	/// For performance reasons, the input length and the returned offset are not validated against negative values.
	/// The method also does not validate that the returned offset is greater than the input length.
	/// <see cref="Index"/> is expected to be used with collections that always have a non-negative length or count.
	/// If the returned offset is negative and is then used to index a collection, an out-of-range exception is thrown,
	/// with the same effect as validation.
	/// </remarks>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public Int32 GetOffset(Int32 length)
	{
		Int32 offset = this._value;
		if (this.IsFromEnd)
		{
			// offset = length - (~value)
			// offset = length + (~(~value) + 1)
			// offset = length + value + 1

			offset += length + 1;
		}
		return offset;
	}

	/// <inheritdoc/>
	public override String ToString() => this.IsFromEnd ? $"^{this.Value}" : ((UInt32)this.Value).ToString();
	/// <inheritdoc/>
	public Boolean Equals(Index other) => this._value == other._value;
	/// <inheritdoc/>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS927)]
#endif
	public override Boolean Equals([NotNullWhen(true)] Object? value)
		=> value is Index index && this._value == index._value;
	/// <inheritdoc/>
	public override Int32 GetHashCode() => this._value;

	/// <summary>
	/// Converts an <see cref="Int32"/> to an <see cref="Index"/>.
	/// </summary>
	/// <param name="value">The <see cref="Int32"/> to convert.</param>
	public static implicit operator Index(Int32 value) => Index.FromStart(value);
}
#endif