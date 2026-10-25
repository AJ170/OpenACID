using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Equip
{
	public class TutorialEquipGotoInventory : TutorialAssassinSelectionGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Assassin Inventory";
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

		public override void Activate()
		{
			base.Activate();
			string[] itemsInFrontTechnicalNames = new string[1] { TutorialHelper.TutorialEquipItem };
			TutorialHelper.PreInitInventorySorting(itemsInFrontTechnicalNames);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideEquip(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.AssassinSelection;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		public override int GetHeaderText()
		{
			return 4299;
		}

		public override int GetDescriptionText()
		{
			return 4301;
		}
	}
}
