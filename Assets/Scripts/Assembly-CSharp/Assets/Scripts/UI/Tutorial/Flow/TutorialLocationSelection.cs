using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow
{
	public class TutorialLocationSelection : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Location selection";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatGuideEraSelect;
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
			return TutorialHelper.ConditionDefaultCheck(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.Mission;
		}

		public override int GetHeaderText()
		{
			return 3782;
		}

		public override int GetDescriptionText()
		{
			return 3775;
		}
	}
}
