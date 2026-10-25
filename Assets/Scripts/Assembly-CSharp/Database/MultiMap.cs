using System.Collections.Generic;

namespace Database
{
	public class MultiMap<TKey, TValue>
	{
		private Dictionary<TKey, HashSet<TValue>> m_dictionary = new Dictionary<TKey, HashSet<TValue>>();

		public IEnumerable<TKey> Keys
		{
			get
			{
				return m_dictionary.Keys;
			}
		}

		public HashSet<TValue> this[TKey _key]
		{
			get
			{
				HashSet<TValue> value;
				if (!m_dictionary.TryGetValue(_key, out value))
				{
					value = new HashSet<TValue>();
					m_dictionary[_key] = value;
				}
				return value;
			}
		}

		public bool ContainsKey(TKey _key)
		{
			return m_dictionary.ContainsKey(_key);
		}

		public void Add(TKey _key, TValue _value)
		{
			HashSet<TValue> value;
			if (m_dictionary.TryGetValue(_key, out value))
			{
				value.Add(_value);
				return;
			}
			value = new HashSet<TValue>();
			value.Add(_value);
			m_dictionary[_key] = value;
		}

		public int GetValueItemCount()
		{
			int num = 0;
			foreach (HashSet<TValue> value in m_dictionary.Values)
			{
				num += value.Count;
			}
			return num;
		}
	}
}
