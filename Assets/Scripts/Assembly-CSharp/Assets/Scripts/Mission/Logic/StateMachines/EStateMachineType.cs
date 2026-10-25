namespace Assets.Scripts.Mission.Logic.StateMachines
{
	public enum EStateMachineType
	{
		Invalid = -1,
		CharacterAwareness = 0,
		CharacterCondition = 1,
		CharacterVitality = 2,
		EnemyVitality = 3,
		PlayerVitality = 4,
		InteractionCharacterMain = 5,
		ObjectInteraction = 6,
		PlayerAlertLevel = 7,
		AssassinApproachingTarget = 8,
		PlayerProfile = 9,
		PlayerVision = 10,
		CharacterDrawWeaponMelee = 11,
		CharacterDrawWeaponRanged = 12,
		CharacterInteraction = 13,
		EnemyMain = 14,
		CrowdMain = 15,
		PlayerMain = 16,
		HirelingMain = 17,
		PlayerDisguise = 18,
		ProjectileFlight = 19,
		PatrolMain = 20,
		PatrolSeduce = 21,
		GameMain = 22,
		PlayerStalkingZone = 23,
		Count = 24
	}
}
