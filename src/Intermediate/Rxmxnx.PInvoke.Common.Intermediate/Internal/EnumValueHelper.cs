#if NETFRAMEWORK && !NET46_OR_GREATER
using Array = Rxmxnx.PInvoke.Internal.FrameworkCompat.ArrayCompat;
#endif
#if !NETCOREAPP2_1_OR_GREATER
using MemoryMarshal = Rxmxnx.PInvoke.Internal.FrameworkCompat.MemoryMarshalCompat;
#endif

namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Value <see cref="Enum"/> helper.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS2743)]
#endif
internal static unsafe class EnumValueHelper<TEnum> where TEnum : struct, Enum
{
	/// <summary>
	/// Internal array.
	/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	private static readonly ReadOnlyMemory<TEnum> memory;

	/// <summary>
	/// Size of <typeparamref name="TEnum"/>.
	/// </summary>
#pragma warning disable CS8500
	// ReSharper disable once MemberCanBePrivate.Global
	public static readonly Int32 SizeOf = sizeof(TEnum);
#pragma warning restore CS8500

	/// <summary>
	/// Represents the total number of unique values defined in the enumeration <typeparamref name="TEnum"/>.
	/// </summary>
	// ReSharper disable once StaticMemberInGenericType
	public static Int32 Count { get; }
	/// <summary>
	/// Provides a readonly span of values of type <typeparamref name="TEnum"/>.
	/// </summary>
	public static ReadOnlySpan<TEnum> Span
	{
#if NETFRAMEWORK || NETSTANDARD2_0
		[SecuritySafeCritical]
#endif
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		get
		{
			// ReSharper disable once ConvertIfStatementToReturnStatement
			if (EnumValueHelper<TEnum>.Count == 0) return default;
#if NET7_0_OR_GREATER
			return EnumValueHelper<TEnum>.memory.Span;
#else
			ReadOnlyMemory<TEnum> m = EnumValueHelper<TEnum>.memory;
			return EnumValueHelper<TEnum>.SizeOf switch
			{
				sizeof(Byte) => MemoryMarshal.Cast<Byte, TEnum>(EnumValueHelper<TEnum>.CastMemory<Byte>(m).Span),
				sizeof(UInt16) => MemoryMarshal.Cast<UInt16, TEnum>(EnumValueHelper<TEnum>.CastMemory<UInt16>(m).Span),
				sizeof(UInt32) => MemoryMarshal.Cast<UInt32, TEnum>(EnumValueHelper<TEnum>.CastMemory<UInt32>(m).Span),
				sizeof(UInt64) => MemoryMarshal.Cast<UInt64, TEnum>(EnumValueHelper<TEnum>.CastMemory<UInt64>(m).Span),
				_ => m.Span,
			};
#endif
		}
	}

	/// <summary>
	/// Static constructor.
	/// </summary>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3963)]
#endif
	static EnumValueHelper()
	{
#if NET5_0_OR_GREATER
		TEnum[] values = Enum.GetValues<TEnum>();
#else
		TEnum[] values = (TEnum[])Enum.GetValues(typeof(TEnum));
#endif
		EnumValueHelper<TEnum>.Count = values.Length;

		if (EnumValueHelper<TEnum>.Count == 0) return;
#if NET7_0_OR_GREATER
		EnumValueHelper<TEnum>.memory = new(values);
#else
		EnumValueHelper<TEnum>.memory = EnumValueHelper<TEnum>.SizeOf switch
		{
			sizeof(Byte) => EnumValueHelper<TEnum>.CreateMemory<Byte>(values),
			sizeof(UInt16) => EnumValueHelper<TEnum>.CreateMemory<UInt16>(values),
			sizeof(UInt32) => EnumValueHelper<TEnum>.CreateMemory<UInt32>(values),
			sizeof(UInt64) => EnumValueHelper<TEnum>.CreateMemory<UInt64>(values),
			_ => new(values),
		};
#endif
	}

	/// <summary>
	/// Pins the internal memory of the current <typeparamref name="TEnum"/> array
	/// to a fixed memory address, enabling its usage in unmanaged code.
	/// </summary>
	/// <returns>A <see cref="MemoryHandle"/> structure representing the pinned memory.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static MemoryHandle Pin()
	{
		if (EnumValueHelper<TEnum>.Count == 0) return default;
		ReadOnlyMemory<TEnum> m = EnumValueHelper<TEnum>.memory;
#if NET7_0_OR_GREATER
		return m.Pin();
#else
		return EnumValueHelper<TEnum>.SizeOf switch
		{
			sizeof(Byte) => EnumValueHelper<TEnum>.CastMemory<Byte>(m).Pin(),
			sizeof(UInt16) => EnumValueHelper<TEnum>.CastMemory<UInt16>(m).Pin(),
			sizeof(UInt32) => EnumValueHelper<TEnum>.CastMemory<UInt32>(m).Pin(),
			sizeof(UInt64) => EnumValueHelper<TEnum>.CastMemory<UInt64>(m).Pin(),
			_ => m.Pin(),
		};
#endif
	}

#if !NET7_0_OR_GREATER
	/// <summary>
	/// Creates memory from the specified array of enum values, using a specified unmanaged primitive type for internal
	/// representation.
	/// </summary>
	/// <typeparam name="TPrimitive">The unmanaged primitive type used for memory layout.</typeparam>
	/// <param name="values">The array of enum values to create memory from.</param>
	/// <returns>A read-only memory instance containing the enum values.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	private static ReadOnlyMemory<TEnum> CreateMemory<TPrimitive>(TEnum[] values) where TPrimitive : unmanaged
	{
		if (values.Length == 0) return Array.Empty<TEnum>();
		TPrimitive[] newArray = new TPrimitive[values.Length];
		MemoryMarshal.Cast<TEnum, Byte>(values).CopyTo(MemoryMarshal.Cast<TPrimitive, Byte>(newArray.AsSpan()));
		return new(Unsafe.As<TPrimitive[], TEnum[]>(ref newArray));
	}
	/// <summary>
	/// Casts the provided memory of the current enum type to a memory of the provided unmanaged primitive type.
	/// </summary>
	/// <typeparam name="TPrimitive">The unmanaged primitive type to cast the memory to.</typeparam>
	/// <param name="unsafeMem">The memory representing the current enum type to be cast.</param>
	/// <returns>A <see cref="ReadOnlyMemory{TPrimitive}"/> representing the cast memory.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	private static ReadOnlyMemory<TPrimitive> CastMemory<TPrimitive>(ReadOnlyMemory<TEnum> unsafeMem)
		where TPrimitive : unmanaged
		=> Unsafe.As<ReadOnlyMemory<TEnum>, ReadOnlyMemory<TPrimitive>>(ref unsafeMem);
#endif
}