using AcierProtocol;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class ChooseAssassinSpeedSkillEffect<TPayload> : SkillEffectBase<AssassinCharacter, TPayload> where TPayload : ISkillPayload
	{
		private readonly EMovementSpeed m_Speed;

		public ChooseAssassinSpeedSkillEffect(IRule _owner, ETargetSource _targetSource, EMovementSpeed _speed)
			: base(EEffectType.ChooseAssassinSpeed, _owner, _targetSource)
		{
			m_Speed = _speed;
		}

		protected override IEffectInstance Apply(AssassinCharacter _entity, IBuffedStatsCollectionHandle _stats)
		{
			base.Apply(_entity, _stats);
			TargetEntityId.GetEntity<AssassinCharacter>().AssassinCharacterStateMachines.PlayerProfile.ChosenSpeed = m_Speed;
			return null;
		}
	}
}
