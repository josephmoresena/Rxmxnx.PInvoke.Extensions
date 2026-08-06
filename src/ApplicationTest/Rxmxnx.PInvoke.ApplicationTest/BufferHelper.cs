using System;
using System.IO;
using System.Linq;

using Rxmxnx.PInvoke.Buffers;
#if NET5_0_OR_GREATER
using System.Diagnostics.CodeAnalysis;

#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
#if NET5_0_OR_GREATER
	[UnconditionalSuppressMessage("Trimming", "IL2026")]
	[UnconditionalSuppressMessage("Trimming", "IL2091")]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
#endif
	internal static class BufferHelper
	{
		public static readonly Action<TextWriter> RegisterMetadataObject = BufferHelper
			.GetRegister<Composite<Atomic<Object>, Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>>();
		public static readonly Action<TextWriter> RegisterMetadataValue = BufferHelper
			.GetRegister<Composite<Atomic<ValueTuple<Int32, String>>, Composite<
					Composite<Atomic<ValueTuple<Int32, String>>, Atomic<ValueTuple<Int32, String>>,
						ValueTuple<Int32, String>>,
					Composite<Atomic<ValueTuple<Int32, String>>, Atomic<ValueTuple<Int32, String>>,
						ValueTuple<Int32, String>>, ValueTuple<Int32, String>>, ValueTuple<Int32, String>>,
				ValueTuple<Int32, String>>();
		public static readonly Action<TextWriter> RegisterMetadataNullableValue = BufferHelper
			.GetNullableRegister<Composite<Atomic<ValueTuple<Int32, String>?>, Composite<
					Composite<Atomic<ValueTuple<Int32, String>?>, Atomic<ValueTuple<Int32, String>?>,
						ValueTuple<Int32, String>?>,
					Composite<Atomic<ValueTuple<Int32, String>?>, Atomic<ValueTuple<Int32, String>?>,
						ValueTuple<Int32, String>?>, ValueTuple<Int32, String>?>, ValueTuple<Int32, String>?>,
				ValueTuple<Int32, String>>();

		public static void CollectGarbage(TextWriter writer)
		{
			writer.WriteLine("Begin GC.Collect()");
			GC.Collect();
#if NETFRAMEWORK || NETCOREAPP
			if (!SystemInfo.IsMonoRuntime)
				GC.WaitForFullGCComplete();
			else
				GC.WaitForPendingFinalizers();
#elif UAP10_0_16299
			GC.WaitForFullGCComplete();
#else
			GC.WaitForPendingFinalizers();
#endif
			writer.WriteLine("End GC.Collect()");
		}
		public static void Generate(ScopedBuffer<Int32> buff, TextWriter writer)
		{
			BufferHelper.PrintBufferInfo(buff, writer);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = RuntimeHelper.Shared.Next();

			BufferHelper.Print<Int32>(buff.Span, writer);
			BufferHelper.CollectGarbage(writer);
			BufferHelper.Print<Int32>(buff.Span, writer);
#if NET9_0_OR_GREATER
			RefStructHelper.PointerFeature(buff.Span, buff.InStack, writer);
#endif
		}
		public static void Generate(ScopedBuffer<String?> buff, TextWriter writer)
		{
			BufferHelper.PrintBufferInfo(buff, writer);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = $"Index: {i} Value: {Guid.NewGuid()}";

			BufferHelper.Print<String?>(buff.Span, writer);
			BufferHelper.CollectGarbage(writer);
			BufferHelper.Print<String?>(buff.Span, writer);
#if NET9_0_OR_GREATER
			RefStructHelper.PointerFeature(buff.Span, buff.InStack, writer);
#endif
		}
		public static void Generate(ScopedBuffer<Double?> buff, TextWriter writer)
		{
			BufferHelper.PrintBufferInfo(buff, writer);
			for (Int32 i = 0; i < buff.Span.Length; i++)
#if !CSHARP9_0
				buff.Span[i] = RuntimeHelper.Shared.Next(0, 5) >= 2 ? (Double?)RuntimeHelper.Shared.NextDouble() : null;
#else
				buff.Span[i] = RuntimeHelper.Shared.Next(0, 5) >= 2 ? RuntimeHelper.Shared.NextDouble() : null;
#endif

			BufferHelper.Print<Double?>(buff.Span, writer);
			BufferHelper.CollectGarbage(writer);
			BufferHelper.Print<Double?>(buff.Span, writer);
#if NET9_0_OR_GREATER
			RefStructHelper.PointerFeature(buff.Span, buff.InStack, writer);
#endif
		}
		public static void Generate(ScopedBuffer<(Int32, String)> buff, TextWriter writer)
		{
			BufferHelper.PrintBufferInfo(buff, writer);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = (RuntimeHelper.Shared.Next(), $"Index: {i} Value: {Guid.NewGuid()}");

			BufferHelper.Print<ValueTuple<Int32, String>>(buff.Span, writer);
			BufferHelper.CollectGarbage(writer);
			BufferHelper.Print<ValueTuple<Int32, String>>(buff.Span, writer);
#if NET9_0_OR_GREATER
			RefStructHelper.PointerFeature(buff.Span, buff.InStack, writer);
#endif
		}
		public static void Generate(ScopedBuffer<(Int32, String)?> buff, TextWriter writer)
		{
			BufferHelper.PrintBufferInfo(buff, writer);
			for (Int32 i = 0; i < buff.Span.Length; i++)
			{
#if !CSHARP9_0
				buff.Span[i] = RuntimeHelper.Shared.Next(0, 5) >= 2 ?
					(ValueTuple<Int32, String>?)new ValueTuple<Int32, String>(
						RuntimeHelper.Shared.Next(), $"Index: {i} Value: {Guid.NewGuid()}") :
					null;
#else
				buff.Span[i] = RuntimeHelper.Shared.Next(0, 5) >= 2 ?
					(RuntimeHelper.Shared.Next(), $"Index: {i} Value: {Guid.NewGuid()}") :
					null;
#endif
			}

			BufferHelper.Print<ValueTuple<Int32, String>?>(buff.Span, writer);
			BufferHelper.CollectGarbage(writer);
			BufferHelper.Print<ValueTuple<Int32, String>?>(buff.Span, writer);
#if NET9_0_OR_GREATER
			RefStructHelper.PointerFeature(buff.Span, buff.InStack, writer);
#endif
		}

		private static void Print<T>(ReadOnlySpan<T> span, TextWriter writer)
		{
#if !NET9_0_OR_GREATER
			foreach (ref readonly T item in span)
#else
			ReadOnlySpan<T>.Enumerator enumerator = span.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ref readonly T item = ref enumerator.Current;
#endif
				writer.WriteLine(item);
#if NET9_0_OR_GREATER
			}
#endif
		}
		private static void PrintBufferInfo<T>(ScopedBuffer<T> buff, TextWriter writer)
		{
			writer.WriteLine($"Span Size: {buff.Span.Length}\t" + $"Buffer Size: {buff.FullLength}\t" +
			                 $"In Stack: {buff.InStack}\t" +
			                 $"Components: {String.Join(", ", buff.BufferMetadata?.Select(c => c.Size) ?? Enumerable.Empty<UInt16>())}");
#if NET9_0_OR_GREATER
			RefStructHelper.PointerFeature((ReadOnlySpan<T>)buff.Span, buff.InStack, writer);
#endif
		}
		private static Action<TextWriter> GetRegister<TBuffer>() where TBuffer : struct, IManagedBinaryBuffer<Object>
		{
#if !CSHARP9_0
			return BufferHelper.Register<TBuffer>;
#else
			return static writer =>
			{
				BufferManager.Register<TBuffer>();
				writer.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
			};
#endif
		}
		private static Action<TextWriter> GetRegister<TBuffer, T>() where TBuffer : struct, IManagedBinaryBuffer<T>
			where T : struct
		{
#if !CSHARP9_0
			return BufferHelper.RegisterValue<TBuffer, T>;
#else
			return static writer =>
			{
				BufferManager.Register<T, TBuffer>();
				writer.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
			};
#endif
		}
		private static Action<TextWriter> GetNullableRegister<TBuffer, T>()
			where TBuffer : struct, IManagedBinaryBuffer<T?> where T : struct
		{
#if !CSHARP9_0
			return BufferHelper.RegisterNullableValue<TBuffer, T>;
#else
			return static writer =>
			{
				BufferManager.RegisterNullable<T, TBuffer>();
				writer.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
			};
#endif
		}
#if !CSHARP9_0
		private static void Register<TBuffer>(TextWriter writer) where TBuffer : struct, IManagedBinaryBuffer<Object>
		{
			BufferManager.Register<TBuffer>();
			writer.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
		}
		private static void RegisterValue<TBuffer, T>(TextWriter writer) where TBuffer : struct, IManagedBinaryBuffer<T> where T : struct
		{
			BufferManager.Register<T, TBuffer>();
			writer.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
		}
		private static void RegisterNullableValue<TBuffer, T>(TextWriter writer) where TBuffer : struct, IManagedBinaryBuffer<T?>
			where T : struct
		{
			BufferManager.RegisterNullable<T, TBuffer>();
			writer.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
		}
#endif
	}
}