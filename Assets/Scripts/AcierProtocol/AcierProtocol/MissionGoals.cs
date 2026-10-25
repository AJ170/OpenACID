using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MissionGoals")]
	public class MissionGoals : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "MissionGoal")]
		public class MissionGoal : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "MappingEntry")]
			public class MappingEntry : IExtensible
			{
				private EObjectiveBoundWildcards _key = EObjectiveBoundWildcards.Count_EObjectiveBoundWildcards;

				private string _valueString = "";

				private int _valueInt;

				private string[] _valueStrings = new string[0];

				private EAlertLevel _valueAlertLevel;

				private float _valueFloat;

				private IExtension extensionObject;

				[ProtoMember(1, IsRequired = false, Name = "key", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(EObjectiveBoundWildcards.Count_EObjectiveBoundWildcards)]
				public EObjectiveBoundWildcards key
				{
					get
					{
						return _key;
					}
					set
					{
						_key = value;
					}
				}

				[ProtoMember(2, IsRequired = false, Name = "valueString", DataFormat = DataFormat.Default)]
				[DefaultValue("")]
				public string valueString
				{
					get
					{
						return _valueString;
					}
					set
					{
						_valueString = value;
					}
				}

				[ProtoMember(3, IsRequired = false, Name = "valueInt", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(0)]
				public int valueInt
				{
					get
					{
						return _valueInt;
					}
					set
					{
						_valueInt = value;
					}
				}

				[ProtoMember(4, Name = "valueStrings", DataFormat = DataFormat.Default)]
				public string[] valueStrings
				{
					get
					{
						return _valueStrings;
					}
					set
					{
						Array.Resize(ref _valueStrings, value.Length);
						value.CopyTo(_valueStrings, 0);
					}
				}

				[ProtoMember(5, IsRequired = false, Name = "valueAlertLevel", DataFormat = DataFormat.TwosComplement)]
				[DefaultValue(EAlertLevel.Anonymous)]
				public EAlertLevel valueAlertLevel
				{
					get
					{
						return _valueAlertLevel;
					}
					set
					{
						_valueAlertLevel = value;
					}
				}

				[ProtoMember(6, IsRequired = false, Name = "valueFloat", DataFormat = DataFormat.FixedSize)]
				[DefaultValue(0f)]
				public float valueFloat
				{
					get
					{
						return _valueFloat;
					}
					set
					{
						_valueFloat = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private EMissionGoal _goalType = EMissionGoal.Objective_EMissionGoal;

			private long _objectiveId;

			private int _id;

			private EObjective _type;

			private EChallengeType _challengeType;

			private bool _elite;

			private bool _forced;

			private bool _hidden;

			private LootFilter _lootFilter;

			private LangKey _description;

			private MappingEntry[] _mapping = new MappingEntry[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "goalType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EMissionGoal.Objective_EMissionGoal)]
			public EMissionGoal goalType
			{
				get
				{
					return _goalType;
				}
				set
				{
					_goalType = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "objectiveId", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long objectiveId
			{
				get
				{
					return _objectiveId;
				}
				set
				{
					_objectiveId = value;
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "id", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(10, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EObjective.Deliver)]
			public EObjective type
			{
				get
				{
					return _type;
				}
				set
				{
					_type = value;
				}
			}

			[ProtoMember(11, IsRequired = false, Name = "challengeType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EChallengeType.Invalid_EChallengeType)]
			public EChallengeType challengeType
			{
				get
				{
					return _challengeType;
				}
				set
				{
					_challengeType = value;
				}
			}

			[ProtoMember(12, IsRequired = false, Name = "elite", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool elite
			{
				get
				{
					return _elite;
				}
				set
				{
					_elite = value;
				}
			}

			[ProtoMember(5, IsRequired = false, Name = "forced", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool forced
			{
				get
				{
					return _forced;
				}
				set
				{
					_forced = value;
				}
			}

			[ProtoMember(6, IsRequired = false, Name = "hidden", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool hidden
			{
				get
				{
					return _hidden;
				}
				set
				{
					_hidden = value;
				}
			}

			[ProtoMember(7, IsRequired = false, Name = "lootFilter", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LootFilter lootFilter
			{
				get
				{
					return _lootFilter;
				}
				set
				{
					_lootFilter = value;
				}
			}

			[ProtoMember(8, IsRequired = false, Name = "description", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public LangKey description
			{
				get
				{
					return _description;
				}
				set
				{
					_description = value;
				}
			}

			[ProtoMember(9, Name = "mapping", DataFormat = DataFormat.Default)]
			public MappingEntry[] mapping
			{
				get
				{
					return _mapping;
				}
				set
				{
					Array.Resize(ref _mapping, value.Length);
					value.CopyTo(_mapping, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private MissionGoal[] _missionGoals = new MissionGoal[0];

		private int _missionLevel;

		private IExtension extensionObject;

		[ProtoMember(1, Name = "missionGoals", DataFormat = DataFormat.Default)]
		public MissionGoal[] missionGoals
		{
			get
			{
				return _missionGoals;
			}
			set
			{
				Array.Resize(ref _missionGoals, value.Length);
				value.CopyTo(_missionGoals, 0);
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "missionLevel", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int missionLevel
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
