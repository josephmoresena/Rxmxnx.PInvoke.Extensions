using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Rxmxnx.PInvoke.Extensions.Internal
{
    internal abstract class ValueRegion<T>
        where T : unmanaged
    {
        [IndexerName("Position")]
        public virtual T this[Int32 index] => this.AsSpan()[index];
        public virtual T[] ToArray() => this.AsSpan().ToArray();

        protected virtual T[]? AsArray() => default;
        protected abstract ReadOnlySpan<T> AsSpan();

        public ReadOnlySpan<T>.Enumerator GetEnumerator() => this.AsSpan().GetEnumerator();

        public static implicit operator ReadOnlySpan<T>(ValueRegion<T> arrayWrapper) => arrayWrapper.AsSpan();
        public static implicit operator T[]?(ValueRegion<T> arrayWrapper) => arrayWrapper.AsArray();

        public static ValueRegion<T> Create([DisallowNull] T[] array) => new ManagedWrapper(array);
        public static ValueRegion<T> Create(IntPtr ptr, Int32 length)
            => !ptr.IsZero() && length != default ? new NativeWrapper(ptr, length) : NativeWrapper.Empty;

        private sealed class ManagedWrapper : ValueRegion<T>
        {
            private readonly T[] _array;

            public override T this[Int32 index] => this._array[index];

            public ManagedWrapper([DisallowNull] T[] array) => this._array = array;

            protected override T[] AsArray() => this._array;
            protected override ReadOnlySpan<T> AsSpan() => this._array;
        }

        private sealed class NativeWrapper : ValueRegion<T>
        {
            public static readonly NativeWrapper Empty = new(IntPtr.Zero, default);

            private readonly IntPtr _ptr;
            private readonly Int32 _length;

            public NativeWrapper(IntPtr ptr, Int32 length)
            {
                this._ptr = ptr;
                this._length = !this._ptr.Equals(IntPtr.Zero) ? length : default;
            }

            protected override ReadOnlySpan<T> AsSpan() => this._ptr.AsReadOnlySpan<T>(this._length);
        }
    }
}
