using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.NativeUtilitiesTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CreateArrayTest
    {
        [Fact]
        internal void NormalTest()
        {
            CreateTest<Boolean>();
            CreateTest<Byte>();
            CreateTest<Int16>();
            CreateTest<Int32>();
            CreateTest<Int64>();
            CreateTest<Single>();
            CreateTest<Double>();
            CreateTest<Decimal>();
        }

        private static void CreateTest<T>() where T : unmanaged
        {
            Int32 length = Random.Shared.Next(0, 129);
            List<T> list = new List<T>();
            T[] arr = NativeUtilities.CreateArray<T, IList<T>>(length, list,
                (span, state) =>
                {
                    foreach (ref T value in span)
                    {
                        value = TestUtilities.SharedFixture.Create<T>();
                        list.Add(value);
                    }
                });

            Assert.Equal(list, arr);
        }
    }
}
