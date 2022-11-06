using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.MemoryBlockExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class WithSafeFixedTest
    {
        [Fact]
        internal async Task NormalTest() => await Task.WhenAll(
            PinnTest<Boolean>(),
            PinnTest<Byte>(), PinnTest<SByte>(),
            PinnTest<Int16>(), PinnTest<UInt16>(),
            PinnTest<Int32>(), PinnTest<UInt32>(),
            PinnTest<Int64>(), PinnTest<UInt64>(),
            PinnTest<Guid>());

        private static async Task PinnTest<T>() where T : unmanaged
        {
            T[] arr = new T[10];
            Task[] setTasks = new Task[4];
            Task[] getTasks = new Task[setTasks.Length];

            for (Int32 i = 0; i < setTasks.Length; i++)
                setTasks[i] = Task.Run(() => SpanTest(arr));

            await Task.WhenAll(setTasks);

            for (Int32 i = 0; i < getTasks.Length; i++)
                getTasks[i] = Task.Run(() => ReadOnlySpanTest(arr));

            await Task.WhenAll(getTasks);
        }

        private static void SpanTest<T>(T[] arr) where T : unmanaged
        {
            Span<T> span = arr;
            span.WithSafeFixed(TestUtilities.SharedFixture, (s, f) =>
            {
                foreach (ref T value in s)
                    value = f.Create<T>();

                Assert.Equal(s.AsIntPtr(), s.WithSafeFixed(f, (s2, f2) =>
                {
                    return s2.AsIntPtr();
                }));
            });
        }

        private static void ReadOnlySpanTest<T>(T[] arr) where T : unmanaged
        {
            ReadOnlySpan<T> span = arr;
            span.WithSafeFixed(arr, (s, a) =>
            {
                for (Int32 i = 0; i < a.Length; i++)
                    Assert.Equal(s[i], a[i]);

                Assert.Equal(s.AsIntPtr(), s.WithSafeFixed(a, (s2, a2) =>
                {
                    return s2.AsIntPtr();
                }));
            });
        }
    }
}
