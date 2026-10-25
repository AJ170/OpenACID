using System.Collections.Generic;

namespace SharedBaseLib.Core
{
	public class BBSortedList<TKey, TValue> : SortedList<TKey, TValue>
	{
		public BBSortedList()
		{
		}

		public BBSortedList(int capacity)
			: base(capacity)
		{
		}

		public BBSortedList(int capacity, IComparer<TKey> comparer)
			: base(capacity, comparer)
		{
		}

		public BBSortedList(IComparer<TKey> comparer)
			: base(comparer)
		{
		}

		public BBSortedList(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{
		}

		public BBSortedList(IDictionary<TKey, TValue> dictionary, IComparer<TKey> comparer)
			: base(dictionary, comparer)
		{
		}
	}
}
