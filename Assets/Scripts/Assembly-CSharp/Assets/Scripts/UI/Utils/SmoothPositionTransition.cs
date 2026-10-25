using System;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class SmoothPositionTransition
	{
		private float m_fade = 90f;

		private float m_speed = 150f;

		private Vector3 m_posStart;

		private Vector3 m_posTarget;

		private ITransitionable m_transitionable;

		public float Progress { get; private set; }

		public Vector3 CurrentPosition { get; private set; }

		public SmoothPositionTransition(ITransitionable _transitionable, Vector3 _posTarget, float _speed = 150f)
		{
			m_transitionable = _transitionable;
			m_posStart = _transitionable.Position;
			m_posTarget = _posTarget;
			if (Vector3.Distance(m_posStart, m_posTarget) == 0f)
			{
				m_fade = Time.deltaTime;
			}
			m_speed = _speed;
		}

		public bool IsDone()
		{
			return m_fade <= 0f && m_transitionable.IsDone;
		}

		public void InstantFinishTransition()
		{
			if (!IsDone())
			{
				m_fade = 0f;
				Step();
			}
		}

		public void Step()
		{
			m_fade -= m_speed * Time.deltaTime;
			m_fade = Mathf.Clamp(m_fade, 0f, 90f);
			float num = Mathf.Cos(m_fade * ((float)Math.PI / 180f)) * 90f;
			float t = Mathf.Sin(num * ((float)Math.PI / 180f));
			float x = Mathf.Lerp(m_posStart.x, m_posTarget.x, t);
			float y = Mathf.Lerp(m_posStart.y, m_posTarget.y, t);
			float z = Mathf.Lerp(m_posStart.z, m_posTarget.z, t);
			CurrentPosition = new Vector3(x, y, z);
			Progress = 1f - m_fade / 90f;
			m_transitionable.UpdateTransition(this);
		}
	}
}
