namespace Rxmxnx.PInvoke.Internal;

[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
internal unsafe partial class FixedPointer : IEquatable<FixedPointer>
{
	/// <inheritdoc/>
	public virtual Boolean Equals(FixedPointer? other)
		=> other is not null && this.GetMemoryOffset() == other.GetMemoryOffset() &&
			this.BinaryLength == other.BinaryLength && this._isReadOnly == other._isReadOnly;
	/// <inheritdoc/>
	[ExcludeFromCodeCoverage]
	public override Boolean Equals(Object? obj) => this.Equals(obj as FixedPointer);

	/// <inheritdoc/>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public override Int32 GetHashCode()
	{
		HashCode result = new();
		result.Add(new IntPtr(this._ptr));
		result.Add(this.BinaryOffset);
		result.Add(this._binaryLength);
		result.Add(this._isReadOnly);
		if (this.Type is not null)
			result.Add(this.Type);
		return result.ToHashCode();
	}
}