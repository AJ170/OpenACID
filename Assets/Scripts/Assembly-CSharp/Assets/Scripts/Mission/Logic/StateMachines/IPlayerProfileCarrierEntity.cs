using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface IPlayerProfileCarrierEntity : IDefinitionCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		IPlayerProfileCarrier PlayerProfileCarrier { get; }
	}
}
