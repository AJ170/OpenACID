using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class PickpocketSkillEffect<T> : SkillEffectBase<T, EntityIdSkillPayload> where T : AssassinCharacter
	{
		private readonly StatTypes.EStatType m_chanceHirelingLevelFactor;

		private readonly StatTypes.EStatType m_factorHirelingLevelFactor;

		public PickpocketSkillEffect(ISkillRule _owner, ETargetSource _targetSource, StatTypes.EStatType _chanceHirelingLevelFactor, StatTypes.EStatType _factorHirelingLevelFactor)
			: base(EEffectType.Pickpocket, (IRule)_owner, _targetSource)
		{
			m_chanceHirelingLevelFactor = _chanceHirelingLevelFactor;
			m_factorHirelingLevelFactor = _factorHirelingLevelFactor;
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			EnemyCharacter enemyCharacter = base.TypedPayload.Id.TryGetEntity<EnemyCharacter>();
			if (enemyCharacter != null)
			{
				enemyCharacter.TryPickpocket(_entity, Owner.Cast<ISkillRule>().SkillType, _stats, m_chanceHirelingLevelFactor, m_factorHirelingLevelFactor);
			}
			return result;
		}
	}
}
