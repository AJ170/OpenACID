using Assets.Scripts.Animation.States;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskSpecialSeduce : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public int AnimatorNpcId { get; set; }

			public Parameters()
				: base(AnimationTaskType.SpecialSeduce)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.SpecialSeduce;

		private const float RemainingDistanceThreshold = 1f;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Special,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.SpecialSeduce;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskSpecialSeduce()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskSpecialSeduce(Parameters _parameters)
			: base(_parameters)
		{
			OnAddState(new AnimationStatePlaySimple(_parameters.AnimationController, new ParameterValues
			{
				AnimatorNpcId = _parameters.AnimatorNpcId
			}, AnimationStateMachineHashes.StateMachine_SeduceWalkArms, "UpperBodySeduce"));
		}

		public override bool CanCancel()
		{
			return true;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.SeduceWalk);
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			float remainingDistance = AnimationController.Agent.remainingDistance;
			remainingDistance = Mathf.Clamp(remainingDistance, 0f, 1f);
			float weight = remainingDistance / 1f;
			AnimationController.Animator.SetLayerWeight(2, weight);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.Animator.SetLayerWeight(2, 0f);
			AnimationController.Animator.SetCurrentActionTypeUpperBody(ActionTypeUpperBody.Invalid);
		}
	}
}
