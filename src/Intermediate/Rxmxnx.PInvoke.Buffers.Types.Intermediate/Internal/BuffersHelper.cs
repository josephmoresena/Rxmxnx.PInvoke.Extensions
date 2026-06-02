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
		get
			=> !AotInfo.IsReflectionDisabled &&
				(!AppContext.TryGetSwitch("PInvoke.DisableBufferAutoComposition", out Boolean disable) || !disable);
	}

	/// <summary>
	/// Retrieves the maximum value in the given binary space.
	/// </summary>
	/// <param name="space">Maximum binary power in the binary space.</param>
	/// <returns>The maximum value in the given binary space.</returns>
	public static UInt16 GetMaxValue(UInt16 space) => (UInt16)(space * 2 - 1);

	/// <summary>
	/// Retrieves the static metadata required for a buffer of <typeparamref name="TBuffer"/> type.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <typeparam name="TBuffer">Type of the buffer.</typeparam>
	/// <returns>A <see cref="BufferTypeMetadata{T}"/> instance.</returns>
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
	/// Retrieves the components array for the composition type of <typeparamref name="TBufferA"/> and
	/// <typeparamref name="TBufferB"/> .
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer.</typeparam>
	/// <typeparam name="TBufferA">The type of low buffer.</typeparam>
	/// <typeparam name="TBufferB">The type of high buffer.</typeparam>
	/// <returns>The components array for the composition type.</returns>
#if !PACKAGE && NET7_0_OR_GREATER
	[ExcludeFromCodeCoverage]
#endif
	public static BufferTypeMetadata<T>[] GetComponents<T,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBufferA,
		[DynamicallyAccessedMembers(BuffersHelper.DynamicallyAccessedMembers)] TBufferB>()
		where TBufferA : struct, IManagedBinaryBuffer<T> where TBufferB : struct, IManagedBinaryBuffer<T>

	{
		BufferTypeMetadata<T>[] components = new BufferTypeMetadata<T>[2];
		components[0] = BuffersHelper.GetMetadata<T, TBufferA>();
		components[1] = BuffersHelper.GetMetadata<T, TBufferB>();
		return components;
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
	/// Retrieves the capacity of a composite buffer of <paramref name="componentA"/> and <paramref name="componentB"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="componentA">A <see cref="BufferTypeMetadata{T}"/> instance.</param>
	/// <param name="componentB">A <see cref="BufferTypeMetadata"/> instance.</param>
	/// <param name="isBinary">Output. Indicates whether resulting composition type is binary.</param>
	/// <returns>Resulting composition type capacity.</returns>
	public static Int32 GetCapacity<T>(BufferTypeMetadata<T> componentA, BufferTypeMetadata<T> componentB,
		out Boolean isBinary)
	{
		UInt16 sizeA = componentA.Size;
		UInt16 sizeB = componentB.Size;
		isBinary = false;

		if (!componentA.IsBinary || !componentB.IsBinary ||
		    (componentB.Components is { Length: 2, Span: var bComponents, } && bComponents[0] != bComponents[^1]))
		{
			isBinary = false;
		}
		else
		{
			Int32 diff = sizeB - sizeA;
			isBinary = diff >= 0 && diff <= sizeB;
		}
		return componentA.Size + componentB.Size;
	}
	/// <summary>
	/// Creates <see cref="BufferTypeMetadata{T}"/> for <see cref="Composite{TBufferA,TBufferB,T}"/>.
	/// </summary>
	/// <typeparam name="T">The type of items in the buffer</typeparam>
	/// <param name="manager">A <see cref="IMetadataStore"/> instance.</param>
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
	public static BufferTypeMetadata<T>? ComposeWithReflection<T>(IMetadataStore manager,
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
		if (!BuffersHelper.GetMetadataFromType<T>(typeofB).IsBinary || !BuffersHelper.BufferAutoCompositionEnabled)
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
		return manager.AddBinaryMetadata(result);
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