using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Misc
{
	public class UITimerViewPresenter : UIViewPresenterBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIExtendedLabel m_timer;

		[CheckAssigned]
		[SerializeField]
		private UIWidget m_timerContainer;

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
	}
}
