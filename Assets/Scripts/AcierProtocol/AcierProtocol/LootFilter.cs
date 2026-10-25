using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "LootFilter")]
	public class LootFilter : IExtensible
	{
		private string _lootTemplateName = "";

		private LootTypes _lootTypes;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "lootTemplateName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string lootTemplateName
		{
			get
			{
				return _lootTemplateName;
			}
			set
			{
				_lootTemplateName = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "lootTypes", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public LootTypes lootTypes
		{
			get
			{
				return _lootTypes;
			}
			set
			{
				_lootTypes = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
