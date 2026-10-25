using Assets.Scripts.UI.InfoPopup;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialInfoPopUpBase : TutorialButtonProxyClickBase
	{
		public enum EButtonToHighlight
		{
			Single = 0,
			Primary = 1,
			Secondary = 2
		}

		[SerializeField]
		private UIInfoPopup m_infoPopup;

		[SerializeField]
		private EButtonToHighlight m_buttonToHighlight;

		protected void Awake()
		{
		}

		public override void Activate()
		{
			if (m_buttonProxy == null)
			{
				switch (m_buttonToHighlight)
				{
				case EButtonToHighlight.Single:
					m_buttonProxy = m_infoPopup.SingleButton;
					break;
				case EButtonToHighlight.Primary:
					m_buttonProxy = m_infoPopup.PrimaryButton;
					break;
				case EButtonToHighlight.Secondary:
					m_buttonProxy = m_infoPopup.SecondaryButton;
					break;
				}
			}
			m_buttonWidget = m_buttonProxy;
			AddActionCallback();
			if (m_infoPopup.Blocker != null)
			{
				FullscreenBlocker blocker = m_infoPopup.Blocker;
				m_blockers.Add(blocker);
			}
			base.Activate();
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				RemoveActionCallback();
				base.Deactivate();
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
		}

		protected abstract void AddActionCallback();

		protected abstract void RemoveActionCallback();

		protected override void OnClickInternal()
		{
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
		}

		protected override void RemoveButtonFromEventDelegate()
		{
		}
	}
}
