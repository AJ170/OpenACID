using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AchievementMapping")]
	public class AchievementMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "Condition")]
		public class Condition : IExtensible
		{
			private ECounterType _counterType;

			private long _goal;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "counterType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(ECounterType.InvalidCounter)]
			public ECounterType counterType
			{
				get
				{
					return _counterType;
				}
				set
				{
					_counterType = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "goal", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0L)]
			public long goal
			{
				get
				{
					return _goal;
				}
				set
				{
					_goal = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "RankDefinition")]
		public class RankDefinition : IExtensible
		{
			private int _idx;

			private Condition[] _conditions = new Condition[0];

			private EAchievementConditionCombinationType _combinationType;

			private LootTemplate[] _lootTemplates = new LootTemplate[0];

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "idx", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int idx
			{
				get
				{
					return _idx;
				}
				set
				{
					_idx = value;
				}
			}

			[ProtoMember(2, Name = "conditions", DataFormat = DataFormat.Default)]
			public Condition[] conditions
			{
				get
				{
					return _conditions;
				}
				set
				{
					Array.Resize(ref _conditions, value.Length);
					value.CopyTo(_conditions, 0);
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "combinationType", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(EAchievementConditionCombinationType.CombinationTypeAll)]
			public EAchievementConditionCombinationType combinationType
			{
				get
				{
					return _combinationType;
				}
				set
				{
					_combinationType = value;
				}
			}

			[ProtoMember(5, Name = "lootTemplates", DataFormat = DataFormat.Default)]
			public LootTemplate[] lootTemplates
			{
				get
				{
					return _lootTemplates;
				}
				set
				{
					Array.Resize(ref _lootTemplates, value.Length);
					value.CopyTo(_lootTemplates, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "Definition")]
		public class Definition : IExtensible
		{
			private string _id = "";

			private string _categoryId = "";

			private RankDefinition[] _ranks = new RankDefinition[0];

			private bool _hidden;

			private string _name = "";

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "id", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string id
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

			[ProtoMember(2, IsRequired = false, Name = "categoryId", DataFormat = DataFormat.Default)]
			[DefaultValue("")]
			public string categoryId
			{
				get
				{
					return _categoryId;
				}
				set
				{
					_categoryId = value;
				}
			}

			[ProtoMember(3, Name = "ranks", DataFormat = DataFormat.Default)]
			public RankDefinition[] ranks
			{
				get
				{
					return _ranks;
				}
				set
				{
					Array.Resize(ref _ranks, value.Length);
					value.CopyTo(_ranks, 0);
				}
			}

			[ProtoMember(4, IsRequired = false, Name = "hidden", DataFormat = DataFormat.Default)]
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

			[ProtoMember(5, IsRequired = false, Name = "name", DataFormat = DataFormat.Default)]
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Definition[] _achievements = new Definition[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "achievements", DataFormat = DataFormat.Default)]
		public Definition[] achievements
		{
			get
			{
				return _achievements;
			}
			set
			{
				Array.Resize(ref _achievements, value.Length);
				value.CopyTo(_achievements, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
