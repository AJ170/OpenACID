using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

public class AnimationTaskBenchEnter : AnimationTaskBase
{
	public class Parameters : AnimationTaskParameters
	{
		public Bench Bench { get; set; }

		public InteractionLocators.Locator SelectedLocator { get; set; }

		public Parameters()
			: base(AnimationTaskType.BenchEnter)
		{
		}
	}

	public const AnimationTaskType sTaskType = AnimationTaskType.BenchEnter;

	public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
	{
		Layer = AnimationTaskManager.ELayer.Base,
		ParametersTemplate = new Parameters()
	};

	public override AnimationTaskType TaskType
	{
		get
		{
			return AnimationTaskType.BenchEnter;
		}
	}

	public static Parameters ParametersTemplate
	{
		get
		{
			return (Parameters)Properties.ParametersTemplate;
		}
	}

	public AnimationTaskBenchEnter()
		: this(ParametersTemplate)
	{
	}

	public AnimationTaskBenchEnter(Parameters _parameters)
		: base(_parameters)
	{
		Vector3 forward = _parameters.AnimationController.Agent.transform.forward;
		forward.y = 0f;
		forward.Normalize();
		OnAddState(new AnimationStateApproachLink(_parameters.AnimationController, _parameters.SelectedLocator.Position.transform.position, Quaternion.LookRotation(forward)));
		OnAddState(new AnimationStateBenchEnter(_parameters.AnimationController, _parameters.Bench, _parameters.SelectedLocator));
	}

	public override bool CanCancel()
	{
		return false;
	}
}
