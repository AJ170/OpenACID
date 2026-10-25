using System.Collections.Generic;

namespace SharedBaseLib.Core
{
	public class BBList<T> : List<T>
	{
		public BBList()
		{
		}

		public BBList(int capacity)
			: base(capacity)
		{
		}

		public BBList(IEnumerable<T> collection)
			: base(collection)
		{
		}
	}
}
