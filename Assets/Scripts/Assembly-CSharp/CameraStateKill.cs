using Assets.Scripts.Mission.Logic;
using UnityEngine;

public class CameraStateKill : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Kill;
		}
	}

	public CameraStateKill()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(1.7f, 0.2f, 1f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.RightUpperArm,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0f, 0.2f, 0.2f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		ScriptValues scriptValues2 = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(-1.4f, 0.2f, 1f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.RightUpperArm,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0f, 0.2f, 0.2f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		ScriptValues scriptValues3 = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(1.7f, 0.2f, -2.8f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.RightUpperArm,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0f, 0.2f, 0.2f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		ScriptValues scriptValues4 = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(-1.4f, 0.2f, -2.8f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.RightUpperArm,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0f, 0.2f, 0.2f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		m_valuesList = new ScriptValues[4] { scriptValues, scriptValues2, scriptValues3, scriptValues4 };
	}

	protected override GameObject FindTarget()
	{
		EntityId finalKillTarget = CombatController.Instance.FinalKillTarget;
		return finalKillTarget.GameObjectEntity.GameObject;
	}
}
