namespace Rxmxnx.PInvoke;

public partial class CString
{
	/// <summary>
	/// Represents a mutable <see cref="CString"/> instance.
	/// </summary>
	public sealed partial class Builder
	{
		/// <summary>
		/// The default capacity of a <see cref="Builder"/>.
		/// </summary>
		internal const UInt16 DefaultCapacity = 32;

		/// <summary>
		/// Lock object.
		/// </summary>
#if NET9_0_OR_GREATER
		private readonly Lock _lock = new();
#else
		private readonly Object _lock = new();
#endif
		/// <summary>
		/// Initial capacity.
		/// </summary>
		private readonly UInt16 _capacity;
		/// <summary>
		/// Current string chunk.
		/// </summary>
		private Chunk _chunk;

		/// <summary>
		/// Initializes a new instance of the <see cref="Builder"/> class.
		/// </summary>
		public Builder() : this(Builder.DefaultCapacity) { }
		/// <summary>
		/// Initializes a new instance of the <see cref="Builder"/> class using the specified capacity.
		/// </summary>
		/// <param name="capacity">The suggested starting size of this instance.</param>
		public Builder(UInt16 capacity)
		{
			this._capacity = capacity;
			this._chunk = new(capacity);
		}

		/// <summary>
		/// Concatenates the UTF-8 texts of the provided span, using the specified UTF-8 sequence separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="values"/> has more than one element.
		/// </param>
		/// <param name="values">
		/// A sequence that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendJoin(ReadOnlySpan<Byte> separator,
#if !NET9_0_OR_GREATER
			params CString?[] values
#else
			CString?[] values
#endif
		)
			=> this.AppendJoin(separator, values.AsSpan());
		/// <summary>
		/// Concatenates the UTF-8 texts of the provided span, using the specified UTF-8 sequence separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="values"/> has more than one element.
		/// </param>
		/// <param name="values">
		/// A sequence that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendJoin(ReadOnlySpan<Byte> separator,
#if NET9_0_OR_GREATER
			params ReadOnlySpan<CString?> values
#else
			ReadOnlySpan<CString?> values
#endif
		)
		{
			ReadOnlySpan<CString?>.Enumerator enumerator = values.GetEnumerator();
			if (!enumerator.MoveNext()) return this;
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
			{
				this._chunk = this._chunk.Append(enumerator.Current);
				while (enumerator.MoveNext())
				{
					this._chunk = this._chunk.Append(separator);
					this._chunk = this._chunk.Append(enumerator.Current);
				}
			}
			return this;
		}
		/// <summary>
		/// Concatenates the UTF-8 texts of the provided sequence, using the specified UTF-8 sequence separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="sequence"/> has more than one element.
		/// </param>
		/// <param name="sequence">
		/// A sequence that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendJoin(ReadOnlySpan<Byte> separator, CStringSequence sequence)
			=> this.AppendJoin(separator, new CStringSequence.Utf8View(sequence, true));
		/// <summary>
		/// Concatenates the UTF-8 texts of the provided sequence, using the specified UTF-8 sequence separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="sequenceView"/> has more than one element.
		/// </param>
		/// <param name="sequenceView">
		/// A sequence that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendJoin(ReadOnlySpan<Byte> separator, CStringSequence.Utf8View sequenceView)
		{
#if NET9_0_OR_GREATER
			using CStringSequence.Utf8View.Enumerator enumerator = sequenceView.GetEnumerator();
#else
			CStringSequence.Utf8View.Enumerator enumerator = sequenceView.GetEnumerator();
#endif
			if (!enumerator.MoveNext()) return this;
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
			{
				this._chunk = this._chunk.Append(enumerator.Current);
				while (enumerator.MoveNext())
				{
					this._chunk = this._chunk.Append(separator);
					this._chunk = this._chunk.Append(enumerator.Current);
				}
			}
			return this;
		}
		/// <summary>
		/// Appends each non-empty UTF-8 text in <paramref name="sequence"/>.
		/// </summary>
		/// <param name="sequence">A UTF-8 text sequence to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Append(CStringSequence sequence)
		{
			CStringSequence.Utf8View view = new(sequence, false);
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
			{
				foreach (ReadOnlySpan<Byte> value in view)
					this._chunk = this._chunk.Append(value);
			}
			return this;
		}
		/// <summary>
		/// Appends a UTF-8 text to this instance.
		/// </summary>
		/// <param name="value">The read-only byte span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Append(CString value) => this.Append(value.AsSpan());
		/// <summary>
		/// Appends the UTF-8 representation of <paramref name="value"/>.
		/// </summary>
		/// <param name="value">The read-only character span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Append(String value) => this.Append(value.AsSpan());
		/// <summary>
		/// Appends the UTF-8 representation of a specified read-only byte span to this instance.
		/// </summary>
		/// <param name="value">The read-only byte span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Append(ReadOnlySpan<Byte> value)
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk = this._chunk.Append(value);
			return this;
		}
		/// <summary>
		/// Appends the UTF-8 representation of a specified read-only character span to this instance.
		/// </summary>
		/// <param name="value">The read-only character span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Append(ReadOnlySpan<Char> value)
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk = this._chunk.Append(Encoding.UTF8.GetByteCount(value), value);
			return this;
		}
		/// <summary>
		/// Appends the default line terminator to the end of the current instance.
		/// </summary>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine()
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk = this._chunk.Append(CString.NewLine);
			return this;
		}
		/// <summary>
		/// Appends the UTF-8 representation of a specified string followed by the default line terminator to the end of
		/// the current instance.
		/// </summary>
		/// <param name="value">The string to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder AppendLine(String? value)
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
			{
				ReadOnlySpan<Char> chars = value;
				this._chunk = this._chunk.Append(Encoding.UTF8.GetByteCount(chars), chars).Append(CString.NewLine);
			}
			return this;
		}
		/// <summary>
		/// Removes all units from the current <see cref="Builder"/> instance.
		/// </summary>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public Builder Clear()
		{
#if NET9_0_OR_GREATER
			using (this._lock.EnterScope())
#else
			lock (this._lock)
#endif
				this._chunk.Reset(this._capacity);
			return this;
		}
	}
}