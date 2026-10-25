using System;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LootTypes")]
	public class LootTypes : IExtensible
	{
		private Item.EItemCategory[] _itemCategories = new Item.EItemCategory[0];

		private EResourceType[] _resourceTypes = new EResourceType[0];

		private EModuleType[] _moduleTypes = new EModuleType[0];

		private EBlueprintType[] _blueprintTypes = new EBlueprintType[0];

		private EMysteryBoxType[] _mysteryBoxTypes = new EMysteryBoxType[0];

		private ECustomLootTemplateType[] _customLootTemplateTypes = new ECustomLootTemplateType[0];

		private IExtension extensionObject;

		[ProtoMember(1, Name = "itemCategories", DataFormat = DataFormat.TwosComplement)]
		public Item.EItemCategory[] itemCategories
		{
			get
			{
				return _itemCategories;
			}
			set
			{
				Array.Resize(ref _itemCategories, value.Length);
				value.CopyTo(_itemCategories, 0);
			}
		}

		[ProtoMember(2, Name = "resourceTypes", DataFormat = DataFormat.TwosComplement)]
		public EResourceType[] resourceTypes
		{
			get
			{
				return _resourceTypes;
			}
			set
			{
				Array.Resize(ref _resourceTypes, value.Length);
				value.CopyTo(_resourceTypes, 0);
			}
		}

		[ProtoMember(3, Name = "moduleTypes", DataFormat = DataFormat.TwosComplement)]
		public EModuleType[] moduleTypes
		{
			get
			{
				return _moduleTypes;
			}
			set
			{
				Array.Resize(ref _moduleTypes, value.Length);
				value.CopyTo(_moduleTypes, 0);
			}
		}

		[ProtoMember(4, Name = "blueprintTypes", DataFormat = DataFormat.TwosComplement)]
		public EBlueprintType[] blueprintTypes
		{
			get
			{
				return _blueprintTypes;
			}
			set
			{
				Array.Resize(ref _blueprintTypes, value.Length);
				value.CopyTo(_blueprintTypes, 0);
			}
		}

		[ProtoMember(5, Name = "mysteryBoxTypes", DataFormat = DataFormat.TwosComplement)]
		public EMysteryBoxType[] mysteryBoxTypes
		{
			get
			{
				return _mysteryBoxTypes;
			}
			set
			{
				Array.Resize(ref _mysteryBoxTypes, value.Length);
				value.CopyTo(_mysteryBoxTypes, 0);
			}
		}

		[ProtoMember(6, Name = "customLootTemplateTypes", DataFormat = DataFormat.TwosComplement)]
		public ECustomLootTemplateType[] customLootTemplateTypes
		{
			get
			{
				return _customLootTemplateTypes;
			}
			set
			{
				Array.Resize(ref _customLootTemplateTypes, value.Length);
				value.CopyTo(_customLootTemplateTypes, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
