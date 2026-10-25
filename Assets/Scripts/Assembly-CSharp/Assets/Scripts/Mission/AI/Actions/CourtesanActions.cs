using UnityEngine.AI;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using UbiCore;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class CourtesanActions : NpcBehaviorComponent
	{
		public CourtesanActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public BehaviorReturnCode SeduceWalk(BehaviorReturnCode _lastReturnCode)
		{
			return (!base.NpcCharacter.IsAttached) ? SeduceWalkNotAttached() : SeduceWalkAttached(_lastReturnCode);
		}

		public void CancelSeduceWalk()
		{
			AiHelper.RemoveFromPatrol(base.NpcRuntimeData, true);
			CancelUpperBodySeduce();
		}

		public BehaviorReturnCode SeduceGuards(BehaviorReturnCode _lastReturnCode)
		{
			if (!base.NpcCharacter.IsAttached)
			{
				return BehaviorReturnCode.Running;
			}
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlaySeduceDance, null);
		}

		public void CancelSeduceGuards()
		{
			CancelSeduceWalk();
			if (base.NpcCharacter.IsAttached && base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.CourtesanSeduce)
			{
				base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base);
			}
		}

		private BehaviorReturnCode SeduceWalkAttached(BehaviorReturnCode _lastReturnCode)
		{
			if (!AiHelper.AddToPatrol(base.NpcRuntimeData))
			{
				CancelSeduceWalk();
				return BehaviorReturnCode.Failure;
			}
			if (_lastReturnCode != BehaviorReturnCode.Running)
			{
				base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskUpperBodySeduce();
			}
			else if (base.NpcCharacter.Patrol.PatrolComponent.Idles && MathHelper.Approximately(base.NpcCharacter.UnityComponentCache.NavMeshAgent.velocity.sqrMagnitude, 0f))
			{
				CancelUpperBodySeduce();
				return BehaviorReturnCode.Success;
			}
			return BehaviorReturnCode.Running;
		}

		private BehaviorReturnCode SeduceWalkNotAttached()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			if (AiHelper.AddToPatrol(base.NpcRuntimeData))
			{
				Patrol patrol = npcCharacter.Patrol;
				return (!patrol.PatrolComponent.Idles) ? BehaviorReturnCode.Running : BehaviorReturnCode.Success;
			}
			return BehaviorReturnCode.Failure;
		}

		private void CancelUpperBodySeduce()
		{
			if (base.NpcCharacter.IsAttached && base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Special) == AnimationTaskType.SpecialSeduce)
			{
				base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Special);
			}
		}

		private bool PlaySeduceDance()
		{
			EntityId targetId = null;
			Patrol patrol = base.NpcCharacter.Patrol;
			IPatrolSeduceCarrier patrolStateMachines = patrol.PatrolStateMachines;
			if (patrolStateMachines != null)
			{
				PatrolSeduce patrolSeduce = patrolStateMachines.PatrolSeduce;
				if (patrolSeduce != null)
				{
					PatrolSeduce.DancingState.Payload payload = patrolSeduce.GetPayload<PatrolSeduce.DancingState.Payload>(EStateType.PatrolSeduce_Dancing);
					if (payload != null)
					{
						targetId = payload.TargetId;
					}
				}
			}
			bool flag = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskCourtesanSeduce(targetId);
			if (flag)
			{
				AiHelper.RemoveFromPatrol(base.NpcRuntimeData, false);
			}
			return flag;
		}
	}
}
