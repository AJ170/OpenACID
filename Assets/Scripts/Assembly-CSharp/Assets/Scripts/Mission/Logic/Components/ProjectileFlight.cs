using Assets.Scripts.Engine;
using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;
using Assets.Scripts.Mission.Logic.Stats;
using UnityEngine;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class ProjectileFlight : FrameUpdateableComponent
	{
		private float SwerveX = 13f;

		private float SwerveY = 3.9f;

		private float SwerveZ = 4.8f;

		public override int Priority
		{
			get
			{
				return 19;
			}
		}

		public ProjectileFlight(IEntity _entity)
			: base(_entity)
		{
		}

		public override void OnRegistered()
		{
			base.OnRegistered();
			IsActive = true;
		}

		public override void DoFrameUpdate(float _deltaTime)
		{
			Projectile projectile = Entity.Cast<Projectile>();
			ProjectileStats projectileStats = projectile.ProjectileStats;
			Assets.Scripts.Mission.Logic.StateMachines.ProjectileFlight projectileFlight = projectile.ProjectileStateMachines.ProjectileFlight;
			switch (projectileFlight.CurrentStateType)
			{
			case EStateType.Projectile_Flying:
			{
				Character character2 = projectileStats.Get_EntityId(StatTypes.EStatType.Projectile_TargetId).TryGetEntity<Character>();
				if (character2 == null)
				{
					projectileFlight.ChangeState(EStateType.Projectile_Arrived);
					return;
				}
				Vector3 position = projectile.Position;
				Vector3 targetPos = projectile.GetTargetPos(character2);
				if (projectile.DecideArrivedAtTarget(character2, targetPos))
				{
					projectileFlight.ChangeState(EStateType.Projectile_Arrived);
				}
				Vector3 vector = ProjectileHelper.Interpolate(projectile, position, targetPos, _deltaTime);
				Vector3 forward = vector - position;
				if (forward.sqrMagnitude > 0.0001f)
				{
					EProjectileFlightType eProjectileFlightType = (EProjectileFlightType)projectile.ProjectileStats.Get_int(StatTypes.EStatType.Projectile_FlightType);
					if (eProjectileFlightType == EProjectileFlightType.Parabolic)
					{
						projectile.GameObject.transform.Rotate(new Vector3(SwerveX, SwerveY, SwerveZ));
					}
					else
					{
						projectile.Rotation = Quaternion.LookRotation(forward);
					}
				}
				projectile.Position = vector;
				break;
			}
			case EStateType.Projectile_Arrived:
			{
				Character character = projectileStats.Get_EntityId(StatTypes.EStatType.Projectile_TargetId).TryGetEntity<Character>();
				if (character != null)
				{
					projectile.SetTransformToTarget(character);
				}
				break;
			}
			}
			base.DoFrameUpdate(_deltaTime);
		}
	}
}
