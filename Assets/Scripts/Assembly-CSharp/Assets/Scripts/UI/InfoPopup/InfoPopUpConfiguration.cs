using Assets.Scripts.UI.ScreenManager;
using UnityEngine;

namespace Assets.Scripts.UI.InfoPopup
{
	public sealed class InfoPopUpConfiguration
	{
		public delegate void CloseButtonPressHandler();

		public string Heading;

		public Color QualityColor;

		public Color FrameColor;

		public Color QualityTextColor;

		public bool ShowCloseButton;

		public InfoPopUpContentData Data;

		public PopUpButtonConfiguration[] Buttons;

		public CloseButtonPressHandler OnCloseButtonClicked;
	}
}
