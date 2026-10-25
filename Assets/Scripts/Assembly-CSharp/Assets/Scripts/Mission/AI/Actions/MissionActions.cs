using AcierProtocol;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using SharedBaseLib.System;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class MissionActions : NpcBehaviorComponent
	{
		public MissionActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public BehaviorReturnCode ActionApproachMissionAlertedTarget(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode == BehaviorReturnCode.Running)
			{
				return BehaviorReturnCode.Running;
			}
			return StartApproachMissionAlertedTarget();
		}

		public void AttachApproachMissionAlertedTarget()
		{
			StartApproachMissionAlertedTarget();
		}

		private BehaviorReturnCode StartApproachMissionAlertedTarget()
		{
			InteractionCharacter interactionCharacter = base.NpcCharacter as InteractionCharacter;
			if (interactionCharacter == null)
			{
				return BehaviorReturnCode.Failure;
			}
			IPayload currentStatePayload = interactionCharacter.InteractionCharacterStateMachines.InteractionCharacterMain.CurrentStatePayload;
			InteractionCharacterMain.FleeState.FleeTargetPayload fleeTargetPayload = currentStatePayload as InteractionCharacterMain.FleeState.FleeTargetPayload;
			if (fleeTargetPayload == null)
			{
				return BehaviorReturnCode.Failure;
			}
			Vector3 fleeTarget = fleeTargetPayload.FleeTarget;
			NavMeshHit _hit;
			if (NavigationHelper.CascadingSamplePosition(fleeTarget, out _hit, NavMeshLayers.WalkableLayerMaskPreset))
			{
				base.NpcRuntimeData.Mission.TargetApproacher.SetData(base.NpcCharacter.Id, _hit.position, EMovementSpeed.Sprint, 2f);
				return base.NpcRuntimeData.Mission.TargetApproacher.ApproachCurrentTargetAi(fleeTarget);
			}
			return BehaviorReturnCode.Failure;
		}
	}
}
