using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class MissionCharacterStatsHelper : StatsHelper<MissionCharacterStatsHelper>
	{
		public MissionCharacterStatsHelper()
			: base(new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_MissionCharacter, StatTypes.EStatType.__End_Mutable_MissionCharacter), new StatTypes.StatsRange())
		{
		}

		public override void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			base.InitializeMutable(_stats, _instanceCarrier);
			_stats.Set_float(StatTypes.EStatType.MissionCharacter_MaxNotoriety, float.MaxValue);
		}
	}
}
