using System;
#if NETCOREAPP
using System.Text.Json.Serialization;
#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
	public sealed record SerializableMessage<T>
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