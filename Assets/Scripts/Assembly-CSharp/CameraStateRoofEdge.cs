using Assets.Scripts.Mission.Camera;
using UnityEngine;

public class CameraStateRoofEdge : CameraStateAutoRotation
{
	[SerializeField]
	public RoofEdgeValues m_RoofEdgeValues = new RoofEdgeValues();

	private Vector3 m_roofEdgeNormal;

	private bool m_realign = true;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.RoofEdge;
		}
	}

	public CameraStateRoofEdge()
	{
		FetchAgentFacing();
	}

	public override void CopyFrom(ICameraState _src)
	{
		base.CopyFrom(_src);
		CameraStateRoofEdge cameraStateRoofEdge = (CameraStateRoofEdge)_src;
		m_RoofEdgeValues.CopyFrom(cameraStateRoofEdge.m_RoofEdgeValues);
		m_realignMaxPitchEnd = cameraStateRoofEdge.m_realignMaxPitchEnd;
		m_realignMaxPitchStart = cameraStateRoofEdge.m_realignMaxPitchStart;
	}

	protected override void FetchAgentFacing()
	{
		if (AcierInputHandler.Instance != null)
		{
			m_agentFacing = -AcierInputHandler.Instance.RoofEdgeDetection.RoofEdgeNormal;
		}
	}

	protected override void CalculateReference()
	{
		m_referenceTargetPosition = AcierCameraController.AgentDataFilter.Position;
		base.CalculateReference();
	}

	protected override void CalculateZoomRotationWhenUserRotates()
	{
		base.CalculateZoomRotationWhenUserRotates();
		CameraEvents events = AcierCameraController.Events;
		if (events.cameraDragEvent.Delta.sqrMagnitude > 0f)
		{
			m_realign = false;
		}
	}

	protected override Vector3 CalculateRealignDirection()
	{
		if (Vector3.Angle(m_cameraForward, m_agentFacing) > m_RoofEdgeValues.maxUserYawAngle && m_realign)
		{
			Vector3 vector = Quaternion.Euler(0f, m_RoofEdgeValues.maxUserYawAngle * 0f, 0f) * m_agentFacing;
			Vector3 vector2 = Quaternion.Euler(0f, (0f - m_RoofEdgeValues.maxUserYawAngle) * 0.8f, 0f) * m_agentFacing;
			float num = Vector3.Angle(m_cameraForward, vector);
			float num2 = Vector3.Angle(m_cameraForward, vector2);
			return (!(num < num2)) ? vector2 : vector;
		}
		m_realign = false;
		return m_cameraForward;
	}
}
