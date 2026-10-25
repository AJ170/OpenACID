using System;
using Assets.Scripts.UI.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.EnvironmentBridge
{
	public class EarthLookAtTransition : ITransitionable
	{
		private readonly Transform m_earthGlobe;

		private readonly Transform m_earthLookAt;

		private readonly Transform m_earthCamSpace;

		private readonly Transform m_earthOuterAim;

		private readonly Transform m_earthLiftRotate;

		private readonly Vector3 m_posStart;

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
				return !BehaviourSingleton<EnvironmentBridgeMain>.Instance.CameraMenuManager.IsTweening;
			}
		}

		public EarthLookAtTransition(EarthGlobeData _globeData, Vector3 _posStart)
		{
			m_earthGlobe = _globeData.EarthGlobe;
			m_earthLookAt = _globeData.EarthLookAt;
			m_earthCamSpace = _globeData.EarthCamSpace;
			m_earthOuterAim = _globeData.EarthOuterAim;
			m_earthLiftRotate = _globeData.EarthLiftRotate;
			m_posStart = _posStart;
		}

		private static Vector3 GetRadiusPosition(float _angle, float _radius)
		{
			float x = Mathf.Sin(_angle * ((float)Math.PI / 180f)) * _radius;
			float z = Mathf.Cos(_angle * ((float)Math.PI / 180f)) * _radius;
			return new Vector3(x, 0f, z);
		}

		public void UpdateTransition(SmoothPositionTransition _transition)
		{
			Vector3 currentPosition = _transition.CurrentPosition;
			m_earthOuterAim.transform.localPosition = GetRadiusPosition(currentPosition.x, 11f);
			m_earthLiftRotate.transform.localRotation = Quaternion.Euler(new Vector3(currentPosition.y, 0f, 0f));
			m_earthLookAt.transform.localPosition = GetRadiusPosition(currentPosition.z, 10f);
			m_earthCamSpace.transform.LookAt(m_earthOuterAim.transform, m_earthOuterAim.transform.up);
			m_earthGlobe.transform.LookAt(m_earthLookAt.transform, m_earthLookAt.transform.up);
		}
	}
}
