using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Triggers
{
	public class CombatTargetMatchPayloadSkillTrigger<TPayload> : SkillTriggerBase<PlayerCharacter, TPayload> where TPayload : ISkillPayload
	{
		public CombatTargetMatchPayloadSkillTrigger(IRule _owner)
			: base(ETriggerType.CombatCanReact, _owner, ETargetSource.Payload)
		{
			base.Frequency = ETriggerFrequency.Framewise;
			Categories = ETriggerCategoryFlags.InCombat;
		}

		protected override bool IsActive(PlayerCharacter _playerEntity, IBuffedStatsCollectionHandle _stats)
		{
			if (!base.IsActive(_playerEntity, _stats))
			{
				return false;
			}
			IParticipantCarrier participantCarrier = TargetEntityId.TryGetEntity<IParticipantCarrier>();
			if (participantCarrier == null || !participantCarrier.CombatParticipantProxy.IsInCombat || !_playerEntity.CombatParticipantProxy.IsInCombat)
			{
				return false;
			}
			CombatParticipant combatParticipant = participantCarrier.CombatParticipantProxy.CombatParticipant;
			CombatParticipant combatParticipant2 = _playerEntity.CombatParticipantProxy.CombatParticipant;
			return combatParticipant2 != null && combatParticipant != null && combatParticipant2.NextTarget == combatParticipant;
		}
	}
}
