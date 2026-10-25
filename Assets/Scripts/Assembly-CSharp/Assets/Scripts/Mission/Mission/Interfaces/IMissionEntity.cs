using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

namespace Assets.Scripts.Mission.Mission.Interfaces
{
	public interface IMissionEntity : IMissionBase
	{
		EntityId EntityID { get; }

		GameEntity GameEntity { get; }

		Vector3? LastKnownPosition { get; }

		float ReleaseTime { get; }

		void SetLastKnownPosition();

		void PlayInteractionSound();

		void PlaySuccessfullInteractionSound();
	}
}
