using Assets.Scripts.UI.ScreenManager;

namespace Assets.Scripts.UI.Behaviours
{
	public class UIAssassinSelectionHelperWithoutAvatar : UIScreenChild
	{
		protected override void OnScreenInit()
		{
			EventDelegate.Add(m_screen.onActivate, OnScreenActivate);
		}

		protected override void OnScreenDestroy()
		{
			if (!(m_screen == null))
			{
				EventDelegate.Remove(m_screen.onActivate, OnScreenActivate);
			}
		}

		private void OnScreenActivate()
		{
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Display.CreateVisuals = false;
		}
	}
}
