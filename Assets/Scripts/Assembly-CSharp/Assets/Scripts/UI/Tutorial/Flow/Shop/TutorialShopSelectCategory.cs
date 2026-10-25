using AcierProtocol;
using Assets.Scripts.UI.Tutorial.Flow.ShopBases;

namespace Assets.Scripts.UI.Tutorial.Flow.Shop
{
	public class TutorialShopSelectCategory : TutorialShopSelectCategoryBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialSelectShopDecryption";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatGuideShop;
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
			return TutorialHelper.ConditionAllowShowGuideShop(this);
		}

		protected override EShopCategory GetShopCategory()
		{
			return EShopCategory.Shopcat_Modules;
		}

		public override int GetHeaderText()
		{
			return 4950;
		}

		public override int GetDescriptionText()
		{
			return 4941;
		}
	}
}
