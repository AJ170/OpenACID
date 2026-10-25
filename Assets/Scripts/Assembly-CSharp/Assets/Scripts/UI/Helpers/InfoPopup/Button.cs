using System;

namespace Assets.Scripts.UI.Helpers.InfoPopup
{
	public sealed class Button
	{
		public enum ButtonMode
		{
			AddToForge = 0,
			RemoveFromForge = 1,
			StartForge = 2
		}

		public ButtonMode Mode;

		public bool Active;

		public Action<bool> OnClick;
	}
}
