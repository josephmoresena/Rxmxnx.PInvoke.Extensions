namespace Rxmxnx.PInvoke.Tests.DelegateExtensionsTests;

public abstract class DelegatesTests
{
	protected static readonly IFixture Fixture = new Fixture();

	protected static Byte GetByte(Byte value) => value;
	protected static Int32 GetThreadId() => Environment.CurrentManagedThreadId;

	protected delegate T GetValue<T>(T value);
	protected delegate Byte GetByteValue(Byte value);
	protected delegate Int32 GetInt32();
}