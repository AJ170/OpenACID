using UnityEngine;

namespace UbiCore.Helpers
{
	public class SimpleTimer
	{
		private const float INVALID_VALUE = float.PositiveInfinity;

		private float m_time = float.PositiveInfinity;

		private bool started;

		public void Start(float _delay)
		{
			m_time = Time.time + _delay;
			started = true;
		}

		public void Reset()
		{
			m_time = float.PositiveInfinity;
			started = false;
		}

		public bool Finished()
		{
			if (started)
			{
				return Time.time >= m_time;
			}
			return false;
		}

		public bool Started()
		{
			return started;
		}

		public float TimeToFinish()
		{
			return Mathf.Clamp(m_time - Time.time, 0f, m_time);
		}
	}
}
