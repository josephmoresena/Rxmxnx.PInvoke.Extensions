namespace Rxmxnx.PInvoke.Internal;

internal partial class ReadOnlyFixedOffset : IEquatable<ReadOnlyFixedOffset>
{
	/// <inheritdoc/>
	public Boolean Equals(ReadOnlyFixedOffset? other) => this.Equals(other as ReadOnlyFixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(ReadOnlyFixedMemory? other) => base.Equals(other as ReadOnlyFixedOffset);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as ReadOnlyFixedOffset);

	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}