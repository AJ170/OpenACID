using AcierProtocol;

namespace Assets.Scripts.Network.Missions.Mission.Enums
{
	public static class EnumUtil
	{
		public static bool HasFlag(this ECompare _mask, ECompare _flag)
		{
			return (_mask & _flag) == _flag;
		}

		public static bool HasFlag(this ESpawnPointKind _mask, ESpawnPointKind _flag)
		{
			return (_mask & _flag) == _flag;
		}

		public static bool HasFlag(this EObjectSubCategory _mask, EObjectSubCategory _flag)
		{
			return (_mask & _flag) == _flag;
		}
	}
}
