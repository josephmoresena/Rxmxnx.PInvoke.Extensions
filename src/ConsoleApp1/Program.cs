using System.Diagnostics;
using System.Reflection;

using Rxmxnx.PInvoke;
using Rxmxnx.PInvoke.Buffers;

public static class Program
{
	private static readonly Boolean?[] values =
		[true, false, null, null, false, null, true, true, null, false, true, true, null, true, false, false, null,];

	private static Boolean inStack = true;

	public static void Main(String[] args)
	{
		// BufferManager
		// 	.Register<Composite<Composite<Atomic<Object>, Atomic<Object>, Object>, Composite<
		// 		Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>,
		// 		Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>, Object>>();

		Stopwatch sw = new();
		sw.Start();
		// BufferManager.RegisterSpace2<Composite<
		// 	Composite<
		// 		Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 		Composite<Atomic<Object>, Atomic<Object>, Object>, 
		// 		Object>,
		// 	Composite<
		// 		Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 		Composite<Atomic<Object>, Atomic<Object>, Object>, 
		// 		Object>, 
		// 	Object>>();
		//BufferManager.RegisterSpace32<Composite<Atomic<Object>, Atomic<Object>, Object>>();
		sw.Stop();
		
		Console.WriteLine("Total: " + sw.Elapsed);
		
		// BufferManager.RegisterQuadrupleSpace<Composite<
		// 	Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 	Composite<Atomic<Object>, Atomic<Object>, Object>, 
		// 	Object>>();
		using ConsoleTraceListener a = new();
		Trace.Listeners.Add(a);
		
		// BufferManager
		// 	.RegisterSpace<Composite<
		// 		Composite<
		// 			Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, 
		// 			Composite<
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, 
		// 			Object>, 
		// 		Composite<
		// 			Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, 
		// 			Composite<
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, 
		// 			Object>, 
		// 		Object>>();

		// BufferManager
		// 	.RegisterSpace<Composite<
		// 		Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Composite<
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>>();
		// BufferManager
		// 	.RegisterSpace<Composite<
		// 		Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Composite<
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>>();
		// BufferManager
		// 	.RegisterSpace<Composite<
		// 		Composite<
		// 			Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>,
		// 			Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>, 
		// 		Composite<
		// 			Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Composite<
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 				Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>, 
		// 		Object>>();

		// BufferManager
		// 	.Register<Composite<Composite<Atomic<Object>, Atomic<Object>, Object>, Composite<
		// 		Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>,
		// 		Composite<Composite<Atomic<Object>, Atomic<Object>, Object>,
		// 			Composite<Atomic<Object>, Atomic<Object>, Object>, Object>, Object>, Object>>();
		// BufferManager
		// 	.RegisterNullable<Boolean, Composite<
		// 		Composite<Atomic<Boolean?>, Composite<Atomic<Boolean?>, Atomic<Boolean?>, Boolean?>, Boolean?>,
		// 		Composite<Composite<Atomic<Boolean?>, Atomic<Boolean?>, Boolean?>,
		// 			Composite<Atomic<Boolean?>, Atomic<Boolean?>, Boolean?>, Boolean?>, Boolean?>>();

		Console.Write("Size: ");
		Int32 count = Int32.Parse(Console.ReadLine()!);
		BufferManager.Alloc<String?>((UInt16)count, Program.StringTest);

		if (!Program.inStack)
		{
			Console.Write("Minimal buffer ");
			BufferManager.Alloc<String?>((UInt16)count, Program.StringTest, true);
		}

		// BufferManager.Alloc<Boolean?>((UInt16)Random.Shared.Next(1, Program.values.Length), Program.NullableBooleanTest,
		//                               true);
		//
		// Double[] dValues = Enumerable.Range(0, 10).Select(_ => Random.Shared.NextDouble()).ToArray();
		// BufferManager.Alloc<Double, Double[]>((UInt16)Random.Shared.Next(1, dValues.Length), dValues,
		//                                       Program.DoubleTest);
		
	}
	private static void StringTest(ScopedBuffer<String?> buff)
	{
		if (!Program.inStack && !buff.InStack)
		{
			Console.WriteLine(" not available.");
			return;
		}
		Program.inStack = buff.InStack;
		Console.WriteLine("String? In stack: " + buff.InStack + " Allocated: " + buff.FullLength);
		for (Int32 i = 0; i < buff.Span.Length; i++)
			buff.Span[i] = $"Index: {i} Value: {Random.Shared.NextDouble()}";

		Program.PrintStringSpan(buff.Span);

		Console.WriteLine("Begin GC.Collect()");

		GC.Collect();
		GC.WaitForFullGCComplete();

		Console.WriteLine("End GC.Collect()");

		Program.PrintStringSpan(buff.Span);
	}
	private static void PrintStringSpan(ReadOnlySpan<String?> span)
	{
		foreach (ref readonly String? str in span)
			Console.WriteLine(str);
	}

	private static void NullableBooleanTest(ScopedBuffer<Boolean?> buff)
	{
		Console.WriteLine("Boolean? Count: " + buff.Span.Length + " In stack: " + buff.InStack + " Allocated: " +
		                  buff.FullLength);
		Program.values.AsSpan()[..buff.Span.Length].CopyTo(buff.Span);
		Console.WriteLine("Begin GC.Collect()");

		GC.Collect();
		GC.WaitForFullGCComplete();

		Console.WriteLine("End GC.Collect()");

		Console.WriteLine(Program.values.AsSpan()[..buff.Span.Length].SequenceEqual(buff.Span));
	}
	private static void DoubleTest(ScopedBuffer<Double> buff, in Double[] d)
	{
		Console.WriteLine("Double Count: " + buff.Span.Length + " In stack: " + buff.InStack + " Allocated: " +
		                  buff.FullLength);
		d.AsSpan()[..buff.Span.Length].CopyTo(buff.Span);
		Console.WriteLine("Begin GC.Collect()");

		GC.Collect();
		GC.WaitForFullGCComplete();

		Console.WriteLine("End GC.Collect()");

		Console.WriteLine(d.AsSpan()[..buff.Span.Length].SequenceEqual(buff.Span));
	}
}