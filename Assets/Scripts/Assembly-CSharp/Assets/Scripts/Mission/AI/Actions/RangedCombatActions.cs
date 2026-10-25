using AcierProtocol;
using Assets.Scripts.Animation;
using Assets.Scripts.Animation.Tasks;
using Assets.Scripts.Audio;
using Assets.Scripts.Mission.AI.BehaviorLibrary.Components;
using Assets.Scripts.Mission.AI.Helper;
using Assets.Scripts.Mission.AI.NpcBehavior;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.Mission.Utilities;
using Database;
using SharedBaseLib.System;
using UbiCore;
using UbiCore.Debug;
using UbiCore.Messenger;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;

namespace Assets.Scripts.Mission.AI.Actions
{
	public class RangedCombatActions : NpcBehaviorComponent
	{
		public class RuntimeData : BaseRuntimeData, IAnimatorEventListener
		{
			public readonly SimpleGameTimer m_chargingTimer = new SimpleGameTimer();

			public readonly SimpleGameTimer m_keepChargingTimer = new SimpleGameTimer();

			public RuntimeData(NpcRuntimeData _npcRuntimeData)
				: base(_npcRuntimeData)
			{
			}

			public void RegistAsEventListener()
			{
				AnimatorDataController dataController = base.NpcRuntimeData.NpcCharacter.UnityComponentCache.AnimationController.DataController;
				if (!dataController.IsEventListenerRegistered(this))
				{
					dataController.RegisterEventListener(base.NpcRuntimeData.RangedCombat);
				}
			}

			public void OnRangedAttackHit(Projectile _projectile)
			{
				RangedCombatActions.OnRangedAttackHit(_projectile, base.NpcRuntimeData.NpcCharacter);
			}

			public bool OnAnimatorEvent(AnimatorEvent _event)
			{
				if (_event.Type == AnimatorEventType.ProjectileStart)
				{
					PerformDaggerAttack(base.NpcRuntimeData);
					return true;
				}
				return false;
			}
		}

		private const string HitEffectName = "bloodHitBack";

		private const float StrafeFactorMin = 1f;

		private const float StrafeFactorMax = 1.2f;

		private const float StrafeFactorMinNoLoS = 0.5f;

		private const float StrafeFactorMaxNoLoS = 1.2f;

		private const int StrafeXStepCount = 8;

		private const float StrafeXMetersPerStep = 0.5f;

		private const float StrafeBaseFactorX = 0.5f;

		private const float StrafeBaseFactorXRest = 0.5f;

		private const float StrafeBaseFactorZ = 0.5f;

		private const float StrafeBaseFactorZRest = 0.5f;

		private const float StrafeZNavCheckDist = 0.5f;

		private const float StrafeZNavCheckSampleDist = 1.5f;

		private const float StrafeZNavCheckXZDist = 0.25f;

		private const float StrafeZNavCheckXZDistSqr = 0.0625f;

		private const float PlayerRadius = 1.25f;

		public static readonly int s_losRycastLayerMask;

		public static readonly int s_losRycastLayerMaskNear;

		public SimpleGameTimer ChargingTimer
		{
			get
			{
				return base.NpcRuntimeData.RangedCombat.m_chargingTimer;
			}
		}

		public SimpleGameTimer KeepChargingTimer
		{
			get
			{
				return base.NpcRuntimeData.RangedCombat.m_keepChargingTimer;
			}
		}

		public RangedCombatActions(NpcTreeHolder _npcTreeHolder)
			: base(_npcTreeHolder)
		{
		}

		static RangedCombatActions()
		{
			s_losRycastLayerMask = Layers.StaticCollidersLayersMaskPreset.AddLayers("Character");
			s_losRycastLayerMaskNear = Layers.StaticCollidersLayersMaskPreset;
		}

		public bool ConditionKeepChargingIsRunning()
		{
			return KeepChargingTimer.IsRunning;
		}

		public bool ConditionChargingIsFinished()
		{
			return ChargingTimer.IsFinished;
		}

		public bool ConditionChargingIsRunning()
		{
			return ChargingTimer.IsRunning;
		}

		public BehaviorReturnCode ActionUnsheathRanged(BehaviorReturnCode _lastReturnCode)
		{
			UnsheathRanged(GetCharacterDrawWeapon());
			return BehaviorReturnCode.Running;
		}

