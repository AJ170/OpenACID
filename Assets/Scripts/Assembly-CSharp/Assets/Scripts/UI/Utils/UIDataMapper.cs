using System;
using System.Collections.Generic;
using System.Diagnostics;
using AcierProtocol;
using Assets.Scripts.Mission.Logic.Stats;
using Assets.Scripts.UI.Localization;
using Assets.Scripts.UI.Types;
using Database;
using SharedBaseLib.Core;
using UbiCore.Utils;
using UnityEngine;

namespace Assets.Scripts.UI.Utils
{
	public class UIDataMapper : UbiCore.Utils.Singleton<UIDataMapper>
	{
		public static readonly string SHARED_ATLAS_NAME = "SharedAtlas";

		public static readonly string ANIMUS_ATLAS_NAME = "AnimusAtlas";

		public static readonly string INGAME_ATLAS_NAME = "InGame";

		public static readonly float GlobalScrollViewMomentum = 70f;

		public static TextColors UITextColors = new TextColors();

		private static Dictionary<int, string> m_availableLanguages;

		private static Dictionary<int, ButtonConfiguration> m_buttonConfigs;

		private Dictionary<int, LeaderboardTypeDescription> m_leaderboardTypeDescription;

		private Dictionary<int, ClassDescription> m_assassinClassDescriptions;

		private Dictionary<int, ShopCategoryDescription> m_shopCategoryDescriptions;

		private Dictionary<int, ShopProductLabelDescription> m_shopProductLabelDescriptions;

		private Dictionary<int, ItemCategoryDescription> m_itemCategoryDescriptions;

		private Dictionary<int, ItemQualityDescription> m_itemQualityDescriptions;

		private Dictionary<int, MysteryBoxDescription> m_itemMysteryBoxDescriptions;

		private Dictionary<int, MysteryBoxDescription> m_resourceMysteryBoxDescriptions;

		private Dictionary<int, ModifierDescription> m_modifierDescriptions;

		private Dictionary<int, AlertLevelDescription> m_alertLevelDescriptions;

		private Dictionary<int, HeroicRankDescription> m_heroicRankDescription;

		private Dictionary<int, AssassinStatDescription> m_statDescriptions;

		private BBDictionary<string, string> m_tutorialMissionDescription;

		private BBList<TutorialEntitlementDescription> m_tutorialEntitlementDescription;

		private BBList<EntitlementHighlightDescription> m_entitlementHighlightDescription;

		private Dictionary<int, ClassEntitlementDescription> m_classEntitlementDescriptions;

		private Dictionary<int, CurrencyDescription> m_currencyDescriptions;

		private Dictionary<int, UnitDescription> m_unitDescriptions;

		private Dictionary<int, RuleDescription> m_ruleDescriptions;

		private Dictionary<int, ThemeLocaDescription> m_themeLocaDescriptions;

		private Dictionary<string, ThemeIconsDescription> m_themeIconsDescriptions;

		private Dictionary<int, MoodDescription> m_moodDescriptions;

		private Dictionary<int, EraDescription> m_eraDescriptions;

		private Dictionary<int, ObjectiveDescription> m_objectiveDescriptions;

		private Dictionary<int, MissionKindDescription> m_missionKindDescriptions;

		private Dictionary<int, ResourceDescription> m_resourceDescriptions;

		private Dictionary<int, CustomItemDescription> m_customItemDescriptions;

		private Dictionary<int, ErrorDescription> m_errorDescription;

		private Dictionary<int, ErrorGroupDescription> m_errorGroupDescriptions;

		private BBDictionary<int, UIPath> m_friendRelationSprites;

		private BBDictionary<int, UIPath> m_playerLevelSprites;

		private Dictionary<int, SupportDescription> m_supportDescriptions;

		private Dictionary<int, PushNotificationSwitchDescription> m_pushNotificationSwitchDescriptions;

		private Dictionary<int, NewsFeedDescription> m_newsFeedDescription;

		private Dictionary<int, AchievementConfig> m_achievementConfigs;

		private Dictionary<int, FontConfig> m_fontConfigs;

		private Dictionary<int, ForgeConfig> m_forgeConfigs;

		private Dictionary<ELanguages, AbbreviatedNumberMapping> m_abbreviatedNumberMapping;

		private static readonly int[] PlayerLevelLevels = new int[4] { 0, 10, 20, 30 };

		private static readonly string[] PlayerLevelSprites = new string[4] { "UI/Texture/AvatarRank/aUI_icon_playerAvatarRank01", "UI/Texture/AvatarRank/aUI_icon_playerAvatarRank02", "UI/Texture/AvatarRank/aUI_icon_playerAvatarRank03", "UI/Texture/AvatarRank/aUI_icon_playerAvatarRank04" };

		public string EmptyModuleGUID
		{
			get
			{
				return "2f4570db-9517-4d65-b001-0f173ebcf066";
			}
		}

		public string EmptyItemGUID
		{
			get
			{
				return "f5e539f1-9527-4ad3-ac94-1f8302b78839";
			}
		}

		public Dictionary<int, LeaderboardTypeDescription> LeaderboardTypeDescription
		{
			get
			{
				if (m_leaderboardTypeDescription == null)
				{
					CreateLeaderboardTypeDescription();
				}
				return m_leaderboardTypeDescription;
			}
		}

		public Dictionary<int, ClassDescription> AssassinClassDescriptions
		{
			get
			{
				if (m_assassinClassDescriptions == null)
				{
					CreateAssassinClassDescriptions();
				}
				return m_assassinClassDescriptions;
			}
		}

		public Dictionary<int, ShopCategoryDescription> ShopCategoryDescriptions
		{
			get
			{
				if (m_shopCategoryDescriptions == null)
				{
					CreateShopCategoryDescriptions();
				}
				return m_shopCategoryDescriptions;
			}
		}

		public Dictionary<int, ShopProductLabelDescription> ShopProductLabelDescriptions
		{
			get
			{
				if (m_shopProductLabelDescriptions == null)
				{
					CreateShopProductLabelDescriptions();
				}
				return m_shopProductLabelDescriptions;
			}
		}

		public Dictionary<int, ItemCategoryDescription> ItemCategoryDescriptions
		{
			get
			{
				if (m_itemCategoryDescriptions == null)
				{
					CreateItemCategoryDescriptions();
				}
				return m_itemCategoryDescriptions;
			}
		}

		public Dictionary<int, ItemQualityDescription> ItemQualityDescriptions
		{
			get
			{
				if (m_itemQualityDescriptions == null)
				{
					CreateItemQualityDescriptions();
				}
				return m_itemQualityDescriptions;
			}
		}

		public Dictionary<int, MysteryBoxDescription> ItemMysteryBoxDescription
		{
			get
			{
				if (m_itemMysteryBoxDescriptions == null)
				{
					CreateItemMysteryBoxDescriptions();
				}
				return m_itemMysteryBoxDescriptions;
			}
		}

		public Dictionary<int, MysteryBoxDescription> ResourceMysteryBoxDescription
		{
			get
			{
				if (m_resourceMysteryBoxDescriptions == null)
				{
					CreateResourceMysteryBoxDescriptions();
				}
				return m_resourceMysteryBoxDescriptions;
			}
		}

		public Dictionary<int, ModifierDescription> ModifierDescriptions
		{
			get
			{
				if (m_modifierDescriptions == null)
				{
					CreateModifierDescriptions();
				}
				return m_modifierDescriptions;
			}
		}

		public Dictionary<int, AlertLevelDescription> AlertLevelDescriptions
		{
			get
			{
				if (m_alertLevelDescriptions == null)
				{
					CreateAlertLevelDesriptions();
				}
				return m_alertLevelDescriptions;
			}
		}

		public Dictionary<int, HeroicRankDescription> HeroicRankDescription
		{
			get
			{
				if (m_heroicRankDescription == null)
				{
					CreateHeroicRankDescription();
				}
				return m_heroicRankDescription;
			}
		}

		public Dictionary<int, AssassinStatDescription> StatDescriptions
		{
			get
			{
				if (m_statDescriptions == null)
				{
					CreateStatDescriptions();
				}
				return m_statDescriptions;
			}
		}

		public BBDictionary<string, string> TutorialMissionDescription
		{
			get
			{
				if (m_tutorialMissionDescription == null)
				{
					CreateTutorialMissionDescription();
				}
				return m_tutorialMissionDescription;
			}
		}

		public BBList<TutorialEntitlementDescription> TutorialEntitlementDescription
		{
			get
			{
				if (m_tutorialEntitlementDescription == null)
				{
					CreateTutorialEntitlementDescription();
				}
				return m_tutorialEntitlementDescription;
			}
		}

		public BBList<EntitlementHighlightDescription> EntitlementHighlightDescription
		{
			get
			{
				if (m_entitlementHighlightDescription == null)
				{
					CreateEntitlementHighlightDescription();
				}
				return m_entitlementHighlightDescription;
			}
		}

		public Dictionary<int, ClassEntitlementDescription> ClassEntitlementDescriptions
		{
			get
			{
				if (m_classEntitlementDescriptions == null)
				{
					CreateClassEntitlementDescriptions();
				}
				return m_classEntitlementDescriptions;
			}
		}

		public Dictionary<int, CurrencyDescription> CurrencyDescriptions
		{
			get
			{
				if (m_currencyDescriptions == null)
				{
					CreateCurrencyDescriptions();
				}
				return m_currencyDescriptions;
			}
		}

