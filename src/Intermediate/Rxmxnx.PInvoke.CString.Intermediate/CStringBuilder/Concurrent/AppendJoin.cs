namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private readonly partial struct Concurrent
	{
		/// <summary>
		/// Concatenates the UTF-8 texts of the provided span, using the specified UTF-8 sequence separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="enumerator"/> has more than one element.
		/// </param>
		/// <param name="enumerator">
		/// An enumerator that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		// ReSharper disable once MemberCanBePrivate.Global
		public CStringBuilder AppendJoin(ReadOnlySpan<Byte> separator, ReadOnlySpan<CString?>.Enumerator enumerator)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
			{
				builder._chunk = builder._chunk.Append(enumerator.Current);
				while (enumerator.MoveNext())
				{
					builder._chunk = builder._chunk.Append(separator);
					builder._chunk = builder._chunk.Append(enumerator.Current);
				}
			}
			return builder;
		}
		/// <summary>
		/// Concatenates the UTF-8 texts of the provided sequence, using the specified UTF-8 sequence separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="enumerator"/> has more than one element.
		/// </param>
		/// <param name="enumerator">
		/// An enumerator that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		// ReSharper disable once MemberCanBePrivate.Global
		public CStringBuilder AppendJoin(ReadOnlySpan<Byte> separator, CStringSequence.Utf8View.Enumerator enumerator)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
			{
				builder._chunk = builder._chunk.Append(enumerator.Current);
				while (enumerator.MoveNext())
				{
					builder._chunk = builder._chunk.Append(separator);
					builder._chunk = builder._chunk.Append(enumerator.Current);
				}
			}
			return builder;
		}
		/// <summary>
		/// Concatenates the UTF-8 texts of the provided enumerable, using the specified UTF-8 text separator between
		/// each text, then appends the result to the current instance.
		/// </summary>
		/// <param name="separator">
		/// The UTF-8 sequence to use as a separator. <paramref name="separator"/> is included in the joined text
		/// only if <paramref name="enumerator"/> has more than one element.
		/// </param>
		/// <param name="enumerator">
		/// An enumerator that contains the UTF-8 texts to concatenate and append to the current instance.
		/// </param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public CStringBuilder AppendJoin(CString? separator, IEnumerator<CString?> enumerator)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
			{
				builder._chunk = builder._chunk.Append(enumerator.Current);
				while (enumerator.MoveNext())
				{
					builder._chunk = builder._chunk.Append(separator);
					builder._chunk = builder._chunk.Append(enumerator.Current);
				}
			}
			return builder;
		}
	}
}