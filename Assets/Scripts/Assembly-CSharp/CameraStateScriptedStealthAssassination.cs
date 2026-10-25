using UnityEngine;

public class CameraStateScriptedStealthAssassination : CameraStateScripted
{
	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.StealthAssassination;
		}
	}

	public CameraStateScriptedStealthAssassination()
	{
		ScriptValues scriptValues = new ScriptValues
		{
			cameraReference = ScriptValues.Reference.Player,
			cameraOffset = new Vector3(2f, 0.2f, 1.5f),
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
		AnimationTaskAssassinateStealthInstigator animationTaskAssassinateStealthInstigator = (AnimationTaskAssassinateStealthInstigator)taskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
		return animationTaskAssassinateStealthInstigator.Target;
	}
}
