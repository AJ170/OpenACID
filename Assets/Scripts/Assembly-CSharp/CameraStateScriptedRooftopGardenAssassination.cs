using UnityEngine;

public class CameraStateScriptedRooftopGardenAssassination : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.RooftopGardenAssassination;
		}
	}

	public CameraStateScriptedRooftopGardenAssassination()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.PlayerRoot,
			cameraOffset = new Vector3(-2f, 1.7f, -3.5f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.TargetRoot,
			lookAtOffset = new Vector3(0f, 1.2f, 0f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		ScriptValues scriptValues2 = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.PlayerRoot,
			cameraOffset = new Vector3(2f, 1.7f, -3.5f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.TargetRoot,
			lookAtOffset = new Vector3(0f, 1.2f, 0f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		m_valuesList = new ScriptValues[2] { scriptValues, scriptValues2 };
	}

	protected override GameObject FindTarget()
	{
		AnimationTaskManager taskManager = LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager;
		AnimationTaskRooftopGardenKillInstigator animationTaskRooftopGardenKillInstigator = (AnimationTaskRooftopGardenKillInstigator)taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		return animationTaskRooftopGardenKillInstigator.Target;
	}
}
