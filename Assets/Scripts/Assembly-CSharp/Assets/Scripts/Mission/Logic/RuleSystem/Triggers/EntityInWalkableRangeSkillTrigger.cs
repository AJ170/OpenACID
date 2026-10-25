using System;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using SharedBaseLib.System;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EntityInWalkableRangeSkillTrigger<T, TPayload> : EntityInMaxRangeSkillTrigger<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private static float s_MaxRangeSqrt = (float)Math.Sqrt(3.4028234663852886E+38);

		public EntityInWalkableRangeSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource)
			: base(ETriggerType.EntityInWalkableRange, _owner, _frequency, _categories, _targetSource, NavMeshLayers.WalkableLayerMaskPreset, false, (EntityInRange.RangeGetter<T>)MaxRangeGetter)
		{
		}

		private static float MaxRangeGetter(T _entity, EntityId _targetEntityId, IBuffedStatsCollectionHandle _stats)
		{
			return s_MaxRangeSqrt;
		}
	}
}
