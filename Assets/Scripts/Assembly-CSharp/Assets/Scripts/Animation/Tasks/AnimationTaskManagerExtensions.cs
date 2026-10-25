using System;
using System.Collections.ObjectModel;
using AcierProtocol;
using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic;
using Assets.Scripts.Mission.Logic.Combat;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using UbiCore.Messenger;
using UnityEngine;

namespace Assets.Scripts.Animation.Tasks
{
	public static class AnimationTaskManagerExtensions
	{
		private const int SeduceAnimCount = 3;

		public static bool CreateAnimationTaskDagger(this AnimationTaskManager _animationTaskManager, EntityId _targetEntityId)
		{
			AnimationTaskDagger.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskDagger.ParametersTemplate.TargetEntityId = _targetEntityId;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskDagger>(AnimationTaskType.Dagger);
		}

		public static bool CreateAnimationTaskDoctor(this AnimationTaskManager _animationTaskManager, AnimationTaskDoctor.EType _type, EntityId _targetEntityId)
		{
			AnimationTaskDoctor.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskDoctor.ParametersTemplate.Type = _type;
			AnimationTaskDoctor.ParametersTemplate.TargetEntityId = _targetEntityId;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskDoctor>(AnimationTaskType.Doctor);
		}

		public static bool CreateAnimationTaskDeployBomb(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskDeployBomb.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskDeployBomb>(AnimationTaskType.DeployBomb);
		}

		public static bool CreateAnimationTaskThrowSand(this AnimationTaskManager _animationTaskManager, IPositionProvider _targetProvider)
		{
			_animationTaskManager.CancelCurrentTaskForced(AnimationTaskPlayerTurning.Properties.Layer);
			AnimationTaskPlayerTurning.Parameters parametersTemplate = AnimationTaskPlayerTurning.ParametersTemplate;
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			parametersTemplate.TargetProvider = _targetProvider;
			_animationTaskManager.AddAnimationTask(new AnimationTaskPlayerTurning());
			AnimationTaskThrowSand.Parameters parametersTemplate2 = AnimationTaskThrowSand.ParametersTemplate;
			parametersTemplate2.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskThrowSand());
			return true;
		}

		public static bool CreateAnimationTaskThrowSandReaction(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskThrowSandReaction.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskThrowSandReaction>(AnimationTaskType.ThrowSandReaction);
		}

		public static bool CreateAnimationTaskUnsheathWeapon(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskUnsheathWeapon.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskUnsheathWeapon>(AnimationTaskType.UnsheathWeapon);
		}

		public static bool CreateAnimationTaskSheathWeapon(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskSheathWeapon.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskSheathWeapon>(AnimationTaskType.SheathWeapon);
		}

		public static bool CreateAnimationTaskUnsheathBowForcePlay(this AnimationTaskManager _animationTaskManager)
		{
			AnimatorStateMachineStatic.StateHashValue state_UnsheathBow = AnimationStateHashes.State_UnsheathBow;
			bool flag = _animationTaskManager.CreateAnimationTaskUnsheathBow();
			if (flag)
			{
				_animationTaskManager.AnimationController.Animator.Play(state_UnsheathBow.NameHash);
				GetRangedAnimator(_animationTaskManager).Play(AnimationStateHashes.BowAnimationStateHashes.State_UnsheathBow.NameHash);
			}
			return flag;
		}

