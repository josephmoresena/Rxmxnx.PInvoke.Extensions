using System;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
#elif !NETCOREAPP2_1_OR_GREATER && (!NETFRAMEWORK || !MONO && !NET461_OR_GREATER) && !UAP10_0_16299
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
#if !NETCOREAPP2_0_OR_GREATER && !NET46_OR_GREATER && !UAP
using System.Text;

#endif

#else
using System.Text.Json;

#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("Trimming", "IL2026")]
	[UnconditionalSuppressMessage("Trimming", "IL2091")]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
#endif
	internal static class ConvertHelper
	{
		public static SerializableMessage<String> Convert(SerializableMessage<CString> value)
#if !NETCOREAPP2_1_OR_GREATER && (!NETFRAMEWORK || !MONO && !NET461_OR_GREATER) && !UAP10_0_16299
#if !CSHARP9_0
            => new SerializableMessage<String>
#else
			=> new()
#endif
			{
				Title = value.Title?.ToString(), Message = value.Message?.ToString(),
			};
#else
		{
			String serialized = JsonSerializer.Serialize(value, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<SerializableMessage<String>>(
				serialized, AppJsonSerializerContext.SerializerOptions)!;
		}
#endif
		public static SerializableMessage<CString> Convert(SerializableMessage<String> value)
#if !NETCOREAPP2_1_OR_GREATER && (!NETFRAMEWORK || !MONO && !NET461_OR_GREATER) && !UAP10_0_16299
#if !CSHARP9_0
            => new SerializableMessage<CString> { Title = (CString?)value.Title, Message = (CString?)value.Message, };
#else
			=> new() { Title = (CString?)value.Title, Message = (CString?)value.Message, };
#endif
#else
		{
			String serialized = JsonSerializer.Serialize(value, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<SerializableMessage<CString>>(
				serialized, AppJsonSerializerContext.SerializerOptions)!;
		}
#endif
		public static String?[] Convert(CStringSequence sequence)
		{
#if NETCOREAPP2_1_OR_GREATER || NETFRAMEWORK && (MONO || NET461_OR_GREATER) || UAP10_0_16299
			String serialized = JsonSerializer.Serialize(sequence, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<String?[]>(serialized, AppJsonSerializerContext.SerializerOptions)!;
#else
			String?[] result = new String?[sequence.Count];
			Int32 index = 0;
			foreach (ReadOnlySpan<Byte> utf8Text in sequence.CreateView())
			{
				result[index] = !Unsafe.IsNullRef(ref MemoryMarshal.GetReference(utf8Text)) ?
#if NETCOREAPP2_0_OR_GREATER || NET46_OR_GREATER || UAP 
					utf8Text.ToUtf16() :
#elif NET452
					Encoding.UTF8.GetString(utf8Text.ToArray()) :
#else
			        Encoding.UTF8.GetString(utf8Text) :
#endif
					default;
				index++;
			}
			return result;
#endif
		}
		public static CStringSequence Convert(params String?[] sequence)
#if !NETCOREAPP2_1_OR_GREATER && (!NETFRAMEWORK || !MONO && !NET461_OR_GREATER) && !UAP10_0_16299
#if !CSHARP9_0
            => new CStringSequence(sequence);
#else
			=> new(sequence);
#endif
#else
		{
			String serialized = JsonSerializer.Serialize(sequence, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<CStringSequence>(serialized, AppJsonSerializerContext.SerializerOptions)!;
		}
#endif
	}
}