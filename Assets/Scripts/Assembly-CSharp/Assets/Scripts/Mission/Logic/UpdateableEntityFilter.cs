namespace Assets.Scripts.Mission.Logic
{
	public class UpdateableEntityFilter : IEntityFilter
	{
		public EEntityFilterType Type
		{
			get
			{
				return EEntityFilterType.UpdateableEntities;
			}
		}

		public bool Filter(IEntity _entity)
		{
			return _entity is IUpdateableEntity;
		}
	}
}
