using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Screens.MissionScreen;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	public class TutorialHeroicModeRankDropDown : TutorialButtonClickBase
	{
		private const int HeroicRankToSelect = 0;

		private readonly STutorialFlowId m_index = EFlowId.GuideMysteryBoxLootAgain.ToTutorialFlowId();

		[SerializeField]
		protected UIPanel PanelBlocker;

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeRankDropDown";
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

		public override void Activate()
		{
			base.Activate();
			if (PanelBlocker != null)
			{
				PanelBlocker.gameObject.SetActive(true);
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
				}
			}
		}

		protected override void OnSetupInternal()
		{
			UIMissionListContractMissions componentInChildren = Singleton<UIScreenManager>.Instance.CurrentScreen.GetComponentInChildren<UIMissionListContractMissions>();
			componentInChildren.IsInteractionDisabled = true;
			Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank = 0;
			base.OnSetupInternal();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowHeroicWithRank(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.Mission;
		}

		public override int GetHeaderText()
		{
			return 7143;
		}

		public override int GetDescriptionText()
		{
			return 7144;
		}
	}
}
