using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.UI;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateScriptedChest : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.Chest;
		}
	}

	public CameraStateScriptedChest()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(1.7f, 1.8f, 1.4f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 1.5f,
			cameraVelocity = new Vector3(0f, -0.2f, 0f),
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0f, 0.6f, 0f),
			useLookAtBone = false,
			lookAtDamping = 1.5f,
			angularStartSpeed = 0f,
			angularTargetSpeed = 15f,
			angularDamping = 3f
		};
		ScriptValues scriptValues2 = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(-1.4f, 1.8f, 1.4f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 1.5f,
			cameraVelocity = new Vector3(0f, -0.2f, 0f),
			lookAtReference = ScriptValues.Reference.Target,
			lookAtOffset = new Vector3(0f, 0.6f, 0f),
			useLookAtBone = false,
			lookAtDamping = 1.5f,
			angularStartSpeed = 0f,
			angularTargetSpeed = -15f,
			angularDamping = 3f
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
