using Assets.Scripts.Mission.Definitions;
using Assets.Scripts.Mission.Logic.Entities;

namespace Assets.Scripts.Mission.Logic.Components
{
	public abstract class Definition : Component
	{
		public EntityTypes.EType EntityType
		{
			get
			{
				return Entity.Type;
			}
		}

		public sealed override int Priority
		{
			get
			{
				return 0;
			}
		}

		public EntityData.IEntityDefinition EntityDefinition { get; set; }

		protected Definition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
