using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace Rxmxnx.PInvoke.ApplicationTest
{
	internal static class Program
	{
		public static void Main(String[] args)
		{
			RuntimeHelper.PrintRuntimeInfo();
			if (!AotInfo.IsNativeAot)
				new TrimmedByNativeAot(Console.Out).WriteUtf8(
					"This text will be removed when compiled using NativeAOT.");
			Program.MatrixFeature();
			Program.BufferFeature();
			Program.UnicodeFeature();
			Program.GuidFeature();
		}

		private static void MatrixFeature()
		{
			Double[,] mxm = { { 0.3, -2.2, 3.2, 0, }, { 0.12, -2, 0.2, 6, }, { 9, 0, 0, -1, }, { 2, 2.1, -1, 4, }, };
			Double[,] nxn = { { 2, 3, }, { 2, 1, }, };
			Double[,] mxn = { { -1, 1, }, { 4, 0, }, { 2, 1, }, { 1, 3, }, };

			Console.WriteLine("=== 4x4 ===");
			MatrixHelper.Print(mxm);
			Console.WriteLine($"Determinant 4x4: {MatrixHelper.GetDeterminant(mxm, out Double[,] inverse):0.####}");
			Console.WriteLine("=== (4x4)^-1 ===");
			MatrixHelper.Print(inverse);
			Console.WriteLine("=== (4x4)^-1 (Text) -> [] ===");
			MatrixHelper.ToText(inverse).AsSpan().WithSafeFixed(Program.Print);
			Console.WriteLine("=== 2x2 ===");
			MatrixHelper.Print(nxn);
			Console.WriteLine($"Determinant 2x2: {MatrixHelper.GetDeterminant(nxn):0.####}");
			Console.WriteLine("=== 4x2 ===");
			MatrixHelper.Print(mxn);
			Console.WriteLine("=== 4x4 * 4x2 ===");
			MatrixHelper.Print(MatrixHelper.Multiply(mxm, mxn));
			Console.WriteLine("=== 2x2 -> [] ===");
			mxn.AsSpan().WithSafeFixed(Program.Print);
			Console.WriteLine("=== 2x2 (Text) -> [] ===");
			MatrixHelper.ToText(mxn).AsSpan().WithSafeFixed(Program.Print);
		}
		private static void BufferFeature()
		{
			Console.WriteLine("=== Stack alloc [Int32] ===");
			BufferManager.Alloc<Int32>(3, BufferHelper.Generate);
			BufferManager.Alloc<Int32>(5, BufferHelper.Generate);
			Console.WriteLine("=== Stack alloc [Double?] ===");
			BufferManager.Alloc<Double?>(3, BufferHelper.Generate);
			BufferManager.Alloc<Double?>(5, BufferHelper.Generate);
			Console.WriteLine("=== Stack alloc [String] ===");
			BufferManager.Alloc<String?>(3, BufferHelper.Generate);
			if (!BufferManager.BufferAutoCompositionEnabled)
			{
				BufferHelper.RegisterMetadataObject();
				BufferManager.Alloc<String?>(3, BufferHelper.Generate);
			}
			BufferManager.Alloc<String?>(5, BufferHelper.Generate);
			Console.WriteLine("=== Stack alloc [(Int32, String)] ===");
			BufferManager.Alloc<ValueTuple<Int32, String>>(3, BufferHelper.Generate);
			if (!BufferManager.BufferAutoCompositionEnabled)
			{
				BufferHelper.RegisterMetadataValue();
				BufferManager.Alloc<ValueTuple<Int32, String>>(3, BufferHelper.Generate);
			}
			BufferManager.Alloc<ValueTuple<Int32, String>>(5, BufferHelper.Generate);
			Console.WriteLine("=== Stack alloc [(Int32, String)?] ===");
			BufferManager.Alloc<ValueTuple<Int32, String>?>(3, BufferHelper.Generate);
			if (!BufferManager.BufferAutoCompositionEnabled)
			{
				BufferHelper.RegisterMetadataNullableValue();
				BufferManager.Alloc<ValueTuple<Int32, String>?>(3, BufferHelper.Generate);
			}
			BufferManager.Alloc<ValueTuple<Int32, String>?>(5, BufferHelper.Generate);
		}
		private static void UnicodeFeature()
		{
			String?[] texts = { "String0", "String1", null, "String3", "", "String5", };
			CStringSequence sequence = new(texts);
			try
			{
				SerializableMessage<String> serializable = ConvertHelper.Convert(new SerializableMessage<CString>
				{
					Title = (CString)"This is not a message",
					Message = (CString)"This is a UTF-8 message for you.",
				});
				Console.WriteLine(ConvertHelper.Convert(serializable));
				String initialBuffer = sequence.ToString();
				sequence = ConvertHelper.Convert(ConvertHelper.Convert(sequence));
				Console.WriteLine(
					$"Buffer Equality: {initialBuffer == sequence.ToString()}\tBuffer Instance: {Object.ReferenceEquals(initialBuffer, sequence.ToString())}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"**Unable to perform conversion: {ex.Message}**");
			}
			Console.WriteLine("=== Enumerable sequences ===");
			foreach (CString value in sequence)
				Console.WriteLine(!value.IsZero ? value : RuntimeHelper.Null);
#if !NET10_0_OR_GREATER
			foreach (Byte utf8U in RuntimeHelper.Null)
#else
			foreach (Byte utf8U in (IEnumerable<Byte>)RuntimeHelper.Null)
#endif
				Console.Write((Char)utf8U);
			Console.WriteLine("");
			ArrayWrapper<Int32> values = new() { Value = new[] { 1, 2, 3, -1, -2, -3, }, };
			foreach (Int32 val in values)
				Console.WriteLine(val);
			if (sequence.Count > 0)
				Console.WriteLine("=== UTF-8 Enumerable ===");
#if !NET9_0_OR_GREATER
			foreach (ReadOnlySpan<Byte> utf8Span in sequence.CreateView())
#else
			CStringSequence.Utf8View.Enumerator enumerator = sequence.CreateView().GetEnumerator();

			while (enumerator.MoveNext())
			{
				ReadOnlySpan<Byte> utf8Span = enumerator.Current;
#endif
				Console.WriteLine($"Address: 0x{utf8Span.GetUnsafeIntPtr():X}\t" + $"Length: {utf8Span.Length}\t" +
				                  $"Bytes: {Convert.ToBase64String(utf8Span)}\t" +
				                  $"Text: {Encoding.UTF8.GetString(utf8Span)}");
#if NET9_0_OR_GREATER
			}
#endif
		}
		private static void GuidFeature()
		{
			Console.WriteLine("=== Referenceable Wrapper ===");
			IMutableReference<Guid> uuid = IMutableReference.Create(Guid.NewGuid());

			Program.Print(uuid);
			uuid.Reference = Guid.NewGuid();
			Program.Print(uuid);

			Console.WriteLine("=== Fixed Rent ===");
			using IFixedContext<Int64>.IDisposable fRent =
				ArrayPool<Int64>.Shared.RentFixed(10, false, out Int32 arrayLength);
			Console.WriteLine($"Address: 0x{fRent.Pointer:X}\tRequired: {fRent.Values.Length}\tRented: {arrayLength}");
#if !NET9_0_OR_GREATER
			foreach (ref Int64 rLong in fRent.Values)
#else
			Span<Int64>.Enumerator enumerator = fRent.Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ref Int64 rLong = ref enumerator.Current;
#endif
				rLong = RuntimeHelper.Shared.Next();
#if NET9_0_OR_GREATER
			}
#endif
			Program.Print(fRent);
		}

		private static void Print<T>(in IFixedContext<T> ctx)
		{
			Console.Write($"Address: 0x{ctx.Pointer:X}\tItems: {ctx.Values.Length} ");
#if !NET9_0_OR_GREATER
			foreach (T value in ctx.Values)
#else
			Span<T>.Enumerator enumerator = ctx.Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ref T value = ref enumerator.Current;
#endif
				Console.Write($"{value} ");
#if NET9_0_OR_GREATER
			}
#endif
			Console.WriteLine("");
		}
		private static void Print(IMutableReference<Guid> uuid)
		{
			BufferHelper.CollectGarbage();
			ref Guid refU = ref uuid.Reference;
			Console.WriteLine(
				$"Address: 0x{refU.AsBytes().GetUnsafeIntPtr():X}\tWrapper: {uuid.Value}\tRef: {uuid.Reference}");
		}
	}
}