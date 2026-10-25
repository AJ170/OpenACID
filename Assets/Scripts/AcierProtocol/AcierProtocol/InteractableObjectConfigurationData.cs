using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "InteractableObjectConfigurationData")]
	public class InteractableObjectConfigurationData : IExtensible
	{
		private string _guid = "";

		private EObjectCategory _category;

		private EObjectSubCategory[] _subCategories = new EObjectSubCategory[0];

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

		[ProtoMember(2, IsRequired = false, Name = "category", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EObjectCategory.HidingSpot_EObjectCategory)]
		public EObjectCategory category
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

		[ProtoMember(3, Name = "subCategories", DataFormat = DataFormat.TwosComplement)]
		public EObjectSubCategory[] subCategories
		{
			get
			{
				return _subCategories;
			}
			set
			{
				Array.Resize(ref _subCategories, value.Length);
				value.CopyTo(_subCategories, 0);
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
