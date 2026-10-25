using System.Collections.Generic;
using Assets.Scripts.Mission.Logic.Combat;
using UbiCore.Debug;
using UnityEngine;
using UnityEngine.AI;

public class CameraStateCombat_Florent : CameraStateBase_Florent
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Combat;
		}
	}

	public CameraStateCombat_Florent()
	{
		LeadDistance = 1f;
		LeadDamping = 0.5f;
		PositionDamping = 0.1f;
		PositionOffset = new Vector3(0f, 5.5f, -3.4f);
		PositionOffsetForward = new Vector3(0f, 2f, -3f);
		PositionOffsetBack = new Vector3(0f, 8f, -7.5f);
		LookAtDamping = 0.1f;
		LookAtOffset = new Vector3(0f, 0f, 0f);
		LookAtOffsetForward = new Vector3(0f, 1f, 3f);
		LookAtOffsetBack = new Vector3(0f, 1.5f, -3f);
		ZoomDamping = 0.1f;
		ZoomDragDistance = 0.15f;
		RotateDragDistance = 0.45f;
		AutoRealign = false;
		MaxAngleToRealign = 30f;
		RealignDamping = 0.2f;
		MinRealignSpeed = 0.1f;
		MaxRealignSpeed = 0.6f;
	}

	protected override void GetTargets(out Vector3 basePosition, out Vector3 leadPosition, out Quaternion viewRotation)
	{
		Profiling.Start("CameraStateCombat::GetTargets", float.MaxValue);
		NavMeshAgent navMeshAgent = AcierCameraController.NavMeshAgent;
		Transform transform = navMeshAgent.transform;
		basePosition = transform.position;
		leadPosition = basePosition;
		Vector3 vector = basePosition;
		viewRotation = m_viewRotation;
		if (!CombatController.IsActive)
		{
			return;
		}
		CombatController instance = CombatController.Instance;
		int num = instance.ParticipantCount();
		if (num > 0)
		{
			leadPosition = Vector3.zero;
			for (int i = 0; i < num; i++)
			{
				CombatParticipant participant = instance.GetParticipant(i);
				Vector3 vector2 = participant.Position - basePosition;
				Vector3 normalized = vector2.normalized;
				float magnitude = vector2.magnitude;
				leadPosition += basePosition + normalized * Mathf.Min(LeadDistance * 2f, magnitude);
			}
			leadPosition /= (float)num;
		}
		CombatParticipant nextTarget = instance.PlayerParticipant.NextTarget;
		if (nextTarget == null)
		{
			return;
		}
		vector = nextTarget.Position;
		Vector3 vector3 = vector - basePosition;
		vector3.y = 0f;
		vector3.Normalize();
		Quaternion quaternion = Quaternion.LookRotation(vector3) * Quaternion.Euler(0f, 45f * (0f - Mathf.Sign(Vector3.Dot(m_viewRotation * Vector3.right, vector3))), 0f);
		List<Quaternion> list = new List<Quaternion>();
		list.Add(quaternion);
		list.Add(quaternion * Quaternion.Euler(0f, 90f, 0f));
		list.Add(quaternion * Quaternion.Euler(0f, 270f, 0f));
		list.Add(quaternion * Quaternion.Euler(0f, 180f, 0f));
		int index = 0;
		float num2 = float.PositiveInfinity;
		for (int j = 0; j < list.Count; j++)
		{
			Vector3 refPosition = m_refPosition;
			Quaternion quaternion2 = list[j];
			Vector3 vector4 = refPosition + quaternion2 * GetPositionOffset() * GetPositionScale();
			float num3 = 0f;
			num = instance.ParticipantCount();
			for (int k = 0; k < num; k++)
			{
				CombatParticipant participant2 = instance.GetParticipant(k);
				Vector3 vector5 = participant2.Position + Vector3.up * 1f;
				Vector3 vector6 = vector4 - vector5;
				Vector3 normalized2 = vector6.normalized;
				float maxDistance = vector6.magnitude + MinDistanceFromObstacles;
				if (Physics.Raycast(vector5, normalized2, maxDistance, m_raycastLayerMask))
				{
					num3 += 1f;
				}
			}
			num3 += Quaternion.Angle(quaternion2, m_viewRotation) / 360f;
			if (num3 < num2)
			{
				index = j;
				num2 = num3;
			}
		}
		viewRotation = list[index];
		Profiling.Stop();
	}

	public override void Realign()
	{
		Vector3 basePosition;
		Vector3 leadPosition;
		Quaternion viewRotation;
		GetTargets(out basePosition, out leadPosition, out viewRotation);
		m_viewRotation = viewRotation;
		m_zoomAmount = 0f;
	}

	protected override float GetAutoRealignFactor()
	{
		return 1f;
	}

	protected override float GetAutoZoomFactor()
	{
		return 1f;
	}

	protected override float GetAutoLeadFactor()
	{
		return 1f;
	}
}
