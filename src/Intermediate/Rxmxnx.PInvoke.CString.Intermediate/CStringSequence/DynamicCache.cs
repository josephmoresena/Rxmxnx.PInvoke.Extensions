namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Dynamic <see cref="CString"/> cache.
	/// </summary>
	private sealed class DynamicCache : ICStringCache
	{
		/// <summary>
		/// Internal cache.
		/// </summary>
		private readonly ConcurrentDictionary<Int32, WeakReference<CString>> _cache = new();

		CString? IList<CString?>.this[Int32 index]
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
		Boolean ICollection<CString?>.IsReadOnly => false;
		[ExcludeFromCodeCoverage]
		Int32 ICollection<CString?>.Count => this._cache.Count;

		void ICollection<CString?>.Clear()
		{
			Int32[] keys = this._cache.Keys.ToArray();
			foreach (Int32 key in keys.AsSpan())
			{
				if (!this._cache[key].TryGetTarget(out _))
					this._cache.TryRemove(key, out _);
			}
		}

		[ExcludeFromCodeCoverage]
		void IList<CString?>.Insert(Int32 index, CString? item) => this._cache[index] = new(item!);
		[ExcludeFromCodeCoverage]
		void IList<CString?>.RemoveAt(Int32 index) => this._cache.TryRemove(index, out _);
	}
}