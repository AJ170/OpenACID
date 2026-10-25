using Assets.Scripts.Mission.Logic.Components;

namespace Assets.Scripts.Mission.Logic.Entities
{
	public class Pigeonry : InteractableMissionObject
	{
		public override EntityTypes.EType Type
		{
			get
			{
				return EntityTypes.EType.Pigeonry;
			}
		}

		public PigeonryComponent PigeonryComponent { get; private set; }

		public Pigeonry()
		{
			PigeonryComponent = AddComponent<PigeonryComponent>();
		}
	}
}
