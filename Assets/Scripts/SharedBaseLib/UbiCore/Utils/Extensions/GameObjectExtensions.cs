using UnityEngine;

namespace UbiCore.Utils.Extensions
{
	public static class GameObjectExtensions
	{
		public static void SetLayerRecursively(this GameObject _target, int newLayer)
		{
			_target.layer = newLayer;
			foreach (Transform item in _target.transform)
			{
				item.gameObject.SetLayerRecursively(newLayer);
			}
		}

		public static GameObject FindChildByName(this GameObject _target, string _name)
		{
			if (_target.name == _name)
			{
				return _target;
			}
			for (int i = 0; i < _target.transform.childCount; i++)
			{
				GameObject gameObject = _target.transform.GetChild(i).gameObject.FindChildByName(_name);
				if (gameObject != null)
				{
					return gameObject;
				}
			}
			return null;
		}

		public static GameObject FindDirectChildByName(this GameObject _target, string _name)
		{
			foreach (Transform item in _target.transform)
			{
				if (item.gameObject.name == _name)
				{
					return item.gameObject;
				}
			}
			return null;
		}

		public static T AddComponentIfNotExist<T>(this GameObject go) where T : Component
		{
			T val = go.GetComponent<T>();
			if ((Object)val == (Object)null)
			{
				val = go.AddComponent<T>();
			}
			return val;
		}

		public static bool IsAncestor(this GameObject _this, GameObject _other)
		{
			Transform transform = _this.transform;
			Transform transform2 = _other.transform;
			while (transform2.parent != null)
			{
				if (transform == transform2.parent)
				{
					return true;
				}
				transform2 = transform2.parent;
			}
			return false;
		}

		public static bool IsChild(this GameObject _this, GameObject _other)
		{
			return _other.transform.IsChildOf(_this.transform);
		}
	}
}
