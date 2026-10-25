using System;
using System.ComponentModel;
using ProtoBuf;

namespace AcierProtocol
{
	[Serializable]
	[ProtoContract(Name = "ModifyProfileNameRequest")]
	public class ModifyProfileNameRequest : IExtensible
	{
		private string _username = "";

		private string _fbAccessToken = "";

		private string _uplayAccessToken = "";

		private string _weiboAccessToken = "";

		private ESocialNetwork _socialNetwork = ESocialNetwork.Facebook;

		private EModuleOrigin _origin;

		private bool _forceFacebookLinkToCurrent;

		private IExtension extensionObject;

		[ProtoMember(1, IsRequired = false, Name = "username", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string username
		{
			get
			{
				return _username;
			}
			set
			{
				_username = value;
			}
		}

		[ProtoMember(2, IsRequired = false, Name = "fbAccessToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string fbAccessToken
		{
			get
			{
				return _fbAccessToken;
			}
			set
			{
				_fbAccessToken = value;
			}
		}

		[ProtoMember(3, IsRequired = false, Name = "uplayAccessToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string uplayAccessToken
		{
			get
			{
				return _uplayAccessToken;
			}
			set
			{
				_uplayAccessToken = value;
			}
		}

		[ProtoMember(6, IsRequired = false, Name = "weiboAccessToken", DataFormat = DataFormat.Default)]
		[DefaultValue("")]
		public string weiboAccessToken
		{
			get
			{
				return _weiboAccessToken;
			}
			set
			{
				_weiboAccessToken = value;
			}
		}

		[ProtoMember(7, IsRequired = false, Name = "socialNetwork", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(4, IsRequired = false, Name = "origin", DataFormat = DataFormat.TwosComplement)]
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

		[ProtoMember(5, IsRequired = false, Name = "forceFacebookLinkToCurrent", DataFormat = DataFormat.Default)]
		[DefaultValue(false)]
		public bool forceFacebookLinkToCurrent
		{
			get
			{
				return _forceFacebookLinkToCurrent;
			}
			set
			{
				_forceFacebookLinkToCurrent = value;
			}
		}

		IExtension IExtensible.GetExtensionObject(bool createIfMissing)
		{
			return Extensible.GetExtensionObject(ref extensionObject, createIfMissing);
		}
	}
}
