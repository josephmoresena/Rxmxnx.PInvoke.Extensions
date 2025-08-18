using System;
using System.Buffers;
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
			foreach (Byte utf8U in RuntimeHelper.Null)
				Console.Write((Char)utf8U);
			Console.WriteLine("");
			ArrayWrapper<Int32> values = new() { Value = new[] { 1, 2, 3, -1, -2, -3, }, };
			foreach (Int32 val in values)
				Console.WriteLine(val);
			if (sequence.Count > 0)
				Console.WriteLine("=== UTF-8 Enumerable ===");
			CStringSequence.Utf8View.Enumerator enumerator = sequence.CreateView().GetEnumerator();
			while (enumerator.MoveNext())
			{
				Console.WriteLine($"Address: 0x{enumerator.Current.GetUnsafeIntPtr():X}\t" +
				                  $"Length: {enumerator.Current.Length}\t" +
				                  $"Bytes: {Convert.ToBase64String(enumerator.Current)}\t" +
				                  $"Text: {Encoding.UTF8.GetString(enumerator.Current)}");
			}
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
			foreach (ref Int64 rLong in fRent.Values)
				rLong = RuntimeHelper.Shared.Next();
			Program.Print(fRent);
		}

		private static void Print<T>(in IFixedContext<T> ctx)
		{
			Console.Write($"Address: 0x{ctx.Pointer:X}\tItems: {ctx.Values.Length} ");
			foreach (T value in ctx.Values)
				Console.Write($"{value} ");
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