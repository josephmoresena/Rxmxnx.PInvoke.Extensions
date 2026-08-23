// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

// Adopted and adapted by Joseph Moreno in 2026 based on code from Microsoft.Bcl.Memory

#if !NETSTANDARD2_1 && !NETCOREAPP3_0_OR_GREATER && !NET462_OR_GREATER && !UAP10_0_16299
namespace System;

/// <summary>
/// Represent a type can be used to index a collection either from the start or the end.
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
	/// Create an Index pointing at first element.
	/// </summary>
	public static Index Start => new(0);
	/// <summary>
	/// Create an Index pointing at beyond last element.
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
	/// Construct an Index using a value and indicating if the index is from the start or from the end.
	/// </summary>
	/// <param name="value">The index value. it has to be zero or positive number.</param>
	/// <param name="fromEnd">Indicating if the index is from the start or from the end.</param>
	/// <remarks>
	/// If the Index constructed from the end, index value 1 means pointing at the last element and index value 0 means
	/// pointing at beyond last element.
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
	/// Create an Index from the start at the position indicated by the value.
	/// </summary>
	/// <param name="value">The index value from the start.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Index FromStart(Int32 value)
	{
		ValidationUtilities.ThrowIfNegativeLengthOrIndex(value);
		return new(value);
	}
	/// <summary>
	/// Create an Index from the end at the position indicated by the value.
	/// </summary>
	/// <param name="value">The index value from the end.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Index FromEnd(Int32 value)
	{
		ValidationUtilities.ThrowIfNegativeLengthOrIndex(value);
		return new(~value);
	}

	/// <summary>
	/// Calculate the offset from the start using the giving collection length.
	/// </summary>
	/// <param name="length">
	/// The length of the collection that the Index will be used with. length has to be a positive value
	/// </param>
	/// <remarks>
	/// For performance reason, we don't validate the input length parameter and the returned offset value against negative values.
	/// we don't validate either the returned offset is greater than the input length.
	/// It is expected Index will be used with collections which always have non negative length/count. If the returned
	/// offset is negative and then used to index a collection will get out of range exception which will be same
	/// affect as the validation.
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
	/// Converts a <see cref="Int32"/> to a <see cref="Index"/>.
	/// </summary>
	/// <param name="value">The <see cref="Int32"/> to convert.</param>
	public static implicit operator Index(Int32 value) => Index.FromStart(value);
}
#endif