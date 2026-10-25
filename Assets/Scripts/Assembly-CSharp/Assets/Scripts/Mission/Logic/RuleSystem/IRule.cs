using System.Collections.ObjectModel;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.Core;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IRule
	{
		Enum<RuleTypes> Type { get; }

		RuleData.IRuleDefinition Definition { get; }

		BuffedReadonlyStatValueCollection StatValueCollection { get; }

		ReadOnlyCollection<ITrigger> Triggers { get; }

		ReadOnlyCollection<ITrigger> MandatoryTriggers { get; }

		ReadOnlyCollection<IEffect> Effects { get; }

		bool GetAreCategoryFlagsMatching(IRulesCarrier _entity);

		bool IsFulfilled(IRulesCarrier _entity, Predicate<ITrigger, IRulesCarrier> _triggerCondition, out ESkillAvailabilityFlags _availabilityFlags);

		bool MeetsActivityConditions(IRulesCarrier _entity, out ESkillAvailabilityFlags _availabilityFlags);

		void Apply(IRulesCarrier _entity, BBList<IEffect> _cumulatedEffects);
	}
}
