using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Crowd
{
	public class CrowdRelevance : MonoBehaviour
	{
		public delegate void CloseToCameraDelegate();

		public static float CloseDamping = 3f;

		public static float FrustumDamping = 3f;

		public static float LOSDamping = 0.5f;

		public static float DecayDamping = 0.6f;

		public static float FarRadius = 40f;

		public static float CloseRadius = 30f;

		public bool IsCloseToCamera = true;

		public bool IsInFrustum = true;

		public bool IsInLOS = true;

		public float Relevance = 1f;

		public event CloseToCameraDelegate OnEnterCloseToCamera;

		public event CloseToCameraDelegate OnLeaveCloseToCamera;

		public void OnDrawGizmosSelected()
		{
			Gizmos.color = new HSBColor(Mathf.Lerp(0f, 0.35f, Relevance), 1f, 1f).ToColor();
			Gizmos.DrawSphere(base.transform.position, 0.5f);
		}

		public void ResetCold()
		{
			IsCloseToCamera = false;
			IsInFrustum = false;
			IsInLOS = false;
			Relevance = 0f;
			if (this.OnLeaveCloseToCamera != null)
			{
				this.OnLeaveCloseToCamera();
			}
		}

		public void ResetHot()
		{
			IsCloseToCamera = true;
			IsInFrustum = true;
			IsInLOS = true;
			Relevance = 1f;
			if (this.OnEnterCloseToCamera != null)
			{
				this.OnEnterCloseToCamera();
			}
		}

		public void UpdateRelevance(Vector3 _currentPosition, float _dt)
		{
			float magnitude = (_currentPosition - Crowd.Instance.LastCameraPosition).magnitude;
			if (magnitude > FarRadius && IsCloseToCamera)
			{
				IsCloseToCamera = false;
				if (this.OnLeaveCloseToCamera != null)
				{
					this.OnLeaveCloseToCamera();
				}
			}
			if (magnitude < CloseRadius && !IsCloseToCamera)
			{
				IsCloseToCamera = true;
				if (this.OnEnterCloseToCamera != null)
				{
					this.OnEnterCloseToCamera();
				}
			}
			Relevance = Damping.Damp(Relevance, 0f, DecayDamping, 1f, _dt);
			if (IsCloseToCamera)
			{
				Relevance = Damping.Damp(Relevance, 1f, CloseDamping, 1f, _dt);
			}
			Vector3 vector = _currentPosition + Vector3.up * 1.5f;
			Vector3 lastCameraPosition = Crowd.Instance.LastCameraPosition;
			Vector3 vector2 = vector - lastCameraPosition;
			IsInFrustum = GeometryUtility.TestPlanesAABB(Crowd.Instance.LastCameraFrustum, new Bounds(vector, Vector3.one));
			if (IsInFrustum)
			{
				Relevance = Damping.Damp(Relevance, 1f, FrustumDamping, 1f, _dt);
			}
			IsInLOS = IsInFrustum && !Physics.Raycast(lastCameraPosition, vector2.normalized, vector2.magnitude, Layers.StaticCollidersLayersMaskPreset);
			if (IsInLOS)
			{
				Relevance = Damping.Damp(Relevance, 1f, LOSDamping, 1f, _dt);
			}
		}
	}
}
