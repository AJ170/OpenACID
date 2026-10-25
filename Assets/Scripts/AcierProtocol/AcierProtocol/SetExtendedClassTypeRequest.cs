using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SetExtendedClassTypeRequest")]
	public class SetExtendedClassTypeRequest : IExtensible
	{
		private string _assassinId;

		private Assassin.EClassTypeId _extendedClassType = Assassin.EClassTypeId.None;

		private EResourceType _costType = EResourceType.HardCurrency;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = true, Name = "assassinId", DataFormat = DataFormat.Default)]
		public string assassinId
		{
			get
			{
				return _assassinId;
			}
			set
			{
				_assassinId = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "extendedClassType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(Assassin.EClassTypeId.None)]
		public Assassin.EClassTypeId extendedClassType
		{
			get
			{
				return _extendedClassType;
			}
			set
			{
				_extendedClassType = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "costType", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EResourceType.HardCurrency)]
		public EResourceType costType
		{
			get
			{
				return _costType;
			}
			set
			{
				_costType = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
