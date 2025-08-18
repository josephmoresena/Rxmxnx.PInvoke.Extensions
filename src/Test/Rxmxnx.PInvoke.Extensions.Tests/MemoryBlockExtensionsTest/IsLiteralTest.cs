#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.MemoryBlockExtensionsTest;

[ExcludeFromCodeCoverage]
[SuppressMessage("csharpsquid", "S2699")]
public sealed class IsLiteralTest
{
	private const Int32 constantInteger = 49;
	private const String constantString = "CONST_STRING_VALUE";

	private static readonly Int32 roIntegerValue = 67;
	private static readonly String roStringField = "CONST_STRING_FIELD";
	private static readonly Boolean isMonoRuntime = typeof(String).Assembly.GetType("Mono.Runtime") is not null;

	private static Int32 ConstantIntegerProperty => 134;
	private static String ConstantStringProperty => "CONST_STRING_PROPERTY";
	private static ReadOnlySpan<Char> ConstantStringSpan => "CONST_STRING_SPAN";
	private static ReadOnlySpan<Char> ConstantCharSpan
		=> ['C', 'O', 'N', 'S', 'T', '_', 'S', 'T', 'R', 'I', 'N', 'G', '_', 'S', 'P', 'A', 'N', '\0',];
	private static ReadOnlySpan<Byte> ConstantByteSpan => "CONST_BYTE_SPAN"u8;

	[Fact]
	public void Test()
	{
		Int32 value = Random.Shared.Next();

		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref value, 1).IsLiteral());
#pragma warning disable CS9193
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(IsLiteralTest.constantInteger), 1)
		                                 .IsLiteral());
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in IsLiteralTest.roIntegerValue), 1)
		                                 .IsLiteral());
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(IsLiteralTest.ConstantIntegerProperty), 1)
		                                 .IsLiteral());
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(IsLiteralTest.constantString), 1)
		                                 .IsLiteral());
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in IsLiteralTest.roStringField), 1)
		                                 .IsLiteral());
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(IsLiteralTest.ConstantStringProperty), 1)
		                                 .IsLiteral());
#pragma warning restore CS9193

		PInvokeAssert.False(IsLiteralTest.roStringField.AsSpan().IsLiteral());
		PInvokeAssert.False(IsLiteralTest.constantString.AsSpan().IsLiteral());
		PInvokeAssert.False(IsLiteralTest.ConstantStringProperty.AsSpan().IsLiteral());
		PInvokeAssert.False(IsLiteralTest.ConstantStringSpan.IsLiteral());

#if NET7_0_OR_GREATER
		Assert.True(IsLiteralTest.ConstantCharSpan.IsLiteral());
#else
		PInvokeAssert.False(IsLiteralTest.ConstantCharSpan.IsLiteral());
#endif
		PInvokeAssert.Equal(!IsLiteralTest.isMonoRuntime, IsLiteralTest.ConstantByteSpan.IsLiteral());
		PInvokeAssert.Equal(!IsLiteralTest.isMonoRuntime, "LITERAL_BYTE_SPAN"u8.IsLiteral());
	}
	[Fact]
	public static void ReferenceTest()
	{
		ReadOnlySpan<String> stringSpan =
			[IsLiteralTest.roStringField, IsLiteralTest.ConstantStringProperty, IsLiteralTest.constantString,];
		ReadOnlySpan<Int32> integers =
			[IsLiteralTest.constantInteger, IsLiteralTest.ConstantIntegerProperty, IsLiteralTest.roIntegerValue,];
		ReadOnlySpan<Object> objectSpan =
		[
			IsLiteralTest.roStringField, IsLiteralTest.ConstantStringProperty, IsLiteralTest.constantString,
			IsLiteralTest.constantInteger, IsLiteralTest.ConstantIntegerProperty, IsLiteralTest.roIntegerValue,
		];

		PInvokeAssert.False(stringSpan.IsLiteral());
		PInvokeAssert.False(objectSpan.IsLiteral());
		PInvokeAssert.False(integers.IsLiteral());
	}
	[Fact]
	public static void LocalConstTest()
	{
		const String constValue = "LOCAL_STRING_VALUE";
		const Int32 constIntValue = 90;

#pragma warning disable CS9193
		PInvokeAssert.False(MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(constIntValue), 1).IsLiteral());
#pragma warning restore CS9193
		PInvokeAssert.False(constValue.AsSpan().IsLiteral());
	}

#if !NET6_0_OR_GREATER
	private static class Random
	{
		public static readonly System.Random Shared = new();
	}
#endif
}