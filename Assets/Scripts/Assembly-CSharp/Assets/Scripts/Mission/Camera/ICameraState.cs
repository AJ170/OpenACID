using UnityEngine;

namespace Assets.Scripts.Mission.Camera
{
	public interface ICameraState
	{
		Vector3 CurrentPosition { get; }

		Vector3 CurrentRaycastPosition { get; }

		Quaternion CurrentRotation { get; }

		float CurrentFieldOfView { get; }

		bool Outlines { get; }

		bool DrawAvatar { get; }

		bool SpherecastAvoidance { get; }

		AcierCameraController.ECameraState StateType { get; }

		void CopyFrom(ICameraState _src);

		void StartState(ICameraState _previousState);

		void EndState();

		void UpdateState();
	}
}
