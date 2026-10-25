using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class RuleAppliedTrigger<T> : TargetTriggerBase<T> where T : IRulesCarrier
	{
		private readonly Enum<RuleTypes> m_RuleType;

		public RuleAppliedTrigger(IRule _owner, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource, Enum<RuleTypes> _ruleType)
			: base(ETriggerType.RuleApplied, _owner, _targetSource)
		{
			base.Frequency = ETriggerFrequency.Turnwise;
			Categories = _categoryFlags;
			m_RuleType = _ruleType;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && TargetEntityId.GetEntity<IRulesCarrier>().Rules.GetLastApplicationTurn(m_RuleType) == LogicMain.CurrentTurn - 1;
		}
	}
}
