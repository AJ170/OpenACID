using System.Collections.Generic;

namespace Assets.Scripts.Audio
{
	public class PoolEnumeratorConstruct<T> where T : class, new()
	{
		private class ObjIsPooledPair
		{
			public bool IsPooled;

			public T Obj;
		}

		public class SwapCondition
		{
			public delegate bool SwapConditionDelegate(List<T> _list, out T _tSwap);

			public SwapConditionDelegate Condition;

			public object CompareObj;
		}

		public delegate IEnumerator<T> Construct();

		private readonly List<ObjIsPooledPair> m_objects = new List<ObjIsPooledPair>();

		private int m_capacity;

		private IEnumerator<T> m_constructEnumerator;

		public int Capacity
		{
			get
			{
				return m_capacity;
			}
		}

		public PoolEnumeratorConstruct(int _capacity, Construct _construct = null)
		{
			if (_capacity < 0)
			{
				m_capacity = -1;
			}
			else
			{
				m_objects.Capacity = (m_capacity = _capacity);
			}
			if (_construct != null)
			{
				m_constructEnumerator = _construct();
			}
		}

		public bool TryGet(out T _tOut, SwapCondition _swapCondition = null)
		{
			foreach (ObjIsPooledPair @object in m_objects)
			{
				if (@object.IsPooled)
				{
					@object.IsPooled = false;
					_tOut = @object.Obj;
					return true;
				}
			}
			if (m_objects.Count < m_capacity || m_capacity == -1)
			{
				ObjIsPooledPair objIsPooledPair = new ObjIsPooledPair();
				m_objects.Add(objIsPooledPair);
				objIsPooledPair.IsPooled = false;
				_tOut = (objIsPooledPair.Obj = ((m_constructEnumerator == null || !m_constructEnumerator.MoveNext()) ? new T() : m_constructEnumerator.Current));
				return true;
			}
			if (_swapCondition != null && _swapCondition.Condition != null)
			{
				List<T> list = new List<T>();
				for (int i = 0; i < m_objects.Count; i++)
				{
					list.Add(m_objects[i].Obj);
				}
				T _tSwap;
				if (_swapCondition.Condition(list, out _tSwap))
				{
					foreach (ObjIsPooledPair object2 in m_objects)
					{
						if (object2.Obj == _tSwap)
						{
							object2.IsPooled = false;
							_tOut = object2.Obj;
							return true;
						}
					}
				}
			}
			_tOut = (T)null;
			return false;
		}

		public void Free(T _objToPool)
		{
			ObjIsPooledPair objIsPooledPair = m_objects.Find((ObjIsPooledPair _o) => _o.Obj == _objToPool);
			if (objIsPooledPair != null)
			{
				objIsPooledPair.IsPooled = true;
			}
		}
	}
}
