using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.QuestIntro
{
	internal class TutorialQuestIntroEventTab : TutorialGotoBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialQuestIntroEventTab";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26QuestIntro;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		protected void Start()
		{
			OnScreenInit();
		}

		protected void OnDestroy()
		{
			OnScreenDestroy();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}

		public override EScreenID GetScreenId()
		{
			return Singleton<UIScreenManager>.Instance.CurrentScreenId;
		}

		public override int GetHeaderText()
		{
			return 7145;
		}

		public override int GetDescriptionText()
		{
			return 7146;
		}

		protected override EScreenID GetNextScreenId()
		{
			return EScreenID.EventProgressionScreen;
		}
	}
}
