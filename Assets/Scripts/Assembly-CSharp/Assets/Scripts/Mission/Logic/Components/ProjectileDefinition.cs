using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class ProjectileDefinition : Definition
	{
		public ProjectileData.IProjectileDefinition Definition
		{
			get
			{
				return (ProjectileData.IProjectileDefinition)base.EntityDefinition;
			}
			set
			{
				base.EntityDefinition = value;
			}
		}

		public ProjectileDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
