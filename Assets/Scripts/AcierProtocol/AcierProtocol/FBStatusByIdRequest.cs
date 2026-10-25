using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "FBStatusByIdRequest")]
	public class FBStatusByIdRequest : IExtensible
	{
		private string _facebookId = "";

		private ESocialNetwork _socialNetwork = ESocialNetwork.Facebook;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "facebookId", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string facebookId
		{
			get
			{
				return _facebookId;
			}
			set
			{
				_facebookId = value;
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
