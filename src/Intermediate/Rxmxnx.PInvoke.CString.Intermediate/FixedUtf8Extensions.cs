namespace Rxmxnx.PInvoke;

/// <summary>
/// Provides a set of extensions for basic fixing operations with UTF-8 texts.
/// </summary>
#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
[Browsable(false)]
#endif
[EditorBrowsable(EditorBrowsableState.Never)]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static unsafe class FixedUtf8Extensions
{
	/// <summary>
	/// Calculates the number of UTF-8 units produced by the encoding the characters in the specified
	/// <see cref="String"/>.
	/// </summary>
	/// <param name="value">A <see cref="String"/> instance.</param>
	/// <returns>The number of UTF-8 units produced by encoding the specified <see cref="String"/>.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 GetUtf8Count(this String? value)
		=> !String.IsNullOrEmpty(value) ? value.AsSpan().GetUtf8Count() : default;
	/// <summary>
	/// Calculates the number of UTF-8 units produced by the encoding the characters in the specified character span.
	/// </summary>
	/// <param name="chars">The span of characters to encode.</param>
	/// <returns>The number of UTF-8 units produced by encoding the specified character span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 GetUtf8Count(this ReadOnlySpan<Char> chars)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> Encoding.UTF8.GetByteCount(chars);
#else
	{
		if (chars.IsEmpty) return default;
		fixed (Char* ptr = &MemoryMarshal.GetReference(chars))
			return Encoding.UTF8.GetByteCount(ptr, chars.Length);
	}
#endif
	/// <summary>
	/// Calculates the number of characters produced by decoding the <paramref name="source"/> as UTF-8 units.
	/// </summary>
	/// <param name="source">A read-only byte span to decode.</param>
	/// <returns>The number of characters produced by decoding the UTF-8 encoded text.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 GetUtf16Count(this ReadOnlySpan<Byte> source)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> Encoding.UTF8.GetCharCount(source);
#else
	{
		fixed (Byte* ptr = &MemoryMarshal.GetReference(source))
			return Encoding.UTF8.GetCharCount(ptr, source.Length);
	}
#endif

	/// <summary>
	/// Calculates the number of characters produced by decoding the <paramref name="source"/>.
	/// </summary>
	/// <param name="source">A read-only span of <see cref="byte"/> elements representing a UTF-8 encoded text.</param>
	/// <returns>The number of characters produced by decoding the UTF-8 encoded text.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 GetCharCountFromUtf8(ReadOnlySpan<Byte> source)
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		=> Encoding.UTF8.GetCharCount(source);
#else
	{
		fixed (Byte* ptr = &MemoryMarshal.GetReference(source))
			return Encoding.UTF8.GetCharCount(ptr, source.Length);
	}
#endif
	/// <summary>
	/// Decodes a read-only span of UTF-8 encoded bytes into a UTF-16 encoded <see cref="String"/>.
	/// </summary>
	/// <param name="bytes">The read-only byte span containing the UTF-8 text to decode.</param>
	/// <returns>A new <see cref="String"/> instance containing the decoded text.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static String ToUtf16(this ReadOnlySpan<Byte> bytes)
	{
		// ReSharper disable once ConvertIfStatementToReturnStatement
		if (bytes.IsEmpty) return String.Empty;
#if NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
		return Encoding.UTF8.GetString(bytes);
#elif NETSTANDARD1_3_OR_GREATER || NETCOREAPP || NET46_OR_GREATER || UAP10_0
		fixed (Byte* ptr = &MemoryMarshal.GetReference(bytes))
			return Encoding.UTF8.GetString(ptr, bytes.Length);
#else
		Int32 charCount = Encoding.UTF8.GetMaxCharCount(bytes.Length);
		Int32 byteCount = 2 * charCount;
		if (byteCount <= StackAllocationHelper.StackallocByteThreshold)
			return FixedUtf8Extensions.DecodeUtf8(bytes, stackalloc Char[charCount]);
		IntPtr ptr = Marshal.AllocHGlobal(byteCount);
		try
		{
			return FixedUtf8Extensions.DecodeUtf8(bytes, new(ptr.ToPointer(), charCount));
		}
		finally
		{
			Marshal.FreeHGlobal(ptr);
		}
#endif
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CString? cstr, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IReadOnlyFixedContextAction<Byte>
#else
		where TAction : IReadOnlyFixedContextAction<Byte>, allows ref struct
#endif
	{
		if (cstr is not null && action is not null)
			fixed (void* ptr = cstr)
				action.Accept(new(ptr, cstr.Length));
		else if (action is not null)
			action.Accept(default);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified action has completed.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IReadOnlyFixedContextAction{T}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the action.</param>
	/// <param name="action">A <typeparamref name="TAction"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CString? cstr, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IReadOnlyFixedContextAction<Byte>
#else
		where TAction : struct, IReadOnlyFixedContextAction<Byte>, allows ref struct
#endif
	{
		if (cstr is not null)
			fixed (void* ptr = cstr)
				action.Accept(new(ptr, cstr.Length));
		else
			action.Accept(default);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the function.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TResult, TFunction>(this CString? cstr, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IReadOnlyFixedContextFunction<Byte, TResult>
#else
		where TFunction : IReadOnlyFixedContextFunction<Byte, TResult>, allows ref struct
#endif
	{
		if (cstr is not null && func is not null)
			fixed (void* ptr = cstr)
				result = func.Apply(new(ptr, cstr.Length));
		else if (func is not null)
			result = func.Apply(default);
		else
			Unsafe.SkipInit(out result);
	}
	/// <summary>
	/// Prevents the garbage collector from relocating the current UTF-8 string by pinning its memory
	/// address until the specified function has completed.
	/// </summary>
	/// <typeparam name="TResult">The type of the value returned by the function <paramref name="func"/>.</typeparam>
	/// <typeparam name="TFunction">Type of <see cref="IFixedContextFunction{T,TResult}"/>.</typeparam>
	/// <param name="cstr">The <see cref="CString"/> instance to pin during the function.</param>
	/// <param name="func">A <typeparamref name="TFunction"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TResult, TFunction>(this CString? cstr, ref TFunction func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : struct, IReadOnlyFixedContextFunction<Byte, TResult>
#else
		where TFunction : struct, IReadOnlyFixedContextFunction<Byte, TResult>, allows ref struct
#endif
	{
		if (cstr is not null)
			fixed (void* ptr = cstr)
				result = func.Apply(new(ptr, cstr.Length));
		else
			result = func.Apply(default);
	}
#pragma warning disable CS8500
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes their
	/// memory addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CStringSequence? seq, TAction? action)
#if !NET9_0_OR_GREATER
		where TAction : IFixedPointerListAction
#else
		where TAction : IFixedPointerListAction, allows ref struct
#endif
	{
		if (action is null) return;
		if (seq is null || seq.Count == 0)
		{
			action.Accept(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Information =
#if NETCOREAPP3_0_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(seq, stackalloc FixedPointerInfo[seq.Count]),
#elif NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(
						seq, MemoryMarshal.CreateSpan(
							ref Unsafe.As<Byte, FixedPointerInfo>(
								ref MemoryMarshal.GetReference(stackalloc Byte[seq.Count * sizeof(FixedPointerInfo)])),
							seq.Count)),
#else
					FixedUtf8Extensions.InitializeInfo(seq, Unsafe.AsPointer(
						                                   ref MemoryMarshal.GetReference(
							                                   stackalloc Byte[seq.Count *
								                                   sizeof(FixedPointerInfo)]))),
#endif
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes their
	/// memory addresses until <paramref name="action"/> completes.
	/// </summary>
	/// <typeparam name="TAction">Type of <see cref="IFixedPointerListAction"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="action">A <see cref="IFixedPointerListAction"/> instance.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TAction>(this CStringSequence? seq, ref TAction action)
#if !NET9_0_OR_GREATER
		where TAction : struct, IFixedPointerListAction
#else
		where TAction : struct, IFixedPointerListAction, allows ref struct
#endif
	{
		if (seq is null || seq.Count == 0)
		{
			action.Accept(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			action.Accept(new()
			{
				IsReadOnly = true,
				Information =
#if NETCOREAPP3_0_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(seq, stackalloc FixedPointerInfo[seq.Count]),
#elif NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(
						seq, MemoryMarshal.CreateSpan(
							ref Unsafe.As<Byte, FixedPointerInfo>(
								ref MemoryMarshal.GetReference(stackalloc Byte[seq.Count * sizeof(FixedPointerInfo)])),
							seq.Count)),
#else
					FixedUtf8Extensions.InitializeInfo(seq, Unsafe.AsPointer(
						                                   ref MemoryMarshal.GetReference(
							                                   stackalloc Byte[seq.Count *
								                                   sizeof(FixedPointerInfo)]))),
#endif
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes
	/// their memory addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult>(this CStringSequence? seq, TFunction? func, out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		if (func is null)
		{
			Unsafe.SkipInit(out result);
			return;
		}
		if (seq is null || seq.Count == 0)
		{
			result = func.Apply(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Information =
#if NETCOREAPP3_0_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(seq, stackalloc FixedPointerInfo[seq.Count]),
#elif NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(
						seq, MemoryMarshal.CreateSpan(
							ref Unsafe.As<Byte, FixedPointerInfo>(
								ref MemoryMarshal.GetReference(stackalloc Byte[seq.Count * sizeof(FixedPointerInfo)])),
							seq.Count)),
#else
					FixedUtf8Extensions.InitializeInfo(seq, Unsafe.AsPointer(
						                                   ref MemoryMarshal.GetReference(
							                                   stackalloc Byte[seq.Count *
								                                   sizeof(FixedPointerInfo)]))),
#endif
			});
		}
	}
	/// <summary>
	/// Prevents the garbage collector from reallocating given <see cref="CStringSequence"/> elements and fixes
	/// their memory addresses until <paramref name="func"/> completes.
	/// </summary>
	/// <typeparam name="TFunction">Type of <see cref="IFixedPointerListFunction{TResult}"/>.</typeparam>
	/// <typeparam name="TResult">The type of the return value of <paramref name="func"/>.</typeparam>
	/// <param name="seq">Current <see cref="CStringSequence"/> instance.</param>
	/// <param name="func">A <see cref="IFixedPointerListFunction{TResult}"/> instance.</param>
	/// <param name="result">Output. Function result.</param>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WithSafeFixed<TFunction, TResult>(this CStringSequence? seq, ref TFunction func,
		out TResult result)
#if !NET9_0_OR_GREATER
		where TFunction : IFixedPointerListFunction<TResult>
#else
		where TFunction : IFixedPointerListFunction<TResult>, allows ref struct
#endif
	{
		if (seq is null || seq.Count == 0)
		{
			result = func.Apply(default);
			return;
		}
		fixed (void* _ = &seq.GetPinnableReference())
		{
			result = func.Apply(new()
			{
				IsReadOnly = true,
				Information =
#if NETCOREAPP3_0_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(seq, stackalloc FixedPointerInfo[seq.Count]),
#elif NETSTANDARD2_1 || NETCOREAPP2_1_OR_GREATER
					FixedUtf8Extensions.InitializeInfo(
						seq, MemoryMarshal.CreateSpan(
							ref Unsafe.As<Byte, FixedPointerInfo>(
								ref MemoryMarshal.GetReference(stackalloc Byte[seq.Count * sizeof(FixedPointerInfo)])),
							seq.Count)),
#else
					FixedUtf8Extensions.InitializeInfo(seq, Unsafe.AsPointer(
						                                   ref MemoryMarshal.GetReference(
							                                   stackalloc Byte[seq.Count *
								                                   sizeof(FixedPointerInfo)]))),
#endif
			});
		}
	}
#pragma warning restore CS8500

#if !NETSTANDARD2_1 && !NETCOREAPP2_1_OR_GREATER
	/// <summary>
	/// Initializes the <paramref name="bytePtr"/> span with <paramref name="source"/> elements information.
	/// </summary>
	/// <param name="source">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="bytePtr">Destination pointer.</param>
	/// <returns>Initialized <see cref="FixedPointerInfo"/> read-only span.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<FixedPointerInfo> InitializeInfo(CStringSequence source, void* bytePtr)
	{
		Span<FixedPointerInfo> info = MemoryMarshalCompat.CreateUnsafeSpan<FixedPointerInfo>(bytePtr, source.Count);
		return FixedUtf8Extensions.InitializeInfo(source, info);
	}
#endif
	/// <summary>
	/// Initializes the <paramref name="span"/> span with <paramref name="source"/> elements information.
	/// </summary>
	/// <param name="source">A <see cref="CStringSequence"/> instance.</param>
	/// <param name="span">Destination span.</param>
	/// <returns>Initialized <see cref="FixedPointerInfo"/> read-only span.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private static ReadOnlySpan<FixedPointerInfo> InitializeInfo(CStringSequence source, Span<FixedPointerInfo> span)
	{
		CStringSequence.Utf8View view = new(source, true);
		Int32 i = 0;
		foreach (ReadOnlySpan<Byte> value in view)
		{
			span[i++] = new()
			{
				Pointer = Unsafe.AsPointer(ref MemoryMarshal.GetReference(value)),
				Count = value.Length,
				SizeOf = sizeof(Byte),
				IsUnmanaged = true,
#if NETSTANDARD2_1 || NETCOREAPP3_0_OR_GREATER
				ConstructorOrFunctionPointer = default,
#endif
#if !NET5_0_OR_GREATER
				TypeOrFunctionPointer = CStringSequence.TypePointer,
#else
				TypeOrFunctionPointer = FixedPointerInfo.ToUnmanaged(&NativeUtilities.GetType<Byte>),
#endif
			};
		}
		return span;
	}
#if !NETSTANDARD1_3_OR_GREATER && !NETCOREAPP && !NET46_OR_GREATER && !UAP10_0
	/// <summary>
	/// Creates a string using <paramref name="chars"/> as temporal buffer.
	/// </summary>
	/// <param name="bytes">The read-only byte span containing the UTF-8 text to decode.</param>
	/// <param name="chars">Temporal UTF-16 buffer.</param>
	/// <returns>A new <see cref="String"/> instance containing the decoded text.</returns>
	private static String DecodeUtf8(ReadOnlySpan<Byte> bytes, Span<Char> chars)
	{
		Int32 strLen;
		fixed (Byte* b = &MemoryMarshal.GetReference(bytes))
		fixed (Char* c = &MemoryMarshal.GetReference(chars))
			strLen = Encoding.UTF8.GetChars(b, bytes.Length, c, chars.Length);
		return chars[..strLen].ToString();
	}
#endif
}