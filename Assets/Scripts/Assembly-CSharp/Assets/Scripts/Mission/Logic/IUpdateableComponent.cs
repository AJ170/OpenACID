namespace Assets.Scripts.Mission.Logic
{
	public interface IUpdateableComponent : IUpdateable, IComponent
	{
		new bool IsActive { get; set; }
	}
}
