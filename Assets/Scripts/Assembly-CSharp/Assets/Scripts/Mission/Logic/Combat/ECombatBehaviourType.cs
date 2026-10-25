namespace Assets.Scripts.Mission.Logic.Combat
{
	public enum ECombatBehaviourType
	{
		Invalid = 0,
		InitAttack = 1,
		InitUnaware = 2,
		InitWithSkill = 3,
		Idle = 4,
		Attack = 5,
		Parry = 6,
		GetHit = 7,
		Die = 8,
		SkillAct = 9,
		SkillReact = 10,
		External = 11,
		Taunt = 12
	}
}
