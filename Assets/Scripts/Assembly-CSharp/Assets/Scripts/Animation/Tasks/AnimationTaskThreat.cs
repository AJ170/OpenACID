using Assets.Scripts.Mission.Logic;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	internal class AnimationTaskThreat : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public IGameEntity TargetEntity { get; set; }

			public bool PlayRandomThreat { get; set; }

			public Parameters()
				: base(AnimationTaskType.Threat)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.Threat;

		public const int MinAnimatorNpcId = 1;

		public const int MaxAnimatorNpcId = 3;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.Threat;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskThreat()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskThreat(Parameters _parameters)
			: base(_parameters)
		{
			AutoAnimationRotationToTarget autoAnimationRotationToTarget = new AutoAnimationRotationToTarget(AnimationController.EntityId.GameEntity, _parameters.TargetEntity)
			{
				Speed = 2f,
				FireAndForget = true
			};
			autoAnimationRotationToTarget = new AutoAnimationRotationToTarget(_parameters.TargetEntity, AnimationController.EntityId.GameEntity)
			{
				Speed = 2f,
				FireAndForget = true
			};
			if (_parameters.PlayRandomThreat)
			{
				Threat();
			}
		}

		public override void OnEnter()
		{
			base.OnEnter();
			OnEnterSetParameters(AnimationController.Animator);
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			AnimationController.StateMachine.StateMachineEntered -= OnThreatAnimEntered;
			OnExitSetParameters(AnimationController.Animator);
		}

		public override void OnDispose()
		{
			base.OnDispose();
			AnimationController.StateMachine.StateMachineEntered -= OnThreatAnimEntered;
		}

		public override bool CanCancel()
		{
			return true;
		}

		private void Threat()
		{
			AnimationController.StateMachine.StateMachineEntered += OnThreatAnimEntered;
			Animator animator = AnimationController.Animator;
			int npcId = LogicMain.Random.Rand(1, 3);
			animator.SetCurrentNpcId(npcId);
			animator.SetCurrentActionType(ActionType.Threat);
		}

		private void OnThreatAnimEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _stateMachineEventArgs)
		{
			if (_stateMachineEventArgs.StateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Threaten))
			{
				AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
				AnimationController.StateMachine.StateMachineEntered -= OnThreatAnimEntered;
			}
		}

		private void OnEnterSetParameters(Animator _animator)
		{
			_animator.SetCurrentGeneralState(GeneralState.Combat);
			_animator.SetCurrentMovementState(MovementState.Idle);
		}

		private void OnExitSetParameters(Animator _animator)
		{
			_animator.SetCurrentGeneralState(GeneralState.Default);
			AnimationController.Animator.SetCurrentActionType(ActionType.Invalid);
		}
	}
}
