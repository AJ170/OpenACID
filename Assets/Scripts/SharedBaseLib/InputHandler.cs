using UnityEngine;

public class InputHandler : MonoBehaviour
{
	private static readonly string m_name = "InputHandler";

	private bool m_isEnabled;

	public bool isEnabled
	{
		get
		{
			return m_isEnabled;
		}
		set
		{
			m_isEnabled = value;
		}
	}

	public static T FindOrCreate<T>(Transform _parent) where T : InputHandler
	{
		if ((Object)Find<T>(_parent) != (Object)null)
		{
			return null;
		}
		GameObject obj = new GameObject(m_name);
		obj.transform.parent = _parent;
		return obj.AddComponent<T>();
	}

	public static T Find<T>(Transform _parent) where T : InputHandler
	{
		Transform transform = _parent.FindChild(m_name);
		if (transform != null)
		{
			return transform.GetComponent<T>();
		}
		return null;
	}

	public static bool ToggleAllHandlers(bool _enabled)
	{
		InputHandler[] obj = Object.FindObjectsOfType(typeof(InputHandler)) as InputHandler[];
		bool flag = false;
		InputHandler[] array = obj;
		foreach (InputHandler inputHandler in array)
		{
			flag = flag || inputHandler.isEnabled;
			inputHandler.isEnabled = _enabled;
		}
		return flag;
	}
}
