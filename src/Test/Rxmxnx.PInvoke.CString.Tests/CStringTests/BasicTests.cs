#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

namespace Rxmxnx.PInvoke.Tests.CStringTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class BasicTests
{
	[Fact]
	public void EmptyTest()
	{
		using MemoryHandle _ = CString.Empty.TryPin(out Boolean pinned);
		ReadOnlySpan<Byte> emptySpan = default(CString?);
		CString? nullCStr = default(Byte[]);
		Byte[] emptyBytes = pinned ? CString.GetBytes(CString.Empty) : [default,];

		CString noEmpty1 = CString.Create(emptyBytes);
		CString noEmpty2 = CString.Create(new Byte[] { default, default, });
		CString noEmpty3 = noEmpty2[..1];
		CString noEmpty4 = noEmpty2[1..];
		CString noEmpty5 = new(() => emptyBytes);

		CString zero = CString.CreateUnsafe(IntPtr.Zero, 0);
		CString empty2 = emptyBytes;
		CString empty3 = noEmpty1[..0];
		CString empty4 = CString.Create(() => emptyBytes);
		CString empty5 = new Byte[] { default, default, };

		PInvokeAssert.Equal(CString.Zero, zero);
		PInvokeAssert.NotEqual(CString.Empty, zero); // Empty <> Zero
		PInvokeAssert.Equal(String.Empty, CString.Empty.ToString());
		PInvokeAssert.Equal(CString.Empty, (CString)String.Empty);
		PInvokeAssert.Equal(String.Empty, zero.ToString());
		PInvokeAssert.NotEqual(zero, (CString)String.Empty);
		PInvokeAssert.True(CString.IsNullOrEmpty(zero));
		PInvokeAssert.True(CString.IsNullOrEmpty(CString.Empty));
		PInvokeAssert.True(CString.IsNullOrEmpty(default));

		PInvokeAssert.Equal(0, zero.CompareTo(CString.Empty));
		PInvokeAssert.Equal(0, zero.CompareTo(String.Empty));
		PInvokeAssert.Equal(0, zero.CompareTo((Object)CString.Empty));
		PInvokeAssert.Equal(0, zero.CompareTo((Object)String.Empty));

		PInvokeAssert.True(default(CString) == nullCStr);
		PInvokeAssert.True(default(String) == default(CString));
		PInvokeAssert.True(default(CString) == default(String));
		PInvokeAssert.False(default(CString) == zero); // Zero is not null.
		PInvokeAssert.False(zero == default(CString)); // Zero is not null.
		PInvokeAssert.True(String.Empty == CString.Empty);
		PInvokeAssert.True(CString.Empty == String.Empty);

		PInvokeAssert.False(default(CString) != nullCStr);
		PInvokeAssert.False(default(String) != default(CString));
		PInvokeAssert.False(default(CString) != default(String));
		PInvokeAssert.False(String.Empty != CString.Empty);
		PInvokeAssert.False(CString.Empty != String.Empty);

		PInvokeAssert.True(default(CString) != CString.Empty);
		PInvokeAssert.True(default(String) != CString.Empty);
		PInvokeAssert.True(default(CString) != String.Empty);
		PInvokeAssert.True(CString.Empty != default(CString));
		PInvokeAssert.True(String.Empty != default(CString));
		PInvokeAssert.True(CString.Empty != default(String));

		PInvokeAssert.Null(CString.Create(default(ReadOnlySpanFunc<Byte>)));
		PInvokeAssert.Null(CString.Create(default(Byte[])));
		PInvokeAssert.Null(nullCStr);
		PInvokeAssert.NotNull(zero);
		PInvokeAssert.False(zero is null);

		PInvokeAssert.Throws<ArgumentNullException>(() => CString.GetBytes(default!));
		PInvokeAssert.Throws<ArgumentException>(() => zero!.CompareTo(new Object()));

		PInvokeAssert.True(emptySpan.IsEmpty);

		PInvokeAssert.NotEqual(CString.Empty, noEmpty1);
		PInvokeAssert.False(noEmpty1.IsReference);
		PInvokeAssert.False(noEmpty1.IsNullTerminated);
		PInvokeAssert.False(noEmpty1.IsFunction);
		PInvokeAssert.False(noEmpty1.IsSegmented);
		PInvokeAssert.Equal(1, noEmpty1.Length);
		PInvokeAssert.NotEqual(CString.Empty, noEmpty2);
		PInvokeAssert.False(noEmpty2.IsReference);
		PInvokeAssert.False(noEmpty2.IsNullTerminated);
		PInvokeAssert.False(noEmpty2.IsFunction);
		PInvokeAssert.False(noEmpty2.IsSegmented);
		PInvokeAssert.Equal(2, noEmpty2.Length);
		PInvokeAssert.NotEqual(CString.Empty, noEmpty3);
		PInvokeAssert.False(noEmpty3.IsReference);
		PInvokeAssert.True(noEmpty3.IsNullTerminated);
		PInvokeAssert.False(noEmpty3.IsFunction);
		PInvokeAssert.False(noEmpty3.IsSegmented);
		PInvokeAssert.Equal(1, noEmpty3.Length);
		PInvokeAssert.NotEqual(CString.Empty, noEmpty4);
		PInvokeAssert.False(noEmpty4.IsReference);
		PInvokeAssert.True(noEmpty4.IsNullTerminated);
		PInvokeAssert.False(noEmpty4.IsFunction);
		PInvokeAssert.True(noEmpty4.IsSegmented);
		PInvokeAssert.Equal(1, noEmpty4.Length);
		PInvokeAssert.NotEqual(CString.Empty, noEmpty5);
		PInvokeAssert.False(noEmpty5.IsReference);
		PInvokeAssert.True(noEmpty5.IsNullTerminated);
		PInvokeAssert.True(noEmpty5.IsFunction);
		PInvokeAssert.False(noEmpty5.IsSegmented);
		PInvokeAssert.Equal(1, noEmpty5.Length);

		PInvokeAssert.Equal(CString.Empty, empty2);
		PInvokeAssert.False(empty2.IsReference);
		PInvokeAssert.True(empty2.IsNullTerminated);
		PInvokeAssert.False(empty2.IsFunction);
		PInvokeAssert.Equal(CString.Empty, empty3);
		PInvokeAssert.False(empty3.IsReference);
		PInvokeAssert.True(empty3.IsNullTerminated);
		PInvokeAssert.Equal(!pinned, empty3.IsFunction);
		PInvokeAssert.Equal(CString.Empty, empty4);
		PInvokeAssert.False(empty4.IsReference);
		PInvokeAssert.True(empty4.IsNullTerminated);
		PInvokeAssert.True(empty4.IsFunction);
		PInvokeAssert.Equal(CString.Empty, empty5);
		PInvokeAssert.False(empty4.IsReference);
		PInvokeAssert.True(empty5.IsNullTerminated);
		PInvokeAssert.False(empty5.IsFunction);

		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
		{
			String str = TestSet.Utf16Text[i];
			Boolean equal = String.IsNullOrEmpty(str);
			PInvokeAssert.Equal(equal, CString.Empty == str);
			PInvokeAssert.Equal(equal, CString.Empty == new CString(TestSet.Utf8Text[i]));
		}
	}

