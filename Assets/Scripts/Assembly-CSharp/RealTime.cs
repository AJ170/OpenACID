using UnityEngine;

public class RealTime : MonoBehaviour
{
	private const float s_MaxRealtimedelta = 0.15f;

	public static float time
	{
		get
		{
			return Time.unscaledTime;
		}
	}

	public static float deltaTime
	{
		get
		{
			return Mathf.Min(Time.unscaledDeltaTime, 0.15f);
		}
	}
}
