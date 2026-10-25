using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using SharedBaseLib.System;
using UnityEngine;

public class AnimationTaskJumpDown : AnimationTaskBase, IAnimation, IParkourAnimation
{
	public class Parameters : AnimationTaskParameters
	{
		public OffMeshLinkEntity OffMeshLink { get; set; }

		public Parameters()
			: base(AnimationTaskType.JumpDown)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.JumpDown;

	public static float LandOffset = 1f;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.JumpDown;
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

	public AnimationTaskJumpDown()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskJumpDown(Parameters _parameters)
		: base(_parameters)
	{
		OffMeshLinkEntity = _parameters.OffMeshLink;
		Vector3 vector = _parameters.OffMeshLink.EndPosition;
		NavMeshHit hit;
		if (NavMesh.SamplePosition(_parameters.OffMeshLink.EndPosition + _parameters.OffMeshLink.Rotation * Vector3.forward * LandOffset, out hit, LandOffset, NavMeshLayers.WalkableLayerMaskPreset))
		{
			vector = hit.position;
		}
		else
		{
			Debug.LogWarning("Could not find proper NavMesh position to land, using link end position");
		}
		Destination = vector;
		Height = _parameters.OffMeshLink.StartPosition.y - _parameters.OffMeshLink.EndPosition.y;
		OnAddState(new AnimationStateJumpDown(_parameters.AnimationController, _parameters.OffMeshLink.StartPosition, vector));
	}

	public override bool CanCancel()
	{
		return false;
	}
}