	[Fact]
	public void NormalTest()
	{
		Int32 lenght = TestSet.Utf16Text.Count;
		CString[,] cstr = new CString[5, lenght];
		BasicTests.CreateCStringFromString(cstr);
		BasicTests.CreateCStringFromFunction(cstr);
		BasicTests.CreateCStringFromBytes(cstr);
		BasicTests.CreateCStringFromNullTerminatedBytes(cstr);
		BasicTests.CreateCStringFromFunctionNonLiteral(cstr);

		for (Int32 i = 0; i < lenght; i++)
		{
			CString cstr1 = cstr[0, i];
			for (Int32 j = 1; j <= 4; j++)
			{
				CString cstr2 = cstr[j, i];
				PInvokeAssert.Equal(cstr1, cstr2);
				switch (j)
				{
					case 1:
						BasicTests.AssertFromFunction(cstr2);
						break;
					case 2:
						BasicTests.AssertFromBytes(cstr2);
						break;
					case 3:
						BasicTests.AssertFromNullTerminatedBytes(cstr2);
						break;
					case 4:
						BasicTests.AssertFromFunctionNonLiteral(cstr2);
						break;
				}
				BasicTests.RefEnumerationTest(cstr1, cstr2);
				BasicTests.EnumerationTest(cstr1, cstr2);
				PInvokeAssert.True(cstr2.Equals(TestSet.Utf16Text[i]));
				PInvokeAssert.True(cstr2.Equals((Object)TestSet.Utf16Text[i]));
				PInvokeAssert.True(cstr2.Equals((Object)cstr1));
				PInvokeAssert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
				PInvokeAssert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
				PInvokeAssert.Equal(cstr1.ToArray(), cstr2.ToArray());
				PInvokeAssert.Equal(0, cstr1.CompareTo(cstr2));
			}
			BasicTests.AssertFromString(cstr1);
			PInvokeAssert.True(cstr1.Equals(TestSet.Utf16Text[i]));
			PInvokeAssert.Equal(0, cstr1.CompareTo(TestSet.Utf16Text[i]));
			PInvokeAssert.Equal(TestSet.Utf16Text[i].GetHashCode(), cstr1.GetHashCode());
			PInvokeAssert.Equal(cstr1.Equals(CString.Empty), cstr1.Equals(String.Empty));
		}
	}

