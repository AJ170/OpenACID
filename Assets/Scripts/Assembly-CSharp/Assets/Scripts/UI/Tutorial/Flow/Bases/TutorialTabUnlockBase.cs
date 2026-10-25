using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.ScreenManager;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialTabUnlockBase : TutorialFlowBase
	{
		[SerializeField]
		private UIButtonHeader.EHeaderType m_buttoinHeaderType;

		private UIButtonHeader m_buttonHeader;

		public override void Activate()
		{
			if (m_buttonHeader == null)
			{
				m_buttonHeader = Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.ButtonHeaderManager.GetButtonByHeaderType(m_buttoinHeaderType);
			}
			base.Activate();
			Singleton<UIScreenManager>.Instance.SceneData.FullScreenBlocker.SetActive(true);
			EventDelegate.Add(m_buttonHeader.OnUnlockChangeCompleted, OnButtonUnlockComplete, true);
		}

		public override void Deactivate()
		{
			base.Deactivate();
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.FullScreenBlocker != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.FullScreenBlocker.SetActive(false);
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			if (m_buttonHeader != null)
			{
				EventDelegate.Remove(m_buttonHeader.OnUnlockChangeCompleted, OnButtonUnlockComplete);
			}
			m_buttonHeader = null;
		}

		private void OnButtonUnlockComplete()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}
	}
}
