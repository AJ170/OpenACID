using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "CreateAssassinRequest")]
	public class CreateAssassinRequest : IExtensible
	{
		private Assassin.EClassTypeId _classType = Assassin.EClassTypeId.None;

		private int _firstNameLocaId;

		private int _lastNameLocaId;

		private string _headGuid = "";

		private int _numAssassins;

		private string _customName = "";

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

		[ProtoMember(4, IsRequired = false, Name = "headGuid", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string headGuid
		{
			get
			{
				return _headGuid;
			}
			set
			{
				_headGuid = value;
			}
		}

		[ProtoMember(5, IsRequired = false, Name = "numAssassins", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(0)]
		public int numAssassins
		{
			get
			{
				return _numAssassins;
			}
			set
			{
				_numAssassins = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "customName", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string customName
		{
			get
			{
				return _customName;
			}
			set
			{
				_customName = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
