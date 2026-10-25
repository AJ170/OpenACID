using Assets.Scripts.Engine;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public static class AutoUpdateManager
	{
		private static readonly BBList<IAutoUpdateable> s_Updateables = new BBList<IAutoUpdateable>();

		private static bool m_IsUpdating;

		public static void Reset()
		{
			s_Updateables.Clear();
		}

		public static void RegisterUpdateable(IAutoUpdateable _updateable)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(!m_IsUpdating))
			{
				s_Updateables.Add(_updateable);
			}
		}

		public static void UnregisterUpdateable(IAutoUpdateable _updateable)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(!m_IsUpdating))
			{
				s_Updateables.Remove(_updateable);
			}
		}

		public static void UpdateFrame(float _gameTimeDelta)
		{
			m_IsUpdating = true;
			int count = s_Updateables.Count;
			for (int i = 0; i < count; i++)
			{
				if (s_Updateables[i].UpdateFrame(_gameTimeDelta))
				{
					s_Updateables.RemoveAt(i);
					count = s_Updateables.Count;
					i--;
				}
			}
			m_IsUpdating = false;
		}
	}
}
