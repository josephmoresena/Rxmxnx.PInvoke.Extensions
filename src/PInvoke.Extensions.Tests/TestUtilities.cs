using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

namespace PInvoke.Extensions.Tests
{
    [ExcludeFromCodeCoverage]
    internal static class TestUtilities
    {
        public static readonly Fixture SharedFixture = GetFixture();

        private static Fixture GetFixture()
        {
            Fixture result = new();
            result.Register<IntPtr>(() => new IntPtr(Environment.Is64BitProcess ? result.Create<Int64>() : result.Create<Int32>()));
            result.Register<UIntPtr>(() => new UIntPtr(Environment.Is64BitProcess ? result.Create<UInt64>() : result.Create<UInt32>()));
            return result;
        }
    }
}
