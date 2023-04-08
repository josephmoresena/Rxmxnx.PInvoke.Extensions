namespace Rxmxnx.PInvoke.Tests.CStringTests.JoinTests;

public abstract class JoinTestBase
{
    protected static IFixture fixture = new Fixture();

    protected static Byte GetByteSeparator()
    {
        Int32 result = Random.Shared.Next(33, 127);
        return (Byte)result;
    }
}

