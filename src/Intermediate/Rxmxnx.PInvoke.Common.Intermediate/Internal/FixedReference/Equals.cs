namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedReference<T> : IEquatable<FixedReference<T>>
{
	/// <inheritdoc/>
	public Boolean Equals(FixedReference<T>? other) => this.Equals(other as FixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedReference<T>);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as FixedReference<T>);

	/// <inheritdoc/>
	public override Int32 GetHashCode() => base.GetHashCode();
}