using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Globals")]
	public class Globals : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "ItemComparisonQualityBudgetMapping")]
		public class ItemComparisonQualityBudgetMapping : IExtensible
		{
			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private double _value;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Quality.EQuality.Mundane)]
			public Quality.EQuality quality
			{
				get
				{
					return _quality;
				}
				set
				{
					_quality = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "value", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0.0)]
			public double value
			{
				get
				{
					return _value;
				}
				set
				{
					_value = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "EnergyBarMapping")]
		public class EnergyBarMapping : IExtensible
		{
			private uint _baseMaxEnergy = 40u;

			private uint _initalMaxEnergy = 40u;

			private int _energyIncrementStep = 1;

			private int _missionEnergyCost = 8;

			private ulong _timeIncrementStepInSeconds = 120uL;

			private int _minimumAllowedEnergy;

			private Resources _energyRefillCosts;

			private bool _reduceEnergyOnRetry = true;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "baseMaxEnergy", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(40L)]
			public uint baseMaxEnergy
			{
				get
				{
					return _baseMaxEnergy;
				}
				set
				{
					_baseMaxEnergy = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "initalMaxEnergy", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(40L)]
			public uint initalMaxEnergy
			{
				get
				{
					return _initalMaxEnergy;
				}
				set
				{
					_initalMaxEnergy = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "energyIncrementStep", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(1)]
			public int energyIncrementStep
			{
				get
				{
					return _energyIncrementStep;
				}
				set
				{
					_energyIncrementStep = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "missionEnergyCost", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(8)]
			public int missionEnergyCost
			{
				get
				{
					return _missionEnergyCost;
				}
				set
				{
					_missionEnergyCost = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "timeIncrementStepInSeconds", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(120f)]
			public ulong timeIncrementStepInSeconds
			{
				get
				{
					return _timeIncrementStepInSeconds;
				}
				set
				{
					_timeIncrementStepInSeconds = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "minimumAllowedEnergy", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int minimumAllowedEnergy
			{
				get
				{
					return _minimumAllowedEnergy;
				}
				set
				{
					_minimumAllowedEnergy = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "energyRefillCosts", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resources energyRefillCosts
			{
				get
				{
					return _energyRefillCosts;
				}
				set
				{
					_energyRefillCosts = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "reduceEnergyOnRetry", DataFormat = DataFormat.Default)]
			[DefaultValue(true)]
			public bool reduceEnergyOnRetry
			{
				get
				{
					return _reduceEnergyOnRetry;
				}
				set
				{
					_reduceEnergyOnRetry = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "MissionLevelMapping")]
		public class MissionLevelMapping : IExtensible
		{
			private uint _missionLevel;

			private uint _powRecommendation;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "missionLevel", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint missionLevel
			{
				get
				{
					return _missionLevel;
				}
				set
				{
					_missionLevel = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "powRecommendation", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public uint powRecommendation
			{
				get
				{
					return _powRecommendation;
				}
				set
				{
					_powRecommendation = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private int _initialInventorySize;

		private int _initialSkillpoints;

		private int _initialSoftCurrency;

		private int _numHirelings;

		private int _maxAssassinLevel;

		private int _maxSkillLevel;

		private int _numSkillpointsPerLevelup;

		private int _maxSkillWheelEntries;

		private int _maxItemStackSize;

		private long _version;

		private int _numAssassinSlotsPerPurchase;

		private int _initialAssassinSlots = 2;

		private int _maxFriendsHirelings = 5;

		private long _friendHirelingsActiveThresholdSeconds = 86400L;

		private Resource _usedNonFriendHirelingFcReward;

		private Resource _usedFriendHirelingFcReward;

		private long _userHirelingsActiveThresholdSeconds = 1209600L;

		private Resources _addFriendReward;

		private int _addFriendRewardCoolDown = 604800;

		private double _itemModRecurringWeightModifier = 0.5;

		private double _failedStoryMissionScRewardFactor;

		private double _failedStoryMissionXpRewardFactor;

		private double _failedStoryMissionHtRewardFactor;

		private double _failedRandomMissionScRewardFactor;

		private double _failedRandomMissionXpRewardFactor;

		private double _failedRandomMissionHtRewardFactor;

		private int _mailBodyCharLimit = 2000;

		private int _mailSubjectCharLimit = 128;

		private string _userProfileNameRegex = "^(?=.{1,19}$)(([\\d\\p{L}_'-]+)(\\s[\\d\\p{L}_'-]+)*)+";

		private int _dailyRewardTime;

		private int _dailyRewardNextDaysTeaserCnt = 1;

		private double _itemComparisonModFactor;

		private ItemComparisonQualityBudgetMapping[] _itemComparisonQualityBudgetMappings = new ItemComparisonQualityBudgetMapping[0];

		private double _finishedStoryMissionsRewardMultiplierSc;

		private double _finishedStoryMissionsRewardMultiplierXp;

		private double _finishedStoryMissionsRewardMultiplierHt;

		private int _dailyRewardCrossOverTime = 300;

		private Resources _shareMissionCost;

		private long _mailExpirationTimeSystem = 7776000L;

		private long _mailExpirationTimeP2P = 7776000L;

		private long _mailExpirationTimeMission = 2592000L;

		private int _mailCategoryLimitP2P = 60;

		private int _mailCategoryLimitMission = 40;

		private Resources _refreshMissionListCost;

		private Resources _customAssassinNameCost;

		private string _avatarNameRegex = "^(?=.{1,19}$)(([\\d\\p{L}_'-]+)(\\s[\\d\\p{L}_'-]+)*)+";

		private ModuleQualityMapping _moduleQualityMapping;

		private bool _reviveFeatureEnabled;

		private Resources _reviveCosts;

		private float _itemModWeightWeaponDamage;

		private float _itemModWeightArmorRating;

		private float _itemPowerExponential;

		private float _itemPowerFactor;

		private float _reviveTimer;

		private int _missionsInContractList = 3;

		private Resources _skillPointsTutorialRewardAfterCompletion;

		private Resources _forgeTutorialRewardAfterCompletion;

		private Resources _shopTutorialRewardAfterCompletion;

		private Resources _mysteryBoxTutorialRewardAfterCompletion;

		private string _forgeTutorialInputBlueprintId = "";

		private string[] _defaultEntitlements = new string[0];

		private EnergyBarMapping _energyBarMapping;

		private string _itemLevelReductionSkillGuid = "";

		private string _HumanoidMilitiaGuid = "";

		private string _HumanoidEliteGuid = "";

		private string _HumanoidThiefGuid = "";

		private string _HumanoidSeekerGuid = "";

		private string _HumanoidAgileGuid = "";

		private string _HumanoidArcherGuid = "";

		private string _HumanoidLopurumGuid = "";

		private string _HumanoidSquadLeaderGuid = "";

		private string _HumanoidMercenaryGuid = "";

		private string _HumanoidCrowssbowmanGuid = "";

		private string _HumanoidCrowleaderGuid = "";

		private string _HumanoidGuardCaptainGuid = "";

		private string _HumanoidBruteGuid = "";

		private string _HumanoidPapalGuardGuid = "";

		private string _tutorialEquipItemGuidStr = "";

		private string _tutorialEquipModuleGuidStr = "";

		private string _tutorialShopSpecialItemGuidStr = "";

		private string _tutorialShopItemToHideGuidStr = "";

		private string _tutorialShopHeroicRank1GuidStr = "";

		private string _tutorialForgeItemOneGuidStr = "";

		private string _tutorialForgeItemTwoGuidStr = "";

		private string _tutorialCampaignGuidStr = "";

		private float _eliteMissionChanceInitial;

		private float _eliteMissionChanceIncrease;

		private float _eliteMissionScMultiplier;

		private float _eliteMissionXpMultiplier;

		private float _eliteMissionHtMultiplier;

		private ulong _eliteMissionTimer = 21600000uL;

		private float _SuggestedMissionItemPowerValueA;

		private float _SuggestedMissionItemPowerValueB;

		private float _SuggestedMissionItemPowerValueC;

		private float _SuggestedMissionItemPowerValueD;

		private float _SuggestedMissionItemPowerValueE;

		private float _SuggestedMissionItemPowerValueF;

		private Resource _userFriendHirelingReward;

		private Resource _userNonFriendHirelingReward;

		private float _hirelingRewardForulaMultiplier = 0.3f;

		private string _itemPowerBonusSkillGuid = "";

		private string[] _eyePatchLootTemplateIds = new string[0];

		private MissionLevelMapping[] _missionLevelMapping = new MissionLevelMapping[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "initialInventorySize", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int initialInventorySize
		{
			get
			{
				return _initialInventorySize;
			}
			set
			{
				_initialInventorySize = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "initialSkillpoints", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int initialSkillpoints
		{
			get
			{
				return _initialSkillpoints;
			}
			set
			{
				_initialSkillpoints = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "initialSoftCurrency", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int initialSoftCurrency
		{
			get
			{
				return _initialSoftCurrency;
			}
			set
			{
				_initialSoftCurrency = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "numHirelings", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int numHirelings
		{
			get
			{
				return _numHirelings;
			}
			set
			{
				_numHirelings = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "maxAssassinLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxAssassinLevel
		{
			get
			{
				return _maxAssassinLevel;
			}
			set
			{
				_maxAssassinLevel = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "maxSkillLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxSkillLevel
		{
			get
			{
				return _maxSkillLevel;
			}
			set
			{
				_maxSkillLevel = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "numSkillpointsPerLevelup", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int numSkillpointsPerLevelup
		{
			get
			{
				return _numSkillpointsPerLevelup;
			}
			set
			{
				_numSkillpointsPerLevelup = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "maxSkillWheelEntries", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxSkillWheelEntries
		{
			get
			{
				return _maxSkillWheelEntries;
			}
			set
			{
				_maxSkillWheelEntries = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "maxItemStackSize", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxItemStackSize
		{
			get
			{
				return _maxItemStackSize;
			}
			set
			{
				_maxItemStackSize = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long version
		{
			get
			{
				return _version;
			}
			set
			{
				_version = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "numAssassinSlotsPerPurchase", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int numAssassinSlotsPerPurchase
		{
			get
			{
				return _numAssassinSlotsPerPurchase;
			}
			set
			{
				_numAssassinSlotsPerPurchase = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "initialAssassinSlots", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(2)]
		public int initialAssassinSlots
		{
			get
			{
				return _initialAssassinSlots;
			}
			set
			{
				_initialAssassinSlots = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "maxFriendsHirelings", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(5)]
		public int maxFriendsHirelings
		{
			get
			{
				return _maxFriendsHirelings;
			}
			set
			{
				_maxFriendsHirelings = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "friendHirelingsActiveThresholdSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(86400L)]
		public long friendHirelingsActiveThresholdSeconds
		{
			get
			{
				return _friendHirelingsActiveThresholdSeconds;
			}
			set
			{
				_friendHirelingsActiveThresholdSeconds = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "usedNonFriendHirelingFcReward", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource usedNonFriendHirelingFcReward
		{
			get
			{
				return _usedNonFriendHirelingFcReward;
			}
			set
			{
				_usedNonFriendHirelingFcReward = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "usedFriendHirelingFcReward", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource usedFriendHirelingFcReward
		{
			get
			{
				return _usedFriendHirelingFcReward;
			}
			set
			{
				_usedFriendHirelingFcReward = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "userHirelingsActiveThresholdSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(1209600L)]
		public long userHirelingsActiveThresholdSeconds
		{
			get
			{
				return _userHirelingsActiveThresholdSeconds;
			}
			set
			{
				_userHirelingsActiveThresholdSeconds = value;
			}
		}

		[ProtoMember(32, IsRequired = false, Name = "addFriendReward", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources addFriendReward
		{
			get
			{
				return _addFriendReward;
			}
			set
			{
				_addFriendReward = value;
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "addFriendRewardCoolDown", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(604800)]
		public int addFriendRewardCoolDown
		{
			get
			{
				return _addFriendRewardCoolDown;
			}
			set
			{
				_addFriendRewardCoolDown = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "itemModRecurringWeightModifier", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.5)]
		public double itemModRecurringWeightModifier
		{
			get
			{
				return _itemModRecurringWeightModifier;
			}
			set
			{
				_itemModRecurringWeightModifier = value;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "failedStoryMissionScRewardFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double failedStoryMissionScRewardFactor
		{
			get
			{
				return _failedStoryMissionScRewardFactor;
			}
			set
			{
				_failedStoryMissionScRewardFactor = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "failedStoryMissionXpRewardFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double failedStoryMissionXpRewardFactor
		{
			get
			{
				return _failedStoryMissionXpRewardFactor;
			}
			set
			{
				_failedStoryMissionXpRewardFactor = value;
			}
		}

		[ProtoMember(83, IsRequired = false, Name = "failedStoryMissionHtRewardFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double failedStoryMissionHtRewardFactor
		{
			get
			{
				return _failedStoryMissionHtRewardFactor;
			}
			set
			{
				_failedStoryMissionHtRewardFactor = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "failedRandomMissionScRewardFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double failedRandomMissionScRewardFactor
		{
			get
			{
				return _failedRandomMissionScRewardFactor;
			}
			set
			{
				_failedRandomMissionScRewardFactor = value;
			}
		}

		[ProtoMember(42, IsRequired = false, Name = "failedRandomMissionXpRewardFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double failedRandomMissionXpRewardFactor
		{
			get
			{
				return _failedRandomMissionXpRewardFactor;
			}
			set
			{
				_failedRandomMissionXpRewardFactor = value;
			}
		}

		[ProtoMember(82, IsRequired = false, Name = "failedRandomMissionHtRewardFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double failedRandomMissionHtRewardFactor
		{
			get
			{
				return _failedRandomMissionHtRewardFactor;
			}
			set
			{
				_failedRandomMissionHtRewardFactor = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "mailBodyCharLimit", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(2000)]
		public int mailBodyCharLimit
		{
			get
			{
				return _mailBodyCharLimit;
			}
			set
			{
				_mailBodyCharLimit = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "mailSubjectCharLimit", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(128)]
		public int mailSubjectCharLimit
		{
			get
			{
				return _mailSubjectCharLimit;
			}
			set
			{
				_mailSubjectCharLimit = value;
			}
		}

		[ProtoMember(46, IsRequired = false, Name = "userProfileNameRegex", DataFormat = DataFormat.Default)]
		[DefaultValue("^(?=.{1,19}$)(([\\d\\p{L}_'-]+)(\\s[\\d\\p{L}_'-]+)*)+")]
		public string userProfileNameRegex
		{
			get
			{
				return _userProfileNameRegex;
			}
			set
			{
				_userProfileNameRegex = value;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "dailyRewardTime", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int dailyRewardTime
		{
			get
			{
				return _dailyRewardTime;
			}
			set
			{
				_dailyRewardTime = value;
			}
		}

		[ProtoMember(48, IsRequired = false, Name = "dailyRewardNextDaysTeaserCnt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(1)]
		public int dailyRewardNextDaysTeaserCnt
		{
			get
			{
				return _dailyRewardNextDaysTeaserCnt;
			}
			set
			{
				_dailyRewardNextDaysTeaserCnt = value;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "itemComparisonModFactor", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double itemComparisonModFactor
		{
			get
			{
				return _itemComparisonModFactor;
			}
			set
			{
				_itemComparisonModFactor = value;
			}
		}

		[ProtoMember(50, Name = "itemComparisonQualityBudgetMappings", DataFormat = DataFormat.Default)]
		public ItemComparisonQualityBudgetMapping[] itemComparisonQualityBudgetMappings
		{
			get
			{
				return _itemComparisonQualityBudgetMappings;
			}
			set
			{
				Array.Resize(ref _itemComparisonQualityBudgetMappings, value.Length);
				value.CopyTo(_itemComparisonQualityBudgetMappings, 0);
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "finishedStoryMissionsRewardMultiplierSc", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double finishedStoryMissionsRewardMultiplierSc
		{
			get
			{
				return _finishedStoryMissionsRewardMultiplierSc;
			}
			set
			{
				_finishedStoryMissionsRewardMultiplierSc = value;
			}
		}

		[ProtoMember(52, IsRequired = false, Name = "finishedStoryMissionsRewardMultiplierXp", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double finishedStoryMissionsRewardMultiplierXp
		{
			get
			{
				return _finishedStoryMissionsRewardMultiplierXp;
			}
			set
			{
				_finishedStoryMissionsRewardMultiplierXp = value;
			}
		}

		[ProtoMember(84, IsRequired = false, Name = "finishedStoryMissionsRewardMultiplierHt", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0.0)]
		public double finishedStoryMissionsRewardMultiplierHt
		{
			get
			{
				return _finishedStoryMissionsRewardMultiplierHt;
			}
			set
			{
				_finishedStoryMissionsRewardMultiplierHt = value;
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "dailyRewardCrossOverTime", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(300)]
		public int dailyRewardCrossOverTime
		{
			get
			{
				return _dailyRewardCrossOverTime;
			}
			set
			{
				_dailyRewardCrossOverTime = value;
			}
		}

		[ProtoMember(55, IsRequired = false, Name = "shareMissionCost", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources shareMissionCost
		{
			get
			{
				return _shareMissionCost;
			}
			set
			{
				_shareMissionCost = value;
			}
		}

		[ProtoMember(56, IsRequired = false, Name = "mailExpirationTimeSystem", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(7776000L)]
		public long mailExpirationTimeSystem
		{
			get
			{
				return _mailExpirationTimeSystem;
			}
			set
			{
				_mailExpirationTimeSystem = value;
			}
		}

		[ProtoMember(57, IsRequired = false, Name = "mailExpirationTimeP2P", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(7776000L)]
		public long mailExpirationTimeP2P
		{
			get
			{
				return _mailExpirationTimeP2P;
			}
			set
			{
				_mailExpirationTimeP2P = value;
			}
		}

		[ProtoMember(58, IsRequired = false, Name = "mailExpirationTimeMission", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(2592000L)]
		public long mailExpirationTimeMission
		{
			get
			{
				return _mailExpirationTimeMission;
			}
			set
			{
				_mailExpirationTimeMission = value;
			}
		}

		[ProtoMember(59, IsRequired = false, Name = "mailCategoryLimitP2P", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(60)]
		public int mailCategoryLimitP2P
		{
			get
			{
				return _mailCategoryLimitP2P;
			}
			set
			{
				_mailCategoryLimitP2P = value;
			}
		}

		[ProtoMember(60, IsRequired = false, Name = "mailCategoryLimitMission", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(40)]
		public int mailCategoryLimitMission
		{
			get
			{
				return _mailCategoryLimitMission;
			}
			set
			{
				_mailCategoryLimitMission = value;
			}
		}

		[ProtoMember(61, IsRequired = false, Name = "refreshMissionListCost", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources refreshMissionListCost
		{
			get
			{
				return _refreshMissionListCost;
			}
			set
			{
				_refreshMissionListCost = value;
			}
		}

		[ProtoMember(62, IsRequired = false, Name = "customAssassinNameCost", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources customAssassinNameCost
		{
			get
			{
				return _customAssassinNameCost;
			}
			set
			{
				_customAssassinNameCost = value;
			}
		}

		[ProtoMember(63, IsRequired = false, Name = "avatarNameRegex", DataFormat = DataFormat.Default)]
		[DefaultValue("^(?=.{1,19}$)(([\\d\\p{L}_'-]+)(\\s[\\d\\p{L}_'-]+)*)+")]
		public string avatarNameRegex
		{
			get
			{
				return _avatarNameRegex;
			}
			set
			{
				_avatarNameRegex = value;
			}
		}

		[ProtoMember(64, IsRequired = false, Name = "moduleQualityMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public ModuleQualityMapping moduleQualityMapping
		{
			get
			{
				return _moduleQualityMapping;
			}
			set
			{
				_moduleQualityMapping = value;
			}
		}

		[ProtoMember(65, IsRequired = false, Name = "reviveFeatureEnabled", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool reviveFeatureEnabled
		{
			get
			{
				return _reviveFeatureEnabled;
			}
			set
			{
				_reviveFeatureEnabled = value;
			}
		}

		[ProtoMember(66, IsRequired = false, Name = "reviveCosts", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources reviveCosts
		{
			get
			{
				return _reviveCosts;
			}
			set
			{
				_reviveCosts = value;
			}
		}

		[ProtoMember(67, IsRequired = false, Name = "itemModWeightWeaponDamage", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float itemModWeightWeaponDamage
		{
			get
			{
				return _itemModWeightWeaponDamage;
			}
			set
			{
				_itemModWeightWeaponDamage = value;
			}
		}

		[ProtoMember(68, IsRequired = false, Name = "itemModWeightArmorRating", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float itemModWeightArmorRating
		{
			get
			{
				return _itemModWeightArmorRating;
			}
			set
			{
				_itemModWeightArmorRating = value;
			}
		}

		[ProtoMember(69, IsRequired = false, Name = "itemPowerExponential", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float itemPowerExponential
		{
			get
			{
				return _itemPowerExponential;
			}
			set
			{
				_itemPowerExponential = value;
			}
		}

		[ProtoMember(70, IsRequired = false, Name = "itemPowerFactor", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float itemPowerFactor
		{
			get
			{
				return _itemPowerFactor;
			}
			set
			{
				_itemPowerFactor = value;
			}
		}

		[ProtoMember(71, IsRequired = false, Name = "reviveTimer", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float reviveTimer
		{
			get
			{
				return _reviveTimer;
			}
			set
			{
				_reviveTimer = value;
			}
		}

		[ProtoMember(72, IsRequired = false, Name = "missionsInContractList", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(3)]
		public int missionsInContractList
		{
			get
			{
				return _missionsInContractList;
			}
			set
			{
				_missionsInContractList = value;
			}
		}

		[ProtoMember(76, IsRequired = false, Name = "skillPointsTutorialRewardAfterCompletion", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources skillPointsTutorialRewardAfterCompletion
		{
			get
			{
				return _skillPointsTutorialRewardAfterCompletion;
			}
			set
			{
				_skillPointsTutorialRewardAfterCompletion = value;
			}
		}

		[ProtoMember(77, IsRequired = false, Name = "forgeTutorialRewardAfterCompletion", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources forgeTutorialRewardAfterCompletion
		{
			get
			{
				return _forgeTutorialRewardAfterCompletion;
			}
			set
			{
				_forgeTutorialRewardAfterCompletion = value;
			}
		}

		[ProtoMember(80, IsRequired = false, Name = "shopTutorialRewardAfterCompletion", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources shopTutorialRewardAfterCompletion
		{
			get
			{
				return _shopTutorialRewardAfterCompletion;
			}
			set
			{
				_shopTutorialRewardAfterCompletion = value;
			}
		}

		[ProtoMember(81, IsRequired = false, Name = "mysteryBoxTutorialRewardAfterCompletion", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources mysteryBoxTutorialRewardAfterCompletion
		{
			get
			{
				return _mysteryBoxTutorialRewardAfterCompletion;
			}
			set
			{
				_mysteryBoxTutorialRewardAfterCompletion = value;
			}
		}

		[ProtoMember(86, IsRequired = false, Name = "forgeTutorialInputBlueprintId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string forgeTutorialInputBlueprintId
		{
			get
			{
				return _forgeTutorialInputBlueprintId;
			}
			set
			{
				_forgeTutorialInputBlueprintId = value;
			}
		}

		[ProtoMember(78, Name = "defaultEntitlements", DataFormat = DataFormat.Default)]
		public string[] defaultEntitlements
		{
			get
			{
				return _defaultEntitlements;
			}
			set
			{
				Array.Resize(ref _defaultEntitlements, value.Length);
				value.CopyTo(_defaultEntitlements, 0);
			}
		}

		[ProtoMember(79, IsRequired = false, Name = "energyBarMapping", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public EnergyBarMapping energyBarMapping
		{
			get
			{
				return _energyBarMapping;
			}
			set
			{
				_energyBarMapping = value;
			}
		}

		[ProtoMember(85, IsRequired = false, Name = "itemLevelReductionSkillGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string itemLevelReductionSkillGuid
		{
			get
			{
				return _itemLevelReductionSkillGuid;
			}
			set
			{
				_itemLevelReductionSkillGuid = value;
			}
		}

		[ProtoMember(87, IsRequired = false, Name = "HumanoidMilitiaGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidMilitiaGuid
		{
			get
			{
				return _HumanoidMilitiaGuid;
			}
			set
			{
				_HumanoidMilitiaGuid = value;
			}
		}

		[ProtoMember(88, IsRequired = false, Name = "HumanoidEliteGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidEliteGuid
		{
			get
			{
				return _HumanoidEliteGuid;
			}
			set
			{
				_HumanoidEliteGuid = value;
			}
		}

		[ProtoMember(89, IsRequired = false, Name = "HumanoidThiefGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidThiefGuid
		{
			get
			{
				return _HumanoidThiefGuid;
			}
			set
			{
				_HumanoidThiefGuid = value;
			}
		}

		[ProtoMember(90, IsRequired = false, Name = "HumanoidSeekerGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidSeekerGuid
		{
			get
			{
				return _HumanoidSeekerGuid;
			}
			set
			{
				_HumanoidSeekerGuid = value;
			}
		}

		[ProtoMember(91, IsRequired = false, Name = "HumanoidAgileGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidAgileGuid
		{
			get
			{
				return _HumanoidAgileGuid;
			}
			set
			{
				_HumanoidAgileGuid = value;
			}
		}

		[ProtoMember(92, IsRequired = false, Name = "HumanoidArcherGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidArcherGuid
		{
			get
			{
				return _HumanoidArcherGuid;
			}
			set
			{
				_HumanoidArcherGuid = value;
			}
		}

		[ProtoMember(93, IsRequired = false, Name = "HumanoidLopurumGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidLopurumGuid
		{
			get
			{
				return _HumanoidLopurumGuid;
			}
			set
			{
				_HumanoidLopurumGuid = value;
			}
		}

		[ProtoMember(94, IsRequired = false, Name = "HumanoidSquadLeaderGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidSquadLeaderGuid
		{
			get
			{
				return _HumanoidSquadLeaderGuid;
			}
			set
			{
				_HumanoidSquadLeaderGuid = value;
			}
		}

		[ProtoMember(95, IsRequired = false, Name = "HumanoidMercenaryGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidMercenaryGuid
		{
			get
			{
				return _HumanoidMercenaryGuid;
			}
			set
			{
				_HumanoidMercenaryGuid = value;
			}
		}

		[ProtoMember(96, IsRequired = false, Name = "HumanoidCrowssbowmanGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidCrowssbowmanGuid
		{
			get
			{
				return _HumanoidCrowssbowmanGuid;
			}
			set
			{
				_HumanoidCrowssbowmanGuid = value;
			}
		}

		[ProtoMember(97, IsRequired = false, Name = "HumanoidCrowleaderGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidCrowleaderGuid
		{
			get
			{
				return _HumanoidCrowleaderGuid;
			}
			set
			{
				_HumanoidCrowleaderGuid = value;
			}
		}

		[ProtoMember(98, IsRequired = false, Name = "HumanoidGuardCaptainGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidGuardCaptainGuid
		{
			get
			{
				return _HumanoidGuardCaptainGuid;
			}
			set
			{
				_HumanoidGuardCaptainGuid = value;
			}
		}

		[ProtoMember(99, IsRequired = false, Name = "HumanoidBruteGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidBruteGuid
		{
			get
			{
				return _HumanoidBruteGuid;
			}
			set
			{
				_HumanoidBruteGuid = value;
			}
		}

		[ProtoMember(100, IsRequired = false, Name = "HumanoidPapalGuardGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string HumanoidPapalGuardGuid
		{
			get
			{
				return _HumanoidPapalGuardGuid;
			}
			set
			{
				_HumanoidPapalGuardGuid = value;
			}
		}

		[ProtoMember(101, IsRequired = false, Name = "tutorialEquipItemGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialEquipItemGuidStr
		{
			get
			{
				return _tutorialEquipItemGuidStr;
			}
			set
			{
				_tutorialEquipItemGuidStr = value;
			}
		}

		[ProtoMember(102, IsRequired = false, Name = "tutorialEquipModuleGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialEquipModuleGuidStr
		{
			get
			{
				return _tutorialEquipModuleGuidStr;
			}
			set
			{
				_tutorialEquipModuleGuidStr = value;
			}
		}

		[ProtoMember(103, IsRequired = false, Name = "tutorialShopSpecialItemGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialShopSpecialItemGuidStr
		{
			get
			{
				return _tutorialShopSpecialItemGuidStr;
			}
			set
			{
				_tutorialShopSpecialItemGuidStr = value;
			}
		}

		[ProtoMember(104, IsRequired = false, Name = "tutorialShopItemToHideGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialShopItemToHideGuidStr
		{
			get
			{
				return _tutorialShopItemToHideGuidStr;
			}
			set
			{
				_tutorialShopItemToHideGuidStr = value;
			}
		}

		[ProtoMember(105, IsRequired = false, Name = "tutorialShopHeroicRank1GuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialShopHeroicRank1GuidStr
		{
			get
			{
				return _tutorialShopHeroicRank1GuidStr;
			}
			set
			{
				_tutorialShopHeroicRank1GuidStr = value;
			}
		}

		[ProtoMember(106, IsRequired = false, Name = "tutorialForgeItemOneGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialForgeItemOneGuidStr
		{
			get
			{
				return _tutorialForgeItemOneGuidStr;
			}
			set
			{
				_tutorialForgeItemOneGuidStr = value;
			}
		}

		[ProtoMember(107, IsRequired = false, Name = "tutorialForgeItemTwoGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialForgeItemTwoGuidStr
		{
			get
			{
				return _tutorialForgeItemTwoGuidStr;
			}
			set
			{
				_tutorialForgeItemTwoGuidStr = value;
			}
		}

		[ProtoMember(108, IsRequired = false, Name = "tutorialCampaignGuidStr", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string tutorialCampaignGuidStr
		{
			get
			{
				return _tutorialCampaignGuidStr;
			}
			set
			{
				_tutorialCampaignGuidStr = value;
			}
		}

		[ProtoMember(111, IsRequired = false, Name = "eliteMissionChanceInitial", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float eliteMissionChanceInitial
		{
			get
			{
				return _eliteMissionChanceInitial;
			}
			set
			{
				_eliteMissionChanceInitial = value;
			}
		}

		[ProtoMember(112, IsRequired = false, Name = "eliteMissionChanceIncrease", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float eliteMissionChanceIncrease
		{
			get
			{
				return _eliteMissionChanceIncrease;
			}
			set
			{
				_eliteMissionChanceIncrease = value;
			}
		}

		[ProtoMember(113, IsRequired = false, Name = "eliteMissionScMultiplier", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float eliteMissionScMultiplier
		{
			get
			{
				return _eliteMissionScMultiplier;
			}
			set
			{
				_eliteMissionScMultiplier = value;
			}
		}

		[ProtoMember(114, IsRequired = false, Name = "eliteMissionXpMultiplier", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float eliteMissionXpMultiplier
		{
			get
			{
				return _eliteMissionXpMultiplier;
			}
			set
			{
				_eliteMissionXpMultiplier = value;
			}
		}

		[ProtoMember(115, IsRequired = false, Name = "eliteMissionHtMultiplier", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float eliteMissionHtMultiplier
		{
			get
			{
				return _eliteMissionHtMultiplier;
			}
			set
			{
				_eliteMissionHtMultiplier = value;
			}
		}

		[ProtoMember(121, IsRequired = false, Name = "eliteMissionTimer", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(21600000f)]
		public ulong eliteMissionTimer
		{
			get
			{
				return _eliteMissionTimer;
			}
			set
			{
				_eliteMissionTimer = value;
			}
		}

		[ProtoMember(116, IsRequired = false, Name = "SuggestedMissionItemPowerValueA", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float SuggestedMissionItemPowerValueA
		{
			get
			{
				return _SuggestedMissionItemPowerValueA;
			}
			set
			{
				_SuggestedMissionItemPowerValueA = value;
			}
		}

		[ProtoMember(117, IsRequired = false, Name = "SuggestedMissionItemPowerValueB", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float SuggestedMissionItemPowerValueB
		{
			get
			{
				return _SuggestedMissionItemPowerValueB;
			}
			set
			{
				_SuggestedMissionItemPowerValueB = value;
			}
		}

		[ProtoMember(118, IsRequired = false, Name = "SuggestedMissionItemPowerValueC", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float SuggestedMissionItemPowerValueC
		{
			get
			{
				return _SuggestedMissionItemPowerValueC;
			}
			set
			{
				_SuggestedMissionItemPowerValueC = value;
			}
		}

		[ProtoMember(119, IsRequired = false, Name = "SuggestedMissionItemPowerValueD", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float SuggestedMissionItemPowerValueD
		{
			get
			{
				return _SuggestedMissionItemPowerValueD;
			}
			set
			{
				_SuggestedMissionItemPowerValueD = value;
			}
		}

		[ProtoMember(120, IsRequired = false, Name = "SuggestedMissionItemPowerValueE", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float SuggestedMissionItemPowerValueE
		{
			get
			{
				return _SuggestedMissionItemPowerValueE;
			}
			set
			{
				_SuggestedMissionItemPowerValueE = value;
			}
		}

		[ProtoMember(122, IsRequired = false, Name = "SuggestedMissionItemPowerValueF", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float SuggestedMissionItemPowerValueF
		{
			get
			{
				return _SuggestedMissionItemPowerValueF;
			}
			set
			{
				_SuggestedMissionItemPowerValueF = value;
			}
		}

		[ProtoMember(123, IsRequired = false, Name = "userFriendHirelingReward", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource userFriendHirelingReward
		{
			get
			{
				return _userFriendHirelingReward;
			}
			set
			{
				_userFriendHirelingReward = value;
			}
		}

		[ProtoMember(124, IsRequired = false, Name = "userNonFriendHirelingReward", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resource userNonFriendHirelingReward
		{
			get
			{
				return _userNonFriendHirelingReward;
			}
			set
			{
				_userNonFriendHirelingReward = value;
			}
		}

		[ProtoMember(125, IsRequired = false, Name = "hirelingRewardForulaMultiplier", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0.3f)]
		public float hirelingRewardForulaMultiplier
		{
			get
			{
				return _hirelingRewardForulaMultiplier;
			}
			set
			{
				_hirelingRewardForulaMultiplier = value;
			}
		}

		[ProtoMember(126, IsRequired = false, Name = "itemPowerBonusSkillGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string itemPowerBonusSkillGuid
		{
			get
			{
				return _itemPowerBonusSkillGuid;
			}
			set
			{
				_itemPowerBonusSkillGuid = value;
			}
		}

		[ProtoMember(127, Name = "eyePatchLootTemplateIds", DataFormat = DataFormat.Default)]
		public string[] eyePatchLootTemplateIds
		{
			get
			{
				return _eyePatchLootTemplateIds;
			}
			set
			{
				Array.Resize(ref _eyePatchLootTemplateIds, value.Length);
				value.CopyTo(_eyePatchLootTemplateIds, 0);
			}
		}

		[ProtoMember(128, Name = "missionLevelMapping", DataFormat = DataFormat.Default)]
		public MissionLevelMapping[] missionLevelMapping
		{
			get
			{
				return _missionLevelMapping;
			}
			set
			{
				Array.Resize(ref _missionLevelMapping, value.Length);
				value.CopyTo(_missionLevelMapping, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
