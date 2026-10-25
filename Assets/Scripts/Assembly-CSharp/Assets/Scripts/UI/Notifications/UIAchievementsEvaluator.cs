using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UIAchievementsEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Achievements;
			}
		}

		public override void Init()
		{
			base.Init();
			Singleton<StateManager>.Instance.AchievementsManager.AchievementDataUpdated += OnAchievementDataUpdated;
		}

		public override void Destroy()
		{
			base.Destroy();
			Singleton<StateManager>.Instance.AchievementsManager.AchievementDataUpdated -= OnAchievementDataUpdated;
		}

		public override void Update()
		{
			OnAchievementUpdate();
		}

		private void OnAchievementDataUpdated()
		{
			Update();
		}

		private void OnAchievementUpdate()
		{
			base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished && Singleton<StateManager>.Instance.AchievementsManager.AchievementCategoryData.HasUnread;
		}
	}
}
