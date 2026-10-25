using Assets.Scripts.Database;
using Assets.Scripts.Mission.UI;
using Assets.Scripts.UI.State;
using UbiCore.Utils;

namespace Assets.Scripts.UI.Notifications
{
	public class UIInventoryEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Inventory;
			}
		}

		public override void Init()
		{
			base.Init();
			EventDelegate.Add(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange, OnLocalStorageUpdate);
		}

		public override void Destroy()
		{
			base.Destroy();
			EventDelegate.Remove(Singleton<StateManager>.Instance.ItemNotificationManager.onInventoryNotificationChange, OnLocalStorageUpdate);
		}

		public override void Update()
		{
			OnLocalStorageUpdate();
		}

		private void OnLocalStorageUpdate()
		{
			base.Value = UbiCore.Utils.Singleton<UiMain>.Instance.TutorialManager.IsBasicTutorialFinished && UbiCore.Utils.Singleton<LocalStorage>.Instance.ItemUnseen;
		}
	}
}
