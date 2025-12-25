namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Dynamic <see cref="CString"/> cache.
	/// </summary>
	private abstract class FixedCache : CStringCacheBase
	{
		/// <summary>
		/// Gaps.
		/// </summary>
		private readonly IImmutableSet<Int32> _emptyIndices;

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="emptyIndices">Set indices with empty elements.</param>
		private FixedCache(IImmutableSet<Int32>? emptyIndices)
			=> this._emptyIndices = emptyIndices ?? ImmutableHashSet<Int32>.Empty;

		/// <summary>
		/// Calculates real index.
		/// </summary>
		/// <param name="index">Required index.</param>
		/// <returns>The real index at cache.</returns>
#if !PACKAGE
		[SuppressMessage(SuppressMessageConstants.CSharpSquid, SuppressMessageConstants.CheckIdS3267)]
#endif
		private Int32 GetRealIndex(Int32 index)
		{
			Int32 count = 0;
			// ReSharper disable once LoopCanBeConvertedToQuery
			foreach (Int32 i in this._emptyIndices)
			{
				if (i < index)
					count++;
			}
			return index - count;
		}

		/// <summary>
		/// Creates a <see cref="FixedCache"/> instance.
		/// </summary>
		/// <param name="count">Non-empty entries count.</param>
		/// <param name="emptyIndices">Empty caps.</param>
		/// <returns>A <see cref="FixedCache"/> instance.</returns>
		public static FixedCache CreateFixedCache(Int32 count, IImmutableSet<Int32>? emptyIndices = default)
			=> count <= 32 ? new CStringCache(count, emptyIndices) : new WeakCache(count, emptyIndices);

		/// <summary>
		/// A <see cref="CString"/> cache.
		/// </summary>
		private sealed class CStringCache : FixedCache
		{
			/// <summary>
			/// Internal cache.
			/// </summary>
			private readonly CString?[] _cache;

			/// <inheritdoc/>
			public override CString? this[Int32 index]
			{
				get => this._cache[this.GetRealIndex(index)];
				set => this._cache[this.GetRealIndex(index)] = value;
			}
			/// <inheritdoc/>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public override Int32 Count => this._cache.Length;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="count">Total elements in cache.</param>
			/// <param name="emptyIndices">Set indices with empty elements.</param>
			public CStringCache(Int32 count, IImmutableSet<Int32>? emptyIndices) : base(emptyIndices)
				=> this._cache = new CString?[count];

			/// <inheritdoc/>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public override void Insert(Int32 index, CString? item) => this._cache[this.GetRealIndex(index)] = item;
			/// <inheritdoc/>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public override void RemoveAt(Int32 index) => this._cache[this.GetRealIndex(index)] = default;
		}

		/// <summary>
		/// A <see cref="WeakReference{CString}"/> cache.
		/// </summary>
		private sealed class WeakCache : FixedCache
		{
			/// <summary>
			/// Internal cache.
			/// </summary>
			private readonly WeakReference<CString>?[] _cache;

			/// <inheritdoc/>
			public override CString? this[Int32 index]
			{
				get
					=> this._cache[this.GetRealIndex(index)] is { } weak && weak.TryGetTarget(out CString? result) ?
						result :
						default;
				set
				{
					if (this._cache[this.GetRealIndex(index)] is { } weak)
						weak.SetTarget(value!);
					else
						this._cache[this.GetRealIndex(index)] = new(value!);
				}
			}
			/// <inheritdoc/>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public override Int32 Count => this._cache.Length;

			/// <summary>
			/// Constructor.
			/// </summary>
			/// <param name="count">Total elements in cache.</param>
			/// <param name="emptyIndices">Set indices with empty elements.</param>
			public WeakCache(Int32 count, IImmutableSet<Int32>? emptyIndices) : base(emptyIndices)
				=> this._cache = new WeakReference<CString>[count];

			/// <inheritdoc/>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public override void RemoveAt(Int32 index) => this._cache[this.GetRealIndex(index)] = default;
			/// <inheritdoc/>
#if !PACKAGE
			[ExcludeFromCodeCoverage]
#endif
			public override void Insert(Int32 index, CString? item)
			{
				if (this._cache[this.GetRealIndex(index)] is { } weak)
					weak.SetTarget(item!);
				else
					this._cache[this.GetRealIndex(index)] = new(item!);
			}
		}
	}
}