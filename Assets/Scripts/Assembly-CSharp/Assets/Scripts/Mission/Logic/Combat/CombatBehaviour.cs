using Assets.Scripts.Animation;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public abstract class CombatBehaviour
	{
		public abstract ECombatBehaviourType BehaviourType { get; }

		public CombatParticipant Participant { get; private set; }

		public AnimationController AnimationController { get; private set; }

		public AnimatorDataController AnimatorDataController { get; private set; }

		public CombatController CombatController { get; private set; }

		public Animator Animator { get; private set; }

		public NavMeshAgent Agent { get; private set; }

		protected CombatBehaviour(CombatParticipant participant)
		{
			Participant = participant;
		}

		public virtual void End()
		{
		}

		public virtual void Start(params object[] _params)
		{
			AnimationController = Participant.GameEntity.UnityComponentCache.AnimationController;
			AnimatorDataController = Participant.Proxy.GameObject.GetComponent<AnimatorDataController>();
			Animator = Participant.GameEntity.UnityComponentCache.Animator;
			CombatController = CombatController.Instance;
			Agent = Participant.Agent;
		}

		public virtual void Update()
		{
		}

		public virtual bool Act()
		{
			return false;
		}

		public virtual bool React(CombatParticipant _source, params object[] _params)
		{
			return true;
		}

		public virtual void OnAnimatorTransitionStarted(object _sender, AnimatorStateMachine.TransitionEventArgs _transition)
		{
		}

		public virtual void OnAnimatorStateEntered(object _sender, AnimatorStateMachine.StateEventArgs _state)
		{
		}

		public virtual void OnAnimatorStateMachineEntered(object _sender, AnimatorStateMachine.StateMachineEventArgs _machine)
		{
		}

		public virtual void OnComboEvent(AnimatorEvent _event)
		{
		}

		public virtual void OnHitEvent(AnimatorEvent _event)
		{
		}

		public virtual void OnDodgeEvent(AnimatorEvent _event)
		{
		}

		public virtual void OnParryEvent(AnimatorEvent _event)
		{
		}

		public virtual void OnReactionStartEvent(AnimatorEvent _event)
		{
		}

		public virtual void OnAllowReactEvent(AnimatorEvent _event)
		{
		}

		protected void Run()
		{
			float magnitude = Agent.velocity.magnitude;
			if (!Agent.hasPath || Agent.remainingDistance < CombatController.StrafeDistanceStop)
			{
				Animator.SetCurrentMovementState(MovementState.Idle);
				Agent.ResetPath();
			}
			else if (magnitude > 0.2f)
			{
				Vector3 vector = Agent.steeringTarget - Participant.Position;
				float magnitude2 = vector.magnitude;
				if (magnitude2 < 0.05f)
				{
					vector = Agent.pathEndPosition - Participant.Position;
					magnitude2 = vector.magnitude;
				}
				if (magnitude2 > 0.01f && Vector3.Dot(vector / magnitude2, Participant.Rotation * Vector3.forward) > 0.7f)
				{
					float num = Vector3.Dot(Participant.Rotation * Vector3.forward, Agent.velocity);
					Animator.SetCurrentSpeed(AnimationController.GetSpeed(Vector3.forward * num));
					Animator.SetCurrentMovementState(MovementState.Movement);
					Animator.SetCurrentBankLeftRight(AnimationController.GetBankAmount(Agent));
					Participant.FacePosition(Agent.steeringTarget);
				}
				else
				{
					Vector3 vector2 = Quaternion.Inverse(Participant.Rotation) * Agent.velocity;
					vector2.y = 0f;
					vector2.Normalize();
					Animator.SetCurrentStrafeDirectionX(vector2.x);
					Animator.SetCurrentStrafeDirectionZ(vector2.z);
					Animator.SetCurrentSpeed(AnimationController.GetSpeed(Agent.velocity));
					Animator.SetCurrentMovementState(MovementState.Strafe);
				}
			}
			else
			{
				Animator.SetCurrentMovementState(MovementState.Idle);
			}
		}

		protected void Strafe()
		{
			float magnitude = Agent.velocity.magnitude;
			if (Agent.hasPath)
			{
				Vector3[] corners = Agent.path.corners;
				Vector3 position = Agent.transform.position;
				for (int i = 0; i < corners.Length; i++)
				{
					position = corners[i];
				}
			}
			if (!Agent.hasPath || Agent.remainingDistance < CombatController.StrafeDistanceStop)
			{
				Animator.SetCurrentMovementState(MovementState.Idle);
				Agent.ResetPath();
			}
			else if (Agent.speed > CombatController.AgentStrafeSpeed)
			{
				float num = Vector3.Dot(Participant.Rotation * Vector3.forward, Agent.velocity);
				Animator.SetCurrentSpeed(AnimationController.GetSpeed(Vector3.forward * num));
				Animator.SetCurrentMovementState(MovementState.Movement);
				Animator.SetCurrentBankLeftRight(AnimationController.GetBankAmount(Agent));
				Participant.FacePosition(Agent.steeringTarget);
			}
			else if (magnitude > 0.2f)
			{
				Vector3 vector = Quaternion.Inverse(Participant.Rotation) * Agent.velocity;
				vector.y = 0f;
				vector.Normalize();
				Animator.SetCurrentStrafeDirectionX(vector.x);
				Animator.SetCurrentStrafeDirectionZ(vector.z);
				Animator.SetCurrentSpeed(AnimationController.GetSpeed(Agent.velocity));
				Animator.SetCurrentMovementState(MovementState.Strafe);
			}
			else
			{
				Animator.SetCurrentMovementState(MovementState.Idle);
			}
		}
	}
}
