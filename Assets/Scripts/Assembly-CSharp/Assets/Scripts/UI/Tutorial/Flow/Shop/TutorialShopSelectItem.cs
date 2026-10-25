using AcierProtocol;
using Assets.Scripts.UI.Tutorial.Flow.ShopBases;

namespace Assets.Scripts.UI.Tutorial.Flow.Shop
{
	public class TutorialShopSelectItem : TutorialShopSelectItemBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialShopSelectItem";
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

		protected override string GetItemToSelect()
		{
			return TutorialHelper.TutorialShopSpecialItem;
		}

		public override EFlowId GetGotoShopStep()
		{
			return EFlowId.ProfileCreation;
		}

		public override int GetHeaderText()
		{
			return 4951;
		}

		public override int GetDescriptionText()
		{
			return 4942;
		}
	}
}
