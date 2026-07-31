namespace Rxmxnx.PInvoke;

public partial class CStringBuilder
{
	private readonly partial struct Concurrent
	{
		/// <summary>
		/// Appends the UTF-8 representation of the specified character span followed by the default line terminator to
		/// the end of the current instance.
		/// </summary>
		/// <param name="value">The read-only span of characters to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public CStringBuilder AppendLine(ReadOnlySpan<Char> value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value).Append(CString.NewLine);
			return builder;
		}
		/// <summary>
		/// Appends the specified UTF-8 units read-only span followed by the default line terminator to the end of
		/// the current instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only span to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
		public CStringBuilder AppendLine(ReadOnlySpan<Byte> value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value).Append(CString.NewLine);
			return builder;
		}
		/// <summary>
		/// Appends the specified UTF-8 units read-only sequence followed by the default line terminator to the end of
		/// the current instance.
		/// </summary>
		/// <param name="value">The UTF-8 units read-only sequence to append.</param>
		/// <returns>A reference to this instance after the append operation has completed.</returns>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public CStringBuilder AppendLine(ReadOnlySequence<Byte> value)
		{
#if NET9_0_OR_GREATER
			using (lockObj.EnterScope())
#else
			lock (lockObj)
#endif
				builder._chunk = builder._chunk.Append(value).Append(CString.NewLine);
			return builder;
		}
	}
}