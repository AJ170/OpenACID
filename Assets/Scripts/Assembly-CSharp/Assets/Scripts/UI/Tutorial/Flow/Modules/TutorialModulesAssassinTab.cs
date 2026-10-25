using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Modules
{
	internal class TutorialModulesAssassinTab : TutorialRootGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialModulesAssassinTab";
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

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideModules(this);
		}

		public override EScreenID GetScreenId()
		{
			EScreenID result = EScreenID.Mission;
			if (Singleton<UIScreenManager>.Instance.CurrentScreenId != EScreenID.NewsFeedScreen)
			{
				result = Singleton<UIScreenManager>.Instance.CurrentScreenId;
			}
			return result;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.AssassinSelection;
		}

		public override int GetHeaderText()
		{
			return 7317;
		}

		public override int GetDescriptionText()
		{
			return 7319;
		}
	}
}
