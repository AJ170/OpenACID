using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class AwareInteractionCharacter : InteractionCharacter, IAwareInteractionCharacterStatsCarrier, ICharacterStatsCarrier, IDefinitionCarrier, ICharacterStateMachinesCarrier, IInteractionCharacterStateMachinesCarrier, IAwareInteractionCharacterStateMachinesCarrier, IEntityStatsCarrier, IHeadIconCarrier, IInteractionCharacterStatsCarrier, IStateMachinesCarrier, IEntity, ICharacterVitalityCarrierEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.AwareInteractionCharacter;
			}
		}

		public AwareInteractionCharacterStats AwareInteractionCharacterStats
		{
			get
			{
				return (AwareInteractionCharacterStats)m_charStats;
			}
		}

		public AwareInteractionCharacterStateMachines AwareInteractionCharacterStateMachines
		{
			get
			{
				return (AwareInteractionCharacterStateMachines)CharacterStateMachines;
			}
		}

		public AwareInteractionCharacter()
			: base((IEntity entity) => new AwareInteractionCharacterStateMachines(entity), (IEntity entity) => new AwareInteractionCharacterStats(entity))
		{
		}
	}
}
