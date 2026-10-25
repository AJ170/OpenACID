using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class StaticObject : Object, IDefinitionCarrier, IStaticObjectDefinitionCarrier, IEntityStatsCarrier, IStaticObjectStatsCarrier
	{
		public override Definition Definition
		{
			get
			{
				return StaticObjectDefinition;
			}
		}

		public override EntityStats EntityStats
		{
			get
			{
				return StaticObjectStats;
			}
		}

		public StaticObjectDefinition StaticObjectDefinition { get; private set; }

		public StaticObjectStats StaticObjectStats { get; private set; }

		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.StaticObject;
			}
		}

		public StaticObject()
		{
			StaticObjectDefinition = AddComponent<StaticObjectDefinition>();
			StaticObjectStats = AddComponent<StaticObjectStats>();
		}
	}
}
