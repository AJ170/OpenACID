using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Animation.States
{
	public class AnimationStatePlayGeneric : AnimationStateBase
	{
		public abstract class AnimTrigger
		{
			public event Action Triggered
			{
				add
				{
					this.m_Triggered = (Action)Delegate.Combine(this.m_Triggered, value);
				}
				remove
				{
					this.m_Triggered = (Action)Delegate.Remove(this.m_Triggered, value);
				}
			}

			private event Action m_Triggered;

			public abstract void OnEnter(AnimationStatePlayGeneric _state);

			public abstract void OnExit(AnimationStatePlayGeneric _state);

			public abstract void Dispose(AnimationStatePlayGeneric _state);

			protected void FireTriggered()
			{
				if (this.m_Triggered != null)
				{
					this.m_Triggered();
				}
			}
		}

		public class AnimatorEventTrigger : AnimTrigger, IAnimatorEventListener
		{
			private readonly EntityId m_EntityId;

			private readonly AnimatorEventType m_EventType;

			public AnimatorEventTrigger(EntityId _entityId, AnimatorEventType _eventType)
			{
				m_EventType = _eventType;
				m_EntityId = _entityId;
			}

			public AnimatorEventTrigger(AnimatorEventType _eventType)
				: this(EntityId.InvalidId, _eventType)
			{
			}

			public override void OnEnter(AnimationStatePlayGeneric _state)
			{
				AnimatorDataController animatorDataController = null;
				if (m_EntityId == EntityId.InvalidId)
				{
					animatorDataController = _state.AnimationController.DataController;
				}
				else
				{
					GameEntity gameEntity = m_EntityId.TryGetEntity<GameEntity>();
					if (gameEntity != null && gameEntity.IsAttached)
					{
						animatorDataController = gameEntity.UnityComponentCache.AnimationController.DataController;
					}
				}
				if (animatorDataController == null)
				{
					FireTriggered();
				}
				else
				{
					animatorDataController.RegisterEventListener(this);
				}
			}

			public override void OnExit(AnimationStatePlayGeneric _state)
			{
				Dispose(_state);
			}

			public override void Dispose(AnimationStatePlayGeneric _state)
			{
				AnimatorDataController animatorDataController = null;
				if (m_EntityId == EntityId.InvalidId)
				{
					animatorDataController = _state.AnimationController.DataController;
				}
				else
				{
					GameEntity gameEntity = m_EntityId.TryGetEntity<GameEntity>();
					if (gameEntity != null && gameEntity.IsAttached)
					{
						animatorDataController = gameEntity.UnityComponentCache.AnimationController.DataController;
					}
				}
				if (animatorDataController != null)
				{
					animatorDataController.UnregisterEventListener(this);
				}
			}

			public bool OnAnimatorEvent(AnimatorEvent _event)
			{
				if (_event.Type == m_EventType)
				{
					FireTriggered();
					return true;
				}
				return false;
			}
		}

		public class StateMachineEnteredTrigger : AnimTrigger
		{
			private readonly EventHandler<AnimatorStateMachine.StateMachineEventArgs> m_Handler;

			private readonly AnimatorStateMachineStatic.HashValue m_StateMachineHash;

			public StateMachineEnteredTrigger(AnimatorStateMachineStatic.HashValue _stateMachineHash)
			{
				m_Handler = OnStateMachineEntered;
				m_StateMachineHash = _stateMachineHash;
			}

			public override void OnEnter(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateMachineEntered += m_Handler;
			}

			private void OnStateMachineEntered(object sender, AnimatorStateMachine.StateMachineEventArgs e)
			{
				if (e.StateMachine.CompareNameHash(m_StateMachineHash))
				{
					FireTriggered();
				}
			}

			public override void OnExit(AnimationStatePlayGeneric _state)
			{
				Dispose(_state);
			}

			public override void Dispose(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateMachineEntered -= m_Handler;
			}
		}

		public class StateMachineExitedTrigger : AnimTrigger
		{
			private readonly EventHandler<AnimatorStateMachine.StateMachineEventArgs> m_Handler;

			private readonly AnimatorStateMachineStatic.HashValue m_StateMachineHash;

			public StateMachineExitedTrigger(AnimatorStateMachineStatic.HashValue _stateMachineHash)
			{
				m_Handler = OnStateMachineExited;
				m_StateMachineHash = _stateMachineHash;
			}

			public override void OnEnter(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateMachineExited += m_Handler;
			}

			private void OnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs e)
			{
				if (e.StateMachine.CompareNameHash(m_StateMachineHash))
				{
					FireTriggered();
				}
			}

			public override void OnExit(AnimationStatePlayGeneric _state)
			{
				Dispose(_state);
			}

			public override void Dispose(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateMachineExited -= m_Handler;
			}
		}

		public class StateEnteredTrigger : AnimTrigger
		{
			private readonly EventHandler<AnimatorStateMachine.StateEventArgs> m_Handler;

			private readonly AnimatorStateMachineStatic.HashValue m_StateHash;

			public StateEnteredTrigger(AnimatorStateMachineStatic.HashValue _stateHash)
			{
				m_Handler = OnStateEntered;
				m_StateHash = _stateHash;
			}

			public override void OnEnter(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateEntered += m_Handler;
			}

			private void OnStateEntered(object sender, AnimatorStateMachine.StateEventArgs e)
			{
				if (e.State.CompareNameHash(m_StateHash))
				{
					FireTriggered();
				}
			}

			public override void OnExit(AnimationStatePlayGeneric _state)
			{
				Dispose(_state);
			}

			public override void Dispose(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateEntered -= m_Handler;
			}
		}

		public class StateExitedTrigger : AnimTrigger
		{
			private readonly EventHandler<AnimatorStateMachine.StateEventArgs> m_Handler;

			private readonly AnimatorStateMachineStatic.HashValue m_StateHash;

			public StateExitedTrigger(AnimatorStateMachineStatic.HashValue _stateHash)
			{
				m_Handler = OnStateExited;
				m_StateHash = _stateHash;
			}

			public override void OnEnter(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateExited += m_Handler;
			}

			private void OnStateExited(object sender, AnimatorStateMachine.StateEventArgs e)
			{
				if (e.State.CompareNameHash(m_StateHash))
				{
					FireTriggered();
				}
			}

			public override void OnExit(AnimationStatePlayGeneric _state)
			{
				Dispose(_state);
			}

			public override void Dispose(AnimationStatePlayGeneric _state)
			{
				_state.AnimationController.StateMachine.StateExited -= m_Handler;
			}
		}

		private readonly ParameterValues m_Parameters;

		private readonly ParameterValues m_PrevParameters = new ParameterValues();

		private readonly AnimTrigger m_StartTrigger;

		private readonly AnimTrigger m_ResetTrigger;

		private readonly AnimTrigger m_FinishedTrigger;

		private readonly Action m_StartHandler;

		private readonly Action m_ResetHandler;

		private readonly Action m_FinishedHandler;

		private readonly string m_name;

		private int m_LoopCount;

		public AutoAnimationRotation AutoAnimationRotation { get; private set; }

		public AnimationStatePlayGeneric(AnimationController _animationController, ParameterValues _parameters, AnimTrigger _startTrigger, AnimTrigger _resetTrigger, AnimTrigger _finishedTrigger, string _name, int _loopCount = 1)
			: base(AnimationStateType.PlayGeneric, _animationController)
		{
			m_Parameters = _parameters;
			m_StartTrigger = _startTrigger;
			m_ResetTrigger = _resetTrigger;
			m_FinishedTrigger = _finishedTrigger;
			m_StartHandler = OnStartTriggered;
			m_ResetHandler = OnResetTriggered;
			m_FinishedHandler = OnFinishedTriggered;
			m_LoopCount = _loopCount;
			m_name = _name;
		}

		public override void OnEnter()
		{
			base.OnEnter();
			m_PrevParameters.Read(AnimationController, m_Parameters);
			if (m_StartTrigger == null)
			{
				OnStartTriggered();
			}
			else
			{
				m_StartTrigger.OnEnter(this);
				m_StartTrigger.Triggered += m_StartHandler;
			}
			if (m_ResetTrigger != null)
			{
				m_ResetTrigger.OnEnter(this);
				m_ResetTrigger.Triggered += m_ResetHandler;
			}
			if (m_FinishedTrigger != null)
			{
				m_FinishedTrigger.OnEnter(this);
				m_FinishedTrigger.Triggered += m_FinishedHandler;
			}
			if (AutoAnimationRotation != null)
			{
				AutoAnimationRotation.Enter();
			}
		}

		public override void OnExit(bool _isCanceled)
		{
			base.OnExit(_isCanceled);
			if (_isCanceled)
			{
				ResetParameters();
			}
			if (m_StartTrigger != null)
			{
				m_StartTrigger.OnExit(this);
			}
			if (m_ResetTrigger != null)
			{
				m_ResetTrigger.OnExit(this);
			}
			if (m_FinishedTrigger != null)
			{
				m_FinishedTrigger.OnExit(this);
			}
			if (AutoAnimationRotation != null)
			{
				AutoAnimationRotation.Exit();
			}
		}

		public override string ToString()
		{
			return m_name;
		}

		public override void OnDispose()
		{
			if (m_StartTrigger != null)
			{
				m_StartTrigger.Triggered -= m_StartHandler;
				m_StartTrigger.Dispose(this);
			}
			if (m_ResetTrigger != null)
			{
				m_ResetTrigger.Triggered -= m_ResetHandler;
				m_ResetTrigger.Dispose(this);
			}
			if (m_FinishedTrigger != null)
			{
				m_FinishedTrigger.Triggered -= m_FinishedHandler;
				m_FinishedTrigger.Dispose(this);
			}
		}

		public AutoAnimationRotation AddAutoAnimationRotation(IPositionProvider _targetDirection = null)
		{
			if (Assets.Scripts.Engine.Helpers.Verify(AutoAnimationRotation == null))
			{
				AutoAnimationRotation = AddAutoUpdateable(new AutoAnimationRotationToTarget(AnimationController.EntityId.GameEntity, _targetDirection));
			}
			return AutoAnimationRotation;
		}

		private void OnStartTriggered()
		{
			m_Parameters.Write(AnimationController);
		}

		private void OnResetTriggered()
		{
			m_LoopCount--;
			if (m_LoopCount <= 0)
			{
				ResetParameters();
			}
		}

		private void OnFinishedTriggered()
		{
			base.IsFinished = true;
		}

		private void ResetParameters()
		{
			m_PrevParameters.Write(AnimationController);
		}
	}
}
