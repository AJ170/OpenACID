using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class EntityInMinRangeApproachingSkillTrigger<T, TPayload> : EntityInMinRangeSkillTrigger<T, TPayload> where T : ISkillsCarrier, IGameEntity where TPayload : ISkillPayload
	{
		public EntityInMinRangeApproachingSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, int _layerMask, bool _planeDistance, EntityInRange.RangeGetter<T> _minRangeGetter)
			: base(ETriggerType.EntityInMinRangeApproaching, _owner, _frequency, _categories, _targetSource, _layerMask, _planeDistance, _minRangeGetter)
		{
		}
	}
}
