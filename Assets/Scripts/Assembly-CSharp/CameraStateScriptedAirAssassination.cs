using UnityEngine;

public class CameraStateScriptedAirAssassination : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.AirAssassination;
		}
	}

	public CameraStateScriptedAirAssassination()
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
		m_valuesList = new ScriptValues[2] { scriptValues, scriptValues2 };
	}

	protected override GameObject FindTarget()
	{
		AnimationTaskManager taskManager = LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager;
		AnimationTaskAssassinateAirInstigator animationTaskAssassinateAirInstigator = (AnimationTaskAssassinateAirInstigator)taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		return animationTaskAssassinateAirInstigator.Target;
	}
}
