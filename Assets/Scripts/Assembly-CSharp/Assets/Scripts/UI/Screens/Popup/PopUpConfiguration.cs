using Assets.Scripts.UI.ScreenManager;
using Database;

namespace Assets.Scripts.UI.Screens.Popup
{
	public sealed class PopUpConfiguration
	{
		public delegate void CloseButtonPressHandler();

		public string Heading;

		public bool DeactivateButtonsOnTap = true;

		public bool ShowCloseButton;

		public GameID CloseButtonAudioControllerId;

		public PopUpContentData Data;

		public PopUpButtonConfiguration[] Buttons;

		public CloseButtonPressHandler OnCloseButtonClicked;

		public UIPopup.EContentAlignTarget Aligntarget;

		public float TimerDuration;

		public EventDelegate.Callback OnTimerRanOut;
	}
}
