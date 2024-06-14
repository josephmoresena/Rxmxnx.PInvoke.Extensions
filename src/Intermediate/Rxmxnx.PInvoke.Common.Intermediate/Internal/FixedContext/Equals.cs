namespace Rxmxnx.PInvoke.Internal;

internal partial class FixedContext<T> : IEquatable<FixedContext<T>>
{
	/// <inheritdoc/>
	public Boolean Equals(FixedContext<T>? other) => this.Equals(other as FixedMemory);
	/// <inheritdoc/>
	public override Boolean Equals(FixedMemory? other) => base.Equals(other as FixedContext<T>);
	/// <inheritdoc/>
	public override Boolean Equals(Object? obj) => base.Equals(obj as FixedContext<T>);
}