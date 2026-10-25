using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Engine.ExtensionMethods
{
	public static class UnityExtensions
	{
		public static IEnumerable<GameObject> GetChildren(this Transform _owner)
		{
			for (int i = 0; i < _owner.childCount; i++)
			{
				yield return _owner.GetChild(i).gameObject;
			}
		}

		public static Vector3 ComputeMeshSize(this GameObject _gameObject)
		{
			Vector3 vector = Vector3.zero;
			Renderer[] componentsInChildren = _gameObject.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				Vector3 size = renderer.bounds.size;
				vector = Vector3.Max(size, vector);
			}
			Vector3 localScale = _gameObject.transform.localScale;
			vector.x /= localScale.x;
			vector.y /= localScale.y;
			return vector;
		}

		public static Bounds ComputeMeshBounds(this GameObject _gameObject)
		{
			Bounds result = default(Bounds);
			Renderer[] componentsInChildren = _gameObject.GetComponentsInChildren<Renderer>();
			Renderer[] array = componentsInChildren;
			foreach (Renderer renderer in array)
			{
				result.Encapsulate(renderer.bounds);
			}
			Vector3 localScale = _gameObject.transform.localScale;
			Vector3 scale = new Vector3(1f / localScale.x, 1f / localScale.y, 1f / localScale.z);
			result.extents.Scale(scale);
			result.center.Scale(scale);
			return result;
		}

		public static Rect Scale(this Rect _rect, float _scale)
		{
			return new Rect(_rect.xMin * _scale, _rect.yMin * _scale, _rect.width * _scale, _rect.height * _scale);
		}

		public static Rect Scale(this Rect _rect, Vector2 _scale)
		{
			return new Rect(_rect.xMin * _scale.x, _rect.yMin * _scale.y, _rect.width * _scale.x, _rect.height * _scale.y);
		}

		public static void ParentTo(this Transform _child, Transform _parent)
		{
			_child.parent = _parent;
			_child.localPosition = Vector3.zero;
			_child.localRotation = Quaternion.identity;
			_child.localScale = Vector3.one;
		}

		public static void ParentTo(this GameObject _child, Transform _parent)
		{
			_child.transform.ParentTo(_parent);
		}

		public static void ParentTo(this GameObject _child, GameObject _parent)
		{
			_child.transform.ParentTo(_parent.transform);
		}

		public static void ParentTo(this GameObject _child, Component _parent)
		{
			_child.transform.ParentTo(_parent.transform);
		}

		public static void ParentTo(this Transform _child, GameObject _parent)
		{
			_child.ParentTo(_parent.transform);
		}

		public static void ParentTo(this Transform _child, Component _parent)
		{
			_child.ParentTo(_parent.transform);
		}

		public static string ColorToHex(this Color32 _color)
		{
			return "#" + _color.r.ToString("X2") + _color.g.ToString("X2") + _color.b.ToString("X2");
		}

		public static void DestroyAuto(this Object _object)
		{
			Object.Destroy(_object);
		}

		public static Vector2 To2D(this Vector3 _vec)
		{
			return new Vector2(_vec.x, _vec.z);
		}

		public static Vector3 To3D(this Vector2 _vec, float _y = 0f)
		{
			return new Vector3(_vec.x, _y, _vec.y);
		}
	}
}
