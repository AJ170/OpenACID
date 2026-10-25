using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	internal class TutorialHeroicModeShopTab : TutorialRootGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeShopTab";
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

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowHeroicWithoutRank(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.Mission;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.ShopScreen;
		}

		public override int GetHeaderText()
		{
			return 7141;
		}

		public override int GetDescriptionText()
		{
			return 7142;
		}
	}
}
