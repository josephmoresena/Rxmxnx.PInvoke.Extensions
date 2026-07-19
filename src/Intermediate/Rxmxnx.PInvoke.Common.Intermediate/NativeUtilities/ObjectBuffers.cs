#pragma warning disable CS0169
#if PACKAGE
using B2 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
		Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>;
using B3 =
	Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		System.Object>;
using B4 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		System.Object>;
using B5 = Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.
	Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using B6 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
			Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using B7 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				System.Object>,
			System.Object>, Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
using B8 =
	Rxmxnx.PInvoke.Buffers.Composite<
		Rxmxnx.PInvoke.Buffers.Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, Rxmxnx.PInvoke.Buffers.
		Composite<
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>,
			Rxmxnx.PInvoke.Buffers.Composite<Rxmxnx.PInvoke.Buffers.Atomic<System.Object>,
				Rxmxnx.PInvoke.Buffers.Atomic<System.Object>, System.Object>, System.Object>, System.Object>;
#endif

namespace Rxmxnx.PInvoke;

public partial class NativeUtilities
{
	/// <summary>
	/// Internal buffer of 2 objects.
	/// </summary>
	private struct Buffer2
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B2 _m;
#endif
	}

	/// <summary>
	/// Internal buffer of 3 objects.
	/// </summary>
	private struct Buffer3
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
		private Object _2;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B3 _m;
#endif
	}

	/// <summary>
	/// Internal buffer of 4 objects.
	/// </summary>
	private struct Buffer4
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
		private Object _2;
		private Object _3;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B4 _m;
#endif
	}

	/// <summary>
	/// Internal buffer of 5 objects.
	/// </summary>
	private struct Buffer5
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
		private Object _2;
		private Object _3;
		private Object _4;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B5 _m;
#endif
	}

	/// <summary>
	/// Internal buffer of 6 objects.
	/// </summary>
	private struct Buffer6
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
		private Object _2;
		private Object _3;
		private Object _4;
		private Object _5;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B6 _m;
#endif
	}

	/// <summary>
	/// Internal buffer of 7 objects.
	/// </summary>
	private struct Buffer7
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
		private Object _2;
		private Object _3;
		private Object _4;
		private Object _5;
		private Object _6;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B7 _m;
#endif
	}

	/// <summary>
	/// Internal buffer of 8 objects.
	/// </summary>
	private struct Buffer8
	{
#if !PACKAGE
		private Object _0;
		private Object _1;
		private Object _2;
		private Object _3;
		private Object _4;
		private Object _5;
		private Object _6;
		private Object _7;
#else
		/// <summary>
		/// Buffer instance.
		/// </summary>
		private B8 _m;
#endif
	}
}