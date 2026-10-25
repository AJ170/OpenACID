using AcierProtocol;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class ReactOnTargetActions : NpcBehaviorComponent
	{
		public class RuntimeData : BaseRuntimeData
		{
			public readonly SimpleGameTimer m_pushDelayTimer = new SimpleGameTimer();

			public readonly SimpleGameTimer m_pushApproachTimer = new SimpleGameTimer();

			public readonly SimpleGameTimer m_dismissCooldownTimer = new SimpleGameTimer();

			public readonly SimpleGameTimer m_waitAfterStareTimer = new SimpleGameTimer();

			public RuntimeData(NpcRuntimeData _npcRuntimeData)
				: base(_npcRuntimeData)
			{
			}
		}

		private const float ApproachMinTime = 1f;

		private const float DelayToApproachFactor = 0.5f;

		private const float DismissCooldown = 40f;

		private const int EnterCloseTurnMaxDelta = 2;

		public SimpleGameTimer PushDelayTimer
		{
			get
			{
				return base.NpcRuntimeData.ReactOnTarget.m_pushDelayTimer;
			}
		}

		public SimpleGameTimer PushApproachTimer
		{
			get
			{
				return base.NpcRuntimeData.ReactOnTarget.m_pushApproachTimer;
			}
		}

		public SimpleGameTimer DismissCooldownTimer
		{
			get
			{
				return base.NpcRuntimeData.ReactOnTarget.m_dismissCooldownTimer;
			}
		}

		public SimpleGameTimer WaitAfterStareTimer
		{
			get
			{
				return base.NpcRuntimeData.ReactOnTarget.m_waitAfterStareTimer;
			}
		}

		public ReactOnTargetActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public bool IsTurnPlaying()
		{
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			if (animationController == null)
			{
				return false;
			}
			AnimationTaskType currentTaskType = animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			return currentTaskType == AnimationTaskType.NpcTurning;
		}

		public bool IsPushOrTurnPlaying()
		{
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			if (animationController == null)
			{
				return false;
			}
			AnimationTaskType currentTaskType = animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			return currentTaskType == AnimationTaskType.NpcTurning || currentTaskType == AnimationTaskType.PushInstigator;
		}

		public bool IsDismissOrTurnPlaying()
		{
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			if (animationController == null)
			{
				return false;
			}
			AnimationTaskType currentTaskType = animationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			return currentTaskType == AnimationTaskType.NpcTurning || currentTaskType == AnimationTaskType.Dismiss;
		}

		public bool IsPushDelayFinished()
		{
			return PushDelayTimer.IsFinished;
		}

		public bool IsPushApproachFinished()
		{
			return PushApproachTimer.IsFinished;
		}

		public bool IsDismissCooldownRunning()
		{
			return DismissCooldownTimer.IsRunning;
		}

		public bool IsWaitAfterStareRunning()
		{
			return WaitAfterStareTimer.IsRunning;
		}

		public BehaviorReturnCode ActionUpdatePushDelay(BehaviorReturnCode _lastReturnCode)
		{
			int num = base.NpcCharacter.CharacterStats.Get_int(StatTypes.EStatType.Character_Detection_CloseRangeEnterTurn);
			int num2 = LogicMain.CurrentTurn - num;
			if (num2 <= 2 && !PushDelayTimer.IsRunning && !PushApproachTimer.IsRunning)
			{
				float duration = LogicMain.PlayerEntity.CharacterStats.Get_float(StatTypes.EStatType.Player_EnemyPushDelay);
				PushDelayTimer.Start(duration);
			}
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionApproachTargetForPush(BehaviorReturnCode _lastReturnCode)
		{
			base.NpcRuntimeData.ReactOnTarget.TargetApproacher.SetData(base.NpcCharacter.Id, EntityId.InvalidId, EMovementSpeed.WalkFast, 0.48999998f, 4f);
			return base.NpcRuntimeData.ReactOnTarget.TargetApproacher.ApproachCurrentTargetAi();
		}

		public BehaviorReturnCode ActionStartApproachTimer(BehaviorReturnCode _lastReturnCode)
		{
			float maxPushDelay = LogicMain.GameDefinition.MaxPushDelay;
			float num = LogicMain.PlayerEntity.CharacterStats.Get_float(StatTypes.EStatType.Player_EnemyPushDelay);
			float duration = 1f + Mathf.Max(0f, maxPushDelay - num * 0.5f);
			PushApproachTimer.Start(duration);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionPush(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlayPush, IsPushOrTurnPlaying);
		}

		public BehaviorReturnCode ActionDismiss(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlayDismiss, IsDismissOrTurnPlaying);
		}

		public BehaviorReturnCode ActionStareAtTarget(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlayStare, IsTurnPlaying);
		}

		public BehaviorReturnCode ActionRestartWaitAfterStare(BehaviorReturnCode _lastReturnCode)
		{
			PlayerCharacter targetAsPlayer = base.NpcCharacter.GetTargetAsPlayer();
			float duration = targetAsPlayer.CharacterStats.Get_float(StatTypes.EStatType.Player_EnemyWaitAfterStareDuration);
			WaitAfterStareTimer.Start(duration);
			return BehaviorReturnCode.Success;
		}

		private bool PlayPush()
		{
			if (PlayerHelper.EnterBeingPushed(base.NpcCharacter.Id))
			{
				AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
				animationController.TaskManager.CreateAnimationTasksPushInstigator(LogicMain.PlayerEntity);
				return true;
			}
			return false;
		}

		private bool PlayDismiss()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			PlayerCharacter targetAsPlayer = npcCharacter.GetTargetAsPlayer();
			DismissCooldownTimer.Start(40f);
			return AiHelper.PlayDismiss(npcCharacter, null, targetAsPlayer.Position);
		}

		private bool PlayStare()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			PlayerCharacter targetAsPlayer = base.NpcCharacter.GetTargetAsPlayer();
			return AiHelper.PlayStare(npcCharacter, targetAsPlayer.Position);
		}
	}
}
