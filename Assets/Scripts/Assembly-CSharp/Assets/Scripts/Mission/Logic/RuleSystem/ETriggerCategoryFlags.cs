using System;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	[Flags]
	public enum ETriggerCategoryFlags
	{
		None = 0,
		Roaming = 1,
		InCombat = 2,
		HidingInObject = 4,
		HidingInPatrol = 8,
		HidingInStalkingZone = 0x10,
		BeingChased = 0x20,
		Parcour = 0x40,
		Other = 0x80,
		NonInteractive = 0x100,
		NoSkillFired = 0x200,
		NoBirth = 0x400,
		Count = 0x800,
		PlayerMainStates = 0xEF,
		MainStates = 0xFF,
		Exclusions = 0x700
	}
}
