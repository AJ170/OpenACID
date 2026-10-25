using System.Collections.Generic;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Behaviours;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	public class TutorialHeroicModeSelectRank : TutorialButtonClickBase
	{
		private const int HeroicRankToSelect = 1;

		[SerializeField]
		private UIAssassinRankDisplay m_rankDisplay;

		[SerializeField]
		protected UITable m_table;

		[SerializeField]
		protected UIPanel PanelBlocker;

		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxClaim.ToTutorialFlowId();

		private int m_panelBockerDepth;

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeSelectRank";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26HeroicMode;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected override bool OptionsButtonClickable
		{
			get
			{
				return false;
			}
		}

		protected void Start()
		{
			OnScreenInit();
		}

		protected void OnDestroy()
		{
			OnScreenDestroy();
		}

		public override void Activate()
		{
			UIMissionListContractMissions componentInChildren = Singleton<UIScreenManager>.Instance.CurrentScreen.GetComponentInChildren<UIMissionListContractMissions>();
			componentInChildren.IsInteractionDisabled = true;
			base.Activate();
		}

		public override void Deactivate()
		{
			if (IsActive)
			{
				UIMissionListContractMissions componentInChildren = Singleton<UIScreenManager>.Instance.CurrentScreen.GetComponentInChildren<UIMissionListContractMissions>();
				componentInChildren.IsInteractionDisabled = false;
				base.Deactivate();
				if (PanelBlocker != null)
				{
					PanelBlocker.gameObject.SetActive(false);
					PanelBlocker.depth = m_panelBockerDepth;
				}
			}
		}

		protected override void OnDestroyBase()
		{
			base.OnDestroyBase();
			m_button = null;
		}

		protected override void OnSetupInternal()
		{
			List<Transform> children = m_table.Children;
			foreach (Transform item in children)
			{
				UIDropDownItem componentInChildren = item.GetComponentInChildren<UIDropDownItem>();
				if (componentInChildren.Data.Data.Equals(1))
				{
					m_button = item.GetComponentInChildren<UIExtendedButton>();
					break;
				}
			}
			base.OnSetupInternal();
			if (PanelBlocker != null)
			{
				PanelBlocker.gameObject.SetActive(true);
				m_panelBockerDepth = PanelBlocker.depth;
				PanelBlocker.depth = m_buttonWidget.panel.depth - 1;
			}
		}

		protected override void OnClickInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, null);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowHeroicWithRank(this);
		}

		public override EScreenID GetScreenId()
		{
			return Singleton<UIScreenManager>.Instance.CurrentScreenId;
		}

		public override int GetHeaderText()
		{
			return 7143;
		}

		public override int GetDescriptionText()
		{
			return 7337;
		}
	}
}
