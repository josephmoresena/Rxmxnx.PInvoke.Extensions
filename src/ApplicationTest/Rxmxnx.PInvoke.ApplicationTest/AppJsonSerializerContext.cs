#if NETCOREAPP || NETFRAMEWORK || UAP
using System.Text.Json;
#if CSHARP9_0 && NET8_0_OR_GREATER && CSHARP9_0
using System;
using System.Text.Json.Serialization;

#endif
#if !NETCOREAPP2_1_OR_GREATER && !NET461_OR_GREATER && !UAP10_0_16299
using CStringJsonConverter = Rxmxnx.PInvoke.Json.CStringJsonConverter;
using CStringSequenceJsonConverter = Rxmxnx.PInvoke.Json.CStringSequenceJsonConverter;

#elif !NET8_0_OR_GREATER || !CSHARP9_0
using CStringJsonConverter = Rxmxnx.PInvoke.CString.JsonConverter;
using CStringSequenceJsonConverter = Rxmxnx.PInvoke.CStringSequence.JsonConverter;

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
						Converters = { new CStringJsonConverter(), new CStringSequenceJsonConverter(), },
					};
#endif
			}
		}
	}
}
#endif