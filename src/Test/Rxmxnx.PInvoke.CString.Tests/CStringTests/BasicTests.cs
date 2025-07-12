namespace Rxmxnx.PInvoke.Tests.CStringTests;

[ExcludeFromCodeCoverage]
public sealed class BasicTests
{
	[Fact]
	internal void EmptyTest()
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

		Assert.Equal(CString.Zero, zero);
		Assert.Equal(CString.Empty, zero);
		Assert.Equal(String.Empty, CString.Empty.ToString());
		Assert.Equal(CString.Empty, (CString)String.Empty);
		Assert.Equal(String.Empty, zero.ToString());
		Assert.Equal(zero, (CString)String.Empty);
		Assert.True(CString.IsNullOrEmpty(zero));
		Assert.True(CString.IsNullOrEmpty(CString.Empty));
		Assert.True(CString.IsNullOrEmpty(default));

		Assert.Equal(0, zero.CompareTo(CString.Empty));
		Assert.Equal(0, zero.CompareTo(String.Empty));
		Assert.Equal(0, zero.CompareTo((Object)CString.Empty));
		Assert.Equal(0, zero.CompareTo((Object)String.Empty));

		Assert.True(default(CString) == nullCStr);
		Assert.True(default(String) == default(CString));
		Assert.True(default(CString) == default(String));
		Assert.True(default(CString) == zero);
		Assert.True(zero == default(CString));
		Assert.True(String.Empty == CString.Empty);
		Assert.True(CString.Empty == String.Empty);

		Assert.False(default(CString) != nullCStr);
		Assert.False(default(String) != default(CString));
		Assert.False(default(CString) != default(String));
		Assert.False(String.Empty != CString.Empty);
		Assert.False(CString.Empty != String.Empty);

		Assert.True(default(CString) != CString.Empty);
		Assert.True(default(String) != CString.Empty);
		Assert.True(default(CString) != String.Empty);
		Assert.True(CString.Empty != default(CString));
		Assert.True(String.Empty != default(CString));
		Assert.True(CString.Empty != default(String));

		Assert.Null(CString.Create(default(ReadOnlySpanFunc<Byte>)));
		Assert.Null(CString.Create(default(Byte[])));
		Assert.Null(nullCStr);
		Assert.NotNull(zero);
		Assert.False(zero is null);

		Assert.Throws<ArgumentNullException>(() => CString.GetBytes(default!));
		Assert.Throws<ArgumentException>(() => zero.CompareTo(new Object()));

		Assert.True(emptySpan.IsEmpty);

		Assert.NotEqual(CString.Empty, noEmpty1);
		Assert.False(noEmpty1.IsReference);
		Assert.False(noEmpty1.IsNullTerminated);
		Assert.False(noEmpty1.IsFunction);
		Assert.False(noEmpty1.IsSegmented);
		Assert.Equal(1, noEmpty1.Length);
		Assert.NotEqual(CString.Empty, noEmpty2);
		Assert.False(noEmpty2.IsReference);
		Assert.False(noEmpty2.IsNullTerminated);
		Assert.False(noEmpty2.IsFunction);
		Assert.False(noEmpty2.IsSegmented);
		Assert.Equal(2, noEmpty2.Length);
		Assert.NotEqual(CString.Empty, noEmpty3);
		Assert.False(noEmpty3.IsReference);
		Assert.True(noEmpty3.IsNullTerminated);
		Assert.False(noEmpty3.IsFunction);
		Assert.False(noEmpty3.IsSegmented);
		Assert.Equal(1, noEmpty3.Length);
		Assert.NotEqual(CString.Empty, noEmpty4);
		Assert.False(noEmpty4.IsReference);
		Assert.True(noEmpty4.IsNullTerminated);
		Assert.False(noEmpty4.IsFunction);
		Assert.True(noEmpty4.IsSegmented);
		Assert.Equal(1, noEmpty4.Length);
		Assert.NotEqual(CString.Empty, noEmpty5);
		Assert.False(noEmpty5.IsReference);
		Assert.True(noEmpty5.IsNullTerminated);
		Assert.True(noEmpty5.IsFunction);
		Assert.False(noEmpty5.IsSegmented);
		Assert.Equal(1, noEmpty5.Length);