		public Dictionary<int, UnitDescription> UnitDescriptions
		{
			get
			{
				if (m_unitDescriptions == null)
				{
					CreateUnitDescriptions();
				}
				return m_unitDescriptions;
			}
		}

		public Dictionary<int, ForgeConfig> ForgeDescriptions
		{
			get
			{
				if (m_forgeConfigs == null)
				{
					CreateForgeConfig();
				}
				return m_forgeConfigs;
			}
		}

		public Dictionary<ELanguages, AbbreviatedNumberMapping> AbbreviatedNumberMapping
		{
			get
			{
				if (m_abbreviatedNumberMapping == null)
				{
					CreateAbbreviateNumbersMapping();
				}
				return m_abbreviatedNumberMapping;
			}
		}

		public Dictionary<int, RuleDescription> RuleDescriptions
		{
			get
			{
				if (m_ruleDescriptions == null)
				{
					CreateRuleDescriptions();
				}
				return m_ruleDescriptions;
			}
		}

		public Dictionary<int, ThemeLocaDescription> ThemeLocaDescriptions
		{
			get
			{
				if (m_themeLocaDescriptions == null)
				{
					CreateThemeLocaDescriptions();
				}
				return m_themeLocaDescriptions;
			}
		}

		public Dictionary<string, ThemeIconsDescription> ThemeIconsDescriptions
		{
			get
			{
				if (m_themeIconsDescriptions == null)
				{
					CreateThemeIconsDescriptions();
				}
				return m_themeIconsDescriptions;
			}
		}

		public Dictionary<int, MoodDescription> MoodDescriptions
		{
			get
			{
				if (m_moodDescriptions == null)
				{
					CreateMoodDescriptions();
				}
				return m_moodDescriptions;
			}
		}

		public Dictionary<int, EraDescription> EraDescriptions
		{
			get
			{
				if (m_eraDescriptions == null)
				{
					CreateEraDescriptions();
				}
				return m_eraDescriptions;
			}
		}

		public Dictionary<int, ObjectiveDescription> ObjectiveDescriptions
		{
			get
			{
				if (m_objectiveDescriptions == null)
				{
					CreateObjectiveDescriptions();
				}
				return m_objectiveDescriptions;
			}
		}

		public Dictionary<int, MissionKindDescription> MissionKindDescriptions
		{
			get
			{
				if (m_missionKindDescriptions == null)
				{
					CreateMissionKindDescriptions();
				}
				return m_missionKindDescriptions;
			}
		}

		public Dictionary<int, ResourceDescription> ResourceDescriptions
		{
			get
			{
				if (m_resourceDescriptions == null)
				{
					CreateResourceDescriptions();
				}
				return m_resourceDescriptions;
			}
		}

		public Dictionary<int, CustomItemDescription> CustomItemDescriptions
		{
			get
			{
				if (m_customItemDescriptions == null)
				{
					CreateCustomItemDescriptions();
				}
				return m_customItemDescriptions;
			}
		}

		public Dictionary<int, ErrorDescription> ErrorDescriptions
		{
			get
			{
				if (m_errorDescription == null)
				{
					CreateErrorDescriptions();
				}
				return m_errorDescription;
			}
		}

		public Dictionary<int, ErrorGroupDescription> ErrorGroupDescriptions
		{
			get
			{
				if (m_errorGroupDescriptions == null)
				{
					CreateErrorGroupDescriptions();
				}
				return m_errorGroupDescriptions;
			}
		}

		public BBDictionary<int, UIPath> FriendRelationSprite
		{
			get
			{
				if (m_friendRelationSprites == null)
				{
					CreateFriendRelationSprites();
				}
				return m_friendRelationSprites;
			}
		}

		public BBDictionary<int, UIPath> PlayerLevelSprite
		{
			get
			{
				if (m_playerLevelSprites == null)
				{
					CreatePlayerLevelSprites();
				}
				return m_playerLevelSprites;
			}
		}

		public Dictionary<int, SupportDescription> SupportDescription
		{
			get
			{
				if (m_supportDescriptions == null)
				{
					CreateSupportDescription();
				}
				return m_supportDescriptions;
			}
		}

		public Dictionary<int, PushNotificationSwitchDescription> PushNotificationSwitchDescription
		{
			get
			{
				if (m_pushNotificationSwitchDescriptions == null)
				{
					CreatePushNotificationSwitchDescription();
				}
				return m_pushNotificationSwitchDescriptions;
			}
		}

		public static Dictionary<int, string> AvailableLanguages
		{
			get
			{
				if (m_availableLanguages == null)
				{
					CreateAvailableLanguages();
				}
				return m_availableLanguages;
			}
		}

		public Dictionary<int, FontConfig> FontConfigs
		{
			get
			{
				if (m_fontConfigs == null)
				{
					CreateFontConfigs();
				}
				return m_fontConfigs;
			}
		}

		public Dictionary<int, NewsFeedDescription> NewsFeedDescription
		{
			get
			{
				if (m_newsFeedDescription == null)
				{
					CreateNewsFeedDescription();
				}
				return m_newsFeedDescription;
			}
		}

		public static Dictionary<int, ButtonConfiguration> ButtonConfigurations
		{
			get
			{
				if (m_buttonConfigs == null)
				{
					CreateButtonConfigs();
				}
				return m_buttonConfigs;
			}
		}

		public Dictionary<int, AchievementConfig> AchievementConfigurations
		{
			get
			{
				if (m_achievementConfigs == null)
				{
					CreateAchievementsConfig();
				}
				return m_achievementConfigs;
			}
		}

		[Conditional("UNITY_RELEASE")]
		[Conditional("UNITY_DEBUG")]
		public static void Validate()
		{
		}

		private static void CreateAvailableLanguages()
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			dictionary.Add(0, "Localization/en_BB_bin");
			dictionary.Add(1, "Localization/English_US_bin");
			dictionary.Add(2, "Localization/English_GB_bin");
			dictionary.Add(3, "Localization/German_DE_bin");
			dictionary.Add(4, "Localization/Spanish_ES_bin");
			dictionary.Add(5, "Localization/Italian_IT_bin");
			dictionary.Add(6, "Localization/Japanese_JA_bin");
			dictionary.Add(7, "Localization/Korean_KO_bin");
			dictionary.Add(8, "Localization/Portuguese_BR_bin");
			dictionary.Add(9, "Localization/Russian_RU_bin");
			dictionary.Add(10, "Localization/SimpChinese_CH_bin");
			dictionary.Add(11, "Localization/TradChinese_TW_bin");
			dictionary.Add(12, "Localization/French_FR_bin");
			m_availableLanguages = dictionary;
		}

		private void CreateFontConfigs()
		{
			m_fontConfigs = new Dictionary<int, FontConfig>
			{
				{
					0,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					1,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					2,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					3,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					4,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					5,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					6,
					new FontConfig
					{
						GlyphOffsetY = 15f,
						LineSpacing = 0,
						MixedOffsetY = 5f
					}
				},
				{
					7,
					new FontConfig
					{
						GlyphOffsetY = 15f,
						LineSpacing = 0,
						MixedOffsetY = 5f
					}
				},
				{
					8,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					9,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					10,
					new FontConfig
					{
						GlyphOffsetY = 15f,
						LineSpacing = 0,
						MixedOffsetY = 5f
					}
				},
				{
					11,
					new FontConfig
					{
						GlyphOffsetY = 15f,
						LineSpacing = 0,
						MixedOffsetY = 5f
					}
				},
				{
					12,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				},
				{
					13,
					new FontConfig
					{
						GlyphOffsetY = 0f,
						LineSpacing = 0,
						MixedOffsetY = 0f
					}
				}
			};
		}

		private void CreateLeaderboardTypeDescription()
		{
			m_leaderboardTypeDescription = new Dictionary<int, LeaderboardTypeDescription>
			{
				{
					0,
					new LeaderboardTypeDescription
					{
						ScoreType = 6565,
						ScoreTypeIcon = "aUI_icon_statPower"
					}
				},
				{
					1,
					new LeaderboardTypeDescription
					{
						ScoreType = 6565,
						ScoreTypeIcon = "aUI_icon_statPower"
					}
				},
				{
					2,
					new LeaderboardTypeDescription
					{
						ScoreType = 6565,
						ScoreTypeIcon = "aUI_icon_statPower"
					}
				},
				{
					3,
					new LeaderboardTypeDescription
					{
						ScoreType = 9558,
						ScoreTypeIcon = "aUI_icon_eventstar"
					}
				},
				{
					4,
					new LeaderboardTypeDescription
					{
						ScoreType = 9558,
						ScoreTypeIcon = "aUI_icon_eventstar"
					}
				},
				{
					5,
					new LeaderboardTypeDescription
					{
						ScoreType = 9558,
						ScoreTypeIcon = "aUI_icon_eventstar"
					}
				}
			};
		}

