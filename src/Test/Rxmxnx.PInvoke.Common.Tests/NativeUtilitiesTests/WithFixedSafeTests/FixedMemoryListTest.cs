#if !NETCOREAPP
using Fact = NUnit.Framework.TestAttribute;
#endif

using IEnumerator = System.Collections.IEnumerator;

namespace Rxmxnx.PInvoke.Tests.NativeUtilitiesTests.WithFixedSafeTests;

[TestFixture]
[ExcludeFromCodeCoverage]
public sealed class FixedMemoryListTest
{
	private static readonly IFixture fixture = new Fixture();

	private readonly Array[] _array = FixedMemoryListTest.GetArray();

	[Fact]
	public void EmptyTest()
	{
		FixedMemoryList fml = default;

		PInvokeAssert.Equal(0, fml.Count);
		PInvokeAssert.True(fml.IsEmpty);
		PInvokeAssert.Empty(fml.ToArray());
		fml.Unload();
	}

	[Fact]
	public void Test()
	{
		Span<Byte> s0 = (Span<Byte>)this._array[0]!;
		Span<Guid> s1 = (Span<Guid>)this._array[1]!;
		Span<Int16> s2 = (Span<Int16>)this._array[2]!;
		Span<Int32> s3 = (Span<Int32>)this._array[3]!;
		Span<Int64> s4 = (Span<Int64>)this._array[4]!;
		Span<SByte> s5 = (Span<SByte>)this._array[5]!;
		Span<UInt16> s6 = (Span<UInt16>)this._array[6]!;
		Span<UInt32> s7 = (Span<UInt32>)this._array[7]!;

		ReadOnlySpan<Byte> ros0 = (ReadOnlySpan<Byte>)this._array[0]!;
		ReadOnlySpan<Guid> ros1 = (ReadOnlySpan<Guid>)this._array[1]!;
		ReadOnlySpan<Int16> ros2 = (ReadOnlySpan<Int16>)this._array[2]!;
		ReadOnlySpan<Int32> ros3 = (ReadOnlySpan<Int32>)this._array[3]!;
		ReadOnlySpan<Int64> ros4 = (ReadOnlySpan<Int64>)this._array[4]!;
		ReadOnlySpan<SByte> ros5 = (ReadOnlySpan<SByte>)this._array[5]!;
		ReadOnlySpan<UInt16> ros6 = (ReadOnlySpan<UInt16>)this._array[6]!;
		ReadOnlySpan<UInt32> ros7 = (ReadOnlySpan<UInt32>)this._array[7]!;

		NativeUtilities.WithSafeFixed(s0, s1, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, this, FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, this, FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		NativeUtilities.WithSafeFixed(s0, s1, s2, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, s2, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, this, FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, this, FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, this, FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, this, FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, this, FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, this,
		                              FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, this, FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, this,
		                              FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, this, FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, this,
		                              FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, s7, this.ActionTest);
		NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, s7, this, FixedMemoryListTest.ActionTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, s7, this.ActionReadOnlyTest);
		NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, s7, this,
		                                      FixedMemoryListTest.ActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, ros7, this.ReadOnlyActionReadOnlyTest);
		NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, ros7, this,
		                              FixedMemoryListTest.ReadOnlyActionReadOnlyTest);

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, this.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, this.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, this, FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, this, FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, this.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, this.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, this, FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(
			NativeUtilities.WithSafeFixed(ros0, ros1, ros2, this, FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, this.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, this.FuncReadOnlyTest));
		this.ArrayTest(
			NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, this, FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, this,
		                                             FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, this.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, this.FuncReadOnlyTest));
		this.ArrayTest(
			NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, this, FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, this,
		                                             FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, this.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, this.FuncReadOnlyTest));
		this.ArrayTest(
			NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, this, FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(
			NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, this,
		                                             FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, this.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, this.FuncReadOnlyTest));
		this.ArrayTest(
			NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, this,
			                                      FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6,
		                                             this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, this,
		                                             FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));

		this.ArrayTest(NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, s7, this.FuncTest));
		this.ArrayTest(
			NativeUtilities.WithSafeFixed(s0, s1, s2, s3, s4, s5, s6, s7, this, FixedMemoryListTest.FuncTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, s7, this.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeReadOnlyFixed(s0, s1, s2, s3, s4, s5, s6, s7, this,
		                                                     FixedMemoryListTest.FuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, ros7,
		                                             this.ReadOnlyFuncReadOnlyTest));
		this.ArrayTest(NativeUtilities.WithSafeFixed(ros0, ros1, ros2, ros3, ros4, ros5, ros6, ros7, this,
		                                             FixedMemoryListTest.ReadOnlyFuncReadOnlyTest));
	}

	private void ActionTest(FixedMemoryList fml) => this.ActionFullTest(fml);
	private void ActionReadOnlyTest(ReadOnlyFixedMemoryList fml) => this.ActionTest(fml, false);
	private void ReadOnlyActionReadOnlyTest(ReadOnlyFixedMemoryList fml) => this.ActionTest(fml, true);
	private void ActionTest(ReadOnlyFixedMemoryList fml, Boolean readOnly)
	{
		PInvokeAssert.False(fml.IsEmpty);
		IReadOnlyFixedMemory[] mems = fml.ToArray();
		for (Int32 i = 0; i < fml.Count; i++)
		{
			switch (i)
			{
				case 0:
					FixedMemoryListTest.ArrayTest<Byte>(fml[0], this._array[0], readOnly);
					break;
				case 1:
					FixedMemoryListTest.ArrayTest<Guid>(fml[1], this._array[1], readOnly);
					break;
				case 2:
					FixedMemoryListTest.ArrayTest<Int16>(fml[2], this._array[2], readOnly);
					break;
				case 3:
					FixedMemoryListTest.ArrayTest<Int32>(fml[3], this._array[3], readOnly);
					break;
				case 4:
					FixedMemoryListTest.ArrayTest<Int64>(fml[4], this._array[4], readOnly);
					break;
				case 5:
					FixedMemoryListTest.ArrayTest<SByte>(fml[5], this._array[5], readOnly);
					break;
				case 6:
					FixedMemoryListTest.ArrayTest<UInt16>(fml[6], this._array[6], readOnly);
					break;
				case 7:
					FixedMemoryListTest.ArrayTest<UInt32>(fml[7], this._array[7], readOnly);
					break;
			}
			PInvokeAssert.Equal(fml[i], mems[i]);
		}
		FixedMemoryListTest.EnumeratorTest(fml);
	}
	private void ActionFullTest(FixedMemoryList fml)
	{
		PInvokeAssert.False(fml.IsEmpty);
		IFixedMemory[] mems = fml.ToArray();
		for (Int32 i = 0; i < fml.Count; i++)
		{
			switch (i)
			{
				case 0:
					FixedMemoryListTest.ArrayTest<Byte>(fml[0], this._array[0], false);
					break;
				case 1:
					FixedMemoryListTest.ArrayTest<Guid>(fml[1], this._array[1], false);
					break;
				case 2:
					FixedMemoryListTest.ArrayTest<Int16>(fml[2], this._array[2], false);
					break;
				case 3:
					FixedMemoryListTest.ArrayTest<Int32>(fml[3], this._array[3], false);
					break;
				case 4:
					FixedMemoryListTest.ArrayTest<Int64>(fml[4], this._array[4], false);
					break;
				case 5:
					FixedMemoryListTest.ArrayTest<SByte>(fml[5], this._array[5], false);
					break;
				case 6:
					FixedMemoryListTest.ArrayTest<UInt16>(fml[6], this._array[6], false);
					break;
				case 7:
					FixedMemoryListTest.ArrayTest<UInt32>(fml[7], this._array[7], false);
					break;
			}
			PInvokeAssert.Equal(fml[i], mems[i]);
		}
		FixedMemoryListTest.EnumeratorTest(fml);
	}
	private Byte[][] FuncTest(FixedMemoryList fml)
	{
		this.ActionFullTest(fml);
		Byte[][] result = new Byte[fml.Count][];
		for (Int32 i = 0; i < fml.Count; i++)
			result[i] = fml[i].Bytes.ToArray();
		return result;
	}
	private Byte[][] FuncReadOnlyTest(ReadOnlyFixedMemoryList fml)
	{
		this.ActionTest(fml, false);
		Byte[][] result = new Byte[fml.Count][];
		for (Int32 i = 0; i < fml.Count; i++)
			result[i] = fml[i].Bytes.ToArray();
		return result;
	}
	private Byte[][] ReadOnlyFuncReadOnlyTest(ReadOnlyFixedMemoryList fml)
	{
		this.ActionTest(fml, true);
		Byte[][] result = new Byte[fml.Count][];
		for (Int32 i = 0; i < fml.Count; i++)
			result[i] = fml[i].Bytes.ToArray();
		return result;
	}
	private void ArrayTest(Byte[][] bytes)
	{
		for (Int32 i = 0; i < bytes.Length; i++)
		{
			switch (i)
			{
				case 0:
					FixedMemoryListTest.ArrayTest<Byte>(bytes[0], this._array[0]);
					break;
				case 1:
					FixedMemoryListTest.ArrayTest<Guid>(bytes[1], this._array[1]);
					break;
				case 2:
					FixedMemoryListTest.ArrayTest<Int16>(bytes[2], this._array[2]);
					break;
				case 3:
					FixedMemoryListTest.ArrayTest<Int32>(bytes[3], this._array[3]);
					break;
				case 4:
					FixedMemoryListTest.ArrayTest<Int64>(bytes[4], this._array[4]);
					break;
				case 5:
					FixedMemoryListTest.ArrayTest<SByte>(bytes[5], this._array[5]);
					break;
				case 6:
					FixedMemoryListTest.ArrayTest<UInt16>(bytes[6], this._array[6]);
					break;
				case 7:
					FixedMemoryListTest.ArrayTest<UInt32>(bytes[7], this._array[7]);
					break;
			}
		}
	}

	private static Array[] GetArray()
	{
		Array[] arr = new Array[15];
		for (Int32 i = 0; i < arr.Length; i++)
		{
			arr[i] = i switch
			{
				00 => FixedMemoryListTest.fixture.CreateMany<Byte>(10).ToArray(),
				01 => FixedMemoryListTest.fixture.CreateMany<Guid>(10).ToArray(),
				02 => FixedMemoryListTest.fixture.CreateMany<Int16>(10).ToArray(),
				03 => FixedMemoryListTest.fixture.CreateMany<Int32>(10).ToArray(),
				04 => FixedMemoryListTest.fixture.CreateMany<Int64>(10).ToArray(),
				05 => FixedMemoryListTest.fixture.CreateMany<SByte>(10).ToArray(),
				06 => FixedMemoryListTest.fixture.CreateMany<UInt16>(10).ToArray(),
				07 => FixedMemoryListTest.fixture.CreateMany<UInt32>(10).ToArray(),
				_ => default!,
			};
		}
		return arr;
	}
	private static void ActionTest(FixedMemoryList fml, FixedMemoryListTest test) => test.ActionTest(fml, false);
	private static void ActionReadOnlyTest(ReadOnlyFixedMemoryList fml, FixedMemoryListTest test)
		=> test.ActionTest(fml, false);
	private static void ReadOnlyActionReadOnlyTest(ReadOnlyFixedMemoryList fml, FixedMemoryListTest test)
		=> test.ReadOnlyActionReadOnlyTest(fml);
	private static Byte[][] FuncTest(FixedMemoryList fml, FixedMemoryListTest test) => test.FuncTest(fml);
	private static Byte[][] FuncReadOnlyTest(ReadOnlyFixedMemoryList fml, FixedMemoryListTest test)
		=> test.FuncReadOnlyTest(fml);
	private static Byte[][] ReadOnlyFuncReadOnlyTest(ReadOnlyFixedMemoryList fml, FixedMemoryListTest test)
		=> test.ReadOnlyFuncReadOnlyTest(fml);
	private static unsafe void ArrayTest<T>(IReadOnlyFixedMemory mem, Array arr, Boolean readOnly) where T : unmanaged
	{
		IReadOnlyFixedContext<T> ctx = (IReadOnlyFixedContext<T>)mem;
		T[] arrT = (T[])arr;

		PInvokeAssert.Equal(arrT, ctx.Values.ToArray());
		PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arrT.AsSpan()),
		                                  ref MemoryMarshal.GetReference(ctx.Values)));
		fixed (void* ptr = arrT)
			PInvokeAssert.Equal((IntPtr)ptr, ctx.Pointer);

		if (!readOnly)
		{
			IFixedContext<T> ctxx = (IFixedContext<T>)mem;
			PInvokeAssert.Equal(arrT, ctxx.Values.ToArray());
			PInvokeAssert.Equal(mem.Bytes.ToArray(), ctxx.Bytes.ToArray());
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(arrT.AsSpan()),
			                                  ref MemoryMarshal.GetReference(ctxx.Values)));
			PInvokeAssert.True(Unsafe.AreSame(ref MemoryMarshal.GetReference(mem.Bytes),
			                                  ref MemoryMarshal.GetReference(ctxx.Bytes)));
		}
		else
		{
			PInvokeAssert.Throws<InvalidCastException>(() => (IFixedContext<T>)mem);
		}
	}
	private static void ArrayTest<T>(Byte[] bytes, Array arr) where T : unmanaged
	{
		T[] arrT = (T[])arr;
		ReadOnlySpan<Byte> spanByte = MemoryMarshal.AsBytes(arrT.AsSpan());
		ReadOnlySpan<T> spanT = MemoryMarshal.Cast<Byte, T>(bytes);

		PInvokeAssert.Equal(bytes, spanByte.ToArray());
		PInvokeAssert.Equal(arrT, spanT.ToArray());
	}
	private static void EnumeratorTest(ReadOnlyFixedMemoryList fml)
	{
		IEnumerator arrEnumerator = fml.ToArray().GetEnumerator();
		using IDisposable? unknown = arrEnumerator as IDisposable;
		ReadOnlyFixedMemoryList.Enumerator fmlEnumerator = fml.GetEnumerator();

		while (fmlEnumerator.MoveNext() && arrEnumerator.MoveNext())
			PInvokeAssert.Equal(arrEnumerator.Current, fmlEnumerator.Current);
	}
	private static void EnumeratorTest(FixedMemoryList fml)
	{
		IEnumerator arrEnumerator = fml.ToArray().GetEnumerator();
		using IDisposable? unknown = arrEnumerator as IDisposable;
		FixedMemoryList.Enumerator fmlEnumerator = fml.GetEnumerator();

		while (fmlEnumerator.MoveNext() && arrEnumerator.MoveNext())
			PInvokeAssert.Equal(arrEnumerator.Current, fmlEnumerator.Current);
	}
}