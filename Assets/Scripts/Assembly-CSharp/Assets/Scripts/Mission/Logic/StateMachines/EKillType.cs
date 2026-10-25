using System;

namespace Assets.Scripts.Mission.Logic.StateMachines
{
	[Flags]
	public enum EKillType
	{
		None = 0,
		Scripted = 1,
		Combat = 2,
		AssassinationStealth = 4,
		AssassinationLeap = 8,
		AssassinationAir = 0x10,
		AssassinationBench = 0x20,
		AssassinationHaystack = 0x40,
		AssassinationRooftopGarden = 0x80,
		Skill = 0x100,
		DoT = 0x200,
		Token = 0x400,
		Meelee = 0x800,
		Suicide = 0x1000,
		Bomb = 0x2000,
		Assassination = 0xFC,
		Primary = 0xFF,
		Flags = 0x3F00
	}
}
