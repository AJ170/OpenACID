using UnityEngine;

namespace Assets.Scripts.UI.ScreenManager
{
	public abstract class UIScreenChild : MonoBehaviour
	{
		protected UIScreen m_screen;

		public bool IsDestroyingScreen { get; private set; }

		protected abstract void OnScreenInit();

		protected abstract void OnScreenDestroy();

		public virtual void OnBackKey()
		{
		}

		public void ScreenInit()
		{
			m_screen = UIScreen.CurrentUIScreen;
			IsDestroyingScreen = false;
			OnScreenInit();
		}

		public void ScreenDestroy()
		{
			IsDestroyingScreen = true;
			if (m_screen != null && m_screen.IsActivated)
			{
				m_screen.DeActivate();
			}
			OnScreenDestroy();
			m_screen = null;
		}
	}
}
