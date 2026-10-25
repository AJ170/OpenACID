using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public sealed class CategoryTrigger<T> : CategoryTriggerBase<T> where T : IRulesCarrier
	{
		public CategoryTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
			: base(ETriggerType.Category, _owner, _frequency, _categoryFlags)
		{
		}

		public override bool GetIsCategoryMatching(IRulesCarrier _entity)
		{
			return true;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && base.GetIsCategoryMatching(_entity);
		}
	}
}
