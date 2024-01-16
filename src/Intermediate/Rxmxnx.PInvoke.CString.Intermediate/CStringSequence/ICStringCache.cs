namespace Rxmxnx.PInvoke;

public partial class CStringSequence
{
	/// <summary>
	/// Internal <see cref="CString"/> list.
	/// </summary>
	private interface ICStringCache : IList<CString?>
	{
		Boolean ICollection<CString?>.IsReadOnly => true;

		[ExcludeFromCodeCoverage]
		void ICollection<CString?>.Clear() { }

		[ExcludeFromCodeCoverage]
		void ICollection<CString?>.Add(CString? item) => throw new NotImplementedException();
		[ExcludeFromCodeCoverage]
		Boolean ICollection<CString?>.Contains(CString? item) => throw new NotImplementedException();
		[ExcludeFromCodeCoverage]
		void ICollection<CString?>.CopyTo(CString?[] array, Int32 arrayIndex) => throw new NotImplementedException();
		[ExcludeFromCodeCoverage]
		Boolean ICollection<CString?>.Remove(CString? item) => throw new NotImplementedException();
		[ExcludeFromCodeCoverage]
		Int32 IList<CString?>.IndexOf(CString? item) => throw new NotImplementedException();
		[ExcludeFromCodeCoverage]
		IEnumerator<CString?> IEnumerable<CString?>.GetEnumerator() => throw new NotImplementedException();
		[ExcludeFromCodeCoverage]
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => throw new NotImplementedException();
	}
}