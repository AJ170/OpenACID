using System;
using System.Diagnostics;
using UnityEngine;

public static class DebugGizmos
{
	private static readonly float ms_arrowHeadLength = 0.25f;

	private static readonly float ms_arrowHeadAngle = 20f;

	private static Quaternion? s_rotation;

	[Conditional("UNITY_DEBUG")]
	public static void DrawCube(Vector3 _position, Color _color, Vector3 _scale)
	{
		Vector3 vector = _scale * 0.5f;
		Vector3[] array = new Vector3[8]
		{
			_position + new Vector3(vector.x, vector.y, vector.z),
			_position + new Vector3(0f - vector.x, vector.y, vector.z),
			_position + new Vector3(0f - vector.x, 0f - vector.y, vector.z),
			_position + new Vector3(vector.x, 0f - vector.y, vector.z),
			_position + new Vector3(vector.x, vector.y, 0f - vector.z),
			_position + new Vector3(0f - vector.x, vector.y, 0f - vector.z),
			_position + new Vector3(0f - vector.x, 0f - vector.y, 0f - vector.z),
			_position + new Vector3(vector.x, 0f - vector.y, 0f - vector.z)
		};
		UnityEngine.Debug.DrawLine(array[0], array[1], _color);
		UnityEngine.Debug.DrawLine(array[1], array[2], _color);
		UnityEngine.Debug.DrawLine(array[2], array[3], _color);
		UnityEngine.Debug.DrawLine(array[3], array[0], _color);
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawPoint(Vector3 _position, Color _color, float _scale, float _duration = 0f)
	{
		Vector3[] array = new Vector3[6]
		{
			_position + Vector3.up * _scale,
			_position - Vector3.up * _scale,
			_position + Vector3.right * _scale,
			_position - Vector3.right * _scale,
			_position + Vector3.forward * _scale,
			_position - Vector3.forward * _scale
		};
		UnityEngine.Debug.DrawLine(array[0], array[1], _color, _duration);
		UnityEngine.Debug.DrawLine(array[2], array[3], _color, _duration);
		UnityEngine.Debug.DrawLine(array[4], array[5], _color, _duration);
		UnityEngine.Debug.DrawLine(array[0], array[2], _color, _duration);
		UnityEngine.Debug.DrawLine(array[0], array[3], _color, _duration);
		UnityEngine.Debug.DrawLine(array[0], array[4], _color, _duration);
		UnityEngine.Debug.DrawLine(array[0], array[5], _color, _duration);
		UnityEngine.Debug.DrawLine(array[1], array[2], _color, _duration);
		UnityEngine.Debug.DrawLine(array[1], array[3], _color, _duration);
		UnityEngine.Debug.DrawLine(array[1], array[4], _color, _duration);
		UnityEngine.Debug.DrawLine(array[1], array[5], _color, _duration);
		UnityEngine.Debug.DrawLine(array[4], array[2], _color, _duration);
		UnityEngine.Debug.DrawLine(array[4], array[3], _color, _duration);
		UnityEngine.Debug.DrawLine(array[5], array[2], _color, _duration);
		UnityEngine.Debug.DrawLine(array[5], array[3], _color, _duration);
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawPath(NavMeshPath _path, Color _color, float _duration = 0f)
	{
		int num = _path.corners.Length;
		if (num >= 2)
		{
			Vector3 start = _path.corners[0];
			for (int i = 1; i < num; i++)
			{
				Vector3 vector = _path.corners[i];
				UnityEngine.Debug.DrawLine(start, vector, _color, _duration);
				start = vector;
			}
		}
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawPlane(Vector3 _position, Vector3 _direction, Color _color, float _duration = 0f)
	{
		Quaternion quaternion = Quaternion.LookRotation(_direction);
		Vector3[] array = new Vector3[4]
		{
			_position + quaternion * Vector3.left,
			_position + quaternion * Vector3.up,
			_position + quaternion * Vector3.right,
			_position + quaternion * Vector3.down
		};
		UnityEngine.Debug.DrawLine(array[0], array[1], _color, _duration);
		UnityEngine.Debug.DrawLine(array[1], array[2], _color, _duration);
		UnityEngine.Debug.DrawLine(array[2], array[3], _color, _duration);
		UnityEngine.Debug.DrawLine(array[3], array[0], _color, _duration);
		DrawArrowInternal(_position, _direction, _color, _duration);
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawCircle(Vector3 _position, Vector3 _direction, float _radius, Color _color, float _duration = 0f)
	{
		DrawCircleInternal(_position, _direction, _radius, _color, _duration);
	}

	private static void DrawCircleInternal(Vector3 _position, Vector3 _direction, float _radius, Color _color, float _duration)
	{
		Quaternion quaternion = Quaternion.LookRotation(_direction);
		Vector3 start = _position + quaternion * new Vector3(0f, _radius, 0f);
		Vector3 vector = _position;
		for (int i = 1; i < 16; i++)
		{
			float num = (float)i / 15f;
			float x = Mathf.Sin(num * (float)Math.PI * 2f) * _radius;
			float y = Mathf.Cos(num * (float)Math.PI * 2f) * _radius;
			vector = _position + quaternion * new Vector3(x, y, 0f);
			if (_duration == 0f)
			{
				UnityEngine.Debug.DrawLine(start, vector, _color);
			}
			else
			{
				UnityEngine.Debug.DrawLine(start, vector, _color, _duration);
			}
			start = vector;
		}
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawSphere(Vector3 _position, float _radius, Color _color, float _duration = 0f)
	{
		DrawCircleInternal(_position, Vector3.right, _radius, _color, _duration);
		DrawCircleInternal(_position, Vector3.up, _radius, _color, _duration);
		DrawCircleInternal(_position, Vector3.forward, _radius, _color, _duration);
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawArrow(Vector3 _position, Vector3 _direction, Color _color, float _duration = 0f)
	{
		DrawArrowInternal(_position, _direction, _color, _duration);
	}

	private static void DrawArrowInternal(Vector3 _position, Vector3 _direction, Color _color, float _duration = 0f)
	{
		if (!(_direction.magnitude <= 0.006f))
		{
			UnityEngine.Debug.DrawRay(_position, _direction, _color, _duration);
			Vector3 vector = Quaternion.LookRotation(_direction) * Quaternion.Euler(0f, 180f + ms_arrowHeadAngle, 0f) * Vector3.forward;
			Vector3 vector2 = Quaternion.LookRotation(_direction) * Quaternion.Euler(0f, 180f - ms_arrowHeadAngle, 0f) * Vector3.forward;
			UnityEngine.Debug.DrawRay(_position + _direction, vector * ms_arrowHeadLength, _color, _duration);
			UnityEngine.Debug.DrawRay(_position + _direction, vector2 * ms_arrowHeadLength, _color, _duration);
		}
	}

	[Conditional("UNITY_DEBUG")]
	public static void DrawToken(Quaternion rotation, Vector3 position)
	{
		Matrix4x4 matrix = Gizmos.matrix;
		if (!s_rotation.HasValue)
		{
			s_rotation = Quaternion.AngleAxis(90f, -Vector3.left);
		}
		Quaternion q = rotation * s_rotation.Value;
		Vector3 vector = Vector3.up * 2f;
		Gizmos.matrix = Matrix4x4.TRS(position + vector, q, Vector3.one);
		Gizmos.DrawFrustum(Vector3.zero, 20f, 2f, 1f, 1f);
		Gizmos.DrawSphere(Vector3.forward * 0.68f, 0.4f);
		Gizmos.matrix = matrix;
	}
}
