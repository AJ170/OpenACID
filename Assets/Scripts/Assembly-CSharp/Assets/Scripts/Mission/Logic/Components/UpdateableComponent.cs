namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class UpdateableComponent : Component, IUpdateable, IComponent, IUpdateableComponent
	{
		public virtual bool IsActive { get; set; }

		protected UpdateableComponent(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
