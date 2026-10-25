using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class CharacterDetectionStatsHelper : StatsHelper<CharacterDetectionStatsHelper>
	{
		public CharacterDetectionStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_CharacterDetection, StatTypes.EStatType.__End_Immutable_CharacterDetection), new StatTypes.StatsRange(), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Computed_CharacterDetection, StatTypes.EStatType.__End_Computed_CharacterDetection))
		{
		}

		public override void UpdateComputedTurn(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier, int _deltaTurns)
		{
			if (_instanceCarrier != null)
			{
				NpcCharacter entity = (NpcCharacter)_instanceCarrier.GameEntity;
				CharacterDetectionStatsComputationHelper.UpdateDetectionStats(entity, entity.GetTargetAsPlayer());
			}
		}
	}
}
