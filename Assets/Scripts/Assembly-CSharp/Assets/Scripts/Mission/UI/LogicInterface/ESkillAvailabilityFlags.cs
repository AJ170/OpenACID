using System;

namespace Assets.Scripts.Mission.UI.LogicInterface
{
	[Flags]
	public enum ESkillAvailabilityFlags
	{
		Unknown = 0,
		NotEquipped = 1,
		SkillIsPassive = 2,
		SlotSkillNotSelected = 4,
		IsActive = 8,
		IsCoolingDown = 0x10,
		TargetNotInRange = 0x20,
		NotAnonymous = 0x40,
		NotInCombat = 0x80,
		NotOutOfCombat = 0x100,
		NoTargetSelected = 0x200
	}
}
