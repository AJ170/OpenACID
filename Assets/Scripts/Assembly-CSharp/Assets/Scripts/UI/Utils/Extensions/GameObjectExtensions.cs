using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class GameObjectExtensions
	{
		public static string GetPath(this GameObject _obj)
		{
			return _obj.transform.GetPath();
		}

		public static GameObject GetRootGameObject(this GameObject _obj)
		{
			GameObject gameObject = _obj;
			while (gameObject != null && gameObject.transform.parent != null)
			{
				gameObject = gameObject.transform.parent.gameObject;
			}
			return gameObject;
		}

		public static string GetHierarchyPath(this GameObject _go)
		{
			string text = string.Empty;
			if (_go != null)
			{
				Transform transform = _go.transform;
				while (transform != null)
				{
					text = " -> " + transform.gameObject.name + " " + text;
					transform = transform.transform.parent;
				}
				text = "(null)" + text;
			}
			return text;
		}
	}
}
