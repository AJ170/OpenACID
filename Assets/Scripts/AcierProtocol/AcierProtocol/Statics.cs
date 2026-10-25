using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Statics")]
	public class Statics : IExtensible
	{
		private long _staticsVersion;

		private XpPerLevelTable _xpPerLevelTable;

		private AvatarClassDefinitions _avatarClassDefinitions;

		private LocationUnlockMappings _locationUnlockMappings;

		private ItemSellMapping _itemSellMapping;

		private TimeToHardCurrencyMapping _timeToHardCurrencyMapping;

		private ShopTable _shopTable;

		private InventorySlotCostsMapping _inventorySlotCostsMapping;

		private AssassinSlotCostsMapping _assassinSlotCostsMapping;

		private Globals _globals;

		private ShopTable _hcShopTable;

		private AchievementCategoryMapping _achievementCategoryMapping;

		private AchievementMapping _achievementMapping;

		private StoryMissionRewardMapping _storyMissionRewardMapping;

		private DailyRewardBuyBackMapping _dailyRewardBuyBackMapping;

		private ResourceCostMapping _resourceCostMapping;

		private SkillTrainingTimeMapping _skillTrainingTimeMapping;

		private EventTemplates _eventTemplates;

		private ForgeMapping _forgeMapping;

		private MysteryBoxMapping _mysteryBoxMapping;

		private CampaignCatalog _campaignCatalog;

		private HeroicRankMapping _heroicRankMapping;

		private MissionLootScalingMapping _missionLootScalingMapping;

		private MissionTypeLootMapping _missionTypeLootMapping;

		private TutorialCategoryMapping _tutorialCategoryMapping;

		private MailBoxMapping _mailBoxMapping;

		private LevelSkillPointMapping _levelSkillPointMapping;

		private SecondSkillTreeCostMapping _secondSkillTreeCostMapping;

		private CounterTypeLocaKeyMapping _counterTypeLocaKeyMapping;

		private ResourcePropertiesMapping _resourcePropertiesMapping;

		private EventsUserFriendlyMapping _eventsUserFriendlyMapping;

		private IExtension extensionObject;

		[ProtoMember(99, IsRequired = false, Name = "staticsVersion", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long staticsVersion
		{
			get
			{
				return _staticsVersion;
			}
			set
			{
				_staticsVersion = value;
			}
		}

		[ProtoMember(1, IsRequired = false, Name = "xpPerLevelTable", DataFormat = DataFormat.Default)]
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

		[ProtoMember(2, IsRequired = false, Name = "avatarClassDefinitions", DataFormat = DataFormat.Default)]
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

		[ProtoMember(4, IsRequired = false, Name = "locationUnlockMappings", DataFormat = DataFormat.Default)]
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

		[ProtoMember(5, IsRequired = false, Name = "itemSellMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(8, IsRequired = false, Name = "timeToHardCurrencyMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(9, IsRequired = false, Name = "shopTable", DataFormat = DataFormat.Default)]
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

		[ProtoMember(11, IsRequired = false, Name = "inventorySlotCostsMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(12, IsRequired = false, Name = "assassinSlotCostsMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(13, IsRequired = false, Name = "globals", DataFormat = DataFormat.Default)]
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

		[ProtoMember(22, IsRequired = false, Name = "hcShopTable", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ShopTable hcShopTable
		{
			get
			{
				return _hcShopTable;
			}
			set
			{
				_hcShopTable = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "achievementCategoryMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(24, IsRequired = false, Name = "achievementMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(26, IsRequired = false, Name = "storyMissionRewardMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(29, IsRequired = false, Name = "resourceCostMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(30, IsRequired = false, Name = "skillTrainingTimeMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(31, IsRequired = false, Name = "eventTemplates", DataFormat = DataFormat.Default)]
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

		[ProtoMember(33, IsRequired = false, Name = "forgeMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(34, IsRequired = false, Name = "mysteryBoxMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(35, IsRequired = false, Name = "campaignCatalog", DataFormat = DataFormat.Default)]
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

		[ProtoMember(36, IsRequired = false, Name = "heroicRankMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(37, IsRequired = false, Name = "missionLootScalingMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(41, IsRequired = false, Name = "missionTypeLootMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(42, IsRequired = false, Name = "tutorialCategoryMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(43, IsRequired = false, Name = "mailBoxMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(44, IsRequired = false, Name = "levelSkillPointMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(45, IsRequired = false, Name = "secondSkillTreeCostMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(47, IsRequired = false, Name = "counterTypeLocaKeyMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(48, IsRequired = false, Name = "resourcePropertiesMapping", DataFormat = DataFormat.Default)]
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

		[ProtoMember(50, IsRequired = false, Name = "eventsUserFriendlyMapping", DataFormat = DataFormat.Default)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
