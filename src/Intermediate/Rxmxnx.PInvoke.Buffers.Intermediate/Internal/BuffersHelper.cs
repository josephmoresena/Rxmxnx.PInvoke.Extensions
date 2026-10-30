namespace Rxmxnx.PInvoke.Internal;

/// <summary>
/// Static class helper.
/// </summary>
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
#endif
internal static class BuffersHelper
{
	/// <summary>
	/// Name of <see cref="Atomic{T}.TypeMetadata"/>  static field.
	/// </summary>
	public const String TypeMetadataName = nameof(Atomic<>.TypeMetadata);
	/// <summary>
	/// Flags of metadata static member.
	/// </summary>
	public const BindingFlags GetMetadataFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
	/// <summary>
	/// Flags of dynamic accessed member types.
	/// </summary>
	public const DynamicallyAccessedMemberTypes DynamicallyAccessedMembers =
		DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicFields |
		DynamicallyAccessedMemberTypes.PublicFields;
	/// <summary>
	/// Type of <see cref="Composite{TBufferA,TBufferB,T}"/>.
	/// </summary>
	public static readonly Type TypeofComposite = typeof(Composite<,,>);

#if !NET7_0_OR_GREATER
	/// <summary>
	/// Metadata cache.
	/// </summary>
	private static readonly ConcurrentDictionary<Type, BufferTypeMetadata> metadataCache = new();
#endif
	/// <summary>
	/// Indicates whether metadata for any required buffer is auto-composed.
	/// </summary>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static Boolean BufferAutoCompositionEnabled
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
#if NETSTANDARD1_3_OR_GREATER || NETCOREAPP || NET46_OR_GREATER || UAP10_0
		get
			=> !AotInfo.IsReflectionDisabled &&
				(!AppContext.TryGetSwitch("PInvoke.DisableBufferAutoComposition", out Boolean disable) || !disable);
#else
		get => true;
#endif
	}

	/// <summary>
	/// Retrieves the binary space for <paramref name="count"/>.
	/// </summary>
	/// <param name="count">Number of items in the binary space.</param>
	/// <returns>The amount of the binary space.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static UInt16 GetSpaceFor(UInt16 count)
	{
		Debug.Assert(count > 0);
#if NETCOREAPP3_0_OR_GREATER
		return (UInt16)(1u << BitOperations.Log2(count));
#else
		UInt32 value = count;
		value |= value >> 1;
		value |= value >> 2;
		value |= value >> 4;
		value |= value >> 8;
		return (UInt16)((value + 1) >> 1);
#endif
	}
	/// <summary>
	/// Calculates the number of leading zeros (unused bits) for a UInt16.
	/// </summary>
	/// <param name="value">The value to evaluate.</param>
	/// <returns>The number of unused bits out of 16.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static Int32 GetLeadingZeros(UInt16 value)
	{
#if NETCOREAPP3_0_OR_GREATER
		return BitOperations.LeadingZeroCount(value) - 16;
#else
		if (value == 0)
			return 16;

		Int32 zeros = 16;

		if (value >= 0x0100)
		{
			zeros -= 8;
			value >>= 8;
		}

		if (value >= 0x0010)
		{
			zeros -= 4;
			value >>= 4;
		}

		if (value >= 0x0004)
		{
			zeros -= 2;
			value >>= 2;
		}

		if (value >= 0x0002)
			zeros--;

		return zeros - 1;
#endif
	}
	/// <summary>
	/// Retrieves the binary capacity for <paramref name="count"/>.
	/// </summary>
	/// <param name="count">Number of items in the binary space.</param>
	/// <returns>The amount of the binary capacity.</returns>
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	public static UInt16 GetCapacityFor(UInt16 count)
	{
		Debug.Assert(count > 0);
		UInt32 value = count;
		value |= value >> 1;
		value |= value >> 2;
		value |= value >> 4;
		value |= value >> 8;
		return (UInt16)value;
	}
	/// <summary>
	/// Retrieves the component sizes for given <paramref name="count"/>.
	/// </summary>
	/// <param name="components">Components buffer.</param>
	/// <param name="count">Number of items in the required buffer.</param>
	/// <returns>Enumeration of components' sizes.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	public static Span<UInt16> GetBinaryComponents(Span<UInt16> components, UInt16 count)
	{
		Int32 found = 0;
		for (Int32 i = 0; i < 16; i++)
		{
			UInt16 mask = (UInt16)(1 << i);
			if ((count & mask) != 0) components[found++] = mask;
		}
		return components[..found];
	}
	/// <summary>
	/// Retrieves the static metadata required for a buffer of <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE && NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static BufferTypeMetadata<T> GetStaticMetadata<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T>
	{
#if !NET7_0_OR_GREATER
		IntPtr stackAllocation = IntPtr.Zero;
		ref TBuffer dummyBufferRef = ref Unsafe.As<IntPtr, TBuffer>(ref stackAllocation);
		BufferTypeMetadata<T> staticMetadata = dummyBufferRef.GetStaticTypeMetadata();
		return BuffersHelper.Cache(staticMetadata.BufferType, staticMetadata);
#else
		return IManagedBuffer<T>.GetMetadata<TBuffer>();
#endif
	}
	/// <summary>
	/// Element count of <see cref="Composite{TBufferA,TBufferB,T}"/>.
	/// </summary>
	/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
	/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <returns>Element count of the composition.</returns>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	public static unsafe UInt16 CompositeSize<TBufferA, TBufferB, T>() where TBufferA : struct where TBufferB : struct
	{
#pragma warning disable CS8500
		return (UInt16)(sizeof(TBufferA) / sizeof(T) + sizeof(TBufferB) / sizeof(T));
#pragma warning restore CS8500
	}
	/// <summary>
	/// Indicates whether <typeparamref name="TBufferA"/> and <typeparamref name="TBufferB"/> satisfy the size rule of a
	/// binary composition.
	/// </summary>
	/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
	/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <returns>
	/// <see langword="true"/> when the low buffer is not larger than the high buffer and the high buffer size is a
	/// power of two; otherwise, <see langword="false"/>.
	/// </returns>
