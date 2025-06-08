namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public class AsMemoryTest
{
	private static readonly IFixture fixture = new Fixture();
	private static readonly Type extensionsType = typeof(MemoryBlockExtensions);
	private static readonly ImmutableDictionary<Int32, MethodInfo> asMemories = AsMemoryTest.extensionsType
		.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "AsMemory")
		.ToImmutableDictionary(m => m.GetParameters()[0].ParameterType.GetArrayRank(), m => m);

	[Fact]
	internal void AsMemoryCountTest() => Assert.Equal(31, AsMemoryTest.asMemories.Count);
	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	[InlineData(16)]
	[InlineData(17)]
	[InlineData(18)]
	[InlineData(19)]
	[InlineData(20)]
	[InlineData(21)]
	[InlineData(22)]
	[InlineData(23)]
	[InlineData(24)]
	[InlineData(25)]
	[InlineData(26)]
	[InlineData(27)]
	[InlineData(28)]
	[InlineData(29)]
	[InlineData(30)]
	[InlineData(31)]
	[InlineData(32)]
	internal void ByteTest(Int32 dimension) => AsMemoryTest.GenericTest<Byte>(dimension);
	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	[InlineData(16)]
	[InlineData(17)]
	[InlineData(18)]
	[InlineData(19)]
	[InlineData(20)]
	[InlineData(21)]
	[InlineData(22)]
	[InlineData(23)]
	[InlineData(24)]
	[InlineData(25)]
	[InlineData(26)]
	[InlineData(27)]
	[InlineData(28)]
	[InlineData(29)]
	[InlineData(30)]
	[InlineData(31)]
	[InlineData(32)]
	internal void Int32Test(Int32 dimension) => AsMemoryTest.GenericTest<Int32>(dimension);
	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	[InlineData(16)]
	[InlineData(17)]
	[InlineData(18)]
	[InlineData(19)]
	[InlineData(20)]
	[InlineData(21)]
	[InlineData(22)]
	[InlineData(23)]
	[InlineData(24)]
	[InlineData(25)]
	[InlineData(26)]
	[InlineData(27)]
	[InlineData(28)]
	[InlineData(29)]
	[InlineData(30)]
	[InlineData(31)]
	[InlineData(32)]
	internal void StringTest(Int32 dimension) => AsMemoryTest.GenericTest<String>(dimension);
	[Theory]
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
	[InlineData(11)]
	[InlineData(12)]
	[InlineData(13)]
	[InlineData(14)]
	[InlineData(15)]
	[InlineData(16)]
	[InlineData(17)]
	[InlineData(18)]
	[InlineData(19)]
	[InlineData(20)]
	[InlineData(21)]
	[InlineData(22)]
	[InlineData(23)]
	[InlineData(24)]
	[InlineData(25)]
	[InlineData(26)]
	[InlineData(27)]
	[InlineData(28)]
	[InlineData(29)]
	[InlineData(30)]
	[InlineData(31)]
	[InlineData(32)]
	internal void ObjectTest(Int32 dimension) => AsMemoryTest.GenericTest<Object>(dimension);

	private static unsafe void GenericTest<T>(Int32 count)
	{
		Int32[] lengths = Enumerable.Range(0, count).Select(_ => Random.Shared.Next(1, 3)).ToArray();
		Array arr = AsMemoryTest.CreateArray<T>(lengths);
		MethodInfo method = AsMemoryTest.asMemories[arr.Rank];

		GC.Collect();
		GC.WaitForFullGCComplete();

		Memory<T> emptyMemory = Assert.IsType<Memory<T>>(method.MakeGenericMethod(typeof(T)).Invoke(null, [null,]));
		Memory<T> memory = Assert.IsType<Memory<T>>(method.MakeGenericMethod(typeof(T)).Invoke(null, [arr,]));
		Assert.Equal(Memory<T>.Empty, emptyMemory);
		Assert.Equal(memory.Length, arr.Length);
		IEnumerator enumerator = arr.GetEnumerator();
		foreach (T value in memory.Span)
		{
			enumerator.MoveNext();
			Assert.Equal(value, enumerator.Current);
		}
		(enumerator as IDisposable)?.Dispose();
		if (!typeof(T).IsValueType)
		{
			Assert.Throws<ArgumentException>(() => memory.Pin());
			return;
		}
		using MemoryHandle handle = memory.Pin();
		ref T first = ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetArrayDataReference(arr));
		for (Int32 i = 0; i < arr.Length; i++)
		{
			Assert.True(Unsafe.AreSame(ref Unsafe.Add(ref first, i),
			                           ref Unsafe.Add(ref Unsafe.AsRef<T>(handle.Pointer), i)));
		}
		MemoryHandle handle2 = memory.Pin();
		handle2.Dispose();
		handle2.Dispose();

		GC.Collect();
		GC.WaitForFullGCComplete();
	}
	private static Array CreateArray<T>(Int32[] lengths)
	{
		Array arr = Array.CreateInstance(typeof(T), lengths);
		Span<T> span = MemoryMarshal.CreateSpan(ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetArrayDataReference(arr)),
		                                        arr.Length);
		T[] data = AsMemoryTest.fixture.CreateMany<T>(arr.Length).ToArray();
		data.CopyTo(span);
		return arr;
	}
}