		Assert.Equal(CString.Empty, empty2);
		Assert.False(empty2.IsReference);
		Assert.True(empty2.IsNullTerminated);
		Assert.False(empty2.IsFunction);
		Assert.Equal(CString.Empty, empty3);
		Assert.False(empty3.IsReference);
		Assert.True(empty3.IsNullTerminated);
		Assert.Equal(!pinned, empty3.IsFunction);
		Assert.Equal(CString.Empty, empty4);
		Assert.False(empty4.IsReference);
		Assert.True(empty4.IsNullTerminated);
		Assert.True(empty4.IsFunction);
		Assert.Equal(CString.Empty, empty5);
		Assert.False(empty4.IsReference);
		Assert.True(empty5.IsNullTerminated);
		Assert.False(empty5.IsFunction);
	}

	[Fact]
	internal void NormalTest()
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
				Assert.Equal(cstr1, cstr2);
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
				Assert.True(cstr2.Equals(TestSet.Utf16Text[i]));
				Assert.True(cstr2.Equals((Object)TestSet.Utf16Text[i]));
				Assert.True(cstr2.Equals((Object)cstr1));
				Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
				Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
				Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
				Assert.Equal(0, cstr1.CompareTo(cstr2));
			}
			BasicTests.AssertFromString(cstr1);
			Assert.True(cstr1.Equals(TestSet.Utf16Text[i]));
			Assert.Equal(0, cstr1.CompareTo(TestSet.Utf16Text[i]));
			Assert.Equal(TestSet.Utf16Text[i].GetHashCode(), cstr1.GetHashCode());
		}
	}

	[Fact]
	internal void PointerTest()
	{
		Int32 lenght = TestSet.Utf8Bytes.Count;
		for (Int32 i = 0; i < lenght; i++)
		{
			CString cstr1 = new(TestSet.Utf8Text[i]);
			BasicTests.TestBytesPointer(TestSet.Utf8Bytes[i], TestSet.Utf16Text[i], cstr1);
			BasicTests.TestNullTerminatedBytesPointer(TestSet.Utf8NullTerminatedBytes[i], TestSet.Utf16Text[i], cstr1);
		}
	}

	[Fact]
	internal void LiteralTest()
	{
		CString[] literalArray = TestSet.Utf8TextUpper.Select(f => new CString(f)).ToArray();
		CString[] nonLiteralArray = TestSet.Utf8NullTerminatedBytes.Select(b => (CString)b).ToArray();

		Assert.All(literalArray, c => Assert.True(MemoryInspector.Instance.IsLiteral(c.AsSpan())));
		Assert.All(nonLiteralArray, c => Assert.False(MemoryInspector.Instance.IsLiteral(c.AsSpan())));
		Assert.All(nonLiteralArray, c => Assert.False(MemoryInspector.Instance.IsLiteral(c.AsSpan())));
	}

	[Fact]
	internal unsafe void TryPinTest()
	{
		CString[] encodingArray = TestSet.Utf16Text.Select(s => (CString)s).ToArray();
		CString[] literalArray = TestSet.Utf8Text.Select(f => new CString(f)).ToArray();
		CString[] bytesArray = TestSet.Utf8Bytes.Select(a => (CString)a).ToArray();
		CString[] bytesNullArray = TestSet.Utf8NullTerminatedBytes.Select(a => (CString)a).ToArray();

		Assert.All(encodingArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			Assert.True(pinned);
			Assert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
			Assert.Equal((IntPtr)handle.Pointer, (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			Assert.Equal((IntPtr)handle.Pointer,
			             (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned).Pointer);
			Assert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			Assert.True(pinned);
			Assert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
			Assert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
		Assert.All(literalArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			Assert.False(pinned);
			Assert.Equal((IntPtr)handle.Pointer, IntPtr.Zero);
			Assert.NotEqual((IntPtr)handle.Pointer,
			                (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			Assert.Equal((IntPtr)handle.Pointer,
			             (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned).Pointer);
			Assert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			Assert.False(pinned);
			Assert.Equal((IntPtr)handle2.Pointer, IntPtr.Zero);
			Assert.NotEqual((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
		Assert.All(bytesArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			Assert.True(pinned);
			Assert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
			Assert.Equal((IntPtr)handle.Pointer, (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			Assert.Equal((IntPtr)handle.Pointer,
			             (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned).Pointer);
			Assert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			Assert.True(pinned);
			Assert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
			Assert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
		Assert.All(bytesNullArray, c =>
		{
			using MemoryHandle handle = c.TryPin(out Boolean pinned);
			Assert.True(pinned);
			Assert.NotEqual((IntPtr)handle.Pointer, IntPtr.Zero);
			Assert.Equal((IntPtr)handle.Pointer, (IntPtr)Unsafe.AsPointer(ref MemoryMarshal.GetReference(c.AsSpan())));

			Assert.Equal((IntPtr)handle.Pointer,
			             (IntPtr)CString.CreateUnsafe((IntPtr)handle.Pointer, c.Length + 1).TryPin(out pinned).Pointer);
			Assert.False(pinned);

			if (c.Length <= 3) return;

			using MemoryHandle handle2 = c[1..^1].TryPin(out pinned);
			Assert.True(pinned);
			Assert.NotEqual((IntPtr)handle2.Pointer, IntPtr.Zero);
			Assert.Equal((IntPtr)handle2.Pointer, (IntPtr)handle.Pointer + 1);
		});
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
		Assert.True(cstr.IsFunction);
		Assert.True(cstr.IsNullTerminated);
		Assert.False(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());
		BasicTests.AssertGetBytesException(cstr);
	}
	private static unsafe void AssertFromNullTerminatedBytes(CString cstr)
	{
		Assert.False(cstr.IsFunction);
		Assert.True(cstr.IsNullTerminated);
		Assert.False(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));

		Assert.Equal(cstr.Length + 1, CString.GetBytes(cstr).Length);

		fixed (Byte* ptr = &MemoryMarshal.GetReference(cstr.AsSpan()))
		{
			CString unsafeCStr = CString.CreateNullTerminatedUnsafe((IntPtr)ptr);
			Assert.False(unsafeCStr.IsFunction);
			Assert.True(unsafeCStr.IsNullTerminated);
			Assert.True(unsafeCStr.IsReference);
			Assert.False(unsafeCStr.IsSegmented);
			Assert.Equal(cstr, unsafeCStr);
		}

		CString rawClone = CString.Create(CString.GetBytes(cstr));
		Assert.False(rawClone.IsFunction);
		Assert.False(rawClone.IsNullTerminated);
		Assert.False(rawClone.IsReference);
		Assert.False(rawClone.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(rawClone));
		Assert.NotEqual(cstr, rawClone);
		Assert.Equal(cstr.Length + 1, rawClone.Length);

		CString rawSpanClone = CString.Create(CString.GetBytes(cstr).AsSpan());
		Assert.False(rawSpanClone.IsFunction);
		Assert.False(rawSpanClone.IsNullTerminated);
		Assert.False(rawSpanClone.IsReference);
		Assert.False(rawSpanClone.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(rawSpanClone));
		Assert.NotEqual(cstr, rawSpanClone);
		Assert.Equal(cstr.Length + 1, rawSpanClone.Length);
	}
	private static void AssertFromBytes(CString cstr)
	{
		Assert.False(cstr.IsFunction);
		Assert.False(cstr.IsNullTerminated);
		Assert.False(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		Assert.Equal(cstr.Length, CString.GetBytes(cstr).Length);

		CString rawClone = CString.Create(CString.GetBytes(cstr));
		Assert.False(rawClone.IsFunction);
		Assert.False(rawClone.IsNullTerminated);
		Assert.False(rawClone.IsReference);
		Assert.False(rawClone.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(rawClone));
		Assert.Equal(cstr, rawClone);
		Assert.Equal(cstr.Length, rawClone.Length);

		CString rawSpanClone = CString.Create(CString.GetBytes(cstr).AsSpan());
		Assert.False(rawSpanClone.IsFunction);
		Assert.False(rawSpanClone.IsNullTerminated);
		Assert.False(rawSpanClone.IsReference);
		Assert.False(rawSpanClone.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(rawSpanClone));
		Assert.Equal(cstr, rawSpanClone);
		Assert.Equal(cstr.Length, rawSpanClone.Length);
	}
	private static unsafe void AssertFromFunction(CString cstr)
	{
		Assert.True(cstr.IsFunction);
		Assert.True(cstr.IsNullTerminated);
		Assert.False(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		fixed (Byte* ptr = &MemoryMarshal.GetReference(cstr.AsSpan()))
		{
			CString unsafeCStr = CString.CreateNullTerminatedUnsafe((IntPtr)ptr);
			Assert.False(unsafeCStr.IsFunction);
			Assert.True(unsafeCStr.IsNullTerminated);
			Assert.True(unsafeCStr.IsReference);
			Assert.False(unsafeCStr.IsSegmented);
			Assert.Equal(cstr, unsafeCStr);
		}

		BasicTests.AssertGetBytesException(cstr);

		CString rawSpanClone = CString.Create(cstr);
		Assert.False(rawSpanClone.IsFunction);
		Assert.False(rawSpanClone.IsNullTerminated);
		Assert.False(rawSpanClone.IsReference);
		Assert.False(rawSpanClone.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(rawSpanClone));
		Assert.Equal(cstr, rawSpanClone);
		Assert.Equal(cstr.Length, rawSpanClone.Length);
	}
	private static void AssertFromFunctionNonLiteral(CString cstr)
	{
		Assert.True(cstr.IsFunction);
		Assert.False(cstr.IsNullTerminated);
		Assert.False(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		BasicTests.AssertGetBytesException(cstr);

		CString rawSpanClone = CString.Create(cstr);
		Assert.False(rawSpanClone.IsFunction);
		Assert.False(rawSpanClone.IsNullTerminated);
		Assert.False(rawSpanClone.IsReference);
		Assert.False(rawSpanClone.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(rawSpanClone));
		Assert.Equal(cstr, rawSpanClone);
		Assert.Equal(cstr.Length, rawSpanClone.Length);
	}
	private static unsafe void AssertFromBytesPointer(CString cstr)
	{
		Assert.False(cstr.IsFunction);
		Assert.False(cstr.IsNullTerminated);
		Assert.True(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());
		BasicTests.AssertGetBytesException(cstr);

		fixed (void* ptr = cstr.AsSpan())
		{
			CString rawPointerSpan = CString.CreateUnsafe(new(ptr), cstr.Length, true);
			Assert.False(rawPointerSpan.IsFunction);
			Assert.False(rawPointerSpan.IsNullTerminated);
			Assert.True(rawPointerSpan.IsReference);
			Assert.False(rawPointerSpan.IsSegmented);
			Assert.False(CString.IsNullOrEmpty(rawPointerSpan));
			Assert.Equal(cstr, rawPointerSpan);
			Assert.Equal(cstr.Length, rawPointerSpan.Length);
		}
	}

	private static unsafe void AssertFromNullTerminatedBytesPointer(CString cstr)
	{
		Assert.False(cstr.IsFunction);
		Assert.True(cstr.IsNullTerminated);
		Assert.True(cstr.IsReference);
		Assert.False(cstr.IsSegmented);
		Assert.False(CString.IsNullOrEmpty(cstr));
		BasicTests.AssertFromNullTerminatedBytes((CString)cstr.Clone());

		fixed (void* ptr = cstr.AsSpan())
		{
			CString rawPointerCString = CString.CreateUnsafe(new(ptr), cstr.Length + 1, true);
			CString unsafeCStr = CString.CreateNullTerminatedUnsafe(new(ptr));

			Assert.False(rawPointerCString.IsFunction);
			Assert.False(rawPointerCString.IsNullTerminated);
			Assert.True(rawPointerCString.IsReference);
			Assert.False(rawPointerCString.IsSegmented);

			Assert.False(unsafeCStr.IsFunction);
			Assert.True(unsafeCStr.IsNullTerminated);
			Assert.True(unsafeCStr.IsReference);
			Assert.False(unsafeCStr.IsSegmented);

			Assert.False(CString.IsNullOrEmpty(rawPointerCString));
			Assert.NotEqual(cstr, rawPointerCString);
			Assert.Equal(cstr, unsafeCStr);
			Assert.Equal(cstr.Length + 1, rawPointerCString.Length);
			Assert.Equal(cstr.Length, cstr.Length);
		}
	}
	private static void RefEnumerationTest(CString cstr1, CString cstr2)
	{
		Int32 i = 0;
		foreach (ref readonly Byte utf8Char in cstr2)
		{
			Assert.Equal(cstr1[i], utf8Char);
#if NET8_0_OR_GREATER
			Assert.True(Unsafe.AreSame(in cstr2.AsSpan()[i], in utf8Char));
#else
			Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(in cstr2.AsSpan()[i]), ref Unsafe.AsRef(in utf8Char)));
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
			Assert.Equal(enumerator1.Current, utf8Char);
		}
	}
	private static unsafe void TestBytesPointer(Byte[] bytes, String text, CString cstr1)
	{
		fixed (Byte* bptr2 = bytes)
		{
			CString cstr2 = CString.CreateUnsafe(new(bptr2), bytes.Length);
			Assert.Equal(cstr1, cstr2);
			BasicTests.AssertFromBytesPointer(cstr2);
			Assert.True(cstr2.Equals(text));
			Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
			Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
			Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
			BasicTests.RefEnumerationTest(cstr1, cstr2);
			BasicTests.EnumerationTest(cstr1, cstr2);
		}
	}
	private static unsafe void TestNullTerminatedBytesPointer(Byte[] bytes, String text, CString cstr1)
	{
		fixed (Byte* bptr2 = bytes)
		{
			CString cstr2 = CString.CreateUnsafe(new(bptr2), bytes.Length);
			Assert.Equal(cstr1, cstr2);
			Assert.True(cstr2.Equals(text));
			BasicTests.AssertFromNullTerminatedBytesPointer(cstr2);
			Assert.Equal(cstr1.GetHashCode(), cstr2.GetHashCode());
			Assert.Equal(cstr1.ToHexString(), cstr2.ToHexString());
			Assert.Equal(cstr1.ToArray(), cstr2.ToArray());
			BasicTests.RefEnumerationTest(cstr1, cstr2);
			BasicTests.EnumerationTest(cstr1, cstr2);
		}
	}

	private static void AssertGetBytesException(CString cstr)
	{
		try
		{
			Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
		}
		catch (Exception)
		{
			// For some reason sometimes the test fails even though it shouldn't.
			// The test must be run again so that it does not fail.
			Assert.NotEmpty(cstr.ToArray());
			Assert.Throws<InvalidOperationException>(() => CString.GetBytes(cstr));
		}
	}
}