using System.Collections.Generic;
using Database;
using UbiCore.Utils;

public class GlobalGameIDs : UbiCore.Utils.Singleton<GlobalGameIDs>
{
	public GameID m_alternativeCharacterMesh;

	public GameID m_animatorCourtesan;

	public GameID m_animatorDoctor;

	public GameID m_animatorEnemy;

	public GameID m_animatorFemale;

	public GameID m_animatorMale;

	public GameID m_animatorNoncombatNpc;

	public GameID m_animatorPlayer;

	public GameID m_animatorPlayerAnimus;

	public GameID m_enemy;

	public GameID m_femaleCrowd;

	public GameID m_invalidEffectPackage;

	public GameID m_maleCrowd;

	public GameID m_silhouette;

	public GameID m_envInPigeonDynamic;

	public GameID m_envInPigeonDelivery;

	public GameID m_envInPigeonTrigger;

	public GameID m_envInPigeonCoopDynamic;

	public GameID m_envInHaystackDynamic;

	public GameID m_envInRoofGardenDynamic;

	public GameID TintableBackground1X1GameId;

	public GameID TintableBackground1X2GameId;

	public GameID TintableBackground2X1GameId;

	public Dictionary<string, GameID> AdvertiesmentBackgroundItems = new Dictionary<string, GameID>();

	public Dictionary<string, Dictionary<string, GameID>> AdvertiesmentBackgroundColorTables = new Dictionary<string, Dictionary<string, GameID>>();

	public GameID AssassinPreviewPopupBackgroundColor;

	public GameID HeroicShopCategoryNameColor;

	public GameID HeroicShopFrameColor;

	public GameID HeroicShopBackgroundColor;

	public GameID HeroicShopGlowColor;

	public GameID DiscountCrossOutLineColorBright;

	public GameID DiscountCrossOutLineColorDark;

	public GameID DiscountOldPriceColorDark;

	public GameID DiscountOldPriceColorBright;

	public GameID CampaignUnlockBackgroundColor;

	public GameID CampaignUnlockTitleColor;

	public GameID CampaignUnlockExtraTitleColor;

	public GameID NoItemQualityColor;

	public GameID m_mundaneColor;

	public GameID m_commonColor;

	public GameID m_uncommonColor;

	public GameID m_rareColor;

	public GameID m_perfectColor;

	public GameID m_mundaneTextColor;

	public GameID m_commonTextColor;

	public GameID m_uncommonTextColor;

	public GameID m_rareTextColor;

	public GameID m_perfectTextColor;

	public GameID m_skillIconTraceless;

	public GameID m_skillIconFrenzy;

	public GameID m_effectCodex;

	public GameID m_effectChestHighlight;

	public GameID m_startCodexEntryEra;

	public GameID m_startCodexEntryCategory;

	public GameID m_startCodexEntryEntry;

	public GameID ShopcatHeroic;

	public GameID ShopcatModules;

	public GameID ShopcatStarterPacks;

	public GameID ShopcatMysteryBoxes;

	public GameID ShopcatItems;

	public GameID ShopcatForge;

	public GameID EventScoreCurrency;

	public GameID PirateThemeEventScoreCurrency;

	public GameID m_hardCurrency;

	public GameID m_softCurrency;

	public GameID m_softCurrencyStack;

	public GameID m_heroicTokenCurrency;

	public GameID m_assassinSlots;

	public GameID m_itemMailGroup;

	public GameID m_additionalLootChest;

	public GameID m_blueprintEmpty;

	public GameID m_blueprintWeapon;

	public GameID m_blueprintBoots;

	public GameID m_blueprintLegs;

	public GameID m_blueprintChest;

	public GameID m_blueprintArms;

	public GameID m_blueprintShoulder;

	public GameID m_blueprintInsignia;

	public GameID m_blueprintOutfit;

	public GameID m_unkownOutcomeGUID;

	public GameID BlueprintFrameColor;

