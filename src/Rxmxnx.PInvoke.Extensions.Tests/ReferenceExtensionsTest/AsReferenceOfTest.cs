using System;
using System.Diagnostics.CodeAnalysis;

using AutoFixture;

using Xunit;

namespace Rxmxnx.PInvoke.Extensions.Tests.ReferenceExtensionsTest
{
    [ExcludeFromCodeCoverage]
    public sealed class AsReferenceOfTest
    {
        [Fact]
        internal void ExceptionTest()
        {
            Assert.Throws<InvalidOperationException>(SimpleTest<Int32, Int64>);
            Assert.Throws<InvalidOperationException>(SimpleTest<Int64, Int32>);
            Assert.Throws<InvalidOperationException>(SimpleTest<Int16, Byte>);
            Assert.Throws<InvalidOperationException>(SimpleTest<Int64, Int32>);
            Assert.Throws<InvalidOperationException>(SimpleTest<Byte, Int16>);
        }

        [Fact]
        internal void NormalTest()
        {
            SimpleTest<Boolean, Boolean>();
            SimpleTest<Byte, SByte>();
            SimpleTest<Int16, Char>();
            SimpleTest<Char, Half>();
            SimpleTest<Int32, UInt32>();
            SimpleTest<UInt32, Single>();
            SimpleTest<Double, Int64>();
            SimpleTest<Double, DateTime>();
            SimpleTest<Decimal, Guid>();
        }

        private static void SimpleTest<T1, T2>()
            where T1 : unmanaged
            where T2 : unmanaged
        {
            T1 value = TestUtilities.SharedFixture.Create<T1>();
            ref T1 valueRef = ref value;
            ref T2 valueRef2 = ref valueRef.AsReferenceOf<T1, T2>();
            Assert.Equal(valueRef.AsIntPtr(), valueRef2.AsIntPtr());
            if (CastObject(valueRef) is Object obj && CastObject(valueRef2) is Object obj2)
                Assert.Equal(obj, obj2);
        }

        private static Object CastObject<T>(in T value)
            where T : unmanaged
        {
            Type typeofT = typeof(T);
            return Type.GetTypeCode(typeofT) switch
            {
                TypeCode.Boolean => Convert.ToByte(value),
                TypeCode.Char => Convert.ToInt16(value),
                TypeCode.SByte => ((SByte)(Object)value).AsBytes().AsValue<Byte>(),
                TypeCode.Byte => value,
                TypeCode.Int16 => value,
                TypeCode.UInt16 => ((UInt16)(Object)value).AsBytes().AsValue<Int16>(),
                TypeCode.Int32 => value,
                TypeCode.UInt32 => ((UInt32)(Object)value).AsBytes().AsValue<Int32>(),
                TypeCode.Int64 => value,
                TypeCode.UInt64 => ((UInt64)(Object)value).AsBytes().AsValue<Int64>(),
                TypeCode.Single => ((Single)(Object)value).AsBytes().AsValue<Int32>(),
                TypeCode.Double => ((Double)(Object)value).AsBytes().AsValue<Int64>(),
                TypeCode.DateTime => ((DateTime)(Object)value).AsBytes().AsValue<Int64>(),
                _ =>
                    typeofT.Equals(typeof(Decimal)) ? ((Decimal)(Object)value).AsBytes().AsValue<Guid>() :
                    typeofT.Equals(typeof(Guid)) ? value :
                    typeofT.Equals(typeof(Half)) ? ((Half)(Object)value).AsBytes().AsValue<Int16>() :
                    default
            };
        }
    }
}
