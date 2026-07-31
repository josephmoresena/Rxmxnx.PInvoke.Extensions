namespace Rxmxnx.PInvoke;

public sealed partial class CStringBuilder
{
	/// <inheritdoc cref="CStringBuilder.AppendJoin(CString, CString[])"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendJoin(CString? separator,
#if !NET9_0_OR_GREATER
		params CString?[] values
#else
		CString?[] values
#endif
	)
		=> this.ConcurrentAppendJoin(separator, values.AsSpan());
	/// <inheritdoc cref="CStringBuilder.AppendJoin(ReadOnlySpan{Byte}, ReadOnlySpan{CString})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder ConcurrentAppendJoin(ReadOnlySpan<Byte> separator,
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
		return !enumerator.MoveNext() ? this : new Concurrent(this.GetLock(), this).AppendJoin(separator, enumerator);
	}
	/// <inheritdoc cref="CStringBuilder.AppendJoin(CString, CStringSequence)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendJoin(CString? separator, CStringSequence sequence)
		=> this.ConcurrentAppendJoin(separator, new CStringSequence.Utf8View(sequence, true));
	/// <inheritdoc cref="CStringBuilder.AppendJoin(ReadOnlySpan{Byte}, CStringSequence.Utf8View)"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	// ReSharper disable once MemberCanBePrivate.Global
	public CStringBuilder ConcurrentAppendJoin(ReadOnlySpan<Byte> separator, CStringSequence.Utf8View sequenceView)
	{
#if NET9_0_OR_GREATER
		using CStringSequence.Utf8View.Enumerator enumerator = sequenceView.GetEnumerator();
#else
		CStringSequence.Utf8View.Enumerator enumerator = sequenceView.GetEnumerator();
#endif
		return !enumerator.MoveNext() ? this : new Concurrent(this.GetLock(), this).AppendJoin(separator, enumerator);
	}
	/// <inheritdoc cref="CStringBuilder.AppendJoin(CString, IEnumerable{CString})"/>
	/// <remarks>This operation is thread-safe.</remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public CStringBuilder ConcurrentAppendJoin(CString? separator, IEnumerable<CString?> values)
	{
		using IEnumerator<CString?> enumerator = values.GetEnumerator();
		return !enumerator.MoveNext() ? this : new Concurrent(this.GetLock(), this).AppendJoin(separator, enumerator);
	}
}