		public void CancelUnsheathRanged()
		{
			CancelAimAndSheathRanged();
		}

		public BehaviorReturnCode ActionAimWithRanged(BehaviorReturnCode _lastReturnCode)
		{
			BehaviorReturnCode behaviorReturnCode = BehaviorReturnCode.Failure;
			CharacterDrawWeaponBase characterDrawWeapon = GetCharacterDrawWeapon();
			UnsheathRanged(characterDrawWeapon);
			if (characterDrawWeapon.CurrentStateType == EStateType.CharacterDrawWeapon_Unsheathed)
			{
				behaviorReturnCode = ((base.NpcCharacter.IsAttached && !base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAimWithRanged()) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Running);
				if (behaviorReturnCode == BehaviorReturnCode.Running)
				{
					if (!ChargingTimer.IsStarted)
					{
						StartCharging();
					}
					if (base.NpcTreeHolder.NpcDelegateHolder.Knowledge.TargetKnowledge.IsTargetInRange(EDetectionRange.Normal))
					{
						RestartKeepChargingTimer();
					}
				}
			}
			else if (characterDrawWeapon.ChosenWeaponState == EWeaponState.Unsheathed)
			{
				behaviorReturnCode = BehaviorReturnCode.Running;
			}
			return behaviorReturnCode;
		}

		public void AttachAimWithRanged()
		{
			base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAimWithRangedForcePlay();
		}

		public void CancelAimWithRanged()
		{
			CancelAimAndSheathRanged();
		}

		public BehaviorReturnCode ActionShootWithRanged(BehaviorReturnCode _lastReturnCode)
		{
			bool flag = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.TargetKnowledge.TargetAsPlayer.PrepareCombat();
			if (flag && base.NpcCharacter.IsAttached)
			{
				flag = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskAimAndShootWithRanged(true);
			}
			if (flag)
			{
				StartCharging();
				PerformRangedAttack(ProjectileData.EProjectileType.BowArrow, LogicMain.GameDefinition.BowArrowSpeed, base.NpcRuntimeData);
			}
			return (!flag) ? BehaviorReturnCode.Failure : BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionThrowDagger(BehaviorReturnCode _lastReturnCode)
		{
			if (base.NpcCharacter.IsAttached)
			{
				AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
				if (taskManager.CreateAnimationTaskDagger(base.NpcTreeHolder.NpcDelegateHolder.Knowledge.TargetKnowledge.TargetEntity.Id))
				{
					base.NpcRuntimeData.RangedCombat.RegistAsEventListener();
					StartCharging();
					return BehaviorReturnCode.Success;
				}
				return BehaviorReturnCode.Failure;
			}
			StartCharging();
			PerformDaggerAttack(base.NpcRuntimeData);
			return BehaviorReturnCode.Success;
		}

		public BehaviorReturnCode ActionStrafe(BehaviorReturnCode _lastReturnCode)
		{
			Profiling.Start("ActionStrafe", float.MaxValue);
			bool flag = false;
			BehaviorReturnCode behaviorReturnCode = BehaviorReturnCode.Failure;
			Character character = base.NpcTreeHolder.NpcDelegateHolder.Knowledge.TargetKnowledge.TargetEntity as Character;
			Vector3 headPosition = base.NpcCharacter.GetHeadPosition();
			Vector3 headPosition2 = character.GetHeadPosition();
			Vector3 offset = headPosition2 - headPosition;
			bool flag2 = CheckLineOfSight(headPosition, offset);
			if (base.NpcCharacter.IsAttached)
			{
				bool _blocked = false;
				flag = StrafeZ(headPosition, headPosition2, flag2, character.Id, ref _blocked);
				if (!_blocked)
				{
					flag |= StrafeX(headPosition, headPosition2, flag2, character.Id, ref _blocked);
				}
			}
			if (flag)
			{
				RestartKeepChargingTimer();
				SetAimRotationAutoStop(false);
				behaviorReturnCode = BehaviorReturnCode.Success;
			}
			else
			{
				SetAimRotationAutoStop(true);
				if (flag2)
				{
					behaviorReturnCode = BehaviorReturnCode.Success;
				}
				if (base.NpcCharacter.IsAttached)
				{
					IdleType idleType = base.NpcCharacter.CharacterDefinition.Definition.GetIdleType(ENpcIdleCategory.HighProfile);
					AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
					if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.Strafe)
					{
						taskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base);
					}
					taskManager.CreateAnimationTaskIdle(idleType, false, false);
				}
			}
			if (behaviorReturnCode == BehaviorReturnCode.Success)
			{
				AiHelper.RestartAlarmedActivity(base.NpcCharacter as EnemyCharacter);
			}
			Profiling.Stop();
			return behaviorReturnCode;
		}

