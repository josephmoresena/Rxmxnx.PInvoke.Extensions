using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
            Task[] contextActionTasks = new Task[setTasks.Length];
            Task[] contextFunctionTasks = new Task[setTasks.Length];

            for (Int32 i = 0; i < setTasks.Length; i++)
                setTasks[i] = Task.Run(() => SpanTest(arr));

            await Task.WhenAll(setTasks);

            for (Int32 i = 0; i < getTasks.Length; i++)
                getTasks[i] = Task.Run(() => ReadOnlySpanTest(arr));

            await Task.WhenAll(getTasks);

            for (Int32 i = 0; i < contextActionTasks.Length; i++)
                contextActionTasks[i] = Task.Run(() => ContextActionTest(arr));

            await Task.WhenAll(contextActionTasks);

            for (Int32 i = 0; i < contextFunctionTasks.Length; i++)
                contextFunctionTasks[i] = Task.Run(() => ContextFunctionTest(arr));

            await Task.WhenAll(contextFunctionTasks);
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

                Assert.Equal(arr.Length, s.Length);
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

                Assert.Equal(arr.Length, s.Length);
            });
        }
        private static void ContextActionTest<T>(T[] arr) where T : unmanaged
        {
            Span<T> span = arr;
            unsafe
            {
                span.WithSafeFixed((in IReadOnlyFixedContext<T> rctx) =>
                {
                    fixed (void* ptr = arr)
                    {
                        Assert.Equal(new IntPtr(ptr), rctx.Values.AsIntPtr());
                        Assert.Equal(new IntPtr(ptr), rctx.BinaryValues.AsIntPtr());
                    }
                });
                span.WithSafeFixed(arr, (in IReadOnlyFixedContext<T> rctx, T[] arr) =>
                {
                    fixed (void* ptr = arr)
                    {
                        Assert.Equal(new IntPtr(ptr), rctx.Values.AsIntPtr());
                        Assert.Equal(new IntPtr(ptr), rctx.BinaryValues.AsIntPtr());
                    }
                });
                span.WithSafeFixed((in IFixedContext<T> ctx) =>
                {
                    Assert.Equal(arr.Length, ctx.Values.Length);
                    Assert.Equal(arr.Length * sizeof(T), ctx.BinaryValues.Length);
                    ctx.Values.WithSafeFixed(ctx, (in IReadOnlyFixedContext<T> rctx, IFixedContext<T> ctx) =>
                    {
                        Assert.Equal(arr.Length, rctx.Values.Length);
                        Assert.Equal(arr.Length * sizeof(T), rctx.BinaryValues.Length);

                        Assert.True(Unsafe.AreSame(
                            ref MemoryMarshal.GetReference(ctx.Values),
                            ref MemoryMarshal.GetReference(rctx.Values)));
                    });
                });
            }
        }
        private static void ContextFunctionTest<T>(T[] arr) where T : unmanaged
        {
            Span<T> span = arr;
            unsafe
            {
                Assert.True(span.WithSafeFixed((in IReadOnlyFixedContext<T> rctx) =>
                 {
                     fixed (void* ptr = arr)
                     {
                         IntPtr arrPtr = new(ptr);
                         return arrPtr == rctx.Values.AsIntPtr() && arrPtr == rctx.BinaryValues.AsIntPtr();
                     }
                 }));
                Assert.True(span.WithSafeFixed(arr, (in IReadOnlyFixedContext<T> rctx, T[] arr) =>
                {
                    fixed (void* ptr = arr)
                    {
                        IntPtr arrPtr = new(ptr);
                        return arrPtr == rctx.Values.AsIntPtr() && arrPtr == rctx.BinaryValues.AsIntPtr();
                    }
                }));
                Assert.True(span.WithSafeFixed((in IFixedContext<T> ctx) =>
                {
                    return arr.Length == ctx.Values.Length &&
                    arr.Length * sizeof(T) == ctx.BinaryValues.Length &&
                    ctx.Values.WithSafeFixed(ctx, (in IReadOnlyFixedContext<T> rctx, IFixedContext<T> ctx) =>
                    {
                        Assert.Equal(arr.Length, rctx.Values.Length);
                        Assert.Equal(arr.Length * sizeof(T), rctx.BinaryValues.Length);

                        return Unsafe.AreSame(
                            ref MemoryMarshal.GetReference(ctx.Values),
                            ref MemoryMarshal.GetReference(rctx.Values));
                    });
                }));
            }
        }
    }
}