	public GameID BlueprintGlowColor;

	public GameID UnknownWeapon;

	public GameID UnknownShoulder;

	public GameID UnknownChest;

	public GameID UnknownInsignia;

	public GameID UnknownLegs;

	public GameID UnknownBoot;

	public GameID UnknownHiddenblade;

	public GameID UnknownOutfit;

	public GameID UnknownItem;

	public GameID UnknownMysteryBox;

	public GameID LockpicksFrameColor;

	public GameID LockpicksGlowColor;

	public GameID m_mysteryBoxItemPrefab;

	public GameID m_mysteryBoxRessourcePrefab;

	public GameID m_mysteryBoxMiscPrefab;

	public GameID MysterBoxItemsMundane;

	public GameID MysterBoxItemsCommon;

	public GameID MysterBoxItemsUncommon;

	public GameID MysterBoxItemsRare;

	public GameID MysterBoxItemsPerfect;

	public GameID MysterBoxCurrencyMundane;

	public GameID MysterBoxCurrencyCommon;

	public GameID MysterBoxCurrencyUncommon;

	public GameID MysterBoxCurrencyRare;

	public GameID MysterBoxCurrencyPerfect;

	public GameID HeroicRank0PortraitItem;

	public GameID HeroicRank1PortraitItem;

	public GameID HeroicRank2PortraitItem;

	public GameID HeroicRank3PortraitItem;

	public GameID HeroicRank4PortraitItem;

	public GameID HeroicRank5PortraitItem;

	public GameID EagleVisionEntitlement;

	public GameID HidingSpotKillsEntitlement;

	public GameID DoctorEntitlement;

	public GameID CourtesansEntitlement;

	public GameID HirelingEntitlement;

	public GameID EquipSlotInsigniaEntitlement;

	public GameID EquipSlotModuleEntitlement;

	public GameID EquipSlotOutfitEntitlement;

	public GameID FusionEntitlement;

	public GameID CompletedCampaignItalyEntitlement;

	public GameID MysteryBoxEntitlement;

	public GameID SkillTreeEntitlement;

	public GameID ShopEntitlement;

	public GameID EquipmentEntitlement;

	public GameID FriendingEntitlement;

	public GameID DailyQuestsEntitlement;

	public GameID ChallengesEntitlement;

	public GameID ContractsEntitlement;

	public GameID UnlockedBerserkerEntitlement;

	public GameID UnlockedShadowbladeEntitlement;

	public GameID UnlockedTricksterEntitlement;

	public GameID UnlockedThiefEntitlement;

	public GameID m_montrealRegion;

	public GameID ItemLevelReductionSkill;

	public GameID ItemPowerBonusSkill;

	public GameID ArmorRatingBonusSkill;

	public GameID WeaponDamageBonusSkill;

	public static readonly string ItemLevelReductionSkillGuidStr = "f6759e87-5c90-41e2-afdf-ac596ac37096";

	public static readonly string ItemPowerBonusSkillStr = "b725e418-5bfb-4cb0-8263-05b86780e4e3";

	public static readonly string ArmorRatingBonusSkillGuidStr = "e78e8de7-07cd-46f0-bbf0-c5dde625dc4b";

	public static readonly string WeaponDamageBonusSkillGuidStr = "7224bc69-e973-40b9-853c-4db1e38b0672";

	public static readonly string BlueprintEmptyIconGuidStr = "21d920c1-e178-4b53-9aa5-2d8d55f4f081";

	public GameID HumanoidMilitia;

	public GameID HumanoidEliteGuid;

	public GameID HumanoidThiefGuid;

	public GameID HumanoidSeekerGuid;

	public GameID HumanoidAgileGuid;

	public GameID HumanoidArcherGuid;

	public GameID HumanoidLopurumGuid;

	public GameID HumanoidSquadLeaderGuid;

	public GameID HumanoidMercenaryGuid;