		private void RestartKeepChargingTimer()
		{
			float duration = base.NpcCharacter.NpcStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseRangedChargingResetDuration);
			KeepChargingTimer.Start(duration);
		}

		public bool StrafeX(Vector3 _headPos, Vector3 _targetHeadPos, bool _lineOfsight, EntityId _targetId, ref bool _blocked)
		{
			Profiling.Start("StrafeX", float.MaxValue);
			bool flag = false;
			float _directionX = 0f;
			if (!_lineOfsight)
			{
				Vector3 vector = base.NpcCharacter.Rotation * Vector3.left;
				Vector3 vector2 = base.NpcCharacter.Rotation * Vector3.right;
				for (int i = 1; i <= 8; i++)
				{
					int strafeSign = 1;
					Vector3 baseDir = vector2;
					flag = CalcStrafeDirectionX(_headPos, _targetHeadPos, i, 8, strafeSign, 0.5f, baseDir, out _directionX, ref _blocked);
					if (flag)
					{
						break;
					}
					strafeSign = -1;
					baseDir = vector;
					flag = CalcStrafeDirectionX(_headPos, _targetHeadPos, i, 8, strafeSign, 0.5f, baseDir, out _directionX, ref _blocked);
					if (flag)
					{
						break;
					}
				}
			}
			if (flag)
			{
				AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
				flag = taskManager.CreateAnimationTaskStrafeX(_directionX, _targetId);
			}
			Profiling.Stop();
			return flag;
		}

		public bool StrafeZ(Vector3 _headPos, Vector3 _targetHeadPos, bool _lineOfsigth, EntityId _targetId, ref bool _blocked)
		{
			Profiling.Start("StrafeZ", float.MaxValue);
			bool result = false;
			Vector3 assumedStrafeDelta = _targetHeadPos - _headPos;
			float magnitude = assumedStrafeDelta.magnitude;
			float num = base.NpcCharacter.NpcDefinition.Definition.DetectionDistances[2];
			if (magnitude < num)
			{
				float num2 = base.NpcCharacter.NpcDefinition.Definition.DetectionDistances[1];
				float num3 = num2 * ((!_lineOfsigth) ? 0.5f : 1f);
				float b = num2 * ((!_lineOfsigth) ? 1.2f : 1.2f);
				float num4 = 0f;
				if (magnitude > num3)
				{
					num4 = 0.5f + 0.5f * Mathf.InverseLerp(num3, b, magnitude);
				}
				_blocked = false;
				if (num4 > MathHelper.Epsilon)
				{
					_blocked = CheckStrafeBlockedByNavMesh(assumedStrafeDelta);
					AnimationTaskManager taskManager = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager;
					result = !_blocked && taskManager.CreateAnimationTaskStrafeZ(num4, _targetId);
				}
			}
			Profiling.Stop();
			return result;
		}

		private bool CheckStrafeBlockedByNavMesh(Vector3 _assumedStrafeDelta)
		{
			Profiling.Start("CheckStrafeBlockedByNavMesh", float.MaxValue);
			_assumedStrafeDelta.y = 0f;
			_assumedStrafeDelta.Normalize();
			Vector3 position = base.NpcCharacter.Position;
			Vector3 vector = position + _assumedStrafeDelta * 0.5f;
			NavMeshHit hit;
			bool flag = !NavMesh.SamplePosition(vector, out hit, 1.5f, NavMeshLayers.WalkableLayerMaskPreset);
			if (!flag)
			{
				flag = vector.DistanceXZSquared(hit.position) > 0.0625f;
			}
			Profiling.Stop();
			return flag;
		}

		private bool CalcStrafeDirectionX(Vector3 _source, Vector3 _target, int _step, int _stepCount, int _strafeSign, float _mPerStep, Vector3 _baseDir, out float _directionX, ref bool _blocked)
		{
			Profiling.Start("CalcStrafeDirectionX", float.MaxValue);
			_directionX = 0f;
			Vector3 vector = _source + _baseDir * _step * _mPerStep;
			Vector3 offset = _target - vector;
			bool flag = CheckLineOfSight(vector, offset);
			if (flag)
			{
				_directionX = (float)_strafeSign * (0.5f + 0.5f * (float)_step / (float)_stepCount);
				_blocked = CheckStrafeBlockedByNavMesh(vector - _source);
			}
			Profiling.Stop();
			return flag && !_blocked && Mathf.Abs(_directionX) > MathHelper.Epsilon;
		}

		private static bool CheckLineOfSight(Vector3 _headPos, Vector3 _offset)
		{
			float num = _offset.magnitude;
			int layerMask = s_losRycastLayerMaskNear;
			if (num > 1.25f)
			{
				num -= 1.25f;
				layerMask = s_losRycastLayerMask;
			}
			return !Physics.Raycast(_headPos, _offset, num, layerMask);
		}

		private void SetAimRotationAutoStop(bool _autoStop)
		{
			if (base.NpcCharacter.IsAttached)
			{
				AnimationTaskAimWithCrossbow animationTaskAimWithCrossbow = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.UpperBody) as AnimationTaskAimWithCrossbow;
				if (animationTaskAimWithCrossbow != null)
				{
					animationTaskAimWithCrossbow.SetRotationAutoStop(_autoStop);
				}
			}
		}

		private void UnsheathRanged(CharacterDrawWeaponBase _drawRangedWeapon)
		{
			AiHelper.UnsheathWeapon(base.NpcCharacter, _drawRangedWeapon, EWeaponCategory.Ranged, true);
		}

		private CharacterDrawWeaponBase GetCharacterDrawWeapon()
		{
			ICharacterDrawRangedWeaponCarrier characterDrawRangedWeaponCarrier = base.NpcCharacter.CharacterStateMachines as ICharacterDrawRangedWeaponCarrier;
			return characterDrawRangedWeaponCarrier.CharacterDrawRangedWeapon;
		}

		private void StartCharging()
		{
			if (ChargingTimer.IsFinished)
			{
				float duration = base.NpcRuntimeData.NpcCharacter.NpcStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseRangedChargingDuration);
				ChargingTimer.Start(duration);
			}
			else if (!ChargingTimer.IsStarted)
			{
				float duration2 = base.NpcRuntimeData.NpcCharacter.NpcStats.Get_float(StatTypes.EStatType.Character_Conflict_BaseRangedFirstShotDuration);
				ChargingTimer.Start(duration2);
			}
		}

		private void CancelAimAndSheathRanged()
		{
			if (base.NpcCharacter.IsAttached)
			{
				AnimationTaskAimWithRangedBase currentTaskAimWithRanged = base.NpcCharacter.UnityComponentCache.AnimationController.TaskManager.GetCurrentTaskAimWithRanged();
				if (currentTaskAimWithRanged != null)
				{
					currentTaskAimWithRanged.StopAiming();
				}
			}
			ChargingTimer.Reset();
			base.NpcCharacter.ChooseWeaponState(EWeaponCategory.Ranged, EWeaponState.Sheathed);
		}

		private static void OnRangedAttackHit(Projectile _projectile, NpcCharacter _shooter)
		{
			EntityId entityId = _projectile.ProjectileStats.Get_EntityId(StatTypes.EStatType.Projectile_TargetId);
			PlayerCharacter playerCharacter = entityId.TryGetEntity<PlayerCharacter>();
			CombatHelper.ApplyRangedWeaponDamage(_shooter, playerCharacter);
			bool hasZeroHealth = playerCharacter.GetHasZeroHealth();
			if (hasZeroHealth && playerCharacter.GetIsImmortal())
			{
				playerCharacter.PlayerCharacterStateMachines.CharacterVitality.ChangeState(EStateType.Character_Alive);
			}
			if (playerCharacter.GetIsInParcour())
			{
				playerCharacter.Effects.OnEffectTrigger("bloodHitBack");
				int num = playerCharacter.PlayerStats.Get_int(StatTypes.EStatType.Player_ClimbingHitCount) + 1;
				playerCharacter.PlayerStats.Set_int(StatTypes.EStatType.Player_ClimbingHitCount, num);
				if (hasZeroHealth || ShouldFallDown(num, playerCharacter))
				{
					ParcourCancelEvent.ECancelType eCancelCancelType = (hasZeroHealth ? ParcourCancelEvent.ECancelType.Die : ParcourCancelEvent.ECancelType.Hit);
					Messenger.Instance.Broadcast(new ParcourCancelEvent(playerCharacter.Id, eCancelCancelType));
				}
				if (hasZeroHealth)
				{
					playerCharacter.UnityComponentCache.AnimationController.TaskManager.CreateAnimationTaskKnockdowned(null, playerCharacter, false, false);
				}
			}
			else if (hasZeroHealth || (!playerCharacter.GetIsNavigating() && !playerCharacter.GetIsInCombat()))
			{
				AnimationTaskManager taskManager = playerCharacter.UnityComponentCache.AnimationController.TaskManager;
				if (taskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) != AnimationTaskType.GetHit && taskManager.CreateAnimationTaskGetHit(_shooter, AttackMeleeType.Dagger))
				{
					PlayerMain playerMain = playerCharacter.AssassinCharacterStateMachines.PlayerMain;
					EStateType currentStateType = playerMain.CurrentStateType;
					if (currentStateType != EStateType.Generic_WaitingForAnim && playerMain.ScheduledStateType == EStateType.Invalid)
					{
						playerMain.EnterCallbackDelayState(EStateType.Generic_WaitingForAnimCancelable, currentStateType, currentStateType, AnimationTaskType.GetHit);
					}
				}
			}
			HumanoidDef.EArmorType armorType = HumanoidDef.EArmorType.LightLeather;
			EntityWrapperComponent component = playerCharacter.GameObject.GetComponent<EntityWrapperComponent>();
			if (component != null && component.Entity != null)
			{
				ICharacterDefinitionCarrier entity = component.GetEntity<Character>();
				if (entity != null)
				{
					HumanoidDef item = Main.SafeGameDb.GetItem<HumanoidDef>(entity.CharacterDefinition.Definition.HumanoidGameID);
					if (item != null)
					{
						armorType = item.ArmorType;
					}
				}
			}
			IAudioHandle handle = CombatController.Instance.AudioController.GetHandle(EAudioWeaponType.Arrow, armorType);
			if (handle != null)
			{
				handle.Load();
				handle.Play(false, playerCharacter.GameObject.GetComponent<AudioSource>());
				IAudioHandle audioHandle = AudioManager.Instance.CreateHandle(UbiCore.Utils.Singleton<AudioGameIds>.Instance.ac_vo_player_takehit, ELoadStrategy.LoadNext, EStoppedFinishedStrategy.UnloadCurrent);
				audioHandle.Load();
				audioHandle.Play(false, null, 0.1f);
			}
		}

		private static void PerformDaggerAttack(NpcRuntimeData _npcRuntimeData)
		{
			PerformRangedAttack(ProjectileData.EProjectileType.Dagger, LogicMain.GameDefinition.DefaultDaggerSpeed, _npcRuntimeData);
		}

		private static void PerformRangedAttack(ProjectileData.EProjectileType _type, float _speed, NpcRuntimeData _npcRuntimeData)
		{
			NpcCharacter npcCharacter = _npcRuntimeData.NpcCharacter;
			npcCharacter.CancelMovement(false);
			bool flag = CombatHelper.DecideToDodge(npcCharacter, LogicMain.PlayerEntity);
			Projectile projectile = ProjectileHelper.SpawnProjectile(_type, npcCharacter, LogicMain.PlayerEntity.Id, _speed, flag);
			if (!flag)
			{
				projectile.ProjectileStateMachines.ProjectileFlight.HitAction = _npcRuntimeData.RangedCombat.OnRangedAttackHit;
			}
		}

		private static bool ShouldFallDown(int _climbingHitCount, PlayerCharacter _targetAsPlayer)
		{
			AnimationTaskClimb animationTaskClimb = _targetAsPlayer.UnityComponentCache.AnimationController.TaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base) as AnimationTaskClimb;
			if (animationTaskClimb != null)
			{
				if (animationTaskClimb.HangingAtEdge)
				{
					return true;
				}
				if (_climbingHitCount > 1)
				{
					return true;
				}
			}
			return false;
		}
	}
}
