using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Stats
{
	public class NpcStatsHelper : StatsHelper<NpcStatsHelper>
	{
		private const int InvalidAnimatorNpcId = -1;

		public NpcStatsHelper()
			: base(new StatTypes.StatsRange(StatTypes.EStatType.__Start_Immutable_Npc, StatTypes.EStatType.__End_Immutable_Npc), new StatTypes.StatsRange(StatTypes.EStatType.__Start_Mutable_Npc, StatTypes.EStatType.__End_Mutable_Npc), new StatTypes.StatsRange())
		{
		}

		public override void InitializeMutable(IStatValueCollection _stats, IInstanceCarrier _instanceCarrier)
		{
			base.InitializeMutable(_stats, _instanceCarrier);
			if (_instanceCarrier.GameEntity is NpcCharacter)
			{
				if (_instanceCarrier.GameEntity.GameObjectParent != null)
				{
					MissionNpcFormation component = _instanceCarrier.GameEntity.GameObjectParent.GetComponent<MissionNpcFormation>();
					EntityId id = component.EntityID.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>().Id;
					_stats.Set_EntityId(StatTypes.EStatType.Npc_PatrolId, id);
				}
				_stats.Set_int(StatTypes.EStatType.Npc_AnimatorNpcId, -1);
				_stats.Set_bool(StatTypes.EStatType.Npc_DidSeeBombExplosion, false);
				_stats.Set_Vector3(StatTypes.EStatType.Npc_FleeSourcePosition, new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
			}
		}
	}
}
