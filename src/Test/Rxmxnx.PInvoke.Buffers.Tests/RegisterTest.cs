using Rxmxnx.PInvoke.Buffers;

namespace Rxmxnx.PInvoke.Tests;

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

	[Fact]
	internal void BooleanTest() => RegisterTest.StructTest<Boolean>();
	[Fact]
	internal void ByteTest() => RegisterTest.StructTest<Byte>();
	[Fact]
	internal void Int16Test() => RegisterTest.StructTest<Int16>();
	[Fact]
	internal void Int32Test() => RegisterTest.StructTest<Int32>();
	[Fact]
	internal void Int64Test() => RegisterTest.StructTest<Int64>();
	[Fact]
	internal void StringWrapperTest() => RegisterTest.StructTest<WrapperStruct<String?>>();
	[Fact]
	internal void ObjectTest()
	{
		MethodInfo registerObjectInfo = typeof(BufferManager).GetMethods(BindingFlags.Public | BindingFlags.Static)
		                                                     .First(m => m.Name == nameof(BufferManager.Register) &&
			                                                            m.GetGenericArguments().Length == 1);
		Int32 pow = Random.Shared.Next(2, 10);
		_ = RegisterTest.GetCompositeType<Object>(pow, out Int32 sizeofT, out Type resultType);
		Int32 sizeOfResultType = RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate<Func<Int32>>()();
		registerObjectInfo.MakeGenericMethod(resultType).CreateDelegate<Action>()();
		Assert.Equal(sizeOfResultType / sizeofT,
		             ((IManagedBinaryBuffer<Object>)Activator.CreateInstance(resultType)!).Metadata.Size);
	}

	private static void StructTest<T>() where T : struct
	{
		Int32 pow = Random.Shared.Next(2, 10);
		Type typeofT = RegisterTest.GetCompositeType<T>(pow, out Int32 sizeofT, out Type resultType);
		Int32 sizeOfResultType = RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate<Func<Int32>>()();
		RegisterTest.registerInfo.MakeGenericMethod(typeofT, resultType).CreateDelegate<Action>()();
		Assert.Equal(sizeOfResultType / sizeofT,
		             ((IManagedBinaryBuffer<T>)Activator.CreateInstance(resultType)!).Metadata.Size);

		typeofT = RegisterTest.GetCompositeType<T>(pow, out sizeofT, out resultType);
		sizeOfResultType = RegisterTest.sizeOfInfo.MakeGenericMethod(resultType).CreateDelegate<Func<Int32>>()();
		RegisterTest.registerInfo.MakeGenericMethod(typeofT, resultType).CreateDelegate<Action>()();
		Assert.Equal(sizeOfResultType / sizeofT,
		             ((IManagedBinaryBuffer<T>)Activator.CreateInstance(resultType)!).Metadata.Size);
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
}