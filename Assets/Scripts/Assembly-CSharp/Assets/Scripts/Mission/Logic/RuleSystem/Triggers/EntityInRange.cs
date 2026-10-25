using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public static class EntityInRange
	{
		public delegate float RangeGetter<T>(T _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats) where T : ISkillsCarrier, IGameEntity;

		public static RangeGetter<T> GetRangeFromStats<T>(StatTypes.EStatType _statType) where T : ISkillsCarrier, IGameEntity
		{
			return (T _entity, EntityId _targetId, IBuffedStatsCollectionHandle _stats) => _stats.Get_float(_statType);
		}
	}
}
