using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "MysteryBoxData")]
	public class MysteryBoxData : IExtensible
	{
		private EMysteryBoxType _mysteryBoxType = EMysteryBoxType.MysteryBoxItemType;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "mysteryBoxType", DataFormat = DataFormat.TwosComplement)]
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

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
