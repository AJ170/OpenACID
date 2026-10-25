using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateScriptedPigeonCoop : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.PigeonCoop;
		}
	}

	public CameraStateScriptedPigeonCoop()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 1.5f,
			cameraOffset = new Vector3(1.8f, 1.5f, 1.8f),
			cameraVelocity = new Vector3(0f, -0.1f, 0f),
			lookAtReference = ScriptValues.Reference.Player,
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.LeftHand,
			lookAtDamping = 2.5f,
			angularStartSpeed = 0f,
			angularTargetSpeed = 15f,
			angularDamping = 1f
		};
		ScriptValues scriptValues2 = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 1.5f,
			cameraOffset = new Vector3(-1.8f, 1.5f, 1.8f),
			cameraVelocity = new Vector3(0f, -0.1f, 0f),
			lookAtReference = ScriptValues.Reference.Player,
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.LeftHand,
			lookAtDamping = 2.5f,
			angularStartSpeed = 0f,
			angularTargetSpeed = -15f,
			angularDamping = 1f
		};
		m_valuesList = new ScriptValues[2] { scriptValues, scriptValues2 };
	}

	protected override GameObject FindTarget()
	{
		EntityId currentInteractionObject = UbiCore.Utils.Singleton<UiMain>.Instance.LogicInterface.CurrentInteractionObject;
		if (currentInteractionObject == EntityId.InvalidId)
		{
			return null;
		}
		return currentInteractionObject.GameEntity.GameObject;
	}
}
