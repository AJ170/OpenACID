using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CustomLootData")]
	public class CustomLootData : IExtensible
	{
		private ECustomLootTemplateType _customLootType = ECustomLootTemplateType.LockPick;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "customLootType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ECustomLootTemplateType.LockPick)]
		public ECustomLootTemplateType customLootType
		{
			get
			{
				return _customLootType;
			}
			set
			{
				_customLootType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
