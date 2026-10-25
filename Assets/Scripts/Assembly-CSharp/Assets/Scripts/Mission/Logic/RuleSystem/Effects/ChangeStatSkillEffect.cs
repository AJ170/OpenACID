using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ChangeStatSkillEffect<T, TPayload, TValueWrapperType> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload where TValueWrapperType : class, IStatValueWrapper, new()
	{
		public class EffectInstance : EffectInstanceBase
		{
			private bool m_Aborted;

			private readonly EntityId m_TargetId;

			private readonly StatTypes.EStatType m_StatType;

			private readonly Func<IReadonlyStatValue, TValueWrapperType> m_ValueAbortModifier;

			private readonly Predicate<IEntity> m_activityPredicate;

			public EffectInstance(IEffect _effect, EntityId _entityId, StatTypes.EStatType _statType, Func<IReadonlyStatValue, TValueWrapperType> _abortModifier, Predicate<IEntity> _activityPredicate)
				: base(_effect)
			{
				m_TargetId = _entityId;
				m_StatType = _statType;
				m_ValueAbortModifier = _abortModifier;
				m_activityPredicate = _activityPredicate;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (m_activityPredicate == null || !m_activityPredicate(m_TargetId.Entity))
				{
					Abort(_entity);
					return false;
				}
				return true;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				if (!m_Aborted)
				{
					EntityStats entityStats = m_TargetId.GetEntity<IEntity>().EntityStats;
					IStatValue statValue = entityStats.GetStatValue(m_StatType);
					TValueWrapperType wrapper = m_ValueAbortModifier(statValue);
					statValue.CopyFrom(wrapper);
					m_Aborted = true;
				}
			}
		}

		private readonly StatTypes.EStatType m_StatType;

		private readonly Func<IReadonlyStatValue, TValueWrapperType> m_ValueModifier;

		private readonly Func<IReadonlyStatValue, TValueWrapperType> m_ValueAbortModifier;

		private readonly Predicate<IEntity> m_activityPredicate;

		public ChangeStatSkillEffect(IRule _owner, ETargetSource _targetSource, StatTypes.EStatType _statType, Func<IReadonlyStatValue, TValueWrapperType> _valueModifier, Func<IReadonlyStatValue, TValueWrapperType> _abortModifier, Predicate<IEntity> _activityPredicate)
			: base(EEffectType.ChangeTargetStats, _owner, _targetSource)
		{
			m_StatType = _statType;
			m_ValueModifier = _valueModifier;
			m_ValueAbortModifier = _abortModifier;
			m_activityPredicate = _activityPredicate;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			EntityStats entityStats = TargetEntityId.GetEntity<IEntity>().EntityStats;
			IStatValue statValue = entityStats.GetStatValue(m_StatType);
			TValueWrapperType wrapper = m_ValueModifier(statValue);
			statValue.CopyFrom(wrapper);
			return new EffectInstance(this, TargetEntityId, m_StatType, m_ValueAbortModifier, m_activityPredicate);
		}
	}
}
