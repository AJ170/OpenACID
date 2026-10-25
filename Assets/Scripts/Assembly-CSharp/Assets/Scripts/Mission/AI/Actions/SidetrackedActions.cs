using System;
using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.States;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.UI.LogicInterface;
using Assets.Scripts.Mission.Utilities;
using SharedBaseLib.System;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class SidetrackedActions : NpcBehaviorComponent
	{
		public class RuntimeData : BaseRuntimeData
		{
			public readonly SimpleGameTimer m_threatCooldown = new SimpleGameTimer();

			public RuntimeData(NpcRuntimeData _npcRuntimeData)
				: base(_npcRuntimeData)
			{
			}
		}

		private const float MinThreatCooldown = 4f;

		private const float MaxThreatCooldown = 6.5f;

		private const float MinTimeToArgueAfterSeduced = 1.5f;

		private const float MaxTimeToArgueAfterSeduced = 2.5f;

		// Created on first use: Unity 5.4+ forbids NavMeshPath construction in field initializers.
		private NavMeshPath m_dummyPath__lazy;

		private NavMeshPath m_dummyPath
		{
			get
			{
				return m_dummyPath__lazy ?? (m_dummyPath__lazy = new NavMeshPath());
			}
		}

		private NavMeshHit m_dummyHit;

		private static readonly int[] ZoneDirections = new int[13]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12
		};

		private readonly int[] ShoutNpcIds = new int[1] { 5 };

		private readonly int[] MeleeHitTaskTypes = new int[1] { 81 };

		private readonly int[] ExecuteOnGroundTaskTypes = new int[1] { 64 };

		private readonly int[] ShooutTaskTypes = new int[2] { 34, 57 };

		private SimpleGameTimer ThreatCooldown
		{
			get
			{
				return base.NpcRuntimeData.Sidetracked.m_threatCooldown;
			}
		}

		public SidetrackedActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		public BehaviorReturnCode ActionSweetMusicLureReaction(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionWaitForAnim(_lastReturnCode, base.NpcCharacter, StartSweetMusicLureAnim, new int[1] { 55 });
		}

		private bool StartSweetMusicLureAnim()
		{
			PlayerCharacter targetAsPlayer = base.NpcCharacter.GetTargetAsPlayer();
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			return animationController.TaskManager.CreateAnimationTasksSweetMusicReaction(targetAsPlayer.Position);
		}

		public BehaviorReturnCode ActionApproachTargetSweetMusic(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode == BehaviorReturnCode.Running)
			{
				base.NpcRuntimeData.Sidetracked.TargetApproacher.Update();
				if (base.NpcRuntimeData.Sidetracked.TargetApproacher.CheckIsRunning())
				{
					return BehaviorReturnCode.Running;
				}
				return BehaviorReturnCode.Success;
			}
			if (!base.NpcCharacter.IsAttached)
			{
				return BehaviorReturnCode.Failure;
			}
			PlayerCharacter targetAsPlayer = base.NpcCharacter.GetTargetAsPlayer();
			Vector3 vector = targetAsPlayer.Position;
			NavMeshPath _path = new NavMeshPath();
			int autoPassableNavMeshLayerMask = base.NpcCharacter.GetAutoPassableNavMeshLayerMask();
			PlayerMain playerMain = targetAsPlayer.PlayerCharacterStateMachines.PlayerMain;
			if (playerMain.CurrentStateType == EStateType.PlayerMain_HidingInObject)
			{
				EntityId entityId = playerMain.GetPayload<PlayerMain.InteractingWithEntityState.InteractingWithEntityPayload>(EStateType.PlayerMain_HidingInObject).EntityId;
				if (entityId != EntityId.InvalidId)
				{
					InteractableObject entity = entityId.GetEntity<InteractableObject>();
					InteractionLocators.Locator _locator;
					if (entity.InteractionLocators.ChooseClosestLocator(base.NpcCharacter.Position, autoPassableNavMeshLayerMask, out _locator, out _path))
					{
						if (entity.Type == EntityTypes.EType.Bench)
						{
							Vector3 position = _locator.Position.transform.position;
							Vector3 vector2 = position - targetAsPlayer.Position;
							vector2.Normalize();
							vector = position + vector2;
						}
						else
						{
							vector = (_locator.Position.transform.position + _locator.Step.transform.position) / 2f;
							vector.y = _locator.Position.transform.position.y;
							if (NavigationHelper.CascadingSamplePosition(vector, out m_dummyHit, autoPassableNavMeshLayerMask))
							{
								vector = m_dummyHit.position;
							}
						}
					}
				}
			}
			else if (playerMain.CurrentStateType == EStateType.PlayerMain_HidingInPatrol)
			{
				PlayerMain.HidingInPatrolState.HidingInPatrolPayload payload = playerMain.GetPayload<PlayerMain.HidingInPatrolState.HidingInPatrolPayload>(EStateType.PlayerMain_HidingInPatrol);
				Assets.Scripts.Mission.Logic.Entities.Patrol entity2 = payload.PatrolId.GetEntity<Assets.Scripts.Mission.Logic.Entities.Patrol>();
				Vector3 _position;
				if (!entity2.PatrolComponent.Formation.GetTargetWorldPosition(targetAsPlayer.Id, out _position))
				{
					return BehaviorReturnCode.Failure;
				}
				if (!NavMesh.CalculatePath(base.NpcCharacter.Position, _position, autoPassableNavMeshLayerMask, _path) || _path.status != 0)
				{
					return BehaviorReturnCode.Failure;
				}
				float num = 0f;
				bool flag = false;
				for (int num2 = _path.corners.Length - 1; num2 >= 1; num2--)
				{
					float num3 = Vector3.Distance(_path.corners[num2], _path.corners[num2 - 1]);
					if (num + num3 >= 3f)
					{
						float t = (3f - num) / num3;
						vector = Vector3.Lerp(_path.corners[num2], _path.corners[num2 - 1], t);
						flag = true;
						break;
					}
					num += num3;
				}
				if (!flag)
				{
					vector = base.NpcCharacter.Position;
				}
			}
			else if (targetAsPlayer.GetIsStalking())
			{
				PlayerStalkingZone.StalkingZoneState.StalkingZoneStatePayload payload2 = targetAsPlayer.PlayerCharacterStateMachines.PlayerStalkingZone.GetPayload<PlayerStalkingZone.StalkingZoneState.StalkingZoneStatePayload>(EStateType.PlayerStalkingZone_Stalking);
				StalkingZone entity3 = payload2.StalkingZone.GetEntity<StalkingZone>();
				vector = entity3.StalkingZoneComponent.ChooseClosestColliderPositionInStalkingZone(base.NpcCharacter.Position);
			}
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			AnimationTaskManager.ELayer layer = AnimationTaskLureReaction.Properties.Layer;
			bool flag2 = animationController.TaskManager.CancelCurrentTask(layer);
			if (flag2)
			{
				float speedValue = animationController.TaskManager.AnimationController.CalculateSpeed(EMovementSpeed.Walk);
				float buffedStat_float = LogicMain.PlayerEntity.Skills.GetBuffedStat_float("Distraction_01".ToEnum<SkillTypes>(), StatTypes.EStatType.Rule_AttractionRange);
				base.NpcRuntimeData.Sidetracked.TargetApproacher.SetData(base.NpcCharacter.Id, vector, speedValue, 0.75f, buffedStat_float * buffedStat_float);
				base.NpcRuntimeData.Sidetracked.TargetApproacher.Approach();
			}
			return (!flag2) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running;
		}

		public BehaviorReturnCode ActionJackInTheBoxReaction(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionWaitForAnim(_lastReturnCode, base.NpcCharacter, StartJackInTheBoxLureAnim, new int[1] { 55 });
		}

		private bool StartJackInTheBoxLureAnim()
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.StateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null)
			{
				return false;
			}
			JackBoxConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<JackBoxConditionState.Payload>(EStateType.CharacterCondition_JackBoxed);
			AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
			return animationController.TaskManager.CreateAnimationTasksSweetMusicReaction(payload.LookAtPosition);
		}

		public void AttachApproachTargetJackInTheBox()
		{
			ApproachTargetJackInTheBox(true);
		}

		public BehaviorReturnCode ActionApproachTargetJackInTheBox(BehaviorReturnCode _lastReturnCode)
		{
			if (_lastReturnCode == BehaviorReturnCode.Running)
			{
				base.NpcRuntimeData.Sidetracked.TargetApproacher.Update();
				if (base.NpcRuntimeData.Sidetracked.TargetApproacher.CheckIsRunning())
				{
					return BehaviorReturnCode.Running;
				}
				return BehaviorReturnCode.Success;
			}
			return (!ApproachTargetJackInTheBox(false)) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running;
		}

		private bool ApproachTargetJackInTheBox(bool _shortcutRestart)
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.StateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null)
			{
				return false;
			}
			JackBoxConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<JackBoxConditionState.Payload>(EStateType.CharacterCondition_JackBoxed);
			float buffedStat_float = LogicMain.PlayerEntity.Skills.GetBuffedStat_float("Jack_in_the_Box_01".ToEnum<SkillTypes>(), StatTypes.EStatType.Rule_AttractionRange);
			if (base.NpcCharacter.IsAttached || _shortcutRestart)
			{
				AnimationController animationController = base.NpcCharacter.UnityComponentCache.AnimationController;
				AnimationTaskManager.ELayer layer = AnimationTaskLureReaction.Properties.Layer;
				if (!animationController.TaskManager.CancelCurrentTask(layer))
				{
					return false;
				}
				float speedValue = animationController.TaskManager.AnimationController.CalculateSpeed(EMovementSpeed.Walk);
				base.NpcRuntimeData.Sidetracked.TargetApproacher.SetData(base.NpcCharacter.Id, payload.TargetPosition, speedValue, 0.75f, buffedStat_float * buffedStat_float);
				base.NpcRuntimeData.Sidetracked.TargetApproacher.Approach();
			}
			else
			{
				NavMeshPath path = new NavMeshPath();
				int autoPassableNavMeshLayerMask = base.NpcCharacter.GetAutoPassableNavMeshLayerMask();
				if (!AiHelper.CanApproachWithSampling(base.NpcCharacter, payload.TargetPosition, autoPassableNavMeshLayerMask, false, path))
				{
					return false;
				}
				float defaultSpeedValue = EMovementSpeed.Walk.GetDefaultSpeedValue();
				float defaultSpeedValue2 = EMovementSpeed.Run.GetDefaultSpeedValue();
				float value = defaultSpeedValue2 * path.CalculateLength() / buffedStat_float;
				value = Mathf.Clamp(value, defaultSpeedValue, defaultSpeedValue2);
				NavAgentSurrogate navAgentSurrogate = base.NpcCharacter.NavAgentSurrogate;
				navAgentSurrogate.Speed = value;
				navAgentSurrogate.Path = path;
			}
			return true;
		}

		public void AttachApproachTargetSeduced()
		{
			AiHelper.AttachApproachTarget(base.NpcCharacter, base.NpcRuntimeData.Sidetracked.DummyPath, EMovementSpeed.Walk);
		}

		public BehaviorReturnCode ActionApproachTargetSeduced(BehaviorReturnCode _lastReturnCode)
		{
			return ActionApproachConditionTarget(9f, EStateType.CharacterCondition_Seduced, EMovementSpeed.Walk);
		}

		public BehaviorReturnCode ActionIdleAtTargetSeduced(BehaviorReturnCode _lastReturnCode)
		{
			if (!base.NpcCharacter.IsAttached)
			{
				return BehaviorReturnCode.Running;
			}
			return AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlaySeduced, UpdateSeduced);
		}

		public void CancelIdleAtTargetSeduced()
		{
			if (base.NpcCharacter.IsAttached && base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.EnemySeduced)
			{
				base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base);
			}
		}

		public void AttachApproachTargetGreedy()
		{
			AiHelper.AttachApproachTarget(base.NpcCharacter, base.NpcRuntimeData.Sidetracked.TargetApproacher.PathToTarget, EMovementSpeed.Run);
		}

		public BehaviorReturnCode ActionApproachTargetGreedy(BehaviorReturnCode _lastReturnCode)
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.StateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null)
			{
				return BehaviorReturnCode.Failure;
			}
			GreedyConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<GreedyConditionState.Payload>(EStateType.CharacterCondition_Greedy);
			base.NpcRuntimeData.Sidetracked.TargetApproacher.SetData(base.NpcCharacter.Id, payload.Target, EMovementSpeed.Run, 0.5625f);
			BehaviorReturnCode behaviorReturnCode = base.NpcRuntimeData.Sidetracked.TargetApproacher.ApproachTargetAi();
			if (behaviorReturnCode == BehaviorReturnCode.Success)
			{
				payload.ReachedTarget();
			}
			return behaviorReturnCode;
		}

		public BehaviorReturnCode ActionIdleAtTargetGreedy(BehaviorReturnCode _lastReturnCode)
		{
			if (!base.NpcCharacter.IsAttached)
			{
				return BehaviorReturnCode.Running;
			}
			AnimatorStateMachine stateMachine = base.NpcCharacter.UnityComponentCache.AnimationController.StateMachine;
			if (stateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_CoinThrowReaction.NameHash)
			{
				return BehaviorReturnCode.Running;
			}
			BehaviorReturnCode behaviorReturnCode = AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlayGreedy, UpdateGreedy);
			if (behaviorReturnCode == BehaviorReturnCode.Success)
			{
				ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.StateMachines as ICharacterConditionCarrier;
				GreedyConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<GreedyConditionState.Payload>(EStateType.CharacterCondition_Greedy);
				payload.FindNewTarget();
			}
			return behaviorReturnCode;
		}

		public void AttachApproachTargetStickyBombPanic()
		{
			AiHelper.AttachApproachTarget(base.NpcCharacter, base.NpcRuntimeData.Sidetracked.TargetApproacher.PathToTarget, EMovementSpeed.Run);
		}

		public BehaviorReturnCode ActionApproachTargetStickyBombPanic(BehaviorReturnCode _lastReturnCode)
		{
			ICharacterConditionCarrier characterConditionCarrier = base.NpcCharacter.StateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null)
			{
				return BehaviorReturnCode.Failure;
			}
			StickyBombPanicConditionState.Payload payload = characterConditionCarrier.CharacterCondition.GetPayload<StickyBombPanicConditionState.Payload>(EStateType.CharacterCondition_StickyBombPanic);
			base.NpcRuntimeData.Sidetracked.TargetApproacher.SetData(base.NpcCharacter.Id, payload.TargetPosition, EMovementSpeed.Run, 0.5625f);
			BehaviorReturnCode behaviorReturnCode = base.NpcRuntimeData.Sidetracked.TargetApproacher.ApproachTargetAi();
			if (behaviorReturnCode == BehaviorReturnCode.Success)
			{
				payload.TargetPosition = FindNewStickyBombPanicZone(base.NpcCharacter.Position);
				base.NpcRuntimeData.Sidetracked.TargetApproacher.SetData(base.NpcCharacter.Id, payload.TargetPosition, EMovementSpeed.Run, 0.5625f);
				base.NpcRuntimeData.Sidetracked.TargetApproacher.ApproachTargetAi();
			}
			return behaviorReturnCode;
		}

		public BehaviorReturnCode ActionIdleAtTargetStickyBombPanic(BehaviorReturnCode _lastReturnCode)
		{
			if (!base.NpcCharacter.IsAttached)
			{
				return BehaviorReturnCode.Running;
			}
			AnimatorStateMachine stateMachine = base.NpcCharacter.UnityComponentCache.AnimationController.StateMachine;
			if (stateMachine.GetCurrentStateMachineHash() == AnimationStateMachineHashes.StateMachine_StickyBombReaction.NameHash)
			{
				return BehaviorReturnCode.Running;
			}
			BehaviorReturnCode behaviorReturnCode = AiHelper.ActionGeneric(_lastReturnCode, base.NpcCharacter, PlayStickyBombPanic, UpdateStickyBombPanic);
			if (behaviorReturnCode == BehaviorReturnCode.Success && base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.IsStickyBombPaniced())
			{
				ActionApproachTargetStickyBombPanic(BehaviorReturnCode.NotCalled);
			}
			return behaviorReturnCode;
		}

		public BehaviorReturnCode ActionShoutAtEscortee(BehaviorReturnCode _lastReturnCode)
		{
			return AiHelper.ActionWaitForAnim(_lastReturnCode, base.NpcCharacter, PlayShout, ShooutTaskTypes, false);
		}

		public void AttachApproachTargetThreatVictim()
		{
			AiHelper.AttachApproachTarget(base.NpcCharacter, base.NpcRuntimeData.Sidetracked.TargetApproacher.PathToTarget, EMovementSpeed.WalkFast);
		}

		public BehaviorReturnCode ActionApproachTargetThreatVictim(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = (NpcCharacter)base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
			float approachedDistanceSqr = 0f;
			if (npcCharacter.Patrol.VictimComponent is Captive)
			{
				approachedDistanceSqr = 4f;
			}
			else if (npcCharacter.Patrol.VictimComponent is Escort)
			{
				approachedDistanceSqr = 12.25f;
			}
			return ActionApproachConditionTarget(approachedDistanceSqr, EStateType.CharacterCondition_ThreateningVictim, EMovementSpeed.WalkFast);
		}

		public void CancelApproachTargetThreatVictim()
		{
			NpcCharacter npcTargetFromCondition = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
			if (npcTargetFromCondition != null)
			{
				npcTargetFromCondition.Patrol.VictimComponent.DeregisterArrivedEnemy(base.NpcCharacter.Id);
			}
		}

		public void AttachIdleAtTargetThreatVictim()
		{
			base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskIdleForcePlay(IdleType.HighProfile);
		}

		public BehaviorReturnCode ActionIdleAtTargetThreatVictim(BehaviorReturnCode _lastReturnCode)
		{
			BehaviorReturnCode behaviorReturnCode = BehaviorReturnCode.Running;
			if (_lastReturnCode == BehaviorReturnCode.Running)
			{
				if (base.NpcCharacter.IsAttached && ThreatCooldown.IsFinished)
				{
					NpcCharacter npcTargetFromCondition = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
					AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
					taskManager.CreateAnimationTaskThreat(npcTargetFromCondition, true);
					RestartThreatCooldown();
				}
				else if (!ThreatCooldown.IsStarted)
				{
					RestartThreatCooldown();
				}
				return BehaviorReturnCode.Running;
			}
			NpcCharacter npcTargetFromCondition2 = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
			if (npcTargetFromCondition2.Patrol.VictimComponent.RegisterArrivedEnemy(base.NpcCharacter.Id))
			{
				base.NpcCharacter.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Unsheathed);
				if (base.NpcCharacter.IsAttached)
				{
					AnimationTaskManager taskManager2 = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
					taskManager2.CreateAnimationTaskThreat(npcTargetFromCondition2, true);
					RestartThreatCooldown();
				}
				return BehaviorReturnCode.Running;
			}
			return BehaviorReturnCode.Failure;
		}

		public void CancelIdleAtTargetThreatVictim()
		{
			NpcCharacter npcTargetFromCondition = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
			if (!npcTargetFromCondition.Patrol.VictimComponent.ThreateningFinished)
			{
				npcTargetFromCondition.Patrol.VictimComponent.DeregisterArrivedEnemy(base.NpcCharacter.Id);
			}
		}

		public BehaviorReturnCode ActionKillEscortee(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			return AiHelper.ActionWaitForAnim(_lastReturnCode, npcCharacter, OnEnterActionKillEscortee, MeleeHitTaskTypes, false);
		}

		public BehaviorReturnCode ActionKillCaptive(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			return AiHelper.ActionWaitForAnim(_lastReturnCode, npcCharacter, OnEnterActionKillCaptive, ExecuteOnGroundTaskTypes, false);
		}

		public BehaviorReturnCode ActionRegisterVictimDead(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter victim = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetVictim();
			victim.Patrol.VictimComponent.EndKillVictim();
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionEndThreatening(BehaviorReturnCode _lastReturnCode)
		{
			NpcCharacter victim = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetVictim();
			victim.Patrol.VictimComponent.DeregisterArrivedEnemy(base.NpcCharacter.Id);
			return BehaviorReturnCode.Success;
		}

		private bool PlaySeduced()
		{
			StateMachines stateMachines = base.NpcCharacter.StateMachines;
			ICharacterConditionCarrier characterConditionCarrier = stateMachines as ICharacterConditionCarrier;
			if (characterConditionCarrier == null)
			{
				return false;
			}
			return true;
		}

		private bool UpdateSeduced()
		{
			CharacterDrawWeaponMelee characterDrawMeleeWeapon = ((ICharacterDrawMeleeWeaponCarrier)base.NpcCharacter.StateMachines).CharacterDrawMeleeWeapon;
			switch (characterDrawMeleeWeapon.CurrentStateType)
			{
			case EStateType.CharacterDrawWeapon_Unsheathed:
				base.NpcCharacter.ChooseWeaponState(EWeaponCategory.Melee, EWeaponState.Sheathed);
				return true;
			case EStateType.Generic_WaitingForAnim:
				return true;
			default:
			{
				StateMachines stateMachines = base.NpcCharacter.StateMachines;
				ICharacterConditionCarrier characterConditionCarrier = stateMachines as ICharacterConditionCarrier;
				if (characterConditionCarrier == null)
				{
					return false;
				}
				AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
				Assets.Scripts.Mission.Logic.Entities.Patrol patrol = (Assets.Scripts.Mission.Logic.Entities.Patrol)base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetTargetFromCondition(EStateType.CharacterCondition_Seduced);
				PatrolSeduce patrolSeduce = patrol.PatrolStateMachines.PatrolSeduce;
				PatrolSeduce.DancingState.Payload payload = patrolSeduce.GetPayload<PatrolSeduce.DancingState.Payload>(EStateType.PatrolSeduce_Dancing);
				if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.EnemySeduced && payload != null && payload.ActiveTimer.IsRunning && payload.ActiveTimer.TimeToFinish < LogicMain.Random.Rand(1.5f, 2.5f))
				{
					taskManager.CreateAnimationTaskIdle(IdleType.StandAndArgumentMale);
				}
				else if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.EnemySeduced && payload != null && payload.ActiveTimer.IsStarted && payload.ActiveTimer.TimeToFinish > 2.5f)
				{
					TargetConditionState.Payload payload2 = characterConditionCarrier.CharacterCondition.GetPayload<TargetConditionState.Payload>(EStateType.CharacterCondition_Seduced);
					EntityId target = payload2.Target;
					return base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskEnemySeduced(target);
				}
				return true;
			}
			}
		}

		private bool PlayGreedy()
		{
			AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
			return taskManager.CreateAnimationTaskGreedy();
		}

		private bool UpdateGreedy()
		{
			AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
			return taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.Greedy;
		}

		private bool PlayStickyBombPanic()
		{
			AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
			return taskManager.CreateAnimationTaskStickyBombPanic();
		}

		private bool PlayShout()
		{
			return AiHelper.PlayDismiss(base.NpcCharacter, ShoutNpcIds, base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetVictim().Position);
		}

		private bool UpdateStickyBombPanic()
		{
			AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
			return taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.StickyBombPanic;
		}

		private Vector3 FindNewStickyBombPanicZone(Vector3 _currentPosition)
		{
			for (uint num = (uint)(ZoneDirections.Length - 1); num >= 1; num--)
			{
				uint num2 = LogicMain.Random.Rand(0u, num);
				int num3 = ZoneDirections[num];
				ZoneDirections[num] = ZoneDirections[num2];
				ZoneDirections[num2] = num3;
			}
			float num4 = LogicMain.Random.Rand(0f, (float)Math.PI * 2f / (float)ZoneDirections.Length);
			for (int i = 0; i < ZoneDirections.Length; i++)
			{
				float f = num4 + (float)ZoneDirections[i] * ((float)Math.PI * 2f) / (float)ZoneDirections.Length;
				Vector3 _zonePosition = _currentPosition + new Vector3(Mathf.Sin(f), 0f, Mathf.Cos(f)) * 2.5f;
				if (CanReachZone(ref _zonePosition, _currentPosition))
				{
					return _zonePosition;
				}
			}
			return _currentPosition;
		}

		private bool CanReachZone(ref Vector3 _zonePosition, Vector3 _currentPosition)
		{
			int walkableLayerMaskPreset = NavMeshLayers.WalkableLayerMaskPreset;
			if (!NavMesh.SamplePosition(_zonePosition, out m_dummyHit, 2f, walkableLayerMaskPreset))
			{
				return false;
			}
			_zonePosition = new Vector3(_zonePosition.x, m_dummyHit.position.y, _zonePosition.z);
			if (m_dummyHit.position.DistanceXZSquared(_zonePosition) > 0.0001f)
			{
				return false;
			}
			if (!NavMesh.FindClosestEdge(_zonePosition, out m_dummyHit, walkableLayerMaskPreset))
			{
				return false;
			}
			if (m_dummyHit.position.DistanceXZSquared(_zonePosition) < 0.5625f)
			{
				return false;
			}
			if (!NavMesh.CalculatePath(_currentPosition, _zonePosition, walkableLayerMaskPreset, m_dummyPath))
			{
				return false;
			}
			float num = m_dummyPath.CalculateLength();
			return num <= 1.5f * Vector3.Distance(_currentPosition, _zonePosition);
		}

		private BehaviorReturnCode ActionApproachConditionTarget(float _approachedDistanceSqr, EStateType _characterConditionState, EMovementSpeed _speed)
		{
			GameEntity targetFromCondition = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetTargetFromCondition(_characterConditionState);
			base.NpcRuntimeData.Sidetracked.TargetApproacher.SetData(base.NpcCharacter.Id, targetFromCondition.Id, _speed, _approachedDistanceSqr);
			return base.NpcRuntimeData.Sidetracked.TargetApproacher.ApproachTargetAi();
		}

		private bool OnEnterActionKillEscortee()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			NpcCharacter npcTargetFromCondition = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
			if (npcCharacter.IsAttached)
			{
				npcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskHitEscortee(npcTargetFromCondition.Position);
			}
			npcTargetFromCondition.Patrol.VictimComponent.StartKillVictim(npcCharacter);
			return true;
		}

		private bool OnEnterActionKillCaptive()
		{
			NpcCharacter npcCharacter = base.NpcCharacter;
			NpcCharacter npcTargetFromCondition = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.CommonKnowledge.GetNpcTargetFromCondition(EStateType.CharacterCondition_ThreateningVictim);
			if (npcCharacter.IsAttached)
			{
				npcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskExecuteOnGround(npcTargetFromCondition);
			}
			npcTargetFromCondition.Patrol.VictimComponent.StartKillVictim(npcCharacter);
			return true;
		}

		private void RestartThreatCooldown()
		{
			float duration = LogicMain.Random.Rand(4f, 6.5f);
			ThreatCooldown.Start(duration);
		}
	}
}
