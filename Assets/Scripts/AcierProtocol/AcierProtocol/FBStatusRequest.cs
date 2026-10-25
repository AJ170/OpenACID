using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "FBStatusRequest")]
	public class FBStatusRequest : IExtensible
	{
		private string _accessToken = "";

		private ESocialNetwork _socialNetwork = ESocialNetwork.Facebook;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "accessToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string accessToken
		{
			get
			{
				return _accessToken;
			}
			set
			{
				_accessToken = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "socialNetwork", DataFormat = DataFormat.TwosComplement)]
		[DefaultValue(ESocialNetwork.Facebook)]
		public ESocialNetwork socialNetwork
		{
			get
			{
				return _socialNetwork;
			}
			set
			{
				_socialNetwork = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
