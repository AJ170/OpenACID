using AcierProtocol;
using Assets.Scripts.UI.Utils.Extensions.Shop;
using SharedBaseLib.Core;

namespace Assets.Scripts.UI.State
{
	public class TemplateManager
	{
		private ShopTable m_inGameShopTable;

		private ShopTable m_directPurchaseShopTable;

		private ShopTable m_directPurchaseHustonShopTable;

		private AchievementCategoryMapping m_achievementCategoryMapping;

		private AchievementMapping m_achievementMapping;

		public int AdColonySortId { get; set; }

		public bool ShowAdColonyLastInShop { get; set; }

		public int BuyInventorySlotsSortId { get; set; }

		public bool ShowShopEffect { get; set; }

		public bool ShowHCInWallet { get; set; }

		public AvatarClassDefinitions AvatarDefinition { get; set; }

		public XpPerLevelTable XpPerLevelTable { get; set; }

		public ItemSellMapping ItemSellMapping { get; set; }

		public LocationUnlockMappings LocationUnlockMappings { get; set; }

		public CounterTypeLocaKeyMapping CounterTypeLocaKeyMapping { get; set; }

		public ShopTable ShopTable
		{
			get
			{
				return GetAllShopTables();
			}
			set
			{
				m_inGameShopTable = value;
			}
		}

		public ShopTable DirectPurchasesShopTable
		{
			get
			{
				return m_directPurchaseShopTable;
			}
			set
			{
				m_directPurchaseHustonShopTable = value;
				BBList<ShopBundle> bBList = new BBList<ShopBundle>();
				bBList.AddRange(value.bundles);
				for (int i = 0; i < m_inGameShopTable.bundles.Length; i++)
				{
					ShopBundle shopBundle = m_inGameShopTable.bundles[i];
					if (shopBundle.IsDirectPurchasesBundle())
					{
						bBList.Add(shopBundle);
					}
				}
				ShopTable shopTable = new ShopTable();
				shopTable.version = m_directPurchaseHustonShopTable.version;
				shopTable.bundles = m_directPurchaseHustonShopTable.bundles;
				shopTable.bundles = shopTable.bundles.AddRange(bBList);
				m_directPurchaseShopTable = shopTable;
			}
		}

		public TimeToHardCurrencyMapping TimeToHardCurrencyMapping { get; set; }

		public InventorySlotCostsMapping InventorySlotsCostsMapping { get; set; }

		public AssassinSlotCostsMapping AssassinSlotsCostsMapping { get; set; }

		public Globals Globals { get; set; }

		public AchievementCategoryMapping AchievementCategoryMapping
		{
			get
			{
				return (m_achievementCategoryMapping == null) ? new AchievementCategoryMapping() : m_achievementCategoryMapping;
			}
			set
			{
				m_achievementCategoryMapping = value;
			}
		}

		public AchievementMapping AchievementMapping
		{
			get
			{
				return (m_achievementMapping == null) ? new AchievementMapping() : m_achievementMapping;
			}
			set
			{
				m_achievementMapping = value;
			}
		}

		public Events Events { get; set; }

		public SkillTrainingTimeMapping SkillTrainingTimeMapping { get; set; }

		public SecondSkillTreeCostMapping SecondSkillTreeCostMapping { get; set; }

		public LevelSkillPointMapping LevelSkillPointMapping { get; set; }

		public ForgeMapping ForgeMapping { get; set; }

		public MysteryBoxMapping MysteryBoxMapping { get; set; }

		public CampaignCatalog CampaignCatalog { get; set; }

		public StoryMissionRewardMapping StoryMissionRewardMapping { get; set; }

		public HeroicRankMapping.Entry[] HeroicRankMapping { get; set; }

		public MissionLootScalingMapping MissionLootScalingMapping { get; set; }

		public MissionTypeLootMapping MissionTypeLootMapping { get; set; }

		public TutorialCategoryMapping TutorialCategoryMapping { get; set; }

		public MailBoxMapping MailBoxMapping { get; set; }

		public ResourcePropertiesMapping ResourcePropertiesMapping { get; set; }

		public EventsUserFriendlyMapping EventsUserFriendlyMapping { get; set; }

		public EventTemplates EventTemplates { get; set; }

		public TemplateManager()
		{
			ShowShopEffect = false;
			ShowHCInWallet = false;
			ShowAdColonyLastInShop = true;
			AdColonySortId = ((!ShowAdColonyLastInShop) ? 20 : 1000);
			BuyInventorySlotsSortId = int.MaxValue;
		}

		private ShopTable GetAllShopTables()
		{
			if (m_directPurchaseHustonShopTable == null)
			{
				return m_inGameShopTable;
			}
			if (m_inGameShopTable != null)
			{
				ShopTable shopTable = new ShopTable();
				shopTable.version = m_inGameShopTable.version;
				shopTable.bundles = m_inGameShopTable.bundles;
				shopTable.bundles = shopTable.bundles.AddRange(m_directPurchaseHustonShopTable.bundles);
				return shopTable;
			}
			return m_directPurchaseHustonShopTable;
		}
	}
}
