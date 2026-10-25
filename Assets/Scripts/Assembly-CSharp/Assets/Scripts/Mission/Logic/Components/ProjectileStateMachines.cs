using Assets.Scripts.Mission.Logic.Entities;
using Assets.Scripts.Mission.Logic.StateMachines;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class ProjectileStateMachines : StateMachines, IStateMachines, IUpdateable, ITurnUpdateable, IComponent, IUpdateableComponent, ITurnUpdateableComponent, Assets.Scripts.Mission.Logic.StateMachines.IProjectileFlightCarrier
	{
		public Assets.Scripts.Mission.Logic.StateMachines.ProjectileFlight ProjectileFlight { get; private set; }

		public ProjectileStateMachines(IEntity _entity)
			: base(_entity)
		{
			ProjectileFlight = AddStateMachine(new Assets.Scripts.Mission.Logic.StateMachines.ProjectileFlight((Projectile)_entity));
		}
	}
}
