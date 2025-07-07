namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public class AsSpanTest
{
	private static readonly IFixture fixture = new Fixture();
	private static readonly Type extensionsType = typeof(MemoryBlockExtensions);
	private static readonly ImmutableDictionary<Int32, MethodInfo> asSpans = AsSpanTest.extensionsType
		.GetMethods(BindingFlags.Public | BindingFlags.Static).Where(m => m.Name == "AsSpan")
		.ToImmutableDictionary(m => m.GetParameters()[0].ParameterType.GetArrayRank(), m => m);
	private static readonly MethodInfo genericArrayTest =
		typeof(AsSpanTest).GetMethod(nameof(AsSpanTest.GenericArrayTest),
		                             BindingFlags.NonPublic | BindingFlags.Static)!;

	[Fact]
	internal void AsSpanCountTest() => Assert.Equal(31, AsSpanTest.asSpans.Count);
#if NET5_0_OR_GREATER
	[SkippableTheory]
#else
	[Theory]
#endif
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
	internal void ByteTest(Int32 dimension) => AsSpanTest.GenericTest<Byte>(dimension);
#if NET5_0_OR_GREATER
	[SkippableTheory]
#else
	[Theory]
#endif
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
	internal void Int32Test(Int32 dimension) => AsSpanTest.GenericTest<Int32>(dimension);
#if NET5_0_OR_GREATER
	[SkippableTheory]
#else
	[Theory]
#endif
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
#if NET6_0_OR_GREATER
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
#endif
	internal void StringTest(Int32 dimension) => AsSpanTest.GenericTest<String>(dimension);
#if NET5_0_OR_GREATER
	[SkippableTheory]
#else
	[Theory]
#endif
	[InlineData(2)]
	[InlineData(3)]
	[InlineData(4)]
	[InlineData(5)]
	[InlineData(6)]
	[InlineData(7)]
	[InlineData(8)]
	[InlineData(9)]
	[InlineData(10)]
#if NET6_0_OR_GREATER
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
#endif
	internal void ObjectTest(Int32 dimension) => AsSpanTest.GenericTest<Object>(dimension);

	private static void GenericTest<T>(Int32 count)
	{
#if NET5_0_OR_GREATER
		Skip.If(IntPtr.Size == sizeof(Int32) && count > 15);
#else
		if (IntPtr.Size == sizeof(Int32) && count > 15) return;
#endif
		Int32[] lengths = Enumerable.Range(0, count).Select(_ => Random.Shared.Next(1, 3)).ToArray();

		AsMemoryTest.CollectGarbage();

		Array arr = AsSpanTest.CreateArray<T>(lengths);
		AsSpanTest.genericArrayTest.MakeGenericMethod(typeof(T), arr.GetType()).Invoke(null, [arr,]);

		AsMemoryTest.CollectGarbage();
	}
	private static void GenericArrayTest<T, TArray>(Array arr) where TArray : class
	{
		MethodInfo method = AsSpanTest.asSpans[arr.Rank];
#if NET5_0_OR_GREATER
		GetSpanDelegate<T, TArray> func = method.MakeGenericMethod(typeof(T))
		                                        .CreateDelegate<GetSpanDelegate<T, TArray>>();
#else
		GetSpanDelegate<T, TArray> func = (GetSpanDelegate<T, TArray>)method.MakeGenericMethod(typeof(T))
		                                                                    .CreateDelegate(
			                                                                    typeof(GetSpanDelegate<T, TArray>));
#endif
		Span<T> emptySpan = func(default);
		Span<T> span = func(arr as TArray);
		Assert.True(emptySpan.IsEmpty);
		Assert.Equal(span.Length, arr.Length);
		Assert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(span),
		                           ref MemoryMarshalCompat.GetArrayDataReference<T>(arr)));
		IEnumerator enumerator = arr.GetEnumerator();
		foreach (ref T value in span)
		{
			enumerator.MoveNext();
			Assert.Equal(value, enumerator.Current);
		}
		(enumerator as IDisposable)?.Dispose();
	}

	private static Array CreateArray<T>(Int32[] lengths)
	{
		Array arr = Array.CreateInstance(typeof(T), lengths);
#if NET6_0_OR_GREATER
		Span<T> span = MemoryMarshal.CreateSpan(ref Unsafe.As<Byte, T>(ref MemoryMarshal.GetArrayDataReference(arr)),
#else
		Span<T> span = MemoryMarshal.CreateSpan(ref MemoryMarshalCompat.GetArrayDataReference<T>(arr),
#endif
		                                        arr.Length);
		T[] data = AsSpanTest.fixture.CreateMany<T>(arr.Length).ToArray();
		data.CopyTo(span);
		return arr;
	}
	private delegate Span<T> GetSpanDelegate<T, in TArray>(TArray? array);
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}