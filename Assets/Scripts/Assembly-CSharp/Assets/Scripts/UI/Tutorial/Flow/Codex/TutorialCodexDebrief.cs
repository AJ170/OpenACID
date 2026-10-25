using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow.Codex
{
	internal class TutorialCodexDebrief : TutorialButtonProxyClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.GuideShopSelectItem.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "TutorialCodexDebrief";
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
			return 8405;
		}

		protected override void OnClickInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, null);
		}

		protected override int GetButtonText()
		{
			return 3791;
		}
	}
}
