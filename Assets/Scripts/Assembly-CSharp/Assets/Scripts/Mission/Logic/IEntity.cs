using System.Collections.ObjectModel;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic
{
	public interface IEntity : IDefinitionCarrier, IEntityStatsCarrier
	{
		bool IsValid { get; }

		EntityId Id { get; }

		EntityTypes.EType Type { get; }

		ReadOnlyCollection<IComponent> LogicComponents { get; }

		ReadOnlyCollection<IUpdateableComponent> UpdateableLogicComponents { get; }

		bool IsActive { get; set; }

		bool IsReadyForUpdate { get; }

		event Entity.EntityRegistrationDelegate OnEntityRegistered;

		event Entity.EntityRegistrationDelegate OnEntityInitialized;

		event Entity.EntityRegistrationDelegate OnEntityUnregistered;

		T FindComponent<T>() where T : IComponent;

		void SortComponents();

		void OnRegistered();

		void OnUnregistered();
	}
}
