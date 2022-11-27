using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.UnmanagedValueExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class WithSafeFixedTest
    {
        [Fact]
        internal void NullTest()
        {
            GenericNullTest<Byte>();
            GenericNullTest<Int16>();
            GenericNullTest<Int32>();
            GenericNullTest<Int64>();
            GenericNullTest<Guid>();
        }

        [Fact]
        internal async Task NormalTest()
        {
            await Task.WhenAll(
                GenericNormalTestAsync<Byte>(),
                GenericNormalTestAsync<Int16>(),
                GenericNormalTestAsync<Int32>(),
                GenericNormalTestAsync<Int64>(),
                GenericNormalTestAsync<Int64>()
                ).ConfigureAwait(false);
        }

        private static void GenericNullTest<T>() where T : unmanaged
        {
            Array.Empty<T>().WithSafeFixed(Array.Empty<T>(), FullReadOnlyTest);
            T[] nullArr = default;

            IMutableWrapper<Boolean> called = InputValue.CreateReference(false);

            nullArr.WithSafeFixed(nullArr, (in IReadOnlyFixedContext<T> ctx, T[] arr) =>
            {
                called.SetInstance(true);
            });
            Assert.False(called.Value);
            nullArr.WithSafeFixed((in IReadOnlyFixedContext<T> ctx) =>
            {
                called.SetInstance(true);
            });
            Assert.False(called.Value);
            Assert.Null(nullArr.WithSafeFixed(Array.Empty<T>(), (in IReadOnlyFixedContext<T> ctx, T[] arr) =>
            {
                called.SetInstance(true);
                return arr;
            }));
            Assert.False(called.Value);
            Assert.Null(nullArr.WithSafeFixed((in IReadOnlyFixedContext<T> ctx) =>
            {
                called.SetInstance(true);
                return String.Empty;
            }));
            Assert.False(called.Value);


            nullArr.WithSafeFixed(nullArr, (in IFixedContext<T> ctx, T[] arr) =>
            {
                called.SetInstance(true);
            });
            Assert.False(called.Value);
            nullArr.WithSafeFixed((in IFixedContext<T> ctx) =>
            {
                called.SetInstance(true);
            });
            Assert.False(called.Value);
            Assert.Null(nullArr.WithSafeFixed(Array.Empty<T>(), (in IFixedContext<T> ctx, T[] arr) =>
            {
                called.SetInstance(true);
                return arr;
            }));
            Assert.False(called.Value);
            Assert.Null(nullArr.WithSafeFixed((in IFixedContext<T> ctx) =>
            {
                called.SetInstance(true);
                return String.Empty;
            }));
            Assert.False(called.Value);
        }

        private static async Task GenericNormalTestAsync<T>() where T : unmanaged
        {
            T[][] strings = Enumerable.Repeat(10, 50).Select(i => new T[Random.Shared.Next(0, i)]).ToArray();
            Task[] tasks = new Task[strings.Length];
            for (Int32 i = 0; i < tasks.Length; i++)
                tasks[i] = Task.Factory.StartNew((o) =>
                {
                    T[] arr = o as T[];

                    arr.WithSafeFixed(arr, FullTest);
                    arr.WithSafeFixed(arr, FullReadOnlyTest);
                }, strings[i]);

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private static void FullTest<T>(in IFixedContext<T> ctx, T[] arr) where T : unmanaged
        {
            Assert.Equal(ctx.Values.Length, arr.Length);
            for (Int32 i = 0; i < arr.Length; i++)
            {
                ctx.Values[i] = TestUtilities.SharedFixture.Create<T>();
                Assert.Equal(ctx.Values[i], arr[i]);
                if (ctx.Values.Length > 0)
                    Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(ctx.Values[0]), ref Unsafe.AsRef(arr.AsSpan()[0])));
                arr.WithSafeFixed((in IFixedContext<T> ctx2) =>
                {
                    Assert.Equal(ctx2.Values.AsIntPtr(), ctx2.BinaryValues.AsIntPtr());
                    unsafe
                    {
                        Assert.Equal(ctx2.Values.Length * sizeof(T), ctx2.BinaryValues.Length);
                    }
                });
                Assert.Equal(ctx.Values.AsIntPtr(), arr.WithSafeFixed((in IFixedContext<T> ctx2) =>
                {
                    return ctx2.Values.AsIntPtr();
                }));
                T[] arr2 = arr.WithSafeFixed(TestUtilities.SharedFixture, (in IFixedContext<T> ctx2, IFixture fix) =>
                {
                    T[] arr2 = fix.CreateMany<T>(ctx2.Values.Length).ToArray();
                    for (Int32 j = 0; j < arr2.Length; j++)
                        ctx2.Values[j] = arr2[j];
                    return arr2;
                });
                Assert.Equal(arr, arr2);
            }
        }
        private static void FullReadOnlyTest<T>(in IReadOnlyFixedContext<T> ctx, T[] arr) where T : unmanaged
        {
            Assert.Equal(ctx.Values.Length, arr.Length);
            if (ctx.Values.Length > 0)
                Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(ctx.Values[0]), ref Unsafe.AsRef(arr.AsSpan()[0])));
            arr.WithSafeFixed((in IReadOnlyFixedContext<T> ctx2) =>
            {
                Assert.Equal(ctx2.Values.AsIntPtr(), ctx2.BinaryValues.AsIntPtr());
                unsafe
                {
                    Assert.Equal(ctx2.Values.Length * sizeof(T), ctx2.BinaryValues.Length);
                }
            });
            Assert.Equal(ctx.Values.AsIntPtr(), arr.WithSafeFixed((in IReadOnlyFixedContext<T> ctx2) =>
            {
                return ctx2.Values.AsIntPtr();
            }));
            T[] arr2 = arr.WithSafeFixed(TestUtilities.SharedFixture, (in IReadOnlyFixedContext<T> ctx2, IFixture fix) =>
            {
                T[] arr2 = fix.Create<T[]>();
                return NativeUtilities.CreateArray(ctx2.Values.Length + arr2.Length, ctx2, (Span<T> s, IReadOnlyFixedContext<T> ctx2) =>
                {
                    ctx2.Values.CopyTo(s);
                    arr2.CopyTo(s[ctx2.Values.Length..]);
                });
            });
            Assert.Equal(arr, arr2[0..arr.Length]);
        }
    }
}
