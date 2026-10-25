using System.Collections.Generic;
using UbiCore.Utils;
using UnityEngine;

public class SlowMotionEventManager : MonoBehaviour, IAnimatorEventListener, ITimeScaleProvider
{
	internal class RunningSlowMotion
	{
		private SlowMotionEventData m_eventData;

		private float m_remainingTime;

		private float m_result = float.MinValue;

		public float TimeScale
		{
			get
			{
				return m_result;
			}
		}

		public RunningSlowMotion(SlowMotionEventData _data)
		{
			m_eventData = _data;
			m_remainingTime = _data.FadeInDuration + _data.Duration + _data.FadeOutDuration;
		}

		private float Evaluate()
		{
			if (m_remainingTime <= m_eventData.FadeOutDuration)
			{
				return Interpolate.Ease01(m_eventData.FadeOutEase, m_remainingTime / m_eventData.FadeOutDuration);
			}
			float num = m_remainingTime - m_eventData.FadeOutDuration;
			if (num <= m_eventData.Duration)
			{
				return 1f;
			}
			num -= m_eventData.Duration;
			return Interpolate.Ease01(m_eventData.FadeInEase, 1f - num / m_eventData.FadeInDuration);
		}

		public float Update(float dt)
		{
			m_remainingTime -= dt;
			if (m_remainingTime <= 0f)
			{
				return float.MinValue;
			}
			return m_result = Mathf.Lerp(1f, m_eventData.TimeScale, Evaluate());
		}
	}

	private AnimatorDataController m_animatorDataController;

	private List<RunningSlowMotion> m_running = new List<RunningSlowMotion>();

	private List<RunningSlowMotion> m_finished = new List<RunningSlowMotion>();

	private float m_lastRealtime = float.MinValue;

	private void Start()
	{
		m_lastRealtime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
	}

	private void OnEnable()
	{
		TimeScaleManager.RegisterSlowMotion(this);
		m_animatorDataController = GetComponent<AnimatorDataController>();
		m_animatorDataController.RegisterEventListener(this);
	}

	private void OnDisable()
	{
		TimeScaleManager.UnregisterSlowMotion(this);
		m_animatorDataController.UnregisterEventListener(this);
		m_animatorDataController = null;
	}

	public float Evaluate()
	{
		float realTime = UbiCore.Utils.Singleton<Timer>.Instance.realTime;
		float num;
		if (UbiCore.Utils.Singleton<Timer>.Instance.PauseState != Timer.EPauseState.PauseGeneral && m_running.Count > 0)
		{
			float dt = realTime - m_lastRealtime;
			foreach (RunningSlowMotion item in m_running)
			{
				if (item.Update(dt) == float.MinValue)
				{
					m_finished.Add(item);
				}
			}
			foreach (RunningSlowMotion item2 in m_finished)
			{
				m_running.Remove(item2);
			}
			if (m_running.Count > 0)
			{
				num = m_running[0].TimeScale;
				for (int i = 1; i < m_running.Count; i++)
				{
					num = Mathf.Min(m_running[i].TimeScale, num);
				}
			}
			else
			{
				num = 1f;
			}
		}
		else
		{
			num = 1f;
		}
		m_lastRealtime = realTime;
		return num;
	}

	public void AddSlowMotion(SlowMotionEventData _slowMotion)
	{
		m_running.Add(new RunningSlowMotion(_slowMotion));
	}

	public void ClearSlowMotion()
	{
		m_running.Clear();
	}

	public bool OnAnimatorEvent(AnimatorEvent _event)
	{
		AnimatorEventType type = _event.Type;
		if (type == AnimatorEventType.SlowMotion)
		{
			SlowMotionEventData data = _event.Payload as SlowMotionEventData;
			m_running.Add(new RunningSlowMotion(data));
		}
		return false;
	}
}
