namespace UbiCore.Utils
{
	public abstract class Singleton<T> where T : Singleton<T>, new()
	{
		private static T m_Instance;

		public static T Instance
		{
			get
			{
				return m_Instance;
			}
		}

		public static bool IsAvailable
		{
			get
			{
				return m_Instance != null;
			}
		}

		public static void Initialize()
		{
			if (m_Instance == null)
			{
				m_Instance = new T();
			}
		}

		public static void Shutdown()
		{
			if (m_Instance != null)
			{
				m_Instance.OnDestroy();
			}
			m_Instance = null;
		}

		protected virtual void OnDestroy()
		{
		}
	}
}
