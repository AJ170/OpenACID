using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.Popup;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialUIPopupBase : TutorialButtonClickBase
	{
		[SerializeField]
		protected UIPopup m_popup;

		protected override void OnSetupInternal()
		{
			if (m_popup == null)
			{
				m_popup = Singleton<UIScreenManager>.Instance.GetScreen(EScreenID.UIPopUp) as UIPopup;
			}
			m_button = m_popup.GetButton(GetButtonIndexToHighlight());
			if (m_popup.Blocker != null)
			{
				FullscreenBlocker blocker = m_popup.Blocker;
				m_blockers.Add(blocker);
			}
			base.OnSetupInternal();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_button = null;
		}

		protected abstract int GetButtonIndexToHighlight();
	}
}
