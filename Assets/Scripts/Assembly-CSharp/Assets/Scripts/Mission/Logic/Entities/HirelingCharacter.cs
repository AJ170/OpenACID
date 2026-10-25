using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class HirelingCharacter : AssassinCharacter, IDefinitionCarrier, ICharacterStateMachinesCarrier, IAssassinCharacterStateMachinesCarrier, IHirelingCharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, IPlayerMainCarrierEntity, IPlayerProfileCarrierEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.HirelingCharacter;
			}
		}

		public override AssassinInteraction AssassinInteraction { get; protected set; }

		public HirelingCharacterStateMachines HirelingCharacterStateMachines { get; private set; }

		public HirelingCharacter()
			: base((IEntity entity) => new HirelingCharacterStateMachines(entity))
		{
			HirelingCharacterStateMachines = (HirelingCharacterStateMachines)AssassinCharacterStateMachines;
			AssassinInteraction = AddComponent(new AssassinInteraction(this, HirelingCharacterStateMachines.HirelingMain));
		}
	}
}
