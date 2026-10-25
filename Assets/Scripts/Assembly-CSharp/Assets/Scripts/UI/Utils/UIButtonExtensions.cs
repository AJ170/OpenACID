using Assets.Scripts.UI.Behaviours.Buttons;
using Assets.Scripts.UI.Localization;

namespace Assets.Scripts.UI.Utils
{
	public static class UIButtonExtensions
	{
		public static void SetLabel(this UIExtendedButton _button, string _text)
		{
			UIButtonLabelHandler component = _button.gameObject.GetComponent<UIButtonLabelHandler>();
			if (component != null)
			{
				component.SetText(_text);
				return;
			}
			UIExtendedLabel componentInChildren = _button.GetComponentInChildren<UIExtendedLabel>();
			if (componentInChildren != null)
			{
				componentInChildren.text = _text;
			}
		}

		public static void SetLabel(this UIExtendedButton _button, StringId _id)
		{
			UIButtonLabelHandler component = _button.gameObject.GetComponent<UIButtonLabelHandler>();
			if (component != null)
			{
				component.SetText(_id);
				return;
			}
			UIExtendedLabel componentInChildren = _button.GetComponentInChildren<UIExtendedLabel>();
			if (componentInChildren != null)
			{
				componentInChildren.LocalizationKey = _id;
			}
		}

		public static void SetSoundIntent(this UIExtendedButton _button, EButtonIntent _intent)
		{
			UIButtonSounds component = _button.GetComponent<UIButtonSounds>();
			if (component != null && component.ButtonIntent != _intent)
			{
				component.ButtonIntent = _intent;
			}
		}
	}
}
