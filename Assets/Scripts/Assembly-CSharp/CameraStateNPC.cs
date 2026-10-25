using Assets.Scripts.Mission.Logic;
using UnityEngine;

public class CameraStateNPC : CameraStateScripted
{
	private Animator m_animator;

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.NPC;
		}
	}

	public CameraStateNPC()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(-1f, 0.5f, 0.8f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.RightUpperArm,
			cameraDamping = 1.5f,
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0.95f, -0.8f, -0.18f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Head,
			lookAtDamping = 1.5f
		};
		m_valuesList = new ScriptValues[1] { scriptValues };
	}

	protected override GameObject FindTarget()
	{
		EntityId currentInteractionNpc = PlayerHelper.CurrentInteractionNpc;
		if (currentInteractionNpc.IsValid)
		{
			m_animator = currentInteractionNpc.GameEntity.GameObject.GetComponent<Animator>();
			return currentInteractionNpc.GameEntity.GameObject;
		}
		return null;
	}

	protected override Vector3 CalculateRaycastStart()
	{
		if (m_animator != null)
		{
			return m_animator.GetBoneTransform(m_values.lookAtBone).position + Vector3.up * 0.6f;
		}
		return m_lookAtDampedPosition;
	}
}
