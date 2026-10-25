using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EnemySelectedTrigger : CategoryTriggerBase<PlayerCharacter>
	{
		public EnemySelectedTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags)
			: base(ETriggerType.EnemySelected, _owner, _frequency, _categoryFlags)
		{
		}

		public override ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity)
		{
			return ESkillAvailabilityFlags.NoTargetSelected;
		}

		protected override bool IsActive(PlayerCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && LogicMain.LogicInterface.SelectedEntityId.TryGetEntity<EnemyCharacter>() != null;
		}
	}
}
