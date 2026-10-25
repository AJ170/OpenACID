using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UnityTimer
	{
		public delegate void OnIntevalHandler(float _total, int _loop);

		private const string m_CoroutineName = "co_UpdateTimer";

		public OnIntevalHandler OnInteval;

		private bool m_enabled = true;

		private bool m_running;

		private int m_loopCount;

		private float m_total;

		private float m_interval;

		private readonly MonoBehaviour m_behaviour;

		public float Interval
		{
			set
			{
				m_interval = Mathf.Max(0f, value);
			}
		}

		public bool Enabled
		{
			get
			{
				return m_enabled;
			}
			set
			{
				m_enabled = value;
			}
		}

		public UnityTimer(MonoBehaviour _behaviour)
		{
			m_behaviour = _behaviour;
		}

		public UnityTimer(MonoBehaviour _behaviour, float _interval)
		{
			m_behaviour = _behaviour;
			m_interval = _interval;
		}

		public UnityTimer(MonoBehaviour _behaviour, float _interval, OnIntevalHandler _callback)
		{
			m_behaviour = _behaviour;
			m_interval = _interval;
			OnInteval = (OnIntevalHandler)Delegate.Combine(OnInteval, _callback);
		}

		public void Start()
		{
			Clear();
			m_running = true;
			m_behaviour.StartCoroutine(co_UpdateTimer());
		}

		public void Stop()
		{
			Clear();
			m_running = false;
		}

		public void Pause()
		{
			m_running = false;
		}

		private void Clear()
		{
			m_loopCount = 0;
			m_total = 0f;
		}

		private IEnumerator co_UpdateTimer()
		{
			if (m_interval <= 0f)
			{
				yield break;
			}
			WaitForEndOfFrame waitForFrame = new WaitForEndOfFrame();
			float start = RealTime.time;
			while (m_running)
			{
				yield return waitForFrame;
				if (m_running && RealTime.time - start > m_interval)
				{
					m_total += RealTime.time - start;
					m_loopCount++;
					start = RealTime.time;
					if (OnInteval != null && Enabled)
					{
						OnInteval(m_total, m_loopCount);
					}
				}
			}
		}
	}
}
