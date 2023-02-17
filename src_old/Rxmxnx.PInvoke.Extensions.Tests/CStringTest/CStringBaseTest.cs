using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AutoFixture;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringTest
{
    [ExcludeFromCodeCoverage]
    public abstract class CStringBaseTest
    {
        protected static Byte[] CreateUtf8String()
            => Encoding.UTF8.GetBytes(TestUtilities.SharedFixture.Create<String>());

        protected static Byte[] CreateUtf8StringNulTerminated()
            => CreateUtf8String().Concat(Enumerable.Repeat<Byte>(default, 1)).ToArray();
    }
}
