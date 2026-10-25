using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionResult")]
	public class MissionResult : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "NotorietyStats")]
		public class NotorietyStats : IExtensible
		{
			private EAlertLevel _level;

			private int _num;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EAlertLevel.Anonymous)]
			public EAlertLevel level
			{
				get
				{
					return _level;
				}
				set
				{
					_level = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int num
			{
				get
				{
					return _num;
				}
				set
				{
					_num = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "SkillStats")]
		public class SkillStats : IExtensible
		{
			private int _num;

			private int _duration;

			private int _ruleGuidHash;

			private IExtension extensionObject;

			[ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int num
			{
				get
				{
					return _num;
				}
				set
				{
					_num = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int duration
			{
				get
				{
					return _duration;
				}
				set
				{
					_duration = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "ruleGuidHash", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int ruleGuidHash
			{
				get
				{
					return _ruleGuidHash;
				}
				set
				{
					_ruleGuidHash = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "HotspotStats")]
		public class HotspotStats : IExtensible
		{
			private string _hotspotName = "";

			private int _num;

			private int _duration;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "hotspotName", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string hotspotName
			{
				get
				{
					return _hotspotName;
				}
				set
				{
					_hotspotName = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int num
			{
				get
				{
					return _num;
				}
				set
				{
					_num = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int duration
			{
				get
				{
					return _duration;
				}
				set
				{
					_duration = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "NpcStats")]
		public class NpcStats : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "SkillUsageStats")]
			public class SkillUsageStats : IExtensible
			{
				private int _num;

				private int _ruleGuidHash;

				private IExtension extensionObject;

				[ProtoMember(2, IsRequired = false, Name = "num", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int num
				{
					get
					{
						return _num;
					}
					set
					{
						_num = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "ruleGuidHash", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int ruleGuidHash
				{
					get
					{
						return _ruleGuidHash;
					}
					set
					{
						_ruleGuidHash = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private int _npcTypeGameIdHash;

			private SkillUsageStats[] _skillUsageStats = new SkillUsageStats[0];

			private int _killCount;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "npcTypeGameIdHash", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int npcTypeGameIdHash
			{
				get
				{
					return _npcTypeGameIdHash;
				}
				set
				{
					_npcTypeGameIdHash = value;
				}
			}

			[ProtoMember(2, Name = "skillUsageStats", DataFormat = DataFormat.Default)]
			public SkillUsageStats[] skillUsageStats
			{
				get
				{
					return _skillUsageStats;
				}
				set
				{
					Array.Resize(ref _skillUsageStats, value.Length);
					value.CopyTo(_skillUsageStats, 0);
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "killCount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int killCount
			{
				get
				{
					return _killCount;
				}
				set
				{
					_killCount = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private int _assassinationCount;

		private int _combatCount;

		private int _enemiesKilledInCombatCount;

		private int _enterGroundCount;

		private int _enterRoofCount;

		private int _fledCount;

		private int _killedEnemyCount;

		private float _lowestBaseHealth;

		private float _lowestBaseHealthMax;

		private int _usedHidingSpotCount;

		private int _usedSkillsCount;

		private NotorietyStats[] _notorietyStats = new NotorietyStats[0];

		private SkillStats[] _skillStats = new SkillStats[0];

		private HotspotStats[] _hotspotStats = new HotspotStats[0];

		private int _bleedingSplinterBombSeconds;

		private int _dieByYourOwnBombCount;

		private int _restOnBenchSeconds;

		private int _enemiesFledCount;

		private int _airAssassinationCount;

		private int _killsWhileGrabbingMoney;

		private int _killsWhileKnockedDown;

		private int _killsOutOfSmokeBombCover;

		private int _killsBySkills;

		private int _enemiesKilledOnRooftops;

		private int _enemiesKilledWith5PercentHealth;

		private int _enemiesKilledWithCombo;

		private int _diesInMission;

		private int _enemiesKilledWithin10Minutes;

		private int _rooftopGardenAssassinationCount;

		private int _sentCourtesansCount;

		private NpcStats[] _npcStats = new NpcStats[0];

		private int[] _succeededChallengeIds = new int[0];

		private int _duration;

		private long _failReasonID;

		private int _failReasonLocaID;

		private int _killsFromCivilianCover;

		private int _flushedPigeons;

		private int _switchedTarget;

		private int _successfullyParried;

		private int _successfullHirelingAttacks;

		private PerformanceTrack _performanceTrack;

		private TrackingDeviceInfo _deviceInfoTrack;

		private int _treasureChestsOpenedInMission;

		private int _messagesDeliveredInMission;

		private int _assassinatedMissionTargets;

		private int _lockPickUsed;

		private int _enemiesShredded;

		private int _frightenedTimeInSeconds;

		private int _enemiesKilledByBomb;

		private int _criticalHits;

		private int _crowsKilled;

		private int _secondsTracedBySeekers;

		private int _maxSecondsOnGround;

		private int _enemiesRemaining;

		private int _reviveCountOnClient;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "assassinationCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int assassinationCount
		{
			get
			{
				return _assassinationCount;
			}
			set
			{
				_assassinationCount = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "combatCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int combatCount
		{
			get
			{
				return _combatCount;
			}
			set
			{
				_combatCount = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "enemiesKilledInCombatCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesKilledInCombatCount
		{
			get
			{
				return _enemiesKilledInCombatCount;
			}
			set
			{
				_enemiesKilledInCombatCount = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "enterGroundCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enterGroundCount
		{
			get
			{
				return _enterGroundCount;
			}
			set
			{
				_enterGroundCount = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "enterRoofCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enterRoofCount
		{
			get
			{
				return _enterRoofCount;
			}
			set
			{
				_enterRoofCount = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "fledCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int fledCount
		{
			get
			{
				return _fledCount;
			}
			set
			{
				_fledCount = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "killedEnemyCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int killedEnemyCount
		{
			get
			{
				return _killedEnemyCount;
			}
			set
			{
				_killedEnemyCount = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "lowestBaseHealth", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float lowestBaseHealth
		{
			get
			{
				return _lowestBaseHealth;
			}
			set
			{
				_lowestBaseHealth = value;
			}
		}

		[ProtoMember(9, IsRequired = false, Name = "lowestBaseHealthMax", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float lowestBaseHealthMax
		{
			get
			{
				return _lowestBaseHealthMax;
			}
			set
			{
				_lowestBaseHealthMax = value;
			}
		}

		[ProtoMember(10, IsRequired = false, Name = "usedHidingSpotCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int usedHidingSpotCount
		{
			get
			{
				return _usedHidingSpotCount;
			}
			set
			{
				_usedHidingSpotCount = value;
			}
		}

		[ProtoMember(11, IsRequired = false, Name = "usedSkillsCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int usedSkillsCount
		{
			get
			{
				return _usedSkillsCount;
			}
			set
			{
				_usedSkillsCount = value;
			}
		}

		[ProtoMember(12, Name = "notorietyStats", DataFormat = DataFormat.Default)]
		public NotorietyStats[] notorietyStats
		{
			get
			{
				return _notorietyStats;
			}
			set
			{
				Array.Resize(ref _notorietyStats, value.Length);
				value.CopyTo(_notorietyStats, 0);
			}
		}

		[ProtoMember(13, Name = "skillStats", DataFormat = DataFormat.Default)]
		public SkillStats[] skillStats
		{
			get
			{
				return _skillStats;
			}
			set
			{
				Array.Resize(ref _skillStats, value.Length);
				value.CopyTo(_skillStats, 0);
			}
		}

		[ProtoMember(14, Name = "hotspotStats", DataFormat = DataFormat.Default)]
		public HotspotStats[] hotspotStats
		{
			get
			{
				return _hotspotStats;
			}
			set
			{
				Array.Resize(ref _hotspotStats, value.Length);
				value.CopyTo(_hotspotStats, 0);
			}
		}

		[ProtoMember(15, IsRequired = false, Name = "bleedingSplinterBombSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int bleedingSplinterBombSeconds
		{
			get
			{
				return _bleedingSplinterBombSeconds;
			}
			set
			{
				_bleedingSplinterBombSeconds = value;
			}
		}

		[ProtoMember(16, IsRequired = false, Name = "dieByYourOwnBombCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int dieByYourOwnBombCount
		{
			get
			{
				return _dieByYourOwnBombCount;
			}
			set
			{
				_dieByYourOwnBombCount = value;
			}
		}

		[ProtoMember(17, IsRequired = false, Name = "restOnBenchSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int restOnBenchSeconds
		{
			get
			{
				return _restOnBenchSeconds;
			}
			set
			{
				_restOnBenchSeconds = value;
			}
		}

		[ProtoMember(18, IsRequired = false, Name = "enemiesFledCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesFledCount
		{
			get
			{
				return _enemiesFledCount;
			}
			set
			{
				_enemiesFledCount = value;
			}
		}

		[ProtoMember(19, IsRequired = false, Name = "airAssassinationCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int airAssassinationCount
		{
			get
			{
				return _airAssassinationCount;
			}
			set
			{
				_airAssassinationCount = value;
			}
		}

		[ProtoMember(20, IsRequired = false, Name = "killsWhileGrabbingMoney", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int killsWhileGrabbingMoney
		{
			get
			{
				return _killsWhileGrabbingMoney;
			}
			set
			{
				_killsWhileGrabbingMoney = value;
			}
		}

		[ProtoMember(21, IsRequired = false, Name = "killsWhileKnockedDown", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int killsWhileKnockedDown
		{
			get
			{
				return _killsWhileKnockedDown;
			}
			set
			{
				_killsWhileKnockedDown = value;
			}
		}

		[ProtoMember(22, IsRequired = false, Name = "killsOutOfSmokeBombCover", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int killsOutOfSmokeBombCover
		{
			get
			{
				return _killsOutOfSmokeBombCover;
			}
			set
			{
				_killsOutOfSmokeBombCover = value;
			}
		}

		[ProtoMember(23, IsRequired = false, Name = "killsBySkills", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int killsBySkills
		{
			get
			{
				return _killsBySkills;
			}
			set
			{
				_killsBySkills = value;
			}
		}

		[ProtoMember(24, IsRequired = false, Name = "enemiesKilledOnRooftops", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesKilledOnRooftops
		{
			get
			{
				return _enemiesKilledOnRooftops;
			}
			set
			{
				_enemiesKilledOnRooftops = value;
			}
		}

		[ProtoMember(25, IsRequired = false, Name = "enemiesKilledWith5PercentHealth", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesKilledWith5PercentHealth
		{
			get
			{
				return _enemiesKilledWith5PercentHealth;
			}
			set
			{
				_enemiesKilledWith5PercentHealth = value;
			}
		}

		[ProtoMember(26, IsRequired = false, Name = "enemiesKilledWithCombo", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesKilledWithCombo
		{
			get
			{
				return _enemiesKilledWithCombo;
			}
			set
			{
				_enemiesKilledWithCombo = value;
			}
		}

		[ProtoMember(27, IsRequired = false, Name = "diesInMission", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int diesInMission
		{
			get
			{
				return _diesInMission;
			}
			set
			{
				_diesInMission = value;
			}
		}

		[ProtoMember(28, IsRequired = false, Name = "enemiesKilledWithin10Minutes", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesKilledWithin10Minutes
		{
			get
			{
				return _enemiesKilledWithin10Minutes;
			}
			set
			{
				_enemiesKilledWithin10Minutes = value;
			}
		}

		[ProtoMember(29, IsRequired = false, Name = "rooftopGardenAssassinationCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int rooftopGardenAssassinationCount
		{
			get
			{
				return _rooftopGardenAssassinationCount;
			}
			set
			{
				_rooftopGardenAssassinationCount = value;
			}
		}

		[ProtoMember(30, IsRequired = false, Name = "sentCourtesansCount", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int sentCourtesansCount
		{
			get
			{
				return _sentCourtesansCount;
			}
			set
			{
				_sentCourtesansCount = value;
			}
		}

		[ProtoMember(31, Name = "npcStats", DataFormat = DataFormat.Default)]
		public NpcStats[] npcStats
		{
			get
			{
				return _npcStats;
			}
			set
			{
				Array.Resize(ref _npcStats, value.Length);
				value.CopyTo(_npcStats, 0);
			}
		}

		[ProtoMember(32, Name = "succeededChallengeIds", DataFormat = DataFormat.TwosComplement)]
		public int[] succeededChallengeIds
		{
			get
			{
				return _succeededChallengeIds;
			}
			set
			{
				Array.Resize(ref _succeededChallengeIds, value.Length);
				value.CopyTo(_succeededChallengeIds, 0);
			}
		}

		[ProtoMember(33, IsRequired = false, Name = "duration", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int duration
		{
			get
			{
				return _duration;
			}
			set
			{
				_duration = value;
			}
		}

		[ProtoMember(34, IsRequired = false, Name = "failReasonID", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0L)]
		public long failReasonID
		{
			get
			{
				return _failReasonID;
			}
			set
			{
				_failReasonID = value;
			}
		}

		[ProtoMember(35, IsRequired = false, Name = "failReasonLocaID", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int failReasonLocaID
		{
			get
			{
				return _failReasonLocaID;
			}
			set
			{
				_failReasonLocaID = value;
			}
		}

		[ProtoMember(36, IsRequired = false, Name = "killsFromCivilianCover", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int killsFromCivilianCover
		{
			get
			{
				return _killsFromCivilianCover;
			}
			set
			{
				_killsFromCivilianCover = value;
			}
		}

		[ProtoMember(37, IsRequired = false, Name = "flushedPigeons", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int flushedPigeons
		{
			get
			{
				return _flushedPigeons;
			}
			set
			{
				_flushedPigeons = value;
			}
		}

		[ProtoMember(38, IsRequired = false, Name = "switchedTarget", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int switchedTarget
		{
			get
			{
				return _switchedTarget;
			}
			set
			{
				_switchedTarget = value;
			}
		}

		[ProtoMember(39, IsRequired = false, Name = "successfullyParried", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int successfullyParried
		{
			get
			{
				return _successfullyParried;
			}
			set
			{
				_successfullyParried = value;
			}
		}

		[ProtoMember(40, IsRequired = false, Name = "successfullHirelingAttacks", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int successfullHirelingAttacks
		{
			get
			{
				return _successfullHirelingAttacks;
			}
			set
			{
				_successfullHirelingAttacks = value;
			}
		}

		[ProtoMember(41, IsRequired = false, Name = "performanceTrack", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public PerformanceTrack performanceTrack
		{
			get
			{
				return _performanceTrack;
			}
			set
			{
				_performanceTrack = value;
			}
		}

		[ProtoMember(42, IsRequired = false, Name = "deviceInfoTrack", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public TrackingDeviceInfo deviceInfoTrack
		{
			get
			{
				return _deviceInfoTrack;
			}
			set
			{
				_deviceInfoTrack = value;
			}
		}

		[ProtoMember(43, IsRequired = false, Name = "treasureChestsOpenedInMission", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int treasureChestsOpenedInMission
		{
			get
			{
				return _treasureChestsOpenedInMission;
			}
			set
			{
				_treasureChestsOpenedInMission = value;
			}
		}

		[ProtoMember(44, IsRequired = false, Name = "messagesDeliveredInMission", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int messagesDeliveredInMission
		{
			get
			{
				return _messagesDeliveredInMission;
			}
			set
			{
				_messagesDeliveredInMission = value;
			}
		}

		[ProtoMember(45, IsRequired = false, Name = "assassinatedMissionTargets", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int assassinatedMissionTargets
		{
			get
			{
				return _assassinatedMissionTargets;
			}
			set
			{
				_assassinatedMissionTargets = value;
			}
		}

		[ProtoMember(46, IsRequired = false, Name = "lockPickUsed", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int lockPickUsed
		{
			get
			{
				return _lockPickUsed;
			}
			set
			{
				_lockPickUsed = value;
			}
		}

		[ProtoMember(47, IsRequired = false, Name = "enemiesShredded", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesShredded
		{
			get
			{
				return _enemiesShredded;
			}
			set
			{
				_enemiesShredded = value;
			}
		}

		[ProtoMember(48, IsRequired = false, Name = "frightenedTimeInSeconds", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int frightenedTimeInSeconds
		{
			get
			{
				return _frightenedTimeInSeconds;
			}
			set
			{
				_frightenedTimeInSeconds = value;
			}
		}

		[ProtoMember(49, IsRequired = false, Name = "enemiesKilledByBomb", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesKilledByBomb
		{
			get
			{
				return _enemiesKilledByBomb;
			}
			set
			{
				_enemiesKilledByBomb = value;
			}
		}

		[ProtoMember(50, IsRequired = false, Name = "criticalHits", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int criticalHits
		{
			get
			{
				return _criticalHits;
			}
			set
			{
				_criticalHits = value;
			}
		}

		[ProtoMember(51, IsRequired = false, Name = "crowsKilled", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int crowsKilled
		{
			get
			{
				return _crowsKilled;
			}
			set
			{
				_crowsKilled = value;
			}
		}

		[ProtoMember(52, IsRequired = false, Name = "secondsTracedBySeekers", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int secondsTracedBySeekers
		{
			get
			{
				return _secondsTracedBySeekers;
			}
			set
			{
				_secondsTracedBySeekers = value;
			}
		}

		[ProtoMember(53, IsRequired = false, Name = "maxSecondsOnGround", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int maxSecondsOnGround
		{
			get
			{
				return _maxSecondsOnGround;
			}
			set
			{
				_maxSecondsOnGround = value;
			}
		}

		[ProtoMember(54, IsRequired = false, Name = "enemiesRemaining", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int enemiesRemaining
		{
			get
			{
				return _enemiesRemaining;
			}
			set
			{
				_enemiesRemaining = value;
			}
		}

		[ProtoMember(55, IsRequired = false, Name = "reviveCountOnClient", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int reviveCountOnClient
		{
			get
			{
				return _reviveCountOnClient;
			}
			set
			{
				_reviveCountOnClient = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
