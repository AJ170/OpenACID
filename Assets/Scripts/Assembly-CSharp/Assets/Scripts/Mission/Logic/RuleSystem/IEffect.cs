using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public interface IEffect
	{
		ETriggerCategoryFlags Categories { get; }

		EEffectType Type { get; }

		IRule Owner { get; }

		Predicate<IEffect, IRulesCarrier> Predicate { get; }

		bool GetIsCategoryMatching(IRulesCarrier _entity);

		void Prepare(IRulesCarrier _entity);

		IEffectInstance Apply(IRulesCarrier _entity, IBuffedStatsCollectionHandle _stats);

		void Reset();
	}
}
