using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UICurrentAssasinEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.SkillPointsCurrent;
			}
		}

		public override void Init()
		{
			base.Init();
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Add(OnAssassinChange);
		}

		public override void Destroy()
		{
			base.Destroy();
			Singleton<StateManager>.Instance.ProfileManager.OnAssassinChange.Remove(OnAssassinChange);
		}

		public override void Update()
		{
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished)
			{
				base.Value = false;
			}
			else if (Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin == null || !IsNewsTypeUnlocked())
			{
				base.Value = false;
			}
			else
			{
				base.Value = Singleton<StateManager>.Instance.ProfileManager.CurrentAssassin.skillPoints > 0;
			}
		}

		private void OnAssassinChange(params object[] _params)
		{
			Update();
		}
	}
}
