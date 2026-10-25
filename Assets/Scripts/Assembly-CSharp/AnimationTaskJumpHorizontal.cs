using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.System;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskJumpHorizontal : AnimationTaskBase, IAnimation, IParkourAnimation
{
	public class Parameters : AnimationTaskParameters
	{
		public OffMeshLinkEntity OffMeshLink { get; set; }

		public Parameters()
			: base(AnimationTaskType.JumpHorizontal)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.JumpHorizontal;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.JumpHorizontal;
		}
	}

	public float Height { get; private set; }

	public Vector3 Destination { get; private set; }

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public OffMeshLinkEntity OffMeshLinkEntity { get; private set; }

	public AnimationTaskJumpHorizontal()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskJumpHorizontal(Parameters _parameters)
		: base(_parameters)
	{
		AnimationController.Agent.ResetPath();
		OffMeshLinkEntity = _parameters.OffMeshLink;
		Vector3 nextPosition = AnimationController.NextPosition;
		Vector3 vector = OffMeshLinkEntity.StartPosition;
		Vector3 vector2 = OffMeshLinkEntity.EndPosition;
		if (OffMeshLinkEntity.OffMeshLink.biDirectional && (nextPosition - vector2).sqrMagnitude < (nextPosition - vector).sqrMagnitude)
		{
			vector = OffMeshLinkEntity.EndPosition;
			vector2 = OffMeshLinkEntity.StartPosition;
		}
		Vector3 normalized = (vector2 - vector).normalized;
		Vector3 sourcePosition = nextPosition + normalized * Vector3.Dot(vector2 - nextPosition, normalized);
		NavMeshHit hit;
		if (NavMesh.SamplePosition(sourcePosition, out hit, 0.5f, NavMeshLayers.WalkableLayerMaskPreset))
		{
			vector2 = hit.position;
		}
		else
		{
			Debug.LogWarning("Could not find proper NavMesh position to land, using link end position");
		}
		Height = vector.y - vector2.y;
		Destination = vector2;
		OnAddState(new AnimationStateJumpHorizontal(_parameters.AnimationController, nextPosition, vector2));
	}

	public override void OnExit(bool _isCanceled)
	{
		base.OnExit(_isCanceled);
		AnimationController.Agent.Warp(AnimationController.NextPosition);
	}

	public override bool CanCancel()
	{
		return false;
	}
}
