using System;
#if NETCOREAPP2_1_OR_GREATER || NETFRAMEWORK && (MONO || NET461_OR_GREATER) || UAP10_0_16299
using System.Text.Json.Serialization;

#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if !CSHARP9_0 || UAP && !UAP10_0_16299
	public sealed class SerializableMessage<T>
#else
	public sealed record SerializableMessage<T>
#endif
		where T : class, IEquatable<String>, IEquatable<T>, IComparable<String>, IComparable<T>
	{
		public T? Title { get; set; }
		public T? Message { get; set; }

#if NETCOREAPP2_1_OR_GREATER || NETFRAMEWORK && (MONO || NET461_OR_GREATER) || UAP10_0_16299
		[JsonConstructor]
		public SerializableMessage() { }
#endif
	}
}