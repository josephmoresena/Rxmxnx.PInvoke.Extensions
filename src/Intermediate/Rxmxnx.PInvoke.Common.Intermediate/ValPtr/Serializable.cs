#if NETSTANDARD2_0_OR_GREATER || NETCOREAPP || NETFRAMEWORK || UAP10_0_16299
namespace Rxmxnx.PInvoke;

[Serializable]
#if !PACKAGE
[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS6640)]
#endif
public readonly unsafe partial struct ValPtr<T> : ISerializable
{
	/// <summary>
	/// Serialization constructor.
	/// </summary>
	/// <param name="info">A <see cref="SerializationInfo"/> instance.</param>
	/// <param name="context">A <see cref="StreamingContext"/> instance.</param>
	/// <exception cref="ArgumentException">If invalid pointer value.</exception>
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
	private ValPtr(SerializationInfo info, StreamingContext context)
		=> this._value = ValidationUtilities.ThrowIfInvalidPointer(info);

#if !PACKAGE
	[ExcludeFromCodeCoverage]
#endif
#if NETFRAMEWORK || NETSTANDARD2_0
	[SecurityCritical]
#endif
	void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
		=> ValidationUtilities.ThrowIfInvalidSerialization(info, this._value);
}
#endif