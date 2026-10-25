using UbiCore.Utils;
using UnityEngine;

public static class Damping
{
	private const float Threshold = 1E-06f;

	public static float Damp(float _from, float _to, float _damping, float _speed)
	{
		return Damp(_from, _to, _damping, _speed, UbiCore.Utils.Singleton<Timer>.Instance.deltaTime);
	}

	public static Vector2 Damp(Vector2 _from, Vector2 _to, float _damping, float _speed)
	{
		return Damp(_from, _to, _damping, _speed, UbiCore.Utils.Singleton<Timer>.Instance.deltaTime);
	}

	public static Vector3 Damp(Vector3 _from, Vector3 _to, float _damping, float _speed)
	{
		return Damp(_from, _to, _damping, _speed, UbiCore.Utils.Singleton<Timer>.Instance.deltaTime);
	}

	public static Quaternion Damp(Quaternion _from, Quaternion _to, float _damping, float _speed)
	{
		return Damp(_from, _to, _damping, _speed, UbiCore.Utils.Singleton<Timer>.Instance.deltaTime);
	}

	public static float CalculateFactor(float _damping, float _speed)
	{
		return CalculateFactor(_damping, _speed, UbiCore.Utils.Singleton<Timer>.Instance.deltaTime);
	}

	public static float Damp(float _from, float _to, float _damping, float _speed, float _dt)
	{
		float num = _dt * _speed;
		float num2 = _damping + num;
		return (!(num2 < 1E-06f)) ? Mathf.Lerp(_from, _to, num / num2) : _to;
	}

	public static Vector2 Damp(Vector2 _from, Vector2 _to, float _damping, float _speed, float _dt)
	{
		float num = _dt * _speed;
		float num2 = _damping + num;
		return (!(num2 < 1E-06f)) ? Vector2.Lerp(_from, _to, num / num2) : _to;
	}

	public static Vector3 Damp(Vector3 _from, Vector3 _to, float _damping, float _speed, float _dt)
	{
		float num = _dt * _speed;
		float num2 = _damping + num;
		return (!(num2 < 1E-06f)) ? Vector3.Lerp(_from, _to, num / num2) : _to;
	}

	public static Quaternion Damp(Quaternion _from, Quaternion _to, float _damping, float _speed, float _dt)
	{
		float num = _dt * _speed;
		float num2 = _damping + num;
		return (!(num2 < 1E-06f)) ? Quaternion.Lerp(_from, _to, num / num2) : _to;
	}

	public static float CalculateFactor(float _damping, float _speed, float _dt)
	{
		if (_speed <= 0f)
		{
			return 0f;
		}
		if (_damping <= 0f)
		{
			return 1f;
		}
		float num = _dt * _speed;
		return num / (num + _damping);
	}
}
