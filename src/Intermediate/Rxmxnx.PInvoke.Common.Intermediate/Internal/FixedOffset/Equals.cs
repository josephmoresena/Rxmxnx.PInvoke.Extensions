namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedOffset : IEquatable<FixedOffset>
{
	/// <inheritdoc/>
	public Boolean Equals(FixedOffset? other) => this.Equals(other as FixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedOffset);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as FixedOffset);

	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}