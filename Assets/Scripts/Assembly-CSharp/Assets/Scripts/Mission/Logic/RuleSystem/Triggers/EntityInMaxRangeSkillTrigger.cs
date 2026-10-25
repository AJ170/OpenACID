using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EntityInMaxRangeSkillTrigger<T, TPayload> : EntityInRangeSkillTriggerBase<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		private readonly EntityInRange.RangeGetter<T> m_MaxRangeGetter;

		protected EntityInMaxRangeSkillTrigger(ETriggerType _triggerType, IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _layerMask, bool _planeDistance, EntityInRange.RangeGetter<T> _maxRangeGetter)
			: base(_triggerType, _owner, _frequency, _categories, _targetSource, _layerMask, _planeDistance)
		{
			m_MaxRangeGetter = _maxRangeGetter;
		}

		public EntityInMaxRangeSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _layerMask, bool _planeDistance, EntityInRange.RangeGetter<T> _maxRangeGetter)
			: this(ETriggerType.EntityInMaxRange, _owner, _frequency, _categories, _targetSource, _layerMask, _planeDistance, _maxRangeGetter)
		{
		}

		public override ESkillAvailabilityFlags GetAvailabilityFlags(IRulesCarrier _entity)
		{
			return ESkillAvailabilityFlags.TargetNotInRange;
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
			if (num2 == float.MaxValue)
			{
				return true;
			}
			return num <= num2 * num2;
		}
	}
}
