using AcierProtocol;

namespace Assets.Scripts.Network.Missions.Mission
{
	public static class ECompareUtils
	{
		public static bool HasFlag(this ECompare _mask, ECompare _flag)
		{
			return (_mask & _flag) == _flag;
		}

		public static bool IsTrue(this ECompare _comparer, EAlertLevel _currentAlertLevel, EAlertLevel _bound)
		{
			bool flag = false;
			if (_comparer.HasFlag(ECompare.Less))
			{
				flag = flag || _currentAlertLevel < _bound;
			}
			if (_comparer.HasFlag(ECompare.Equal))
			{
				flag = flag || _currentAlertLevel == _bound;
			}
			if (_comparer.HasFlag(ECompare.Greater))
			{
				flag = flag || _currentAlertLevel > _bound;
			}
			return flag;
		}
	}
}
