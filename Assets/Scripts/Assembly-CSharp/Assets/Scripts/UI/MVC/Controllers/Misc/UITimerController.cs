using System;
using Assets.Scripts.UI.MVC.UIView.Misc;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Misc
{
	public class UITimerController : UIControllerBase
	{
		[CheckAssigned]
		[SerializeField]
		private UITimerViewPresenter m_timerView;

		private DateTime m_timeDate;

		public void UpdateController(long _timeUTC, Color _color)
		{
			m_timerView.TimerColor = _color;
			UpdateController(_timeUTC);
		}

		public void UpdateController(long _timeUTC)
		{
			m_timeDate = ServerTimeManager.ToUtcTime(_timeUTC);
			base.IsInitialized = true;
			Show();
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			double num = 0.0;
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			num = (m_timeDate - utcNow).TotalSeconds;
			if (num > 0.0)
			{
				m_timerView.Timer = UIDuration.FromSeconds((long)num, true).ToString();
			}
			m_timerView.TimerContainerVisibility = num > 0.0;
		}
	}
}
