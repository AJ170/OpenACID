using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Modules
{
	public class TutorialModulesGotoInventory : TutorialAssassinSelectionGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialModulesGotoInventory";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Modules;
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
			string[] itemsInFrontTechnicalNames = new string[1] { TutorialHelper.TutorialEquipModule };
			TutorialHelper.PreInitInventorySorting(itemsInFrontTechnicalNames);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideModules(this);
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
			return 7151;
		}

		public override int GetDescriptionText()
		{
			return 7152;
		}
	}
}
