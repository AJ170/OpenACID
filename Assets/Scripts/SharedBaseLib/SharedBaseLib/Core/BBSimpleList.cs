using System;

namespace SharedBaseLib.Core
{
	public class BBSimpleList<T>
	{
		private T[] m_data;

		private int m_size;

		public int Capacity
		{
			get
			{
				int result = 0;
				if (m_data != null)
				{
					result = m_data.Length;
				}
				return result;
			}
			set
			{
				if (value != m_data.Length)
				{
					T[] data = m_data;
					m_data = new T[value];
					int num = data.Length;
					if (value < num)
					{
						num = value;
					}
					Array.Copy(data, m_data, num);
				}
			}
		}

		public T[] Data
		{
			get
			{
				return m_data;
			}
		}

		public int Count
		{
			get
			{
				return m_size;
			}
		}

		public T this[int index]
		{
			get
			{
				return m_data[index];
			}
			set
			{
				if (index >= m_size)
				{
					throw new ArgumentOutOfRangeException();
				}
				m_data[index] = value;
			}
		}

		public BBSimpleList(T[] _data)
		{
			if (_data == null)
			{
				throw new ArgumentNullException();
			}
			m_data = _data;
			m_size = m_data.Length;
		}

		public BBSimpleList(int _capacity = 0)
		{
			if (_capacity <= 0)
			{
				_capacity = 4;
			}
			m_data = new T[_capacity];
			m_size = 0;
		}

		public void Add(T _value)
		{
			if (m_size == m_data.Length)
			{
				T[] data = m_data;
				m_data = new T[m_data.Length * 2];
				Array.Copy(data, m_data, data.Length);
			}
			m_data[m_size] = _value;
			m_size++;
		}

		public int IndexOf(T _value)
		{
			for (int i = 0; i < m_size; i++)
			{
				if (m_data[i].Equals(_value))
				{
					return i;
				}
			}
			return -1;
		}

		public bool Contains(T _value)
		{
			return IndexOf(_value) >= 0;
		}

		public bool Remove(T _value)
		{
			int num = IndexOf(_value);
			if (num < 0)
			{
				return false;
			}
			return RemoveAt(num);
		}

		public bool RemoveAt(int _idx)
		{
			if (_idx >= m_size)
			{
				return false;
			}
			for (int i = _idx; i < m_size - 1; i++)
			{
				m_data[i] = m_data[i + 1];
			}
			m_size--;
			m_data[m_size] = default(T);
			return true;
		}

		public void TrimExcess()
		{
			if (m_size > 0 && m_data.Length != m_size)
			{
				T[] data = m_data;
				m_data = new T[m_size];
				Array.Copy(data, m_data, m_size);
			}
		}

		public void Clear()
		{
			if (m_size > 0)
			{
				Array.Clear(m_data, 0, m_data.Length);
				m_size = 0;
			}
		}

		public void AddRange(BBSimpleList<T> _other)
		{
			if (_other != null && _other.Count != 0)
			{
				int num = m_size + _other.Count;
				if (Capacity < num)
				{
					Capacity = num;
				}
				int count = _other.Count;
				for (int i = 0; i < count; i++)
				{
					Add(_other[i]);
				}
			}
		}

		public void AddRange(T[] _other)
		{
			if (_other != null && _other.Length != 0)
			{
				int num = m_size + _other.Length;
				if (Capacity < num)
				{
					Capacity = num;
				}
				int num2 = _other.Length;
				for (int i = 0; i < num2; i++)
				{
					Add(_other[i]);
				}
			}
		}

		public void Insert(int _idx, T _value)
		{
			if (m_size == m_data.Length)
			{
				T[] datum = m_data;
				m_data = new T[m_data.Length * 2];
			}
			Array.Copy(m_data, _idx, m_data, _idx + 1, m_size - _idx);
			m_data[0] = _value;
			m_size++;
		}
	}
}
