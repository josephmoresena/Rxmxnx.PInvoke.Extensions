using System;
#if NETCOREAPP
using System.Text.Json.Serialization;
#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if !CSHARP_90
	public sealed class SerializableMessage<T>
#else
	public sealed record SerializableMessage<T>
#endif
		where T : class, IEquatable<String>, IEquatable<T>, IComparable<String>, IComparable<T>
	{
		public T? Title { get; set; }
		public T? Message { get; set; }

#if NETCOREAPP
		[JsonConstructor]
		public SerializableMessage() { }
#endif
	}
}