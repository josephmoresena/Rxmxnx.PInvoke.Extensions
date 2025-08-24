using System;
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
		public static readonly Action RegisterMetadataObject = BufferHelper
			.GetRegister<Composite<Atomic<Object>, Composite<Atomic<Object>, Atomic<Object>, Object>, Object>>();
		public static readonly Action RegisterMetadataValue = BufferHelper
			.GetRegister<
				Composite<Atomic<ValueTuple<Int32, String>>,
					Composite<Atomic<ValueTuple<Int32, String>>, Atomic<ValueTuple<Int32, String>>,
						ValueTuple<Int32, String>>, ValueTuple<Int32, String>>, ValueTuple<Int32, String>>();
		public static readonly Action RegisterMetadataNullableValue = BufferHelper
			.GetNullableRegister<
				Composite<Atomic<ValueTuple<Int32, String>?>,
					Composite<Atomic<ValueTuple<Int32, String>?>, Atomic<ValueTuple<Int32, String>?>,
						ValueTuple<Int32, String>?>, ValueTuple<Int32, String>?>, ValueTuple<Int32, String>>();

		public static void CollectGarbage()
		{
			Console.WriteLine("Begin GC.Collect()");
			GC.Collect();
			if (!SystemInfo.IsMonoRuntime)
				GC.WaitForFullGCComplete();
			else
				GC.WaitForPendingFinalizers();
			Console.WriteLine("End GC.Collect()");
		}
		public static void Generate(ScopedBuffer<Int32> buff)
		{
			BufferHelper.PrintBufferInfo(buff);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = RuntimeHelper.Shared.Next();

			BufferHelper.Print<Int32>(buff.Span);
			BufferHelper.CollectGarbage();
			BufferHelper.Print<Int32>(buff.Span);
		}
		public static void Generate(ScopedBuffer<String?> buff)
		{
			BufferHelper.PrintBufferInfo(buff);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = $"Index: {i} Value: {Guid.NewGuid()}";

			BufferHelper.Print<String?>(buff.Span);
			BufferHelper.CollectGarbage();
			BufferHelper.Print<String?>(buff.Span);
		}
		public static void Generate(ScopedBuffer<Double?> buff)
		{
			BufferHelper.PrintBufferInfo(buff);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = RuntimeHelper.Shared.Next(0, 5) >= 2 ? RuntimeHelper.Shared.NextDouble() : null;

			BufferHelper.Print<Double?>(buff.Span);
			BufferHelper.CollectGarbage();
			BufferHelper.Print<Double?>(buff.Span);
		}
		public static void Generate(ScopedBuffer<ValueTuple<Int32, String>> buff)
		{
			BufferHelper.PrintBufferInfo(buff);
			for (Int32 i = 0; i < buff.Span.Length; i++)
				buff.Span[i] = (RuntimeHelper.Shared.Next(), $"Index: {i} Value: {Guid.NewGuid()}");

			BufferHelper.Print<ValueTuple<Int32, String>>(buff.Span);
			BufferHelper.CollectGarbage();
			BufferHelper.Print<ValueTuple<Int32, String>>(buff.Span);
		}
		public static void Generate(ScopedBuffer<ValueTuple<Int32, String>?> buff)
		{
			BufferHelper.PrintBufferInfo(buff);
			for (Int32 i = 0; i < buff.Span.Length; i++)
			{
				buff.Span[i] = RuntimeHelper.Shared.Next(0, 5) >= 2 ?
					(RuntimeHelper.Shared.Next(), $"Index: {i} Value: {Guid.NewGuid()}") :
					null;
			}

			BufferHelper.Print<ValueTuple<Int32, String>?>(buff.Span);
			BufferHelper.CollectGarbage();
			BufferHelper.Print<ValueTuple<Int32, String>?>(buff.Span);
		}

		private static void Print<T>(ReadOnlySpan<T> span)
		{
#if !NET9_0_OR_GREATER
			foreach (ref readonly T item in span)
#else
			ReadOnlySpan<T>.Enumerator enumerator = span.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ref readonly T item = ref enumerator.Current;
#endif
				Console.WriteLine(item);
#if NET9_0_OR_GREATER
			}
#endif
		}
		private static void PrintBufferInfo<T>(ScopedBuffer<T> buff)
		{
			Console.WriteLine($"Span Size: {buff.Span.Length}\t" + $"Buffer Size: {buff.FullLength}\t" +
			                  $"In Stack: {buff.InStack}\t" +
			                  $"Components: {String.Join(", ", buff.BufferMetadata?.Select(c => c.Size) ?? Enumerable.Empty<UInt16>())}");
		}
		private static Action GetRegister<TBuffer>() where TBuffer : struct, IManagedBinaryBuffer<Object>
		{
			return static () =>
			{
				BufferManager.Register<TBuffer>();
				Console.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
			};
		}
		private static Action GetRegister<TBuffer, T>() where TBuffer : struct, IManagedBinaryBuffer<T> where T : struct
		{
			return static () =>
			{
				BufferManager.Register<T, TBuffer>();
				Console.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
			};
		}
		private static Action GetNullableRegister<TBuffer, T>() where TBuffer : struct, IManagedBinaryBuffer<T?>
			where T : struct
		{
			return static () =>
			{
				BufferManager.RegisterNullable<T, TBuffer>();
				Console.WriteLine($"{new TBuffer().Metadata.Size} buffer registered.");
			};
		}
	}
}