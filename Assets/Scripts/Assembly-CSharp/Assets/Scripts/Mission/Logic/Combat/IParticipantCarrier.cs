using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Combat
{
	public interface IParticipantCarrier : IDefinitionCarrier, IEntityStatsCarrier, IEntity
	{
		CombatParticipantProxy CombatParticipantProxy { get; }
	}
}
