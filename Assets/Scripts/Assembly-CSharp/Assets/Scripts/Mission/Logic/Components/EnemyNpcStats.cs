using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class EnemyNpcStats : NpcStats, ICharacterConditionStats, ICharacterConflictStatsCarrier, ICharacterDetectionStats, IEnemyStats, IReadonlyStatValueCollection, IStatValueCollection
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(NpcStats.StatsHelpers).Concat(new IStatsHelper[4]
		{
			StatsHelper<EnemyStatsHelper>.Instance,
			StatsHelper<CharacterDetectionStatsHelper>.Instance,
			StatsHelper<CharacterConflictStatsHelper>.Instance,
			StatsHelper<CharacterConditionStatsHelper>.Instance
		}).ToArray();

		private float m_IncrementFactor;

		public override float IncrementFactor
		{
			get
			{
				return m_IncrementFactor;
			}
		}

		public override float FrameUpdateIntervalSeconds
		{
			get
			{
				return (!ForcedUpdate) ? 0.4f : 0f;
			}
		}

		public override int TurnUpdateIntervalTurns
		{
			get
			{
				return ForcedUpdate ? 1 : 5;
			}
		}

		public bool ForcedUpdate
		{
			get
			{
				EnemyCharacter enemyCharacter = (EnemyCharacter)Entity;
				if (enemyCharacter.GetIsInCombat() || enemyCharacter.GetIsChasing())
				{
					return true;
				}
				if (enemyCharacter.Buffs.HasBuffs(StatTypes.EStatType.Character_Conflict_BleedingDamage) || enemyCharacter.Buffs.HasBuffs(StatTypes.EStatType.Character_Conflict_BleedingDamageRelative))
				{
					return true;
				}
				return false;
			}
		}

		public EnemyNpcStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}

		public void SetIncrementFactor(float _incrementFactor)
		{
			m_IncrementFactor = _incrementFactor;
		}

		public override void Attach(EntityWrapperComponent _wrapperComponent)
		{
			base.Attach(_wrapperComponent);
			((EnemyCharacter)Entity).RefreshDisguise(false);
		}
	}
}
