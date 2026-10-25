using UnityEngine;

public class CameraStateScriptedBenchAssassination : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.BenchAssassination;
		}
	}

	public CameraStateScriptedBenchAssassination()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.PlayerRoot,
			cameraOffset = new Vector3(-2.5f, 1.7f, 0.5f),
			useCameraBone = true,
			cameraBone = HumanBodyBones.Spine,
			cameraDamping = 0.2f,
			lookAtReference = ScriptValues.Reference.TargetRoot,
			lookAtOffset = new Vector3(0f, 1.2f, 0f),
			useLookAtBone = true,
			lookAtBone = HumanBodyBones.Spine,
			lookAtDamping = 0.2f
		};
		m_valuesList = new ScriptValues[1] { scriptValues };
	}

	protected override GameObject FindTarget()
	{
		AnimationTaskManager taskManager = LogicMain.PlayerEntity.UnityComponentCache.AnimationController.TaskManager;
		AnimationTaskBenchKillInstigator animationTaskBenchKillInstigator = (AnimationTaskBenchKillInstigator)taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		return animationTaskBenchKillInstigator.Target;
	}
}
