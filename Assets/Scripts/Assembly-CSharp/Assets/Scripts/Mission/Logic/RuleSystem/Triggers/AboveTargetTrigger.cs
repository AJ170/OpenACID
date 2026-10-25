using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AboveTargetTrigger : CategorySkillTriggerBase<AssassinCharacter, IEntityIdSkillPayload>
	{
		private readonly float m_MinHeight;

		public AboveTargetTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _categories, ETargetSource _targetSource, float _minHeight)
			: base(ETriggerType.AboveTarget, _owner, _frequency, _categories, _targetSource)
		{
			m_MinHeight = _minHeight;
		}

		protected override bool IsActive(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			return _entity.Position.y - TargetEntityId.GameObjectEntity.Position.y >= m_MinHeight;
		}
	}
}
