using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IHirelingCharacterStateMachinesCarrier : IDefinitionCarrier, ICharacterStateMachinesCarrier, IAssassinCharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, IPlayerMainCarrierEntity, IPlayerProfileCarrierEntity
	{
		HirelingCharacterStateMachines HirelingCharacterStateMachines { get; }
	}
}
