using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface ISkillsCarrier : IDefinitionCarrier, IEntityStatsCarrier, IRulesCarrier, IStateMachinesCarrier, IEntity, IPlayerMainCarrierEntity, ISuperBuffsStatsCarrier
	{
		Skills Skills { get; }
	}
}
