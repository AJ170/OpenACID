using Assets.Scripts.Mission.Logic;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IMissionSpawnEntity : IMissionBase, IMissionEntity
	{
		EntityId FormerEntityId { get; }
	}
}
