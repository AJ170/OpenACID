using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.UIView.Rankings
{
	public class UIRankBottomView : UIViewPresenterBase
	{
		[CheckAssigned]
		[SerializeField]
		private UIExtendedLabel m_timer;

		public bool TimerContainerVisibility
		{
			get
			{
				return base.ViewRoot.gameObject.activeSelf;
			}
			set
			{
				base.ViewRoot.gameObject.SetActive(value);
			}
		}

		public void SetTimer(string _dateTimer)
		{
			m_timer.text = _dateTimer;
		}
	}
}
