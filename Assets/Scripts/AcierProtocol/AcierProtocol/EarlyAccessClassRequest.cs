using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "EarlyAccessClassRequest")]
	public class EarlyAccessClassRequest : IExtensible
	{
		private Assassin.EClassTypeId _classType = Assassin.EClassTypeId.None;

		private EResourceType _currency = EResourceType.HardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "classType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Assassin.EClassTypeId.None)]
		public Assassin.EClassTypeId classType
		{
			get
			{
				return _classType;
			}
			set
			{
				_classType = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "currency", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EResourceType.HardCurrency)]
		public EResourceType currency
		{
			get
			{
				return _currency;
			}
			set
			{
				_currency = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
