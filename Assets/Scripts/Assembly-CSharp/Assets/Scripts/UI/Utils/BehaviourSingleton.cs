using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public abstract class BehaviourSingleton<T> : MonoBehaviour where T : BehaviourSingleton<T>
	{
		private static T m_instance = (T)null;

		private static bool m_quitting = false;

		private static object m_lock = new object();

		private bool m_inited;

		public static bool IsInitialized
		{
			get
			{
				return !m_quitting && (Object)m_instance != (Object)null;
			}
		}

		public static T Instance
		{
			get
			{
				if (m_quitting)
				{
					Debug.LogWarning(string.Concat("Singleton Instance", typeof(T), " destroyed. No instance returned"));
					return (T)null;
				}
				lock (m_lock)
				{
					if ((Object)m_instance == (Object)null)
					{
						m_instance = Object.FindObjectOfType<T>();
						if ((Object)m_instance == (Object)null)
						{
							GameObject gameObject = new GameObject(string.Concat("__singleton(", typeof(T), ")"));
							m_instance = gameObject.AddComponent<T>();
						}
					}
					return m_instance;
				}
			}
		}

		public static void Reset()
		{
			m_quitting = false;
		}

		public void Init()
		{
			if (!m_inited)
			{
				m_inited = true;
				OnInit();
			}
		}

		public void Purge()
		{
			OnPurge();
			DestroyInstance();
		}

		private void DestroyInstance()
		{
			Object.DestroyImmediate(base.gameObject);
		}

		private void OnDestroy()
		{
			OnPurge();
			m_instance = (T)null;
			if (!Application.isEditor)
			{
				m_quitting = true;
			}
		}

		protected virtual void OnInit()
		{
		}

		protected virtual void OnPurge()
		{
		}
	}
}