		private void CreateAssassinClassDescriptions()
		{
			m_assassinClassDescriptions = new Dictionary<int, ClassDescription>
			{
				{
					2,
					new ClassDescription
					{
						Type = Assassin.EClassTypeId.Berserker,
						DescriptionId = new int[3] { 3497, 3498, 3499 },
						NameId = 298,
						AvatarCreation = "aUI_img_chr_baserker",
						TrainingTab = "aUI_img_chrDual_baserker",
						HirelingDescriptionData = new ClassDescription.HirelingDescription
						{
							ActiveNameId = 1515,
							ActiveDescriptionId = 1506,
							PassiveDescriptionId = 1507,
							PassiveNameId = 1516
						}
					}
				},
				{
					3,
					new ClassDescription
					{
						Type = Assassin.EClassTypeId.Shadowblade,
						DescriptionId = new int[3] { 3494, 3495, 3496 },
						NameId = 301,
						AvatarCreation = "aUI_img_chr_shadowblade",
						TrainingTab = "aUI_img_chrDual_shadowblade",
						HirelingDescriptionData = new ClassDescription.HirelingDescription
						{
							ActiveNameId = 1517,
							ActiveDescriptionId = 1508,
							PassiveDescriptionId = 1509,
							PassiveNameId = 1518
						}
					}
				},
				{
					5,
					new ClassDescription
					{
						Type = Assassin.EClassTypeId.Thief,
						DescriptionId = new int[3] { 3503, 3504, 3505 },
						NameId = 300,
						AvatarCreation = "aUI_img_chr_thief",
						TrainingTab = "aUI_img_chrDual_thief",
						HirelingDescriptionData = new ClassDescription.HirelingDescription
						{
							ActiveNameId = 1558,
							ActiveDescriptionId = 1559,
							PassiveDescriptionId = 1561,
							PassiveNameId = 1560
						}
					}
				},
				{
					4,
					new ClassDescription
					{
						Type = Assassin.EClassTypeId.Trickster,
						DescriptionId = new int[3] { 3500, 3501, 3502 },
						NameId = 302,
						AvatarCreation = "aUI_img_chr_trickster",
						TrainingTab = "aUI_img_chrDual_trickster",
						HirelingDescriptionData = new ClassDescription.HirelingDescription
						{
							ActiveNameId = 1554,
							ActiveDescriptionId = 1555,
							PassiveDescriptionId = 1557,
							PassiveNameId = 1556
						}
					}
				}
			};
		}

		private void CreateShopCategoryDescriptions()
		{
			m_shopCategoryDescriptions = new Dictionary<int, ShopCategoryDescription>
			{
				{
					10,
					new ShopCategoryDescription
					{
						SortId = 10,
						SortNoColonyId = 20,
						IconName = "aUI_img_shop_category_heroic_shop",
						CategoryNameId = 6825,
						ItemGameId = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.ShopcatHeroic
					}
				},
				{
					1,
					new ShopCategoryDescription
					{
						SortId = 0,
						SortNoColonyId = 10,
						IconName = "aUI_img_shop_category_hc_packages",
						CategoryNameId = 2243,
						ItemGameId = GameID.Invalid
					}
				},
				{
					3,
					new ShopCategoryDescription
					{
						SortId = 40,
						SortNoColonyId = 40,
						IconName = "aUI_img_shop_category_boosts",
						CategoryNameId = 2240,
						ItemGameId = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.ShopcatModules
					}
				},
				{
					14,
					new ShopCategoryDescription
					{
						SortId = 50,
						SortNoColonyId = 30,
						IconName = "aUI_img_shop_category_starter_packs",
						CategoryNameId = 6832,
						ItemGameId = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.ShopcatStarterPacks
					}
				},
				{
					13,
					new ShopCategoryDescription
					{
						SortId = 60,
						SortNoColonyId = 60,
						IconName = "aUI_img_shop_category_mystery_box",
						CategoryNameId = 6830,
						ItemGameId = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.ShopcatMysteryBoxes
					}
				},
				{
					9,
					new ShopCategoryDescription
					{
						SortId = 70,
						SortNoColonyId = 50,
						IconName = "aUI_img_shop_category_items",
						CategoryNameId = 6535,
						ItemGameId = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.ShopcatItems
					}
				},
				{
					11,
					new ShopCategoryDescription
					{
						SortId = 80,
						SortNoColonyId = 70,
						IconName = "aUI_img_shop_category_forge",
						CategoryNameId = 6833,
						ItemGameId = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.ShopcatForge
					}
				},
				{
					12,
					new ShopCategoryDescription
					{
						SortId = 30,
						SortNoColonyId = 30,
						IconName = "aUI_img_shop_category_items",
						CategoryNameId = 6831
					}
				},
				{
					7,
					new ShopCategoryDescription
					{
						SortId = 50,
						SortNoColonyId = 40,
						IconName = "aUI_img_shop_category_prestige",
						CategoryNameId = 2241
					}
				},
				{
					2,
					new ShopCategoryDescription
					{
						SortId = 60,
						SortNoColonyId = 40,
						IconName = "aUI_img_shop_category_premium",
						CategoryNameId = 2239
					}
				},
				{
					6,
					new ShopCategoryDescription
					{
						SortId = 20,
						SortNoColonyId = 40,
						IconName = "aUI_img_shop_category_decryption",
						CategoryNameId = 2242
					}
				}
			};
		}

		private void CreateShopProductLabelDescriptions()
		{
			m_shopProductLabelDescriptions = new Dictionary<int, ShopProductLabelDescription>
			{
				{
					2,
					new ShopProductLabelDescription
					{
						LocaKey = 7725,
						Icon = "aUI_fra_label_best_value_icon",
						Background = "aUI_fra_label_best_value",
						HeroicBackground = "aUI_fra_label_heroic_shop"
					}
				},
				{
					1,
					new ShopProductLabelDescription
					{
						LocaKey = 7726,
						Icon = "aUI_fra_label_most_popular_icon",
						Background = "aUI_fra_label_most_popular",
						HeroicBackground = "aUI_fra_label_heroic_shop"
					}
				}
			};
		}

		private void CreateItemMysteryBoxDescriptions()
		{
			m_itemMysteryBoxDescriptions = new Dictionary<int, MysteryBoxDescription>
			{
				{
					1,
					new MysteryBoxDescription
					{
						nameId = 6715,
						descriptionId = 7039
					}
				},
				{
					2,
					new MysteryBoxDescription
					{
						nameId = 6999,
						descriptionId = 7035
					}
				},
				{
					3,
					new MysteryBoxDescription
					{
						nameId = 6995,
						descriptionId = 7040
					}
				},
				{
					4,
					new MysteryBoxDescription
					{
						nameId = 7000,
						descriptionId = 7033
					}
				},
				{
					5,
					new MysteryBoxDescription
					{
						nameId = 7003,
						descriptionId = 7036
					}
				}
			};
		}

		private void CreateResourceMysteryBoxDescriptions()
		{
			m_resourceMysteryBoxDescriptions = new Dictionary<int, MysteryBoxDescription>
			{
				{
					1,
					new MysteryBoxDescription
					{
						nameId = 6720,
						descriptionId = 6731
					}
				},
				{
					2,
					new MysteryBoxDescription
					{
						nameId = 7001,
						descriptionId = 6731
					}
				},
				{
					3,
					new MysteryBoxDescription
					{
						nameId = 6997,
						descriptionId = 6731
					}
				},
				{
					4,
					new MysteryBoxDescription
					{
						nameId = 7002,
						descriptionId = 6731
					}
				},
				{
					5,
					new MysteryBoxDescription
					{
						nameId = 7004,
						descriptionId = 6731
					}
				}
			};
		}

