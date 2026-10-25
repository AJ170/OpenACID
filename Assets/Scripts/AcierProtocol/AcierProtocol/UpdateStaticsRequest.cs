using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "UpdateStaticsRequest")]
	public class UpdateStaticsRequest : IExtensible
	{
		private LootMasterTable _lootMasterTable;

		private MissionConfigurations _missionConfigurations;

		private AssassinLevelToMissionBudget _assassinLevelToMissionBudget;

		private AvatarClassDefinitions _avatarClassDefinitions;

		private ItemSellMapping _itemSellMapping;

		private XpPerLevelTable _xpPerLevelTable;

		private LocationUnlockMappings _locationUnlockMappings;

		private Globals _globals;

		private LevelSliceToRewardMapping _levelSliceToRewardMapping;

		private TimeToHardCurrencyMapping _timeToHardCurrencyMapping;

		private ShopTable _shopTable;

		private StoryMissionRewardMapping _storyMissionRewardMapping;

		private InventorySlotCostsMapping _inventorySlotCostsMapping;

		private AssassinSlotCostsMapping _assassinSlotCostsMapping;

		private MissionCatalog _missionCatalog;

		private MissionList _missionList;

		private RandomNumberBlob _randomNumberBlob;

		private MissionBlackList _missionBlackList;

		private MaxFriendsMapping _maxFriendsMapping;

		private AchievementCategoryMapping _achievementCategoryMapping;

		private AchievementMapping _achievementMapping;

		private MailTemplates _mailTemplates;

		private DailyRewardMapping _dailyRewardMapping;

		private DailyRewardBuyBackMapping _dailyRewardBuyBackMapping;

		private GameDbMapping _gameDbMapping;

		private ResourceCostMapping _resourceCostMapping;

		private ChallengeRewardMultiplierMapping _challengeRewardMultiplierMapping;

		private SkillTrainingTimeMapping _skillTrainingTimeMapping;

		private RequiredSkillPointsMapping _requiredSkillPointsMapping;

		private EventTemplates _eventTemplates;

		private PushNotificationTemplates _pushNotificationTemplates;

		private ForgeMapping _forgeMapping;

		private MysteryBoxMapping _mysteryBoxMapping;

		private EntitlementMapping _entitlementMapping;

		private CampaignCatalog _campaignCatalog;

		private HeroicRankMapping _heroicRankMapping;

		private MissionUnlockMapping _missionUnlockMapping;

		private MissionLootScalingMapping _missionLootScalingMapping;

		private QuestGenerationMapping _questGenerationMapping;

		private MissionAlternatives _missionAlternatives;

		private QualitySettingsMapping _qualitySettingsMapping;

		private MissionTypeLootMapping _missionTypeLootMapping;

		private TutorialCategoryMapping _tutorialCategoryMapping;

		private OverflowMapping _overflowMapping;

		private MailBoxMapping _mailBoxMapping;

		private LevelSkillPointMapping _levelSkillPointMapping;

		private SecondSkillTreeCostMapping _secondSkillTreeCostMapping;

		private EventsUserFriendlyMapping _eventsUserFriendlyMapping;

		private CounterTypeLocaKeyMapping _counterTypeLocaKeyMapping;

		private ExclusiveContentMapping _exclusiveContentMapping;

		private ResourcePropertiesMapping _resourcePropertiesMapping;

		private AdColonyRewardMapping _adColonyRewardMapping;

		private GeneratedLeaderBoardMapping _generatedLeaderBoardMapping;

		private int _articyPerforceChangeList;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "lootMasterTable", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LootMasterTable lootMasterTable
		{
			get
			{
				return _lootMasterTable;
			}
			set
			{
				_lootMasterTable = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "missionConfigurations", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionConfigurations missionConfigurations
		{
			get
			{
				return _missionConfigurations;
			}
			set
			{
				_missionConfigurations = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "assassinLevelToMissionBudget", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinLevelToMissionBudget assassinLevelToMissionBudget
		{
			get
			{
				return _assassinLevelToMissionBudget;
			}
			set
			{
				_assassinLevelToMissionBudget = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "avatarClassDefinitions", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AvatarClassDefinitions avatarClassDefinitions
		{
			get
			{
				return _avatarClassDefinitions;
			}
			set
			{
				_avatarClassDefinitions = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "itemSellMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ItemSellMapping itemSellMapping
		{
			get
			{
				return _itemSellMapping;
			}
			set
			{
				_itemSellMapping = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "xpPerLevelTable", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public XpPerLevelTable xpPerLevelTable
		{
			get
			{
				return _xpPerLevelTable;
			}
			set
			{
				_xpPerLevelTable = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "locationUnlockMappings", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LocationUnlockMappings locationUnlockMappings
		{
			get
			{
				return _locationUnlockMappings;
			}
			set
			{
				_locationUnlockMappings = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "globals", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Globals globals
		{
			get
			{
				return _globals;
			}
			set
			{
				_globals = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "levelSliceToRewardMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LevelSliceToRewardMapping levelSliceToRewardMapping
		{
			get
			{
				return _levelSliceToRewardMapping;
			}
			set
			{
				_levelSliceToRewardMapping = value;
			}
		}

		[ProtoMember(12, IsRequired = false, Name = "timeToHardCurrencyMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TimeToHardCurrencyMapping timeToHardCurrencyMapping
		{
			get
			{
				return _timeToHardCurrencyMapping;
			}
			set
			{
				_timeToHardCurrencyMapping = value;
			}
		}

		[ProtoMember(13, IsRequired = false, Name = "shopTable", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ShopTable shopTable
		{
			get
			{
				return _shopTable;
			}
			set
			{
				_shopTable = value;
			}
		}

		[ProtoMember(14, IsRequired = false, Name = "storyMissionRewardMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public StoryMissionRewardMapping storyMissionRewardMapping
		{
			get
			{
				return _storyMissionRewardMapping;
			}
			set
			{
				_storyMissionRewardMapping = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "inventorySlotCostsMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public InventorySlotCostsMapping inventorySlotCostsMapping
		{
			get
			{
				return _inventorySlotCostsMapping;
			}
			set
			{
				_inventorySlotCostsMapping = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "assassinSlotCostsMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AssassinSlotCostsMapping assassinSlotCostsMapping
		{
			get
			{
				return _assassinSlotCostsMapping;
			}
			set
			{
				_assassinSlotCostsMapping = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "missionCatalog", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionCatalog missionCatalog
		{
			get
			{
				return _missionCatalog;
			}
			set
			{
				_missionCatalog = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "missionList", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionList missionList
		{
			get
			{
				return _missionList;
			}
			set
			{
				_missionList = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "randomNumberBlob", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public RandomNumberBlob randomNumberBlob
		{
			get
			{
				return _randomNumberBlob;
			}
			set
			{
				_randomNumberBlob = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "missionBlackList", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionBlackList missionBlackList
		{
			get
			{
				return _missionBlackList;
			}
			set
			{
				_missionBlackList = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "maxFriendsMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MaxFriendsMapping maxFriendsMapping
		{
			get
			{
				return _maxFriendsMapping;
			}
			set
			{
				_maxFriendsMapping = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "achievementCategoryMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AchievementCategoryMapping achievementCategoryMapping
		{
			get
			{
				return _achievementCategoryMapping;
			}
			set
			{
				_achievementCategoryMapping = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "achievementMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AchievementMapping achievementMapping
		{
			get
			{
				return _achievementMapping;
			}
			set
			{
				_achievementMapping = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "mailTemplates", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailTemplates mailTemplates
		{
			get
			{
				return _mailTemplates;
			}
			set
			{
				_mailTemplates = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "dailyRewardMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public DailyRewardMapping dailyRewardMapping
		{
			get
			{
				return _dailyRewardMapping;
			}
			set
			{
				_dailyRewardMapping = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "dailyRewardBuyBackMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public DailyRewardBuyBackMapping dailyRewardBuyBackMapping
		{
			get
			{
				return _dailyRewardBuyBackMapping;
			}
			set
			{
				_dailyRewardBuyBackMapping = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "gameDbMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public GameDbMapping gameDbMapping
		{
			get
			{
				return _gameDbMapping;
			}
			set
			{
				_gameDbMapping = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "resourceCostMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ResourceCostMapping resourceCostMapping
		{
			get
			{
				return _resourceCostMapping;
			}
			set
			{
				_resourceCostMapping = value;
			}
		}

		[ProtoMember(31, IsRequired = false, Name = "challengeRewardMultiplierMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ChallengeRewardMultiplierMapping challengeRewardMultiplierMapping
		{
			get
			{
				return _challengeRewardMultiplierMapping;
			}
			set
			{
				_challengeRewardMultiplierMapping = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "skillTrainingTimeMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public SkillTrainingTimeMapping skillTrainingTimeMapping
		{
			get
			{
				return _skillTrainingTimeMapping;
			}
			set
			{
				_skillTrainingTimeMapping = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "requiredSkillPointsMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public RequiredSkillPointsMapping requiredSkillPointsMapping
		{
			get
			{
				return _requiredSkillPointsMapping;
			}
			set
			{
				_requiredSkillPointsMapping = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "eventTemplates", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EventTemplates eventTemplates
		{
			get
			{
				return _eventTemplates;
			}
			set
			{
				_eventTemplates = value;
			}
		}

		[ProtoMember(36, IsRequired = false, Name = "pushNotificationTemplates", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public PushNotificationTemplates pushNotificationTemplates
		{
			get
			{
				return _pushNotificationTemplates;
			}
			set
			{
				_pushNotificationTemplates = value;
			}
		}

		[ProtoMember(38, IsRequired = false, Name = "forgeMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ForgeMapping forgeMapping
		{
			get
			{
				return _forgeMapping;
			}
			set
			{
				_forgeMapping = value;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "mysteryBoxMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MysteryBoxMapping mysteryBoxMapping
		{
			get
			{
				return _mysteryBoxMapping;
			}
			set
			{
				_mysteryBoxMapping = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "entitlementMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EntitlementMapping entitlementMapping
		{
			get
			{
				return _entitlementMapping;
			}
			set
			{
				_entitlementMapping = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "campaignCatalog", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public CampaignCatalog campaignCatalog
		{
			get
			{
				return _campaignCatalog;
			}
			set
			{
				_campaignCatalog = value;
			}
		}

		[ProtoMember(42, IsRequired = false, Name = "heroicRankMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public HeroicRankMapping heroicRankMapping
		{
			get
			{
				return _heroicRankMapping;
			}
			set
			{
				_heroicRankMapping = value;
			}
		}

		[ProtoMember(43, IsRequired = false, Name = "missionUnlockMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionUnlockMapping missionUnlockMapping
		{
			get
			{
				return _missionUnlockMapping;
			}
			set
			{
				_missionUnlockMapping = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "missionLootScalingMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionLootScalingMapping missionLootScalingMapping
		{
			get
			{
				return _missionLootScalingMapping;
			}
			set
			{
				_missionLootScalingMapping = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "questGenerationMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public QuestGenerationMapping questGenerationMapping
		{
			get
			{
				return _questGenerationMapping;
			}
			set
			{
				_questGenerationMapping = value;
			}
		}

		[ProtoMember(46, IsRequired = false, Name = "missionAlternatives", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionAlternatives missionAlternatives
		{
			get
			{
				return _missionAlternatives;
			}
			set
			{
				_missionAlternatives = value;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "qualitySettingsMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public QualitySettingsMapping qualitySettingsMapping
		{
			get
			{
				return _qualitySettingsMapping;
			}
			set
			{
				_qualitySettingsMapping = value;
			}
		}

		[ProtoMember(48, IsRequired = false, Name = "missionTypeLootMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MissionTypeLootMapping missionTypeLootMapping
		{
			get
			{
				return _missionTypeLootMapping;
			}
			set
			{
				_missionTypeLootMapping = value;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "tutorialCategoryMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TutorialCategoryMapping tutorialCategoryMapping
		{
			get
			{
				return _tutorialCategoryMapping;
			}
			set
			{
				_tutorialCategoryMapping = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "overflowMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public OverflowMapping overflowMapping
		{
			get
			{
				return _overflowMapping;
			}
			set
			{
				_overflowMapping = value;
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "mailBoxMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public MailBoxMapping mailBoxMapping
		{
			get
			{
				return _mailBoxMapping;
			}
			set
			{
				_mailBoxMapping = value;
			}
		}

		[ProtoMember(52, IsRequired = false, Name = "levelSkillPointMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LevelSkillPointMapping levelSkillPointMapping
		{
			get
			{
				return _levelSkillPointMapping;
			}
			set
			{
				_levelSkillPointMapping = value;
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "secondSkillTreeCostMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public SecondSkillTreeCostMapping secondSkillTreeCostMapping
		{
			get
			{
				return _secondSkillTreeCostMapping;
			}
			set
			{
				_secondSkillTreeCostMapping = value;
			}
		}

		[ProtoMember(54, IsRequired = false, Name = "eventsUserFriendlyMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EventsUserFriendlyMapping eventsUserFriendlyMapping
		{
			get
			{
				return _eventsUserFriendlyMapping;
			}
			set
			{
				_eventsUserFriendlyMapping = value;
			}
		}

		[ProtoMember(55, IsRequired = false, Name = "counterTypeLocaKeyMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public CounterTypeLocaKeyMapping counterTypeLocaKeyMapping
		{
			get
			{
				return _counterTypeLocaKeyMapping;
			}
			set
			{
				_counterTypeLocaKeyMapping = value;
			}
		}

		[ProtoMember(57, IsRequired = false, Name = "exclusiveContentMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ExclusiveContentMapping exclusiveContentMapping
		{
			get
			{
				return _exclusiveContentMapping;
			}
			set
			{
				_exclusiveContentMapping = value;
			}
		}

		[ProtoMember(58, IsRequired = false, Name = "resourcePropertiesMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ResourcePropertiesMapping resourcePropertiesMapping
		{
			get
			{
				return _resourcePropertiesMapping;
			}
			set
			{
				_resourcePropertiesMapping = value;
			}
		}

		[ProtoMember(59, IsRequired = false, Name = "adColonyRewardMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AdColonyRewardMapping adColonyRewardMapping
		{
			get
			{
				return _adColonyRewardMapping;
			}
			set
			{
				_adColonyRewardMapping = value;
			}
		}

		[ProtoMember(60, IsRequired = false, Name = "generatedLeaderBoardMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public GeneratedLeaderBoardMapping generatedLeaderBoardMapping
		{
			get
			{
				return _generatedLeaderBoardMapping;
			}
			set
			{
				_generatedLeaderBoardMapping = value;
			}
		}

		[ProtoMember(56, IsRequired = false, Name = "articyPerforceChangeList", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int articyPerforceChangeList
		{
			get
			{
				return _articyPerforceChangeList;
			}
			set
			{
				_articyPerforceChangeList = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
