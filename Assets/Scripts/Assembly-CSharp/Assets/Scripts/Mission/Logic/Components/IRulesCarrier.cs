using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IRulesCarrier : IDefinitionCarrier, IEntityStatsCarrier, IStateMachinesCarrier, IEntity, IPlayerMainCarrierEntity
	{
		Rules Rules { get; }
	}
}
