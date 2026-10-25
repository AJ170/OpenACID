using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class Projectile : Object, IDefinitionCarrier, IProjectileStateMachinesCarrier, IEntityStatsCarrier, IGameObjectProxyCarrier, IHighlightCarrier, IProjectileFlightCarrier, IProjectileStatsCarrier, ISpacialEntityCarrier, IStateMachinesCarrier, IUnityComponentCacheCarrier, IEntity, IUpdateableEntity, IPositionProvider, IPositionEntity, IGameObjectEntity, IGameEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.Projectile;
			}
		}

		public override Definition Definition
		{
			get
			{
				return ProjectileDefinition;
			}
		}

		public ProjectileDefinition ProjectileDefinition { get; private set; }

		public override EntityStats EntityStats
		{
			get
			{
				return ProjectileStats;
			}
		}

		public ProjectileStats ProjectileStats { get; private set; }

		public Assets.Scripts.Mission.Logic.Components.StateMachines StateMachines
		{
			get
			{
				return ProjectileStateMachines;
			}
		}

		public ProjectileStateMachines ProjectileStateMachines { get; private set; }

		public ProjectileFlight ProjectileFlight { get; private set; }

		public Projectile()
		{
			ProjectileDefinition = AddComponent<ProjectileDefinition>();
			ProjectileStateMachines = AddComponent<ProjectileStateMachines>();
			ProjectileStats = AddComponent<ProjectileStats>();
			ProjectileFlight = AddComponent<ProjectileFlight>();
		}
	}
}
