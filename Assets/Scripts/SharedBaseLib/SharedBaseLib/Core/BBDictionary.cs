using System.Collections.Generic;

namespace SharedBaseLib.Core
{
	public class BBDictionary<TKey, TValue> : Dictionary<TKey, TValue>
	{
		public BBDictionary()
		{
		}

		public BBDictionary(int capacity)
			: base(capacity)
		{
		}

		public BBDictionary(IDictionary<TKey, TValue> dictionary)
			: base(dictionary)
		{
		}

		public BBDictionary(int capacity, IEqualityComparer<TKey> comparer)
			: base(capacity, comparer)
		{
		}

		public BBDictionary(IEqualityComparer<TKey> comparer)
			: base(comparer)
		{
		}

		public BBDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
			: base(dictionary, comparer)
		{
		}
	}
}
