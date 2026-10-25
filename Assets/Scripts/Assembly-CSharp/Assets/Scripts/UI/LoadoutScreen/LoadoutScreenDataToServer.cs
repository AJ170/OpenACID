using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using UnityEngine;

namespace Assets.Scripts.UI.LoadoutScreen
{
	public class LoadoutScreenDataToServer : UIScreenChild
	{
		[SerializeField]
		private UIButton m_button;

		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivated);
		}

		protected override void OnScreenDestroy()
		{
			if (m_screen != null)
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivated);
			}
		}

		private void OnEnable()
		{
			EventDelegate.Add(m_button.onClick, OnButtonClick);
		}

		private void OnDisable()
		{
			EventDelegate.Remove(m_button.onClick, OnButtonClick);
		}

		private void OnScreenActivated()
		{
			m_button.isEnabled = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassinId != null;
		}

		private void OnButtonClick()
		{
			UIMissionHelper.StartCurrentSelectedMissionWithTutorialPopup();
		}
	}
}
