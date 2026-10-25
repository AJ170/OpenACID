using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Forge
{
	internal class TutorialForgeOpenForge : TutorialGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialForgeOpenForge";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25Forge;
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
			string[] itemsInFrontTechnicalNames = new string[2]
			{
				TutorialHelper.TutorialForgeItemOne,
				TutorialHelper.TutorialForgeItemTwo
			};
			TutorialHelper.PreInitInventorySorting(itemsInFrontTechnicalNames);
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideForge(this);
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
			return 6923;
		}

		public override int GetDescriptionText()
		{
			return 6925;
		}
	}
}
