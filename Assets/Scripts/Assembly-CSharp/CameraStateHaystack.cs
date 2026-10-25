using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UnityEngine;

public class CameraStateHaystack : CameraStateOrbit
{
	private bool exited;

	private Haystack m_cachedHaystack;

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
			return AcierCameraController.ECameraState.Haystack;
		}
	}

	private Haystack cachedHaystack
	{
		get
		{
			Haystack current = UbiCore.Utils.Singleton<HotspotService>.Instance.HaystackManager.Current;
			if (current == null)
			{
				return m_cachedHaystack;
			}
			return m_cachedHaystack = current;
		}
	}

	public override void EndState()
	{
		base.EndState();
		m_cachedHaystack = null;
	}

	protected override void CalculateReference()
	{
		if (!exited)
		{
			exited = CameraStateHelper.CachedAnimationTaskType == AnimationTaskType.HayStackExit;
		}
		if (exited)
		{
			m_referenceTargetPosition = AcierCameraController.Animator.GetBoneTransform(HumanBodyBones.Spine).position;
		}
		else
		{
			m_referenceTargetPosition = cachedHaystack.InteractionLocators.Center.transform.position;
		}
		base.CalculateReference();
	}
}
