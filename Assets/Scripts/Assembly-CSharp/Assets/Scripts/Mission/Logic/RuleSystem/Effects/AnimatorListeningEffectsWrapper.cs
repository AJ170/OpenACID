using System;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class AnimatorListeningEffectsWrapper<T> : AnimatorListeningEffectBase<T> where T : IRulesCarrier
	{
		protected class WrapperEffectInstance : Assets.Scripts.Mission.Logic.RuleSystem.WrapperEffectInstance
		{
			protected readonly EventTypeListener m_Listener;

			public WrapperEffectInstance(IEffect _effect, EventTypeListener _listener)
				: base(_effect)
			{
				m_Listener = _listener;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (m_Listener.IsActive)
				{
					return true;
				}
				return base.Update(_entity);
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_Listener.Abort();
				base.Abort(_entity);
			}
		}

		protected class WrapperEventTypeListener : EventTypeListener
		{
			private readonly EffectsWrapper m_Wrapper;

			private readonly WrapperEffectInstance m_EffectInstance;

			private static readonly BBList<IEffectInstance> s_WrappedEffectInstances = new BBList<IEffectInstance>();

			public EffectsWrapper EffectsWrapper
			{
				get
				{
					return m_Wrapper;
				}
			}

			public WrapperEffectInstance EffectInstance
			{
				get
				{
					return m_EffectInstance;
				}
			}

			public WrapperEventTypeListener(IGameEntity _targetEntity, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash, EffectsWrapper _wrapper, AnimatorListeningEffectsWrapper<T> _effect)
				: base(_targetEntity, _eventType, _layer, _animHash)
			{
				m_Wrapper = _wrapper;
				m_EffectInstance = CreateEffectInstance(_effect);
			}

			protected virtual WrapperEffectInstance CreateEffectInstance(AnimatorListeningEffectsWrapper<T> _effect)
			{
				return new WrapperEffectInstance(_effect, this);
			}

			protected override void Triggered(AnimatorEvent _event)
			{
				s_WrappedEffectInstances.ClearIfNotEmpty();
				m_Wrapper.ApplyEffects(s_WrappedEffectInstances);
				m_EffectInstance.WrappedEffectInstances.AddRange(s_WrappedEffectInstances);
			}
		}

		protected readonly EffectsWrapper m_Wrapper;

		public AnimatorListeningEffectsWrapper(IRule _owner, ETargetSource _targetSource, BBList<IEffect> _effects, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash)
			: base(EEffectType.WrappedAnimListening, _owner, _targetSource, _eventType, _layer, _animHash)
		{
			m_Wrapper = CreateEffectsWrapper(_effects, RulesHelper.CategoryMatchingEffectPredicate);
		}

		public AnimatorListeningEffectsWrapper(IRule _owner, ETargetSource _targetSource, BBList<IEffect> _effects, Predicate<IEffect> _effectsPredicate, AnimatorEventType _eventType)
			: this(_owner, _targetSource, _effects, _eventType, AnimationTaskManager.ELayer.Base, (AnimatorStateMachineStatic.HashValue)null)
		{
		}

		protected override EventTypeListener CreateListener(T _entity, IBuffedStatsCollectionHandle _stats, IGameEntity _targetEntity)
		{
			EffectsWrapper effectsWrapper = m_Wrapper.Clone().Cast<EffectsWrapper>();
			effectsWrapper.Entity = _entity;
			effectsWrapper.StatValueCollection = _stats.StatValueCollection;
			return new WrapperEventTypeListener(_targetEntity, m_EventType, m_Layer, m_AnimHash, effectsWrapper, this);
		}

		protected override IEffectInstance CreateEffectInstance(EventTypeListener _listener)
		{
			return ((WrapperEventTypeListener)_listener).EffectInstance;
		}

		protected virtual EffectsWrapper CreateEffectsWrapper(BBList<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _predicate)
		{
			return new EffectsWrapper(_effects, _predicate);
		}
	}
}
