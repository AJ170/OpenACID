using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class RooftopGarden : HideoutObject
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.RooftopGarden;
			}
		}

		public RooftopGardenClothes RooftopGardenClothes { get; private set; }

		public RooftopGarden()
		{
			RooftopGardenClothes = AddComponent<RooftopGardenClothes>();
		}
	}
}
