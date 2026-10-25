using System;
using System.Collections.ObjectModel;

namespace SharedBaseLib.Core
{
	public class RingBuffer<T>
	{
		private T[] m_data;

		private ReadOnlyCollection<T> m_dataReadOnly;

		private int m_size;

		private int m_start;

		private int m_end;

		private int m_valueCount;

		public T this[int i]
		{
			get
			{
				return GetValue(i);
			}
		}

		public int Count
		{
			get
			{
				return m_valueCount;
			}
		}

		public int Capacity
		{
			get
			{
				return m_size;
			}
		}

		public RingBuffer(int _size)
		{
			m_size = _size;
			m_data = new T[m_size];
			m_dataReadOnly = Array.AsReadOnly(m_data);
		}

		public void Add(T _value)
		{
			m_data[m_end] = _value;
			m_end = (m_end + 1) % m_size;
			if (m_start == m_end)
			{
				m_start = (m_start + 1) % m_size;
			}
			else
			{
				m_valueCount++;
			}
		}

		public T GetValue(int _index)
		{
			return m_data[(m_start + _index) % m_size];
		}

		public void Clear()
		{
			for (int i = 0; i < m_data.Length; i++)
			{
				m_data[i] = default(T);
			}
			m_start = 0;
			m_end = 0;
			m_valueCount = 0;
		}

		public bool IsEmpty()
		{
			return m_start == m_end;
		}

		public ReadOnlyCollection<T> GetData()
		{
			return m_dataReadOnly;
		}
	}
}
