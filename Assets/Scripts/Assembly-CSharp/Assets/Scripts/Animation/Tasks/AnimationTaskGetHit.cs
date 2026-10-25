using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public class AnimationTaskGetHit : AnimationTaskBase
	{
		public class Parameters : AnimationTaskParameters
		{
			public IPositionProvider Source { get; set; }

			public AttackMeleeType AttackMeleeType { get; set; }

			public Vector3 ReactionDirection { get; set; }

			public bool ForceDead { get; set; }

			public Parameters()
				: base(AnimationTaskType.GetHit)
			{
			}
		}

		public const AnimationTaskType sTaskType = AnimationTaskType.GetHit;

		private readonly IPositionProvider m_source;

		private readonly AttackMeleeType m_attackMeleeType;

		private readonly bool m_forceDead;

		private bool m_targetDied;

		private bool m_stateReached;

		private bool m_taskEntered;

		public static readonly AnimationTaskProperties Properties = new AnimationTaskProperties
		{
			Layer = AnimationTaskManager.ELayer.Base,
			ParametersTemplate = new Parameters()
		};

		public override AnimationTaskType TaskType
		{
			get
			{
				return AnimationTaskType.GetHit;
			}
		}

		public static Parameters ParametersTemplate
		{
			get
			{
				return (Parameters)Properties.ParametersTemplate;
			}
		}

		public AnimationTaskGetHit()
			: this(ParametersTemplate)
		{
		}

		public AnimationTaskGetHit(Parameters _parameters)
			: base(_parameters)
		{
			m_source = _parameters.Source;
			m_attackMeleeType = _parameters.AttackMeleeType;
			m_forceDead = _parameters.ForceDead;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			m_taskEntered = true;
			AnimationController.ApplyRootMotion = true;
			if (AnimationController.Agent.enabled)
			{
				AnimationController.Agent.Stop();
				AnimationController.Agent.updatePosition = false;
				AnimationController.Agent.updateRotation = false;
			}
			AnimationController.StateMachine.StateEntered += OnStateEntered;
			Character entity = AnimationController.EntityId.GetEntity<Character>();
			entity.SetKnowinglyInCombat();
			bool flag = m_forceDead || entity.GetHasZeroHealth();
			bool isKnockdowned = entity.GetIsKnockdowned();
			Vector3 roundedReactionDir;
			if (m_source != null && !isKnockdowned)
			{
				roundedReactionDir = ((!flag) ? CombatHelper.ReactionDirectionAngle(m_source, entity) : CombatHelper.ReactionDirection(m_source, entity));
				if (roundedReactionDir.z < 0f)
				{
					entity.FacePosition(entity.Position + (entity.Position - m_source.Position), 0f);
				}
			}
			else
			{
				roundedReactionDir = new Vector3(LogicMain.Random.RandF() - 0.5f, LogicMain.Random.RandF() - 0.5f);
				roundedReactionDir.Normalize();
			}
			if (isKnockdowned)
			{
				AnimationController.Animator.SetCurrentReactionType(ReactionType.GetHurtOnGround);
				m_targetDied = flag;
			}
			else if (flag)
			{
				AttackMeleeType type = CombatHelper.DieAnimationDirection(m_attackMeleeType, roundedReactionDir);
				AnimationController.Animator.SetCurrentReactionType(ReactionType.Die);
				AnimationController.Animator.SetCurrentReactionDirectionX(roundedReactionDir.x);
				AnimationController.Animator.SetCurrentReactionDirectionZ(roundedReactionDir.z);
				AnimationController.Animator.SetCurrentAttackMeleeType(type);
				m_targetDied = true;
			}
			else
			{
				AnimationController.Animator.SetCurrentReactionType(ReactionType.GetHit);
				AnimationController.Animator.SetCurrentReactionDirectionX(roundedReactionDir.x);
				AnimationController.Animator.SetCurrentReactionDirectionZ(roundedReactionDir.z);
				m_targetDied = false;
			}
		}

		public override void OnUpdate()
		{
			base.OnUpdate();
			AnimationHelper.StayOnNavMesh(base.Owner);
		}

		private void OnStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
			if (_state.Layer != (int)base.Layer)
			{
				return;
			}
			if (m_stateReached)
			{
				base.IsFinished = true;
				return;
			}
			for (AnimatorStateMachineStatic.StateMachine parentStateMachine = _state.State.ParentStateMachine; parentStateMachine != null; parentStateMachine = parentStateMachine.ParentStateMachine)
			{
				if (parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_GetHit) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_PushBack) || parentStateMachine.CompareNameHash(AnimationStateMachineHashes.StateMachine_Die))
				{
					AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
					AnimationController.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
					m_stateReached = true;
					if (m_targetDied)
					{
						AnimationController.Animator.SetCurrentGeneralState(GeneralState.Default);
						base.IsFinished = true;
					}
					break;
				}
			}
		}

		public override void OnDispose()
		{
			base.OnDispose();
			if (m_taskEntered)
			{
				if (!m_stateReached)
				{
					AnimationController.Animator.SetCurrentReactionType(ReactionType.Invalid);
					AnimationController.Animator.SetCurrentAttackMeleeType(AttackMeleeType.Invalid);
				}
				if (m_targetDied)
				{
					AnimationController.Animator.SetCurrentStopAction(false);
				}
			}
			AnimationController.StateMachine.StateEntered -= OnStateEntered;
		}

		public override bool CanCancel()
		{
			Character entity = AnimationController.EntityId.GetEntity<Character>();
			return !entity.GetHasZeroHealth();
		}
	}
}
