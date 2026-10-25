using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.SkillPoints
{
	internal class TutorialSkillPointsCongratulation : TutorialGoBackBase
	{
		private readonly STutorialFlowId m_index = EFlowId.DEPRECATEDGuideHirelingSelect.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialSkillPointsCongratulation";
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
			return TutorialHelper.ConditionAllowShowGuideSkillPointsAfterFinishedTraining(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.AssassinSkillTree;
		}

		public override int GetHeaderText()
		{
			return 6834;
		}

		public override int GetDescriptionText()
		{
			return 6840;
		}
	}
}
