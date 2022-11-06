using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.CStringSequenceTest
{
    [ExcludeFromCodeCoverage]
    public sealed class CreateTest
    {
        [Fact]
        internal void NormalTest()
        {
            String[] strValue = TestUtilities.SharedFixture.CreateMany<String>(100).Select(x =>
            {
                Int32 start = Random.Shared.Next(0, x.Length);
                Int32 end = Random.Shared.Next(start, x.Length);
                return x[start..end];
            }).ToArray();

            CStringSequence sequence = CStringSequence.Create(
                strValue, CreateSequence, strValue.Select(x => x.Length).ToArray());

            sequence.Transform(strValue, (s, v) =>
            {
                for (Int32 i = 0; i < v.Length; i++)
                {
                    CString cstr = s[i];
                    Assert.True(cstr.IsNullTerminated);

                    if (cstr.Length > 0)
                    {
                        Assert.True(cstr.IsReference);
                        Assert.False(cstr.IsSegmented);
                        Assert.Equal(v[i], s[i].ToString());
                    }
                    else
                    {
                        Assert.False(cstr.IsReference);
                        Assert.Equal(CString.Empty, cstr);
                    }
                }

                Assert.Equal(sequence.ToString().AsSpan().AsIntPtr(), sequence.Transform(s.ToArray(), (s2, v2) =>
                {
                    IntPtr? result = default;
                    for (Int32 i = 0; i < s2.Length; i++)
                    {
                        Assert.Equal(s2[i], v2[i]);
                        Assert.Equal(s2[i].AsSpan().AsIntPtr(), v2[i].AsSpan().AsIntPtr());

                        if (result is null && s2[i].Length > 0)
                            result = s2[i].AsSpan().AsIntPtr();
                    }
                    return result ?? CString.Empty.AsSpan().AsIntPtr();
                }));
            });
        }

        private static void CreateSequence(Span<Byte> span, Int32 index, String[] strValue)
        {
            Encoding.UTF8.GetBytes(strValue[index]).CopyTo(span);
        }
    }
}
