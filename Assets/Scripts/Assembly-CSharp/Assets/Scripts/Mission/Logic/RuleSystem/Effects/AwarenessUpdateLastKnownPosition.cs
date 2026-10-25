using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.RuleSystem.Effects
{
	public class AwarenessUpdateLastKnownPosition<T, TPayload> : SkillEffectBase<T, TPayload> where T : ISkillsCarrier where TPayload : ISkillPayload
	{
		public AwarenessUpdateLastKnownPosition(IRule _owner, ETargetSource _targetSource)
			: base(EEffectType.UpdateLastKnownPosition, _owner, _targetSource)
		{
		}

		protected override IEffectInstance Apply(T _entity, IBuffedStatsCollectionHandle _stats)
		{
			IEffectInstance result = base.Apply(_entity, _stats);
			NpcCharacter npcCharacter = TargetEntityId.Entity as NpcCharacter;
			if (npcCharacter != null)
			{
				ICharacterAwarenessCarrier characterAwarenessCarrier = npcCharacter.StateMachines as ICharacterAwarenessCarrier;
				if (characterAwarenessCarrier != null)
				{
					CharacterAwareness.AlarmedState.AlarmedPayload payload = characterAwarenessCarrier.CharacterAwareness.GetPayload<CharacterAwareness.AlarmedState.AlarmedPayload>(EStateType.CharacterAwareness_Alarmed);
					IPositionProvider target = _entity as IPositionProvider;
					payload.UpdateLastKnownPosition(npcCharacter, target);
				}
			}
			return result;
		}
	}
}
