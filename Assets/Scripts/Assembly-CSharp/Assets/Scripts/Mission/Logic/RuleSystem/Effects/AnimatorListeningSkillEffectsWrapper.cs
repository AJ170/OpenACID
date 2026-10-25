using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class AnimatorListeningSkillEffectsWrapper<T, TPayload> : AnimatorListeningEffectsWrapper<T>, ISkillPayloadCarrier, ITargetCarrier, IEffect, ITargetEffect, ISkillEffect where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		protected new class WrapperEffectInstance : AnimatorListeningEffectsWrapper<T>.WrapperEffectInstance, IEffectInstaceResult, ISkillEffectInstanceResult
		{
			private bool m_AllowsSkipCooldown;

			public bool AllowsSkipCooldown
			{
				get
				{
					return m_AllowsSkipCooldown;
				}
			}

			public override IEffectInstaceResult Result
			{
				get
				{
					return this;
				}
			}

			public WrapperEffectInstance(IEffect _effect, EventTypeListener _listener, bool _allowsSkipCooldown)
				: base(_effect, _listener)
			{
				m_AllowsSkipCooldown = _allowsSkipCooldown;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_AllowsSkipCooldown &= m_Listener.IsActive;
				base.Abort(_entity);
			}
		}

		protected new class WrapperEventTypeListener : AnimatorListeningEffectsWrapper<T>.WrapperEventTypeListener
		{
			public WrapperEventTypeListener(IGameEntity _targetEntity, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash, EffectsWrapper _wrapper, AnimatorListeningEffectsWrapper<T> _effect)
				: base(_targetEntity, _eventType, _layer, _animHash, _wrapper, _effect)
			{
			}

			protected override AnimatorListeningEffectsWrapper<T>.WrapperEffectInstance CreateEffectInstance(AnimatorListeningEffectsWrapper<T> _effect)
			{
				AnimatorListeningSkillEffectsWrapper<T, TPayload> animatorListeningSkillEffectsWrapper = (AnimatorListeningSkillEffectsWrapper<T, TPayload>)_effect;
				return new WrapperEffectInstance(_effect, this, animatorListeningSkillEffectsWrapper.AllowsSkipCooldown);
			}
		}

		public ISkillPayload Payload { get; set; }

		public TPayload TypedPayload
		{
			get
			{
				return (TPayload)Payload;
			}
		}

		public bool AllowsSkipCooldown { get; private set; }

		public AnimatorListeningSkillEffectsWrapper(IRule _owner, ETargetSource _targetSource, BBList<IEffect> _effects, bool _allowsSkipCooldown, AnimatorEventType _eventType, AnimationTaskManager.ELayer _layer, AnimatorStateMachineStatic.HashValue _animHash)
			: base(_owner, _targetSource, _effects, _eventType, _layer, _animHash)
		{
			AllowsSkipCooldown = _allowsSkipCooldown;
		}

		public AnimatorListeningSkillEffectsWrapper(IRule _owner, ETargetSource _targetSource, BBList<IEffect> _effects, bool _allowsSkipCooldown, AnimatorEventType _eventType)
			: this(_owner, _targetSource, _effects, _allowsSkipCooldown, _eventType, AnimationTaskManager.ELayer.Base, (AnimatorStateMachineStatic.HashValue)null)
		{
			AllowsSkipCooldown = _allowsSkipCooldown;
		}

		protected override EffectsWrapper CreateEffectsWrapper(BBList<IEffect> _effects, Predicate<IEffect, IRulesCarrier> _predicate)
		{
			return new SkillEffectsWrapper(_effects, _predicate);
		}

		protected override EventTypeListener CreateListener(T _entity, IBuffedStatsCollectionHandle _stats, IGameEntity _targetEntity)
		{
			EffectsWrapper effectsWrapper = m_Wrapper.Clone().Cast<EffectsWrapper>();
			effectsWrapper.Entity = _entity;
			effectsWrapper.StatValueCollection = _stats.StatValueCollection;
			WrapperEventTypeListener wrapperEventTypeListener = new WrapperEventTypeListener(_targetEntity, m_EventType, m_Layer, m_AnimHash, effectsWrapper, this);
			wrapperEventTypeListener.EffectsWrapper.Cast<SkillEffectsWrapper>().Payload = Payload;
			return wrapperEventTypeListener;
		}

		public IEffectInstance Apply(ISkillsCarrier _entity)
		{
			using (IBuffedStatsCollectionHandle stats = Owner.StatValueCollection.Apply(_entity))
			{
				return Apply(_entity, stats);
			}
		}

		public override void Reset()
		{
			base.Reset();
			Payload = null;
		}
	}
}
