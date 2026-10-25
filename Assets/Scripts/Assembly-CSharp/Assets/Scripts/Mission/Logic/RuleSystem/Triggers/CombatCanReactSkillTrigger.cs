using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class CombatCanReactSkillTrigger<T, TPayload> : SkillTriggerBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		private readonly CharacterCondition.EActionAllowed m_action;

		public CombatCanReactSkillTrigger(IRule _owner, ETargetSource _targetSource, CharacterCondition.EActionAllowed _action)
			: base(ETriggerType.CombatCanReact, _owner, _targetSource)
		{
			base.Frequency = ETriggerFrequency.Framewise;
			Categories = ETriggerCategoryFlags.InCombat;
			m_action = _action;
		}

		protected override bool IsActive(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_entity, _stats))
			{
				return false;
			}
			IParticipantCarrier participantCarrier = TargetEntityId.TryGetEntity<IParticipantCarrier>();
			return participantCarrier != null && participantCarrier.CombatParticipantProxy.CanCombatSkillReact(m_action);
		}
	}
}
