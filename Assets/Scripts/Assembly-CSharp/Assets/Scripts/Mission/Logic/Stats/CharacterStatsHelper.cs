using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CharacterStatsHelper : StatsHelper<CharacterStatsHelper>
	{
		public CharacterStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Character, StatTypes.EStatType.__End_Immutable_Character), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Character, StatTypes.EStatType.__End_Mutable_Character), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_Character, StatTypes.EStatType.__End_Computed_Character))
		{
		}

		public override void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			base.InitializeMutable(_stats, _instanceCarrier);
			_stats.Set_int(StatTypes.EStatType.Character_BirthTurn, LogicMain.CurrentTurn + 1);
			if (_stats.Get_int(StatTypes.EStatType.Entity_DisplayNameLocaKey) == 0)
			{
				_stats.Set_int(StatTypes.EStatType.Entity_DisplayNameLocaKey, _stats.GetLocalizedNameLocaId(CharacterHelper.ECharacterNameType.Plain));
			}
			if (_stats.Get_int(StatTypes.EStatType.Character_PortraitDisplayNameLocaKey) == 0)
			{
				_stats.Set_int(StatTypes.EStatType.Character_PortraitDisplayNameLocaKey, _stats.GetLocalizedNameLocaId(CharacterHelper.ECharacterNameType.PortraitWithLevel));
			}
		}

		public override void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
		}
	}
}
