using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UIAllAssassinsEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Assassins;
			}
		}

		public override void Update()
		{
			if (!UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished)
			{
				base.Value = false;
				return;
			}
			if (Singleton<StateManager>.Instance.ProfileManager.Profile.assassins == null || Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins == null)
			{
				base.Value = false;
				return;
			}
			for (int i = 0; i < Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins.Length; i++)
			{
				if (Singleton<StateManager>.Instance.ProfileManager.Profile.assassins.assassins[i].skillPoints > 0)
				{
					base.Value = true;
					return;
				}
			}
			base.Value = false;
		}
	}
}
