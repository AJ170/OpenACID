using System.Collections.Generic;
using Assets.Scripts.Animation;
using Assets.Scripts.Mission.Logic.Entities;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

public class AnimationTaskParkourSequence : AnimationTaskBase, IAnimation, IParkourAnimation
{
	public class Parameters : AnimationTaskParameters
	{
		public OffMeshLinkEntity OffMeshLink { get; set; }

		public Parameters()
			: base(AnimationTaskType.ParkourSequence)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.ParkourSequence;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.ParkourSequence;
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

	public AnimationTaskParkourSequence()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskParkourSequence(Parameters _parameters)
		: base(_parameters)
	{
		AnimationController.Agent.ResetPath();
		OffMeshLinkEntity offMeshLinkEntity = (OffMeshLinkEntity = _parameters.OffMeshLink);
		OffMeshLink offMeshLink2 = offMeshLinkEntity.OffMeshLink;
		Transform startTransform = offMeshLink2.startTransform;
		Transform endTransform = offMeshLink2.endTransform;
		bool flag = false;
		if (offMeshLink2.biDirectional)
		{
			Vector3 nextPosition = AnimationController.NextPosition;
			if ((nextPosition - endTransform.position).sqrMagnitude < (nextPosition - startTransform.position).sqrMagnitude)
			{
				flag = true;
			}
		}
		List<ParkourStep> stepList = offMeshLinkEntity.StepList;
		for (int i = 0; i < stepList.Count; i++)
		{
			int num = ((!flag) ? i : (stepList.Count - 1 - i));
			ParkourStep parkourStep = stepList[num];
			ParkourStep parkourStep2 = ((num <= 0) ? null : stepList[num - 1]);
			ParkourStep parkourStep3 = ((num >= stepList.Count - 1) ? null : stepList[num + 1]);
			if (flag)
			{
				ParkourStep parkourStep4 = parkourStep2;
				parkourStep2 = parkourStep3;
				parkourStep3 = parkourStep4;
			}
			Vector3 vector = ((!flag) ? parkourStep.StartPosition : parkourStep.EndPosition);
			Vector3 vector2 = ((!flag) ? parkourStep.EndPosition : parkourStep.StartPosition);
			Vector3 vector3 = ((!flag) ? parkourStep.StartTangent : parkourStep.EndTangent);
			Vector3 vector4 = ((!flag) ? parkourStep.EndTangent : parkourStep.StartTangent);
			if (i == 0)
			{
				Vector3 normalized = (vector2 - vector).Mask(new Vector3(1f, 0f, 1f)).normalized;
				AnimationStateTurn animationStateTurn = new AnimationStateTurn(_parameters.AnimationController, normalized);
				if (!animationStateTurn.IsFinished)
				{
					OnAddState(animationStateTurn);
				}
			}
			switch (parkourStep.StepType)
			{
			case ParkourStep.eParkourStepType.Balance:
			{
				bool fadeStart = parkourStep2 == null || parkourStep2.StepType != ParkourStep.eParkourStepType.Balance;
				bool fadeEnd = parkourStep3 == null || parkourStep3.StepType != ParkourStep.eParkourStepType.Balance;
				OnAddState(new AnimationStateBalance(_parameters.AnimationController, vector, vector2, vector + vector3, vector2 + vector4, fadeStart, fadeEnd));
				break;
			}
			case ParkourStep.eParkourStepType.Jump:
				OnAddState(new AnimationStateJumpHorizontal(_parameters.AnimationController, vector, vector2));
				break;
			}
		}
		Destination = ((!flag) ? offMeshLinkEntity.EndPosition : offMeshLinkEntity.StartPosition);
		Height = ((!flag) ? (offMeshLinkEntity.EndPosition.y - offMeshLinkEntity.StartPosition.y) : (offMeshLinkEntity.StartPosition.y - offMeshLinkEntity.EndPosition.y));
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
