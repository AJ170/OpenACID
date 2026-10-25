using System;

namespace Assets.Scripts.Mission.Logic
{
	[Flags]
	public enum EHidingSpotFlags
	{
		None = 0,
		Haystack = 1,
		Bench = 2,
		RooftopGarden = 4,
		Crowd = 8,
		StalkingZone = 0x10
	}
}
