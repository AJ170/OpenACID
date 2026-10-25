using UnityEngine;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	public class UIPauseBehaviour : UIBaseBehaviour
	{
		public delegate void TapPause();

		[SerializeField]
		private UISprite m_pauseButtonBackground;

		[SerializeField]
		private UISprite m_pauseIcon;

		public TapPause OnTapPause;

		public override UIScreenWidgetManager.EUiScreenWidget ScreenWidgetType
		{
			get
			{
				return UIScreenWidgetManager.EUiScreenWidget.Pause;
			}
		}

		private void Awake()
		{
		}

		public void Show()
		{
			base.gameObject.SetActive(true);
		}

		public void Exclude()
		{
			base.gameObject.SetActive(false);
		}

		public void SetToSoundButton(bool _isSoundOn)
		{
			m_pauseIcon.spriteName = ((!_isSoundOn) ? "aUI_icon_soundoff" : "aUI_icon_soundon");
		}

		public void SetToOptionButton()
		{
			m_pauseIcon.spriteName = "hud_icon_options";
		}

		private void OnPress(bool _isPressed)
		{
			if (!_isPressed && UICamera.hoveredObject == base.gameObject && OnTapPause != null)
			{
				OnTapPause();
			}
			UpdateButtonBackground(_isPressed);
		}

		private void OnDrag()
		{
			UpdateButtonBackground(UICamera.hoveredObject == base.gameObject);
		}

		private void UpdateButtonBackground(bool _pressed)
		{
			m_pauseButtonBackground.spriteName = "hud_btn_toggle_" + ((!_pressed) ? "up" : "down");
		}
	}
}
