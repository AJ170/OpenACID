using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class PinTransition : ITransitionable
	{
		private Transform m_environmentPin;

		private Transform m_guiPin;

		private Vector3 m_posStart;

		private Camera m_gameCamera;

		private Camera m_uiCamera;

		private SmoothPositionTransition m_waitForTransition;

		public Vector3 Position
		{
			get
			{
				return m_posStart;
			}
		}

		public bool IsDone
		{
			get
			{
				return !BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.IsTweening && m_waitForTransition.IsDone();
			}
		}

		public PinTransition(Transform _environmentPin, Transform _guiPin, Vector3 _posStart, Camera _gameCamera, Camera _uiCamera, SmoothPositionTransition _waitForTransition)
		{
			m_environmentPin = _environmentPin;
			m_guiPin = _guiPin;
			m_posStart = _posStart;
			m_gameCamera = _gameCamera;
			m_uiCamera = _uiCamera;
			m_waitForTransition = _waitForTransition;
		}

		public void UpdateTransition(SmoothPositionTransition _transition)
		{
			Vector3 position = m_gameCamera.WorldToViewportPoint(m_environmentPin.position);
			position = m_uiCamera.ViewportToWorldPoint(position);
			m_guiPin.transform.position = new Vector3(position.x, position.y, 0f);
			m_guiPin.localScale = Vector3.one * Mathf.Clamp(_transition.Progress * 1.3f, 0f, 1f);
		}
	}
}