		public static bool CreateAnimationTaskUnsheathBow(this AnimationTaskManager _animationTaskManager)
		{
			bool flag = _animationTaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base) == AnimationTaskType.UnsheathBow;
			if (!flag && _animationTaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.Base))
			{
				AnimationTaskUnsheathBow.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskUnsheathBow.ParametersTemplate.BowAnimator = GetRangedAnimator(_animationTaskManager);
				_animationTaskManager.AddAnimationTask(new AnimationTaskUnsheathBow());
				flag = true;
			}
			return flag;
		}

		public static bool CreateAnimationTaskSheathBow(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskSheathBow.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskSheathBow.ParametersTemplate.BowAnimator = GetRangedAnimator(_animationTaskManager);
			return _animationTaskManager.CreateAnimationTask<AnimationTaskSheathBow>(AnimationTaskType.SheathBow);
		}

		public static bool CreateAnimationTaskUnsheathCrossbowForcePlay(this AnimationTaskManager _animationTaskManager)
		{
			AnimatorStateMachineStatic.StateHashValue state_UnsheathCrossbowWithoutAim = AnimationStateHashes.State_UnsheathCrossbowWithoutAim;
			bool flag = _animationTaskManager.CreateAnimationTaskUnsheathCrossbow();
			if (flag)
			{
				_animationTaskManager.AnimationController.Animator.Play(state_UnsheathCrossbowWithoutAim.NameHash);
				GetRangedAnimator(_animationTaskManager).Play(AnimationStateHashes.CrossbowAnimationStateHashes.State_LoadedCrossbow.NameHash);
			}
			return flag;
		}

		public static bool CreateAnimationTaskUnsheathCrossbow(this AnimationTaskManager _animationTaskManager)
		{
			bool flag = _animationTaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.UpperBody) == AnimationTaskType.UnsheathCrossbow;
			if (!flag && _animationTaskManager.CancelCurrentTask(AnimationTaskManager.ELayer.UpperBody))
			{
				AnimationTaskUnsheathCrossbow.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskUnsheathCrossbow.ParametersTemplate.AttackRangeType = AttackRangeType.WeaponReady;
				_animationTaskManager.AddAnimationTask(new AnimationTaskUnsheathCrossbow());
				flag = true;
			}
			return flag;
		}

		public static bool CreateAnimationTaskSheathCrossbow(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskSheathCrossbow.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskSheathCrossbow>(AnimationTaskType.SheathCrossbow);
		}

		public static bool CreateAnimationTaskAimWithRangedForcePlay(this AnimationTaskManager _animationTaskManager)
		{
			bool flag = _animationTaskManager.CreateAnimationTaskAimWithRanged();
			if (flag)
			{
				WeaponHandler weaponHandler = _animationTaskManager.GetWeaponHandler(EWeaponCategory.Ranged);
				WeaponHandler.WeaponDesc currentWeaponDesc = weaponHandler.CurrentWeaponDesc;
				_animationTaskManager.AnimationController.Animator.Play(currentWeaponDesc.AimState.NameHash);
				GetRangedAnimator(_animationTaskManager).Play(currentWeaponDesc.AimStateWeapon.NameHash);
			}
			return flag;
		}

		public static bool CreateAnimationTaskAimWithRanged(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskAimWithRangedBase currentTaskAimWithRanged = _animationTaskManager.GetCurrentTaskAimWithRanged();
			if (currentTaskAimWithRanged != null)
			{
				currentTaskAimWithRanged.RestartAiming();
			}
			return _animationTaskManager.CreateAnimationTaskAimAndShootWithRanged(false);
		}

		public static bool CreateAnimationTaskAimAndShootWithRanged(this AnimationTaskManager _animationTaskManager, bool _shoot)
		{
			WeaponHandler weaponHandler = _animationTaskManager.GetWeaponHandler(EWeaponCategory.Ranged);
			AnimationTaskAimWithRangedBase.Parameters parametersTemplate;
			AnimationTaskType arg;
			Func<AnimationTaskType, bool, bool> func;
			switch (WeaponHandler.WeaponTypesToType(weaponHandler.CurrentWeaponDesc.Type))
			{
			case EWeaponType.Bow:
				parametersTemplate = AnimationTaskAimWithBow.ParametersTemplate;
				arg = AnimationTaskType.AimWithBow;
				func = _animationTaskManager.CreateAnimationTask<AnimationTaskAimWithBow>;
				break;
			case EWeaponType.Crossbow:
				parametersTemplate = AnimationTaskAimWithCrossbow.ParametersTemplate;
				arg = AnimationTaskType.AimWithCrossbow;
				func = _animationTaskManager.CreateAnimationTask<AnimationTaskAimWithCrossbow>;
				break;
			default:
				return false;
			}
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			parametersTemplate.WeaponAnimator = weaponHandler.Animator;
			parametersTemplate.TargetEntity = LogicMain.PlayerEntity;
			if (func(arg, false))
			{
				return !_shoot || _animationTaskManager.GetCurrentTaskAimWithRanged().Shoot();
			}
			return false;
		}

		public static bool CreateAnimationTaskHealAtDoctor(this AnimationTaskManager _animationTaskManager, EntityId _targetEntityId)
		{
			AnimationTaskHealAtDoctor.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskHealAtDoctor.ParametersTemplate.TargetEntityId = _targetEntityId;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskHealAtDoctor>(AnimationTaskType.PlayerHealAtDoctor);
		}

		public static bool CreateAnimationTaskStunned(this AnimationTaskManager _animationTaskManager, Character _target, bool _fromGround)
		{
			AnimationTaskStunned.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskStunned.ParametersTemplate.Target = _target;
			AnimationTaskStunned.ParametersTemplate.FromGround = _fromGround;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskStunned>(AnimationTaskType.Stunned);
		}

		public static bool CreateAnimationTaskStunningStrikeReaction(this AnimationTaskManager _animationTaskManager, Character _target)
		{
			AnimationTaskStunningStrikeReaction.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskStunningStrikeReaction.ParametersTemplate.Target = _target;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskStunningStrikeReaction>(AnimationTaskType.StunningStrikeReaction);
		}

		public static bool CreateAnimationTaskKnockdowned(this AnimationTaskManager _animationTaskManager, IPositionProvider _source, Character _target, bool _forceToGround, bool _cancelCurrentTask = true)
		{
			AnimationTaskKnockdowned.Parameters parametersTemplate = AnimationTaskKnockdowned.ParametersTemplate;
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			parametersTemplate.Source = _source;
			parametersTemplate.Target = _target;
			parametersTemplate.ForceToGround = _forceToGround;
			if (_cancelCurrentTask)
			{
				return _animationTaskManager.CreateAnimationTask<AnimationTaskKnockdowned>(AnimationTaskType.Knockdowned);
			}
			_animationTaskManager.AddAnimationTask(new AnimationTaskKnockdowned());
			return true;
		}

		public static bool CreateAnimationTaskSmokeBombReaction(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskSmokeBombReaction.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskSmokeBombReaction>(AnimationTaskType.SmokeBombReaction);
		}

		public static bool CreateAnimationTaskBalance(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskBalance.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskBalance.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskBalance.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskBalance());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskCombat(this AnimationTaskManager _animationTaskManager, IParticipantCarrier _character)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskCombat.Properties.Layer;
			if (_animationTaskManager.GetTaskCount(layer) > 0)
			{
				AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(layer);
				if (currentTask.TaskType == AnimationTaskType.Combat)
				{
					AnimationTaskCombat animationTaskCombat = currentTask as AnimationTaskCombat;
					if (animationTaskCombat != null && animationTaskCombat.Participant == _character.CombatParticipantProxy.CombatParticipant)
					{
						return false;
					}
				}
			}
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskCombat.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskCombat.ParametersTemplate.Participant = _character.CombatParticipantProxy.CombatParticipant;
				_animationTaskManager.AddAnimationTask(new AnimationTaskCombat());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskDead(this AnimationTaskManager _animationTaskManager)
		{
			ReadOnlyCollection<AnimationTaskBase> currentTasks = _animationTaskManager.GetCurrentTasks(AnimationTaskDead.Properties.Layer);
			for (int i = 0; i < currentTasks.Count; i++)
			{
				if (currentTasks[i].TaskType == AnimationTaskType.Dead)
				{
					return false;
				}
			}
			AnimationTaskDead.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskDead());
			_animationTaskManager.AnimationController.Animator.SetLayerWeight(1, 0f);
			return true;
		}

		public static void CreateAnimationTaskMoveToForcePlay(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, float _speed)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskMoveTo.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				Animator animator = _animationTaskManager.AnimationController.Animator;
				animator.SetCurrentGeneralState(GeneralState.Default);
				animator.SetCurrentIdleType(IdleType.Neutral);
				animator.SetCurrentMovementState(MovementState.Movement);
				animator.Play(AnimationStateHashes.State_WalkRun_Footl.NameHash, 0, UnityEngine.Random.Range(0f, 0.9f));
				AnimationTaskMoveTo.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskMoveTo.ParametersTemplate.Path = _path;
				AnimationTaskMoveTo.ParametersTemplate.Speed = _speed;
				AnimationTaskMoveTo.ParametersTemplate.Accelerate = false;
				AnimationTaskMoveTo.ParametersTemplate.ResetAgentOnCancel = true;
				AnimationTaskMoveTo.ParametersTemplate.StoppingDistanceSq = float.MaxValue;
				AnimationTaskMoveTo.ParametersTemplate.StartIdleWhenFinishedPolicy = EStartIdleAnimAfterTaskPolicy.DontCare;
				AnimationTaskMoveTo.ParametersTemplate.LayerMask = _animationTaskManager.AnimationController.EntityId.GetEntity<Character>().GetAutoPassableNavMeshLayerMask();
				_animationTaskManager.AddAnimationTask(new AnimationTaskMoveTo());
			}
		}

		public static void CreateAnimationTaskMoveTo(this AnimationTaskManager _animationTaskManager, MoveToEvent _eventMoveTo)
		{
			_animationTaskManager.CreateAnimationTaskMoveTo(_eventMoveTo.Path, -1f);
		}

		public static void CreateAnimationTaskMoveTo(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, float _speed)
		{
			_animationTaskManager.CreateAnimationTaskMoveTo(_path, _speed, true, EStartIdleAnimAfterTaskPolicy.DontCare);
		}

		public static void CreateAnimationTaskMoveTo(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, float _speed, bool _resetAgentOnCancel, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, bool _accelerate = true, float _stoppingDistanceSq = float.MaxValue, int _layerMask = 0)
		{
			bool flag = true;
			AnimationTaskManager.ELayer layer = AnimationTaskMoveTo.Properties.Layer;
			if (_animationTaskManager.GetTaskCount(layer) > 0)
			{
				AnimationTaskMoveTo animationTaskMoveTo = null;
				AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(layer);
				if (currentTask.TaskType == AnimationTaskType.MoveTo)
				{
					animationTaskMoveTo = (AnimationTaskMoveTo)currentTask;
					animationTaskMoveTo.OnUpdateTarget(_path, _speed, _resetAgentOnCancel, _startIdleWhenFinishedPolicy, true, _stoppingDistanceSq, _layerMask);
					flag = false;
				}
			}
			if (flag && _animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskMoveTo.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskMoveTo.ParametersTemplate.Path = _path;
				AnimationTaskMoveTo.ParametersTemplate.Accelerate = _accelerate;
				AnimationTaskMoveTo.ParametersTemplate.Speed = _speed;
				AnimationTaskMoveTo.ParametersTemplate.ResetAgentOnCancel = _resetAgentOnCancel;
				AnimationTaskMoveTo.ParametersTemplate.StoppingDistanceSq = _stoppingDistanceSq;
				AnimationTaskMoveTo.ParametersTemplate.StartIdleWhenFinishedPolicy = _startIdleWhenFinishedPolicy;
				AnimationTaskMoveTo.ParametersTemplate.LayerMask = ((_layerMask != 0) ? _layerMask : _animationTaskManager.AnimationController.EntityId.GetEntity<Character>().GetAutoPassableNavMeshLayerMask());
				_animationTaskManager.AddAnimationTask(new AnimationTaskMoveTo());
			}
		}

		public static void CreateAnimationTaskPlayerMoveTo(this AnimationTaskManager _animationTaskManager, MoveToEvent _eventMoveTo)
		{
			_animationTaskManager.CreateAnimationTaskPlayerMoveTo(_eventMoveTo.Path, _eventMoveTo.SpeedFactor, true, EStartIdleAnimAfterTaskPolicy.DontCare, float.MaxValue, _eventMoveTo.EnableFacing, _eventMoveTo.FacingDirection);
		}

		public static void CreateAnimationTaskPlayerMoveTo(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, float _speedFactor)
		{
			_animationTaskManager.CreateAnimationTaskPlayerMoveTo(_path, _speedFactor, true, EStartIdleAnimAfterTaskPolicy.DontCare, float.MaxValue, false, Vector3.zero);
		}

		public static void CreateAnimationTaskPlayerMoveTo(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, float _speedFactor, bool _resetAgentOnCancel)
		{
			_animationTaskManager.CreateAnimationTaskPlayerMoveTo(_path, _speedFactor, _resetAgentOnCancel, EStartIdleAnimAfterTaskPolicy.DontCare, float.MaxValue, false, Vector3.zero);
		}

		public static void CreateAnimationTaskPlayerMoveTo(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, float _speedFactor, bool _resetAgentOnCancel, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, float _stoppingDistanceSq, bool _enableFacing, Vector3 _facingDirection)
		{
			AnimationController animationController = _animationTaskManager.AnimationController;
			float speed;
			if (_speedFactor >= 0f)
			{
				float a = animationController.CalculateSpeed(EMovementSpeed.Walk);
				float b = animationController.CalculateSpeed(animationController.ReferenceMovementSpeed);
				speed = Mathf.Lerp(a, b, _speedFactor);
			}
			else
			{
				speed = animationController.CalculateSpeed(animationController.ReferenceMovementSpeed);
			}
			_animationTaskManager.CreateAnimationTaskPlayerMoveTo(_path, _resetAgentOnCancel, _startIdleWhenFinishedPolicy, _stoppingDistanceSq, _enableFacing, _facingDirection, speed, _animationTaskManager.AnimationController.EntityId.GetEntity<Character>().GetAutoPassableNavMeshLayerMask());
		}

		public static void CreateAnimationTaskPlayerMoveTo(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, bool _resetAgentOnCancel, EStartIdleAnimAfterTaskPolicy _startIdleWhenFinishedPolicy, float _stoppingDistanceSq, bool _enableFacing, Vector3 _facingDirection, float _speed, int _layerMask)
		{
			bool flag = true;
			AnimationTaskManager.ELayer layer = AnimationTaskPlayerMoveTo.Properties.Layer;
			if (_animationTaskManager.GetTaskCount(layer) > 0)
			{
				AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(layer);
				if (currentTask.TaskType == AnimationTaskType.PlayerMoveTo)
				{
					AnimationTaskPlayerMoveTo animationTaskPlayerMoveTo = (AnimationTaskPlayerMoveTo)currentTask;
					animationTaskPlayerMoveTo.OnUpdateTarget(_path, _speed, _enableFacing, _facingDirection, _resetAgentOnCancel, _startIdleWhenFinishedPolicy, _stoppingDistanceSq, _layerMask);
					flag = false;
				}
			}
			if (flag && _animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskPlayerMoveTo.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskPlayerMoveTo.ParametersTemplate.Path = _path;
				AnimationTaskPlayerMoveTo.ParametersTemplate.Speed = _speed;
				AnimationTaskPlayerMoveTo.ParametersTemplate.EnableFacing = _enableFacing;
				AnimationTaskPlayerMoveTo.ParametersTemplate.FacingDirection = _facingDirection;
				AnimationTaskPlayerMoveTo.ParametersTemplate.ResetAgentOnCancel = _resetAgentOnCancel;
				AnimationTaskPlayerMoveTo.ParametersTemplate.StoppingDistanceSq = _stoppingDistanceSq;
				AnimationTaskPlayerMoveTo.ParametersTemplate.StartIdleWhenFinishedPolicy = _startIdleWhenFinishedPolicy;
				AnimationTaskPlayerMoveTo.ParametersTemplate.LayerMask = _layerMask;
				_animationTaskManager.AddAnimationTask(new AnimationTaskPlayerMoveTo());
			}
		}

		public static bool CreateAnimationTaskStrafeX(this AnimationTaskManager _animationTaskManager, float _directionX, EntityId _targetId)
		{
			AnimationTaskStrafe animationTaskStrafe = _animationTaskManager.CreateAnimationTaskStrafe(_targetId);
			if (animationTaskStrafe != null)
			{
				animationTaskStrafe.OnUpdateDirectionX(_directionX);
			}
			return animationTaskStrafe != null;
		}

		public static bool CreateAnimationTaskStrafeZ(this AnimationTaskManager _animationTaskManager, float _directionZ, EntityId _targetId)
		{
			AnimationTaskStrafe animationTaskStrafe = _animationTaskManager.CreateAnimationTaskStrafe(_targetId);
			if (animationTaskStrafe != null)
			{
				animationTaskStrafe.OnUpdateDirectionZ(_directionZ);
			}
			return animationTaskStrafe != null;
		}

		public static bool CreateAnimationTaskClimb(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskClimb.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskClimb.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskClimb.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskClimb());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskClimb(this AnimationTaskManager _animationTaskManager, JumpHorizontalToWallEvent _event)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskClimb.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskClimb.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskClimb.ParametersTemplate.OffMeshLink = new OffMeshLinkEntity(_event.Route, _event.StartPosition, _event.TargetPosition);
				_animationTaskManager.AddAnimationTask(new AnimationTaskClimb());
				result = true;
			}
			return result;
		}

		public static void CreateAnimationTaskIdleForcePlay(this AnimationTaskManager _animationTaskManager)
		{
			_animationTaskManager.CreateAnimationTaskIdleForcePlay(IdleType.Neutral);
		}

		public static void CreateAnimationTaskIdleForcePlay(this AnimationTaskManager _animationTaskManager, IdleType _idleType)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskIdle.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimatorStateMachineStatic.StateHashValue stateHashValue = _idleType.GetStateHashValue();
				Animator animator = _animationTaskManager.AnimationController.Animator;
				animator.SetCurrentGeneralState(GeneralState.Default);
				animator.SetCurrentMovementState(MovementState.Idle);
				animator.SetCurrentIdleType(_idleType);
				animator.Play(stateHashValue.NameHash, 0, UnityEngine.Random.Range(0f, 0.9f));
				AnimationTaskIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskIdle.ParametersTemplate.IdleType = _idleType;
				AnimationTaskIdle.ParametersTemplate.IdleTypeOnTransition = IdleType.Invalid;
				AnimationTaskIdle.ParametersTemplate.CrossFadeToState = new AnimatorStateMachineStatic.StateHashValue();
				AnimationTaskIdle.ParametersTemplate.CrossFadeDuration = 0f;
				AnimationTaskIdle.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager);
				_animationTaskManager.AddAnimationTask(new AnimationTaskIdle());
			}
		}

		public static bool CreateAnimationTaskNpcTurningAndIdle(this AnimationTaskManager _animationTaskManager, IdleType _idleType, Vector3 _lookAt)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskIdle.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AddNpcTurning(_animationTaskManager, _lookAt, AnimationTaskNpcTurning.TurnAccuracy.High);
				IdleType idleTypeOnTransition = _animationTaskManager.GetIdleTypeOnTransition();
				AnimationTaskIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskIdle.ParametersTemplate.IdleType = _idleType;
				AnimationTaskIdle.ParametersTemplate.IdleTypeOnTransition = idleTypeOnTransition;
				AnimationTaskIdle.ParametersTemplate.CrossFadeToState = new AnimatorStateMachineStatic.StateHashValue();
				AnimationTaskIdle.ParametersTemplate.CrossFadeDuration = 0f;
				AnimationTaskIdle.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager);
				_animationTaskManager.AddAnimationTask(new AnimationTaskIdle());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskNpcTurningAndIdle(this AnimationTaskManager _animationTaskManager, IdleType _idleType, float _targetAngle)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskIdle.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AddNpcTurning(_animationTaskManager, _targetAngle, AnimationTaskNpcTurning.TurnAccuracy.High);
				IdleType idleTypeOnTransition = _animationTaskManager.GetIdleTypeOnTransition();
				AnimationTaskIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskIdle.ParametersTemplate.IdleType = _idleType;
				AnimationTaskIdle.ParametersTemplate.IdleTypeOnTransition = idleTypeOnTransition;
				AnimationTaskIdle.ParametersTemplate.CrossFadeToState = new AnimatorStateMachineStatic.StateHashValue();
				AnimationTaskIdle.ParametersTemplate.CrossFadeDuration = 0f;
				AnimationTaskIdle.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager);
				_animationTaskManager.AddAnimationTask(new AnimationTaskIdle());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskIdle(this AnimationTaskManager _animationTaskManager, IdleType _idleType, bool _startWithCrossFade = false, bool _cancel = true, float _crossFadeDuration = 0f)
		{
			IdleType idleTypeOnTransition = _animationTaskManager.GetIdleTypeOnTransition();
			bool result = false;
			bool flag = true;
			AnimationTaskManager.ELayer layer = AnimationTaskIdle.Properties.Layer;
			if (_animationTaskManager.GetTaskCount(layer) > 0)
			{
				AnimationTaskIdle animationTaskIdle = null;
				AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(layer);
				if (currentTask.TaskType == AnimationTaskType.Idle && !_startWithCrossFade)
				{
					animationTaskIdle = (AnimationTaskIdle)currentTask;
					animationTaskIdle.OnUpdateIdleType(_idleType, idleTypeOnTransition);
					flag = false;
					result = true;
				}
				else if (_startWithCrossFade)
				{
					while (_animationTaskManager.GetTaskCount(layer) > 0)
					{
						_animationTaskManager.CancelCurrentTaskForced(layer);
					}
				}
			}
			if (_startWithCrossFade || (flag && _animationTaskManager.GetTaskCount(layer) <= 0) || (_cancel && _animationTaskManager.CancelCurrentTask(layer)))
			{
				AnimationTaskIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskIdle.ParametersTemplate.IdleType = _idleType;
				AnimationTaskIdle.ParametersTemplate.IdleTypeOnTransition = idleTypeOnTransition;
				if (_startWithCrossFade)
				{
					_animationTaskManager.CancelCurrentTaskForced(AnimationTaskManager.ELayer.Base);
					_animationTaskManager.CancelCurrentTaskForced(AnimationTaskManager.ELayer.UpperBody);
					AnimatorStateMachineStatic.StateHashValue stateHashStatic = CharacterData.GetStateHashStatic(_animationTaskManager.GetIdleTypeOnTransition());
					AnimationTaskIdle.ParametersTemplate.CrossFadeToState = stateHashStatic;
					AnimationTaskIdle.ParametersTemplate.CrossFadeDuration = _crossFadeDuration;
				}
				else
				{
					AnimationTaskIdle.ParametersTemplate.CrossFadeToState = new AnimatorStateMachineStatic.StateHashValue();
					AnimationTaskIdle.ParametersTemplate.CrossFadeDuration = 0f;
				}
				AnimationTaskIdle.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager);
				_animationTaskManager.AddAnimationTask(new AnimationTaskIdle());
				result = true;
			}
			return result;
		}

		public static void CreateAnimationTaskIdle(this AnimationTaskManager _animationTaskManager, bool _startWithCrossfade, float _crossFadeDuration = 0.5f)
		{
			IdleType idleType = ((_animationTaskManager.AnimationController.ReferenceIdleType != IdleType.Invalid) ? _animationTaskManager.AnimationController.ReferenceIdleType : ((_animationTaskManager.AnimationController.CharacterDefinition != null) ? _animationTaskManager.AnimationController.CharacterDefinition.DefaultIdleType : IdleType.Neutral));
			_animationTaskManager.CreateAnimationTaskIdle(idleType, _startWithCrossfade, true, _crossFadeDuration);
		}

		public static void CreateAnimationTaskIdle(this AnimationTaskManager _animationTaskManager)
		{
			_animationTaskManager.CreateAnimationTaskIdle(false, 0f);
		}

		public static void CreateAnimationTaskIdleWithCrossfade(this AnimationTaskManager _animationTaskManager, float _crossFadeDuration)
		{
			_animationTaskManager.CreateAnimationTaskIdle(true, _crossFadeDuration);
		}

		public static bool CreateAnimationTaskJumpDown(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskJumpDown.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskJumpDown.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskJumpDown.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskJumpDown());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskRunPassOver(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskRunPassOver.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskRunPassOver.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskRunPassOver.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskRunPassOver());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskRunPassOverDown(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskRunPassOverDown.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskRunPassOverDown.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskRunPassOverDown.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskRunPassOverDown());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskParkourSequence(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskParkourSequence.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskParkourSequence.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskParkourSequence.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskParkourSequence());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskLeapOfFaith(this AnimationTaskManager _animationTaskManager, Haystack _haystack)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskLeapOfFaith.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskLeapOfFaith.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskLeapOfFaith.ParametersTemplate.Haystack = _haystack;
				_animationTaskManager.AddAnimationTask(new AnimationTaskLeapOfFaith());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskJumpDownAnywhere(this AnimationTaskManager _animationTaskManager, JumpAnywhereEvent _event)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskJumpDownAnywhere.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskJumpDownAnywhere.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskJumpDownAnywhere.ParametersTemplate.JumpAnywhereEvent = _event;
				_animationTaskManager.AddAnimationTask(new AnimationTaskJumpDownAnywhere());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskJumpHorizontal(this AnimationTaskManager _animationTaskManager, OffMeshLinkEntity _entity)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskJumpHorizontal.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskJumpHorizontal.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskJumpHorizontal.ParametersTemplate.OffMeshLink = _entity;
				_animationTaskManager.AddAnimationTask(new AnimationTaskJumpHorizontal());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskClimbRebound(this AnimationTaskManager _animationTaskManager, ClimbReboundEvent _event)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskClimbRebound.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskClimbRebound.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskClimbRebound.ParametersTemplate.ClimbReboundEvent = _event;
				_animationTaskManager.AddAnimationTask(new AnimationTaskClimbRebound());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskBenchEnter(this AnimationTaskManager _animationTaskManager, Bench _bench, InteractionLocators.Locator _selectedLocator)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskBenchEnter.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskBenchEnter.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskBenchEnter.ParametersTemplate.Bench = _bench;
				AnimationTaskBenchEnter.ParametersTemplate.SelectedLocator = _selectedLocator;
				_animationTaskManager.AddAnimationTask(new AnimationTaskBenchEnter());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskBenchExit(this AnimationTaskManager _animationTaskManager, bool _startRunning, Vector3 _destination)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskBenchExit.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskBenchExit.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskBenchExit.ParametersTemplate.StartRunning = _startRunning;
				AnimationTaskBenchExit.ParametersTemplate.Destination = _destination;
				_animationTaskManager.AddAnimationTask(new AnimationTaskBenchExit());
				result = true;
			}
			return result;
		}

		public static void CreateAnimationTaskBenchIdle(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskBenchIdle.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskBenchIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskBenchIdle());
		}

		public static void CreateAnimationTaskNpcBenchIdle(this AnimationTaskManager _animationTaskManager, Bench.Seat _seat)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskNpcBenchIdle.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskNpcBenchIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskNpcBenchIdle.ParametersTemplate.Seat = _seat;
			_animationTaskManager.AddAnimationTask(new AnimationTaskNpcBenchIdle());
		}

		public static bool CreateAnimationTaskRooftopGardenEnter(this AnimationTaskManager _animationTaskManager, RooftopGarden _rooftopGarden, InteractionLocators.Locator _selectedLocator)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskRooftopGardenEnter.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskRooftopGardenEnter.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskRooftopGardenEnter.ParametersTemplate.Hideout = _rooftopGarden;
				AnimationTaskRooftopGardenEnter.ParametersTemplate.SelectedLocator = _selectedLocator;
				_animationTaskManager.AddAnimationTask(new AnimationTaskRooftopGardenEnter());
				result = true;
			}
			return result;
		}

		public static void CreateAnimationTaskRooftopGardenIdle(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHayStackIdle.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskRooftopGardenIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskRooftopGardenIdle());
		}

		public static bool CreateAnimationTaskRooftopGardenExit(this AnimationTaskManager _animationTaskManager, HideoutObject _hideout, InteractionLocators.Locator _locator, Vector3 _destination, bool _startRunning)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskRooftopGardenExit.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskRooftopGardenExit.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskRooftopGardenExit.ParametersTemplate.Destination = _destination;
				AnimationTaskRooftopGardenExit.ParametersTemplate.Hideout = _hideout;
				AnimationTaskRooftopGardenExit.ParametersTemplate.Locator = _locator;
				AnimationTaskRooftopGardenExit.ParametersTemplate.StartRunning = _startRunning;
				_animationTaskManager.AddAnimationTask(new AnimationTaskRooftopGardenExit());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskHayStackEnter(this AnimationTaskManager _animationTaskManager, HideoutObject _hayStack, InteractionLocators.Locator _selectedLocator)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskHayStackEnter.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskHayStackEnter.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskHayStackEnter.ParametersTemplate.Hideout = _hayStack;
				AnimationTaskHayStackEnter.ParametersTemplate.SelectedLocator = _selectedLocator;
				_animationTaskManager.AddAnimationTask(new AnimationTaskHayStackEnter());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskHayStackExit(this AnimationTaskManager _animationTaskManager, Vector3 _destination, HideoutObject _hideout, bool _startRunning)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskHayStackExit.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskHayStackExit.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskHayStackExit.ParametersTemplate.Destination = _destination;
				AnimationTaskHayStackExit.ParametersTemplate.Hideout = _hideout;
				AnimationTaskHayStackExit.ParametersTemplate.StartRunning = _startRunning;
				_animationTaskManager.AddAnimationTask(new AnimationTaskHayStackExit());
				result = true;
			}
			return result;
		}

		public static void CreateAnimationTaskHayStackIdle(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHayStackIdle.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskHayStackIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskHayStackIdle());
		}

		public static bool CreateAnimationTaskCodex(this AnimationTaskManager _animationTaskManager, Codex _codex, InteractionLocators.Locator _selectedLocator)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskCodex.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskCodex.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskCodex.ParametersTemplate.Codex = _codex;
				AnimationTaskCodex.ParametersTemplate.SelectedLocator = _selectedLocator;
				_animationTaskManager.AddAnimationTask(new AnimationTaskCodex());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskCapturedNpc(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskCapturedNpc.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskCapturedNpc.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskCapturedNpc());
			return true;
		}

		public static bool CreateAnimationTaskPigeonDelivery(this AnimationTaskManager _animationTaskManager, Pigeonry _pigeonry)
		{
			AnimationTaskPigeonDelivery.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskPigeonDelivery.ParametersTemplate.PigeonryId = _pigeonry.Id;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskPigeonDelivery>(AnimationTaskType.PigeonDelivery);
		}

		public static bool CreateAnimationTaskMissionDeliverInstigator(this AnimationTaskManager _animationTaskManager, EntityId _instigatorId, EntityId _targetId)
		{
			AnimationTaskMissionDeliverInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskMissionDeliverInstigator>(AnimationTaskType.MissionDeliverInstigator);
		}

		public static bool CreateAnimationTaskMissionDeliverTarget(this AnimationTaskManager _animationTaskManager, EntityId _instigatorId, EntityId _targetId)
		{
			AnimationTaskMissionDeliverTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskMissionDeliverTarget>(AnimationTaskType.MissionDeliverTarget);
		}

		public static bool CreateAnimationTaskInteractableObjectLocation(this AnimationTaskManager _animationTaskManager, NavMeshPath _path, EntityId _id, InteractionLocators.Locator _selectedLocator, int _loopCount)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskInteractWithObject.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskInteractWithObject.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskInteractWithObject.ParametersTemplate.Id = _id;
				AnimationTaskInteractWithObject.ParametersTemplate.SelectedLocator = _selectedLocator;
				AnimationTaskInteractWithObject.ParametersTemplate.LoopCount = _loopCount;
				_animationTaskManager.AddAnimationTask(new AnimationTaskInteractWithObject());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskNpcTurning(this AnimationTaskManager _animationTaskManager, Vector3 _lookAtPos)
		{
			Vector3 direction = _lookAtPos - _animationTaskManager.AnimationController.EntityId.GameEntity.Position;
			return _animationTaskManager.CreateAnimationTaskNpcTurning(direction.AngleZX());
		}

		public static bool CreateAnimationTaskNpcTurning(this AnimationTaskManager _animationTaskManager, float _targetAngle)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskNpcTurning.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AddNpcTurning(_animationTaskManager, _targetAngle, AnimationTaskNpcTurning.TurnAccuracy.High);
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTasksPushInstigator(this AnimationTaskManager _animationTaskManager, Character _target)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskPushInstigator.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskPushInstigator.Parameters parametersTemplate = AnimationTaskPushInstigator.ParametersTemplate;
				parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				parametersTemplate.Target = _target.Id;
				_animationTaskManager.AddAnimationTask(new AnimationTaskPushInstigator());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTasksPushTarget(this AnimationTaskManager _animationTaskManager, EntityId _instigator)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskPushTarget.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskPushTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskPushTarget.ParametersTemplate.Instigator = _instigator;
				_animationTaskManager.AddAnimationTask(new AnimationTaskPushTarget());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTasksDismiss(this AnimationTaskManager _animationTaskManager, Vector3 _lookAtPos, int[] _npcIds = null)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskDismiss.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AddNpcTurning(_animationTaskManager, _lookAtPos, AnimationTaskNpcTurning.TurnAccuracy.Low);
				AnimationTaskDismiss.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskDismiss.ParametersTemplate.NpcIds = _npcIds;
				_animationTaskManager.AddAnimationTask(new AnimationTaskDismiss());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTasksSweetMusicReaction(this AnimationTaskManager _animationTaskManager, Vector3 _lookAtPos, bool _shortcutRestart = false)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskLureReaction.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				if (_shortcutRestart)
				{
					_animationTaskManager.AnimationController.Animator.SetCurrentMovementState(MovementState.Movement);
					_animationTaskManager.AnimationController.Animator.Play(AnimationStateHashes.State_WalkRun_Footl.NameHash);
				}
				else
				{
					AnimationTaskLureReaction.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
					_animationTaskManager.AddAnimationTask(new AnimationTaskLureReaction());
					AddNpcTurning(_animationTaskManager, _lookAtPos, AnimationTaskNpcTurning.TurnAccuracy.Low);
				}
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskPokeIntoHaystack(this AnimationTaskManager _animationTaskManager, Vector3 _lookAtPos, float _correctionAngle)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskPokeIntoHaystack.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				Vector3 direction = _lookAtPos - _animationTaskManager.AnimationController.EntityId.GameEntity.Position;
				float targetAngle = direction.AngleZX();
				AddNpcTurning(_animationTaskManager, targetAngle, AnimationTaskNpcTurning.TurnAccuracy.High);
				AnimationTaskPokeIntoHaystack.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				_animationTaskManager.AddAnimationTask(new AnimationTaskPokeIntoHaystack());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskThreat(this AnimationTaskManager _animationTaskManager, IGameEntity _targetDirection, bool _playRandomThreat)
		{
			AnimationTaskThreat.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskThreat.ParametersTemplate.TargetEntity = _targetDirection;
			AnimationTaskThreat.ParametersTemplate.PlayRandomThreat = _playRandomThreat;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskThreat>(AnimationTaskType.Threat);
		}

		public static bool CreateAnimationTaskHitEscortee(this AnimationTaskManager _animationTaskManager, Vector3 _lookAtPos)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHitEscortee.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				_animationTaskManager.AnimationController.EntityId.GetEntity<Character>().RotateInstantly(_lookAtPos);
				AnimationTaskHitEscortee.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				_animationTaskManager.AddAnimationTask(new AnimationTaskHitEscortee());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskExecuteOnGround(this AnimationTaskManager _animationTaskManager, IGameEntity _victim)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHitEscortee.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskExecuteOnGround.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskExecuteOnGround.ParametersTemplate.VictimId = _victim.Id;
				_animationTaskManager.AddAnimationTask(new AnimationTaskExecuteOnGround());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskGetExecutedOnGround(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHitEscortee.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskGetExecutedOnGround.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				_animationTaskManager.AddAnimationTask(new AnimationTaskGetExecutedOnGround());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskGetHit(this AnimationTaskManager _animationTaskManager, IPositionProvider _source, AttackMeleeType _attackType)
		{
			return _animationTaskManager.CreateAnimationTaskGetHit(_source, Vector3.zero, _attackType, true);
		}

		public static bool CreateAnimationTaskGetHit(this AnimationTaskManager _animationTaskManager, Vector3 _reactionDirection, AttackMeleeType _attackType, bool _cancelCurrentTask)
		{
			return _animationTaskManager.CreateAnimationTaskGetHit(null, _reactionDirection, _attackType, _cancelCurrentTask);
		}

		public static bool CreateAnimationTaskGetHit(this AnimationTaskManager _animationTaskManager, IPositionProvider _source, Vector3 _reactionDirection, AttackMeleeType _attackType, bool _cancelCurrentTask, bool _forceDead = false)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskGetHit.Properties.Layer;
			AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(AnimationTaskManager.ELayer.Base);
			if (currentTask != null && currentTask.TaskType == AnimationTaskType.Knockdowned)
			{
				currentTask.Cast<AnimationTaskKnockdowned>().OnGotHit();
			}
			if (!_cancelCurrentTask || _animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskGetHit.ParametersTemplate.ReactionDirection = _reactionDirection;
				AnimationTaskGetHit.ParametersTemplate.Source = _source;
				AnimationTaskGetHit.ParametersTemplate.ForceDead = _forceDead;
				AddGetHit(_animationTaskManager, _attackType);
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskDieFromFall(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskDieFromFall.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTaskForced(layer))
			{
				AnimationTaskDieFromFall.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				_animationTaskManager.AddAnimationTask(new AnimationTaskDieFromFall());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskAssassinateStealthInstigator(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskAssassinateStealthInstigator.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskAssassinateStealthInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskAssassinateStealthInstigator.ParametersTemplate.Instigator = _instigator;
				AnimationTaskAssassinateStealthInstigator.ParametersTemplate.Target = _target;
				_animationTaskManager.AddAnimationTask(new AnimationTaskAssassinateStealthInstigator());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskAssassinateStealthTarget(this AnimationTaskManager _animationTaskManager, Character _instigator, Character _target)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskAssassinateStealthTarget.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTaskForced(layer))
			{
				AnimationTaskAssassinateStealthTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskAssassinateStealthTarget.ParametersTemplate.Instigator = _instigator;
				AnimationTaskAssassinateStealthTarget.ParametersTemplate.Target = _target;
				_animationTaskManager.AddAnimationTask(new AnimationTaskAssassinateStealthTarget());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskAssassinateLeapInstigator(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, bool _withKillCam)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskAssassinateLeapInstigator.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskAssassinateLeapInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskAssassinateLeapInstigator.ParametersTemplate.Instigator = _instigator;
				AnimationTaskAssassinateLeapInstigator.ParametersTemplate.Target = _target;
				AnimationTaskAssassinateLeapInstigator.ParametersTemplate.WithKillCam = _withKillCam;
				_animationTaskManager.AddAnimationTask(new AnimationTaskAssassinateLeapInstigator());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskAssassinateLeapTarget(this AnimationTaskManager _animationTaskManager, Character _instigator, Character _target)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskAssassinateLeapTarget.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTaskForced(layer))
			{
				AnimationTaskAssassinateLeapTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskAssassinateLeapTarget.ParametersTemplate.Instigator = _instigator;
				AnimationTaskAssassinateLeapTarget.ParametersTemplate.Target = _target;
				_animationTaskManager.AddAnimationTask(new AnimationTaskAssassinateLeapTarget());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskAssassinateAirInstigator(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, bool _withKillCam)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskAssassinateAirInstigator.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskAssassinateAirInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskAssassinateAirInstigator.ParametersTemplate.Instigator = _instigator;
				AnimationTaskAssassinateAirInstigator.ParametersTemplate.Target = _target;
				AnimationTaskAssassinateAirInstigator.ParametersTemplate.WithKillCam = _withKillCam;
				_animationTaskManager.AddAnimationTask(new AnimationTaskAssassinateAirInstigator());
				result = true;
			}
			return result;
		}

		public static bool CreateAnimationTaskAssassinateAirTarget(this AnimationTaskManager _animationTaskManager, Character _instigator, Character _target)
		{
			bool result = false;
			AnimationTaskManager.ELayer layer = AnimationTaskAssassinateAirTarget.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskAssassinateAirTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskAssassinateAirTarget.ParametersTemplate.Instigator = _instigator;
				AnimationTaskAssassinateAirTarget.ParametersTemplate.Target = _target;
				_animationTaskManager.AddAnimationTask(new AnimationTaskAssassinateAirTarget());
				result = true;
			}
			return result;
		}

		public static void CreateAnimationTaskBenchKillInstigator(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, float _distanceToTarget, float _distanceToTargetNormalized, AnimationController.Direction _direction)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskBenchKillInstigator.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskBenchKillInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskBenchKillInstigator.ParametersTemplate.Instigator = _instigator;
			AnimationTaskBenchKillInstigator.ParametersTemplate.Target = _target;
			AnimationTaskBenchKillInstigator.ParametersTemplate.DistanceToTarget = _distanceToTarget;
			AnimationTaskBenchKillInstigator.ParametersTemplate.DistanceToTargetNormalized = _distanceToTargetNormalized;
			AnimationTaskBenchKillInstigator.ParametersTemplate.Direction = _direction;
			_animationTaskManager.AddAnimationTask(new AnimationTaskBenchKillInstigator());
		}

		public static void CreateAnimationTaskBenchKillTarget(this AnimationTaskManager _animationTaskManager, GameObject _target, Bench _bench, float _distanceToTarget, float _distanceToTargetNormalized, AnimationController.Direction _direction)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskBenchKillTarget.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskBenchKillTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskBenchKillTarget.ParametersTemplate.Bench = _bench;
			AnimationTaskBenchKillTarget.ParametersTemplate.Target = _target;
			AnimationTaskBenchKillTarget.ParametersTemplate.DistanceToTarget = _distanceToTarget;
			AnimationTaskBenchKillTarget.ParametersTemplate.DistanceToTargetNormalized = _distanceToTargetNormalized;
			AnimationTaskBenchKillTarget.ParametersTemplate.Direction = _direction;
			_animationTaskManager.AddAnimationTask(new AnimationTaskBenchKillTarget());
		}

		public static void CreateAnimationTaskHayStackKillInstigator(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, float _distanceHayStackToTarget)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHayStackKillInstigator.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskHayStackKillInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskHayStackKillInstigator.ParametersTemplate.Instigator = _instigator;
			AnimationTaskHayStackKillInstigator.ParametersTemplate.Target = _target;
			AnimationTaskHayStackKillInstigator.ParametersTemplate.DistanceHideoutToTarget = _distanceHayStackToTarget;
			_animationTaskManager.AddAnimationTask(new AnimationTaskHayStackKillInstigator());
		}

		public static void CreateAnimationTaskHayStackKillTarget(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, float _distanceHayStackToTarget)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskHayStackKillTarget.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskHayStackKillTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskHayStackKillTarget.ParametersTemplate.Instigator = _instigator;
			AnimationTaskHayStackKillTarget.ParametersTemplate.Target = _target;
			AnimationTaskHayStackKillTarget.ParametersTemplate.DistanceHideoutToTarget = _distanceHayStackToTarget;
			_animationTaskManager.AddAnimationTask(new AnimationTaskHayStackKillTarget());
		}

		public static void CreateAnimationTaskRooftopGardenKillInstigator(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, float _distanceRooftopGardenToTarget, InteractionLocators.Locator _locator)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskRooftopGardenKillInstigator.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskRooftopGardenKillInstigator.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskRooftopGardenKillInstigator.ParametersTemplate.Instigator = _instigator;
			AnimationTaskRooftopGardenKillInstigator.ParametersTemplate.Target = _target;
			AnimationTaskRooftopGardenKillInstigator.ParametersTemplate.DistanceHideoutToTarget = _distanceRooftopGardenToTarget;
			AnimationTaskRooftopGardenKillInstigator.ParametersTemplate.Locator = _locator;
			_animationTaskManager.AddAnimationTask(new AnimationTaskRooftopGardenKillInstigator());
		}

		public static void CreateAnimationTaskRooftopGardenKillTarget(this AnimationTaskManager _animationTaskManager, GameObject _instigator, GameObject _target, float _distanceRooftopGardenToTarget)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskRooftopGardenKillTarget.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskRooftopGardenKillTarget.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskRooftopGardenKillTarget.ParametersTemplate.Instigator = _instigator;
			AnimationTaskRooftopGardenKillTarget.ParametersTemplate.Target = _target;
			AnimationTaskRooftopGardenKillTarget.ParametersTemplate.DistanceHideoutToTarget = _distanceRooftopGardenToTarget;
			_animationTaskManager.AddAnimationTask(new AnimationTaskRooftopGardenKillTarget());
		}

		public static void CreateAnimationTaskBlackjackInstigator(this AnimationTaskManager _animationTaskManager, EntityId _instigator, EntityId _target, AnimationController.Direction _targetDir)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskRooftopGardenKillInstigator.Properties.Layer;
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskBlackjackInstigator.Parameters parametersTemplate = AnimationTaskBlackjackInstigator.ParametersTemplate;
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			parametersTemplate.Instigator = _instigator;
			parametersTemplate.Target = _target;
			parametersTemplate.TargetDir = _targetDir;
			_animationTaskManager.AddAnimationTask(new AnimationTaskBlackjackInstigator());
		}

		public static bool CreateAnimationTaskBlackjackTarget(this AnimationTaskManager _animationTaskManager, EntityId _instigator, EntityId _target, AnimationController.Direction _targetDir)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskBlackjackTarget.Properties.Layer;
			AnimationTaskType currentTaskType = _animationTaskManager.GetCurrentTaskType(AnimationTaskManager.ELayer.Base);
			if (currentTaskType == AnimationTaskType.BlackjackTarget)
			{
				return false;
			}
			Character entity = _target.GetEntity<Character>();
			if (entity.GetIsBeingAssassinated())
			{
				return false;
			}
			if (entity.GetIsKnockdowned())
			{
				return true;
			}
			_animationTaskManager.CancelCurrentTaskForced(layer);
			AnimationTaskBlackjackTarget.Parameters parametersTemplate = AnimationTaskBlackjackTarget.ParametersTemplate;
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			parametersTemplate.Instigator = _instigator;
			parametersTemplate.Target = _target;
			parametersTemplate.TargetDir = _targetDir;
			_animationTaskManager.AddAnimationTask(new AnimationTaskBlackjackTarget());
			return true;
		}

		public static void CreateAnimationTaskThrowSmokeBomb(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskThrowSmokeBomb.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskThrowSmokeBomb());
		}

		public static void CreateAnimationTaskThrowCoins(this AnimationTaskManager _animationTaskManager, IPositionProvider _targetProvider)
		{
			_animationTaskManager.CancelCurrentTaskForced(AnimationTaskPlayerTurning.Properties.Layer);
			AnimationTaskPlayerTurning.Parameters parametersTemplate = AnimationTaskPlayerTurning.ParametersTemplate;
			parametersTemplate.TargetProvider = _targetProvider;
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskPlayerTurning());
			_animationTaskManager.CancelCurrentTaskForced(AnimationTaskThrowCoins.Properties.Layer);
			AnimationTaskThrowCoins.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskThrowCoins());
		}

		public static void CreateAnimationTaskSplinterBomb(this AnimationTaskManager _animationTaskManager, IPositionProvider _targetProvider)
		{
			_animationTaskManager.CancelCurrentTaskForced(AnimationTaskPlayerTurning.Properties.Layer);
			AnimationTaskPlayerTurning.Parameters parametersTemplate = AnimationTaskPlayerTurning.ParametersTemplate;
			parametersTemplate.TargetProvider = _targetProvider;
			parametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskPlayerTurning());
			AnimationTaskSplinterBomb.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			_animationTaskManager.AddAnimationTask(new AnimationTaskSplinterBomb());
		}

		public static void CreateAnimationTaskFanOfKnives(this AnimationTaskManager _animationTaskManager, EntityId _targetEntityId)
		{
			AnimationTaskFanOfKnives.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskFanOfKnives.ParametersTemplate.TargetEntityId = _targetEntityId;
			_animationTaskManager.CreateAnimationTask<AnimationTaskFanOfKnives>(AnimationTaskType.FanOfKnives);
		}

		public static bool CreateAnimationTaskSurgeOfFury(this AnimationTaskManager _animationTaskManager, EntityId _targetEntityId)
		{
			AnimationTaskSurgeOfFury.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskSurgeOfFury.ParametersTemplate.TargetEntityId = _targetEntityId;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskSurgeOfFury>(AnimationTaskType.SurgeOfFury);
		}

		public static bool CreateAnimationTaskCourtesanSeduce(this AnimationTaskManager _animationTaskManager, EntityId _targetId)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskCourtesanSeduce.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AddNpcTurning(_animationTaskManager, _targetId, AnimationTaskNpcTurning.TurnAccuracy.High);
				AnimationTaskCourtesanSeduce.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskCourtesanSeduce.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager, 3);
				_animationTaskManager.AddAnimationTask(new AnimationTaskCourtesanSeduce());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskUpperBodySeduce(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(AnimationTaskManager.ELayer.UpperBody);
			if (currentTask != null && currentTask.TaskType == AnimationTaskType.SpecialSeduce)
			{
				return true;
			}
			AnimationTaskSpecialSeduce.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskSpecialSeduce.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager);
			return _animationTaskManager.CreateAnimationTask<AnimationTaskSpecialSeduce>(AnimationTaskType.SpecialSeduce);
		}

		public static bool CreateAnimationTaskEnemySeduced(this AnimationTaskManager _animationTaskManager, EntityId _targetId)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskEnemySeduced.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AddNpcTurning(_animationTaskManager, _targetId, AnimationTaskNpcTurning.TurnAccuracy.Low);
				AnimationTaskEnemySeduced.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskEnemySeduced.ParametersTemplate.AnimatorNpcId = GetAnimatorNpcId(_animationTaskManager);
				_animationTaskManager.AddAnimationTask(new AnimationTaskEnemySeduced());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskGreedy(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskGreedy.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskGreedy.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				_animationTaskManager.AddAnimationTask(new AnimationTaskGreedy());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskStickyBombPanic(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskStickyBombPanic.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskStickyBombPanic.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				_animationTaskManager.AddAnimationTask(new AnimationTaskStickyBombPanic());
				return true;
			}
			return false;
		}

		public static bool CreateAnimationTaskHoldCombatIdle(this AnimationTaskManager _animationTaskManager)
		{
			AnimationTaskHoldCombatIdle.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			return _animationTaskManager.CreateAnimationTask<AnimationTaskHoldCombatIdle>(AnimationTaskType.HoldCombatIdle);
		}

		public static bool CreateAnimationTaskNpcLookAround(this AnimationTaskManager _animationTaskManager, float _sweepAngle)
		{
			AnimationTaskManager.ELayer layer = AnimationTaskStickyBombPanic.Properties.Layer;
			if (_animationTaskManager.CancelCurrentTask(layer))
			{
				float y = _animationTaskManager.AnimationController.EntityId.GameEntity.Rotation.eulerAngles.y;
				AddNpcTurning(_animationTaskManager, y - _sweepAngle, AnimationTaskNpcTurning.TurnAccuracy.High);
				AddNpcTurning(_animationTaskManager, y + _sweepAngle, AnimationTaskNpcTurning.TurnAccuracy.High);
				return true;
			}
			return false;
		}

		public static AnimationTaskAimWithRangedBase GetCurrentTaskAimWithRanged(this AnimationTaskManager _animationTaskManager)
		{
			WeaponHandler weaponHandler = _animationTaskManager.GetWeaponHandler(EWeaponCategory.Ranged);
			EWeaponType weaponType = WeaponHandler.WeaponTypesToType(weaponHandler.CurrentWeaponDesc.Type);
			AnimationTaskManager.ELayer unsheathAnimationLayer = CharacterDrawWeaponBase.GetUnsheathAnimationLayer(weaponType);
			AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(unsheathAnimationLayer);
			return currentTask as AnimationTaskAimWithRangedBase;
		}

		public static WeaponHandler GetWeaponHandler(this AnimationTaskManager _taskManager, EWeaponCategory _category)
		{
			IWeaponHandlerCacheCarrier entity = _taskManager.AnimationController.GetEntity<IWeaponHandlerCacheCarrier>();
			return entity.WeaponHandlerCache.GetWeaponHandler(_category);
		}

		private static void AddGetHit(AnimationTaskManager _animationTaskManager, AttackMeleeType _attackType)
		{
			AnimationTaskGetHit.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskGetHit.ParametersTemplate.AttackMeleeType = _attackType;
			_animationTaskManager.AddAnimationTask(new AnimationTaskGetHit());
		}

		private static void AddNpcTurning(AnimationTaskManager _animationTaskManager, EntityId _targetId, AnimationTaskNpcTurning.TurnAccuracy _turnAccuracy)
		{
			if (_targetId != EntityId.InvalidId)
			{
				AddNpcTurning(_animationTaskManager, _targetId.GameEntity.Position, _turnAccuracy);
			}
		}

		private static void AddNpcTurning(AnimationTaskManager _animationTaskManager, Vector3 _lookAtPos, AnimationTaskNpcTurning.TurnAccuracy _turnAccuracy)
		{
			Vector3 direction = _lookAtPos - _animationTaskManager.AnimationController.EntityId.GameEntity.Position;
			AddNpcTurning(_animationTaskManager, direction.AngleZX(), _turnAccuracy);
		}

		private static void AddNpcTurning(AnimationTaskManager _animationTaskManager, float _targetAngle, AnimationTaskNpcTurning.TurnAccuracy _turnAccuracy)
		{
			AnimationTaskNpcTurning.ParametersTemplate.TargetAngle = _targetAngle;
			AnimationTaskNpcTurning.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
			AnimationTaskNpcTurning.ParametersTemplate.TurnAccuracy = _turnAccuracy;
			_animationTaskManager.AddAnimationTask(new AnimationTaskNpcTurning());
		}

		private static AnimationTaskStrafe CreateAnimationTaskStrafe(this AnimationTaskManager _animationTaskManager, EntityId _targetId)
		{
			AnimationTaskStrafe animationTaskStrafe = null;
			bool flag = true;
			AnimationTaskManager.ELayer layer = AnimationTaskStrafe.Properties.Layer;
			if (_animationTaskManager.GetTaskCount(layer) > 0)
			{
				AnimationTaskBase currentTask = _animationTaskManager.GetCurrentTask(layer);
				if (currentTask.TaskType == AnimationTaskType.Strafe)
				{
					animationTaskStrafe = (AnimationTaskStrafe)currentTask;
					flag = false;
				}
			}
			if (flag && _animationTaskManager.CancelCurrentTask(layer))
			{
				AnimationTaskStrafe.ParametersTemplate.AnimationController = _animationTaskManager.AnimationController;
				AnimationTaskStrafe.ParametersTemplate.DirectionX = 0f;
				AnimationTaskStrafe.ParametersTemplate.DirectionZ = 0f;
				animationTaskStrafe = new AnimationTaskStrafe();
				_animationTaskManager.AddAnimationTask(animationTaskStrafe);
			}
			return animationTaskStrafe;
		}

		private static Animator GetRangedAnimator(AnimationTaskManager _taskManager)
		{
			WeaponHandler weaponHandler = _taskManager.GetWeaponHandler(EWeaponCategory.Ranged);
			return weaponHandler.Animator;
		}

		private static IdleType GetIdleTypeOnTransition(this AnimationTaskManager _animationTaskManager)
		{
			Character character = _animationTaskManager.AnimationController.Character;
			return (character != null) ? character.GetIdleTypeOnTransition() : IdleType.Neutral;
		}

		private static int GetAnimatorNpcId(AnimationTaskManager _animationTaskManager, int _range = -1)
		{
			if (_animationTaskManager.AnimationController.EntityId != EntityId.InvalidId)
			{
				NpcCharacter npcCharacter = _animationTaskManager.AnimationController.EntityId.Entity as NpcCharacter;
				if (npcCharacter != null)
				{
					int num = npcCharacter.NpcStats.Get_int(StatTypes.EStatType.Npc_AnimatorNpcId);
					if (num < 1)
					{
						num = ((_range <= 0) ? AnimationHelper.ClampAnimatorNpcId((int)npcCharacter.Id.Slot) : AnimationHelper.ClampAnimatorNpcId((int)npcCharacter.Id.Slot, _range));
					}
					return num;
				}
			}
			return -1;
		}
	}
}
