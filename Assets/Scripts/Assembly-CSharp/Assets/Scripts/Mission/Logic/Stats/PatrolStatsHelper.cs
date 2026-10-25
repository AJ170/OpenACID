using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class PatrolStatsHelper : StatsHelper<PatrolStatsHelper>
	{
		public PatrolStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Patrol, StatTypes.EStatType.__End_Immutable_Patrol), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Patrol, StatTypes.EStatType.__End_Mutable_Patrol), new StatTypes.StatsRange())
		{
		}

		public override void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			base.InitializeMutable(_stats, _instanceCarrier);
			if (_instanceCarrier != null)
			{
				EPatrolType type = _instanceCarrier.Entity.Cast<Assets.Scripts.Mission.Logic.Entities.Patrol>().PatrolDefinition.Definition.Type;
				if (type == EPatrolType.CevilianHidingSpot || type == EPatrolType.CourtesanHidingSpot)
				{
					_stats.Set_bool(StatTypes.EStatType.Patrol_Enabled, true);
				}
			}
		}
	}
}
