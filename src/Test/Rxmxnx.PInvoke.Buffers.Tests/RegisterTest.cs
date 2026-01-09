namespace Rxmxnx.PInvoke.Tests;

[TestFixture]
[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class RegisterTest
{
	private static readonly Type compositeType = typeof(Composite<,,>);
	private static readonly MethodInfo sizeOfInfo =
		typeof(Unsafe).GetMethod(nameof(Unsafe.SizeOf), BindingFlags.Public | BindingFlags.Static)!;
	private static readonly MethodInfo registerInfo = typeof(BufferManager)
	                                                  .GetMethods(BindingFlags.Public | BindingFlags.Static)
	                                                  .First(m => m.Name == nameof(BufferManager.Register) &&
		                                                         m.GetGenericArguments().Length == 2);
	private static readonly MethodInfo registerNullableInfo = typeof(BufferManager)
	                                                          .GetMethods(BindingFlags.Public | BindingFlags.Static)
	                                                          .First(m => m.Name ==
		                                                                 nameof(BufferManager.RegisterNullable));

	[Fact]
	public void BooleanTest() => RegisterTest.StructTest<Boolean>();
	[Fact]
	public void ByteTest() => RegisterTest.StructTest<Byte>();
	[Fact]
	public void Int16Test() => RegisterTest.StructTest<Int16>();
	[Fact]
	public void Int32Test() => RegisterTest.StructTest<Int32>();
	[Fact]
	public void Int64Test() => RegisterTest.StructTest<Int64>();
	[Fact]
	public void StringWrapperTest() => RegisterTest.StructTest<WrapperStruct<String?>>();
	[Fact]
	public void ObjectTest()
	{
		MethodInfo registerObjectInfo = typeof(BufferManager).GetMethods(BindingFlags.Public | BindingFlags.Static)
		                                                     .First(m => m.Name == nameof(BufferManager.Register) &&
			                                                            m.GetGenericArguments().Length == 1);
		Int32 pow = Random.Shared.Next(3, 10);
		_ = RegisterTest.GetCompositeType<Object>(pow, out Int32 sizeofT, out Type resultType);

#if NET5_0_OR_GREATER
		Int32 sizeOfResultType = RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate<Func<Int32>>()();
#else
		Int32 sizeOfResultType =
			((Func<Int32>)RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate(typeof(Func<Int32>)))();
#endif
		IManagedBinaryBuffer<Object>? buffer = (IManagedBinaryBuffer<Object>?)Activator.CreateInstance(resultType);
#if NET5_0_OR_GREATER
		registerObjectInfo.MakeGenericMethod(resultType).CreateDelegate<Action>()();
#else
		((Action)registerObjectInfo.MakeGenericMethod(resultType).CreateDelegate(typeof(Action)))();
#endif
		PInvokeAssert.NotNull(buffer);
		PInvokeAssert.Equal(sizeOfResultType / sizeofT, buffer.Metadata.Size);
		PInvokeAssert.True(buffer.Metadata.IsBinary);
		PInvokeAssert.Equal(buffer.Metadata.Size != 1 ? 2 : 0, buffer.Metadata.ComponentCount);
	}

	private static void StructTest<T>() where T : struct
	{
		Int32 pow = Random.Shared.Next(3, 10);
		Type typeofT = RegisterTest.GetCompositeType<T>(pow, out Int32 sizeofT, out Type resultType);
#if NET5_0_OR_GREATER
		Int32 sizeOfResultType = RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate<Func<Int32>>()();
#else
		Int32 sizeOfResultType =
			((Func<Int32>)RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate(typeof(Func<Int32>)))();
#endif
		IManagedBinaryBuffer<T>? buffer = (IManagedBinaryBuffer<T>?)Activator.CreateInstance(resultType);
		PInvokeAssert.NotNull(buffer);
		PInvokeAssert.Equal(sizeOfResultType / sizeofT, buffer.Metadata.Size);
		PInvokeAssert.True(buffer.Metadata.IsBinary);
		PInvokeAssert.Equal(buffer.Metadata.Size != 1 ? 2 : 0, buffer.Metadata.ComponentCount);
#if NET5_0_OR_GREATER
		RegisterTest.registerInfo.MakeGenericMethod(typeofT, resultType).CreateDelegate<Action>()();
#else
		((Action)RegisterTest.registerInfo.MakeGenericMethod(typeofT, resultType).CreateDelegate(typeof(Action)))();
#endif

		_ = RegisterTest.GetCompositeType<T?>(pow, out sizeofT, out resultType);
#if NET5_0_OR_GREATER
		sizeOfResultType = RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate<Func<Int32>>()();
#else
		sizeOfResultType =
			((Func<Int32>)RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate(typeof(Func<Int32>)))();
#endif
		IManagedBinaryBuffer<T?>? nullableBuffer = (IManagedBinaryBuffer<T?>?)Activator.CreateInstance(resultType);
#if NET5_0_OR_GREATER
		RegisterTest.registerNullableInfo.MakeGenericMethod(typeofT, resultType).CreateDelegate<Action>()();
#else
		((Action)RegisterTest.registerNullableInfo.MakeGenericMethod(typeofT, resultType)
		                     .CreateDelegate(typeof(Action)))();
#endif
		PInvokeAssert.NotNull(nullableBuffer);
		PInvokeAssert.Equal(sizeOfResultType / sizeofT, nullableBuffer.Metadata.Size);
		PInvokeAssert.True(nullableBuffer.Metadata.IsBinary);
		PInvokeAssert.Equal(nullableBuffer.Metadata.Size != 1 ? 2 : 0, nullableBuffer.Metadata.ComponentCount);
	}

	private static Type GetCompositeType<T>(Int32 pow, out Int32 sizeofT, out Type resultType)
	{
		Type typeofT = typeof(T);
		Type[] types = new Type[pow];
		types[0] = typeof(Atomic<T>);
		for (Int32 i = 1; i < types.Length; i++)
			types[i] = RegisterTest.GetCompositeType(types[i - 1], typeofT);
		resultType = RegisterTest.GetCompositeType(types, typeofT);
		sizeofT = Unsafe.SizeOf<T>();
		return typeofT;
	}
	private static Type GetCompositeType(Type componentType, Type itemType)
		=> RegisterTest.compositeType.MakeGenericType(componentType, componentType, itemType);
	private static Type GetCompositeType(Span<Type> components, Type itemType)
	{
		Type? initial = default;
		foreach (Type component in components)
		{
			if (Random.Shared.NextDouble() < 0.5)
				initial = initial is null ?
					initial :
					RegisterTest.compositeType.MakeGenericType(initial, component, itemType);
		}
		return initial ?? components[Random.Shared.Next(0, components.Length)];
	}
#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}