namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <summary>
	/// Concatenates the UTF-8 texts of the provided span, using the specified UTF-8 sequence separator between
	/// each text, then appends the result to the current instance.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 text to use as a separator. <paramref name="separator"/> is included in the joined text
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// A sequence that contains the UTF-8 texts to concatenate and append to the current instance.
	/// </param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendJoin(CString? separator,
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
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder AppendJoin(ReadOnlySpan<Byte> separator,
#if NET9_0_OR_GREATER
		params ReadOnlySpan<CString?> values
#else
		ReadOnlySpan<CString?> values
#endif
	)
	{
#if NET10_0_OR_GREATER
		using ReadOnlySpan<CString?>.Enumerator enumerator = values.GetEnumerator();
#else
		ReadOnlySpan<CString?>.Enumerator enumerator = values.GetEnumerator();
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
	/// Concatenates the UTF-8 texts of the provided sequence, using the specified UTF-8 sequence separator between
	/// each text, then appends the result to the current instance.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 text to use as a separator. <paramref name="separator"/> is included in the joined text
	/// only if <paramref name="sequence"/> has more than one element.
	/// </param>
	/// <param name="sequence">
	/// A sequence that contains the UTF-8 texts to concatenate and append to the current instance.
	/// </param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendJoin(CString? separator, CStringSequence sequence)
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
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder AppendJoin(ReadOnlySpan<Byte> separator, CStringSequence.Utf8View sequenceView)
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
	/// Concatenates the UTF-8 texts of the provided enumerable, using the specified UTF-8 text separator between
	/// each text, then appends the result to the current instance.
	/// </summary>
	/// <param name="separator">
	/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
	/// only if <paramref name="values"/> has more than one element.
	/// </param>
	/// <param name="values">
	/// A collection that contains the UTF-8 texts to concatenate and append to the current instance.
	/// </param>
	/// <returns>A reference to this instance after the append operation has completed.</returns>
	public CStringBuilder AppendJoin(CString? separator, IEnumerable<CString?> values)
	{
		using IEnumerator<CString?> enumerator = values.GetEnumerator();
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
}