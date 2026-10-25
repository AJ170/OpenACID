using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ForgeMapping")]
	public class ForgeMapping : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "CostData")]
		public class CostData : IExtensible
		{
			[Serializable]
			[ProtoContract(Name = "Entry")]
			public class Entry : IExtensible
			{
				private Quality.EQuality _quality = Quality.EQuality.Mundane;

				private int _level;

				private Resource _cost;

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

				[ProtoMember(2, IsRequired = false, Name = "level", DataFormat = DataFormat.TwosComplement)]
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

				[ProtoMember(3, IsRequired = false, Name = "cost", DataFormat = DataFormat.Default)]
				[DefaultValue(null)]
				public Resource cost
				{
					get
					{
						return _cost;
					}
					set
					{
						_cost = value;
					}
				}

				IExtension IExtensible.GetExtensionObject(bool createIfMissing)
				{
					return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
				}
			}

			private Entry[] _entries = new Entry[0];

			private IExtension extensionObject;

			[ProtoMember(1, Name = "entries", DataFormat = DataFormat.Default)]
			public Entry[] entries
			{
				get
				{
					return _entries;
				}
				set
				{
					Array.Resize(ref _entries, value.Length);
					value.CopyTo(_entries, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "ReRollCost")]
		public class ReRollCost : IExtensible
		{
			private Resource _cost;

			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "cost", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource cost
			{
				get
				{
					return _cost;
				}
				set
				{
					_cost = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "NeededSlotCount")]
		public class NeededSlotCount : IExtensible
		{
			private int _slotCount;

			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "slotCount", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int slotCount
			{
				get
				{
					return _slotCount;
				}
				set
				{
					_slotCount = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "AutoCompletionCost")]
		public class AutoCompletionCost : IExtensible
		{
			private Resource _cost;

			private Quality.EQuality _quality = Quality.EQuality.Mundane;

			private int _occupiedSlots;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "cost", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource cost
			{
				get
				{
					return _cost;
				}
				set
				{
					_cost = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "quality", DataFormat = DataFormat.TwosComplement)]
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

			[ProtoMember(3, IsRequired = false, Name = "occupiedSlots", DataFormat = DataFormat.TwosComplement)]
			[DefaultValue(0)]
			public int occupiedSlots
			{
				get
				{
					return _occupiedSlots;
				}
				set
				{
					_occupiedSlots = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private float _levelReferenceFactor;

		private ReRollCost[] _reRollCosts = new ReRollCost[0];

		private NeededSlotCount[] _neededSlotCounts = new NeededSlotCount[0];

		private AutoCompletionCost[] _autoCompletionCosts = new AutoCompletionCost[0];

		private CostData _costData;

		private LootTemplate _tutorialRerollLoot;

		private LootTemplate _tutorialForgeLoot;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "levelReferenceFactor", DataFormat = DataFormat.FixedSize)]
		[DefaultValue(0f)]
		public float levelReferenceFactor
		{
			get
			{
				return _levelReferenceFactor;
			}
			set
			{
				_levelReferenceFactor = value;
			}
		}

		[ProtoMember(2, Name = "reRollCosts", DataFormat = DataFormat.Default)]
		public ReRollCost[] reRollCosts
		{
			get
			{
				return _reRollCosts;
			}
			set
			{
				Array.Resize(ref _reRollCosts, value.Length);
				value.CopyTo(_reRollCosts, 0);
			}
		}

		[ProtoMember(3, Name = "neededSlotCounts", DataFormat = DataFormat.Default)]
		public NeededSlotCount[] neededSlotCounts
		{
			get
			{
				return _neededSlotCounts;
			}
			set
			{
				Array.Resize(ref _neededSlotCounts, value.Length);
				value.CopyTo(_neededSlotCounts, 0);
			}
		}

		[ProtoMember(5, Name = "autoCompletionCosts", DataFormat = DataFormat.Default)]
		public AutoCompletionCost[] autoCompletionCosts
		{
			get
			{
				return _autoCompletionCosts;
			}
			set
			{
				Array.Resize(ref _autoCompletionCosts, value.Length);
				value.CopyTo(_autoCompletionCosts, 0);
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "costData", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public CostData costData
		{
			get
			{
				return _costData;
			}
			set
			{
				_costData = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "tutorialRerollLoot", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LootTemplate tutorialRerollLoot
		{
			get
			{
				return _tutorialRerollLoot;
			}
			set
			{
				_tutorialRerollLoot = value;
			}
		}

		[ProtoMember(8, IsRequired = false, Name = "tutorialForgeLoot", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LootTemplate tutorialForgeLoot
		{
			get
			{
				return _tutorialForgeLoot;
			}
			set
			{
				_tutorialForgeLoot = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
