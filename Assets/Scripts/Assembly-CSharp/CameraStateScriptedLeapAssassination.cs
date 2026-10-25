using UnityEngine;

public class CameraStateScriptedLeapAssassination : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.LeapAssassination;
		}
	}

	public CameraStateScriptedLeapAssassination()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(2f, 0.2f, 1.6f),
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
			cameraOffset = new Vector3(-1.8f, 0.2f, 1.5f),
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
		AnimationTaskAssassinateLeapInstigator animationTaskAssassinateLeapInstigator = (AnimationTaskAssassinateLeapInstigator)taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		return animationTaskAssassinateLeapInstigator.Target;
	}
}
