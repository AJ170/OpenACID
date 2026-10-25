using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "SendFriendRequestRequest")]
	public class SendFriendRequestRequest : IExtensible
	{
		private string _userIdHash = "";

		private EModuleOrigin _origin;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "userIdHash", DataFormat = DataFormat.Default)]
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

		[ProtoMember(2, IsRequired = false, Name = "origin", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EModuleOrigin.ModuleOrigin_DailyReward)]
		public EModuleOrigin origin
		{
			get
			{
				return _origin;
			}
			set
			{
				_origin = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
