#if NETCOREAPP

using System.Text.Json;
#if CSHARP9_0
using System;
using System.Text.Json.Serialization;

#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if NET8_0_OR_GREATER && CSHARP9_0
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
#if !NET8_0_OR_GREATER || !CSHARP9_0
		private static JsonSerializerOptions? options;
#endif

		public static JsonSerializerOptions SerializerOptions
		{
			get
			{
#if NET8_0_OR_GREATER && CSHARP9_0
				return AppJsonSerializerContext.Default.Options;
#else
				return AppJsonSerializerContext.options ??=
#if CSHARP9_0
					new()
#else
					new JsonSerializerOptions
#endif
					{
						Converters = { new CString.JsonConverter(), new CStringSequence.JsonConverter(), },
					};
#endif
			}
		}
	}
}
#endif