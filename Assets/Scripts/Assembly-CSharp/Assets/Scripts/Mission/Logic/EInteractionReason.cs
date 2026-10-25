namespace Assets.Scripts.Mission.Logic
{
	public enum EInteractionReason
	{
		Invalid = -1,
		AssassinationLeap = 0,
		AssassinationStealth = 1,
		AssassinationStealthFallback = 2,
		AssassinationAir = 3,
		ThrowSmokeBomb = 4,
		ThrowSplinterBomb = 5,
		ThrowSplinterBombHireling = 6,
		ThrowStickyBomb = 7,
		ThrowStickyBombHireling = 8,
		Combat = 9,
		InteractWithNpc = 10,
		InteractWithObject = 11,
		HidingInPatrol = 12,
		TriggerSelectedSkill = 13,
		OffMeshLink = 14,
		LetCourtesansSeduce = 15,
		FreeCapturedNpcs = 16,
		Pickpocketing = 17,
		LossyDreams = 18,
		ThrowSand = 19,
		SilentDagger = 20,
		FanOfKnives = 21,
		Count = 22
	}
}
