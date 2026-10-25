using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public class AutoUpdateablesCarrier
	{
		private BBList<IAutoUpdateable> m_AutoUpdateables;

		public T AddAutoUpdateable<T>(T _autoUpdateable) where T : IAutoUpdateable
		{
			if (m_AutoUpdateables == null)
			{
				m_AutoUpdateables = new BBList<IAutoUpdateable>();
			}
			m_AutoUpdateables.Add(_autoUpdateable);
			return _autoUpdateable;
		}

		public void StartAutoUpdateables()
		{
			if (m_AutoUpdateables != null)
			{
				for (int i = 0; i < m_AutoUpdateables.Count; i++)
				{
					m_AutoUpdateables[i].Start();
				}
			}
		}

		public void StopAutoUpdateables()
		{
			if (m_AutoUpdateables == null)
			{
				return;
			}
			for (int num = m_AutoUpdateables.Count - 1; num >= 0; num--)
			{
				if (m_AutoUpdateables[num].IsRunning)
				{
					m_AutoUpdateables[num].Stop();
				}
			}
		}
	}
}
