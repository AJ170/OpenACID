using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "GetForeignAssassinRequest")]
	public class GetForeignAssassinRequest : IExtensible
	{
		private EForeignAssassinType _type = EForeignAssassinType.Hireling;

		private string _hirelingId = "";

		private string _userIdHash = "";

		private string _assassinId = "";

		private string _userId = "";

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "type", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EForeignAssassinType.Hireling)]
		public EForeignAssassinType type
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

		[ProtoMember(2, IsRequired = false, Name = "hirelingId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string hirelingId
		{
			get
			{
				return _hirelingId;
			}
			set
			{
				_hirelingId = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "userIdHash", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string userIdHash
		{
			get
			{
				return _userIdHash;
			}
			set
			{
				_userIdHash = value;
			}
		}

		[ProtoMember(4, IsRequired = false, Name = "assassinId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
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

		[ProtoMember(5, IsRequired = false, Name = "userId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string userId
		{
			get
			{
				return _userId;
			}
			set
			{
				_userId = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
