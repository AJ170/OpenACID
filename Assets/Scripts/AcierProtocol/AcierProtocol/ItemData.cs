using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ItemData")]
	public class ItemData : IExtensible
	{
		private Item.EItemCategory _category = Item.EItemCategory.None;

		private int _level;

		private int _baseValue;

		private Modification[] _modifications = new Modification[0];

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Item.EItemCategory.None)]
		public Item.EItemCategory category
		{
			get
			{
				return _category;
			}
			set
			{
				_category = value;
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

		[ProtoMember(3, IsRequired = false, Name = "baseValue", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int baseValue
		{
			get
			{
				return _baseValue;
			}
			set
			{
				_baseValue = value;
			}
		}

		[ProtoMember(4, Name = "modifications", DataFormat = DataFormat.Default)]
		public Modification[] modifications
		{
			get
			{
				return _modifications;
			}
			set
			{
				Array.Resize(ref _modifications, value.Length);
				value.CopyTo(_modifications, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
