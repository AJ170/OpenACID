using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CharacterConditionStatsHelper : StatsHelper<CharacterConditionStatsHelper>
	{
		public CharacterConditionStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_CharacterCondition, StatTypes.EStatType.__End_Computed_CharacterCondition))
		{
		}

		public override void UpdateComputedExplicit(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsKnockdowned, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsStunned, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsSmokeBombed, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsSanded, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsSeduced, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsGreedy, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsJackBoxed, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsStickyBombPaniced, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsThreateningVictim, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsConfused, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsLuredBySweetMusic, 0);
			_stats.Set_int(StatTypes.EStatType.Character_Condition_IsFleeing, 0);
		}
	}
}
