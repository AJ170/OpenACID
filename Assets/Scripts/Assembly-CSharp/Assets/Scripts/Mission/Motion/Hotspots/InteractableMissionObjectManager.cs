using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using UnityEngine;

namespace Assets.Scripts.Mission.Motion.Hotspots
{
	public class InteractableMissionObjectManager : HotspotManager<InteractableMissionObject>
	{
		public InteractableMissionObjectManager()
			: base("MissionObjects")
		{
		}

		protected override void CreateObject(GameObject _go)
		{
		}

		protected override bool Interact(AssassinCharacter _assassin, InteractableMissionObject _entity, NavMeshAgent _agent)
		{
			return PlayerHelper.AssassinInteraction.TapEntity(_entity.Id);
		}

		public override bool LeaveInteraction(AssassinCharacter _assassin, Vector3 _destination)
		{
			if (!base.LeaveInteraction(_assassin, _destination))
			{
				return false;
			}
			return PlayerHelper.AssassinInteraction.CanAbortObjectInteraction() && PlayerHelper.AssassinInteraction.AbortObjectInteraction();
		}
	}
}
