using UnityEngine;

namespace Assets.Scripts.Mission.Logic
{
	public interface IComponent
	{
		IEntity Entity { get; }

		IGameEntity GameEntity { get; }

		GameObject GameObject { get; }

		int Priority { get; }

		void Attach(EntityWrapperComponent _wrapperComponent);

		void Detach();

		void OnRegistered();

		void OnUnregistered();

		void OnActivate();
	}
}
