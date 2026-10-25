using System;

namespace Assets.Scripts.Mission.Logic.Combat
{
	[Flags]
	public enum EApplyDamageFlags
	{
		None = 0,
		ApplyStrengthDmg = 1,
		ApplyDeflect = 2,
		ApplyCriticalDmg = 4,
		ApplyArmour = 8,
		ApplyAnonymous = 0x10,
		ApplySkills = 0x20,
		ApplyKillMove = 0x40,
		ApplyGunpowder = 0x2000,
		ForceCriticalDamage = 0x80,
		ApplyHirelingLevelScaling1 = 0x100,
		ApplyHirelingLevelScaling2 = 0x200,
		ApplyHirelingLevelScaling3 = 0x400,
		ApplyHirelingLevelScaling4 = 0x800,
		IsSkillOrigin = 0x1000,
		SimulateOnly = 0x4000,
		StandardAttack = 0x2D,
		StandardDeflect = 0x2F,
		KillMoveDeflect = 0xEF,
		SkillNormal = 0x1008,
		SkillBomb = 0x3008,
		RangedAttack = 0xC,
		SkillDamageOverTime = 0x1000
	}
}
