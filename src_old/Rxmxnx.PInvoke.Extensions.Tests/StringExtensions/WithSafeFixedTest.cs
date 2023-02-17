using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.StringExtensions
{
    [ExcludeFromCodeCoverage]
    public sealed class WithSafeFixedTest
    {
        [Fact]
        internal void NullTest()
        {
            String.Empty.WithSafeFixed(String.Empty, FullTest);
            String nullStr = default;

            IMutableWrapper<Boolean> called = InputValue.CreateReference(false);
            nullStr.WithSafeFixed(nullStr, (in IReadOnlyFixedContext<Char> ctx, String str) =>
            {
                called.SetInstance(true);
            });
            Assert.False(called.Value);
            nullStr.WithSafeFixed((in IReadOnlyFixedContext<Char> ctx) =>
            {
                called.SetInstance(true);
            });
            Assert.False(called.Value);
            Assert.Null(nullStr.WithSafeFixed(String.Empty, (in IReadOnlyFixedContext<Char> ctx, String str) =>
            {
                called.SetInstance(true);
                return str;
            }));
            Assert.False(called.Value);
            Assert.Null(nullStr.WithSafeFixed((in IReadOnlyFixedContext<Char> ctx) =>
            {
                called.SetInstance(true);
                return String.Empty;
            }));
            Assert.False(called.Value);
        }

        [Fact]
        internal async Task NormalTestAsync()
        {
            String[] strings = TestUtilities.SharedFixture.CreateMany<String>(50).ToArray();
            Task[] tasks = new Task[strings.Length];
            for (Int32 i = 0; i < tasks.Length; i++)
                tasks[i] = Task.Factory.StartNew((o) =>
                {
                    String str = o as String;
                    str.WithSafeFixed(str, FullTest);
                }, strings[i]);

            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        private static void FullTest(in IReadOnlyFixedContext<Char> ctx, String str)
        {
            Assert.Equal(ctx.Values.Length, str.Length);
            if (ctx.Values.Length > 0)
                Assert.True(Unsafe.AreSame(ref Unsafe.AsRef(ctx.Values[0]), ref Unsafe.AsRef(str.AsSpan()[0])));
            str.WithSafeFixed((in IReadOnlyFixedContext<Char> ctx2) =>
            {
                Assert.Equal(ctx2.Values.AsIntPtr(), ctx2.BinaryValues.AsIntPtr());
                Assert.Equal(ctx2.Values.Length * sizeof(Char), ctx2.BinaryValues.Length);
            });
            Assert.Equal(ctx.Values.AsIntPtr(), str.WithSafeFixed((in IReadOnlyFixedContext<Char> ctx2) =>
            {
                return ctx2.Values.AsIntPtr();
            }));
            Assert.Contains(str, str.WithSafeFixed(TestUtilities.SharedFixture, (in IReadOnlyFixedContext<Char> ctx2, IFixture fix) =>
            {
                String str2 = fix.Create<String>();
                return String.Create(ctx2.Values.Length + str2.Length, ctx2, (s, ctx2) =>
                {
                    ctx2.Values.CopyTo(s);
                    str2.CopyTo(s[ctx2.Values.Length..]);
                });
            }));
        }
    }
}