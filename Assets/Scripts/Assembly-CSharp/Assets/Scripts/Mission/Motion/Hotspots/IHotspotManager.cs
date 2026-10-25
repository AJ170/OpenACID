using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public interface IHotspotManager
	{
		bool Interact(AssassinCharacter _assassin, GameObject _gameObject, NavMeshAgent _agent);

		bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination);

		void Initialize();
	}
}
