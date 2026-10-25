using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MysteryBoxChain")]
	public class MysteryBoxChain : IExtensible
	{
		[Serializable]
		[ProtoContract(Name = "RolledLoot")]
		public class RolledLoot : IExtensible
		{
			private InventoryElement _element;

			private Resource _resources;

			private bool _inInventory;

			private IExtension extensionObject;

			[ProtoMember(1, IsRequired = false, Name = "element", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public InventoryElement element
			{
				get
				{
					return _element;
				}
				set
				{
					_element = value;
				}
			}

			[ProtoMember(2, IsRequired = false, Name = "resources", DataFormat = DataFormat.Default)]
			[DefaultValue(null)]
			public Resource resources
			{
				get
				{
					return _resources;
				}
				set
				{
					_resources = value;
				}
			}

			[ProtoMember(3, IsRequired = false, Name = "inInventory", DataFormat = DataFormat.Default)]
			[DefaultValue(false)]
			public bool inInventory
			{
				get
				{
					return _inInventory;
				}
				set
				{
					_inInventory = value;
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		[Serializable]
		[ProtoContract(Name = "Slot")]
		public class Slot : IExtensible
		{
			private RolledLoot[] _rolledLoot = new RolledLoot[0];

			private IExtension extensionObject;

			[ProtoMember(1, Name = "rolledLoot", DataFormat = DataFormat.Default)]
			public RolledLoot[] rolledLoot
			{
				get
				{
					return _rolledLoot;
				}
				set
				{
					Array.Resize(ref _rolledLoot, value.Length);
					value.CopyTo(_rolledLoot, 0);
				}
			}

			IExtension IExtensible.GetExtensionObject(bool createIfMissing)
			{
				return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
			}
		}

		private Quality.EQuality _quality = Quality.EQuality.Mundane;

		private EMysteryBoxType _mysteryBoxType = EMysteryBoxType.MysteryBoxItemType;

		private string _lastUsedInventoryElementId = "";

		private Slot[] _slots = new Slot[0];

		private Slot _bonus;

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

		[ProtoMember(2, IsRequired = false, Name = "mysteryBoxType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EMysteryBoxType.MysteryBoxItemType)]
		public EMysteryBoxType mysteryBoxType
		{
			get
			{
				return _mysteryBoxType;
			}
			set
			{
				_mysteryBoxType = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "lastUsedInventoryElementId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string lastUsedInventoryElementId
		{
			get
			{
				return _lastUsedInventoryElementId;
			}
			set
			{
				_lastUsedInventoryElementId = value;
			}
		}

		[ProtoMember(3, Name = "slots", DataFormat = DataFormat.Default)]
		public Slot[] slots
		{
			get
			{
				return _slots;
			}
			set
			{
				Array.Resize(ref _slots, value.Length);
				value.CopyTo(_slots, 0);
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "bonus", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public Slot bonus
		{
			get
			{
				return _bonus;
			}
			set
			{
				_bonus = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
