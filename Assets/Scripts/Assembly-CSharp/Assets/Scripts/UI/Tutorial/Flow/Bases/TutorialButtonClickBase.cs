using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.Bases
{
	public abstract class TutorialButtonClickBase : TutorialClickBase
	{
		[SerializeField]
		protected UIButton m_button;

		public override void Activate()
		{
			base.Activate();
			if (m_buttonWidget != null)
			{
				m_buttonWidget.gameObject.SetActive(true);
			}
		}

		protected override void OnSetupInternal()
		{
			if (m_buttonWidget == null)
			{
				m_buttonWidget = m_button.GetComponent<UIWidget>();
			}
			base.OnSetupInternal();
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_buttonWidget = null;
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
			if (m_button != null)
			{
				EventDelegate.Add(m_button.onClick, base.OnClick);
			}
		}

		protected override void RemoveButtonFromEventDelegate()
		{
			if (m_button != null)
			{
				EventDelegate.Remove(m_button.onClick, base.OnClick);
			}
		}
	}
}
