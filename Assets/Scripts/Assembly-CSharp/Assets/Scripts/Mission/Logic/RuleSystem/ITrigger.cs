using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface ITrigger
	{
		ETriggerFrequency Frequency { get; }

		ETriggerCategoryFlags Categories { get; }

		ETriggerSign Sign { get; }

		ETriggerType Type { get; }

		bool IsFrequencyMatching { get; }

		bool TriggerCondition { get; }

		bool IsMandatory { get; }

		IRule Owner { get; }

		Predicate<ITrigger, IRulesCarrier, IBuffedStatsCollectionHandle> Predicate { get; }

		bool GetIsCategoryMatching(IRulesCarrier _entity);

		ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity);

		bool IsActive(IRulesCarrier _entity, IBuffedStatsCollectionHandle _stats);

		void Reset();
	}
}
