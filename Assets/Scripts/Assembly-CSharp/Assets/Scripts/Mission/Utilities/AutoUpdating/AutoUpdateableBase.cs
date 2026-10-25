using Assets.Scripts.Engine;

namespace Assets.Scripts.Mission.Utilities.AutoUpdating
{
	public abstract class AutoUpdateableBase : IAutoUpdateable
	{
		public delegate void FinishedHandler(AutoUpdateableBase _autoUpdateable);

		private bool m_IsRunning;

		private bool m_FireAndForget;

		public bool FireAndForget
		{
			get
			{
				return m_FireAndForget;
			}
			set
			{
				m_FireAndForget = value;
				if (m_FireAndForget && !IsRunning)
				{
					Start();
				}
			}
		}

		public FinishedHandler CurrentFinishedHandler { get; set; }

		public bool IsRunning
		{
			get
			{
				return m_IsRunning;
			}
		}

		public virtual void Start()
		{
			if (Assets.Scripts.Engine.Helpers.Verify(!IsRunning))
			{
				m_IsRunning = true;
				AutoUpdateManager.RegisterUpdateable(this);
			}
		}

		public virtual void Stop()
		{
			if (Assets.Scripts.Engine.Helpers.Verify(IsRunning))
			{
				AutoUpdateManager.UnregisterUpdateable(this);
				m_IsRunning = false;
			}
		}

		public bool UpdateFrame(float _gameTimeDelta)
		{
			bool flag = UpdateFrameInternal(_gameTimeDelta);
			if (flag)
			{
				m_IsRunning = false;
				if (CurrentFinishedHandler != null)
				{
					CurrentFinishedHandler(this);
				}
			}
			return flag;
		}

		protected abstract bool UpdateFrameInternal(float _gameTimeDelta);
	}
}
