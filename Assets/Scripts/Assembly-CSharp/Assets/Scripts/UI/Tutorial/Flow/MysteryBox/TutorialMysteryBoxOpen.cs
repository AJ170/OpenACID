using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.MysteryBox
{
	internal class TutorialMysteryBoxOpen : TutorialGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Mystery Box Open";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25MysteryBox;
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
			return TutorialHelper.ConditionAllowShowGuideMysteryBox(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.InventoryScreen;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.MysteryBox;
		}

		public override int GetHeaderText()
		{
			return 6956;
		}

		public override int GetDescriptionText()
		{
			return 6899;
		}
	}
}
