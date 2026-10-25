using AcierProtocol;
using Assets.Scripts.UI.Tutorial.Flow.ShopBases;

namespace Assets.Scripts.UI.Tutorial.Flow.Shop
{
	public class TutorialShopBuyItem : TutorialShopBuyItemBase
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();

		private long m_addedVirtualHardCurrency;

		public override string Name
		{
			get
			{
				return "TutorialShopBuyItem";
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

		protected override string GetItemToBuy()
		{
			return TutorialHelper.TutorialShopSpecialItem;
		}

		public override int GetHeaderText()
		{
			return 4952;
		}

		public override int GetDescriptionText()
		{
			return 4943;
		}
	}
}
