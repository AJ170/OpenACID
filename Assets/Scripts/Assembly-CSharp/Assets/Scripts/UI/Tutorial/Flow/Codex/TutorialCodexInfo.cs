using AcierProtocol;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;

namespace Assets.Scripts.UI.Tutorial.Flow.Codex
{
	internal class TutorialCodexInfo : TutorialButtonProxyClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideLoadout.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Codex Information";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Codex;
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
			return TutorialHelper.ConditionAllowShowGuideCodexTab(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.CodexScreen;
		}

		public override int GetHeaderText()
		{
			return 7201;
		}

		public override int GetDescriptionText()
		{
			return 7188;
		}

		protected override int GetButtonText()
		{
			return 3791;
		}
	}
}
