using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CharacterConspiciuousStatsHelper : StatsHelper<CharacterConspiciuousStatsHelper>
	{
		public CharacterConspiciuousStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_CharacterConspicuous, StatTypes.EStatType.__End_Mutable_CharacterConspicuous), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_CharacterConspicuous, StatTypes.EStatType.__End_Computed_CharacterConspicuous))
		{
		}

		public override void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
			base.UpdateComputedTurn(_stats, _instanceCarrier, _deltaTurns);
			_stats.Set_float(StatTypes.EStatType.Character_Conspicuous_NotorietyIncreaseDelay, 0f);
		}
	}
}
