namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	public enum ETriggerCategory
	{
		Invalid = -1,
		Roaming = 0,
		InCombat = 1,
		HidingInObject = 2,
		HidingInPatrol = 3,
		HidingInStalkingZone = 4,
		BeingChased = 5,
		Parcour = 6,
		Other = 7,
		NonInteractive = 8,
		NoSkillFired = 9,
		NoBirth = 10,
		Count = 11
	}
}
