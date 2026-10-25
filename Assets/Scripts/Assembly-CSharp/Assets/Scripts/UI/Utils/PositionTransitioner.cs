using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class PositionTransitioner : ITransitionable
	{
		private Transform m_transform;

		private Vector3 m_currentPosition;

		public Vector3 Position
		{
			get
			{
				return m_currentPosition;
			}
			set
			{
				m_currentPosition = value;
			}
		}

		public float Progress { get; private set; }

		public bool IsDone
		{
			get
			{
				return true;
			}
		}

		public PositionTransitioner(Transform _transform)
		{
			m_transform = _transform;
			m_currentPosition = m_transform.position;
			Progress = 0f;
		}

		public void UpdateTransition(SmoothPositionTransition _transition)
		{
			m_currentPosition = _transition.CurrentPosition;
			m_transform.position = _transition.CurrentPosition;
			Progress = _transition.Progress;
		}
	}
}
