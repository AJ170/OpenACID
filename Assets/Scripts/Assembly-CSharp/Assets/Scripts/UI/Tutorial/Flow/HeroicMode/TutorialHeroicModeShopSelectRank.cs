using AcierProtocol;
using Assets.Scripts.UI.Tutorial.Flow.ShopBases;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	public class TutorialHeroicModeShopSelectRank : TutorialShopSelectItemBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeShopSelectRank";
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

		protected override string GetItemToSelect()
		{
			return TutorialHelper.TutorialShopHeroicRank1Item;
		}

		public override EFlowId GetGotoShopStep()
		{
			return EFlowId.IntroMission;
		}

		public override int GetHeaderText()
		{
			return 7312;
		}

		public override int GetDescriptionText()
		{
			return 7310;
		}
	}
}
