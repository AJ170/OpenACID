using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "Modification")]
	public class Modification : IExtensible
	{
		private int _effectValue;

		private string _skillGuid = "";

		private EModuleModifier _moduleModifier;

		private bool _plainItemPower;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "effectValue", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int effectValue
		{
			get
			{
				return _effectValue;
			}
			set
			{
				_effectValue = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "skillGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string skillGuid
		{
			get
			{
				return _skillGuid;
			}
			set
			{
				_skillGuid = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "moduleModifier", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EModuleModifier.ClientModifier)]
		public EModuleModifier moduleModifier
		{
			get
			{
				return _moduleModifier;
			}
			set
			{
				_moduleModifier = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "plainItemPower", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool plainItemPower
		{
			get
			{
				return _plainItemPower;
			}
			set
			{
				_plainItemPower = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
