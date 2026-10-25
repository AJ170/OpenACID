using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State.MissionManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	internal class TutorialHeroicModeContractsTab : TutorialGotoMissionSubMenu
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeContractsTab";
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

		protected void Start()
		{
			OnScreenInit();
		}

		protected void OnDestroy()
		{
			OnScreenDestroy();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowHeroicWithRank(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.ShopScreen;
		}

		protected override MissionSelectionManager.EMissionMenu GetNextSubMenu()
		{
			return MissionSelectionManager.EMissionMenu.ContractMissions;
		}

		public override int GetHeaderText()
		{
			return 7312;
		}

		public override int GetDescriptionText()
		{
			return 7311;
		}
	}
}
