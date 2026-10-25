namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIEnemyUnitFrameBehaviour : UIUnitFrameBehaviour
	{
		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.EnemyUnitFrame;
			}
		}

		public override void Init()
		{
			if (!IsInitialized)
			{
				base.Init();
				base.gameObject.SetActive(false);
			}
		}
	}
}
