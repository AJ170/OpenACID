using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.Core;
using SharedBaseLib.System.Extensions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class PlayerStats : CharacterStats, IInstanceCarrier, IInstanceStatValueCollection, IEntityStats, ICharacterConditionStats, ICharacterConflictStatsCarrier, ICharacterConspiciuousStats, IReadonlyStatValueCollection, IStatValueCollection, ISuperBuffStats
	{
		public new static readonly IStatsHelper[] StatsHelpers = new BBList<IStatsHelper>(CharacterStats.StatsHelpers).Concat(new IStatsHelper[4]
		{
			StatsHelper<PlayerStatsHelper>.Instance,
			StatsHelper<CharacterConspiciuousStatsHelper>.Instance,
			StatsHelper<CharacterConflictStatsHelper>.Instance,
			StatsHelper<CharacterConditionStatsHelper>.Instance
		}).ToArray();

		private readonly SuperBuffs m_SuperBuffs = new SuperBuffs();

		public PlayerStats(IEntity _entity)
			: base(_entity, StatsHelpers)
		{
		}

		public EHidingSpotFlags GetBuffed_EHidingSpotFlags(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			return (EHidingSpotFlags)m_SuperBuffs.GetBuffed_int(_statType, _statValueCollection, _definitionId);
		}

		public int GetBuffed_int(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			return m_SuperBuffs.GetBuffed_int(_statType, _statValueCollection, _definitionId);
		}

		public float GetBuffed_float(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			return m_SuperBuffs.GetBuffed_float(_statType, _statValueCollection, _definitionId);
		}

		public bool GetBuffed_bool(StatTypes.EStatType _statType, IReadonlyStatValueCollection _statValueCollection, int _definitionId = -1)
		{
			return m_SuperBuffs.GetBuffed_bool(_statType, _statValueCollection, _definitionId);
		}

		public void AddSuperBuff(IBuff _buff, int _definitionId = -1)
		{
			m_SuperBuffs.AddBuff(_buff, _definitionId);
		}

		public bool RemoveSuperBuff(IBuff _buff, int _definitionId = -1)
		{
			return m_SuperBuffs.RemoveBuff(_buff, _definitionId);
		}

		public override void DoTurnUpdate(int _deltaTurns)
		{
			m_SuperBuffs.DoTurnUpdate(_deltaTurns);
			base.DoTurnUpdate(_deltaTurns);
		}
	}
}
