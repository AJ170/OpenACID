using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow
{
	public class TutorialMissionSelection : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Mission selection";
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
			return 3783;
		}

		public override int GetDescriptionText()
		{
			return 3776;
		}
	}
}
