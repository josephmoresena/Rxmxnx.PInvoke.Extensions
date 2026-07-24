namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
public unsafe partial class NativeUtilities
#else
public partial class NativeUtilities
#endif
{
	/// <summary>
	/// Retrieves the decimal value of <paramref name="hexCharacter"/>.
	/// </summary>
	/// <param name="hexCharacter">ASCII hexadecimal character.</param>
	/// <returns>Decimal value from <paramref name="hexCharacter"/>.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static Byte GetDecimalValue(Byte hexCharacter)
		=> hexCharacter switch
		{
			(Byte)'a' => 10,
			(Byte)'A' => 10,
			(Byte)'b' => 11,
			(Byte)'B' => 11,
			(Byte)'c' => 12,
			(Byte)'C' => 12,
			(Byte)'d' => 13,
			(Byte)'D' => 13,
			(Byte)'e' => 14,
			(Byte)'E' => 14,
			(Byte)'f' => 15,
			(Byte)'F' => 15,
			(Byte)'1' => 1,
			(Byte)'2' => 2,
			(Byte)'3' => 3,
			(Byte)'4' => 4,
			(Byte)'5' => 5,
			(Byte)'6' => 6,
			(Byte)'7' => 7,
			(Byte)'8' => 8,
			(Byte)'9' => 9,
			_ => 0,
		};
	/// <summary>
	/// Returns a reference to the element of the array at index 0.
	/// </summary>
	/// <typeparam name="T">The type of items in the array.</typeparam>
	/// <param name="array">A <see typeparamref="T"/> array.</param>
	/// <returns>A reference to the element at index 0.</returns>
	internal static ref T GetArrayDataReference<T>(T[] array)
		=> ref array.Length > 0 ?
			ref Unsafe.AsRef(in array[0]) :
			ref MemoryMarshal.GetReference(new ReadOnlyMemory<T>(array).Span);
	/// <summary>
	/// Determines whether the specified <see cref="MethodBase"/> represents executable code that originates
	/// from a statically compiled image (AOT/R2R) rather than dynamically generated runtime code.
	/// </summary>
	/// <param name="methodBase">The <see langword="MethodBase"/> to evaluate.</param>
	/// <returns>
	/// <see langword="true"/> if the method is backed by image-compiled code; otherwise, <see langword="false"/>.
	/// </returns>
	/// <remarks>
	/// Returns <see langword="false"/> for open generic methods and on platforms where memory inspection is not supported.
	/// In reflection-free runtimes, valid method handles are treated as image-backed code.
	/// </remarks>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	internal static Boolean IsImageMethodUnsafe(MethodBase methodBase)
	{
		if (methodBase.ContainsGenericParameters || AotInfo.IsDynamicCode(methodBase)) return false;
		return AotInfo.IsImageMethodUnsafe(methodBase.MethodHandle);
	}
	/// <summary>
	/// Retrieves a concurrent value from <paramref name="fieldReference"/>.
	/// </summary>
	/// <typeparam name="T">Type of the concurrent field.</typeparam>
	/// <param name="fieldReference">Reference. A <typeparamref name="T"/> value.</param>
	/// <returns>Concurrent value instance.</returns>
	internal static T GetConcurrentObject<T>(ref T? fieldReference) where T : class, new()
	{
		if (Volatile.Read(ref fieldReference) is { } existing) return existing;
		T newObj = new();
		T? previous = Interlocked.CompareExchange(ref fieldReference, newObj, null);
		return previous ?? newObj;
	}
#if !NET5_0_OR_GREATER
	/// <summary>
	/// Creates a <see cref="Type"/> span from <typeparamref name="TBuffer"/> reference.
	/// </summary>
	/// <typeparam name="TBuffer">A <see cref="ValueType"/> buffer type.</typeparam>
	/// <param name="buffer">Managed reference to <typeparamref name="TBuffer"/> value.</param>
	/// <returns>Created <see cref="Type"/> span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Span<Type> CreateTypeSpan<TBuffer>(ref TBuffer buffer)
#if !PACKAGE
		where TBuffer : struct
#else
		where TBuffer : struct, IManagedBinaryBuffer<Object>
#endif
	{
#if !PACKAGE
#pragma warning disable CS8500
		Int32 length = sizeof(TBuffer) / IntPtr.Size;
#pragma warning restore CS8500
#else
		Int32 length = buffer.Metadata.Size;
#endif
		ref Type r0 = ref Unsafe.As<TBuffer, Type>(ref buffer);
		return MemoryMarshal.CreateSpan(ref r0, length);
	}
	/// <summary>
	/// Creates a <see cref="Func{IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory}"/> span from
	/// <typeparamref name="TBuffer"/> reference.
	/// </summary>
	/// <typeparam name="TBuffer">A <see cref="ValueType"/> buffer type.</typeparam>
	/// <param name="buffer">Managed reference to <typeparamref name="TBuffer"/> value.</param>
	/// <returns>Created <see cref="Func{IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory}"/> span.</returns>
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	internal static Span<Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>> CreateConstructorSpan<TBuffer>(
		ref TBuffer buffer)
#if !PACKAGE
		where TBuffer : struct
#else
		where TBuffer : struct, IManagedBinaryBuffer<Object>
#endif
	{
#if !PACKAGE
#pragma warning disable CS8500
		Int32 length = sizeof(TBuffer) / IntPtr.Size;
#pragma warning restore CS8500
#else
		Int32 length = buffer.Metadata.Size;
#endif
		ref Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory> r0 =
			ref Unsafe.As<TBuffer, Func<IntPtr, Int32, FixedValueHandle, ReadOnlyFixedMemory>>(ref buffer);
		return MemoryMarshal.CreateSpan(ref r0, length);
	}
#else
	/// <summary>
	/// Generic <see langword="typeof"/> call.
	/// </summary>
	/// <typeparam name="T">Generic type.</typeparam>
	/// <returns>The CLR type for <typeparamref name="T"/>.</returns>
	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static Type GetType<T>() => typeof(T);
#endif
}