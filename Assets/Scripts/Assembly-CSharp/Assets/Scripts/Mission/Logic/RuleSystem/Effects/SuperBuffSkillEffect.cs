using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class SuperBuffSkillEffect<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly IBuffGenerator m_BuffGenerator;

		private readonly int m_RuleDefinitionId;

		private readonly Predicate<ISuperBuffsStatsCarrier> m_ActivityPredicate;

		public ISuperBuffsStatsCarrier TargetEntity
		{
			get
			{
				return TargetEntityId.GetEntity<ISuperBuffsStatsCarrier>();
			}
		}

		public SuperBuffSkillEffect(IRule _owner, ETargetSource _targetSource, IBuffGenerator _buffGenerator, int _ruleDefinitionId)
			: this(_owner, _targetSource, _buffGenerator, _ruleDefinitionId, (Predicate<ISuperBuffsStatsCarrier>)null)
		{
		}

		public SuperBuffSkillEffect(IRule _owner, ETargetSource _targetSource, IBuffGenerator _buffGenerator, int _ruleDefinitionId, Predicate<ISuperBuffsStatsCarrier> _activityPredicate)
			: base(EEffectType.SuperBuff, _owner, _targetSource)
		{
			m_BuffGenerator = _buffGenerator;
			m_RuleDefinitionId = _ruleDefinitionId;
			m_ActivityPredicate = _activityPredicate;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			return SuperBuffEffect<T>.Apply(_entity, this, TargetEntity, m_BuffGenerator, m_RuleDefinitionId, m_ActivityPredicate);
		}
	}
}
