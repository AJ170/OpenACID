using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IAssassinCharacterStateMachinesCarrier : IDefinitionCarrier, ICharacterStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity, IPlayerMainCarrierEntity, IPlayerProfileCarrierEntity
	{
		AssassinCharacterStateMachines AssassinCharacterStateMachines { get; }
	}
}
