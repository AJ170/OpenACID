using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateRooftopGarden : CameraStateOrbit
{
	private bool exited;

	private RooftopGarden m_cachedRooftopGarden;

	public override bool Outlines
	{
		get
		{
			return false;
		}
	}

	public override bool SpherecastAvoidance
	{
		get
		{
			return true;
		}
	}

	public override AcierCameraController.ECameraState StateType
	{
		get
		{
			return AcierCameraController.ECameraState.RooftopGarden;
		}
	}

	private RooftopGarden cachedRooftopGarden
	{
		get
		{
			RooftopGarden current = UbiCore.Utils.Singleton<HotspotService>.Instance.RooftopGardenManager.Current;
			if (current == null)
			{
				return m_cachedRooftopGarden;
			}
			return m_cachedRooftopGarden = current;
		}
	}

	public override void EndState()
	{
		base.EndState();
		m_cachedRooftopGarden = null;
	}

	protected override void CalculateReference()
	{
		if (!exited)
		{
			exited = CameraStateHelper.CachedAnimationTaskType == AnimationTaskType.RooftopGardenExit;
		}
		if (exited)
		{
			m_referenceTargetPosition = AcierCameraController.Animator.GetBoneTransform(HumanBodyBones.Spine).position;
		}
		else
		{
			m_referenceTargetPosition = cachedRooftopGarden.InteractionLocators.Center.transform.position;
		}
		base.CalculateReference();
	}
}
