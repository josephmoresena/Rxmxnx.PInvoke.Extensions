#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class SpanConstructorTest
{
	[Fact]
	public void Test()
	{
		Int32 lenght = TestSet.Utf16Text.Count;
		CString[,] cstr = new CString[3, lenght];
		SpanConstructorTest.CreateCStringFromFunction(cstr);
		SpanConstructorTest.CreateCStringFromBytes(cstr);
		SpanConstructorTest.CreateCStringFromNullTerminatedBytes(cstr);

		for (Int32 i = 0; i < lenght; i++)
		{
			CString cstr1 = cstr[0, i];
			for (Int32 j = 1; j < 3; j++)
			{
				CString cstr2 = cstr[j, i];
				PInvokeAssert.Equal(cstr1, cstr2);
				switch (j)
				{
					case 1:
						SpanConstructorTest.AssertFromBytes(cstr2);
						break;
					case 2:
						SpanConstructorTest.AssertFromNullTerminatedBytes(cstr2);
						break;
				}
				PInvokeAssert.True(cstr2.Equals(TestSet.Utf16Text[i]));
				PInvokeAssert.True(cstr2.Equals((Object)TestSet.Utf16Text[i]));
				PInvokeAssert.True(cstr2.Equals((Object)cstr1));
				PInvokeAssert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
				PInvokeAssert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
				PInvokeAssert.Equal(cstr1.ToArray(), cstr2.ToArray());
				PInvokeAssert.Equal(0, cstr1.CompareTo(cstr2));
			}
			SpanConstructorTest.AssertFromFunction(cstr1);
			PInvokeAssert.True(cstr1.Equals(TestSet.Utf16Text[i]));
			PInvokeAssert.Equal(0, cstr1.CompareTo(TestSet.Utf16Text[i]));
			PInvokeAssert.Equal(TestSet.Utf16Text[i].GetHashCode(), cstr1.GetHashCode());
		}
	}

	private static void AssertFromFunction(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		PInvokeAssert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);
	}
	private static void AssertFromBytes(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		PInvokeAssert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);
	}
	private static void AssertFromNullTerminatedBytes(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		PInvokeAssert.Equal(cstr.Length + 2, CString.GetBytes(cstr).Length);
	}
	private static void CreateCStringFromFunction(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
			cstr[0, i] = new(TestSet.Utf8Text[i]());
	}
	private static void CreateCStringFromBytes(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8Bytes.Count; i++)
			cstr[1, i] = new(TestSet.Utf8Bytes[i]);
	}
	private static void CreateCStringFromNullTerminatedBytes(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8NullTerminatedBytes.Count; i++)
			cstr[2, i] = new(TestSet.Utf8NullTerminatedBytes[i]);
	}
}