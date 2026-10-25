using System.Collections.Generic;

namespace SharedBaseLib.Core
{
	public class BBHashSet<T> : HashSet<T>
	{
		public BBHashSet()
		{
		}

		public BBHashSet(IEnumerable<T> collection, IEqualityComparer<T> comparer)
			: base(collection, comparer)
		{
		}

		public BBHashSet(IEnumerable<T> collection)
			: base(collection)
		{
		}

		public BBHashSet(IEqualityComparer<T> comparer)
			: base(comparer)
		{
		}
	}
}
