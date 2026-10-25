using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class DelaySkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier, ISuperBuffsStatsCarrier where TPayload : ISkillPayload
	{
		protected class DelayEffectInstance : WrapperEffectInstance, IEffectInstaceResult, ISkillEffectInstanceResult
		{
			private readonly DelayCondition m_condition;

			private readonly SkillEffectsWrapper m_wrapper;

			private readonly EntityId m_targetEntityId;

			private bool m_active;

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

			public DelayEffectInstance(IEffect _effect, EntityId _targetEntityId, DelayCondition _condition, SkillEffectsWrapper _wrapper, bool _allowsSkipCooldown)
				: base(_effect)
			{
				m_targetEntityId = _targetEntityId;
				m_condition = _condition;
				m_wrapper = _wrapper;
				m_active = false;
				m_AllowsSkipCooldown = _allowsSkipCooldown;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				T val = m_wrapper.Entity.Cast<T>();
				if (!m_active)
				{
					using (IBuffedStatsCollectionHandle stats = m_wrapper.StatValueCollection.Apply(val))
					{
						if (m_condition(Effect, val, stats, m_targetEntityId, (TPayload)m_wrapper.Payload))
						{
							m_wrapper.ApplyEffects(base.WrappedEffectInstances);
							m_active = true;
							return false;
						}
					}
				}
				return !m_active || base.Update(_entity);
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_AllowsSkipCooldown &= !m_active;
				base.Abort(_entity);
			}
		}

		public delegate bool DelayCondition(IEffect _effect, T _entity, IBuffedStatsCollectionHandle _stats, EntityId _targetEntityId, TPayload _payload);

		private readonly DelayCondition m_condition;

		private readonly SkillEffectsWrapper m_wrapper;

		private readonly bool m_AllowsSkipCooldown;

		public DelaySkillEffect(IRule _owner, ETargetSource _targetSource, DelayCondition _condition, bool _allowsSkipCooldown, BBList<IEffect> _effects)
			: base(EEffectType.WrappedDelayEffects, _owner, _targetSource)
		{
			m_condition = _condition;
			m_wrapper = new SkillEffectsWrapper(_effects);
			m_AllowsSkipCooldown = _allowsSkipCooldown;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			SkillEffectsWrapper skillEffectsWrapper = m_wrapper.Clone().Cast<SkillEffectsWrapper>();
			skillEffectsWrapper.Entity = _entity;
			skillEffectsWrapper.Payload = Payload;
			skillEffectsWrapper.StatValueCollection = _stats.StatValueCollection;
			return new DelayEffectInstance(this, TargetEntityId, m_condition, skillEffectsWrapper, m_AllowsSkipCooldown);
		}
	}
}
