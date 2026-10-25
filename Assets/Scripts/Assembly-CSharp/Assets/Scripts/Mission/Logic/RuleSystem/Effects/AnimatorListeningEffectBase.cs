using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public abstract class AnimatorListeningEffectBase<T> : TargetEffectBase<T> where T : IRulesCarrier
	{
		protected class EffectInstance : EffectInstanceBase
		{
			private readonly EventTypeListener m_Listener;

			public EffectInstance(IEffect _effect, EventTypeListener _listener)
				: base(_effect)
			{
				m_Listener = _listener;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				return m_Listener.IsActive;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_Listener.Abort();
			}
		}

		protected abstract class EventTypeListener : IAnimatorEventListener, IDetachableAnimatorEventListener
		{
			public bool IsActive { get; private set; }

			private AnimatorEventType EventType { get; set; }

			private AnimationTaskManager.ELayer Layer { get; set; }

			private AnimatorStateMachineStatic.HashValue AnimHash { get; set; }

			private IGameEntity TargetEntity { get; set; }

			protected EventTypeListener(IGameEntity _targetEntity, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash)
			{
				EventType = _eventType;
				Layer = _layer;
				AnimHash = _animHash;
				TargetEntity = _targetEntity;
				_targetEntity.UnityComponentCache.AnimationController.DataController.RegisterEventListener(this);
				if (AnimHash != null && !AnimHash.IsInvalid)
				{
					if (AnimHash is AnimatorStateMachineStatic.StateHashValue)
					{
						TargetEntity.UnityComponentCache.AnimationController.StateMachine.StateExited += StateMachineOnStateExited;
					}
					else if (AnimHash is AnimatorStateMachineStatic.StateMachineHashValue)
					{
						TargetEntity.UnityComponentCache.AnimationController.StateMachine.StateMachineExited += StateMachineOnStateMachineExited;
					}
				}
				IsActive = true;
			}

			private void StateMachineOnStateExited(object sender, AnimatorStateMachine.StateEventArgs stateEventArgs)
			{
				if (stateEventArgs.State.CompareNameHash(AnimHash))
				{
					Abort();
				}
			}

			private void StateMachineOnStateMachineExited(object sender, AnimatorStateMachine.StateMachineEventArgs stateMachineEventArgs)
			{
				if (stateMachineEventArgs.StateMachine.CompareNameHash(AnimHash))
				{
					Abort();
				}
			}

			public bool OnAnimatorEvent(AnimatorEvent _event)
			{
				if (!Assets.Scripts.Engine.Helpers.Verify(IsActive))
				{
					return true;
				}
				if (_event.Type == EventType)
				{
					Triggered(_event);
					IsActive = false;
					return true;
				}
				return false;
			}

			public void Abort()
			{
				if (IsActive)
				{
					TargetEntity.UnityComponentCache.AnimationController.DataController.UnregisterEventListener(this);
					Detach();
				}
			}

			public bool Detach()
			{
				IsActive = false;
				if (AnimHash != null && !AnimHash.IsInvalid)
				{
					if (AnimHash is AnimatorStateMachineStatic.StateHashValue)
					{
						TargetEntity.UnityComponentCache.AnimationController.StateMachine.StateExited -= StateMachineOnStateExited;
					}
					else if (AnimHash is AnimatorStateMachineStatic.StateMachineHashValue)
					{
						TargetEntity.UnityComponentCache.AnimationController.StateMachine.StateMachineExited -= StateMachineOnStateMachineExited;
					}
				}
				return true;
			}

			protected abstract void Triggered(AnimatorEvent _event);
		}

		protected readonly AnimatorEventType m_EventType;

		protected readonly AnimationTaskManager.ELayer m_Layer;

		protected readonly AnimatorStateMachineStatic.HashValue m_AnimHash;

		protected AnimatorListeningEffectBase(EEffectType _type, IRule _owner, ETargetSource _targetSource, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash)
			: base(_type, _owner, _targetSource)
		{
			m_EventType = _eventType;
			m_Layer = _layer;
			m_AnimHash = _animHash;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			EventTypeListener listener = CreateListener(_entity, _stats, TargetEntityId.GameEntity);
			return CreateEffectInstance(listener);
		}

		protected abstract EventTypeListener CreateListener(T _entity, IBuffedStatsCollectionHandle _stats, IGameEntity _targetEntity);

		protected virtual IEffectInstance CreateEffectInstance(EventTypeListener _listener)
		{
			return new EffectInstance(this, _listener);
		}
	}
}
