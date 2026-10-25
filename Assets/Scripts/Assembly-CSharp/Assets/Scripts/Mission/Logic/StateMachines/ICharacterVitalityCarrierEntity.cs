using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public interface ICharacterVitalityCarrierEntity : ICharacterStatsCarrier, IDefinitionCarrier, IEntityStatsCarrier, IHeadIconCarrier, IStateMachinesCarrier, IEntity
	{
		ICharacterVitalityCarrier CharacterVitalityCarrier { get; }

		event CharacterDiedEventHandler CharacterDied;

		void InvokeCharacterDied();
	}
}
