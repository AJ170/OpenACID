using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SuperBuffEffect<T> : TargetEffectBase<T> where T : IRulesCarrier
	{
		public class EffectInstance : EffectInstanceBase
		{
			private readonly ISuperBuffsStatsCarrier m_TargetEntity;

			private readonly IBuff m_Buff;

			private readonly int m_RuleDefinitionId;

			private readonly Predicate<ISuperBuffsStatsCarrier> m_ActivityPredicate;

			public EffectInstance(IEffect _effect, ISuperBuffsStatsCarrier _targetEntity, IBuff _buff, int _ruleDefinitionId, Predicate<ISuperBuffsStatsCarrier> _activityPredicate)
				: base(_effect)
			{
				m_TargetEntity = _targetEntity;
				m_Buff = _buff;
				m_RuleDefinitionId = _ruleDefinitionId;
				m_ActivityPredicate = _activityPredicate;
			}

			public override bool Update(IRulesCarrier _entity)
			{
				if (m_Buff.IsRunning && m_ActivityPredicate != null && !m_ActivityPredicate(m_TargetEntity))
				{
					Abort(_entity);
				}
				return m_Buff.IsRunning;
			}

			public override void Abort(IRulesCarrier _entity)
			{
				m_TargetEntity.SuperBuffStats.RemoveSuperBuff(m_Buff, m_RuleDefinitionId);
			}
		}

		private readonly IBuffGenerator m_BuffGenerator;

		private readonly int m_RuleDefinitionId;

		private readonly Predicate<ISuperBuffsStatsCarrier> m_ActivityPredicate;

		private static readonly BBList<IBuff> s_BuffList = new BBList<IBuff>();

		public ISuperBuffsStatsCarrier TargetEntity
		{
			get
			{
				return TargetEntityId.GetEntity<ISuperBuffsStatsCarrier>();
			}
		}

		public SuperBuffEffect(IRule _owner, ETargetSource _targetSource, IBuffGenerator _buffGenerator, int _ruleDefinitionId)
			: this(_owner, _targetSource, _buffGenerator, _ruleDefinitionId, (Predicate<ISuperBuffsStatsCarrier>)null)
		{
		}

		public SuperBuffEffect(IRule _owner, ETargetSource _targetSource, IBuffGenerator _buffGenerator, int _ruleDefinitionId, Predicate<ISuperBuffsStatsCarrier> _activityPredicate)
			: base(EEffectType.SuperBuff, _owner, _targetSource)
		{
			m_BuffGenerator = _buffGenerator;
			m_RuleDefinitionId = _ruleDefinitionId;
			m_ActivityPredicate = _activityPredicate;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			return Apply(_entity, this, TargetEntity, m_BuffGenerator, m_RuleDefinitionId, m_ActivityPredicate);
		}

		public static IEffectInstance Apply(IRulesCarrier _rulesCarrier, IEffect _effect, ISuperBuffsStatsCarrier _targetEntity, IBuffGenerator _buffGenerator, int _ruleDefinitionId, Predicate<ISuperBuffsStatsCarrier> _activityPredicate)
		{
			WrapperEffectInstance wrapperEffectInstance = new WrapperEffectInstance(_effect);
			_buffGenerator.MakeBuffs(s_BuffList, _rulesCarrier, _targetEntity);
			for (int i = 0; i < s_BuffList.Count; i++)
			{
				_targetEntity.SuperBuffStats.AddSuperBuff(s_BuffList[i], _ruleDefinitionId);
				wrapperEffectInstance.WrappedEffectInstances.Add(new EffectInstance(_effect, _targetEntity, s_BuffList[i], _ruleDefinitionId, _activityPredicate));
			}
			return wrapperEffectInstance;
		}
	}
}
