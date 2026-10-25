using AcierProtocol;
using Assets.Scripts.UI.Tutorial.Flow.ShopBases;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	public class TutorialHeroicModeShopSelectCategory : TutorialShopSelectCategoryBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeShopSelectCategory";
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

		protected override EShopCategory GetShopCategory()
		{
			return EShopCategory.Shopcat_Heroic;
		}

		public override int GetHeaderText()
		{
			return 7312;
		}

		public override int GetDescriptionText()
		{
			return 7309;
		}
	}
}
