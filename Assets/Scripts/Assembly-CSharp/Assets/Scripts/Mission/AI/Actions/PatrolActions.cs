using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class PatrolActions : NpcBehaviorComponent
	{
		public class RuntimeData : BaseRuntimeData
		{
			private bool m_demandAttached;

			public RuntimeData(NpcRuntimeData _npcRuntimeData)
				: base(_npcRuntimeData)
			{
			}

			public void DemandAttached(bool _demand)
			{
				if (_demand && !m_demandAttached)
				{
					base.NpcRuntimeData.NpcCharacter.CanBePooled = false;
				}
				else if (!_demand && m_demandAttached)
				{
					base.NpcRuntimeData.NpcCharacter.CanBePooled = true;
				}
				m_demandAttached = _demand;
			}
		}

		private const float CloseToPatrolThreshold = 4f;

		private const float CloseToPatrolThresholdSqr = 16f;

		public PatrolActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public void AttachDefault()
		{
			if (base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.CanActCommon())
			{
				NpcCharacter npcCharacter = base.NpcCharacter;
				Patrol patrol = npcCharacter.Patrol;
				if (patrol != null)
				{
					PatrolHelper.ForceIdleOfWaypoint(npcCharacter, patrol.PatrolComponent);
				}
			}
		}

		public BehaviorReturnCode ActionReturnToPatrol(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			Patrol patrol = npcCharacter.Patrol;
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				base.NpcRuntimeData.Patrol.TargetApproacher.SetData(base.NpcCharacter.Id, EntityId.InvalidId, EMovementSpeed.WalkFast, 16f);
			}
			return (!patrol.PatrolComponent.CurrentWaypoint.WarpTarget) ? base.NpcRuntimeData.Patrol.TargetApproacher.ApproachSteeringTargetAi(patrol) : BehaviorReturnCode.Success;
		}

		public void AttachReturnToPatrol()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			NavMeshAgent navMeshAgent = npcCharacter.UnityComponentCache.NavMeshAgent;
			Patrol patrol = npcCharacter.Patrol;
			if (navMeshAgent.enabled && navMeshAgent.CalculatePath(patrol.Position, base.NpcRuntimeData.Patrol.DummyPath))
			{
				AnimationController animationController = npcCharacter.UnityComponentCache.AnimationController;
				animationController.TaskManager.CreateAnimationTaskMoveToForcePlay(base.NpcRuntimeData.Patrol.DummyPath, animationController.CalculateSpeed(EMovementSpeed.WalkFast));
			}
		}

		public BehaviorReturnCode ActionPatrol(BehaviorReturnCode _lastReturnCode)
		{
			RuntimeData patrol = base.NpcRuntimeData.Patrol;
			BehaviorReturnCode behaviorReturnCode = ((!AiHelper.AddToPatrol(base.NpcRuntimeData)) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running);
			bool demand = behaviorReturnCode != BehaviorReturnCode.Failure && base.NpcCharacter.Patrol.PatrolComponent.CurrentWaypoint.WarpTarget;
			patrol.DemandAttached(demand);
			if (_lastReturnCode != BehaviorReturnCode.Running && behaviorReturnCode == BehaviorReturnCode.Running)
			{
				EnemyCharacter enemyCharacter = base.NpcCharacter as EnemyCharacter;
				if (enemyCharacter != null)
				{
					EnemyHelper.EnterRoaming(enemyCharacter);
				}
			}
			return behaviorReturnCode;
		}

		public void CancelPatrol()
		{
			RuntimeData patrol = base.NpcRuntimeData.Patrol;
			patrol.TargetApproacher.Reset();
			patrol.DemandAttached(false);
			AiHelper.RemoveFromPatrol(base.NpcRuntimeData, true);
		}

		public void AttachPatrol()
		{
			base.NpcRuntimeData.Patrol.TargetApproacher.Reset();
			NpcCharacter npcCharacter = base.NpcCharacter;
			ICharacterDrawWeaponCarrier characterDrawWeaponCarrier = npcCharacter.StateMachines as ICharacterDrawWeaponCarrier;
			if (characterDrawWeaponCarrier != null)
			{
				int num = characterDrawWeaponCarrier.CharacterDrawWeapon.Length;
				for (int i = 0; i < num; i++)
				{
					CharacterDrawWeaponBase characterDrawWeaponBase = characterDrawWeaponCarrier.CharacterDrawWeapon[i];
					if (characterDrawWeaponBase != null)
					{
						characterDrawWeaponBase.ReattachDroppedWeapon();
					}
				}
			}
			npcCharacter.Patrol.PatrolComponent.Approacher.PerformMemberAttach(npcCharacter.Id);
		}
	}
}
