namespace Rxmxnx.PInvoke;

#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3011)]
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public static partial class BufferManager
{
	/// <summary>
	/// Name of <see cref="Atomic{T}.TypeMetadata"/>  static field.
	/// </summary>
	internal const String TypeMetadataName = nameof(Atomic<Object>.TypeMetadata);
	/// <summary>
	/// Flags of metadata static member.
	/// </summary>
	internal const BindingFlags GetMetadataFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
	/// <summary>
	/// Flags of dynamic accessed member types.
	/// </summary>
	internal const DynamicallyAccessedMemberTypes DynamicallyAccessedMembers =
		DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.NonPublicFields |
		DynamicallyAccessedMemberTypes.PublicFields;
	/// <summary>
	/// Type of <see cref="Composite{TBufferA,TBufferB,T}"/>.
	/// </summary>
	internal static readonly Type TypeofComposite = typeof(Composite<,,>);
}