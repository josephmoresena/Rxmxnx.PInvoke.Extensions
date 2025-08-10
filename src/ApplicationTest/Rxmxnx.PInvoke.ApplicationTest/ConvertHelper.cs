using System;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

#elif !NETCOREAPP
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

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
#if !NETCOREAPP
            => new()
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
#if !NETCOREAPP
            => new() { Title = (CString?)value.Title, Message = (CString?)value.Message, };
#else
		{
			String serialized = JsonSerializer.Serialize(value, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<SerializableMessage<CString>>(
				serialized, AppJsonSerializerContext.SerializerOptions)!;
		}
#endif
		public static String?[] Convert(CStringSequence sequence)
		{
#if NETCOREAPP
			String serialized = JsonSerializer.Serialize(sequence, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<String?[]>(serialized, AppJsonSerializerContext.SerializerOptions)!;
#else
	        String?[] result = new String?[sequence.Count];
	        Int32 index = 0;
	        foreach (ReadOnlySpan<Byte> utf8Text in sequence.CreateView())
	        {
		        result[index] = !Unsafe.IsNullRef(ref MemoryMarshal.GetReference(utf8Text)) ?
			        Encoding.UTF8.GetString(utf8Text) :
			        default;
		        index++;
	        }
	        return result;
#endif
		}
		public static CStringSequence Convert(params String?[] sequence)
#if !NETCOREAPP
            => new(sequence);
#else
		{
			String serialized = JsonSerializer.Serialize(sequence, AppJsonSerializerContext.SerializerOptions);
			return JsonSerializer.Deserialize<CStringSequence>(serialized, AppJsonSerializerContext.SerializerOptions)!;
		}
#endif
	}
}