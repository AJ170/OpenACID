using AcierProtocol;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class AssassinChosenSpeedSkillTrigger : CategorySkillTriggerBase<AssassinCharacter, ISkillPayload>
	{
		private readonly EMovementSpeed m_speed;

		public AssassinChosenSpeedSkillTrigger(IRule _owner, ETriggerFrequency _frequency, ETriggerCategoryFlags _flags, ETargetSource _target, EMovementSpeed _speed)
			: base(ETriggerType.AssassinChosenSpeed, _owner, _frequency, _flags, _target)
		{
			m_speed = _speed;
		}

		protected override bool IsActive(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			return base.IsActive(_entity, _stats) && m_speed == TargetEntityId.GetEntity<AssassinCharacter>().AssassinCharacterStateMachines.PlayerProfile.ChosenSpeed;
		}
	}
}
