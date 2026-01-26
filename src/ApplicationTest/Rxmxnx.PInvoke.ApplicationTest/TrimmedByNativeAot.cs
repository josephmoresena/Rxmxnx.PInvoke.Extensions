#if NET6_0_OR_GREATER
using System;
using System.IO;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal sealed class TrimmedByNativeAot
	{
		private readonly TextWriter _writer;

		public TrimmedByNativeAot(TextWriter writer) => this._writer = writer;

		public void WriteUtf8(ReadOnlySpan<Char> utf16)
		{
			this._writer.WriteLine("==== Native AOT Trim ====");
			this._writer.WriteLine((CString)utf16.ToString());
		}
	}
}
#endif