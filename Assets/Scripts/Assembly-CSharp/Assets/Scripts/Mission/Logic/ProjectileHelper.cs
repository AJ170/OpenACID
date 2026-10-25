using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Components;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using Database;
using SharedBaseLib.System;
using UbiCore.Utils;
using UbiCore.Utils.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Mission.Logic
{
	public static class ProjectileHelper
	{
		private const float TargetDodgeOffsetY = 1f;

		private const float TargetDodgeOffsetZ = 5f;

		private const float DefaultArrivedDistance = 0.1f;

		private const float MaxArrivedDistance = 0.3f;

		private const float DefaultArrivedDistanceSqr = 0.010000001f;

		private const float MaxArrivedDistanceSqr = 0.09f;

		private const float ArrivedCheckSpeedReference = 5f;

		public static Projectile SpawnProjectile(ProjectileData.EProjectileType _type, IGameEntity _thrower, EntityId _targetId, IBuffedStatsCollectionHandle _stats)
		{
			float velocity = _stats.Get_float(StatTypes.EStatType.Rule_ProjectileSpeed);
			int delay = _stats.Get_int(StatTypes.EStatType.Rule_ProjectileDelay);
			float triggerRadius = _stats.Get_float(StatTypes.EStatType.Rule_ProjectileTriggerRadius);
			int triggerDelay = _stats.Get_int(StatTypes.EStatType.Rule_ProjectileTriggerDelay);
			return SpawnProjectile(_type, _thrower, _targetId, velocity, delay, triggerRadius, triggerDelay, false);
		}

		public static Projectile SpawnProjectile(ProjectileData.EProjectileType _type, IGameEntity _thrower, EntityId _targetId, float _velocity, bool _targetDodges)
		{
			return SpawnProjectile(_type, _thrower, _targetId, _velocity, 0, 0f, 0, _targetDodges);
		}

		private static Projectile SpawnProjectile(ProjectileData.EProjectileType _type, IGameEntity _thrower, EntityId _targetId, float _velocity, int _delay, float _triggerRadius, int _triggerDelay, bool _targetDodges)
		{
			ProjectileData.IProjectileDefinition definition = UbiCore.Utils.Singleton<ProjectileData>.Instance.GetDefinition(_type);
			GameObject gameObject = EffectHelper.SpawnEffectInWorld(definition.EffectGameID);
			Vector3 localScale = gameObject.transform.localScale;
			EntityWrapperComponent wrapper = gameObject.AddComponent<EntityWrapperComponent>();
			Projectile projectile = Helpers.CreateRegisterAttachEntity<Projectile>(definition, wrapper);
			projectile.Scale = localScale;
			Vector3 projectileSpawnPos = GetProjectileSpawnPos(_thrower, _type);
			Character character = _targetId.GetEntity<Character>();
			EProjectileFlightType eProjectileFlightType = EProjectileFlightType.Linear;
			switch (_type)
			{
			case ProjectileData.EProjectileType.SplinterBomb:
			case ProjectileData.EProjectileType.SmokeBomb:
				eProjectileFlightType = EProjectileFlightType.Parabolic;
				break;
			case ProjectileData.EProjectileType.StickyBomb:
			{
				Transform boneTransform = character.UnityComponentCache.Animator.GetBoneTransform(HumanBodyBones.Chest);
				Vector3 up = boneTransform.up;
				Vector3 lhs = _thrower.Position - boneTransform.position;
				up.y = 0f;
				lhs.y = 0f;
				eProjectileFlightType = ((!(Vector3.Dot(lhs, up) > 0f)) ? EProjectileFlightType.StickyBombBack : EProjectileFlightType.StickyBombFront);
				break;
			}
			case ProjectileData.EProjectileType.TripwireBomb:
			case ProjectileData.EProjectileType.JackInTheBox:
				_targetId = _thrower.Id;
				character = _thrower as Character;
				eProjectileFlightType = ((_type != ProjectileData.EProjectileType.JackInTheBox) ? EProjectileFlightType.DeployTrigger : EProjectileFlightType.Deploy);
				break;
			}
			if (eProjectileFlightType == EProjectileFlightType.Deploy || eProjectileFlightType == EProjectileFlightType.DeployTrigger)
			{
				projectile.ProjectileStateMachines.ProjectileFlight.RegisterTarget(character);
			}
			projectile.EntityStats.Set_Vector3(StatTypes.EStatType.Projectile_SourcePosition, projectileSpawnPos);
			projectile.EntityStats.Set_int(StatTypes.EStatType.Projectile_FlightType, (int)eProjectileFlightType);
			projectile.EntityStats.Set_EntityId(StatTypes.EStatType.Projectile_TargetId, _targetId);
			projectile.EntityStats.Set_float(StatTypes.EStatType.Projectile_Speed, _velocity);
			projectile.EntityStats.Set_int(StatTypes.EStatType.Projectile_Delay, _delay);
			projectile.EntityStats.Set_float(StatTypes.EStatType.Projectile_TriggerRadius, _triggerRadius);
			projectile.EntityStats.Set_int(StatTypes.EStatType.Projectile_TriggerDelay, _triggerDelay);
			projectile.EntityStats.Set_bool(StatTypes.EStatType.Projectile_TargetDodges, _targetDodges);
			Vector3 targetPos = projectile.GetTargetPos(character);
			projectile.Position = projectileSpawnPos;
			projectile.Rotation = Quaternion.LookRotation(targetPos - projectileSpawnPos);
			projectile.OnActivate();
			return projectile;
		}

		public static Vector3 GetProjectileSpawnPos(IGameEntity _thrower, ProjectileData.EProjectileType _type)
		{
			if (_thrower.IsAttached)
			{
				Transform transform;
				switch (_type)
				{
				case ProjectileData.EProjectileType.Dagger:
				case ProjectileData.EProjectileType.SplinterBomb:
				case ProjectileData.EProjectileType.SmokeBomb:
				case ProjectileData.EProjectileType.StickyBomb:
				case ProjectileData.EProjectileType.TripwireBomb:
				case ProjectileData.EProjectileType.JackInTheBox:
				case ProjectileData.EProjectileType.FanOfKnivesKnive:
					transform = GetLeftHandTransform(_thrower);
					break;
				case ProjectileData.EProjectileType.BowArrow:
					transform = GetRightHandTransform(_thrower);
					break;
				default:
					return Vector3.zero;
				}
				return transform.position;
			}
			Character character = _thrower as Character;
			return (character != null) ? (_thrower.Position + character.CharacterDefinition.Definition.DefaultProjectileSpawnOffset) : _thrower.Position;
		}

		public static void SetTransformToTarget(this Projectile _projectile, Character _target)
		{
			EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)_projectile.EntityStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
			switch (eProjectileFlightType)
			{
			case EProjectileFlightType.StickyBombFront:
			case EProjectileFlightType.StickyBombBack:
				_projectile.Rotation = ((eProjectileFlightType != EProjectileFlightType.StickyBombFront) ? _target.Rotation : Quaternion.Inverse(_target.Rotation));
				_projectile.Position = _projectile.GetTargetPos(_target);
				break;
			case EProjectileFlightType.Deploy:
			case EProjectileFlightType.DeployTrigger:
			{
				NavMeshHit hit;
				if (NavMesh.SamplePosition(_projectile.Position, out hit, 2f, NavMeshLayers.WalkableLayerMaskPreset))
				{
					_projectile.Position = hit.position;
				}
				_projectile.Rotation = Quaternion.identity;
				break;
			}
			default:
			{
				Vector3 targetPos = _projectile.GetTargetPos(_target);
				Vector3 forward = targetPos - _projectile.Position;
				_projectile.Rotation = ((!(forward.sqrMagnitude < 1E-05f)) ? Quaternion.LookRotation(forward) : Quaternion.Inverse(_target.Rotation));
				_projectile.Position = targetPos;
				break;
			}
			}
		}

		public static bool DecideArrivedAtTarget(this Projectile _projectile, Character _targetEntity, Vector3 _targetPosition)
		{
			ProjectileStats projectileStats = _projectile.ProjectileStats;
			EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)projectileStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
			if (eProjectileFlightType == EProjectileFlightType.Deploy || eProjectileFlightType == EProjectileFlightType.DeployTrigger)
			{
				return false;
			}
			float num = _targetEntity.ComputeCurrentSpeed();
			float arrivedDistanceSqr = Mathf.Lerp(0.010000001f, 0.09f, num / 5f);
			return DecideArrivedAtTarget(_targetPosition, _projectile.Position, arrivedDistanceSqr);
		}

		private static bool DecideArrivedAtTarget(Vector3 _targetPosition, Vector3 _projectilePosition, float _arrivedDistanceSqr)
		{
			return (_targetPosition - _projectilePosition).sqrMagnitude < _arrivedDistanceSqr;
		}

		public static Vector3 GetTargetPos(this Projectile _projectile, Character _target)
		{
			switch (_projectile.ProjectileDefinition.Definition.Type)
			{
			case ProjectileData.EProjectileType.SplinterBomb:
			case ProjectileData.EProjectileType.SmokeBomb:
				return _target.Position;
			case ProjectileData.EProjectileType.StickyBomb:
			{
				EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)_projectile.EntityStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
				return GetStickyProjectilePos(_target, eProjectileFlightType == EProjectileFlightType.StickyBombFront);
			}
			case ProjectileData.EProjectileType.TripwireBomb:
			case ProjectileData.EProjectileType.JackInTheBox:
				return GetDeployProjectilePos(_target, _projectile);
			case ProjectileData.EProjectileType.BowArrow:
				return GetImpactPosWithDodge(_projectile, _target);
			default:
				return _target.GetDefaultProjectileImpactPos();
			}
		}

		private static Vector3 GetImpactPosWithDodge(Projectile _projectile, Character _target)
		{
			Vector3 defaultProjectileImpactPos = _target.GetDefaultProjectileImpactPos();
			if (_projectile.EntityStats.Get_bool(StatTypes.EStatType.Projectile_TargetDodges))
			{
				Vector3 vector = _projectile.EntityStats.Get_Vector3(StatTypes.EStatType.Projectile_SourcePosition);
				defaultProjectileImpactPos.y += 1f;
				Vector3 vector2 = (defaultProjectileImpactPos - vector).normalized * 5f;
				defaultProjectileImpactPos += vector2;
			}
			return defaultProjectileImpactPos;
		}

		private static Vector3 GetStickyProjectilePos(Character _target, bool _bombOnFront)
		{
			float num = ((!_bombOnFront) ? (-0.15f) : 0.16f);
			if (_target.IsAttached)
			{
				Transform boneTransform = _target.UnityComponentCache.Animator.GetBoneTransform(HumanBodyBones.Chest);
				return boneTransform.position + boneTransform.up * num;
			}
			if (_target.GetDeadOrAboutToDie())
			{
				return _target.Position + new Vector3(0f, 0.1f, 0f);
			}
			return _target.Position + _target.Rotation * new Vector3(0f, 1.1f, num);
		}

		private static Vector3 GetDeployProjectilePos(Character _target, Projectile _projectile)
		{
			if (_projectile.ProjectileStateMachines.ProjectileFlight.CurrentStateType != EStateType.Projectile_Flying)
			{
				return _projectile.Position;
			}
			if (_target.IsAttached)
			{
				Transform leftHandTransform = GetLeftHandTransform(_target);
				return leftHandTransform.position + leftHandTransform.rotation * new Vector3(-0.1f, 0f, 0f);
			}
			if (_target.GetDeadOrAboutToDie())
			{
				return _target.Position + new Vector3(0f, 0.1f, 0f);
			}
			return _target.Position + _target.Rotation * new Vector3(0f, 1.1f, 0f);
		}

		public static Vector3 Interpolate(Projectile _projectile, Vector3 _position, Vector3 _target, float _deltaTime)
		{
			ProjectileStats projectileStats = _projectile.ProjectileStats;
			switch ((EProjectileFlightType)projectileStats.Get_int(StatTypes.EStatType.Projectile_FlightType))
			{
			case EProjectileFlightType.Parabolic:
			case EProjectileFlightType.StickyBombFront:
			case EProjectileFlightType.StickyBombBack:
				return InterpolateParabolic(_projectile, _position, _target, _deltaTime);
			case EProjectileFlightType.Deploy:
			case EProjectileFlightType.DeployTrigger:
				return _target;
			default:
				return InterpolateLinear(_projectile, _position, _target, _deltaTime);
			}
		}

		private static Vector3 InterpolateLinear(Projectile _projectile, Vector3 _position, Vector3 _target, float _deltaTime)
		{
			float num = _projectile.ProjectileStats.Get_float(StatTypes.EStatType.Projectile_Speed);
			Vector3 vector = _target - _position;
			return _position + vector * Mathf.Min(num * _deltaTime / vector.magnitude, 1f);
		}

		private static Vector3 InterpolateParabolic(Projectile _projectile, Vector3 _position, Vector3 _target, float _deltaTime)
		{
			float num = _projectile.ProjectileStats.Get_float(StatTypes.EStatType.Projectile_Speed);
			Vector3 vector = _projectile.ProjectileStats.Get_Vector3(StatTypes.EStatType.Projectile_SourcePosition);
			Vector3 vector2 = _position - vector;
			vector2.y = 0f;
			Vector3 vector3 = _target - vector;
			vector3.y = 0f;
			float magnitude = vector3.magnitude;
			float num2 = Mathf.Clamp01((vector2.magnitude + num * _deltaTime) / magnitude);
			float num3 = 1f - (num2 - 0.5f) * (num2 - 0.5f) * 4f;
			num3 = num3 * magnitude / num;
			_position = vector + vector3 * num2;
			_position.y += num3;
			_position.y += (_target.y - vector.y) * num2;
			return _position;
		}

		private static Transform GetLeftHandTransform(IGameEntity _entity)
		{
			return GetChildTransform(_entity, "LeftHand_Weapon_Ref_Jnt");
		}

		private static Transform GetRightHandTransform(IGameEntity _entity)
		{
			return GetChildTransform(_entity, "RightHand_Weapon_Ref_Jnt");
		}

		private static Transform GetChildTransform(IGameEntity _entity, string _childName)
		{
			return _entity.GameObject.FindDirectChildByName(_childName).transform;
		}
	}
}
