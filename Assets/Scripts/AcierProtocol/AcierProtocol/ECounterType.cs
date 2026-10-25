using ProtoBuf;

namespace AcierProtocol
{
	[ProtoContract(Name = "ECounterType")]
	public enum ECounterType
	{
		[ProtoEnum(Name = "InvalidCounter", Value = 0)]
		InvalidCounter = 0,
		[ProtoEnum(Name = "MissionsFinished", Value = 1)]
		MissionsFinished = 1,
		[ProtoEnum(Name = "StoryMissionsFinished", Value = 3)]
		StoryMissionsFinished = 3,
		[ProtoEnum(Name = "RandomMissionsFinished", Value = 4)]
		RandomMissionsFinished = 4,
		[ProtoEnum(Name = "BoughtAssassinSlots", Value = 5)]
		BoughtAssassinSlots = 5,
		[ProtoEnum(Name = "BoughtInventorySlots", Value = 6)]
		BoughtInventorySlots = 6,
		[ProtoEnum(Name = "AirAssassinationKills", Value = 7)]
		AirAssassinationKills = 7,
		[ProtoEnum(Name = "FleesFromCombat", Value = 8)]
		FleesFromCombat = 8,
		[ProtoEnum(Name = "ModulesUsed", Value = 9)]
		ModulesUsed = 9,
		[ProtoEnum(Name = "DiesInMissions", Value = 10)]
		DiesInMissions = 10,
		[ProtoEnum(Name = "CollectedClassOutfits", Value = 11)]
		CollectedClassOutfits = 11,
		[ProtoEnum(Name = "KillsOnRooftops", Value = 12)]
		KillsOnRooftops = 12,
		[ProtoEnum(Name = "UsedSmokeBombs", Value = 13)]
		UsedSmokeBombs = 13,
		[ProtoEnum(Name = "KillsBySkills", Value = 14)]
		KillsBySkills = 14,
		[ProtoEnum(Name = "CreatedAssassins", Value = 15)]
		CreatedAssassins = 15,
		[ProtoEnum(Name = "PlayerPaymentValue", Value = 16)]
		PlayerPaymentValue = 16,
		[ProtoEnum(Name = "LendAssassinAsHireling", Value = 17)]
		LendAssassinAsHireling = 17,
		[ProtoEnum(Name = "AchievementsUnlocked", Value = 19)]
		AchievementsUnlocked = 19,
		[ProtoEnum(Name = "EarnedSoftCurrency", Value = 20)]
		EarnedSoftCurrency = 20,
		[ProtoEnum(Name = "NumHirelingsUsed", Value = 22)]
		NumHirelingsUsed = 22,
		[ProtoEnum(Name = "NumFriendsInList", Value = 23)]
		NumFriendsInList = 23,
		[ProtoEnum(Name = "NumConnectsToFacebook", Value = 24)]
		NumConnectsToFacebook = 24,
		[ProtoEnum(Name = "LevelUpsWithBerserker", Value = 25)]
		LevelUpsWithBerserker = 25,
		[ProtoEnum(Name = "LevelUpsWithShadowBlade", Value = 26)]
		LevelUpsWithShadowBlade = 26,
		[ProtoEnum(Name = "LevelUpsWithTrickster", Value = 27)]
		LevelUpsWithTrickster = 27,
		[ProtoEnum(Name = "LevelUpsWithThief", Value = 28)]
		LevelUpsWithThief = 28,
		[ProtoEnum(Name = "UnlockedTier4ActiveSkills", Value = 29)]
		UnlockedTier4ActiveSkills = 29,
		[ProtoEnum(Name = "InvestedSkillpoints", Value = 30)]
		InvestedSkillpoints = 30,
		[ProtoEnum(Name = "ExistingBerserkers", Value = 31)]
		ExistingBerserkers = 31,
		[ProtoEnum(Name = "ExistingTrickster", Value = 32)]
		ExistingTrickster = 32,
		[ProtoEnum(Name = "ExistingShadowblades", Value = 33)]
		ExistingShadowblades = 33,
		[ProtoEnum(Name = "KillsWhileGrabingMoney", Value = 34)]
		KillsWhileGrabingMoney = 34,
		[ProtoEnum(Name = "KillsWhileKnockedDown", Value = 35)]
		KillsWhileKnockedDown = 35,
		[ProtoEnum(Name = "AttractedGuardsWithDistraction", Value = 36)]
		AttractedGuardsWithDistraction = 36,
		[ProtoEnum(Name = "DisguiseTimeInSeconds", Value = 37)]
		DisguiseTimeInSeconds = 37,
		[ProtoEnum(Name = "KillTokenComboKills", Value = 38)]
		KillTokenComboKills = 38,
		[ProtoEnum(Name = "EnemyFleesFromCombat", Value = 39)]
		EnemyFleesFromCombat = 39,
		[ProtoEnum(Name = "KillsWhileInConflictWithoutHit", Value = 40)]
		KillsWhileInConflictWithoutHit = 40,
		[ProtoEnum(Name = "KillingSpree10Minutes", Value = 41)]
		KillingSpree10Minutes = 41,
		[ProtoEnum(Name = "EnemyDragsIntoRoofgarden", Value = 42)]
		EnemyDragsIntoRoofgarden = 42,
		[ProtoEnum(Name = "KillsOutOfCivilianCover", Value = 43)]
		KillsOutOfCivilianCover = 43,
		[ProtoEnum(Name = "KillsOutOfSmokeBombCover", Value = 44)]
		KillsOutOfSmokeBombCover = 44,
		[ProtoEnum(Name = "FlushedPigeons", Value = 45)]
		FlushedPigeons = 45,
		[ProtoEnum(Name = "ShovesByGuards", Value = 46)]
		ShovesByGuards = 46,
		[ProtoEnum(Name = "HidingsInHaystacks", Value = 47)]
		HidingsInHaystacks = 47,
		[ProtoEnum(Name = "KillsWith5PercentageHealth", Value = 48)]
		KillsWith5PercentageHealth = 48,
		[ProtoEnum(Name = "EagleVisionUsageInSeconds", Value = 49)]
		EagleVisionUsageInSeconds = 49,
		[ProtoEnum(Name = "Sendcourtesans", Value = 50)]
		Sendcourtesans = 50,
		[ProtoEnum(Name = "CompletedMissionChallenges", Value = 51)]
		CompletedMissionChallenges = 51,
		[ProtoEnum(Name = "MaxLevelWithBerserker", Value = 52)]
		MaxLevelWithBerserker = 52,
		[ProtoEnum(Name = "MaxLevelWithShadowBlade", Value = 53)]
		MaxLevelWithShadowBlade = 53,
		[ProtoEnum(Name = "MaxLevelWithTrickster", Value = 54)]
		MaxLevelWithTrickster = 54,
		[ProtoEnum(Name = "MaxLevelWithThief", Value = 55)]
		MaxLevelWithThief = 55,
		[ProtoEnum(Name = "AssassinationKills", Value = 56)]
		AssassinationKills = 56,
		[ProtoEnum(Name = "Kills", Value = 57)]
		Kills = 57,
		[ProtoEnum(Name = "Combats", Value = 58)]
		Combats = 58,
		[ProtoEnum(Name = "EnteredRoofs", Value = 59)]
		EnteredRoofs = 59,
		[ProtoEnum(Name = "EnteredGround", Value = 60)]
		EnteredGround = 60,
		[ProtoEnum(Name = "UsedHidingSpots", Value = 62)]
		UsedHidingSpots = 62,
		[ProtoEnum(Name = "UsedSkills", Value = 63)]
		UsedSkills = 63,
		[ProtoEnum(Name = "ExistingThiefs", Value = 64)]
		ExistingThiefs = 64,
		[ProtoEnum(Name = "BleedingSplinterBombSeconds", Value = 65)]
		BleedingSplinterBombSeconds = 65,
		[ProtoEnum(Name = "DieByYourOwnBomb", Value = 66)]
		DieByYourOwnBomb = 66,
		[ProtoEnum(Name = "RestOnBenchSeconds", Value = 67)]
		RestOnBenchSeconds = 67,
		[ProtoEnum(Name = "SkillTreeResetGEQUAL15SPApplied", Value = 68)]
		SkillTreeResetGEQUAL15SPApplied = 68,
		[ProtoEnum(Name = "UsedDoctorSpots", Value = 85)]
		UsedDoctorSpots = 85,
		[ProtoEnum(Name = "UsedRooftopGardensOnStormyMap", Value = 86)]
		UsedRooftopGardensOnStormyMap = 86,
		[ProtoEnum(Name = "PickpocketedCoins", Value = 87)]
		PickpocketedCoins = 87,
		[ProtoEnum(Name = "ReviveFeatureUsed", Value = 88)]
		ReviveFeatureUsed = 88,
		[ProtoEnum(Name = "NumForgedItems", Value = 89)]
		NumForgedItems = 89,
		[ProtoEnum(Name = "NumOpenedMysteryBoxes", Value = 90)]
		NumOpenedMysteryBoxes = 90,
		[ProtoEnum(Name = "BoughtEnergy", Value = 91)]
		BoughtEnergy = 91,
		[ProtoEnum(Name = "QuestRewardReroll", Value = 92)]
		QuestRewardReroll = 92,
		[ProtoEnum(Name = "RankQuestRegenerate", Value = 111)]
		RankQuestRegenerate = 111,
		[ProtoEnum(Name = "AcquireHeroicRank1", Value = 93)]
		AcquireHeroicRank1 = 93,
		[ProtoEnum(Name = "AcquireHeroicRank2", Value = 94)]
		AcquireHeroicRank2 = 94,
		[ProtoEnum(Name = "AcquireHeroicRank3", Value = 95)]
		AcquireHeroicRank3 = 95,
		[ProtoEnum(Name = "AcquireHeroicRank4", Value = 96)]
		AcquireHeroicRank4 = 96,
		[ProtoEnum(Name = "AcquireHeroicRank5", Value = 97)]
		AcquireHeroicRank5 = 97,
		[ProtoEnum(Name = "PillageMissionsWon", Value = 98)]
		PillageMissionsWon = 98,
		[ProtoEnum(Name = "CourierMissionsWon", Value = 99)]
		CourierMissionsWon = 99,
		[ProtoEnum(Name = "ParkourMissionsWon", Value = 100)]
		ParkourMissionsWon = 100,
		[ProtoEnum(Name = "AssassinationMissionsWon", Value = 101)]
		AssassinationMissionsWon = 101,
		[ProtoEnum(Name = "KillContractMissionsWon", Value = 102)]
		KillContractMissionsWon = 102,
		[ProtoEnum(Name = "BountyHuntMissionsWon", Value = 103)]
		BountyHuntMissionsWon = 103,
		[ProtoEnum(Name = "RecoveryMissionsWon", Value = 104)]
		RecoveryMissionsWon = 104,
		[ProtoEnum(Name = "EscortMissionsWon", Value = 105)]
		EscortMissionsWon = 105,
		[ProtoEnum(Name = "TailMissionsWon", Value = 106)]
		TailMissionsWon = 106,
		[ProtoEnum(Name = "StalkMissionsWon", Value = 107)]
		StalkMissionsWon = 107,
		[ProtoEnum(Name = "LiberationMissionsWon", Value = 108)]
		LiberationMissionsWon = 108,
		[ProtoEnum(Name = "TrackDownMissionsWon", Value = 112)]
		TrackDownMissionsWon = 112,
		[ProtoEnum(Name = "CrowHuntMissionsWon", Value = 534)]
		CrowHuntMissionsWon = 534,
		[ProtoEnum(Name = "FinishCampaignOneNormal", Value = 109)]
		FinishCampaignOneNormal = 109,
		[ProtoEnum(Name = "FinishAllTrainingMissions", Value = 110)]
		FinishAllTrainingMissions = 110,
		[ProtoEnum(Name = "SecondClassUnlocking", Value = 113)]
		SecondClassUnlocking = 113,
		[ProtoEnum(Name = "InventorySlotPurchases", Value = 114)]
		InventorySlotPurchases = 114,
		[ProtoEnum(Name = "FinishCampaignTwoNormal", Value = 115)]
		FinishCampaignTwoNormal = 115,
		[ProtoEnum(Name = "ChallengesCompletedInCampaignBorgia", Value = 116)]
		ChallengesCompletedInCampaignBorgia = 116,
		[ProtoEnum(Name = "ChallengesCompletedInCampaignForli", Value = 117)]
		ChallengesCompletedInCampaignForli = 117,
		[ProtoEnum(Name = "MissionsCompletedMonteriggioni", Value = 118)]
		MissionsCompletedMonteriggioni = 118,
		[ProtoEnum(Name = "MissionsCompletedFirenze", Value = 119)]
		MissionsCompletedFirenze = 119,
		[ProtoEnum(Name = "MissionsCompletedRoma", Value = 120)]
		MissionsCompletedRoma = 120,
		[ProtoEnum(Name = "MissionsCompletedForli", Value = 121)]
		MissionsCompletedForli = 121,
		[ProtoEnum(Name = "MissionsCompletedACMS1", Value = 127)]
		MissionsCompletedACMS1 = 127,
		[ProtoEnum(Name = "MissionsCompletedACMG1", Value = 128)]
		MissionsCompletedACMG1 = 128,
		[ProtoEnum(Name = "NumConnectsToWeibo", Value = 122)]
		NumConnectsToWeibo = 122,
		[ProtoEnum(Name = "NumConnectsToUPlay", Value = 123)]
		NumConnectsToUPlay = 123,
		[ProtoEnum(Name = "LockPicksUsed", Value = 124)]
		LockPicksUsed = 124,
		[ProtoEnum(Name = "MissionsRetried", Value = 125)]
		MissionsRetried = 125,
		[ProtoEnum(Name = "MissionsFailed", Value = 126)]
		MissionsFailed = 126,
		[ProtoEnum(Name = "EnemiesShredded", Value = 129)]
		EnemiesShredded = 129,
		[ProtoEnum(Name = "FrightenedTimeInSeconds", Value = 130)]
		FrightenedTimeInSeconds = 130,
		[ProtoEnum(Name = "EnemiesKilledByBomb", Value = 131)]
		EnemiesKilledByBomb = 131,
		[ProtoEnum(Name = "CriticalHits", Value = 132)]
		CriticalHits = 132,
		[ProtoEnum(Name = "CrowsKilled", Value = 133)]
		CrowsKilled = 133,
		[ProtoEnum(Name = "ReviveCount", Value = 134)]
		ReviveCount = 134,
		[ProtoEnum(Name = "PlunderedTreasureChests", Value = 135)]
		PlunderedTreasureChests = 135,
		[ProtoEnum(Name = "SecondsTracedBySeekers", Value = 136)]
		SecondsTracedBySeekers = 136,
		[ProtoEnum(Name = "PlunderedBootyChests", Value = 137)]
		PlunderedBootyChests = 137,
		[ProtoEnum(Name = "MaxSecondsOnGround", Value = 138)]
		MaxSecondsOnGround = 138,
		[ProtoEnum(Name = "EyePatchEquippedCount", Value = 139)]
		EyePatchEquippedCount = 139,
		[ProtoEnum(Name = "KegsOfRumConsumed", Value = 140)]
		KegsOfRumConsumed = 140,
		[ProtoEnum(Name = "FinishedWithNoSurvivors", Value = 141)]
		FinishedWithNoSurvivors = 141,
		[ProtoEnum(Name = "MaxAchievementCounterIdx", Value = 300)]
		MaxAchievementCounterIdx = 300,
		[ProtoEnum(Name = "ContractChallengesCompleted", Value = 301)]
		ContractChallengesCompleted = 301,
		[ProtoEnum(Name = "HealthLimitContractChallengesCompleted", Value = 519)]
		HealthLimitContractChallengesCompleted = 519,
		[ProtoEnum(Name = "KillTypeLimitContractChallengesCompleted", Value = 520)]
		KillTypeLimitContractChallengesCompleted = 520,
		[ProtoEnum(Name = "DontKillTypeLimitContractChallengesCompleted", Value = 521)]
		DontKillTypeLimitContractChallengesCompleted = 521,
		[ProtoEnum(Name = "NoAssassinationContractChallengesCompleted", Value = 522)]
		NoAssassinationContractChallengesCompleted = 522,
		[ProtoEnum(Name = "NoCombatContractChallengesCompleted", Value = 523)]
		NoCombatContractChallengesCompleted = 523,
		[ProtoEnum(Name = "NoDoctorContractChallengesCompleted", Value = 524)]
		NoDoctorContractChallengesCompleted = 524,
		[ProtoEnum(Name = "NoFleeingContractChallengesCompleted", Value = 525)]
		NoFleeingContractChallengesCompleted = 525,
		[ProtoEnum(Name = "NoHidingSpotsContractChallengesCompleted", Value = 526)]
		NoHidingSpotsContractChallengesCompleted = 526,
		[ProtoEnum(Name = "NoKillContractChallengesCompleted", Value = 527)]
		NoKillContractChallengesCompleted = 527,
		[ProtoEnum(Name = "NoSkillsContractChallengesCompleted", Value = 528)]
		NoSkillsContractChallengesCompleted = 528,
		[ProtoEnum(Name = "OnlyAssassinationsContractChallengesCompleted", Value = 529)]
		OnlyAssassinationsContractChallengesCompleted = 529,
		[ProtoEnum(Name = "StayAnonymousContractChallengesCompleted", Value = 530)]
		StayAnonymousContractChallengesCompleted = 530,
		[ProtoEnum(Name = "StayUndetectedContractChallengesCompleted", Value = 531)]
		StayUndetectedContractChallengesCompleted = 531,
		[ProtoEnum(Name = "TimeLimitContractChallengesCompleted", Value = 532)]
		TimeLimitContractChallengesCompleted = 532,
		[ProtoEnum(Name = "TreasureChestContractChallengesCompleted", Value = 533)]
		TreasureChestContractChallengesCompleted = 533,
		[ProtoEnum(Name = "DailyQuestsCompleted", Value = 314)]
		DailyQuestsCompleted = 314,
		[ProtoEnum(Name = "DailyQuestChainsCompleted", Value = 315)]
		DailyQuestChainsCompleted = 315,
		[ProtoEnum(Name = "UsedHirelingsFromFriends", Value = 316)]
		UsedHirelingsFromFriends = 316,
		[ProtoEnum(Name = "AddedFriendsAfterUsingHireling", Value = 317)]
		AddedFriendsAfterUsingHireling = 317,
		[ProtoEnum(Name = "SpentSkillPoints", Value = 318)]
		SpentSkillPoints = 318,
		[ProtoEnum(Name = "SpentSoftCurrency", Value = 319)]
		SpentSoftCurrency = 319,
		[ProtoEnum(Name = "SpentHardCurrency", Value = 320)]
		SpentHardCurrency = 320,
		[ProtoEnum(Name = "SpentHeroicCurrency", Value = 321)]
		SpentHeroicCurrency = 321,
		[ProtoEnum(Name = "EarnedXp", Value = 322)]
		EarnedXp = 322,
		[ProtoEnum(Name = "DailyLoginRewardCompleteDayX", Value = 323)]
		DailyLoginRewardCompleteDayX = 323,
		[ProtoEnum(Name = "MissionsCompletedWithChestsOpened", Value = 324)]
		MissionsCompletedWithChestsOpened = 324,
		[ProtoEnum(Name = "MissionsCompletedWithTreasureChestsOpened", Value = 325)]
		MissionsCompletedWithTreasureChestsOpened = 325,
		[ProtoEnum(Name = "MissionsCompletedWithPigeonCoupsUsed", Value = 326)]
		MissionsCompletedWithPigeonCoupsUsed = 326,
		[ProtoEnum(Name = "MissionsCompletedWithMessagesDelivered", Value = 327)]
		MissionsCompletedWithMessagesDelivered = 327,
		[ProtoEnum(Name = "MissionsCompletedWithAssassinatedTargets", Value = 328)]
		MissionsCompletedWithAssassinatedTargets = 328,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledMilitia", Value = 329)]
		MissionsCompletedWithEnemiesKilledMilitia = 329,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledElite", Value = 330)]
		MissionsCompletedWithEnemiesKilledElite = 330,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledThief", Value = 331)]
		MissionsCompletedWithEnemiesKilledThief = 331,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledSeeker", Value = 332)]
		MissionsCompletedWithEnemiesKilledSeeker = 332,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledAgile", Value = 333)]
		MissionsCompletedWithEnemiesKilledAgile = 333,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledArcher", Value = 334)]
		MissionsCompletedWithEnemiesKilledArcher = 334,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledLopurum", Value = 335)]
		MissionsCompletedWithEnemiesKilledLopurum = 335,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledSquadLeader", Value = 336)]
		MissionsCompletedWithEnemiesKilledSquadLeader = 336,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledMercenary", Value = 337)]
		MissionsCompletedWithEnemiesKilledMercenary = 337,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledCrowssbowman", Value = 338)]
		MissionsCompletedWithEnemiesKilledCrowssbowman = 338,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledCrowleader", Value = 339)]
		MissionsCompletedWithEnemiesKilledCrowleader = 339,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledGuardCaptain", Value = 340)]
		MissionsCompletedWithEnemiesKilledGuardCaptain = 340,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledBrute", Value = 341)]
		MissionsCompletedWithEnemiesKilledBrute = 341,
		[ProtoEnum(Name = "MissionsCompletedWithEnemiesKilledPapalGuard", Value = 342)]
		MissionsCompletedWithEnemiesKilledPapalGuard = 342,
		[ProtoEnum(Name = "MissionsCompletedWithoutDamage", Value = 343)]
		MissionsCompletedWithoutDamage = 343,
		[ProtoEnum(Name = "MissionsCompletedWithoutCombat", Value = 344)]
		MissionsCompletedWithoutCombat = 344,
		[ProtoEnum(Name = "MissionsCompletedWithoutBeingDetected", Value = 345)]
		MissionsCompletedWithoutBeingDetected = 345,
		[ProtoEnum(Name = "MissionsCompletedWithoutFleeing", Value = 346)]
		MissionsCompletedWithoutFleeing = 346,
		[ProtoEnum(Name = "MissionCompletedWithoutUsingSkills", Value = 347)]
		MissionCompletedWithoutUsingSkills = 347,
		[ProtoEnum(Name = "SpentBlueprints", Value = 348)]
		SpentBlueprints = 348,
		[ProtoEnum(Name = "SpentBlueprintsWeapon", Value = 349)]
		SpentBlueprintsWeapon = 349,
		[ProtoEnum(Name = "SpentBlueprintsBoots", Value = 350)]
		SpentBlueprintsBoots = 350,
		[ProtoEnum(Name = "SpentBlueprintsLegs", Value = 351)]
		SpentBlueprintsLegs = 351,
		[ProtoEnum(Name = "SpentBlueprintsChest", Value = 352)]
		SpentBlueprintsChest = 352,
		[ProtoEnum(Name = "SpentBlueprintsArms", Value = 353)]
		SpentBlueprintsArms = 353,
		[ProtoEnum(Name = "SpentBlueprintsShoulder", Value = 354)]
		SpentBlueprintsShoulder = 354,
		[ProtoEnum(Name = "SpentBlueprintsInsignia", Value = 355)]
		SpentBlueprintsInsignia = 355,
		[ProtoEnum(Name = "OpenedMysteryBoxes", Value = 356)]
		OpenedMysteryBoxes = 356,
		[ProtoEnum(Name = "OpenedMysteryBoxesItem", Value = 357)]
		OpenedMysteryBoxesItem = 357,
		[ProtoEnum(Name = "OpenedMysteryBoxesItemMundane", Value = 358)]
		OpenedMysteryBoxesItemMundane = 358,
		[ProtoEnum(Name = "OpenedMysteryBoxesItemCommon", Value = 359)]
		OpenedMysteryBoxesItemCommon = 359,
		[ProtoEnum(Name = "OpenedMysteryBoxesItemUncommon", Value = 360)]
		OpenedMysteryBoxesItemUncommon = 360,
		[ProtoEnum(Name = "OpenedMysteryBoxesItemRare", Value = 361)]
		OpenedMysteryBoxesItemRare = 361,
		[ProtoEnum(Name = "OpenedMysteryBoxesItemPerfect", Value = 362)]
		OpenedMysteryBoxesItemPerfect = 362,
		[ProtoEnum(Name = "OpenedMysteryBoxesResource", Value = 363)]
		OpenedMysteryBoxesResource = 363,
		[ProtoEnum(Name = "OpenedMysteryBoxesResourceMundane", Value = 364)]
		OpenedMysteryBoxesResourceMundane = 364,
		[ProtoEnum(Name = "OpenedMysteryBoxesResourceCommon", Value = 365)]
		OpenedMysteryBoxesResourceCommon = 365,
		[ProtoEnum(Name = "OpenedMysteryBoxesResourceUncommon", Value = 366)]
		OpenedMysteryBoxesResourceUncommon = 366,
		[ProtoEnum(Name = "OpenedMysteryBoxesResourceRare", Value = 367)]
		OpenedMysteryBoxesResourceRare = 367,
		[ProtoEnum(Name = "OpenedMysteryBoxesResourcePerfect", Value = 368)]
		OpenedMysteryBoxesResourcePerfect = 368,
		[ProtoEnum(Name = "ForgedItems", Value = 369)]
		ForgedItems = 369,
		[ProtoEnum(Name = "ForgedItemsMundane", Value = 370)]
		ForgedItemsMundane = 370,
		[ProtoEnum(Name = "ForgedItemsCommon", Value = 371)]
		ForgedItemsCommon = 371,
		[ProtoEnum(Name = "ForgedItemsUncommon", Value = 372)]
		ForgedItemsUncommon = 372,
		[ProtoEnum(Name = "ForgedItemsRare", Value = 373)]
		ForgedItemsRare = 373,
		[ProtoEnum(Name = "ForgedItemsPerfect", Value = 374)]
		ForgedItemsPerfect = 374,
		[ProtoEnum(Name = "ForgedItemsWeapon", Value = 375)]
		ForgedItemsWeapon = 375,
		[ProtoEnum(Name = "ForgedItemsWeaponMundane", Value = 376)]
		ForgedItemsWeaponMundane = 376,
		[ProtoEnum(Name = "ForgedItemsWeaponCommon", Value = 377)]
		ForgedItemsWeaponCommon = 377,
		[ProtoEnum(Name = "ForgedItemsWeaponUncommon", Value = 378)]
		ForgedItemsWeaponUncommon = 378,
		[ProtoEnum(Name = "ForgedItemsWeaponRare", Value = 379)]
		ForgedItemsWeaponRare = 379,
		[ProtoEnum(Name = "ForgedItemsWeaponPerfect", Value = 380)]
		ForgedItemsWeaponPerfect = 380,
		[ProtoEnum(Name = "ForgedItemsBoots", Value = 381)]
		ForgedItemsBoots = 381,
		[ProtoEnum(Name = "ForgedItemsBootsMundane", Value = 382)]
		ForgedItemsBootsMundane = 382,
		[ProtoEnum(Name = "ForgedItemsBootsCommon", Value = 383)]
		ForgedItemsBootsCommon = 383,
		[ProtoEnum(Name = "ForgedItemsBootsUncommon", Value = 384)]
		ForgedItemsBootsUncommon = 384,
		[ProtoEnum(Name = "ForgedItemsBootsRare", Value = 385)]
		ForgedItemsBootsRare = 385,
		[ProtoEnum(Name = "ForgedItemsBootsPerfect", Value = 386)]
		ForgedItemsBootsPerfect = 386,
		[ProtoEnum(Name = "ForgedItemsLegs", Value = 387)]
		ForgedItemsLegs = 387,
		[ProtoEnum(Name = "ForgedItemsLegsMundane", Value = 388)]
		ForgedItemsLegsMundane = 388,
		[ProtoEnum(Name = "ForgedItemsLegsCommon", Value = 389)]
		ForgedItemsLegsCommon = 389,
		[ProtoEnum(Name = "ForgedItemsLegsUncommon", Value = 390)]
		ForgedItemsLegsUncommon = 390,
		[ProtoEnum(Name = "ForgedItemsLegsRare", Value = 391)]
		ForgedItemsLegsRare = 391,
		[ProtoEnum(Name = "ForgedItemsLegsPerfect", Value = 392)]
		ForgedItemsLegsPerfect = 392,
		[ProtoEnum(Name = "ForgedItemsChest", Value = 393)]
		ForgedItemsChest = 393,
		[ProtoEnum(Name = "ForgedItemsChestMundane", Value = 394)]
		ForgedItemsChestMundane = 394,
		[ProtoEnum(Name = "ForgedItemsChestCommon", Value = 395)]
		ForgedItemsChestCommon = 395,
		[ProtoEnum(Name = "ForgedItemsChestUncommon", Value = 396)]
		ForgedItemsChestUncommon = 396,
		[ProtoEnum(Name = "ForgedItemsChestRare", Value = 397)]
		ForgedItemsChestRare = 397,
		[ProtoEnum(Name = "ForgedItemsChestPerfect", Value = 398)]
		ForgedItemsChestPerfect = 398,
		[ProtoEnum(Name = "ForgedItemsArms", Value = 400)]
		ForgedItemsArms = 400,
		[ProtoEnum(Name = "ForgedItemsArmsMundane", Value = 401)]
		ForgedItemsArmsMundane = 401,
		[ProtoEnum(Name = "ForgedItemsArmsCommon", Value = 402)]
		ForgedItemsArmsCommon = 402,
		[ProtoEnum(Name = "ForgedItemsArmsUncommon", Value = 403)]
		ForgedItemsArmsUncommon = 403,
		[ProtoEnum(Name = "ForgedItemsArmsRare", Value = 404)]
		ForgedItemsArmsRare = 404,
		[ProtoEnum(Name = "ForgedItemsArmsPerfect", Value = 405)]
		ForgedItemsArmsPerfect = 405,
		[ProtoEnum(Name = "ForgedItemsShoulder", Value = 406)]
		ForgedItemsShoulder = 406,
		[ProtoEnum(Name = "ForgedItemsShoulderMundane", Value = 407)]
		ForgedItemsShoulderMundane = 407,
		[ProtoEnum(Name = "ForgedItemsShoulderCommon", Value = 408)]
		ForgedItemsShoulderCommon = 408,
		[ProtoEnum(Name = "ForgedItemsShoulderUncommon", Value = 409)]
		ForgedItemsShoulderUncommon = 409,
		[ProtoEnum(Name = "ForgedItemsShoulderRare", Value = 410)]
		ForgedItemsShoulderRare = 410,
		[ProtoEnum(Name = "ForgedItemsShoulderPerfect", Value = 411)]
		ForgedItemsShoulderPerfect = 411,
		[ProtoEnum(Name = "ForgedItemsInsignia", Value = 412)]
		ForgedItemsInsignia = 412,
		[ProtoEnum(Name = "ForgedItemsInsigniaMundane", Value = 413)]
		ForgedItemsInsigniaMundane = 413,
		[ProtoEnum(Name = "ForgedItemsInsigniaCommon", Value = 414)]
		ForgedItemsInsigniaCommon = 414,
		[ProtoEnum(Name = "ForgedItemsInsigniaUncommon", Value = 415)]
		ForgedItemsInsigniaUncommon = 415,
		[ProtoEnum(Name = "ForgedItemsInsigniaRare", Value = 416)]
		ForgedItemsInsigniaRare = 416,
		[ProtoEnum(Name = "ForgedItemsInsigniaPerfect", Value = 417)]
		ForgedItemsInsigniaPerfect = 417,
		[ProtoEnum(Name = "ForgeConsumedItems", Value = 418)]
		ForgeConsumedItems = 418,
		[ProtoEnum(Name = "ForgeConsumedItemsMundane", Value = 419)]
		ForgeConsumedItemsMundane = 419,
		[ProtoEnum(Name = "ForgeConsumedItemsCommon", Value = 420)]
		ForgeConsumedItemsCommon = 420,
		[ProtoEnum(Name = "ForgeConsumedItemsUncommon", Value = 421)]
		ForgeConsumedItemsUncommon = 421,
		[ProtoEnum(Name = "ForgeConsumedItemsRare", Value = 422)]
		ForgeConsumedItemsRare = 422,
		[ProtoEnum(Name = "ForgeConsumedItemsPerfect", Value = 423)]
		ForgeConsumedItemsPerfect = 423,
		[ProtoEnum(Name = "ForgeConsumedItemsWeapon", Value = 424)]
		ForgeConsumedItemsWeapon = 424,
		[ProtoEnum(Name = "ForgeConsumedItemsWeaponMundane", Value = 425)]
		ForgeConsumedItemsWeaponMundane = 425,
		[ProtoEnum(Name = "ForgeConsumedItemsWeaponCommon", Value = 426)]
		ForgeConsumedItemsWeaponCommon = 426,
		[ProtoEnum(Name = "ForgeConsumedItemsWeaponUncommon", Value = 427)]
		ForgeConsumedItemsWeaponUncommon = 427,
		[ProtoEnum(Name = "ForgeConsumedItemsWeaponRare", Value = 428)]
		ForgeConsumedItemsWeaponRare = 428,
		[ProtoEnum(Name = "ForgeConsumedItemsWeaponPerfect", Value = 429)]
		ForgeConsumedItemsWeaponPerfect = 429,
		[ProtoEnum(Name = "ForgeConsumedItemsBoots", Value = 430)]
		ForgeConsumedItemsBoots = 430,
		[ProtoEnum(Name = "ForgeConsumedItemsBootsMundane", Value = 431)]
		ForgeConsumedItemsBootsMundane = 431,
		[ProtoEnum(Name = "ForgeConsumedItemsBootsCommon", Value = 432)]
		ForgeConsumedItemsBootsCommon = 432,
		[ProtoEnum(Name = "ForgeConsumedItemsBootsUncommon", Value = 433)]
		ForgeConsumedItemsBootsUncommon = 433,
		[ProtoEnum(Name = "ForgeConsumedItemsBootsRare", Value = 434)]
		ForgeConsumedItemsBootsRare = 434,
		[ProtoEnum(Name = "ForgeConsumedItemsBootsPerfect", Value = 435)]
		ForgeConsumedItemsBootsPerfect = 435,
		[ProtoEnum(Name = "ForgeConsumedItemsLegs", Value = 436)]
		ForgeConsumedItemsLegs = 436,
		[ProtoEnum(Name = "ForgeConsumedItemsLegsMundane", Value = 437)]
		ForgeConsumedItemsLegsMundane = 437,
		[ProtoEnum(Name = "ForgeConsumedItemsLegsCommon", Value = 438)]
		ForgeConsumedItemsLegsCommon = 438,
		[ProtoEnum(Name = "ForgeConsumedItemsLegsUncommon", Value = 439)]
		ForgeConsumedItemsLegsUncommon = 439,
		[ProtoEnum(Name = "ForgeConsumedItemsLegsRare", Value = 440)]
		ForgeConsumedItemsLegsRare = 440,
		[ProtoEnum(Name = "ForgeConsumedItemsLegsPerfect", Value = 441)]
		ForgeConsumedItemsLegsPerfect = 441,
		[ProtoEnum(Name = "ForgeConsumedItemsChest", Value = 442)]
		ForgeConsumedItemsChest = 442,
		[ProtoEnum(Name = "ForgeConsumedItemsChestMundane", Value = 443)]
		ForgeConsumedItemsChestMundane = 443,
		[ProtoEnum(Name = "ForgeConsumedItemsChestCommon", Value = 444)]
		ForgeConsumedItemsChestCommon = 444,
		[ProtoEnum(Name = "ForgeConsumedItemsChestUncommon", Value = 445)]
		ForgeConsumedItemsChestUncommon = 445,
		[ProtoEnum(Name = "ForgeConsumedItemsChestRare", Value = 446)]
		ForgeConsumedItemsChestRare = 446,
		[ProtoEnum(Name = "ForgeConsumedItemsChestPerfect", Value = 447)]
		ForgeConsumedItemsChestPerfect = 447,
		[ProtoEnum(Name = "ForgeConsumedItemsArms", Value = 448)]
		ForgeConsumedItemsArms = 448,
		[ProtoEnum(Name = "ForgeConsumedItemsArmsMundane", Value = 449)]
		ForgeConsumedItemsArmsMundane = 449,
		[ProtoEnum(Name = "ForgeConsumedItemsArmsCommon", Value = 450)]
		ForgeConsumedItemsArmsCommon = 450,
		[ProtoEnum(Name = "ForgeConsumedItemsArmsUncommon", Value = 451)]
		ForgeConsumedItemsArmsUncommon = 451,
		[ProtoEnum(Name = "ForgeConsumedItemsArmsRare", Value = 452)]
		ForgeConsumedItemsArmsRare = 452,
		[ProtoEnum(Name = "ForgeConsumedItemsArmsPerfect", Value = 453)]
		ForgeConsumedItemsArmsPerfect = 453,
		[ProtoEnum(Name = "ForgeConsumedItemsShoulder", Value = 454)]
		ForgeConsumedItemsShoulder = 454,
		[ProtoEnum(Name = "ForgeConsumedItemsShoulderMundane", Value = 455)]
		ForgeConsumedItemsShoulderMundane = 455,
		[ProtoEnum(Name = "ForgeConsumedItemsShoulderCommon", Value = 456)]
		ForgeConsumedItemsShoulderCommon = 456,
		[ProtoEnum(Name = "ForgeConsumedItemsShoulderUncommon", Value = 457)]
		ForgeConsumedItemsShoulderUncommon = 457,
		[ProtoEnum(Name = "ForgeConsumedItemsShoulderRare", Value = 458)]
		ForgeConsumedItemsShoulderRare = 458,
		[ProtoEnum(Name = "ForgeConsumedItemsShoulderPerfect", Value = 459)]
		ForgeConsumedItemsShoulderPerfect = 459,
		[ProtoEnum(Name = "ForgeConsumedItemsInsignia", Value = 460)]
		ForgeConsumedItemsInsignia = 460,
		[ProtoEnum(Name = "ForgeConsumedItemsInsigniaMundane", Value = 461)]
		ForgeConsumedItemsInsigniaMundane = 461,
		[ProtoEnum(Name = "ForgeConsumedItemsInsigniaCommon", Value = 462)]
		ForgeConsumedItemsInsigniaCommon = 462,
		[ProtoEnum(Name = "ForgeConsumedItemsInsigniaUncommon", Value = 463)]
		ForgeConsumedItemsInsigniaUncommon = 463,
		[ProtoEnum(Name = "ForgeConsumedItemsInsigniaRare", Value = 464)]
		ForgeConsumedItemsInsigniaRare = 464,
		[ProtoEnum(Name = "ForgeConsumedItemsInsigniaPerfect", Value = 465)]
		ForgeConsumedItemsInsigniaPerfect = 465,
		[ProtoEnum(Name = "ForgeConsumedItemsOutfit", Value = 466)]
		ForgeConsumedItemsOutfit = 466,
		[ProtoEnum(Name = "ForgeConsumedItemsOutfitMundane", Value = 467)]
		ForgeConsumedItemsOutfitMundane = 467,
		[ProtoEnum(Name = "ForgeConsumedItemsOutfitCommon", Value = 468)]
		ForgeConsumedItemsOutfitCommon = 468,
		[ProtoEnum(Name = "ForgeConsumedItemsOutfitUncommon", Value = 469)]
		ForgeConsumedItemsOutfitUncommon = 469,
		[ProtoEnum(Name = "ForgeConsumedItemsOutfitRare", Value = 470)]
		ForgeConsumedItemsOutfitRare = 470,
		[ProtoEnum(Name = "ForgeConsumedItemsOutfitPerfect", Value = 471)]
		ForgeConsumedItemsOutfitPerfect = 471,
		[ProtoEnum(Name = "EquippedBoosts", Value = 472)]
		EquippedBoosts = 472,
		[ProtoEnum(Name = "EquippedBoostsMundane", Value = 473)]
		EquippedBoostsMundane = 473,
		[ProtoEnum(Name = "EquippedBoostsCommon", Value = 474)]
		EquippedBoostsCommon = 474,
		[ProtoEnum(Name = "EquippedBoostsUncommon", Value = 475)]
		EquippedBoostsUncommon = 475,
		[ProtoEnum(Name = "EquippedBoostsRare", Value = 476)]
		EquippedBoostsRare = 476,
		[ProtoEnum(Name = "EquippedBoostsPerfect", Value = 477)]
		EquippedBoostsPerfect = 477,
		[ProtoEnum(Name = "EquippedBoostsDamage", Value = 478)]
		EquippedBoostsDamage = 478,
		[ProtoEnum(Name = "EquippedBoostsDamageMundane", Value = 479)]
		EquippedBoostsDamageMundane = 479,
		[ProtoEnum(Name = "EquippedBoostsDamageCommon", Value = 480)]
		EquippedBoostsDamageCommon = 480,
		[ProtoEnum(Name = "EquippedBoostsDamageUncommon", Value = 481)]
		EquippedBoostsDamageUncommon = 481,
		[ProtoEnum(Name = "EquippedBoostsDamageRare", Value = 482)]
		EquippedBoostsDamageRare = 482,
		[ProtoEnum(Name = "EquippedBoostsDamagePerfect", Value = 483)]
		EquippedBoostsDamagePerfect = 483,
		[ProtoEnum(Name = "EquippedBoostsDefense", Value = 484)]
		EquippedBoostsDefense = 484,
		[ProtoEnum(Name = "EquippedBoostsDefenseMundane", Value = 485)]
		EquippedBoostsDefenseMundane = 485,
		[ProtoEnum(Name = "EquippedBoostsDefenseCommon", Value = 486)]
		EquippedBoostsDefenseCommon = 486,
		[ProtoEnum(Name = "EquippedBoostsDefenseUncommon", Value = 487)]
		EquippedBoostsDefenseUncommon = 487,
		[ProtoEnum(Name = "EquippedBoostsDefenseRare", Value = 488)]
		EquippedBoostsDefenseRare = 488,
		[ProtoEnum(Name = "EquippedBoostsDefensePerfect", Value = 489)]
		EquippedBoostsDefensePerfect = 489,
		[ProtoEnum(Name = "EquippedBoostsSc", Value = 490)]
		EquippedBoostsSc = 490,
		[ProtoEnum(Name = "EquippedBoostsScMundane", Value = 491)]
		EquippedBoostsScMundane = 491,
		[ProtoEnum(Name = "EquippedBoostsScCommon", Value = 492)]
		EquippedBoostsScCommon = 492,
		[ProtoEnum(Name = "EquippedBoostsScUncommon", Value = 493)]
		EquippedBoostsScUncommon = 493,
		[ProtoEnum(Name = "EquippedBoostsScRare", Value = 494)]
		EquippedBoostsScRare = 494,
		[ProtoEnum(Name = "EquippedBoostsScPerfect", Value = 495)]
		EquippedBoostsScPerfect = 495,
		[ProtoEnum(Name = "EquippedBoostsStealth", Value = 496)]
		EquippedBoostsStealth = 496,
		[ProtoEnum(Name = "EquippedBoostsStealthMundane", Value = 497)]
		EquippedBoostsStealthMundane = 497,
		[ProtoEnum(Name = "EquippedBoostsStealthCommon", Value = 498)]
		EquippedBoostsStealthCommon = 498,
		[ProtoEnum(Name = "EquippedBoostsStealthUncommon", Value = 499)]
		EquippedBoostsStealthUncommon = 499,
		[ProtoEnum(Name = "EquippedBoostsStealthRare", Value = 500)]
		EquippedBoostsStealthRare = 500,
		[ProtoEnum(Name = "EquippedBoostsStealthPerfect", Value = 501)]
		EquippedBoostsStealthPerfect = 501,
		[ProtoEnum(Name = "EquippedBoostsXp", Value = 502)]
		EquippedBoostsXp = 502,
		[ProtoEnum(Name = "EquippedBoostsXpMundane", Value = 503)]
		EquippedBoostsXpMundane = 503,
		[ProtoEnum(Name = "EquippedBoostsXpCommon", Value = 504)]
		EquippedBoostsXpCommon = 504,
		[ProtoEnum(Name = "EquippedBoostsXpUncommon", Value = 505)]
		EquippedBoostsXpUncommon = 505,
		[ProtoEnum(Name = "EquippedBoostsXpRare", Value = 506)]
		EquippedBoostsXpRare = 506,
		[ProtoEnum(Name = "EquippedBoostsXpPerfect", Value = 507)]
		EquippedBoostsXpPerfect = 507,
		[ProtoEnum(Name = "EliteMissionsWon", Value = 535)]
		EliteMissionsWon = 535,
		[ProtoEnum(Name = "MissionsCompletedWithCourtesansSent", Value = 536)]
		MissionsCompletedWithCourtesansSent = 536,
		[ProtoEnum(Name = "MissionsCompletedWithDoctorsUsed", Value = 537)]
		MissionsCompletedWithDoctorsUsed = 537,
		[ProtoEnum(Name = "EarnedSocialCurrency", Value = 540)]
		EarnedSocialCurrency = 540,
		[ProtoEnum(Name = "OpenedMysteryBoxesBooty", Value = 550)]
		OpenedMysteryBoxesBooty = 550,
		[ProtoEnum(Name = "OpenedMysteryBoxesBootyMundane", Value = 551)]
		OpenedMysteryBoxesBootyMundane = 551,
		[ProtoEnum(Name = "OpenedMysteryBoxesBootyCommon", Value = 552)]
		OpenedMysteryBoxesBootyCommon = 552,
		[ProtoEnum(Name = "OpenedMysteryBoxesBootyUncommon", Value = 553)]
		OpenedMysteryBoxesBootyUncommon = 553,
		[ProtoEnum(Name = "OpenedMysteryBoxesBootyRare", Value = 554)]
		OpenedMysteryBoxesBootyRare = 554,
		[ProtoEnum(Name = "OpenedMysteryBoxesBootyPerfect", Value = 555)]
		OpenedMysteryBoxesBootyPerfect = 555,
		[ProtoEnum(Name = "OpenedMysteryBoxesMovie", Value = 556)]
		OpenedMysteryBoxesMovie = 556,
		[ProtoEnum(Name = "OpenedMysteryBoxesMovieMundane", Value = 557)]
		OpenedMysteryBoxesMovieMundane = 557,
		[ProtoEnum(Name = "OpenedMysteryBoxesMovieCommon", Value = 558)]
		OpenedMysteryBoxesMovieCommon = 558,
		[ProtoEnum(Name = "OpenedMysteryBoxesMovieUncommon", Value = 559)]
		OpenedMysteryBoxesMovieUncommon = 559,
		[ProtoEnum(Name = "OpenedMysteryBoxesMovieRare", Value = 560)]
		OpenedMysteryBoxesMovieRare = 560,
		[ProtoEnum(Name = "OpenedMysteryBoxesMoviePerfect", Value = 561)]
		OpenedMysteryBoxesMoviePerfect = 561,
		[ProtoEnum(Name = "MissionsCompletedFrightenedTimeInSeconds", Value = 562)]
		MissionsCompletedFrightenedTimeInSeconds = 562,
		[ProtoEnum(Name = "MissionsCompletedEnemiesKilledByBomb", Value = 563)]
		MissionsCompletedEnemiesKilledByBomb = 563,
		[ProtoEnum(Name = "MissionsCompletedReviveCount", Value = 564)]
		MissionsCompletedReviveCount = 564,
		[ProtoEnum(Name = "MissionsCompletedSecondsTracedBySeekers", Value = 565)]
		MissionsCompletedSecondsTracedBySeekers = 565,
		[ProtoEnum(Name = "MissionsCompletedWithNoSurvivors", Value = 566)]
		MissionsCompletedWithNoSurvivors = 566
	}
}
