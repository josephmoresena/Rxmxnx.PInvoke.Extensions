namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Dynamic <see cref="CString"/> cache.
	/// </summary>
	private sealed class DynamicCache : CStringCacheBase
	{
		/// <summary>
		/// Internal cache.
		/// </summary>
		private readonly ConcurrentDictionary<Int32, WeakReference<CString>> _cache;

		/// <inheritdoc/>
		public override CString? this[Int32 index]
		{
			get
			{
				if (this._cache.TryGetValue(index, out WeakReference<CString>? weak) &&
				    weak.TryGetTarget(out CString? result))
					return result;
				return default;
			}
			set
			{
				if (this._cache.TryGetValue(index, out WeakReference<CString>? weak))
					weak.SetTarget(value!);
				else
					this._cache[index] = new(value!);
			}
		}
		/// <inheritdoc/>
		public override Boolean IsReadOnly => false;
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public override Int32 Count => this._cache.Count;

		/// <summary>
		/// Private constructor.
		/// </summary>
		/// <param name="count">Non-empty entries count.</param>
		private DynamicCache(Int32 count) => this._cache = new(Environment.ProcessorCount, count);

		/// <inheritdoc/>
		public override void Clear()
		{
			Int32[] keys = this._cache.Keys.ToArray();
			foreach (Int32 key in keys.AsSpan())
			{
				if (!this._cache[key].TryGetTarget(out _))
					this._cache.TryRemove(key, out _);
			}
		}
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public override void Insert(Int32 index, CString? item) => this._cache[index] = new(item!);
		/// <inheritdoc/>
#if !PACKAGE
		[ExcludeFromCodeCoverage]
#endif
		public override void RemoveAt(Int32 index) => this._cache.TryRemove(index, out _);

		/// <summary>
		/// Creates a <see cref="FixedCache"/> instance.
		/// </summary>
		/// <param name="count">Non-empty entries count.</param>
		/// <returns>A <see cref="FixedCache"/> instance.</returns>
		public static DynamicCache CreateDynamicCache(Int32 count) => new(count);
	}
}