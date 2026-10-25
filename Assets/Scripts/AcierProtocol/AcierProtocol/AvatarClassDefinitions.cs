using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AvatarClassDefinitions")]
	public class AvatarClassDefinitions : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "AvatarClass")]
		public class AvatarClass : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "SecondaryClassData")]
			public class SecondaryClassData : IExtensible
			{
				[Serializable]
				[ProtoContract(Name = "SkillData")]
				public class SkillData : IExtensible
				{
					private string _guid = "";

					private float _parameter;

					private IExtension extensionObject;

					[ProtoMember(1, IsRequired = false, Name = "guid", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string guid
					{
						get
						{
							return _guid;
						}
						set
						{
							_guid = value;
						}
					}

					[ProtoMember(2, IsRequired = false, Name = "parameter", DataFormat = DataFormat.FixedSize)]
					[DefaultValue(0f)]
					public float parameter
					{
						get
						{
							return _parameter;
						}
						set
						{
							_parameter = value;
						}
					}

					IExtension IExtensible.GetExtensionObject(bool createIfMissing)
					{
						return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
					}
				}

				private Assassin.EClassTypeId _classTypeId = Assassin.EClassTypeId.None;

				private SkillData[] _skills = new SkillData[0];

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "classTypeId", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(Assassin.EClassTypeId.None)]
				public Assassin.EClassTypeId classTypeId
				{
					get
					{
						return _classTypeId;
					}
					set
					{
						_classTypeId = value;
					}
				}

				[ProtoMember(2, Name = "skills", DataFormat = DataFormat.Default)]
				public SkillData[] skills
				{
					get
					{
						return _skills;
					}
					set
					{
						Array.Resize(ref _skills, value.Length);
						value.CopyTo(_skills, 0);
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			[Serializable]
			[ProtoContract(Name = "AvatarHeadSettings")]
			public class AvatarHeadSettings : IExtensible
			{
				private string _headGuid = "";

				private string[] _headOptions = new string[0];

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "headGuid", DataFormat = DataFormat.Default)]
				[DefaultValue("")]
				public string headGuid
				{
					get
					{
						return _headGuid;
					}
					set
					{
						_headGuid = value;
					}
				}

				[ProtoMember(2, Name = "headOptions", DataFormat = DataFormat.Default)]
				public string[] headOptions
				{
					get
					{
						return _headOptions;
					}
					set
					{
						Array.Resize(ref _headOptions, value.Length);
						value.CopyTo(_headOptions, 0);
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			[Serializable]
			[ProtoContract(Name = "AvatarTier")]
			public class AvatarTier : IExtensible
			{
				[Serializable]
				[ProtoContract(Name = "AvatarSkill")]
				public class AvatarSkill : IExtensible
				{
					[Serializable]
					[ProtoContract(Name = "TrainingData")]
					public class TrainingData : IExtensible
					{
						private int _level;

						private Resources _costs;

						private Resources _quickFinishCosts;

						private IExtension extensionObject;

						[ProtoMember(1, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
						[DefaultValue(0)]
						public int level
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

						[ProtoMember(3, IsRequired = false, Name = "costs", DataFormat = DataFormat.Default)]
						[DefaultValue(null)]
						public Resources costs
						{
							get
							{
								return _costs;
							}
							set
							{
								_costs = value;
							}
						}

						[ProtoMember(4, IsRequired = false, Name = "quickFinishCosts", DataFormat = DataFormat.Default)]
						[DefaultValue(null)]
						public Resources quickFinishCosts
						{
							get
							{
								return _quickFinishCosts;
							}
							set
							{
								_quickFinishCosts = value;
							}
						}

						IExtension IExtensible.GetExtensionObject(bool createIfMissing)
						{
							return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
						}
					}

					private string _name = "";

					private string _guid = "";

					private TrainingData[] _trainingData = new TrainingData[0];

					private IExtension extensionObject;

					[ProtoMember(1, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string name
					{
						get
						{
							return _name;
						}
						set
						{
							_name = value;
						}
					}

					[ProtoMember(2, IsRequired = false, Name = "guid", DataFormat = DataFormat.Default)]
					[DefaultValue("")]
					public string guid
					{
						get
						{
							return _guid;
						}
						set
						{
							_guid = value;
						}
					}

					[ProtoMember(5, Name = "trainingData", DataFormat = DataFormat.Default)]
					public TrainingData[] trainingData
					{
						get
						{
							return _trainingData;
						}
						set
						{
							Array.Resize(ref _trainingData, value.Length);
							value.CopyTo(_trainingData, 0);
						}
					}

					IExtension IExtensible.GetExtensionObject(bool createIfMissing)
					{
						return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
					}
				}

				private int _id;

				private AvatarSkill _activeSkill;

				private AvatarSkill _passiveSkillOne;

				private AvatarSkill _passiveSkillTwo;

				private bool _passiveOneConnected;

				private Resources _tierPayUnlockCosts;

				private Resources _secondPassivePayUnlockCosts;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int id
				{
					get
					{
						return _id;
					}
					set
					{
						_id = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "activeSkill", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public AvatarSkill activeSkill
				{
					get
					{
						return _activeSkill;
					}
					set
					{
						_activeSkill = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "passiveSkillOne", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public AvatarSkill passiveSkillOne
				{
					get
					{
						return _passiveSkillOne;
					}
					set
					{
						_passiveSkillOne = value;
					}
				}

				[ProtoMember(4, IsRequired = false, Name = "passiveSkillTwo", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public AvatarSkill passiveSkillTwo
				{
					get
					{
						return _passiveSkillTwo;
					}
					set
					{
						_passiveSkillTwo = value;
					}
				}

				[ProtoMember(5, IsRequired = false, Name = "passiveOneConnected", DataFormat = DataFormat.Default)]
				[DefaultValue(false)]
				public bool passiveOneConnected
				{
					get
					{
						return _passiveOneConnected;
					}
					set
					{
						_passiveOneConnected = value;
					}
				}

				[ProtoMember(6, IsRequired = false, Name = "tierPayUnlockCosts", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public Resources tierPayUnlockCosts
				{
					get
					{
						return _tierPayUnlockCosts;
					}
					set
					{
						_tierPayUnlockCosts = value;
					}
				}

				[ProtoMember(7, IsRequired = false, Name = "secondPassivePayUnlockCosts", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public Resources secondPassivePayUnlockCosts
				{
					get
					{
						return _secondPassivePayUnlockCosts;
					}
					set
					{
						_secondPassivePayUnlockCosts = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private Assassin.EClassTypeId _classTypeId = Assassin.EClassTypeId.None;

			private string _humanoidGuid = "";

			private string _weaponGuid = "";

			private AvatarHeadSettings _heads;

			private AvatarTier[] _tiers = new AvatarTier[0];

			private string[] _hirelingAbilityGuids = new string[0];

			private string _colorSetting = "";

			private long _hitPoints;

			private long _strength;

			private long _precision;

			private long _armoreRating;

			private long _incognito;

			private long _evasion;

			private string _noviceHumanoidGuid = "";

			private string _noviceColorSetting = "";

			private int _killTokensRequiredForKill;

			private float _criticalMultiplier;

			private EarlyAccessInfo _earlyAccessInfo;

			private SecondaryClassData[] _secondaryClassInfos = new SecondaryClassData[0];

			private string[] _reqEntitlements = new string[0];

			private int _defaultFirstNameLocaId;

			private int _defaultLastNameLocaId;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "classTypeId", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(Assassin.EClassTypeId.None)]
			public Assassin.EClassTypeId classTypeId
			{
				get
				{
					return _classTypeId;
				}
				set
				{
					_classTypeId = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "humanoidGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string humanoidGuid
			{
				get
				{
					return _humanoidGuid;
				}
				set
				{
					_humanoidGuid = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "weaponGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string weaponGuid
			{
				get
				{
					return _weaponGuid;
				}
				set
				{
					_weaponGuid = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "heads", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public AvatarHeadSettings heads
			{
				get
				{
					return _heads;
				}
				set
				{
					_heads = value;
				}
			}

			[ProtoMember(6, Name = "tiers", DataFormat = DataFormat.Default)]
			public AvatarTier[] tiers
			{
				get
				{
					return _tiers;
				}
				set
				{
					Array.Resize(ref _tiers, value.Length);
					value.CopyTo(_tiers, 0);
				}
			}

			[ProtoMember(7, Name = "hirelingAbilityGuids", DataFormat = DataFormat.Default)]
			public string[] hirelingAbilityGuids
			{
				get
				{
					return _hirelingAbilityGuids;
				}
				set
				{
					Array.Resize(ref _hirelingAbilityGuids, value.Length);
					value.CopyTo(_hirelingAbilityGuids, 0);
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "colorSetting", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string colorSetting
			{
				get
				{
					return _colorSetting;
				}
				set
				{
					_colorSetting = value;
				}
			}

			[ProtoMember(9, IsRequired = false, Name = "hitPoints", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long hitPoints
			{
				get
				{
					return _hitPoints;
				}
				set
				{
					_hitPoints = value;
				}
			}

			[ProtoMember(10, IsRequired = false, Name = "strength", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long strength
			{
				get
				{
					return _strength;
				}
				set
				{
					_strength = value;
				}
			}

			[ProtoMember(11, IsRequired = false, Name = "precision", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long precision
			{
				get
				{
					return _precision;
				}
				set
				{
					_precision = value;
				}
			}

			[ProtoMember(12, IsRequired = false, Name = "armoreRating", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long armoreRating
			{
				get
				{
					return _armoreRating;
				}
				set
				{
					_armoreRating = value;
				}
			}

			[ProtoMember(13, IsRequired = false, Name = "incognito", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long incognito
			{
				get
				{
					return _incognito;
				}
				set
				{
					_incognito = value;
				}
			}

			[ProtoMember(14, IsRequired = false, Name = "evasion", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long evasion
			{
				get
				{
					return _evasion;
				}
				set
				{
					_evasion = value;
				}
			}

			[ProtoMember(15, IsRequired = false, Name = "noviceHumanoidGuid", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string noviceHumanoidGuid
			{
				get
				{
					return _noviceHumanoidGuid;
				}
				set
				{
					_noviceHumanoidGuid = value;
				}
			}

			[ProtoMember(16, IsRequired = false, Name = "noviceColorSetting", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string noviceColorSetting
			{
				get
				{
					return _noviceColorSetting;
				}
				set
				{
					_noviceColorSetting = value;
				}
			}

			[ProtoMember(17, IsRequired = false, Name = "killTokensRequiredForKill", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int killTokensRequiredForKill
			{
				get
				{
					return _killTokensRequiredForKill;
				}
				set
				{
					_killTokensRequiredForKill = value;
				}
			}

			[ProtoMember(18, IsRequired = false, Name = "criticalMultiplier", DataFormat = DataFormat.FixedSize)]
			[DefaultValue(0f)]
			public float criticalMultiplier
			{
				get
				{
					return _criticalMultiplier;
				}
				set
				{
					_criticalMultiplier = value;
				}
			}

			[ProtoMember(19, IsRequired = false, Name = "earlyAccessInfo", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public EarlyAccessInfo earlyAccessInfo
			{
				get
				{
					return _earlyAccessInfo;
				}
				set
				{
					_earlyAccessInfo = value;
				}
			}

			[ProtoMember(20, Name = "secondaryClassInfos", DataFormat = DataFormat.Default)]
			public SecondaryClassData[] secondaryClassInfos
			{
				get
				{
					return _secondaryClassInfos;
				}
				set
				{
					Array.Resize(ref _secondaryClassInfos, value.Length);
					value.CopyTo(_secondaryClassInfos, 0);
				}
			}

			[ProtoMember(21, Name = "reqEntitlements", DataFormat = DataFormat.Default)]
			public string[] reqEntitlements
			{
				get
				{
					return _reqEntitlements;
				}
				set
				{
					Array.Resize(ref _reqEntitlements, value.Length);
					value.CopyTo(_reqEntitlements, 0);
				}
			}

			[ProtoMember(22, IsRequired = false, Name = "defaultFirstNameLocaId", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int defaultFirstNameLocaId
			{
				get
				{
					return _defaultFirstNameLocaId;
				}
				set
				{
					_defaultFirstNameLocaId = value;
				}
			}

			[ProtoMember(23, IsRequired = false, Name = "defaultLastNameLocaId", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int defaultLastNameLocaId
			{
				get
				{
					return _defaultLastNameLocaId;
				}
				set
				{
					_defaultLastNameLocaId = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "AvatarNameSettings")]
		public class AvatarNameSettings : IExtensible
		{
			private int[] _firstNameLocaIds = new int[0];

			private int[] _lastNameLocaIds = new int[0];

			private IExtension extensionObject;

			[ProtoMember(1, Name = "firstNameLocaIds", DataFormat = DataFormat.TwosComplement)]
			public int[] firstNameLocaIds
			{
				get
				{
					return _firstNameLocaIds;
				}
				set
				{
					Array.Resize(ref _firstNameLocaIds, value.Length);
					value.CopyTo(_firstNameLocaIds, 0);
				}
			}

			[ProtoMember(2, Name = "lastNameLocaIds", DataFormat = DataFormat.TwosComplement)]
			public int[] lastNameLocaIds
			{
				get
				{
					return _lastNameLocaIds;
				}
				set
				{
					Array.Resize(ref _lastNameLocaIds, value.Length);
					value.CopyTo(_lastNameLocaIds, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private AvatarClass[] _classes = new AvatarClass[0];

		private long _version;

		private Resources _skilltreeResetCosts;

		private AvatarNameSettings _avatarNameSettings;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "classes", DataFormat = DataFormat.Default)]
		public AvatarClass[] classes
		{
			get
			{
				return _classes;
			}
			set
			{
				Array.Resize(ref _classes, value.Length);
				value.CopyTo(_classes, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "version", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(3, IsRequired = false, Name = "skilltreeResetCosts", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Resources skilltreeResetCosts
		{
			get
			{
				return _skilltreeResetCosts;
			}
			set
			{
				_skilltreeResetCosts = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "avatarNameSettings", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public AvatarNameSettings avatarNameSettings
		{
			get
			{
				return _avatarNameSettings;
			}
			set
			{
				_avatarNameSettings = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
