using UnityEngine;

public class SlowMotionEventData : ScriptableObject
{
	[SerializeField]
	private float m_duration = 1f;

	[SerializeField]
	private float m_fadeInDuration = 0.25f;

	[SerializeField]
	private Interpolate.EaseType m_fadeInEase;

	[SerializeField]
	private float m_fadeOutDuration = 0.25f;

	[SerializeField]
	private Interpolate.EaseType m_fadeOutEase;

	[SerializeField]
	private float m_timeScale = 0.1f;

	public float Duration
	{
		get
		{
			return m_duration;
		}
	}

	public float FadeInDuration
	{
		get
		{
			return m_fadeInDuration;
		}
	}

	public Interpolate.EaseType FadeInEase
	{
		get
		{
			return m_fadeInEase;
		}
	}

	public float FadeOutDuration
	{
		get
		{
			return m_fadeOutDuration;
		}
	}

	public Interpolate.EaseType FadeOutEase
	{
		get
		{
			return m_fadeOutEase;
		}
	}

	public float TimeScale
	{
		get
		{
			return m_timeScale;
		}
	}
}
