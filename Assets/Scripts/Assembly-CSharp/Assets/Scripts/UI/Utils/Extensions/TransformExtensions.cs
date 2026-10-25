using UnityEngine;

namespace Assets.Scripts.UI.Utils.Extensions
{
	public static class TransformExtensions
	{
		public static void ClearChildren(this Transform _root)
		{
			for (int num = _root.childCount - 1; num >= 0; num--)
			{
				Transform child = _root.GetChild(num);
				if (child.gameObject != null)
				{
					if (child.transform != null)
					{
						child.transform.parent = null;
					}
					Object.Destroy(child.gameObject);
				}
			}
		}

		public static void SetChildrensVisibility(this Transform _root, bool _status)
		{
			for (int num = _root.childCount - 1; num >= 0; num--)
			{
				Transform child = _root.GetChild(num);
				if (child.gameObject != null)
				{
					child.gameObject.SetActive(_status);
				}
			}
		}

		public static string GetPath(this Transform _trans)
		{
			if (_trans.parent == null)
			{
				return _trans.name;
			}
			return string.Format("{0}/{1}", _trans.parent.GetPath(), _trans.name);
		}
	}
}
