using Assets.Scripts.Mission.Definitions;

namespace Assets.Scripts.Mission.Logic.Components
{
	public class StaticObjectDefinition : Definition
	{
		public StaticObjectData.IStaticObjectDefinition Definition
		{
			get
			{
				return (StaticObjectData.IStaticObjectDefinition)base.EntityDefinition;
			}
			set
			{
				base.EntityDefinition = value;
			}
		}

		public StaticObjectDefinition(IEntity _entity)
			: base(_entity)
		{
		}
	}
}