	public GameID HumanoidCrowssbowmanGuid;

	public GameID HumanoidCrowleaderGuid;

	public GameID HumanoidGuardCaptainGuid;

	public GameID HumanoidBruteGuid;

	public GameID HumanoidPapalGuardGuid;

	public GameID m_fallbackWeapon;

	public GameID m_floatingFloorEffect;

	public void InitializeSharedIDs(GameDB _gameDB)
	{
		EagleVisionEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.EagleVisionEntitlementGUID);
		HidingSpotKillsEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.HidingSpotKillsEntitlementGUID);
		DoctorEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.DoctorEntitlementGUID);
		CourtesansEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.CourtesansEntitlementGUID);
		HirelingEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.HirelingEntitlementGUID);
		CompletedCampaignItalyEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.CompletedCampaignItalyEntitlementGUID);
		EquipSlotInsigniaEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.EquipSlotInsigniaEntitlementGUID);
		EquipSlotModuleEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.EquipSlotModuleEntitlementGUID);
		EquipSlotOutfitEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.EquipSlotOutfitEntitlementGUID);
		FusionEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.FusionEntitlementGUID);
		MysteryBoxEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.MysteryBoxEntitlementGUID);
		SkillTreeEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.SkillTreeEntitlementGUID);
		ShopEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.ShopEntitlementGUID);
		EquipmentEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.EquipmentEntitlementGUID);
		FriendingEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.FriendingEntitlementGUID);
		DailyQuestsEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.DailyQuestsEntitlementGUID);
		ChallengesEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.ChallengesEntitlementGUID);
		ContractsEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.ContractsEntitlementGUID);
		UnlockedBerserkerEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.UnlockedBerserkerEntitlementID);
		UnlockedShadowbladeEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.UnlockedShadowbladeEntitlementID);
		UnlockedTricksterEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.UnlockedTricksterEntitlementID);
		UnlockedThiefEntitlement = _gameDB.FindGameIdFromGuid(StaticEntitlementIds.UnlockedThiefEntitlementID);
		ItemLevelReductionSkill = _gameDB.FindGameIdFromGuid(ItemLevelReductionSkillGuidStr);
		ItemPowerBonusSkill = _gameDB.FindGameIdFromGuid(ItemPowerBonusSkillStr);
		ArmorRatingBonusSkill = _gameDB.FindGameIdFromGuid(ArmorRatingBonusSkillGuidStr);
		WeaponDamageBonusSkill = _gameDB.FindGameIdFromGuid(WeaponDamageBonusSkillGuidStr);
		HumanoidMilitia = _gameDB.FindGameIdFromGuid("0f43713d-4977-4e9e-b709-281a3a290fa8");
		HumanoidEliteGuid = _gameDB.FindGameIdFromGuid("5f625b03-fa8b-4ae3-b8ed-e02ad8058176");
		HumanoidThiefGuid = _gameDB.FindGameIdFromGuid("7b0811f8-d3f5-4e6d-b54a-1cfb06b60845");
		HumanoidSeekerGuid = _gameDB.FindGameIdFromGuid("4e705478-a886-495d-a726-1da924a374a3");
		HumanoidAgileGuid = _gameDB.FindGameIdFromGuid("745ed809-1ddc-42f6-9f8b-09c88823515c");
		HumanoidArcherGuid = _gameDB.FindGameIdFromGuid("8a5ea1de-56f0-4676-a000-c33d48ef7697");
		HumanoidLopurumGuid = _gameDB.FindGameIdFromGuid("ae6a6e40-f76c-4166-8448-55bd14af328a");
		HumanoidSquadLeaderGuid = _gameDB.FindGameIdFromGuid("36514d67-4baf-4518-8486-c6e64dbd598e");
		HumanoidMercenaryGuid = _gameDB.FindGameIdFromGuid("58c1d7ef-4e26-4dca-8667-6aae65cee5e3");
		HumanoidCrowssbowmanGuid = _gameDB.FindGameIdFromGuid("038e33e8-304f-45d1-9378-909de1b823c3");
		HumanoidCrowleaderGuid = _gameDB.FindGameIdFromGuid("d0c03e64-e378-4d71-acc0-ee96ba29e15d");
		HumanoidGuardCaptainGuid = _gameDB.FindGameIdFromGuid("66fa8352-62be-4fdd-b752-e45e454e9f12");
		HumanoidBruteGuid = _gameDB.FindGameIdFromGuid("996ce653-a6c5-455e-a649-3d64ef36d9d8");
		HumanoidPapalGuardGuid = _gameDB.FindGameIdFromGuid("520620a5-024d-4633-aaf8-21a5f0db7903");
	}

	public void InitializeIDs(GameDB _gameDB)
	{
		m_animatorCourtesan = _gameDB.FindGameIdFromGuid("a75a3969-5107-49ed-b3ef-c0d258b761e5");
		m_animatorDoctor = _gameDB.FindGameIdFromGuid("32a81bd4-2143-4c06-b396-4774a502599d");
		m_animatorEnemy = _gameDB.FindGameIdFromGuid("23896412-14c1-4218-a758-a2bf9d0b8e00");
		m_animatorFemale = _gameDB.FindGameIdFromGuid("54c63a92-8b45-4ae8-9b1a-2ef174467720");
		m_animatorMale = _gameDB.FindGameIdFromGuid("685bfbc3-1f6b-4359-9bcd-629143520e17");
		m_animatorNoncombatNpc = _gameDB.FindGameIdFromGuid("370df3e3-5ceb-46be-8b4c-61e32868d0c4");
		m_animatorPlayer = _gameDB.FindGameIdFromGuid("d2eb7047-ed50-40c0-a1db-d3445729687f");
		m_animatorPlayerAnimus = _gameDB.FindGameIdFromGuid("31fa9d24-72a0-4c79-b9a4-2288042f78d2");
		m_femaleCrowd = _gameDB.FindGameIdFromGuid("ddac503d-625f-486e-a56d-7c552f579cbf");
		m_maleCrowd = _gameDB.FindGameIdFromGuid("9f772dc7-1c87-494c-a5eb-f96793122663");
		m_enemy = _gameDB.FindGameIdFromGuid("0f43713d-4977-4e9e-b709-281a3a290fa8");
		m_invalidEffectPackage = _gameDB.FindGameIdFromGuid("1489e22d-400e-4536-881f-e8f1ed836912");
		m_silhouette = _gameDB.FindGameIdFromGuid("a8b11884-3a2c-4c8a-afba-6d0e4ad6ae7e");
		m_alternativeCharacterMesh = _gameDB.FindGameIdFromGuid("0627036b-829e-4d2b-8e23-27456a9f5c73");
		m_envInPigeonDynamic = _gameDB.FindGameIdFromGuid("5644712f-2099-46f5-b813-0a5f097e0a6a");
		m_envInPigeonDelivery = _gameDB.FindGameIdFromGuid("7509d35e-681e-4d33-8098-0a2fbe7d095c");
		m_envInPigeonTrigger = _gameDB.FindGameIdFromGuid("82e740ca-b42a-4c32-831f-85ede96f1b91");
		m_envInPigeonCoopDynamic = _gameDB.FindGameIdFromGuid("8888f18e-0ecd-4fce-8bc8-9e76e760fdc0");
		m_envInHaystackDynamic = _gameDB.FindGameIdFromGuid("9bb9fc64-bf4b-49af-bed6-357ce61b08b2");
		m_envInRoofGardenDynamic = _gameDB.FindGameIdFromGuid("a468041b-70f4-4019-8337-f8d67d269158");
		AssassinPreviewPopupBackgroundColor = _gameDB.FindGameIdFromGuid("68bacf5f-5aa6-48e6-9a83-3c9900b5203f");
		HeroicShopCategoryNameColor = _gameDB.FindGameIdFromGuid("89f85bd6-1c7c-4770-99e0-f1aaa648092e");
		HeroicShopGlowColor = _gameDB.FindGameIdFromGuid("7775889a-8b67-46be-8e22-5136b899ecfb");
		HeroicShopFrameColor = _gameDB.FindGameIdFromGuid("fb80458c-2b0d-424a-92c6-8d0e0c77de2a");
		HeroicShopBackgroundColor = _gameDB.FindGameIdFromGuid("25041991-841d-413f-836b-d8636f103f1f");
		DiscountCrossOutLineColorBright = _gameDB.FindGameIdFromGuid("a71200d2-7e4f-49ab-907e-b5d0818ede15");
		DiscountCrossOutLineColorDark = _gameDB.FindGameIdFromGuid("d8e96cc5-b902-496b-b8c4-afa489e24732");
		DiscountOldPriceColorBright = _gameDB.FindGameIdFromGuid("6e14b97e-f195-471e-ab23-a1f2f98270ab");
		DiscountOldPriceColorDark = _gameDB.FindGameIdFromGuid("6d248332-e36c-4a38-b7b1-a0696872de3c");
		CampaignUnlockBackgroundColor = _gameDB.FindGameIdFromGuid("eb1aa821-5e6c-4098-923f-d99c2223e170");
		CampaignUnlockTitleColor = _gameDB.FindGameIdFromGuid("c881e254-9808-4eda-bcdd-940e328c3307");
		CampaignUnlockExtraTitleColor = _gameDB.FindGameIdFromGuid("c263e2ed-9a4f-4bd9-802e-bb35dda5dc88");
		NoItemQualityColor = _gameDB.FindGameIdFromGuid("3f0f0a0f-0066-42b3-80f4-1267d57c7210");
		m_mundaneColor = _gameDB.FindGameIdFromGuid("4d56a0b4-830a-40b3-9ce8-6edbdb4f818d");
		m_commonColor = _gameDB.FindGameIdFromGuid("b8f80e8d-fb12-413c-95df-175f20a1cf44");
		m_uncommonColor = _gameDB.FindGameIdFromGuid("45610e62-2dfc-46d9-b432-4f8fa0ecae1c");
		m_rareColor = _gameDB.FindGameIdFromGuid("df65542d-1110-43e0-ba0c-13dcb8e1115c");
		m_perfectColor = _gameDB.FindGameIdFromGuid("3d2004b6-fcf7-4d52-a0ef-326deaa1b4cc");
		m_mundaneTextColor = _gameDB.FindGameIdFromGuid("2aa3a012-ba9b-45d7-bb7e-aa3197d6d399");
		m_commonTextColor = _gameDB.FindGameIdFromGuid("07f742a2-1cbd-4caf-9cfa-05341c105bad");
		m_uncommonTextColor = _gameDB.FindGameIdFromGuid("ce6298fb-3df1-48f4-93a6-b245ce0f7f8c");
		m_rareTextColor = _gameDB.FindGameIdFromGuid("e25cb3d5-1ace-4a10-9e65-58d325ab9a35");
		m_perfectTextColor = _gameDB.FindGameIdFromGuid("5fac36c2-395b-475b-9392-645edf5038ab");
		m_skillIconTraceless = _gameDB.FindGameIdFromGuid("f7b54136-5f9d-4076-b38e-18bf06bada67");
		m_skillIconFrenzy = _gameDB.FindGameIdFromGuid("cdbc264f-d42f-4858-a13f-f0a0a5f15b61");
		m_effectCodex = _gameDB.FindGameIdFromGuid("7ea95bb8-c6c8-411c-9f94-4cdbf9b165c1");
		m_effectChestHighlight = _gameDB.FindGameIdFromGuid("75e04a8b-2c41-4e8f-af1c-41a675f8423f");
		ShopcatHeroic = _gameDB.FindGameIdFromGuid("b93a2f8c-a49f-4f6a-a4db-d156d8f5d8be");
		ShopcatModules = _gameDB.FindGameIdFromGuid("02752040-69f3-42c6-b873-824ac57d40be");
		ShopcatStarterPacks = _gameDB.FindGameIdFromGuid("a9cb2ce5-3d68-4340-b53a-a9715a29d482");
		ShopcatMysteryBoxes = _gameDB.FindGameIdFromGuid("ca1e6cf8-e769-4d26-8537-818dece81ced");
		ShopcatItems = _gameDB.FindGameIdFromGuid("6cdd3e95-bc07-4e97-b223-bbe955addb75");
		ShopcatForge = _gameDB.FindGameIdFromGuid("659afd4d-8230-4ea4-9288-ce923da95e50");
		PirateThemeEventScoreCurrency = _gameDB.FindGameIdFromGuid("65198e23-a857-430c-83ca-2742b6e72124");
		EventScoreCurrency = _gameDB.FindGameIdFromGuid("bcd98262-725b-4f9e-930d-59d3d202552c");
		m_hardCurrency = _gameDB.FindGameIdFromGuid("b67e59cc-d392-412b-81e0-b3175a33a772");
		m_softCurrency = _gameDB.FindGameIdFromGuid("1db64436-1dfc-459b-946d-a6e5b5e295c3");
		m_softCurrencyStack = _gameDB.FindGameIdFromGuid("d8ae98c4-2b6f-4bf1-a817-968500a95d63");
		m_heroicTokenCurrency = _gameDB.FindGameIdFromGuid("3a42ddaf-a5d3-4b8f-87d8-6b61300d7028");
		m_assassinSlots = _gameDB.FindGameIdFromGuid("e47349d6-816f-4193-995f-2929469e59dc");
		m_itemMailGroup = _gameDB.FindGameIdFromGuid("6101bab0-a352-4a9d-b819-011842aa840c");
		m_additionalLootChest = _gameDB.FindGameIdFromGuid("6101bab0-a352-4a9d-b819-011842aa840c");
		m_startCodexEntryEra = _gameDB.FindGameIdFromGuid("e06c02b0-5eb4-4768-b499-3c711b3ba3dc");
		m_startCodexEntryCategory = _gameDB.FindGameIdFromGuid("dab5017e-2eea-42cc-b8d8-df23a0d241bf");
		m_startCodexEntryEntry = _gameDB.FindGameIdFromGuid("29765290-1d10-4f47-8ea8-6a8eed4d07bb");
		UnknownWeapon = _gameDB.FindGameIdFromGuid("231c6ba2-c182-4918-877a-64cf556c8133");
		UnknownShoulder = _gameDB.FindGameIdFromGuid("3c5bff47-a61c-4d32-b328-7954ac49a2af");
		UnknownChest = _gameDB.FindGameIdFromGuid("3eac0abb-b1cd-4b7a-93bb-972cb586f8ef");
		UnknownInsignia = _gameDB.FindGameIdFromGuid("49cd90d6-9b74-4418-ac82-c9b31ed5f8e9");
		UnknownLegs = _gameDB.FindGameIdFromGuid("58cad5ea-1636-4e18-a4cb-3054c158ed8a");
		UnknownBoot = _gameDB.FindGameIdFromGuid("803886b5-87c8-4f2c-94fc-0ed8d0bd0da4");
		UnknownHiddenblade = _gameDB.FindGameIdFromGuid("cd546e8c-7bae-473d-8ab8-e126af06a064");
		UnknownOutfit = _gameDB.FindGameIdFromGuid("ce9c3d16-aa4c-49c1-9bc8-c4a4685a59d2");
		UnknownItem = _gameDB.FindGameIdFromGuid("ea9af566-6cfb-4ed1-9e82-cf0baf7c5925");
		UnknownMysteryBox = _gameDB.FindGameIdFromGuid("8aed7095-ac34-4b2b-9405-3853679a7427");
		m_mysteryBoxItemPrefab = _gameDB.FindGameIdFromGuid("fa81ea8f-bd73-476c-9c6f-456cc656fed0");
		m_mysteryBoxRessourcePrefab = _gameDB.FindGameIdFromGuid("8c795fc3-385b-4012-ad3c-da02f7812da3");
		m_mysteryBoxMiscPrefab = _gameDB.FindGameIdFromGuid("44d3c55e-f4f7-4bd2-b9f5-8b5ee36fb088");
		MysterBoxItemsMundane = _gameDB.FindGameIdFromGuid("7d934673-7308-49b0-b0b9-d3997e815097");
		MysterBoxItemsCommon = _gameDB.FindGameIdFromGuid("57361842-0189-4ae4-b0b4-9a0ae1ff5b80");
		MysterBoxItemsUncommon = _gameDB.FindGameIdFromGuid("67c0ecb7-8f6d-40a1-982a-0d13a7979419");
		MysterBoxItemsRare = _gameDB.FindGameIdFromGuid("0d68ee2c-a52f-4bfb-9a0a-ffb9767683d9");
		MysterBoxItemsPerfect = _gameDB.FindGameIdFromGuid("4a23528b-90ea-4e9f-83fc-c3fe798f53b8");
		MysterBoxCurrencyMundane = _gameDB.FindGameIdFromGuid("b8ccd967-7306-48c2-b407-e11043c7d786");
		MysterBoxCurrencyCommon = _gameDB.FindGameIdFromGuid("607a0572-682f-4bda-910f-dbc2ce0ffd15");
		MysterBoxCurrencyUncommon = _gameDB.FindGameIdFromGuid("4615e586-fe1b-4850-a024-b118524eafd1");
		MysterBoxCurrencyRare = _gameDB.FindGameIdFromGuid("f5f59ccc-13c8-45bb-a00f-05c4d4f7ab73");
		MysterBoxCurrencyPerfect = _gameDB.FindGameIdFromGuid("f49cce72-da6a-477c-b62e-d06001ee48e6");
		HeroicRank0PortraitItem = _gameDB.FindGameIdFromGuid("2ed71839-e1e6-4768-b449-c603b59c1c79");
		HeroicRank1PortraitItem = _gameDB.FindGameIdFromGuid("57981313-87b7-49b6-a658-fcc76714ce23");
		HeroicRank2PortraitItem = _gameDB.FindGameIdFromGuid("51af262a-c535-44c8-b022-01ad872e8a91");
		HeroicRank3PortraitItem = _gameDB.FindGameIdFromGuid("18a536da-dffd-4c3a-9d71-a4bd348a3032");
		HeroicRank4PortraitItem = _gameDB.FindGameIdFromGuid("a701bbe8-4097-4701-af3c-c084a353fc5e");
		HeroicRank5PortraitItem = _gameDB.FindGameIdFromGuid("acc68f14-ba82-479d-a0cd-dd9573cefa22");
		InitializeSharedIDs(_gameDB);
		m_montrealRegion = _gameDB.FindGameIdFromGuid("951b6f2d-1676-4914-b1be-275d6a4705fe");
		m_fallbackWeapon = _gameDB.FindGameIdFromGuid("5100eea8-6a95-46fb-a585-9d6404020caa");
		m_floatingFloorEffect = _gameDB.FindGameIdFromGuid("dc90ad8a-5387-4a4f-a12a-2959a9656ecc");
		m_blueprintEmpty = _gameDB.FindGameIdFromGuid(BlueprintEmptyIconGuidStr);
		m_blueprintWeapon = _gameDB.FindGameIdFromGuid("231c6ba2-c182-4918-877a-64cf556c8133");
		m_blueprintBoots = _gameDB.FindGameIdFromGuid("803886b5-87c8-4f2c-94fc-0ed8d0bd0da4");
		m_blueprintLegs = _gameDB.FindGameIdFromGuid("58cad5ea-1636-4e18-a4cb-3054c158ed8a");
		m_blueprintChest = _gameDB.FindGameIdFromGuid("3eac0abb-b1cd-4b7a-93bb-972cb586f8ef");
		m_blueprintArms = _gameDB.FindGameIdFromGuid("cd546e8c-7bae-473d-8ab8-e126af06a064");
		m_blueprintShoulder = _gameDB.FindGameIdFromGuid("3c5bff47-a61c-4d32-b328-7954ac49a2af");
		m_blueprintInsignia = _gameDB.FindGameIdFromGuid("49cd90d6-9b74-4418-ac82-c9b31ed5f8e9");
		m_blueprintOutfit = _gameDB.FindGameIdFromGuid("ce9c3d16-aa4c-49c1-9bc8-c4a4685a59d2");
		m_unkownOutcomeGUID = _gameDB.FindGameIdFromGuid("ea9af566-6cfb-4ed1-9e82-cf0baf7c5925");
		BlueprintFrameColor = _gameDB.FindGameIdFromGuid("d834a8dc-e861-4f47-9ae8-c7f97ae6964e");
		BlueprintGlowColor = _gameDB.FindGameIdFromGuid("9d68669a-8c03-4054-bb03-330a50a869d6");
		LockpicksFrameColor = _gameDB.FindGameIdFromGuid("dffc46e4-be64-478b-9672-bf1682f2301a");
		LockpicksGlowColor = _gameDB.FindGameIdFromGuid("85791c7a-272f-4ae5-86df-3530d0d1c6dc");
		TintableBackground1X1GameId = _gameDB.FindGameIdFromGuid("388e9f92-cac5-48d1-a5d9-cefb029dd3a4");
		TintableBackground1X2GameId = _gameDB.FindGameIdFromGuid("22def405-babb-4a23-88f2-681927fc26a3");
		TintableBackground2X1GameId = _gameDB.FindGameIdFromGuid("82931c67-59e7-4f52-84fb-76d665753597");
		UpdateAdvertiesmentBackgrounds(_gameDB);
	}

	private void UpdateAdvertiesmentBackgrounds(GameDB _gameDB)
	{
		AdvertiesmentBackgroundColorTables.Clear();
		List<KeyValuePair<GameID, DBEntry>> items = _gameDB.GetItems<ColorTableDef>();
		for (int i = 0; i < items.Count; i++)
		{
			KeyValuePair<GameID, DBEntry> keyValuePair = items[i];
			if (!keyValuePair.Value.Name.StartsWith("ad_background_"))
			{
				continue;
			}
			ColorTableDef colorTableDef = keyValuePair.Value as ColorTableDef;
			if (colorTableDef != null)
			{
				Dictionary<string, GameID> dictionary = new Dictionary<string, GameID>();
				for (int j = 0; j < colorTableDef.Colors.Count; j++)
				{
					GameID gameID = colorTableDef.Colors[j];
					ColorDef item = _gameDB.GetItem<ColorDef>(gameID);
					dictionary.Add(item.Name, gameID);
				}
				AdvertiesmentBackgroundColorTables.Add(keyValuePair.Value.Name, dictionary);
			}
		}
		AdvertiesmentBackgroundItems.Clear();
		List<KeyValuePair<GameID, DBEntry>> items2 = _gameDB.GetItems<ItemDef>();
		for (int k = 0; k < items2.Count; k++)
		{
			KeyValuePair<GameID, DBEntry> keyValuePair2 = items2[k];
			if (keyValuePair2.Value.Name.StartsWith("ad_background_"))
			{
				AdvertiesmentBackgroundItems.Add(keyValuePair2.Value.Name, keyValuePair2.Key);
			}
		}
	}
}
