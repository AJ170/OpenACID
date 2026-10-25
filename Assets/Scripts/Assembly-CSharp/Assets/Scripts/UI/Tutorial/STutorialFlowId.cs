using System;

namespace Assets.Scripts.UI.Tutorial
{
	[Serializable]
	public struct STutorialFlowId
	{
		public static readonly STutorialFlowId Invalid = new STutorialFlowId(-1);

		private readonly int m_id;

		public STutorialFlowId(int _id)
		{
			m_id = _id;
		}

		public bool Equals(STutorialFlowId _other)
		{
			return m_id == _other.m_id;
		}

		public override bool Equals(object _obj)
		{
			if (object.ReferenceEquals(null, _obj))
			{
				return false;
			}
			return _obj is STutorialFlowId && Equals((STutorialFlowId)_obj);
		}

		public override int GetHashCode()
		{
			return m_id.GetHashCode();
		}

		public override string ToString()
		{
			return m_id.ToString();
		}

		public static implicit operator int(STutorialFlowId _other)
		{
			return _other.m_id;
		}

		public static bool operator <(STutorialFlowId _lhs, STutorialFlowId _rhs)
		{
			return _lhs < _rhs.m_id;
		}

		public static bool operator >(STutorialFlowId _lhs, STutorialFlowId _rhs)
		{
			return _lhs > _rhs.m_id;
		}

		public static bool operator ==(STutorialFlowId _lhs, STutorialFlowId _rhs)
		{
			return _lhs == _rhs.m_id;
		}

		public static bool operator !=(STutorialFlowId _lhs, STutorialFlowId _rhs)
		{
			return _lhs != _rhs.m_id;
		}

		public static bool operator <(STutorialFlowId _lhs, long _rhs)
		{
			return _lhs.m_id < _rhs;
		}

		public static bool operator >(STutorialFlowId _lhs, long _rhs)
		{
			return _lhs.m_id > _rhs;
		}

		public static bool operator ==(STutorialFlowId _lhs, long _rhs)
		{
			return _lhs.m_id == _rhs;
		}

		public static bool operator !=(STutorialFlowId _lhs, long _rhs)
		{
			return _lhs.m_id != _rhs;
		}
	}
}