#if !PACKAGE
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	public static unsafe Boolean CanComposeBinary<TBufferA, TBufferB, T>()
		where TBufferA : struct where TBufferB : struct
	{
#pragma warning disable CS8500
		Int32 sizeA = sizeof(TBufferA) / sizeof(T);
		Int32 sizeB = sizeof(TBufferB) / sizeof(T);
#pragma warning restore CS8500
		return sizeA <= sizeB && (sizeB & (sizeB - 1)) == 0;
	}
	/// <summary>
	/// Retrieves metadata required for a buffer of <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if !PACKAGE && NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static BufferTypeMetadata<T> GetMetadata<T, TBuffer>() where TBuffer : struct, IManagedBuffer<T>
	{
#if !NET7_0_OR_GREATER
		if (BuffersHelper.metadataCache.TryGetValue(typeof(TBuffer), out BufferTypeMetadata? result))
			return (BufferTypeMetadata<T>)result;
#endif
		return BuffersHelper.GetStaticMetadata<T, TBuffer>();
	}
#if !PACKAGE
	/// <summary>
	/// Retrieves metadata required for a buffer of <paramref name="bufferType"/> type.
	/// </summary>
	/// <param name="bufferType">Type of buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3218)]
	public static BufferTypeMetadata<T> GetMetadata<T>(
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] Type bufferType)
		=> bufferType == typeof(Atomic<T>) ? Atomic<T>.TypeMetadata : BuffersHelper.GetMetadataFromType<T>(bufferType);
