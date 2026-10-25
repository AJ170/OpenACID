using AcierProtocol;
using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class XpPerLevelTableExtensions
	{
		public static XpPerLevelTable.Entry GetXpLevelEntryFromXp(this XpPerLevelTable _xpTable, long _xp)
		{
			XpPerLevelTable.Entry result = null;
			for (int i = 0; i < _xpTable.entries.Length; i++)
			{
				if (i == _xpTable.entries.Length - 1 || (_xp >= _xpTable.entries[i].xp && _xp < _xpTable.entries[i + 1].xp))
				{
					result = _xpTable.entries[i];
					break;
				}
			}
			return result;
		}

		public static int GetMaxLevel(this XpPerLevelTable _xpTable)
		{
			return _xpTable.entries.Length;
		}

		public static bool IsMaxLevel(this XpPerLevelTable _xpTable, int _level)
		{
			return _level == _xpTable.GetMaxLevel();
		}

		public static long GetMaxXpToNextLevel(this XpPerLevelTable _xpTable, int _currentLevel)
		{
			long result = 1L;
			if (_currentLevel > 0)
			{
				if (_currentLevel > _xpTable.entries.Length)
				{
				}
				int num = Mathf.Min(_currentLevel, _xpTable.entries.Length - 1);
				int num2 = Mathf.Min(_currentLevel - 1, _xpTable.entries.Length - 1);
				if (num == num2)
				{
					num2--;
				}
				result = _xpTable.entries[num].xp - _xpTable.entries[num2].xp;
			}
			return result;
		}

		public static int GetLevelFromXp(this XpPerLevelTable _xpTable, long _xp)
		{
			int result = 0;
			XpPerLevelTable.Entry xpLevelEntryFromXp = _xpTable.GetXpLevelEntryFromXp(_xp);
			if (xpLevelEntryFromXp != null)
			{
				result = xpLevelEntryFromXp.level;
			}
			return result;
		}
	}
}
