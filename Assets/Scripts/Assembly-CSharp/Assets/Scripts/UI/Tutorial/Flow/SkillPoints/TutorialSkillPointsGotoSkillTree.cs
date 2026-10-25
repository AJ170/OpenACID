using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.SkillPoints
{
	internal class TutorialSkillPointsGotoSkillTree : TutorialAssassinSelectionGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialSkillPointsGotoSkillTree";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs25SkillPoints;
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
			return TutorialHelper.ConditionAllowShowGuideSkillPoints(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.AssassinSelection;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.AssassinSkillTree;
		}

		public override int GetHeaderText()
		{
			return 6834;
		}

		public override int GetDescriptionText()
		{
			return 6836;
		}
	}
}
