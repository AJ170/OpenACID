using System;
using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Contracts
{
	public class TutorialContractsDetails : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		private readonly List<EventDelegate> m_callbacks = new List<EventDelegate>();

		public override string Name
		{
			get
			{
				return "TutorialContractsDetails";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Contracts;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}

		protected override void AddButtonToEventDelegate(UIWidget[] _widgets)
		{
			m_callbacks.AddRange(m_button.onClick);
			m_button.onClick.Clear();
			base.AddButtonToEventDelegate(_widgets);
		}

		protected override void RemoveButtonFromEventDelegate()
		{
			base.RemoveButtonFromEventDelegate();
			m_button.onClick.Clear();
			m_button.onClick.AddRange(m_callbacks);
		}

		protected override void OnClickInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, OnStartMission);
		}

		private void OnStartMission(IAsyncResult _ar)
		{
			for (int i = 0; i < m_callbacks.Count; i++)
			{
				EventDelegate eventDelegate = m_callbacks[i];
				eventDelegate.Execute();
			}
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.Mission;
		}

		public override int GetHeaderText()
		{
			return 7138;
		}

		public override int GetDescriptionText()
		{
			return 7140;
		}
	}
}
