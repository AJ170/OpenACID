namespace Assets.Scripts.Mission.Logic
{
	public interface IEntityFilter
	{
		EEntityFilterType Type { get; }

		bool Filter(IEntity _entity);
	}
}
