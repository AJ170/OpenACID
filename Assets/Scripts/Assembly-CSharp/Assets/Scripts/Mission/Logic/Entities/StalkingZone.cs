using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class StalkingZone : GameEntity
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.StalkingZone;
			}
		}

		public override Definition Definition
		{
			get
			{
				return null;
			}
		}

		public override EntityStats EntityStats
		{
			get
			{
				return null;
			}
		}

		public StalkingZoneComponent StalkingZoneComponent { get; private set; }

		public StalkingZone()
		{
			StalkingZoneComponent = AddComponent<StalkingZoneComponent>();
		}
	}
}
