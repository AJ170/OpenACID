using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Motion.Hotspots;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

public class AnimationTaskBalance : AnimationTaskBase, IAnimation, IParkourAnimation
{
	public class Parameters : AnimationTaskParameters
	{
		public OffMeshLinkEntity OffMeshLink { get; set; }

		public Parameters()
			: base(AnimationTaskType.Balance)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.Balance;

	public static float MaxDistanceToStart = 3.5f;

	public static float MaxAngleToAgentForward = 180f;

	public static float MaxAngleToLinkDirection = 20f;

	public static float ReferencePointOffsetOnRope = 2f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.Balance;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public OffMeshLinkEntity OffMeshLinkEntity { get; private set; }

	public Vector3 Destination { get; private set; }

	public float Height { get; private set; }

	public AnimationTaskBalance()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBalance(Parameters _parameters)
		: base(_parameters)
	{
		AnimationController.Agent.ResetPath();
		OffMeshLinkEntity offMeshLinkEntity = (OffMeshLinkEntity = _parameters.OffMeshLink);
		OffMeshLink offMeshLink2 = offMeshLinkEntity.OffMeshLink;
		Transform transform = offMeshLink2.startTransform;
		Transform transform2 = offMeshLink2.endTransform;
		Vector3 position = AnimationController.gameObject.transform.position;
		bool flag = false;
		if (offMeshLink2.biDirectional && (position - transform2.position).sqrMagnitude < (position - transform.position).sqrMagnitude)
		{
			transform = offMeshLink2.endTransform;
			transform2 = offMeshLink2.startTransform;
			flag = true;
		}
		UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.BlockOffMeshLink(offMeshLinkEntity.OffMeshLink, flag, AnimationStateBalance.BlockLinkDuration);
		Vector3 position2 = transform.position;
		Vector3 position3 = transform2.position;
		Vector3 startControlPoint = position2 + transform.forward * transform.localScale.y;
		Vector3 endControlPoint = position3 + transform2.forward * transform2.localScale.y;
		AnimationStateBalance animationStateBalance = new AnimationStateBalance(_parameters.AnimationController, position2, position3, startControlPoint, endControlPoint);
		float magnitude = (position3 - position2).magnitude;
		float progress = Mathf.Clamp01(ReferencePointOffsetOnRope / magnitude);
		Vector3 targetPosition = animationStateBalance.GetTargetPosition(progress);
		Vector3 mask = new Vector3(1f, 0f, 1f);
		Vector3 from = (targetPosition - position).Mask(mask);
		float magnitude2 = from.magnitude;
		Vector3 normalized = (position3 - position2).Mask(mask).normalized;
		Vector3 to = AnimationController.transform.forward.Mask(mask);
		float num = Vector3.Angle(from, to);
		float num2 = Vector3.Angle(from, normalized);
		if (magnitude2 > MaxDistanceToStart || num > MaxAngleToAgentForward || num2 > MaxAngleToLinkDirection)
		{
			AnimationStateTurn animationStateTurn = new AnimationStateTurn(_parameters.AnimationController, from.normalized);
			if (!animationStateTurn.IsFinished)
			{
				OnAddState(animationStateTurn);
			}
			OnAddState(new AnimationStateJumpHorizontal(_parameters.AnimationController, position, targetPosition));
			OnAddState(animationStateBalance);
		}
		else
		{
			AnimationStateTurn animationStateTurn2 = new AnimationStateTurn(_parameters.AnimationController, normalized.normalized);
			if (!animationStateTurn2.IsFinished)
			{
				OnAddState(animationStateTurn2);
			}
			OnAddState(animationStateBalance);
		}
		Destination = ((!flag) ? offMeshLinkEntity.EndPosition : offMeshLinkEntity.StartPosition);
		Height = ((!flag) ? (offMeshLinkEntity.EndPosition.y - offMeshLinkEntity.StartPosition.y) : (offMeshLinkEntity.StartPosition.y - offMeshLinkEntity.EndPosition.y));
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Agent.Warp(AnimationController.NextPosition);
	}

	public override void OnDispose()
	{
		base.OnDispose();
		UbiCore.Utils.Singleton<HotspotService>.Instance.OffMeshLinkManager.UnblockOffMeshLink(OffMeshLinkEntity.OffMeshLink);
	}

	public override bool CanCancel()
	{
		return false;
	}
}
