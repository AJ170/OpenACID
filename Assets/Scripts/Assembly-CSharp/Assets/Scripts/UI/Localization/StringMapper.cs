using System.Collections;
using System.Collections.Generic;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.Localization
{
	public class StringMapper : IEnumerable
	{
		private readonly BBDictionary<uint, object> m_dict = new BBDictionary<uint, object>();

		public int Count
		{
			get
			{
				return m_dict.Count;
			}
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public object this[string key]
		{
			get
			{
				return m_dict[StringCRC32.ComputeLowerCase(key)];
			}
			set
			{
				m_dict[StringCRC32.ComputeLowerCase(key)] = value;
			}
		}

		public IEnumerator GetEnumerator()
		{
			return m_dict.GetEnumerator();
		}

		public void Append(StringMapper _other)
		{
			foreach (KeyValuePair<uint, object> item in _other.m_dict)
			{
				m_dict.Add(item.Key, item.Value);
			}
		}

		public void Add(KeyValuePair<string, object> item)
		{
			uint key = StringCRC32.ComputeLowerCase(item.Key);
			if (!m_dict.ContainsKey(key))
			{
				m_dict.Add(key, item.Value);
			}
			else
			{
				m_dict[key] = item.Value;
			}
		}

		public void Clear()
		{
			m_dict.Clear();
		}

		public bool Remove(KeyValuePair<string, object> item)
		{
			return m_dict.Remove(StringCRC32.ComputeLowerCase(item.Key));
		}

		public void Add(string key, object value)
		{
			uint key2 = StringCRC32.ComputeLowerCase(key);
			if (m_dict.ContainsKey(key2))
			{
				m_dict[key2] = value;
			}
			else
			{
				m_dict.Add(key2, value);
			}
		}

		public bool ContainsKey(string key)
		{
			return m_dict.ContainsKey(StringCRC32.ComputeLowerCase(key));
		}

		public bool Remove(string key)
		{
			return m_dict.Remove(StringCRC32.ComputeLowerCase(key));
		}

		public bool TryGetValue(string key, out object value)
		{
			return m_dict.TryGetValue(StringCRC32.ComputeLowerCase(key), out value);
		}
	}
}
