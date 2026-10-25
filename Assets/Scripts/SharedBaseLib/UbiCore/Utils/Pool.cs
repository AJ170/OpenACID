using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace UbiCore.Utils
{
	public class Pool<T> where T : class
	{
		private int m_MaxSize;

		private BBList<T> m_Slots;

		public int Capacity
		{
			get
			{
				return m_Slots.Capacity;
			}
			private set
			{
				m_Slots.Capacity = value;
			}
		}

		public int CurrentSize
		{
			get
			{
				return m_Slots.Count;
			}
		}

		public int MaxSize
		{
			get
			{
				return m_MaxSize;
			}
		}

		public Pool(int _capacity)
		{
			m_MaxSize = _capacity;
			m_Slots = new BBList<T>(_capacity);
		}

		public void Grow(int _amount)
		{
			m_MaxSize += _amount;
			m_Slots.Capacity += _amount;
		}

		public void Shrink(int _amount)
		{
			m_MaxSize -= _amount;
			m_Slots.Capacity -= _amount;
		}

		public void Push(T _item)
		{
			if (m_Slots.Count < m_MaxSize)
			{
				m_Slots.Add(_item);
			}
		}

		public bool Find(T _item)
		{
			return m_Slots.Contains(_item);
		}

		public T Pop()
		{
			T result = null;
			if (m_Slots.Count > 0)
			{
				result = m_Slots.Last();
				m_Slots.RemoveAt(m_Slots.Count - 1);
			}
			return result;
		}
	}
}
