using System;

namespace Assets.Scripts.Mission.Logic.RuleSystem
{
	[Flags]
	public enum ESkillActivationType
	{
		None = 0,
		Trigger = 1,
		TriggerSlot = 2,
		SelectSlot = 4,
		Passive = 8,
		PassiveHireling = 0x10,
		Slots = 6,
		Actives = 7,
		Passives = 0x18,
		All = 0x1F
	}
}
