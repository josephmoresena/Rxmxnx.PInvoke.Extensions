namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	public readonly ref partial struct Utf8View
	{
		/// <inheritdoc/>
		public override String? ToString() => this._instance?._value;
		/// <inheritdoc/>
		public override Int32 GetHashCode() => this._instance?.GetHashCode() ?? default;
		/// <inheritdoc/>
		public override Boolean Equals([NotNullWhen(true)] Object? obj) => Object.Equals(obj, this._instance);

		/// <summary>
		/// Determines whether two specified <see cref="Utf8View"/> instances have the same value.
		/// </summary>
		/// <param name="left">The first <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <param name="right">The second <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="left"/> is the same as the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static Boolean operator ==(Utf8View left, Utf8View right)
			=> Object.Equals(left._instance, right._instance) && left._excludeEmptyItems == right._excludeEmptyItems;
		/// <summary>
		/// Determines whether two specified <see cref="Utf8View"/> instances have different values.
		/// </summary>
		/// <param name="left">The first <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <param name="right">The second <see cref="Utf8View"/> to compare, or <see langword="null"/>.</param>
		/// <returns>
		/// <see langword="true"/> if the value of <paramref name="left"/> is different from the value
		/// of <paramref name="right"/>; otherwise, <see langword="false"/>.
		/// </returns>
		public static Boolean operator !=(Utf8View left, Utf8View right) => !(left == right);
	}
}