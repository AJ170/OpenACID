namespace Assets.Scripts.Mission.Utilities
{
	public class SimpleGameTimer
	{
		private float m_EndTime = float.MinValue;

		private bool m_IsStarted;

		public bool IsFinished
		{
			get
			{
				return IsStarted && LogicMain.CurrentTime >= m_EndTime;
			}
		}

		public bool IsStarted
		{
			get
			{
				return m_IsStarted;
			}
		}

		public bool IsRunning
		{
			get
			{
				return IsStarted && LogicMain.CurrentTime < m_EndTime;
			}
		}

		public float TimeToFinish
		{
			get
			{
				return m_EndTime - LogicMain.CurrentTime;
			}
		}

		public void Start(float _duration)
		{
			m_EndTime = LogicMain.CurrentTime + _duration;
			m_IsStarted = true;
		}

		public void Reset()
		{
			m_EndTime = float.MinValue;
			m_IsStarted = false;
		}
	}
}
