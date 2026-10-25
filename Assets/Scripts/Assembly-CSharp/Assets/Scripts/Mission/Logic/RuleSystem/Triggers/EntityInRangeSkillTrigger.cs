using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EntityInRangeSkillTrigger<T, TPayload> : EntityInRangeSkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private readonly EntityInRange.RangeGetter<T> m_MaxRangeGetter;

		private readonly EntityInRange.RangeGetter<T> m_MinRangeGetter;

		public EntityInRangeSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _layerMask, bool _planeDistance, EntityInRange.RangeGetter<T> _maxRangeGetter, EntityInRange.RangeGetter<T> _minRangeGetter)
			: base(ETriggerType.EntityInRange, _owner, _frequency, _categories, _targetSource, _layerMask, _planeDistance)
		{
			m_MaxRangeGetter = _maxRangeGetter;
			m_MinRangeGetter = _minRangeGetter;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			float num = ComputeSqrDistance(_entity);
			if (num == float.MaxValue)
			{
				return false;
			}
			float num2 = m_MaxRangeGetter(_entity, TargetEntityId, _stats);
			if (num2 != float.MaxValue && num > num2 * num2)
			{
				return false;
			}
			float num3 = m_MinRangeGetter(_entity, TargetEntityId, _stats);
			if (num3 == float.MaxValue || num < num3 * num3)
			{
				return false;
			}
			return true;
		}
	}
}
