using UbiCore.Utils;
using UnityEngine;

public class SlowMotionDamped : ITimeScaleProvider
{
	private float m_damping;

	private float m_lastRealtime;

	private float m_timeScale = 1f;

	private float m_target = 1f;

	public float Damping
	{
		set
		{
			m_damping = value;
		}
	}

	public float TargetTimeScale
	{
		set
		{
			m_target = value;
		}
	}

	public SlowMotionDamped(float _damping)
	{
		m_damping = _damping;
		m_lastRealtime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
	}

	public float Evaluate()
	{
		float realTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
		float num = realTime - m_lastRealtime;
		m_timeScale = Mathf.Lerp(m_timeScale, m_target, num * m_damping);
		m_lastRealtime = realTime;
		return m_timeScale;
	}
}
