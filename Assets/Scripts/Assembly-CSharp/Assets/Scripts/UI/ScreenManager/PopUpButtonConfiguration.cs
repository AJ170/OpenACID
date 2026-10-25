using AcierProtocol;
using Assets.Scripts.UI.Behaviours.Buttons;

namespace Assets.Scripts.UI.ScreenManager
{
	public class PopUpButtonConfiguration
	{
		public enum EButtonType
		{
			eNormal = 0,
			eSuggestedClick = 1
		}

		public delegate void ButtonPressHandler(object _userData);

		public ButtonPressHandler OnButtonPressed;

		public object Data;

		public string Label;

		public string IconSpriteName;

		public UIScreenManager.EAtlasType IconSpriteAtlas;

		public bool IsEnabled = true;

		public EButtonType ButtonType;

		public EButtonIntent SoundIntent;

		public float Delay;

		public Resource Resource;

		public bool IsDisabledClickAllowed;

		public bool SuppressAudio;
	}
}
