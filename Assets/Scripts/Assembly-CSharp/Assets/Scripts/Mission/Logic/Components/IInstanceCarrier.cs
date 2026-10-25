using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public interface IInstanceCarrier
	{
		IEntity Entity { get; }

		IGameEntity GameEntity { get; }

		GameObject GameObject { get; }
	}
}
