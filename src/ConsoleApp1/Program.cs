using Rxmxnx.PInvoke.Buffers;

public static class Program
{
	private static readonly Boolean?[] values =
		[true, false, null, null, false, null, true, true, null, false, true, true, null, true, false, false, null,];
	private static Boolean inStack;
	public static void Main(String[] args)
	{
		AllocatedBuffer
			.Register<Composed<Composed<Primordial<Object>, Primordial<Object>, Object>, Composed<
				Composed<Composed<Primordial<Object>, Primordial<Object>, Object>,
					Composed<Primordial<Object>, Primordial<Object>, Object>, Object>,
				Composed<Composed<Primordial<Object>, Primordial<Object>, Object>,
					Composed<Primordial<Object>, Primordial<Object>, Object>, Object>, Object>, Object>>();
		AllocatedBuffer.RegisterNullable<Boolean, Composed<Primordial<Boolean?>, Primordial<Boolean?>, Boolean?>>();
		Console.Write("Size: ");
		Int32 count = Int32.Parse(Console.ReadLine()!);
		AllocatedBuffer.Alloc<String?>((UInt16)count, Program.UseStringSpan);

		if (!Program.inStack)
		{
			Console.WriteLine("Minimal buffer.");
			AllocatedBuffer.Alloc<String?>((UInt16)count, Program.UseStringSpan, true);
		}

		AllocatedBuffer.Alloc<Boolean?>((UInt16)Random.Shared.Next(1, Program.values.Length), buff =>
		{
			Console.WriteLine("Boolean? Count: " + buff.Span.Length + " In stack: " + buff.InStack + " Allocated: " +
			                  buff.FullLength);
			Program.values.AsSpan()[..buff.Span.Length].CopyTo(buff.Span);
			Console.WriteLine("Begin GC.Collect()");

			GC.Collect();
			GC.WaitForFullGCComplete();

			Console.WriteLine("End GC.Collect()");

			Console.WriteLine(Program.values.AsSpan()[..buff.Span.Length].SequenceEqual(buff.Span));
		});

		Double[] dValues = Enumerable.Range(0, 10).Select(i => Random.Shared.NextDouble()).ToArray();
		AllocatedBuffer.Alloc((UInt16)Random.Shared.Next(1, dValues.Length), dValues,
		                      (AllocatedBuffer<Double> buff, in Double[] d) =>
		                      {
			                      Console.WriteLine("Double Count: " + buff.Span.Length + " In stack: " + buff.InStack +
			                                        " Allocated: " + buff.FullLength);
			                      d.AsSpan()[..buff.Span.Length].CopyTo(buff.Span);
			                      Console.WriteLine("Begin GC.Collect()");

			                      GC.Collect();
			                      GC.WaitForFullGCComplete();

			                      Console.WriteLine("End GC.Collect()");

			                      Console.WriteLine(d.AsSpan()[..buff.Span.Length].SequenceEqual(buff.Span));
		                      });
	}

	private static void UseStringSpan(AllocatedBuffer<String?> buff)
	{
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
}