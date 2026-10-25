using System;

namespace Assets.Scripts.UI.Ingame.Behaviours
{
	[Flags]
	public enum ETutorialIdCondition
	{
		PlayerIsInLowProfile = 1,
		PlayerIsInHighProfile = 2,
		PlayerIsInCombat = 4,
		PlayerIsNotInCombat = 8
	}
}
