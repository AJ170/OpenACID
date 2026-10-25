using UnityEngine;

public class CameraStateScriptedHaystackAssassination : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.HaystackAssassination;
		}
	}

	public CameraStateScriptedHaystackAssassination()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.PlayerRoot,
			cameraOffset = new Vector3(-2f, 1.7f, -2f),
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
		AnimationTaskHayStackKillInstigator animationTaskHayStackKillInstigator = (AnimationTaskHayStackKillInstigator)taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		return animationTaskHayStackKillInstigator.Target;
	}
}