	[Fact]
	public void PointerTest()
	{
		Int32 lenght = TestSet.Utf8Bytes.Count;
		for (Int32 i = 0; i < lenght; i++)
		{
			CString cstr1 = new(TestSet.Utf8Text[i]);
			BasicTests.TestBytesPointer(TestSet.Utf8Bytes[i], TestSet.Utf16Text[i], cstr1);
			BasicTests.TestNullTerminatedBytesPointer(TestSet.Utf8NullTerminatedBytes[i], TestSet.Utf16Text[i], cstr1);
		}
	}

#if NETCOREAPP
	[Fact]
	public void LiteralTest()
	{
		CString[] literalArray = TestSet.Utf8TextUpper.Select(f => new CString(f)).ToArray();
		CString[] nonLiteralArray = TestSet.Utf8NullTerminatedBytes.Select(b => (CString)b).ToArray();

		Assert.All(literalArray, c => PInvokeAssert.True(MemoryInspector.Instance.IsLiteral(c.AsSpan())));
		Assert.All(nonLiteralArray, c => PInvokeAssert.False(MemoryInspector.Instance.IsLiteral(c.AsSpan())));
		Assert.All(nonLiteralArray, c => PInvokeAssert.False(MemoryInspector.Instance.IsLiteral(c.AsSpan())));
	}
#endif

