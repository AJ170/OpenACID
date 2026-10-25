using Assets.Scripts.UI.MVC.Controllers.Misc;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UITabButtonViewPresenter : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_titleText;

		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_subTitleText;

		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_timer;

		[SerializeField]
		[CheckAssigned]
		private UIWidget m_timerContainer;

		[SerializeField]
		[CheckAssigned]
		private GameObject m_shadows;

		[SerializeField]
		[CheckAssigned]
		private BoxCollider m_boxCollider;

		[SerializeField]
		[CheckAssigned]
		private UITabButtonController m_controller;

		public bool IsSelected
		{
			get
			{
				return !m_shadows.activeSelf;
			}
			set
			{
				m_shadows.SetActive(!value);
			}
		}

		public bool IsColliderEnabled
		{
			get
			{
				return m_boxCollider.enabled;
			}
			set
			{
				m_boxCollider.enabled = value;
			}
		}

		public string Title
		{
			set
			{
				m_titleText.text = value;
			}
		}

		public string SubTitle
		{
			set
			{
				m_subTitleText.text = value;
			}
		}

		public string Timer
		{
			set
			{
				m_timer.text = value;
			}
		}

		public Color TimerColor
		{
			set
			{
				m_timer.color = value;
			}
		}

		public bool TimerContainerVisibility
		{
			get
			{
				return m_timerContainer.gameObject.activeSelf;
			}
			set
			{
				m_timerContainer.gameObject.SetActive(value);
			}
		}

		public bool SubTitleVisibility
		{
			get
			{
				return m_subTitleText.gameObject.activeSelf;
			}
			set
			{
				m_subTitleText.gameObject.SetActive(value);
			}
		}

		private void OnClick()
		{
			if (m_controller.IsInitialized)
			{
				m_controller.OnClickTab();
			}
		}
	}
}
