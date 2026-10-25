using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "AssassinName")]
	public class AssassinName : IExtensible
	{
		private string _overrideName = "";

		private int _firstNameLocaId;

		private int _lastNameLocaId;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "overrideName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string overrideName
		{
			get
			{
				return _overrideName;
			}
			set
			{
				_overrideName = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "firstNameLocaId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int firstNameLocaId
		{
			get
			{
				return _firstNameLocaId;
			}
			set
			{
				_firstNameLocaId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "lastNameLocaId", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int lastNameLocaId
		{
			get
			{
				return _lastNameLocaId;
			}
			set
			{
				_lastNameLocaId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
