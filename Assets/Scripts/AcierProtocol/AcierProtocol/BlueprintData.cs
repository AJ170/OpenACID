using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "BlueprintData")]
	public class BlueprintData : IExtensible
	{
		private EBlueprintType _type = EBlueprintType.BlueprintWeapon;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EBlueprintType.BlueprintWeapon)]
		public EBlueprintType type
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
