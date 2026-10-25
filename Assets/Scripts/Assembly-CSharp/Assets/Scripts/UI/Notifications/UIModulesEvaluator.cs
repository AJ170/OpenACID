using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UIModulesEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Modules;
			}
		}

		public override void Init()
		{
			base.Init();
			EventDelegate.Add(Singleton<StateManager>.Instance.ModuleNotificationManager.onModulesChanged, OnLocalStorageUpdate);
		}

		public override void Destroy()
		{
			base.Destroy();
			EventDelegate.Remove(Singleton<StateManager>.Instance.ModuleNotificationManager.onModulesChanged, OnLocalStorageUpdate);
		}

		public override void Update()
		{
			OnLocalStorageUpdate();
		}

		private void OnLocalStorageUpdate()
		{
			base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished && UbiCore.Utils.Singleton<LocalStorage>.Instance.ModuleUnseen;
		}
	}
}