	[Fact]
	public unsafe void TryPinTest()
	{
		CString[] encodingArray = TestSet.Utf16Text.Select(s => (CString)s).ToArray();
		CString[] literalArray = TestSet.Utf8Text.Select(f => new CString(f)).ToArray();
		CString[] bytesArray = TestSet.Utf8Bytes.Select(a => (CString)a).ToArray();
		CString[] bytesNullArray = TestSet.Utf8NullTerminatedBytes.Select(a => (CString)a).ToArray();

		PInvokeAssert.All(encodingArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			PInvokeAssert.True(pinned);
			PInvokeAssert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned)
			                                   .Pointer);
			PInvokeAssert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			PInvokeAssert.True(pinned);
			PInvokeAssert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
			PInvokeAssert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
		PInvokeAssert.All(literalArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			PInvokeAssert.False(pinned);
			PInvokeAssert.Equal((IntPtr)handle.Pointer, IntPtr.Zero);
			PInvokeAssert.NotEqual((IntPtr)handle.Pointer,
			                       (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned)
			                                   .Pointer);
			PInvokeAssert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			PInvokeAssert.False(pinned);
			PInvokeAssert.Equal((IntPtr)handle2.Pointer, IntPtr.Zero);
			PInvokeAssert.NotEqual((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
		PInvokeAssert.All(bytesArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			PInvokeAssert.True(pinned);
			PInvokeAssert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned)
			                                   .Pointer);
			PInvokeAssert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			PInvokeAssert.True(pinned);
			PInvokeAssert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
			PInvokeAssert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
		PInvokeAssert.All(bytesNullArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			PInvokeAssert.True(pinned);
			PInvokeAssert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			PInvokeAssert.Equal((IntPtr)handle.Pointer,
			                    (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned)
			                                   .Pointer);
			PInvokeAssert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			PInvokeAssert.True(pinned);
			PInvokeAssert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
			PInvokeAssert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
	}

	[Fact]
	public void PlusOperator()
	{
		using TestMemoryHandle handle = new();
		List<Int32> indices = TestSet.GetIndices();
		List<Int32> indices2 = TestSet.GetIndices(indices.Count);
		for (Int32 i = 0; i < indices.Count; i++)
		{
			String? leftStr = TestSet.GetString(indices[i], true);
			CString? leftCStr = TestSet.GetCString(indices[i], handle);

			String? rightStr = TestSet.GetString(indices2[i], true);
			CString? rightCStr = TestSet.GetCString(indices2[i], handle);

			String valueStr = leftStr + rightStr;
			CString valueCStr0 = leftCStr + rightCStr;
			CString valueCStr1 = leftStr + rightCStr;
			CString valueCStr2 = leftCStr + rightStr;

			PInvokeAssert.Equal(valueStr, valueCStr0.ToString());
			PInvokeAssert.True(valueCStr0.AsSpan().SequenceEqual(valueCStr1));
			PInvokeAssert.True(valueCStr0.AsSpan().SequenceEqual(valueCStr2));
		}
	}

	private static void CreateCStringFromString(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf16Text.Count; i++)
			cstr[0, i] = (CString)TestSet.Utf16Text[i];
	}
	private static void CreateCStringFromFunction(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
			cstr[1, i] = new(TestSet.Utf8Text[i]);
	}
	private static void CreateCStringFromBytes(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8Bytes.Count; i++)
			cstr[2, i] = TestSet.Utf8Bytes[i];
	}
	private static void CreateCStringFromNullTerminatedBytes(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8NullTerminatedBytes.Count; i++)
			cstr[3, i] = TestSet.Utf8NullTerminatedBytes[i];
	}
	private static void CreateCStringFromFunctionNonLiteral(CString[,] cstr)
	{
		for (Int32 i = 0; i < TestSet.Utf8Text.Count; i++)
			cstr[4, i] = CString.Create(TestSet.Utf8Text[i]);
	}
	private static void AssertFromString(CString cstr)
	{
		PInvokeAssert.True(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());
		// Now String -> CString, uses Byte[] buffer
		PInvokeAssert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);
	}
	private static unsafe void AssertFromNullTerminatedBytes(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));

		PInvokeAssert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);

		fixed (Byte* ptr = &MemoryMarshal.GetReference(cstr.AsSpan()))
		{
			CString unsafeCStr = CString.CreateNullTerminatedUnsafe((IntPtr)ptr);
			PInvokeAssert.False(unsafeCStr.IsFunction);
			PInvokeAssert.True(unsafeCStr.IsNullTerminated);
			PInvokeAssert.True(unsafeCStr.IsReference);
			PInvokeAssert.False(unsafeCStr.IsSegmented);
			PInvokeAssert.Equal(cstr, unsafeCStr);
		}

		CString rawClone = CString.Create(CString.GetBytes(cstr));
		PInvokeAssert.False(rawClone.IsFunction);
		PInvokeAssert.False(rawClone.IsNullTerminated);
		PInvokeAssert.False(rawClone.IsReference);
		PInvokeAssert.False(rawClone.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(rawClone));
		PInvokeAssert.NotEqual(cstr, rawClone);
		PInvokeAssert.Equal(cstr.Length + 1, rawClone.Length);

		CString rawSpanClone = CString.Create(CString.GetBytes(cstr).AsSpan());
		PInvokeAssert.False(rawSpanClone.IsFunction);
		PInvokeAssert.False(rawSpanClone.IsNullTerminated);
		PInvokeAssert.False(rawSpanClone.IsReference);
		PInvokeAssert.False(rawSpanClone.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(rawSpanClone));
		PInvokeAssert.NotEqual(cstr, rawSpanClone);
		PInvokeAssert.Equal(cstr.Length + 1, rawSpanClone.Length);
	}
	private static void AssertFromBytes(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.False(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		PInvokeAssert.Equal(cstr.Length, CString.GetBytes(cstr).Length);

		CString rawClone = CString.Create(CString.GetBytes(cstr));
		PInvokeAssert.False(rawClone.IsFunction);
		PInvokeAssert.False(rawClone.IsNullTerminated);
		PInvokeAssert.False(rawClone.IsReference);
		PInvokeAssert.False(rawClone.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(rawClone));
		PInvokeAssert.Equal(cstr, rawClone);
		PInvokeAssert.Equal(cstr.Length, rawClone.Length);

		CString rawSpanClone = CString.Create(CString.GetBytes(cstr).AsSpan());
		PInvokeAssert.False(rawSpanClone.IsFunction);
		PInvokeAssert.False(rawSpanClone.IsNullTerminated);
		PInvokeAssert.False(rawSpanClone.IsReference);
		PInvokeAssert.False(rawSpanClone.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(rawSpanClone));
		PInvokeAssert.Equal(cstr, rawSpanClone);
		PInvokeAssert.Equal(cstr.Length, rawSpanClone.Length);
	}
	private static unsafe void AssertFromFunction(CString cstr)
	{
		PInvokeAssert.True(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		fixed (Byte* ptr = &MemoryMarshal.GetReference(cstr.AsSpan()))
		{
			CString unsafeCStr = CString.CreateNullTerminatedUnsafe((IntPtr)ptr);
			PInvokeAssert.False(unsafeCStr.IsFunction);
			PInvokeAssert.True(unsafeCStr.IsNullTerminated);
			PInvokeAssert.True(unsafeCStr.IsReference);
			PInvokeAssert.False(unsafeCStr.IsSegmented);
			PInvokeAssert.Equal(cstr, unsafeCStr);
		}

		BasicTests.AssertGetBytesException(cstr);

		CString rawSpanClone = CString.Create(cstr);
		PInvokeAssert.False(rawSpanClone.IsFunction);
		PInvokeAssert.False(rawSpanClone.IsNullTerminated);
		PInvokeAssert.False(rawSpanClone.IsReference);
		PInvokeAssert.False(rawSpanClone.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(rawSpanClone));
		PInvokeAssert.Equal(cstr, rawSpanClone);
		PInvokeAssert.Equal(cstr.Length, rawSpanClone.Length);
	}
	private static void AssertFromFunctionNonLiteral(CString cstr)
	{
		PInvokeAssert.True(cstr.IsFunction);
		PInvokeAssert.False(cstr.IsNullTerminated);
		PInvokeAssert.False(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		BasicTests.AssertGetBytesException(cstr);

		CString rawSpanClone = CString.Create(cstr);
		PInvokeAssert.False(rawSpanClone.IsFunction);
		PInvokeAssert.False(rawSpanClone.IsNullTerminated);
		PInvokeAssert.False(rawSpanClone.IsReference);
		PInvokeAssert.False(rawSpanClone.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(rawSpanClone));
		PInvokeAssert.Equal(cstr, rawSpanClone);
		PInvokeAssert.Equal(cstr.Length, rawSpanClone.Length);
	}
	private static unsafe void AssertFromBytesPointer(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.False(cstr.IsNullTerminated);
		PInvokeAssert.True(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());
		BasicTests.AssertGetBytesException(cstr);

		fixed (void* ptr = cstr.AsSpan())
		{
			CString rawPointerSpan = CString.CreateUnsafe(new(ptr), cstr.Length, true);
			PInvokeAssert.False(rawPointerSpan.IsFunction);
			PInvokeAssert.False(rawPointerSpan.IsNullTerminated);
			PInvokeAssert.True(rawPointerSpan.IsReference);
			PInvokeAssert.False(rawPointerSpan.IsSegmented);
			PInvokeAssert.False(CString.IsNullOrEmpty(rawPointerSpan));
			PInvokeAssert.Equal(cstr, rawPointerSpan);
			PInvokeAssert.Equal(cstr.Length, rawPointerSpan.Length);
		}
	}

	private static unsafe void AssertFromNullTerminatedBytesPointer(CString cstr)
	{
		PInvokeAssert.False(cstr.IsFunction);
		PInvokeAssert.True(cstr.IsNullTerminated);
		PInvokeAssert.True(cstr.IsReference);
		PInvokeAssert.False(cstr.IsSegmented);
		PInvokeAssert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		fixed (void* ptr = cstr.AsSpan())
		{
			CString rawPointerCString = CString.CreateUnsafe(new(ptr), cstr.Length + 1, true);
			CString unsafeCStr = CString.CreateNullTerminatedUnsafe(new(ptr));

			PInvokeAssert.False(rawPointerCString.IsFunction);
			PInvokeAssert.False(rawPointerCString.IsNullTerminated);
			PInvokeAssert.True(rawPointerCString.IsReference);
			PInvokeAssert.False(rawPointerCString.IsSegmented);

			PInvokeAssert.False(unsafeCStr.IsFunction);
			PInvokeAssert.True(unsafeCStr.IsNullTerminated);
			PInvokeAssert.True(unsafeCStr.IsReference);
			PInvokeAssert.False(unsafeCStr.IsSegmented);

			PInvokeAssert.False(CString.IsNullOrEmpty(rawPointerCString));
			PInvokeAssert.NotEqual(cstr, rawPointerCString);
			PInvokeAssert.Equal(cstr, unsafeCStr);
			PInvokeAssert.Equal(cstr.Length + 1, rawPointerCString.Length);
			PInvokeAssert.Equal(cstr.Length, cstr.Length);
		}
	}
	private static void RefEnumerationTest(CString cstr1, CString cstr2)
	{
		Int32 i = 0;
		foreach (ref readonly Byte utf8Char in cstr2)
		{
			PInvokeAssert.Equal(cstr1[i], utf8Char);
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(in cstr2.AsSpan()[i], in utf8Char));
#else
			PInvokeAssert.True(Unsafe.AreSame(ref Unsafe.AsRef(in cstr2.AsSpan()[i]), ref Unsafe.AsRef(in utf8Char)));
#endif
			i++;
		}
	}
	private static void EnumerationTest(CString cstr1, IEnumerable<Byte> cstr2)
	{
		using IEnumerator<Byte> enumerator1 = (cstr1 as IEnumerable<Byte>).GetEnumerator();
		foreach (Byte utf8Char in cstr2)
		{
			enumerator1.MoveNext();
			PInvokeAssert.Equal(enumerator1.Current, utf8Char);
		}
	}
	private static unsafe void TestBytesPointer(Byte[] bytes, String text, CString cstr1)
	{
		fixed (Byte* bptr2 = bytes)
		{
			CString cstr2 = CString.CreateUnsafe(new(bptr2), bytes.Length);
			PInvokeAssert.Equal(cstr1, cstr2);
			BasicTests.AssertFromBytesPointer(cstr2);
			PInvokeAssert.True(cstr2.Equals(text));
			PInvokeAssert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
			PInvokeAssert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
			PInvokeAssert.Equal(cstr1.ToArray(), cstr2.ToArray());
			BasicTests.RefEnumerationTest(cstr1, cstr2);
			BasicTests.EnumerationTest(cstr1, cstr2);
		}
	}
	private static unsafe void TestNullTerminatedBytesPointer(Byte[] bytes, String text, CString cstr1)
	{
		fixed (Byte* bptr2 = bytes)
		{
			CString cstr2 = CString.CreateUnsafe(new(bptr2), bytes.Length);
			PInvokeAssert.Equal(cstr1, cstr2);
			PInvokeAssert.True(cstr2.Equals(text));
			BasicTests.AssertFromNullTerminatedBytesPointer(cstr2);
			PInvokeAssert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
			PInvokeAssert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
			PInvokeAssert.Equal(cstr1.ToArray(), cstr2.ToArray());
			BasicTests.RefEnumerationTest(cstr1, cstr2);
			BasicTests.EnumerationTest(cstr1, cstr2);
		}
	}

	private static void AssertGetBytesException(CString cstr)
	{
		try
		{
			PInvokeAssert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
		}
		catch (Exception)
		{
			// For some reason sometimes the test fails even though it shouldn't.
			// The test must be run again so that it does not fail.
			PInvokeAssert.NotEmpty(cstr.ToArray());
			PInvokeAssert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
		}
	}
}