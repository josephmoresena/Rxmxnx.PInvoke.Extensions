namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
#if NET7_0_OR_GREATER || !PACKAGE
#if !NET7_0_OR_GREATER
	/// <summary>
	/// Represents a wrapper for a <see cref="CStringSequence"/> instance.
	/// </summary>
#else
	/// <summary>
	/// Represents a wrapper for a <see cref="CStringSequence"/> instance.
	/// </summary>
	/// <remarks>
	/// When marshalling this struct via P/Invoke, all fields are passed as-is; empty sequences are not treated as
	/// null.
	/// </remarks>
	[NativeMarshalling(typeof(InputMarshaller))]
#endif
	[DebuggerDisplay("Count = {Count}")]
	[DebuggerTypeProxy(typeof(CStringSequenceDebugView))]
	public readonly struct Interop : IWrapper<CStringSequence?>, IEquatable<Interop>
	{
		/// <summary>
		/// Internal instance.
		/// </summary>
		public CStringSequence? Value { get; }

		/// <summary>
		/// Internal count.
		/// </summary>
		internal Int32? Count => this.Value?.Count;

		/// <summary>
		/// Private constructor.
		/// </summary>
		/// <param name="instance">A <see cref="CStringSequence"/> instance.</param>
		private Interop(CStringSequence? instance) => this.Value = instance;

		/// <inheritdoc/>
		public Boolean Equals(Interop other) => Object.Equals(this.Value, other.Value);
		/// <inheritdoc/>
		public override Boolean Equals([NotNullWhen(true)] Object? obj)
			=> obj is Interop other ? this.Equals(other) : Object.Equals(this.Value, obj);
		/// <inheritdoc/>
		public override Int32 GetHashCode() => this.Value?.GetHashCode() ?? default;
		/// <inheritdoc/>
		public override String? ToString() => this.Value?.ToString();

		/// <summary>
		/// Defines an implicit conversion of a given <see cref="CStringSequence"/> instance to <see cref="Interop"/>.
		/// </summary>
		/// <param name="value">A <see cref="CStringSequence"/> instance to implicitly convert.</param>
		public static implicit operator Interop?(CStringSequence? value) => new(value);

		/// <summary>
		/// Determines whether two specified <see cref="Interop"/> instances have the same value.
		/// </summary>
		/// <param name="left">The first <see cref="Interop"/> to compare, or <see langword="null"/>.</param>
		/// <param name="right">The second <see cref="Interop"/> to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static Boolean operator ==(Interop left, Interop right) => left.Equals(right);
		/// <summary>
		/// Determines whether two specified <see cref="Interop"/> instances have different values.
		/// </summary>
		/// <param name="left">The first <see cref="Interop"/> to compare, or <see langword="null"/>.</param>
		/// <param name="right">The second <see cref="Interop"/> to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static Boolean operator !=(Interop left, Interop right) => !(left == right);
	}
#endif
}