using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialClickBase : TutorialPopupBase
	{
		[SerializeField]
		protected UIWidget m_buttonWidget;

		[SerializeField]
		protected UIPanel m_buttonPanel;

		protected virtual bool OptionsButtonClickable
		{
			get
			{
				return true;
			}
		}

		protected override void OnScreenInit()
		{
			base.OnScreenInit();
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				TidyUpClickWidget();
				ActivateMenuBarBlockers(false);
				base.Deactivate();
			}
		}

		protected abstract void AddButtonToEventDelegate(UIWidget[] _widgets);

		protected abstract void RemoveButtonFromEventDelegate();

		protected void ActivateBlocker(FullscreenBlocker _blocker)
		{
			ActivateMenuBarBlockers(true);
			if (_blocker != null)
			{
				_blocker.gameObject.SetActive(true);
			}
		}

		protected void BlockerUnblockAndIncrement(FullscreenBlocker _blocker, UIWidget _clickWidget, UIWidget[] _excludedWidgets)
		{
			if (_blocker != null)
			{
				UnblockWidget(_clickWidget, _blocker, _excludedWidgets);
				if (OptionsButtonClickable)
				{
					UnblockOptionsButton(_blocker);
				}
				_blocker.IncrementDepth();
			}
		}

		protected void OnClick()
		{
			OnClickInternal();
		}

		protected virtual void OnClickInternal()
		{
			bool flag = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		protected override void OnSetupInternal()
		{
			base.OnSetupInternal();
			PrepareClickWidget();
		}

		protected void PrepareClickWidget()
		{
			GameObject gameObject = ((!(m_buttonPanel != null)) ? m_buttonWidget.gameObject : m_buttonPanel.gameObject);
			UIWidget[] componentsInChildren = gameObject.GetComponentsInChildren<UIWidget>(true);
			AddButtonToEventDelegate(componentsInChildren);
			for (int i = 0; i < m_blockers.Count; i++)
			{
				ActivateBlocker(m_blockers[i]);
			}
			if (m_highestBlocker != null)
			{
				BlockerUnblockAndIncrement(m_highestBlocker, null, componentsInChildren);
			}
			CreateButtonHighlight(gameObject, Vector3.zero);
		}

		protected void TidyUpClickWidget()
		{
			RemoveButtonFromEventDelegate();
			if (m_highestBlocker != null)
			{
				m_highestBlocker.DecrementDepth();
			}
			for (int i = 0; i < m_blockers.Count; i++)
			{
				FullscreenBlocker fullscreenBlocker = m_blockers[i];
				if ((bool)fullscreenBlocker.gameObject)
				{
					fullscreenBlocker.gameObject.SetActive(false);
					fullscreenBlocker.DecrementDepth();
					fullscreenBlocker.Clear();
				}
			}
		}
	}
}