#endif
	/// <summary>
	/// Creates <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="storage">An <see cref="IMetadataStorage"/> instance.</param>
	/// <param name="typeofA">The type of low buffer.</param>
	/// <param name="typeofB">The type of high buffer.</param>
	/// <returns>
	/// The <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/> buffer.
	/// </returns>
	[UnconditionalSuppressMessage("AOT", "IL2055")]
	[UnconditionalSuppressMessage("AOT", "IL2060")]
	[UnconditionalSuppressMessage("AOT", "IL2077")]
	[UnconditionalSuppressMessage("AOT", "IL3050")]
	[UnconditionalSuppressMessage("Trimming", "IL2055")]
	[UnconditionalSuppressMessage("Trimming", "IL2055")]
	public static BufferTypeMetadata<T>? ComposeWithReflection<T>(IMetadataStorage storage,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] Type typeofA,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] Type typeofB)
	{
#if NET7_0_OR_GREATER
		if (ManagedBinaryBuffer<T>.GetMetadataInfo is null) return default;
		Type? genericType = default;
		BufferTypeMetadata<T>? result;
		try
		{
#pragma warning disable IL3050
			genericType = BuffersHelper.TypeofComposite.MakeGenericType(typeofA, typeofB, typeof(T));
			MethodInfo getGenericMetadataInfo = ManagedBinaryBuffer<T>.GetMetadataInfo.MakeGenericMethod(genericType);
#pragma warning restore IL3050
			Func<BufferTypeMetadata<T>> getGenericMetadata =
				getGenericMetadataInfo.CreateDelegate<Func<BufferTypeMetadata<T>>>();
			result = getGenericMetadata();
		}
		catch (Exception)
		{
			// This may never be called.
			result = ManagedBinaryBuffer<T>.GetMetadata(genericType);
		}
#else
		if ((typeofB != typeof(Atomic<T>) && !BuffersHelper.GetMetadataFromType<T>(typeofB).IsBinary) ||
		    !BuffersHelper.BufferAutoCompositionEnabled)
			// Avoid using reflection to retrieve Atomic<T> metadata.
			return default;
		BufferTypeMetadata<T>? result = default;
		try
		{
			Type genericType = BuffersHelper.TypeofComposite.MakeGenericType(typeofA, typeofB, typeof(T));
			result = BuffersHelper.GetMetadataFromType<T>(genericType);
		}
		catch (Exception)
		{
			// Ignore
		}
#endif
		return storage.AddBinaryMetadata(result);
	}
	/// <summary>
	/// Searches for the first available metadata entry in a page segment.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="r0">Managed reference to the metadata page.</param>
	/// <param name="start">Zero-based index of the first entry to inspect.</param>
	/// <param name="count">Number of entries to inspect.</param>
	/// <returns>
	/// The first available metadata entry within the specified range; otherwise, <see langword="null"/>.
	/// </returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
	[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static unsafe BufferTypeMetadata<T>? Search<T>(ref BufferTypeMetadata<T>? r0, Int32 start, Int32 count)
	{
		Debug.Assert(start >= 0);
		Debug.Assert(count > 0);
		ref BufferTypeMetadata<T>? rS = ref Unsafe.Add(ref r0, start);
#pragma warning disable CS8500
		fixed (void* ptr = &rS)
#pragma warning restore CS8500
		{
			ReadOnlySpan<IntPtr> unsafeSpan = new(ptr, count);
#if NET7_0_OR_GREATER
			Int32 index = unsafeSpan.IndexOfAnyExcept(IntPtr.Zero);
#else
			Int32 index = -1;
			for (Int32 i = 0; i < unsafeSpan.Length; i++)
			{
				IntPtr val = unsafeSpan[i];
				if (val == IntPtr.Zero) continue;
				index = i;
				break;
			}
#endif
			return index < 0 ? default : Unsafe.Add(ref rS, index);
		}
	}

#if !NET7_0_OR_GREATER
#nullable disable
	/// <summary>
	/// Caches <paramref name="typeMetadata"/> for <paramref name="bufferType"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="bufferType">Type of buffer.</param>
	/// <param name="typeMetadata">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
	private static BufferTypeMetadata<T> Cache<T>(Type bufferType, BufferTypeMetadata<T> typeMetadata)
	{
		ValidationUtilities.ThrowIfNullMetadata(bufferType, typeMetadata is null);
		BuffersHelper.metadataCache.TryAdd(bufferType, typeMetadata);
		return typeMetadata;
	}
#nullable restore
#endif
	/// <summary>
	/// Retrieves metadata required for a buffer of <paramref name="bufferType"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="bufferType">Type of buffer.</param>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecuritySafeCritical]
#endif
	[UnconditionalSuppressMessage("Trimming", "IL2070")]
	private static BufferTypeMetadata<T> GetMetadataFromType<T>(
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] Type bufferType)
	{
#if !NET7_0_OR_GREATER
		if (BuffersHelper.metadataCache.TryGetValue(bufferType, out BufferTypeMetadata? result))
			return (BufferTypeMetadata<T>)result;
#else
		BufferTypeMetadata? result = default;
#endif
		try
		{
			if (!AotInfo.IsNativeAot || !AotInfo.IsReflectionDisabled)
			{
				FieldInfo? typeMetadataInfo =
					bufferType.GetField(BuffersHelper.TypeMetadataName, BuffersHelper.GetMetadataFlags);
				result = (BufferTypeMetadata?)typeMetadataInfo?.GetValue(null);
			}
		}
#if NETFRAMEWORK || NETSTANDARD2_0
		catch (SecurityException)
		{
			if (SystemInfo.UsesNativeSpan || SystemInfo.IsMonoRuntime || AotInfo.IsNativeAot)
				throw;
			// Allow .NET Framework, .NET Core 2.0 (Not CoreRT)
		}
#endif
		catch (TargetInvocationException tie)
		{
			if (tie.InnerException is not null)
				throw tie.InnerException;
		}
		result ??= ManagedBinaryBuffer<T>.GetMetadata(bufferType);
#if !NET7_0_OR_GREATER
		return BuffersHelper.Cache(bufferType, result as BufferTypeMetadata<T>);
#else
		ValidationUtilities.ThrowIfNullMetadata(bufferType, result is not BufferTypeMetadata<T>);
		return (result as BufferTypeMetadata<T>)!;
#endif
	}
}