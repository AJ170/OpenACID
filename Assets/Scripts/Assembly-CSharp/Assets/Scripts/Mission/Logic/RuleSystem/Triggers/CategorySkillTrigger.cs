using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public sealed class CategorySkillTrigger<T> : CategorySkillTriggerBase<T, ISkillPayload> where T : ISkillsCarrier
	{
		public CategorySkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
			: base(ETriggerType.Category, _owner, _frequency, _categoryFlags, ETargetSource.Custom)
		{
		}

		public override bool GetIsCategoryMatching(IRulesCarrier _entity)
		{
			return true;
		}

		public override ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity)
		{
			if (Categories.Contains(ETriggerCategoryFlags.InCombat) && !Categories.Intersects(ETriggerCategoryFlags.Roaming | ETriggerCategoryFlags.HidingInObject | ETriggerCategoryFlags.HidingInPatrol | ETriggerCategoryFlags.BeingChased | ETriggerCategoryFlags.Parcour | ETriggerCategoryFlags.Other) && !_entity.Rules.CurrentTriggerCategoryFlags.Contains(ETriggerCategoryFlags.InCombat))
			{
				return ESkillAvailabilityFlags.NotInCombat;
			}
			if (!Categories.Contains(ETriggerCategoryFlags.InCombat) && _entity.Rules.CurrentTriggerCategoryFlags.Contains(ETriggerCategoryFlags.InCombat))
			{
				return ESkillAvailabilityFlags.NotOutOfCombat;
			}
			return base.GetAvailabilityFlags(_entity);
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && base.GetIsCategoryMatching(_entity);
		}
	}
}
