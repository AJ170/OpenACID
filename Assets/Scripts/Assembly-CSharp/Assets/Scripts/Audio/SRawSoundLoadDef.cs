using Database;

namespace Assets.Scripts.Audio
{
	public struct SRawSoundLoadDef
	{
		private readonly GameID m_id;

		public static readonly SRawSoundLoadDef Invalid = new SRawSoundLoadDef(GameID.Invalid);

		public GameID Id
		{
			get
			{
				return m_id;
			}
		}

		public bool IsInvalid
		{
			get
			{
				return m_id == Invalid.Id;
			}
		}

		public SRawSoundLoadDef(GameID _id)
		{
			m_id = _id;
		}

		public bool Equals(GameID _id)
		{
			return m_id == _id;
		}

		public bool Equals(SRawSoundLoadDef _other)
		{
			return m_id == _other.m_id;
		}

		public override bool Equals(object _obj)
		{
			if (object.ReferenceEquals(null, _obj))
			{
				return false;
			}
			return _obj is SRawSoundLoadDef && Equals((SRawSoundLoadDef)_obj);
		}

		public override int GetHashCode()
		{
			return m_id.GetHashCode();
		}

		public static bool operator ==(SRawSoundLoadDef _left, GameID _id)
		{
			return _left.m_id == _id;
		}

		public static bool operator !=(SRawSoundLoadDef _left, GameID _id)
		{
			return _left.m_id != _id;
		}

		public static bool operator ==(SRawSoundLoadDef _left, SRawSoundLoadDef _right)
		{
			return _left.m_id == _right.m_id;
		}

		public static bool operator !=(SRawSoundLoadDef _left, SRawSoundLoadDef _right)
		{
			return _left.m_id != _right.m_id;
		}
	}
}
