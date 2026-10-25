using System.Collections.Generic;

namespace SharedBaseLib.Core
{
	public class BBQueue<T> : Queue<T>
	{
		public BBQueue()
		{
		}

		public BBQueue(IEnumerable<T> collection)
			: base(collection)
		{
		}

		public BBQueue(int capacity)
			: base(capacity)
		{
		}
	}
}
