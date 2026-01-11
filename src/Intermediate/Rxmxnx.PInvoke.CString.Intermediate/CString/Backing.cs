namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Backing class for <see cref="CString"/> instances.
	/// </summary>
	public abstract class Backing : ValueRegion<Byte>.Memory
	{
		/// Indicates whether the total length of
		/// <see cref="Backing.Value"/>
		/// should be used.
		private readonly Boolean _useFullLength;

		/// <inheritdoc/>
		protected sealed override ReadOnlyMemory<Byte> Value { get; }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="memory">A <see cref="ReadOnlyMemory{Byte}"/> instance.</param>
		/// <param name="useFullLength">
		/// Indicates whether the total length of <see cref="Backing.Value"/> should be used.
		/// </param>
		protected Backing(ReadOnlyMemory<Byte> memory, Boolean useFullLength)
		{
			this.Value = memory;
			this._useFullLength = useFullLength;
		}

		/// <summary>
		/// Defines an implicit conversion of a given <see cref="Backing"/> instance to <see cref="CString"/>.
		/// </summary>
		/// <param name="backing">A <see cref="Backing"/> instance to implicitly convert.</param>
		[return: NotNullIfNotNull(nameof(backing))]
		public static implicit operator CString?(Backing? backing)
		{
			if (backing is null) return default;
			if (backing._useFullLength)
				return new(backing, false, false, backing.Value.Length);
			Boolean isNullTerminated = CString.IsNullTerminatedSpan(backing.Value.Span, out Int32 length);
			return new(backing, false, isNullTerminated, length);
		}
		/// <summary>
		/// Defines an explicit conversion of a given <see cref="CString"/> instance to <see cref="Backing"/>.
		/// </summary>
		/// <param name="value">A <see cref="CString"/> instance to explicitly convert.</param>
		public static explicit operator Backing?(CString? value) => value?._data as Backing;

		/// <summary>
		/// Attempts to retrieve the <typeparamref name="TBacking"/> instance from <paramref name="value"/>.
		/// </summary>
		/// <typeparam name="TBacking">A <see cref="Backing"/> type.</typeparam>
		/// <param name="value">A <see cref="CString"/> instance.</param>
		/// <param name="backing">Output. A <see cref="Backing"/> instance.</param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="value"/> is backed by a <typeparamref name="TBacking"/> instance;
		/// otherwise, <see langword="false"/>.
		/// </returns>
		protected static Boolean TryGetBacking<TBacking>(CString? value, out TBacking backing) where TBacking : Backing
		{
			if (value?._data is TBacking result)
			{
				backing = result;
				return true;
			}
			Unsafe.SkipInit(out backing);
			return false;
		}
		/// <summary>
		/// Tries to retrieve a <see cref="ReadOnlyMemory{T}"/> instance representing <paramref name="value"/>.
		/// </summary>
		/// <param name="value">A <see cref="CString"/> instance.</param>
		/// <param name="includeNullTermination">
		/// Indicates whether the null-termination should be included on <paramref name="memory"/>.
		/// </param>
		/// <param name="memory">
		/// Output. A <see cref="ReadOnlyMemory{T}"/> instance representing <paramref name="value"/>.
		/// </param>
		/// <returns>
		/// <see langword="true"/> if <paramref name="memory"/> instance represents <paramref name="value"/>; otherwise,
		/// <see langword="false"/>.
		/// </returns>
		protected static Boolean TryGetMemory(CString? value, Boolean includeNullTermination,
			out ReadOnlyMemory<Byte> memory)
		{
			if (value is null)
			{
				memory = default;
				return true;
			}
			if (value._data.TryGetMemory(out ReadOnlyMemory<Byte> valueMemory))
			{
				memory = includeNullTermination ? valueMemory : valueMemory[..value.Length];
				return true;
			}
			if (CString.IsNullOrEmpty(value) && includeNullTermination)
			{
				memory = default;
				return true;
			}
			Unsafe.SkipInit(out memory);
			return false;
		}
	}
}