using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class LookAtTransitioner : ITransitionable
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

		public bool IsDone
		{
			get
			{
				return true;
			}
		}

		public LookAtTransitioner(Transform _transform)
		{
			m_transform = _transform;
			m_currentPosition = m_transform.position;
		}

		public void UpdateTransition(SmoothPositionTransition _transition)
		{
			m_currentPosition = _transition.CurrentPosition;
			m_transform.LookAt(_transition.CurrentPosition);
		}
	}
}
