using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EntityInMinRangeSkillTrigger<T, TPayload> : EntityInRangeSkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private readonly EntityInRange.RangeGetter<T> m_MinRangeGetter;

		public EntityInMinRangeSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _layerMask, bool _planeDistance, EntityInRange.RangeGetter<T> _minRangeGetter)
			: this(ETriggerType.EntityInMinRange, _owner, _frequency, _categories, _targetSource, _layerMask, _planeDistance, _minRangeGetter)
		{
		}

		protected EntityInMinRangeSkillTrigger(ETriggerType _triggerType, IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _layerMask, bool _planeDistance, EntityInRange.RangeGetter<T> _minRangeGetter)
			: base(_triggerType, _owner, _frequency, _categories, _targetSource, _layerMask, _planeDistance)
		{
			m_MinRangeGetter = _minRangeGetter;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			float num = m_MinRangeGetter(_entity, TargetEntityId, _stats);
			if (num == float.MaxValue)
			{
				return false;
			}
			float num2 = ComputeSqrDistance(_entity);
			if (num2 == float.MaxValue)
			{
				return true;
			}
			return num2 >= num * num;
		}
	}
}
