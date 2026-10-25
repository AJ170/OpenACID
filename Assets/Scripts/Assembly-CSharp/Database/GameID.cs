using System;
using Assets.Scripts.Engine;

namespace Database
{
	public class GameID
	{
		private static readonly string s_emptyGuid = System.Guid.Empty.ToString();

		public static readonly GameID Invalid = new GameID(s_emptyGuid);

		public int Guid { get; private set; }

		public DBEntry ParentDBEntry { get; private set; }

		public string GuidString { get; private set; }

		public GameID(string _guid, DBEntry _parent)
		{
			Guid = ComputeGuidHash(ref _guid);
		}

		public GameID(GameID _default, DBEntry _parent)
		{
			Guid = _default.Guid;
		}

		private GameID(string _guid)
		{
			Guid = ComputeGuidHash(ref _guid);
		}

		private GameID(int _guidHash)
		{
			Guid = _guidHash;
		}

		public static bool IsValid(GameID _id)
		{
			if (object.ReferenceEquals(_id, null))
			{
				return false;
			}
			return !_id.Equals(Invalid);
		}

		internal static GameID CreateDummy(string _guid)
		{
			return new GameID(_guid);
		}

		internal static GameID CreateDummy(int _guidHash)
		{
			return new GameID(_guidHash);
		}

		public override string ToString()
		{
			return string.Format("GameID Hash: {0:X}", Guid);
		}

		public bool Equals(GameID _other)
		{
			return Guid == _other.Guid;
		}

		public override bool Equals(object _obj)
		{
			if (object.ReferenceEquals(null, _obj))
			{
				return false;
			}
			return _obj is GameID && Equals((GameID)_obj);
		}

		public override int GetHashCode()
		{
			return Guid;
		}

		public int CompareTo(GameID _gameID)
		{
			if (Guid < _gameID.Guid)
			{
				return -1;
			}
			return (Guid > _gameID.Guid) ? 1 : 0;
		}

		public static int ComputeGuidHash(ref string _guid)
		{
			if (string.IsNullOrEmpty(_guid) || _guid == s_emptyGuid)
			{
				return 0;
			}
			return _guid.GetBbHashCode();
		}

		public static bool operator ==(GameID _left, GameID _right)
		{
			if (object.ReferenceEquals(_left, _right))
			{
				return true;
			}
			if (object.ReferenceEquals(_left, null))
			{
				return false;
			}
			if (object.ReferenceEquals(_right, null))
			{
				return false;
			}
			return _left.Guid == _right.Guid;
		}

		public static bool operator !=(GameID _left, GameID _right)
		{
			return !(_left == _right);
		}
	}
}
