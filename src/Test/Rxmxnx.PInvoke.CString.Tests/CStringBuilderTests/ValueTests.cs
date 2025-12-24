#if !NETCOREAPP
using InlineData = NUnit.Framework.TestCaseAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringBuilderTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class ValueTests : CStringBuilderTestsBase
{
	private readonly IFixture _fixture = new Fixture();

	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void BooleanTest(Int32 length)
	{
		Helper<Boolean> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void SByteTest(Int32 length)
	{
		Helper<SByte> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void ByteTest(Int32 length)
	{
		Helper<Byte> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v, true),
			AppendU8Null = static (v, c) => c.Append(v, true),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v, true),
			InsertU8Null = static (i, v, c) => c.Insert(i, v, true),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void CharTest(Int32 length)
	{
		Helper<Char> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void DoubleTest(Int32 length)
	{
		Helper<Double> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void DecimalTest(Int32 length)
	{
		Helper<Decimal> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void SingleTest(Int32 length)
	{
		Helper<Single> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void Int16Test(Int32 length)
	{
		Helper<Int16> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void UInt16Test(Int32 length)
	{
		Helper<UInt16> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void Int32Test(Int32 length)
	{
		Helper<Int32> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void UInt32Test(Int32 length)
	{
		Helper<UInt32> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void Int64Test(Int32 length)
	{
		Helper<Int64> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}
	[Theory]
	[InlineData(8)]
	[InlineData(32)]
	[InlineData(256)]
	[InlineData(300)]
	[InlineData(1000)]
	public void UInt64Test(Int32 length)
	{
		Helper<UInt64> helper = new()
		{
			AppendU16 = static (v, s) => s.Append(v),
			AppendU8 = static (v, c) => c.Append(v),
			AppendU8Null = static (v, c) => c.Append(v),
			InsertU16 = static (i, v, s) => s.Insert(i, v),
			InsertU8 = static (i, v, c) => c.Insert(i, v),
			InsertU8Null = static (i, v, c) => c.Insert(i, v),
		};
		this.AppendTest(length, helper);
		this.InsertTest(length, helper);
	}

	private void AppendTest<T>(Int32 length, Helper<T> helper) where T : unmanaged
	{
		T[] values = this._fixture.CreateMany<T>(length).ToArray();
		T?[] valuesNull = this._fixture.CreateMany<T?>(length).ToArray();
		StringBuilder strBuild = new();
		CStringBuilder cstrBuild = new();
		foreach (T value in values.AsSpan())
		{
			helper.AppendU16(value, strBuild);
			Assert.True(Object.ReferenceEquals(cstrBuild, helper.AppendU8(value, cstrBuild)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToString());
		foreach (T? value in valuesNull.AsSpan())
		{
			strBuild.Append(value);
			Assert.True(Object.ReferenceEquals(cstrBuild, helper.AppendU8Null(value, cstrBuild)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToString());
	}
	private void InsertTest<T>(Int32 length, Helper<T> helper) where T : unmanaged
	{
		T[] values = this._fixture.CreateMany<T>(length).ToArray();
		T?[] valuesNull = this._fixture.CreateMany<T?>(length).ToArray();
		String? seed = TestSet.GetString(TestSet.GetIndices(1).FirstOrDefault(), true);
		StringBuilder strBuild = new(seed);
		CStringBuilder cstrBuild = new(seed);

		foreach (T value in values.AsSpan())
		{
			(Int32 utf16Index, Int32 utf8Index) = CStringBuilderTestsBase.GetIndex(strBuild.ToString(), out _);
			helper.InsertU16(utf16Index, value, strBuild);
			Assert.True(Object.ReferenceEquals(cstrBuild, helper.InsertU8(utf8Index, value, cstrBuild)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToString());
		foreach (T? value in valuesNull.AsSpan())
		{
			(Int32 utf16Index, Int32 utf8Index) = CStringBuilderTestsBase.GetIndex(strBuild.ToString(), out _);
			strBuild.Insert(utf16Index, value);
			Assert.True(Object.ReferenceEquals(cstrBuild, helper.InsertU8Null(utf8Index, value, cstrBuild)));
		}
		PInvokeAssert.Equal(strBuild.ToString(), cstrBuild.ToString());
	}

	private sealed class Helper<T> where T : unmanaged
	{
		public Func<T, StringBuilder, StringBuilder> AppendU16 { get; init; } = default!;
		public Func<T, CStringBuilder, CStringBuilder> AppendU8 { get; init; } = default!;
		public Func<T?, CStringBuilder, CStringBuilder> AppendU8Null { get; init; } = default!;
		public Func<Int32, T, StringBuilder, StringBuilder> InsertU16 { get; init; } = default!;
		public Func<Int32, T, CStringBuilder, CStringBuilder> InsertU8 { get; init; } = default!;
		public Func<Int32, T?, CStringBuilder, CStringBuilder> InsertU8Null { get; init; } = default!;
	}
}