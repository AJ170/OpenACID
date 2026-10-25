using Assets.Scripts.UI.ScreenManager;

namespace Assets.Scripts.UI.Animations.ValueObservers
{
	public class UIAssassinInfoObserver : IAnimationObserver
	{
		private void OnEnable()
		{
			Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Show();
		}

		private void OnDisable()
		{
			if (Singleton<UIScreenManager>.Instance.SceneData != null && Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer != null)
			{
				Singleton<UIScreenManager>.Instance.SceneData.AssassinContainer.Hide();
			}
		}
	}
}
