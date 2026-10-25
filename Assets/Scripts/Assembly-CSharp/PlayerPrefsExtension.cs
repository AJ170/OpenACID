using UnityEngine;

public class PlayerPrefsExtension : MonoBehaviour
{
	public static void SetBool(string name, bool booleanValue)
	{
		PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
	}

	public static bool GetBool(string name)
	{
		if (PlayerPrefs.GetInt(name) == 1)
		{
			return true;
		}
		if (PlayerPrefs.GetInt(name) == 0)
		{
			return false;
		}
		return false;
	}
}
