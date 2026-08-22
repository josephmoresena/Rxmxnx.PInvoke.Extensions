using System;
using System.Buffers;
using System.IO;
#if NET10_0_OR_GREATER
using System.Collections.Generic;
#endif

#if !NETCOREAPP2_1_OR_GREATER && !NET46_OR_GREATER && !WINDOWS_UWP
using System.Text;

#endif

namespace Rxmxnx.PInvoke.ApplicationTest
{
	public static class FeatureHelper
	{
		public static void MainEntryPoint(TextWriter writer)
		{
			RuntimeHelper.PrintRuntimeInfo(writer);
#if NET6_0_OR_GREATER
			if (!AotInfo.IsNativeAot)
				new TrimmedByNativeAot(writer).WriteUtf8("This text will be removed when compiled using NativeAOT.");
#endif
			FeatureHelper.MatrixFeature(writer);
			FeatureHelper.BufferFeature(writer);
			FeatureHelper.UnicodeFeature(writer);
			FeatureHelper.GuidFeature(writer);
		}

		#region FeatureMethods
		private static void MatrixFeature(TextWriter writer)
		{
			Double[,] mxm = { { 0.3, -2.2, 3.2, 0, }, { 0.12, -2, 0.2, 6, }, { 9, 0, 0, -1, }, { 2, 2.1, -1, 4, }, };
			Double[,] nxn = { { 2, 3, }, { 2, 1, }, };
			Double[,] mxn = { { -1, 1, }, { 4, 0, }, { 2, 1, }, { 1, 3, }, };

			writer.WriteLine("=== 4x4 ===");
			MatrixHelper.Print(mxm, writer);
			writer.WriteLine($"Determinant 4x4: {MatrixHelper.GetDeterminant(mxm, out Double[,] inverse):0.####}");
			writer.WriteLine("=== (4x4)^-1 ===");
			MatrixHelper.Print(inverse, writer);
			writer.WriteLine("=== (4x4)^-1 (Text) -> [] ===");
			MatrixHelper.ToText(inverse).AsSpan().WithSafeFixed(new PrintAction<String>(writer));
			writer.WriteLine("=== 2x2 ===");
			MatrixHelper.Print(nxn, writer);
			writer.WriteLine($"Determinant 2x2: {MatrixHelper.GetDeterminant(nxn):0.####}");
			writer.WriteLine("=== 4x2 ===");
			MatrixHelper.Print(mxn, writer);
			writer.WriteLine("=== 4x4 * 4x2 ===");
			MatrixHelper.Print(MatrixHelper.Multiply(mxm, mxn), writer);
			writer.WriteLine("=== 2x2 -> [] ===");
			mxn.AsSpan().WithSafeFixed(new PrintAction<Double>(writer));
			writer.WriteLine("=== 2x2 (Text) -> [] ===");
			MatrixHelper.ToText(inverse).AsSpan().WithSafeFixed(new PrintAction<String>(writer));
		}
		private static void BufferFeature(TextWriter writer)
		{
			writer.WriteLine("=== Stack alloc [Int32] ===");
#if !CSHARP9_0
			BufferAction action = new BufferAction(writer) { Count = 3, IsMinimalCount = false };
#else
			BufferAction action = new(writer) { Count = 3, IsMinimalCount = false, };
#endif
			BufferManager<Int32>.Alloc(action);
			action.Count = 5;
			BufferManager<Int32>.Alloc(action);
			writer.WriteLine("=== Stack alloc [Double?] ===");
			action.Count = 3;
			BufferManager<Double?>.Alloc(action);
			action.Count = 5;
			BufferManager<Double?>.Alloc(action);
			writer.WriteLine("=== Stack alloc [String] ===");
			action.Count = 3;
			BufferManager<String?>.Alloc(action);
			action.Count = 5;
			BufferManager<String?>.Alloc(action);
			if (AotInfo.IsReflectionDisabled)
			{
				action.IsMinimalCount = true;
				BufferManager<String?>.Alloc(action);
				action.IsMinimalCount = false;
			}
			if (!BufferManager.BufferAutoCompositionEnabled)
			{
				BufferHelper.RegisterMetadataObject(writer);
				BufferManager<String?>.Alloc(action);
			}
#if !NET8_0_OR_GREATER
			if (AotInfo.IsNativeAot && SystemInfo.IsMonoRuntime)
			{
				action.Count = 15;
				BufferManager<String?>.Alloc(action);
			}
#endif
			writer.WriteLine("=== Stack alloc [(Int32, String)] ===");
			action.Count = 3;
			BufferManager<ValueTuple<Int32, String>>.Alloc(action);
			action.Count = 5;
			BufferManager<ValueTuple<Int32, String>>.Alloc(action);
			if (!BufferManager.BufferAutoCompositionEnabled)
			{
				BufferHelper.RegisterMetadataValue(writer);
				BufferManager<ValueTuple<Int32, String>>.Alloc(action);
			}
			writer.WriteLine("=== Stack alloc [(Int32, String)?] ===");
			action.Count = 3;
			BufferManager<ValueTuple<Int32, String>?>.Alloc(action);
			action.Count = 5;
			BufferManager<ValueTuple<Int32, String>?>.Alloc(action);
			// ReSharper disable once InvertIf
			if (!BufferManager.BufferAutoCompositionEnabled)
			{
				BufferHelper.RegisterMetadataNullableValue(writer);
				BufferManager<ValueTuple<Int32, String>?>.Alloc(action);
			}
		}
		private static void UnicodeFeature(TextWriter writer)
		{
			String?[] texts = { "String0", "String1", null, "String3", "", "String5", };
#if !CSHARP9_0
			CStringSequence sequence = new CStringSequence(texts);
#else
			CStringSequence sequence = new(texts);
#endif
			try
			{
				SerializableMessage<String> serializable = ConvertHelper.Convert(new SerializableMessage<CString>
				{
					Title = (CString?)"This is not a message",
					Message = (CString?)"This is a UTF-8 message for you.",
				});
				writer.WriteLine(ConvertHelper.Convert(serializable));
				String initialBuffer = sequence.ToString();
				sequence = ConvertHelper.Convert(ConvertHelper.Convert(sequence));
				writer.WriteLine(
					$"Buffer Equality: {initialBuffer == sequence.ToString()}\tBuffer Instance: {Object.ReferenceEquals(initialBuffer, sequence.ToString())}");
			}
			catch (Exception ex)
			{
				writer.WriteLine($"**Unable to perform conversion: {ex.Message}**");
			}
			writer.WriteLine("=== Enumerable sequences ===");
			foreach (CString value in sequence)
				writer.WriteLine(!value.IsZero ? value : RuntimeHelper.Null);
#if !NET10_0_OR_GREATER
			foreach (Byte utf8U in RuntimeHelper.Null)
#else
			foreach (Byte utf8U in (IEnumerable<Byte>)RuntimeHelper.Null)
#endif
				writer.Write((Char)utf8U);
			writer.WriteLine("");
#if !CSHARP9_0
			ArrayWrapper<Int32> values = new ArrayWrapper<Int32> { Value = new[] { 1, 2, 3, -1, -2, -3, }, };
#else
			ArrayWrapper<Int32> values = new() { Value = new[] { 1, 2, 3, -1, -2, -3, }, };
#endif
			foreach (Int32 val in values)
				writer.WriteLine(val);
			if (sequence.Count > 0)
				writer.WriteLine("=== UTF-8 Enumerable ===");
#if !NET9_0_OR_GREATER
			foreach (ReadOnlySpan<Byte> utf8Span in sequence.CreateView())
#else
			CStringSequence.Utf8View.Enumerator enumerator = sequence.CreateView().GetEnumerator();

			while (enumerator.MoveNext())
			{
				ReadOnlySpan<Byte> utf8Span = enumerator.Current;
#endif
#if NET5_0_OR_GREATER
				writer.WriteLine($"Address: 0x{utf8Span.GetUnsafeIntPtr():X}\t" + $"Length: {utf8Span.Length}\t" +
#else
				writer.WriteLine($"Address: 0x{utf8Span.GetUnsafeIntPtr().ToString("X")}\t" +
				                 $"Length: {utf8Span.Length}\t" +
#endif
#if !NET452_OR_GREATER && (NETCOREAPP3_0_OR_GREATER || !NETCOREAPP && !WINDOWS_UWP)
				                 $"Bytes: {Convert.ToBase64String(utf8Span)}\t" +
#else
				                 $"Bytes: {Convert.ToBase64String(utf8Span.ToArray())}\t" +
#endif
				                 $"Text: {utf8Span.ToUtf16()}");
#if NET9_0_OR_GREATER
			}
#endif
			writer.WriteLine("=== Building `Bohemian rhapsody` ===");
			Utf8ConcatenationHelper.CStringBuildingFeature(writer);
		}
		private static void GuidFeature(TextWriter writer)
		{
			writer.WriteLine("=== Referenceable Wrapper ===");
#if !NETCOREAPP3_0_OR_GREATER && (NETCOREAPP || NET461_OR_GREATER || WINDOWS_UWP || LEGACY)
			IMutableReference<Guid> uuid = WrapperFactory.CreateReferenceable(Guid.NewGuid());
#else
			IMutableReference<Guid> uuid = IMutableReference.Create(Guid.NewGuid());
#endif

			FeatureHelper.Print(uuid, writer);
			uuid.Reference = Guid.NewGuid();
			FeatureHelper.Print(uuid, writer);

			writer.WriteLine("=== Fixed Rent ===");
			using IDisposable _ =
				ArrayPool<Int64>.Shared.RentFixed(10, out FixedContextValue<Int64> fRent, false, out Int32 arrayLength);
#if NET5_0_OR_GREATER
			writer.WriteLine($"Address: 0x{fRent.Pointer:X}\tRequired: {fRent.Values.Length}\tRented: {arrayLength}");
#else
			writer.WriteLine(
				$"Address: 0x{fRent.Pointer.ToString("X")}\tRequired: {fRent.Values.Length}\tRented: {arrayLength}");
#endif
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
			FeatureHelper.Print(fRent, writer);
		}
		#endregion

		#region PrintMethods
		private static void Print<T>(FixedContextValue<T> ctx, TextWriter writer)
		{
#if NET5_0_OR_GREATER
			writer.Write($"Address: 0x{ctx.Pointer:X}\tItems: {ctx.Values.Length}\t\t");
#else
			writer.Write($"Address: 0x{ctx.Pointer.ToString("X")}\tItems: {ctx.Values.Length}\t\t");
#endif
#if !NET9_0_OR_GREATER
			foreach (T value in ctx.Values)
#else
			Span<T>.Enumerator enumerator = ctx.Values.GetEnumerator();
			while (enumerator.MoveNext())
			{
				ref T value = ref enumerator.Current;
#endif
				writer.Write($"{value} ");
#if NET9_0_OR_GREATER
			}
#endif
			writer.WriteLine("");
		}
		private static void Print(IMutableReference<Guid> uuid, TextWriter writer)
		{
			BufferHelper.CollectGarbage(writer);
			ref Guid refU = ref uuid.Reference;
#if !LEGACY && (NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER || (NETFRAMEWORK && !NET46_OR_GREATER))
#if NET5_0_OR_GREATER
			writer.WriteLine(
				$"Address: 0x{refU.AsBytes().GetUnsafeIntPtr():X}\tWrapper: {uuid.Value}\tRef: {uuid.Reference}");
#else
			writer.WriteLine(
				$"Address: 0x{refU.AsBytes().GetUnsafeIntPtr().ToString("X")}\tWrapper: {uuid.Value}\tRef: {uuid.Reference}");
#endif
#else
			writer.WriteLine(
				$"Address: 0x{refU.GetUnsafeIntPtr().ToString("X")}\tWrapper: {uuid.Value}\tRef: {uuid.Reference}");
#endif
		}
		#endregion

		#region FunctionalInterfaces
		private readonly struct PrintAction<T> : IFixedContextAction<T>
		{
			private readonly TextWriter _writer;
			public PrintAction(TextWriter writer) { this._writer = writer; }
			public void Accept(FixedContextValue<T> ctx) => FeatureHelper.Print(ctx, this._writer);
		}

		private struct BufferAction : IScopedBufferAction<Int32>, IScopedBufferAction<String?>,
			IScopedBufferAction<Double?>, IScopedBufferAction<ValueTuple<Int32, String>>,
			IScopedBufferAction<ValueTuple<Int32, String>?>
		{
			private readonly TextWriter _writer;

			public UInt16 Count { get; set; }
			public Boolean IsMinimalCount { get; set; }

			public BufferAction(TextWriter writer)
			{
				this._writer = writer;
				this.Count = 0;
				this.IsMinimalCount = false;
			}

			public void Accept(ScopedBuffer<(Int32, String)?> buffer) => BufferHelper.Generate(buffer, this._writer);
			public void Accept(ScopedBuffer<(Int32, String)> buffer) => BufferHelper.Generate(buffer, this._writer);
			public void Accept(ScopedBuffer<Double?> buffer) => BufferHelper.Generate(buffer, this._writer);
			public void Accept(ScopedBuffer<String?> buffer) => BufferHelper.Generate(buffer, this._writer);
			public void Accept(ScopedBuffer<Int32> buffer) => BufferHelper.Generate(buffer, this._writer);
		}
		#endregion
	}
}