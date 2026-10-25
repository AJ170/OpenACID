using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.SkillPoints
{
	internal class TutorialSkillPointsAssassinTab : TutorialRootGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialSkillPointsAssassinTab";
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
			return TutorialHelper.ConditionAllowShowGuideSkillPointsNotInAssassinSelection(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.Mission;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.AssassinSelection;
		}

		public override int GetHeaderText()
		{
			return 6834;
		}

		public override int GetDescriptionText()
		{
			return 6835;
		}
	}
}
