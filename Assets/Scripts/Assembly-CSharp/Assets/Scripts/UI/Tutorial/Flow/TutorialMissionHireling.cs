using System;
using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.Helpers;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Tutorial.Flow
{
	public class TutorialMissionHireling : TutorialButtonClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.IntroMission.ToTutorialFlowId();

		public override string Name
		{
			get
			{
				return "Mission Hireling";
			}
		}

		public override ETutorialCategory Category
		{
			get
			{
				return ETutorialCategory.TutCatMs26Hirelings;
			}
		}

		public override STutorialFlowId Index
		{
			get
			{
				return m_index;
			}
		}

		public override int GetHeaderText()
		{
			return 3781;
		}

		public override int GetDescriptionText()
		{
			return 3778;
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionAllowShowGuideHirelings(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.MissionHireling;
		}

		protected override void OnClickInternal()
		{
			UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.FinishCategory(Category, OnCategoryFinished);
		}

		private void OnCategoryFinished(IAsyncResult _ar)
		{
			UIMissionHelper.OnStartCurrentSelectedMissionWithTutorialPopup();
		}
	}
}
