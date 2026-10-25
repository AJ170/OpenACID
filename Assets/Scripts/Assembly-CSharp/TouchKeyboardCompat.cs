using UnityEngine;

// TouchScreenKeyboard.visible only exists in the iOS/Android/WSA players. On PS Vita
// the system IME dialog is up while NGUI's keyboard is open and not finished.
public static class TouchKeyboardCompat
{
	public static bool visible
	{
		get
		{
#if UNITY_PSP2
			TouchScreenKeyboard keyboard = UIInput.mKeyboard;
			return keyboard != null && keyboard.active && !keyboard.done;
#else
			return TouchScreenKeyboard.visible;
#endif
		}
	}
}
