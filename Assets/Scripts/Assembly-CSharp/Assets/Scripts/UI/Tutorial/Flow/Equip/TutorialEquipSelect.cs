using AcierProtocol;
using Assets.Scripts.UI.Tutorial.Flow.InventoryScreenBases;

namespace Assets.Scripts.UI.Tutorial.Flow.Equip
{
	public class TutorialEquipSelect : TutorialInventorySelectBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Inventory Item Select";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Equip;
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
			return TutorialHelper.ConditionAllowShowGuideEquip(this);
		}

		protected override string GetItemTechnicalName()
		{
			return TutorialHelper.TutorialEquipItem;
		}

		public override int GetHeaderText()
		{
			return 4299;
		}

		public override int GetDescriptionText()
		{
			return 4302;
		}
	}
}
