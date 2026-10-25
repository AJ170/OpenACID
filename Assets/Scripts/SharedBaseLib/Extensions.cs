using System;
using System.Collections.Generic;
using SharedBaseLib.Core;
using UnityEngine;

public static class Extensions
{
	public static string ReplaceFirst(this string text, string search, string replace)
	{
		int num = text.IndexOf(search, StringComparison.CurrentCultureIgnoreCase);
		if (num < 0)
		{
			return text;
		}
		return text.Substring(0, num) + replace + text.Substring(num + search.Length);
	}

	public static Transform FindChildRecursive(this Transform target, string name)
	{
		if (target.name == name)
		{
			return target;
		}
		for (int i = 0; i < target.childCount; i++)
		{
			Transform transform = target.GetChild(i).FindChildRecursive(name);
			if (transform != null)
			{
				return transform;
			}
		}
		return null;
	}

	public static float AngleZX(this Vector3 _direction)
	{
		return Mathf.Atan2(_direction.x, _direction.z) * 57.29578f;
	}

	public static Transform[] FindChildrenRecursive(this Transform target, string name)
	{
		BBList<Transform> collection = new BBList<Transform>();
		CollectChildrenRecursive(target, name, ref collection);
		return collection.ToArray();
	}

	public static void CollectChildrenRecursive(Transform target, string name, ref BBList<Transform> collection)
	{
		if (target.name == name)
		{
			collection.Add(target);
		}
		for (int i = 0; i < target.childCount; i++)
		{
			CollectChildrenRecursive(target.GetChild(i), name, ref collection);
		}
	}

	public static float CalculateLength(this NavMeshPath path)
	{
		int num = path.corners.Length;
		if (num < 2)
		{
			return 0f;
		}
		Vector3 a = path.corners[0];
		float num2 = 0f;
		for (int i = 1; i < num; i++)
		{
			Vector3 vector = path.corners[i];
			num2 += Vector3.Distance(a, vector);
			a = vector;
		}
		return num2;
	}

	public static float CalculateLowestDot(this NavMeshPath path)
	{
		int num = path.corners.Length;
		if (num < 3)
		{
			return 1f;
		}
		Vector3 vector = path.corners[0];
		Vector3 vector2 = path.corners[1];
		float num2 = 1f;
		for (int i = 2; i < num; i++)
		{
			Vector3 vector3 = path.corners[i];
			Vector3 vector4 = vector2 - vector;
			Vector3 vector5 = vector3 - vector2;
			num2 = Mathf.Min(num2, Vector3.Dot(vector4.normalized, vector5.normalized));
			vector = vector2;
			vector2 = vector3;
		}
		return num2;
	}

	public static void Resize<T>(this List<T> _theList, int _count, T _val = default(T))
	{
		if (_theList == null || _count < 0)
		{
			return;
		}
		if (_count == 0)
		{
			_theList.Clear();
			return;
		}
		while (_count > _theList.Count)
		{
			_theList.Add(_val);
		}
		if (_count < _theList.Count)
		{
			_theList.RemoveRange(_count, _theList.Count - _count);
		}
	}
}
