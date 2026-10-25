using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "FBStatusResponse")]
	public class FBStatusResponse : IExtensible
	{
		private EFBStatus _status;

		private UserProfile _userProfile;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "status", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(EFBStatus.Registered)]
		public EFBStatus status
		{
			get
			{
				return _status;
			}
			set
			{
				_status = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "userProfile", DataFormat = DataFormat.Default)]
		[DefaultValue(null)]
		public UserProfile userProfile
		{
			get
			{
				return _userProfile;
			}
			set
			{
				_userProfile = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
