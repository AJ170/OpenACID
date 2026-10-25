using System;
using Assets.Scripts.UI.MVC.UIView.Rankings;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Types;
using Assets.Scripts.UI.Utils.Development;
using UnityEngine;

namespace Assets.Scripts.UI.MVC.Controllers.Rankings
{
	public class UIRankTopAssassinController : UIControllerBase
	{
		[SerializeField]
		[CheckAssigned]
		private UIRankBottomView m_rankEntryView;

		private DateTime m_nextRefreshedDate;

		public bool TimerContainerVisibility
		{
			get
			{
				return m_rankEntryView.TimerContainerVisibility;
			}
			set
			{
				m_rankEntryView.TimerContainerVisibility = value;
			}
		}

		public void UpdateRefreshed(long _lastRefreshedUTC)
		{
			m_nextRefreshedDate = ServerTimeManager.ToUtcTime(_lastRefreshedUTC);
		}

		protected override void OnUpdateUnity()
		{
			base.OnUpdateUnity();
			double num = 0.0;
			DateTime utcNow = Singleton<StateManager>.Instance.ServerTimeManager.UtcNow;
			num = (m_nextRefreshedDate - utcNow).TotalSeconds;
			if (num > 0.0)
			{
				m_rankEntryView.SetTimer(UIDuration.FromSeconds((long)num, true).ToString());
			}
			m_rankEntryView.TimerContainerVisibility = num > 0.0;
		}
	}
}