		private void CreateItemCategoryDescriptions()
		{
			m_itemCategoryDescriptions = new Dictionary<int, ItemCategoryDescription>
			{
				{
					2,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_weapon",
						ValueUnitIdNormalCase = 1742,
						ValueUnitIdUpperCase = 1564,
						HelpTextId = 1638,
						CategoryNameId = 1727,
						MissingEntitlementMessage = StringId.Invalid
					}
				},
				{
					13,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_insignia",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1632,
						CategoryNameId = 1722,
						MissingEntitlementMessage = 7195
					}
				},
				{
					8,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_boots",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1639,
						CategoryNameId = 1724,
						MissingEntitlementMessage = StringId.Invalid
					}
				},
				{
					9,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_legs",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1635,
						CategoryNameId = 1728,
						MissingEntitlementMessage = StringId.Invalid
					}
				},
				{
					10,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_chest",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1633,
						CategoryNameId = 1723,
						MissingEntitlementMessage = StringId.Invalid
					}
				},
				{
					11,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_hiddenBlade",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1634,
						CategoryNameId = 1729,
						MissingEntitlementMessage = StringId.Invalid
					}
				},
				{
					12,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_shoulders",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1637,
						CategoryNameId = 1726,
						MissingEntitlementMessage = StringId.Invalid
					}
				},
				{
					14,
					new ItemCategoryDescription
					{
						IconName = "aUI_icon_outfit",
						ValueUnitIdNormalCase = 1741,
						ValueUnitIdUpperCase = 1563,
						HelpTextId = 1636,
						CategoryNameId = 1725,
						MissingEntitlementMessage = 7196
					}
				}
			};
		}

		private void CreateModifierDescriptions()
		{
			m_modifierDescriptions = new Dictionary<int, ModifierDescription>
			{
				{
					304,
					new ModifierDescription
					{
						UnitId = 4926
					}
				},
				{
					305,
					new ModifierDescription
					{
						UnitId = 4924
					}
				}
			};
		}

		private void CreateAlertLevelDesriptions()
		{
			m_alertLevelDescriptions = new Dictionary<int, AlertLevelDescription>
			{
				{
					1,
					new AlertLevelDescription
					{
						DescriptionId = 5759
					}
				},
				{
					0,
					new AlertLevelDescription
					{
						DescriptionId = 5758
					}
				},
				{
					2,
					new AlertLevelDescription
					{
						DescriptionId = 5760
					}
				}
			};
		}

		private void CreateHeroicRankDescription()
		{
			m_heroicRankDescription = new Dictionary<int, HeroicRankDescription>
			{
				{
					0,
					new HeroicRankDescription
					{
						NameId = 6765,
						PortraitItem = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicRank0PortraitItem,
						NormalIcon = "aUI_heroic_rank_badge_none",
						DisabledIcon = "aUI_heroic_rank_badge_none_disabled"
					}
				},
				{
					1,
					new HeroicRankDescription
					{
						NameId = 6766,
						PortraitItem = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicRank1PortraitItem,
						NormalIcon = "aUI_heroic_rank_badge_initiate",
						DisabledIcon = "aUI_heroic_rank_badge_initiate_disabled"
					}
				},
				{
					2,
					new HeroicRankDescription
					{
						NameId = 6767,
						PortraitItem = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicRank2PortraitItem,
						NormalIcon = "aUI_heroic_rank_badge_disciple",
						DisabledIcon = "aUI_heroic_rank_badge_disciple_disabled"
					}
				},
				{
					3,
					new HeroicRankDescription
					{
						NameId = 6770,
						PortraitItem = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicRank3PortraitItem,
						NormalIcon = "aUI_heroic_rank_badge_assassin",
						DisabledIcon = "aUI_heroic_rank_badge_assassin_disabled"
					}
				},
				{
					4,
					new HeroicRankDescription
					{
						NameId = 6768,
						PortraitItem = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicRank4PortraitItem,
						NormalIcon = "aUI_heroic_rank_badge_veteran",
						DisabledIcon = "aUI_heroic_rank_badge_veteran_disabled"
					}
				},
				{
					5,
					new HeroicRankDescription
					{
						NameId = 6769,
						PortraitItem = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HeroicRank5PortraitItem,
						NormalIcon = "aUI_heroic_rank_badge_master",
						DisabledIcon = "aUI_heroic_rank_badge_master_disabled"
					}
				}
			};
		}

		private void CreateStatDescriptions()
		{
			m_statDescriptions = new Dictionary<int, AssassinStatDescription>
			{
				{
					0,
					new AssassinStatDescription
					{
						NameId = 1740,
						DescriptionId = 1757,
						StatTextureIdentifier = "aUI_icon_statHealth",
						Primary = true
					}
				},
				{
					1,
					new AssassinStatDescription
					{
						NameId = 705,
						DescriptionId = 1756,
						StatTextureIdentifier = "aUI_icon_statStrength",
						Primary = true
					}
				},
				{
					5,
					new AssassinStatDescription
					{
						NameId = 1741,
						DescriptionId = 1758,
						StatTextureIdentifier = "aUI_icon_statArmorRating",
						Primary = true
					}
				},
				{
					2,
					new AssassinStatDescription
					{
						NameId = 701,
						DescriptionId = 1752,
						StatTextureIdentifier = "aUI_icon_statEvasion",
						Primary = true
					}
				},
				{
					3,
					new AssassinStatDescription
					{
						NameId = 704,
						DescriptionId = 1755,
						StatTextureIdentifier = "aUI_icon_statPrecision",
						Primary = true
					}
				},
				{
					4,
					new AssassinStatDescription
					{
						NameId = 703,
						DescriptionId = 1754,
						StatTextureIdentifier = "aUI_icon_statIncognito",
						Primary = true
					}
				},
				{
					7,
					new AssassinStatDescription
					{
						NameId = 1742,
						DescriptionId = 1759,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					10,
					new AssassinStatDescription
					{
						NameId = 6682,
						DescriptionId = 6683,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					13,
					new AssassinStatDescription
					{
						NameId = 1743,
						DescriptionId = 1760,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					8,
					new AssassinStatDescription
					{
						NameId = 1744,
						DescriptionId = 1761,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					9,
					new AssassinStatDescription
					{
						NameId = 1745,
						DescriptionId = 1762,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					12,
					new AssassinStatDescription
					{
						NameId = 1746,
						DescriptionId = 1763,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					15,
					new AssassinStatDescription
					{
						NameId = 1747,
						DescriptionId = 1764,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					14,
					new AssassinStatDescription
					{
						NameId = 1748,
						DescriptionId = 1765,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					17,
					new AssassinStatDescription
					{
						NameId = 1749,
						DescriptionId = 1766,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					18,
					new AssassinStatDescription
					{
						NameId = 1750,
						DescriptionId = 1767,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				},
				{
					16,
					new AssassinStatDescription
					{
						NameId = 5397,
						DescriptionId = 5396,
						StatTextureIdentifier = string.Empty,
						Primary = false
					}
				}
			};
		}

		private void CreateTutorialMissionDescription()
		{
			m_tutorialMissionDescription = new BBDictionary<string, string>
			{
				{ "tutorial_0002", "UI/Texture/Tutorial/aUI_img_tutorial_assassination" },
				{ "tutorial_0003", "UI/Texture/Tutorial/aUI_img_tutorial_eagle_vision" },
				{ "tutorial_0004", "UI/Texture/Tutorial/aUI_img_tutorial_hiding_spot_kill" },
				{ "tutorial_0005", "UI/Texture/Tutorial/aUI_img_tutorial_doctor" },
				{ "tutorial_0006", "UI/Texture/Tutorial/aUI_img_tutorial_courtesan" },
				{ "tutorial_0007", "UI/Texture/Tutorial/aUI_img_tutorial_hireling" },
				{ "tutorial_0008", "UI/Texture/Tutorial/aUI_img_tutorial_combat" },
				{ "tutorial_0009", "UI/Texture/Tutorial/aUI_img_tutorial_skills" },
				{ "tutorial_0010", "UI/Texture/Tutorial/aUI_img_tutorial_UI" },
				{ "tutorial_0011", "UI/Texture/Tutorial/aUI_img_tutorial_free_running" }
			};
		}

		private void CreateTutorialEntitlementDescription()
		{
			m_tutorialEntitlementDescription = new BBList<TutorialEntitlementDescription>
			{
				new TutorialEntitlementDescription
				{
					GUID = StaticEntitlementIds.EagleVisionEntitlementGUID,
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.EagleVisionEntitlement,
					MissionId = "tutorial_0003"
				},
				new TutorialEntitlementDescription
				{
					GUID = StaticEntitlementIds.HidingSpotKillsEntitlementGUID,
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HidingSpotKillsEntitlement,
					MissionId = "tutorial_0004"
				},
				new TutorialEntitlementDescription
				{
					GUID = StaticEntitlementIds.DoctorEntitlementGUID,
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.DoctorEntitlement,
					MissionId = "tutorial_0005"
				},
				new TutorialEntitlementDescription
				{
					GUID = StaticEntitlementIds.CourtesansEntitlementGUID,
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.CourtesansEntitlement,
					MissionId = "tutorial_0006"
				},
				new TutorialEntitlementDescription
				{
					GUID = StaticEntitlementIds.HirelingEntitlementGUID,
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HirelingEntitlement,
					MissionId = "tutorial_0007"
				}
			};
		}

		private void CreateEntitlementHighlightDescription()
		{
			m_entitlementHighlightDescription = new BBList<EntitlementHighlightDescription>
			{
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.EagleVisionEntitlement,
					ShowBeforeUnlock = false,
					ShowUnlock = true,
					DescriptionLocked = 6755,
					DescriptionUnlocked = 7275
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HidingSpotKillsEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6756,
					DescriptionUnlocked = 7276
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.DoctorEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6757,
					DescriptionUnlocked = 7278
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.CourtesansEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6758,
					DescriptionUnlocked = 7280
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.HirelingEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6759,
					DescriptionUnlocked = 7282
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.EquipSlotInsigniaEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6760,
					DescriptionUnlocked = 7283
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.EquipSlotModuleEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6761,
					DescriptionUnlocked = 7277
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.EquipSlotOutfitEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6762,
					DescriptionUnlocked = 7281
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.FusionEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6763,
					DescriptionUnlocked = 7279
				},
				new EntitlementHighlightDescription
				{
					EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.CompletedCampaignItalyEntitlement,
					ShowBeforeUnlock = true,
					ShowUnlock = true,
					DescriptionLocked = 6764,
					DescriptionUnlocked = 7284
				}
			};
		}

		private void CreateClassEntitlementDescriptions()
		{
			m_classEntitlementDescriptions = new Dictionary<int, ClassEntitlementDescription>();
			m_classEntitlementDescriptions.Add(2, new ClassEntitlementDescription
			{
				EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnlockedBerserkerEntitlement,
				DescriptionUnlocked = 7689,
				classType = Assassin.EClassTypeId.Berserker,
				entitlementID = StaticEntitlementIds.UnlockedBerserkerEntitlementID
			});
			m_classEntitlementDescriptions.Add(3, new ClassEntitlementDescription
			{
				EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnlockedShadowbladeEntitlement,
				DescriptionUnlocked = 7689,
				classType = Assassin.EClassTypeId.Shadowblade,
				entitlementID = StaticEntitlementIds.UnlockedShadowbladeEntitlementID
			});
			m_classEntitlementDescriptions.Add(4, new ClassEntitlementDescription
			{
				EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnlockedTricksterEntitlement,
				DescriptionUnlocked = 7689,
				classType = Assassin.EClassTypeId.Trickster,
				entitlementID = StaticEntitlementIds.UnlockedTricksterEntitlementID
			});
			m_classEntitlementDescriptions.Add(5, new ClassEntitlementDescription
			{
				EntitlementGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.UnlockedThiefEntitlement,
				DescriptionUnlocked = 7690,
				classType = Assassin.EClassTypeId.Thief,
				entitlementID = StaticEntitlementIds.UnlockedThiefEntitlementID
			});
		}

		private void CreateCurrencyDescriptions()
		{
			m_currencyDescriptions = new Dictionary<int, CurrencyDescription>
			{
				{
					2,
					new CurrencyDescription
					{
						IconName = "aUI_icon_currencySoft",
						SmallIconName = "aUI_icon_currencySoft_S",
						LocaKey = 2330
					}
				},
				{
					3,
					new CurrencyDescription
					{
						IconName = "aUI_icon_currencyHeroic",
						SmallIconName = "aUI_icon_currencyHeroic_S",
						LocaKey = 2331
					}
				},
				{
					1,
					new CurrencyDescription
					{
						IconName = "aUI_icon_currencyHard",
						SmallIconName = "aUI_icon_currencyHard_S",
						LocaKey = 2329
					}
				}
			};
		}

		private void CreateUnitDescriptions()
		{
			m_unitDescriptions = new Dictionary<int, UnitDescription>
			{
				{
					0,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Invalid,
						NameId = 2059,
						NameShortId = 2235
					}
				},
				{
					1,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Meters,
						NameId = 1903,
						NameShortId = 2236
					}
				},
				{
					2,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Seconds,
						NameId = 1901,
						NameShortId = 2238
					}
				},
				{
					3,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Degree,
						NameId = 4601,
						NameShortId = 4602
					}
				},
				{
					4,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Health,
						NameId = 2059,
						NameShortId = 2235
					}
				},
				{
					5,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Turns,
						NameId = 1901,
						NameShortId = 2238
					}
				},
				{
					6,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Chance,
						NameId = 1902,
						NameShortId = 2237
					}
				},
				{
					7,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Factor,
						NameId = 1902,
						NameShortId = 2237
					}
				},
				{
					8,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.Factor,
						NameId = 1902,
						NameShortId = 2237
					}
				},
				{
					9,
					new UnitDescription
					{
						Unit = StatTypes.EStatUnit.MetersPerSecond,
						NameId = 2059,
						NameShortId = 2235
					}
				}
			};
		}

		private void CreateRuleDescriptions()
		{
			m_ruleDescriptions = new Dictionary<int, RuleDescription>
			{
				{ 81, null },
				{ 82, null },
				{ 83, null },
				{ 84, null },
				{
					60,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ArmourRatingBonus,
						NameId = 1879
					}
				},
				{
					62,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ArmourRatingFactor,
						NameId = 6774
					}
				},
				{
					4,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_AttractionRange,
						NameId = 1896
					}
				},
				{
					25,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_BleedAmount,
						NameId = 1871
					}
				},
				{
					26,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_BleedAmountRelative,
						NameId = 1871
					}
				},
				{
					23,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_BleedDuration,
						NameId = 1872
					}
				},
				{
					5,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_Damage,
						NameId = 1863
					}
				},
				{
					6,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_SkillDamageFactor,
						NameId = 6489
					}
				},
				{
					22,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_EffectRadius,
						NameId = 1870
					}
				},
				{
					48,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_EvasionBonus,
						NameId = 1898
					}
				},
				{
					49,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_EvasionFactor,
						NameId = 6777
					}
				},
				{
					86,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FearsomeFactor,
						NameId = 1897
					}
				},
				{
					87,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FearsomeFactorBonus,
						NameId = 1897
					}
				},
				{
					66,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FrightenedAttackDelayFactor,
						NameId = 1874
					}
				},
				{
					64,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FrightenedDuration,
						NameId = 1876
					}
				},
				{
					65,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FrightenedEnemyCount,
						NameId = 1880
					}
				},
				{
					29,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_GreedyAttractionRadius,
						NameId = 1894
					}
				},
				{
					28,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_GreedyDuration,
						NameId = 1893
					}
				},
				{
					42,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_IncognitoBonus,
						NameId = 1887
					}
				},
				{
					44,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_IncognitoFactor,
						NameId = 6776
					}
				},
				{
					63,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_KillTokenChance,
						NameId = 1881
					}
				},
				{
					16,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_KnockdownChance,
						NameId = 1868
					}
				},
				{
					15,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_KnockdownDuration,
						NameId = 1869
					}
				},
				{
					74,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_NotorietyIncreaseDelay,
						NameId = 1888
					}
				},
				{
					39,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PiercingStrikeCount,
						NameId = 1886
					}
				},
				{
					41,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PiercingSurpriseArmourFactor,
						NameId = 1890
					}
				},
				{
					45,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PrecisionBonus,
						NameId = 1878
					}
				},
				{
					46,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PrecisionFactor,
						NameId = 6773
					}
				},
				{
					47,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PrecisionFactorBonus,
						NameId = 6775
					}
				},
				{
					290,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Projectile_Delay,
						NameId = 1895
					}
				},
				{
					69,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_RetrieveHealthAmount,
						NameId = 1892
					}
				},
				{
					70,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_RetrieveHealthRelative,
						NameId = 1892
					}
				},
				{
					71,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_MaxHealthBonus,
						NameId = 6506
					}
				},
				{
					68,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_RetrieveHealthChance,
						NameId = 1891
					}
				},
				{
					13,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ShreddedDuration,
						NameId = 1867
					}
				},
				{
					14,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ShreddedFactor,
						NameId = 1866
					}
				},
				{
					11,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_SilencedDuration,
						NameId = 1882
					}
				},
				{
					40,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PiercingStrikeDamage,
						NameId = 2124
					}
				},
				{
					61,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ArmourRatingBonusDuration,
						NameId = 2125
					}
				},
				{
					73,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_BlindedDuration,
						NameId = 2123
					}
				},
				{
					1,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_SkillDuration,
						NameId = 2122
					}
				},
				{
					2,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_SkillCooldownDuration,
						NameId = 1865
					}
				},
				{
					3,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_SkillCooldownRelativeDurationReduction,
						NameId = 1899
					}
				},
				{
					50,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_StrengthBonus,
						NameId = 1877
					}
				},
				{
					51,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_StrengthFactor,
						NameId = 6772
					}
				},
				{
					12,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_StunDuration,
						NameId = 1864
					}
				},
				{
					32,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_SweetMusicHidingSpots,
						NameId = 1900
					}
				},
				{
					9,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ThrowAngle,
						NameId = 1885
					}
				},
				{
					8,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ThrowRange,
						NameId = 1873
					}
				},
				{
					10,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_ThrowTargets,
						NameId = 1884
					}
				},
				{
					89,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_WeaponDamageBonus,
						NameId = 1883
					}
				},
				{
					43,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_IncognitoBonusDuration,
						NameId = 2146
					}
				},
				{
					33,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_GlitchHidingSpots,
						NameId = 1900
					}
				},
				{
					75,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_NotorietyDecrease,
						NameId = 4305
					}
				},
				{
					99,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FallingDamageReductionFactor,
						NameId = 5620
					}
				},
				{
					97,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PickpocketingChance,
						NameId = 5621
					}
				},
				{
					58,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_CriticalMultiplierFactor,
						NameId = 8424
					}
				},
				{
					38,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FastFingersStrikeCount,
						NameId = 5622
					}
				},
				{
					53,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_CriticalChanceBonus,
						NameId = 5623
					}
				},
				{
					98,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_PickpocketingFactor,
						NameId = 5624
					}
				},
				{
					27,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_DurationFactor,
						NameId = 5695
					}
				},
				{
					96,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_FeintBonus,
						NameId = 6503
					}
				},
				{
					57,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_CriticalMultiplierBonus,
						NameId = 6504
					}
				},
				{
					55,
					new RuleDescription
					{
						Rule = StatTypes.EStatType.Rule_StealthModifierBonus,
						NameId = 6505
					}
				}
			};
		}

		private void CreateThemeLocaDescriptions()
		{
			m_themeLocaDescriptions = new Dictionary<int, ThemeLocaDescription> { 
			{
				1,
				new ThemeLocaDescription
				{
					ThemeLocaReplacement = new Dictionary<int, int>
					{
						{ 9558, 9792 },
						{ 7854, 9801 },
						{ 9559, 9802 },
						{ 9696, 9803 },
						{ 7811, 9985 },
						{ 7812, 9986 },
						{ 7813, 9987 },
						{ 7814, 9988 },
						{ 7815, 9989 },
						{ 7816, 9991 },
						{ 7817, 9789 },
						{ 8062, 9789 },
						{ 7820, 9995 },
						{ 7821, 9996 },
						{ 7822, 9997 },
						{ 7823, 9998 },
						{ 7824, 9999 },
						{ 7825, 10000 },
						{ 7826, 10001 },
						{ 7827, 10002 },
						{ 7828, 10003 },
						{ 7829, 10004 },
						{ 7830, 10005 },
						{ 7831, 10006 },
						{ 7832, 10007 },
						{ 7834, 10009 },
						{ 7835, 10010 },
						{ 7838, 10013 },
						{ 7839, 10014 },
						{ 7808, 10015 },
						{ 7809, 10016 },
						{ 7928, 10018 },
						{ 7941, 10024 },
						{ 7950, 10029 },
						{ 7953, 10032 },
						{ 7954, 10033 },
						{ 7970, 10040 },
						{ 7985, 10044 },
						{ 8068, 10065 },
						{ 8071, 10068 },
						{ 8073, 10069 },
						{ 8082, 10078 },
						{ 8093, 10086 },
						{ 8120, 10111 },
						{ 8126, 10117 },
						{ 8128, 10119 },
						{ 8143, 10122 },
						{ 8144, 10123 },
						{ 8145, 10124 },
						{ 8146, 10125 },
						{ 8147, 10126 },
						{ 8148, 10127 },
						{ 8149, 10128 },
						{ 8151, 10130 },
						{ 8152, 10131 },
						{ 8153, 10132 },
						{ 8154, 10133 },
						{ 8155, 10134 },
						{ 8166, 10135 },
						{ 9769, 10136 },
						{ 9775, 10137 },
						{ 9778, 10140 },
						{ 9779, 10141 },
						{ 9776, 9787 },
						{ 7819, 9786 },
						{ 9777, 9788 },
						{ 9780, 9790 },
						{ 9781, 9791 }
					}
				}
			} };
		}

		private void CreateThemeIconsDescriptions()
		{
			m_themeIconsDescriptions = new Dictionary<string, ThemeIconsDescription> { 
			{
				"aUI_icon_statPower",
				new ThemeIconsDescription
				{
					PirateIconSpriteName = "aUI_icon_mission_stalk"
				}
			} };
		}

		private void CreateMoodDescriptions()
		{
			m_moodDescriptions = new Dictionary<int, MoodDescription>
			{
				{
					0,
					new MoodDescription
					{
						NameId = 2041
					}
				},
				{
					1,
					new MoodDescription
					{
						NameId = 2042
					}
				},
				{
					4,
					new MoodDescription
					{
						NameId = 2045
					}
				},
				{
					5,
					new MoodDescription
					{
						NameId = 2046
					}
				},
				{
					6,
					new MoodDescription
					{
						NameId = 2047
					}
				},
				{
					3,
					new MoodDescription
					{
						NameId = 2043
					}
				},
				{
					2,
					new MoodDescription
					{
						NameId = 2044
					}
				}
			};
		}

		private void CreateEraDescriptions()
		{
			m_eraDescriptions = new Dictionary<int, EraDescription>
			{
				{
					1,
					new EraDescription
					{
						NameId = 673
					}
				},
				{
					2,
					new EraDescription
					{
						NameId = 1777
					}
				}
			};
		}

		private void CreateObjectiveDescriptions()
		{
			m_objectiveDescriptions = new Dictionary<int, ObjectiveDescription>
			{
				{
					8,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_unknownTarget",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					1,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Kill",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					0,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Deliver",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					3,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Steal",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					9,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Follow",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					10,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Free",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					11,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Reach",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					2,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Find",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					4,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Plant",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					5,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Escape",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					6,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Kill",
						IconBackgroundPath = "aUI_fra_missionType"
					}
				},
				{
					7,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_Story",
						IconBackgroundPath = "aUI_fra_missionType_gold"
					}
				},
				{
					12,
					new ObjectiveDescription
					{
						IconPath = "aUI_icon_questionmark",
						IconBackgroundPath = "aUI_fra_missionType_gold"
					}
				}
			};
		}

		private void CreateMissionKindDescriptions()
		{
			m_missionKindDescriptions = new Dictionary<int, MissionKindDescription>
			{
				{
					0,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_mission_escort",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_escort"
					}
				},
				{
					1,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_unknownTarget",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_track_down"
					}
				},
				{
					2,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_mission_bounty_hunt",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_bounty_hunt"
					}
				},
				{
					3,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_Free",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_liberation"
					}
				},
				{
					4,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_mission_kill_contract",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_kill_contract"
					}
				},
				{
					5,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_Deliver",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_courier"
					}
				},
				{
					6,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_Reach",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_parkour"
					}
				},
				{
					7,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_mission_recovery",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_recovery"
					}
				},
				{
					8,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_Steal",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_pillage"
					}
				},
				{
					9,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_Kill",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_assassination"
					}
				},
				{
					10,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_mission_stalk",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_stalk"
					}
				},
				{
					11,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_Follow",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_tail"
					}
				},
				{
					12,
					new MissionKindDescription
					{
						IconPath = "aUI_icon_crow_hunt",
						BackgrounPath = "UI/Texture/MissionObjective/aUI_img_mission_objective_crow_hunt"
					}
				}
			};
		}

		private void CreateItemQualityDescriptions()
		{
			m_itemQualityDescriptions = new Dictionary<int, ItemQualityDescription>
			{
				{
					1,
					new ItemQualityDescription
					{
						Quality = Quality.EQuality.Mundane,
						ColorGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mundaneColor,
						ColorTextGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_mundaneTextColor,
						NameId = 1712
					}
				},
				{
					2,
					new ItemQualityDescription
					{
						Quality = Quality.EQuality.Common,
						ColorGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_commonColor,
						ColorTextGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_commonTextColor,
						NameId = 1711
					}
				},
				{
					3,
					new ItemQualityDescription
					{
						Quality = Quality.EQuality.Uncommon,
						ColorGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_uncommonColor,
						ColorTextGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_uncommonTextColor,
						NameId = 1709
					}
				},
				{
					4,
					new ItemQualityDescription
					{
						Quality = Quality.EQuality.Rare,
						ColorGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_rareColor,
						ColorTextGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_rareTextColor,
						NameId = 1710
					}
				},
				{
					5,
					new ItemQualityDescription
					{
						Quality = Quality.EQuality.Perfect,
						ColorGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_perfectColor,
						ColorTextGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_perfectTextColor,
						NameId = 1567
					}
				}
			};
		}

		private void CreateResourceDescriptions()
		{
			m_resourceDescriptions = new Dictionary<int, ResourceDescription>
			{
				{
					1,
					new ResourceDescription
					{
						ItemGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_hardCurrency,
						InfoDescriptionId = 5071,
						ItemDescriptionId = 5071,
						InfoHeadingId = 2329
					}
				},
				{
					2,
					new ResourceDescription
					{
						ItemGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_softCurrency,
						InfoDescriptionId = 5072,
						ItemDescriptionId = 5072,
						InfoHeadingId = 2330
					}
				},
				{
					3,
					new ResourceDescription
					{
						ItemGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_heroicTokenCurrency,
						InfoDescriptionId = 5073,
						ItemDescriptionId = 5073,
						InfoHeadingId = 2331
					}
				},
				{
					18,
					new ResourceDescription
					{
						ItemGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_assassinSlots,
						InfoDescriptionId = 4552,
						ItemDescriptionId = 4552,
						InfoHeadingId = 4552
					}
				}
			};
		}

		private void CreateCustomItemDescriptions()
		{
			m_customItemDescriptions = new Dictionary<int, CustomItemDescription> { 
			{
				1,
				new CustomItemDescription
				{
					ItemGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.EventScoreCurrency,
					PirateThemeIconGUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.PirateThemeEventScoreCurrency,
					InfoDescriptionId = 9559,
					InfoHeadingId = 9558,
					SmallUnitIcon = "aUI_icon_eventstar"
				}
			} };
		}

		private void CreateErrorDescriptions()
		{
			m_errorDescription = new Dictionary<int, ErrorDescription>
			{
				{
					4000,
					new ErrorDescription
					{
						DescriptionId = 3832
					}
				},
				{
					4001,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4002,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4004,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4005,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4006,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4007,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4008,
					new ErrorDescription
					{
						DescriptionId = 4953
					}
				},
				{
					4009,
					new ErrorDescription
					{
						DescriptionId = 5400
					}
				},
				{
					4010,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4011,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4012,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4013,
					new ErrorDescription
					{
						DescriptionId = 3839
					}
				},
				{
					4014,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4015,
					new ErrorDescription
					{
						DescriptionId = 3841
					}
				},
				{
					4016,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4017,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4018,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4019,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4020,
					new ErrorDescription
					{
						DescriptionId = 3838
					}
				},
				{
					4021,
					new ErrorDescription
					{
						DescriptionId = 3831
					}
				},
				{
					4022,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4023,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4024,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4025,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4026,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4027,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4028,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4029,
					new ErrorDescription
					{
						DescriptionId = 6665
					}
				},
				{
					4030,
					new ErrorDescription
					{
						DescriptionId = 3840
					}
				},
				{
					4031,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4032,
					new ErrorDescription
					{
						DescriptionId = 3833
					}
				},
				{
					4033,
					new ErrorDescription
					{
						DescriptionId = 3899
					}
				},
				{
					4034,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4035,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4064,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4036,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4037,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4038,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4039,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4040,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4041,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4042,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4043,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4044,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4045,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4049,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4301,
					new ErrorDescription
					{
						DescriptionId = 7259
					}
				},
				{
					4050,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4051,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4052,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4053,
					new ErrorDescription
					{
						DescriptionId = 5065
					}
				},
				{
					4054,
					new ErrorDescription
					{
						DescriptionId = 5065
					}
				},
				{
					4055,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4056,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4057,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4060,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4061,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4062,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4100,
					new ErrorDescription
					{
						DescriptionId = 5717
					}
				},
				{
					4101,
					new ErrorDescription
					{
						DescriptionId = 5718
					}
				},
				{
					4102,
					new ErrorDescription
					{
						DescriptionId = 5719
					}
				},
				{
					4103,
					new ErrorDescription
					{
						DescriptionId = 5720
					}
				},
				{
					4104,
					new ErrorDescription
					{
						DescriptionId = 5721
					}
				},
				{
					4105,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4106,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4107,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4108,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4120,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4121,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4122,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4123,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4140,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4141,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4142,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4143,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4144,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4145,
					new ErrorDescription
					{
						DescriptionId = 5273
					}
				},
				{
					4180,
					new ErrorDescription
					{
						DescriptionId = 5752
					}
				},
				{
					4181,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4182,
					new ErrorDescription
					{
						DescriptionId = 6778
					}
				},
				{
					4220,
					new ErrorDescription
					{
						DescriptionId = 6708
					}
				},
				{
					4500,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4501,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4502,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4503,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4504,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4505,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4506,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4507,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4508,
					new ErrorDescription
					{
						DescriptionId = 6986
					}
				},
				{
					4509,
					new ErrorDescription
					{
						DescriptionId = 6714
					}
				},
				{
					4900,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4901,
					new ErrorDescription
					{
						DescriptionId = 4604
					}
				},
				{
					4902,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5000,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5001,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5002,
					new ErrorDescription
					{
						DescriptionId = 5639
					}
				},
				{
					5003,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5004,
					new ErrorDescription
					{
						DescriptionId = 5070
					}
				},
				{
					5008,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5009,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5010,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5011,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5012,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5013,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5014,
					new ErrorDescription
					{
						DescriptionId = 5667
					}
				},
				{
					5015,
					new ErrorDescription
					{
						DescriptionId = 4327
					}
				},
				{
					5016,
					new ErrorDescription
					{
						DescriptionId = 5668
					}
				},
				{
					5017,
					new ErrorDescription
					{
						DescriptionId = 5669
					}
				},
				{
					5018,
					new ErrorDescription
					{
						DescriptionId = 5669
					}
				},
				{
					5019,
					new ErrorDescription
					{
						DescriptionId = 5670
					}
				},
				{
					5020,
					new ErrorDescription
					{
						DescriptionId = 5671
					}
				},
				{
					5021,
					new ErrorDescription
					{
						DescriptionId = 5672
					}
				},
				{
					5022,
					new ErrorDescription
					{
						DescriptionId = 5673
					}
				},
				{
					5023,
					new ErrorDescription
					{
						DescriptionId = 5674
					}
				},
				{
					5024,
					new ErrorDescription
					{
						DescriptionId = 5675
					}
				},
				{
					5025,
					new ErrorDescription
					{
						DescriptionId = 5676
					}
				},
				{
					5026,
					new ErrorDescription
					{
						DescriptionId = 7239
					}
				},
				{
					0,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					2,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					3,
					new ErrorDescription
					{
						DescriptionId = 9770
					}
				},
				{
					1000,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1001,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1002,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1003,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1004,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1005,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1006,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1007,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1008,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1009,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1010,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1011,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1012,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1013,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1014,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1015,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1016,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1017,
					new ErrorDescription
					{
						DescriptionId = 4402
					}
				},
				{
					1018,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1019,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1020,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					3000,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6001,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6002,
					new ErrorDescription
					{
						DescriptionId = 5464
					}
				},
				{
					6003,
					new ErrorDescription
					{
						DescriptionId = 5462
					}
				},
				{
					6004,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6005,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6006,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6007,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6008,
					new ErrorDescription
					{
						DescriptionId = 3954
					}
				},
				{
					6009,
					new ErrorDescription
					{
						DescriptionId = 3953
					}
				},
				{
					6010,
					new ErrorDescription
					{
						DescriptionId = 5390
					}
				},
				{
					6011,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6013,
					new ErrorDescription
					{
						DescriptionId = 7859
					}
				},
				{
					6012,
					new ErrorDescription
					{
						DescriptionId = 5461
					}
				},
				{
					7001,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					7002,
					new ErrorDescription
					{
						DescriptionId = 4320
					}
				},
				{
					7003,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					7004,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					8001,
					new ErrorDescription
					{
						DescriptionId = 4318
					}
				},
				{
					8002,
					new ErrorDescription
					{
						DescriptionId = 3818
					}
				},
				{
					8003,
					new ErrorDescription
					{
						DescriptionId = 5276
					}
				},
				{
					8004,
					new ErrorDescription
					{
						DescriptionId = 4326
					}
				},
				{
					8005,
					new ErrorDescription
					{
						DescriptionId = 6704
					}
				},
				{
					9005,
					new ErrorDescription
					{
						DescriptionId = 7294
					}
				},
				{
					9004,
					new ErrorDescription
					{
						DescriptionId = 7293
					}
				},
				{
					9003,
					new ErrorDescription
					{
						DescriptionId = 7292
					}
				},
				{
					9002,
					new ErrorDescription
					{
						DescriptionId = 7291
					}
				},
				{
					9001,
					new ErrorDescription
					{
						DescriptionId = 7290
					}
				},
				{
					9101,
					new ErrorDescription
					{
						DescriptionId = 8052
					}
				},
				{
					9103,
					new ErrorDescription
					{
						DescriptionId = 9774
					}
				},
				{
					9105,
					new ErrorDescription
					{
						DescriptionId = 9774
					}
				},
				{
					9104,
					new ErrorDescription
					{
						DescriptionId = 9774
					}
				},
				{
					9102,
					new ErrorDescription
					{
						DescriptionId = 9774
					}
				},
				{
					4046,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4047,
					new ErrorDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4048,
					new ErrorDescription
					{
						DescriptionId = 6921
					}
				},
				{
					4063,
					new ErrorDescription
					{
						DescriptionId = 6108
					}
				},
				{
					4201,
					new ErrorDescription
					{
						DescriptionId = 6710
					}
				},
				{
					4200,
					new ErrorDescription
					{
						DescriptionId = 6709
					}
				},
				{
					4202,
					new ErrorDescription
					{
						DescriptionId = 6711
					}
				},
				{
					4950,
					new ErrorDescription
					{
						DescriptionId = 7060
					}
				},
				{
					4240,
					new ErrorDescription
					{
						DescriptionId = 6780
					}
				},
				{
					4241,
					new ErrorDescription
					{
						DescriptionId = 6781
					}
				},
				{
					4242,
					new ErrorDescription
					{
						DescriptionId = 6782
					}
				},
				{
					4243,
					new ErrorDescription
					{
						DescriptionId = 6783
					}
				},
				{
					4244,
					new ErrorDescription
					{
						DescriptionId = 6784
					}
				},
				{
					4245,
					new ErrorDescription
					{
						DescriptionId = 6785
					}
				},
				{
					4331,
					new ErrorDescription
					{
						DescriptionId = 7124
					}
				},
				{
					4332,
					new ErrorDescription
					{
						DescriptionId = 7125
					}
				},
				{
					4333,
					new ErrorDescription
					{
						DescriptionId = 7126
					}
				}
			};
		}

		private void CreateErrorGroupDescriptions()
		{
			m_errorGroupDescriptions = new Dictionary<int, ErrorGroupDescription>
			{
				{
					4000,
					new ErrorGroupDescription
					{
						DescriptionId = 3832
					}
				},
				{
					4001,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4002,
					new ErrorGroupDescription
					{
						DescriptionId = 3842
					}
				},
				{
					4003,
					new ErrorGroupDescription
					{
						DescriptionId = 3903
					}
				},
				{
					4004,
					new ErrorGroupDescription
					{
						DescriptionId = 3902
					}
				},
				{
					4005,
					new ErrorGroupDescription
					{
						DescriptionId = 3835
					}
				},
				{
					4006,
					new ErrorGroupDescription
					{
						DescriptionId = 3900
					}
				},
				{
					4007,
					new ErrorGroupDescription
					{
						DescriptionId = 3901
					}
				},
				{
					4008,
					new ErrorGroupDescription
					{
						DescriptionId = 3836
					}
				},
				{
					4009,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4010,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4011,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4013,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4014,
					new ErrorGroupDescription
					{
						DescriptionId = 6779
					}
				},
				{
					4015,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4018,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4019,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					0,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					1,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					2,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					3,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					30,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					40,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					5000,
					new ErrorGroupDescription
					{
						DescriptionId = 3837
					}
				},
				{
					5001,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					6000,
					new ErrorGroupDescription
					{
						DescriptionId = 3946
					}
				},
				{
					7000,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					8000,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					9000,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					9100,
					new ErrorGroupDescription
					{
						DescriptionId = 8053
					}
				},
				{
					4016,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				},
				{
					4017,
					new ErrorGroupDescription
					{
						DescriptionId = 3819
					}
				}
			};
		}

		private void CreateSupportDescription()
		{
			m_supportDescriptions = new Dictionary<int, SupportDescription>
			{
				{
					0,
					new SupportDescription
					{
						HeadingId = 4639,
						DescriptionId = 4644,
						ButtonTextId = 4643
					}
				},
				{
					1,
					new SupportDescription
					{
						HeadingId = 4641,
						DescriptionId = 4645,
						ButtonTextId = 4643
					}
				},
				{
					3,
					new SupportDescription
					{
						HeadingId = 4640,
						DescriptionId = 4646,
						ButtonTextId = 4643
					}
				},
				{
					2,
					new SupportDescription
					{
						HeadingId = 4642,
						DescriptionId = 4647,
						ButtonTextId = 4643
					}
				},
				{
					4,
					new SupportDescription
					{
						HeadingId = 4651,
						DescriptionId = 4650,
						ButtonTextId = 4643
					}
				},
				{
					5,
					new SupportDescription
					{
						HeadingId = 4649,
						DescriptionId = 4648,
						ButtonTextId = 4643
					}
				},
				{
					6,
					new SupportDescription
					{
						HeadingId = 9716,
						DescriptionId = 9717,
						ButtonTextId = 9715,
						PopupHeadingId = 9716,
						PopupDescriptionId = 9719
					}
				}
			};
		}

		private void CreatePushNotificationSwitchDescription()
		{
			m_pushNotificationSwitchDescriptions = new Dictionary<int, PushNotificationSwitchDescription>
			{
				{
					1,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7049
					}
				},
				{
					2,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7050
					}
				},
				{
					3,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7051
					}
				},
				{
					4,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7053
					}
				},
				{
					5,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7059
					}
				},
				{
					6,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7055
					}
				},
				{
					7,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7057
					}
				},
				{
					8,
					new PushNotificationSwitchDescription
					{
						HeadingId = 7058
					}
				}
			};
		}

		private void CreateFriendRelationSprites()
		{
			m_friendRelationSprites = new BBDictionary<int, UIPath>
			{
				{ 0, "aUI_icon_friend" },
				{ 1, "aUI_icon_friendPending" }
			};
		}

		private void CreateNewsFeedDescription()
		{
			m_newsFeedDescription = new Dictionary<int, NewsFeedDescription>
			{
				{
					0,
					new NewsFeedDescription
					{
						HashTagId = -1,
						SpriteName = "aUI_img_news_general"
					}
				},
				{
					2,
					new NewsFeedDescription
					{
						HashTagId = 4656,
						SpriteName = "aUI_img_news_maintenance"
					}
				},
				{
					1,
					new NewsFeedDescription
					{
						HashTagId = 4655,
						SpriteName = "aUI_img_news_news"
					}
				},
				{
					3,
					new NewsFeedDescription
					{
						HashTagId = 4657,
						SpriteName = "aUI_img_news_shop"
					}
				},
				{
					4,
					new NewsFeedDescription
					{
						HashTagId = 5685,
						SpriteName = "aUI_img_news_challenge"
					}
				},
				{
					5,
					new NewsFeedDescription
					{
						HashTagId = 5688,
						SpriteName = "aUI_img_news_faq"
					}
				},
				{
					6,
					new NewsFeedDescription
					{
						HashTagId = 5684,
						SpriteName = "aUI_img_news_poll"
					}
				},
				{
					7,
					new NewsFeedDescription
					{
						HashTagId = 5686,
						SpriteName = "aUI_img_news_quote"
					}
				},
				{
					8,
					new NewsFeedDescription
					{
						HashTagId = 5687,
						SpriteName = "aUI_img_news_tip"
					}
				}
			};
		}

		private static void CreateButtonConfigs()
		{
			Dictionary<int, ButtonConfiguration> dictionary = new Dictionary<int, ButtonConfiguration>();
			dictionary.Add(0, new ButtonConfiguration
			{
				DefaultColor = new Color(0.3529412f, 2f / 3f, 47f / 51f, 1f),
				HoverColor = new Color(0.3529412f, 2f / 3f, 47f / 51f, 1f),
				PressedColor = new Color(8f / 51f, 0.3529412f, 26f / 51f, 1f),
				DisabledColor = new Color(0.5882353f, 0.5882353f, 0.5882353f, 1f),
				GlowColor = new Color(2f / 3f, 43f / 51f, 1f, 0.5882353f),
				TransitionDuration = 0.05f
			});
			dictionary.Add(1, new ButtonConfiguration
			{
				DefaultColor = new Color(0.4117647f, 49f / 51f, 0.3019608f, 1f),
				HoverColor = new Color(0.4117647f, 49f / 51f, 0.3019608f, 1f),
				PressedColor = new Color(11f / 51f, 26f / 51f, 8f / 51f, 1f),
				DisabledColor = new Color(0.5882353f, 0.5882353f, 0.5882353f, 1f),
				GlowColor = new Color(0.88235295f, 0.88235295f, 0.7647059f, 0.5882353f),
				TransitionDuration = 0.05f
			});
			dictionary.Add(2, new ButtonConfiguration
			{
				DefaultColor = new Color(1f, 1f, 1f, 1f),
				HoverColor = new Color(1f, 1f, 1f, 1f),
				PressedColor = new Color(1f, 1f, 1f, 1f),
				DisabledColor = new Color(1f, 1f, 1f, 1f),
				GlowColor = new Color(1f, 1f, 1f, 1f),
				TransitionDuration = 0.05f
			});
			dictionary.Add(3, new ButtonConfiguration
			{
				DefaultColor = new Color(0.14509805f, 0.3019608f, 0.41568628f, 1f),
				HoverColor = new Color(0.14509805f, 0.3019608f, 0.41568628f, 1f),
				PressedColor = new Color(0.14509805f, 0.3019608f, 0.41568628f, 1f),
				DisabledColor = new Color(0.5882353f, 0.5882353f, 0.5882353f, 1f),
				GlowColor = new Color(2f / 3f, 43f / 51f, 1f, 0.5882353f),
				TransitionDuration = 0.05f
			});
			dictionary.Add(4, new ButtonConfiguration
			{
				DefaultColor = new Color(0.3529412f, 2f / 3f, 47f / 51f, 1f),
				HoverColor = new Color(0.3529412f, 2f / 3f, 47f / 51f, 1f),
				PressedColor = new Color(8f / 51f, 0.3529412f, 26f / 51f, 1f),
				DisabledColor = new Color(1f, 1f, 1f, 1f),
				GlowColor = new Color(2f / 3f, 43f / 51f, 1f, 0.5882353f),
				TransitionDuration = 0.05f
			});
			m_buttonConfigs = dictionary;
		}

		private void CreateAchievementsConfig()
		{
			m_achievementConfigs = new Dictionary<int, AchievementConfig>
			{
				{
					0,
					new AchievementConfig
					{
						SingleRankSpriteName = "aUI_icon_achievement_lvl3",
						MultiRankSpriteName = "aUI_icon_achievement_lvl0",
						EffectHighlightColor = new Color(0.95f, 0.71f, 0.36f, 1f)
					}
				},
				{
					1,
					new AchievementConfig
					{
						SingleRankSpriteName = "aUI_icon_achievement_lvl2",
						MultiRankSpriteName = "aUI_icon_achievement_lvl2",
						EffectHighlightColor = new Color(0f, 0.64f, 0.7f, 1f)
					}
				},
				{
					2,
					new AchievementConfig
					{
						SingleRankSpriteName = "aUI_icon_achievement_lvl1",
						MultiRankSpriteName = "aUI_icon_achievement_lvl1",
						EffectHighlightColor = new Color(0.46f, 0.62f, 1f, 1f)
					}
				}
			};
		}

		private void CreateForgeConfig()
		{
			m_forgeConfigs = new Dictionary<int, ForgeConfig>
			{
				{
					1,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintWeapon
					}
				},
				{
					2,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintBoots
					}
				},
				{
					3,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintLegs
					}
				},
				{
					4,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintChest
					}
				},
				{
					5,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintArms
					}
				},
				{
					6,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintShoulder
					}
				},
				{
					7,
					new ForgeConfig
					{
						GUID = UbiCore.Utils.Singleton<GlobalGameIDs>.Instance.m_blueprintInsignia
					}
				}
			};
		}

		private void CreateAbbreviateNumbersMapping()
		{
			int[] divider = new int[6] { 1000, 1000, 1000000, 1000000, 1000000, 1000000000 };
			int[] locaKey = new int[6] { 10527, 10530, 10525, 10526, 10528, 10529 };
			m_abbreviatedNumberMapping = new Dictionary<ELanguages, AbbreviatedNumberMapping> { 
			{
				ELanguages.en_GB,
				new AbbreviatedNumberMapping
				{
					Divider = divider,
					LocaKey = locaKey
				}
			} };
		}

		private void CreatePlayerLevelSprites()
		{
			int num = PlayerLevelLevels.Length;
			m_playerLevelSprites = new BBDictionary<int, UIPath>();
			for (int i = 0; i < num - 1; i++)
			{
				int num2 = PlayerLevelLevels[i];
				int num3 = PlayerLevelLevels[i + 1];
				for (int j = num2; j < num3; j++)
				{
					m_playerLevelSprites[j] = PlayerLevelSprites[i];
				}
			}
			m_playerLevelSprites[PlayerLevelLevels[num - 1]] = PlayerLevelSprites[num - 1];
		}

		[Conditional("UNITY_RELEASE")]
		[Conditional("UNITY_DEBUG")]
		private static void ValidateErrorCodes()
		{
			Dictionary<int, ErrorDescription> errorDescriptions = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ErrorDescriptions;
			foreach (int value in Enum.GetValues(typeof(ErrorCode)))
			{
			}
		}

		[Conditional("UNITY_DEBUG")]
		[Conditional("UNITY_RELEASE")]
		private static void ValidateErrorGroups()
		{
			Dictionary<int, ErrorGroupDescription> errorGroupDescriptions = UbiCore.Utils.Singleton<UIDataMapper>.Instance.ErrorGroupDescriptions;
			foreach (int value in Enum.GetValues(typeof(ErrorCategory)))
			{
			}
		}
	}
}
