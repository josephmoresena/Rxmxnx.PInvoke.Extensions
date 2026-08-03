namespace Rxmxnx.PInvoke.Internal;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
internal unsafe partial class FixedPointer : IEquatable<FixedPointer>
{
#if !NETSTANDARD2_0_OR_GREATER && !NETCOREAPP2_0_OR_GREATER && !NET461_OR_GREATER && !UAP10_0_16299
	/// <summary>
	/// Internal seed for HashCode.
	/// </summary>
	private static readonly Int32 hashSeed = Guid.NewGuid().GetHashCode();
#endif
	
	/// <inheritdoc/>
	public virtual Boolean Equals(FixedPointer? other)
		=> other is not null && this.GetMemoryOffset() == other.GetMemoryOffset() &&
			this.BinaryLength == other.BinaryLength && this.IsReadOnly == other.IsReadOnly;
	/// <inheritdoc/>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public override Boolean Equals(Object? obj) => this.Equals(obj as FixedPointer);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override Int32 GetHashCode()
	{
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP2_0_OR_GREATER || NET461_OR_GREATER || UAP10_0_16299
		HashCode result = new();
		result.Add(new IntPtr(this._ptr));
		result.Add(this.BinaryOffset);
		result.Add(this._binaryLength);
		result.Add(this.IsReadOnly);
		if (this.Type is not null)
			result.Add(this.Type);
		return result.ToHashCode();
#else
		Int32 hash = FixedPointer.hashSeed;
		unchecked
		{
			hash = hash * 31 + new IntPtr(this._ptr).GetHashCode();
			hash = hash * 31 + this.BinaryOffset;
			hash = hash * 31 + this._binaryLength;
			hash = hash * 31 + (this.IsReadOnly ? 1 : 0);
			if (this.Type is not null)
				hash = hash * 31 + this.Type.GetHashCode();
		}
		return hash;
#endif
	}
}