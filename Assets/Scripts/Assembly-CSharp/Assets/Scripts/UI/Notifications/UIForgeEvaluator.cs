using Assets.Scripts.UI.State;

namespace Assets.Scripts.UI.Notifications
{
	public class UIForgeEvaluator : UINewsEvaluatorBase
	{
		public override ENewsType Type
		{
			get
			{
				return ENewsType.Forge;
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
			base.Value = false;
		}
	}
}
