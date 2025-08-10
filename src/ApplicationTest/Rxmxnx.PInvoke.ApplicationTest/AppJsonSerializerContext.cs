#if NETCOREAPP
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if NET8_0_OR_GREATER
	[JsonSerializable(typeof(String))]
	[JsonSerializable(typeof(CString))]
	[JsonSerializable(typeof(CStringSequence))]
	[JsonSerializable(typeof(String[]))]
	[JsonSerializable(typeof(SerializableMessage<CString>))]
	[JsonSerializable(typeof(SerializableMessage<String>))]
	public partial class AppJsonSerializerContext : JsonSerializerContext
#else
    public static class AppJsonSerializerContext
#endif
	{
		public static JsonSerializerOptions SerializerOptions
#if NET8_0_OR_GREATER
			=> AppJsonSerializerContext.Default.Options;
#else
        {
			get;
		} = new() { Converters = { new CString.JsonConverter(), new CStringSequence.JsonConverter(), }, };
#endif
	}
}
#endif