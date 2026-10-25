using AcierProtocol;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.MVC.Controllers.Events;
using Assets.Scripts.UI.ScreenManager;
using Assets.Scripts.UI.State;
using Assets.Scripts.UI.Tutorial.Flow.Bases;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Tutorial.Flow.QuestIntro
{
	internal class TutorialQuestIntroInfo : TutorialButtonProxyClickBase
	{
		private readonly STutorialFlowId m_index = EFlowId.ProfileCreation.ToTutorialFlowId();

		[SerializeField]
		protected UIEventScreenChild m_eventScreenChild;

		public override string Name
		{
			get
			{
				return "TutorialQuestIntroInfo";
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

		protected override void OnSetupInternal()
		{
			m_eventScreenChild.SelectDailyQuestButton();
			Singleton<StateManager>.Instance.MissionSelectionManager.HeroicMissionRank = 1;
			base.OnSetupInternal();
		}

		public override bool ConditionAllowShow()
		{
			return TutorialHelper.ConditionDefaultCheck(this);
		}

		public override EScreenID GetScreenId()
		{
			return EScreenID.EventProgressionScreen;
		}

		public override int GetHeaderText()
		{
			return 7145;
		}

		public override int GetDescriptionText()
		{
			return 8411;
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
