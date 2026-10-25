using Assets.Scripts.UI.ScreenManager;
using UnityEngine;

namespace Assets.Scripts.UI.SkillWheelImplementation
{
	public class ActivateSkillWheel : MonoBehaviour
	{
		private void OnClick()
		{
			Singleton<UIScreenManager>.Instance.SceneData.UIMenuBars.Hide();
			Singleton<UIScreenManager>.Instance.ChangeScreen(EScreenID.AssassinSkillWheel, new UITweenHandle(null, null));
		}
	}
}
