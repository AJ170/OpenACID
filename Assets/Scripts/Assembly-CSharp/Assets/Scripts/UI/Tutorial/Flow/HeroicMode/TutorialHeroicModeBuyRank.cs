using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Tutorial.Flow.ShopBases;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.HeroicMode
{
	public class TutorialHeroicModeBuyRank : TutorialShopBuyItemBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialHeroicModeBuyRank";
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

		protected override string GetItemToBuy()
		{
			return TutorialHelper.TutorialShopHeroicRank1Item;
		}

		protected override void OnPurchasedInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.NextStep(Category);
		}

		public override int GetHeaderText()
		{
			return 7143;
		}

		public override int GetDescriptionText()
		{
			return 7336;
		}
	}
}
