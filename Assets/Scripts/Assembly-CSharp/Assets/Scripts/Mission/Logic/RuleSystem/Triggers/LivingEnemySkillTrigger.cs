using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class LivingEnemySkillTrigger<TEntity, TPayload> : CategorySkillTriggerBase<TEntity, TPayload> where TEntity : ISkillsCarrier where TPayload : ISkillPayload
	{
		public bool DemandAttached { get; set; }

		public LivingEnemySkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categoryFlags, ETargetSource _targetSource)
			: base(ETriggerType.LivingEnemy, _owner, _frequency, _categoryFlags, _targetSource)
		{
			DemandAttached = true;
		}

		public override ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity)
		{
			return ESkillAvailabilityFlags.NoTargetSelected;
		}

		protected override bool IsActive(TEntity _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			EnemyCharacter enemyCharacter = TargetEntityId.TryGetEntity<EnemyCharacter>();
			return enemyCharacter != null && !enemyCharacter.GetIsDead() && (!DemandAttached || enemyCharacter.IsAttached);
		}
	}
}
