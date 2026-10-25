using Assets.Scripts.Native;

public static class BBAlertViewMgr
{
	public static void ShowAlertView(string _title, string _message, bool _showAlert)
	{
		NativeTools.ShowAlertView(_title, _message);
	}
}
